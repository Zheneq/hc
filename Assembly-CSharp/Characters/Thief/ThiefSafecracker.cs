// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ThiefSafecracker : Ability
{
	public int m_damageAmount = 5;
	public float m_laserRange = 5f;
	public float m_laserWidth = 0.5f;
	public bool m_laserPenetrateLos = true;
	public int m_returnDelay = 1;
	public int m_returnEffectAnimationIndex = 1;
	public float m_knockbackDistance = 2f;
	public KnockbackType m_knockbackType = KnockbackType.PerpendicularAwayFromAimDir;
	public GameObject m_returnSequencePrefab;
	public GameObject m_groundSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Safecracker";
		}
		Targeter = new AbilityUtil_Targeter_KnockbackLaser(
			this,
			m_laserWidth,
			m_laserRange,
			m_laserPenetrateLos,
			-1,
			m_knockbackDistance,
			m_knockbackDistance,
			m_knockbackType,
			false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_damageAmount > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		}
		return numbers;
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> hitActors = GetHitActors(targets, caster, out Vector3 targetPos, null);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			targetPos,
			hitActors.ToArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out Vector3 vector, nonActorTargetInfo);
		ThiefCreateSpoilsMarkerEffect thiefCreateSpoilsMarkerEffect = ServerEffectManager.Get().GetEffect(
			caster, typeof(ThiefCreateSpoilsMarkerEffect)) as ThiefCreateSpoilsMarkerEffect;
		bool removedEffect = false;
		foreach (ActorData target in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(m_damageAmount);
			actorHitResults.AddKnockbackData(new KnockbackHitData(target,
				caster,
				m_knockbackType,
				targets[0].AimDirection,
				caster.GetFreePos(),
				m_knockbackDistance));
			if (thiefCreateSpoilsMarkerEffect != null && !removedEffect)
			{
				actorHitResults.AddEffectForRemoval(thiefCreateSpoilsMarkerEffect, ServerEffectManager.Get().GetActorEffects(caster));
				removedEffect = true;
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(vector));
		positionHitResults.AddEffect(new ThiefSafecrackerBoomerangEffect(
			AsEffectSource(),
			Board.Get().GetSquareFromVec3(vector),
			null,
			caster,
			vector,
			m_returnDelay,
			m_damageAmount,
			m_laserRange,
			m_laserWidth,
			m_knockbackType,
			m_knockbackDistance,
			m_returnSequencePrefab,
			m_groundSequencePrefab,
			m_returnEffectAnimationIndex));
		abilityResults.StorePositionHit(positionHitResults);
	}

	// added in rogues
	private List<ActorData> GetHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out Vector3 endPos,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			m_laserRange,
			m_laserWidth,
			caster,
			caster.GetOtherTeams(),
			m_laserPenetrateLos,
			-1,
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		endPos = laserCoords.end;
		return actorsInLaser;
	}
#endif
}
