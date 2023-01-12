using System.Collections.Generic;
using UnityEngine;

public class ChainAbility_ShootNearestActor : Ability
{
	[Header("-- On Hit - Enemy ----------------------------------------")]
	public int m_enemyDamageAmount = 3;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- On Hit - Ally")]
	public int m_allyHealAmount = 3;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Target selection")]
	public float m_maxRange = 5f;
	public bool m_includeAllies;
	public bool m_includeEnemies = true;
	public bool m_penetrateLos;
	[Header("-- Sequences ----------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "CHAIN_ABILITY_SHOOT_NEAREST_ACTOR";
		}
		m_sequencePrefab = m_castSequencePrefab;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> hitActors = GetHitActors(targets, caster, null);
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetFreePos(),
			hitActors.ToArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		foreach (ActorData actorData in GetHitActors(targets, caster, nonActorTargetInfo))
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			if (actorData.GetTeam() == caster.GetTeam())
			{
				actorHitResults.SetBaseHealing(m_allyHealAmount);
				actorHitResults.AddStandardEffectInfo(m_allyHitEffect);
			}
			else
			{
				actorHitResults.SetBaseDamage(m_enemyDamageAmount);
				actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private new List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> validTeams = new List<Team>();
		if (m_includeEnemies)
		{
			validTeams.AddRange(caster.GetOtherTeams());
		}
		if (m_includeAllies)
		{
			validTeams.Add(caster.GetTeam());
		}
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
			caster.GetLoSCheckPos(), m_maxRange, m_penetrateLos, caster, validTeams, null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadius);
		if (actorsInRadius.Contains(caster))
		{
			actorsInRadius.Remove(caster);
		}
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadius, caster.GetFreePos(), targets[0].AimDirection);
		TargeterUtils.LimitActorsToMaxNumber(ref actorsInRadius, 1);
		List<ActorData> result;
		if (actorsInRadius.Count > 0)
		{
			Vector3 vector = actorsInRadius[0].GetLoSCheckPos() - caster.GetLoSCheckPos();
			float laserRangeInSquares = vector.magnitude / Board.Get().squareSize + 0.5f;
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = caster.GetLoSCheckPos();
			result = AreaEffectUtils.GetActorsInLaser(
				laserCoords.start,
				vector.normalized,
				laserRangeInSquares,
				0.5f,
				caster,
				validTeams,
				m_penetrateLos,
				1,
				false,
				true,
				out laserCoords.end,
				nonActorTargetInfo);
		}
		else
		{
			result = new List<ActorData>();
		}
		return result;
	}
#endif
}
