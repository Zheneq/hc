// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiWindBlade : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 0.6f;
	public float m_minRangeBeforeBend = 1f;
	public float m_maxRangeBeforeBend = 5.5f;
	public float m_maxTotalRange = 7.5f;
	public float m_maxBendAngle = 45f;
	public bool m_penetrateLoS;
	public int m_maxTargets = 1;
	[Header("-- Damage")]
	public int m_laserDamageAmount = 5;
	public int m_damageChangePerTarget;
	public StandardEffectInfo m_laserHitEffect;
	[Header("-- Shielding per enemy hit on start of Next Turn")]
	public int m_shieldingPerEnemyHitNextTurn;
	public int m_shieldingDuration = 1;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SamuraiWindBlade m_abilityMod;
	private Samurai_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedLaserHitEffect;
	
#if SERVER
	// added in rogues
	private Passive_Samurai m_passive;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Wind Blade";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
#if SERVER
		// added in rogues
		m_passive = GetPassiveOfType<Passive_Samurai>();
#endif
		SetCachedFields();
		m_syncComponent = ActorData.GetComponent<Samurai_SyncComponent>();
		ClearTargeters();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BendingLaser targeter = new AbilityUtil_Targeter_BendingLaser(
				this,
				GetLaserWidth(),
				GetMinRangeBeforeBend(),
				GetMaxRangeBeforeBend(),
				GetMaxTotalRange(),
				GetMaxBendAngle(),
				PenetrateLoS(),
				GetMaxTargets());
			targeter.SetUseMultiTargetUpdate(true);
			Targeters.Add(targeter);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (!Targeters.IsNullOrEmpty())
		{
			AbilityUtil_Targeter_BendingLaser targeter = Targeters[0] as AbilityUtil_Targeter_BendingLaser;
			if (targeter.DidStopShort())
			{
				return 1;
			}
		}
		return 2;
	}

	public override bool ShouldAutoConfirmIfTargetingOnEndTurn()
	{
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxTotalRange();
	}

	private void SetCachedFields()
	{
		m_cachedLaserHitEffect = m_abilityMod != null
			? m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public float GetMinRangeBeforeBend()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minRangeBeforeBendMod.GetModifiedValue(m_minRangeBeforeBend)
			: m_minRangeBeforeBend;
	}

	public float GetMaxRangeBeforeBend()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxRangeBeforeBendMod.GetModifiedValue(m_maxRangeBeforeBend)
			: m_maxRangeBeforeBend;
	}

	public float GetMaxTotalRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTotalRangeMod.GetModifiedValue(m_maxTotalRange)
			: m_maxTotalRange;
	}

	public float GetMaxBendAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxBendAngleMod.GetModifiedValue(m_maxBendAngle)
			: m_maxBendAngle;
	}

	public bool PenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS)
			: m_penetrateLoS;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetLaserDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public int GetDamageChangePerTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageChangePerTargetMod.GetModifiedValue(m_damageChangePerTarget)
			: m_damageChangePerTarget;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		return m_cachedLaserHitEffect ?? m_laserHitEffect;
	}

	public int GetShieldingPerEnemyHitNextTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldingPerEnemyHitNextTurnMod.GetModifiedValue(m_shieldingPerEnemyHitNextTurn)
			: m_shieldingPerEnemyHitNextTurn;
	}

	public int GetShieldingDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldingDurationMod.GetModifiedValue(m_shieldingDuration)
			: m_shieldingDuration;
	}

	public int CalcDamage(int hitOrder)
	{
		int damage = GetLaserDamageAmount();
		if (GetDamageChangePerTarget() > 0 && hitOrder > 0)
		{
			damage += GetDamageChangePerTarget() * hitOrder;
		}
		return damage;
	}

	public int GetHitOrderIndexFromTargeters(ActorData actor, int currentTargetIndex)
	{
		int num = 0;
		if (Targeters != null)
		{
			for (int i = 0; i < Targeters.Count && i <= currentTargetIndex; i++)
			{
				if (Targeters[i] is AbilityUtil_Targeter_BendingLaser targeter)
				{
					foreach (ActorData current in targeter.m_ordererdHitActors)
					{
						if (current == actor)
						{
							return num;
						}
						num++;
					}
				}
			}
		}
		return -1;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, m_laserDamageAmount);
		AddTokenInt(tokens, "DamageChangePerTarget", string.Empty, m_damageChangePerTarget);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "ShieldingPerEnemyHitNextTurn", string.Empty, m_shieldingPerEnemyHitNextTurn);
		AddTokenInt(tokens, "ShieldingDuration", string.Empty, m_shieldingDuration);
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, List<AbilityTarget> targets)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxBendAngle = GetMaxBendAngle();
		Vector3 aimDirection = targets[0].AimDirection;
		if (maxBendAngle > 0f && maxBendAngle < 360f)
		{
			aimDir = Vector3.RotateTowards(aimDirection, aimDir, (float)Math.PI / 180f * maxBendAngle, 0f);
		}
		return aimDir;
	}

	private float GetClampedRangeInSquares(ActorData targetingActor, AbilityTarget currentTarget)
	{
		float dist = (currentTarget.FreePos - targetingActor.GetLoSCheckPos()).magnitude;
		if (dist < GetMinRangeBeforeBend() * Board.Get().squareSize)
		{
			return GetMinRangeBeforeBend();
		}
		if (dist > GetMaxRangeBeforeBend() * Board.Get().squareSize)
		{
			return GetMaxRangeBeforeBend();
		}
		return dist / Board.Get().squareSize;
	}

	private float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		float clampedRangeInSquares = GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = targetingActor.GetLoSCheckPos() + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
		return GetMaxTotalRange() - clampedRangeInSquares;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_laserDamageAmount > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		}
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (m_laserDamageAmount > 0)
		{
			int damage = GetLaserDamageAmount();
			if (GetDamageChangePerTarget() > 0)
			{
				int hitOrderIndexFromTargeters = GetHitOrderIndexFromTargeters(targetActor, currentTargeterIndex);
				damage = CalcDamage(hitOrderIndexFromTargeters);
			}
			if (m_syncComponent != null)
			{
				// reactor
				damage += m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
				// rogues (inlined)
				// int num2 = 0;
				// if (m_syncComponent.IsSelfBuffActive(ref num2))
				// {
				// 	damage += num2;
				// }
				// damage += m_syncComponent.GetExtraDamageFromQueuedSelfBuff();
			}
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiWindBlade))
		{
			m_abilityMod = abilityMod as AbilityMod_SamuraiWindBlade;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		if (m_passive != null)
		{
			List<ActorData> actorsToConsider = additionalData.m_abilityResults.HitActorList();
			m_passive.NumEnemyHitWindBlade = AbilityUtils.GetEnemyCount(actorsToConsider, caster);
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out List<Vector3> endPoints, out List<ActorData> actorsHitAfterBounce, nonActorTargetInfo);
		if (hitActors.Count > 1)
		{
			ActorData value = hitActors[0];
			hitActors[0] = hitActors[hitActors.Count - 1];
			hitActors[hitActors.Count - 1] = value;
		}
		BouncingShotSequence.ExtraParams extraParams = new BouncingShotSequence.ExtraParams
		{
			segmentPts = new List<Vector3>()
		};
		extraParams.segmentPts.AddRange(endPoints);
		extraParams.segmentPts.RemoveAt(0);
		extraParams.laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
		foreach (ActorData actorData in hitActors)
		{
			if (actorsHitAfterBounce.Contains(actorData))
			{
				extraParams.laserTargets[actorData] = new AreaEffectUtils.BouncingLaserInfo(endPoints[0], 1);
			}
			else
			{
				extraParams.laserTargets[actorData] = new AreaEffectUtils.BouncingLaserInfo(caster.GetLoSCheckPos(), 0);
			}
		}
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			endPoints[endPoints.Count - 1],
			hitActors.ToArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[] { extraParams });
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out List<Vector3> endPoints, out List<ActorData> actorsHitAfterBounce, nonActorTargetInfo);
		for (int i = 0; i < hitActors.Count; i++)
		{
			ActorData actorData = hitActors[i];
			Vector3 origin = caster.GetLoSCheckPos();
			bool flag = false;
			if (actorsHitAfterBounce.Contains(actorData))
			{
				origin = endPoints[1];
				flag = true;
			}
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, origin));
			actorHitResults.AddStandardEffectInfo(GetLaserHitEffect());
			int num = CalcDamage(i);
			if (m_syncComponent != null)
			{
				num += m_syncComponent.GetExtraDamageFromQueuedSelfBuff();
			}
			actorHitResults.SetBaseDamage(num);
			if (flag)
			{
				actorHitResults.SetIgnoreCoverMinDist(true);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<Vector3> endPoints,
		out List<ActorData> actorsHitAfterBounce,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, false, true);
		endPoints = new List<Vector3>();
		endPoints.Add(caster.GetLoSCheckPos());
		float num = GetClampedRangeInSquares(caster, targets[0]);
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			endPoints[0],
			targets[0].AimDirection,
			num,
			GetLaserWidth(),
			caster,
			relevantTeams,
			PenetrateLoS(),
			GetMaxTargets(),
			false,
			true,
			out Vector3 laserEndPos,
			nonActorTargetInfo);
		endPoints.Add(laserEndPos);
		if (actorsInLaser.Count < GetMaxTargets() && (laserEndPos - endPoints[0]).magnitude > num * Board.Get().squareSize - 0.1f)
		{
			num = GetDistanceRemaining(caster, targets[0], out Vector3 adjustedStartPosWithOffset);
			Vector3 vector2 = targets[1].FreePos;
			if ((targets[1].FreePos - targets[0].FreePos).magnitude < Mathf.Epsilon)
			{
				vector2 += targets[0].AimDirection * 10f;
			}
			Vector3 targeterClampedAimDirection = GetTargeterClampedAimDirection((vector2 - adjustedStartPosWithOffset).normalized, targets);
			adjustedStartPosWithOffset = VectorUtils.GetAdjustedStartPosWithOffset(
				adjustedStartPosWithOffset,
				adjustedStartPosWithOffset + targeterClampedAimDirection,
				-0.2f);
			endPoints[1] = adjustedStartPosWithOffset;
			Vector3 item;
			actorsHitAfterBounce = AreaEffectUtils.GetActorsInLaser(
				adjustedStartPosWithOffset,
				targeterClampedAimDirection,
				num,
				GetLaserWidth(),
				caster,
				relevantTeams,
				PenetrateLoS(),
				GetMaxTargets(),
				false,
				true,
				out item,
				nonActorTargetInfo);
			for (int i = actorsHitAfterBounce.Count - 1; i >= 0; i--)
			{
				ActorData item2 = actorsHitAfterBounce[i];
				if (actorsInLaser.Contains(item2))
				{
					actorsHitAfterBounce.RemoveAt(i);
				}
			}
			actorsInLaser.AddRange(actorsHitAfterBounce);
			endPoints.Add(item);
		}
		else
		{
			actorsHitAfterBounce = new List<ActorData>();
		}
		return actorsInLaser;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster.GetTeam() != target.GetTeam())
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.SamuraiStats.NumEnemiesHit_WindBlade);
		}
	}
#endif
}
