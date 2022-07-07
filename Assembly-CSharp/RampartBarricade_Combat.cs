// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RampartBarricade_Combat : Ability
{
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private RampartBarricade_Prep m_prepAbility;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Barricade - Chain - Knockback";
		}
		m_prepAbility = GetComponent<AbilityData>().GetAbilityOfType(typeof(RampartBarricade_Prep)) as RampartBarricade_Prep;
		if (m_prepAbility == null)
		{
			Debug.LogError("Rampart Barricade Chain: did not find parent ability");
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, targets[0].FreePos, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		if (m_prepAbility != null)
		{
			m_prepAbility.GetBarrierPositionAndFacing(targets, out Vector3 _, out Vector3 aimDir);
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			foreach (ActorData target in GetHitActors(targets, caster, nonActorTargetInfo))
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, caster.GetFreePos()));
				actorHitResults.SetBaseDamage(m_prepAbility.GetDamageAmount());
				actorHitResults.AddStandardEffectInfo(m_prepAbility.GetEnemyHitEffect());
				if (m_prepAbility.GetKnockbackDistance() > 0f)
				{
					KnockbackHitData knockbackData = new KnockbackHitData(target, caster, m_prepAbility.GetKnockbackType(), aimDir, caster.GetFreePos(), m_prepAbility.GetKnockbackDistance());
					actorHitResults.AddKnockbackData(knockbackData);
				}
				abilityResults.StoreActorHit(actorHitResults);
			}
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	// added in rogues
	private new List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list;
		if (m_prepAbility != null)
		{
			m_prepAbility.GetBarrierPositionAndFacing(targets, out Vector3 _, out Vector3 vector2);
			Vector3 loSCheckPos = caster.GetLoSCheckPos();
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = loSCheckPos;
			list = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector2, m_prepAbility.GetLaserRange() + 0.5f, m_prepAbility.GetLaserWidth(), caster, caster.GetOtherTeams(), m_prepAbility.m_penetrateLos, -1, m_prepAbility.m_laserLengthIgnoreLos, true, out laserCoords.end, nonActorTargetInfo, null, true);
			if (list.Count > 0)
			{
				List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, -1f * vector2, 2f, m_prepAbility.GetLaserWidth(), caster, caster.GetOtherTeams(), true, -1, true, true, out Vector3 _, null, null, true);
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (actorsInLaser.Contains(list[i]))
					{
						list.RemoveAt(i);
					}
				}
			}
		}
		else
		{
			list = new List<ActorData>();
		}
		return list;
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		return null;
	}

	// added in rogues
	public override bool UseTargeterGridPosForCameraBounds()
	{
		return false;
	}
#endif
}
