// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBendingArrow : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 0.6f;
	public float m_minRangeBeforeBend = 1f;
	public float m_maxRangeBeforeBend = 5.5f;
	public float m_maxTotalRange = 7.5f;
	public float m_maxBendAngle = 45f;
	public bool m_penetrateLoS;
	public int m_maxTargets = 1;
	public bool m_startTargeterFadeAtActorRadius = true;
	[Header("-- Damage")]
	public int m_laserDamageAmount = 5;
	public StandardEffectInfo m_laserHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	
	private AbilityMod_ArcherBendingArrow m_abilityMod;
	private Archer_SyncComponent m_syncComp;
	private ArcherHealingDebuffArrow m_healArrowAbility;
	private AbilityData.ActionType m_healArrowActionType = AbilityData.ActionType.INVALID_ACTION;
	private ActorTargeting m_actorTargeting;
	private StandardEffectInfo m_cachedLaserHitEffect;
	private StandardEffectInfo m_cachedEffectToHealingDebuffTarget;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Bending Arrow";
		}
		m_syncComp = GetComponent<Archer_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			m_healArrowAbility = GetAbilityOfType(typeof(ArcherHealingDebuffArrow)) as ArcherHealingDebuffArrow;
			if (m_healArrowAbility != null)
			{
				m_healArrowActionType = component.GetActionTypeOfAbility(m_healArrowAbility);
			}
		}
		m_actorTargeting = GetComponent<ActorTargeting>();
		Targeters.Clear();
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
			targeter.m_startFadeAtActorRadius = m_startTargeterFadeAtActorRadius;
			Targeters.Add(targeter);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return !Targeters.IsNullOrEmpty()
		       && (Targeters[0] as AbilityUtil_Targeter_BendingLaser).DidStopShort()
			? 1
			: 2;
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
		Vector3 loSCheckPos = targetingActor.GetLoSCheckPos();
		float magnitude = (currentTarget.FreePos - loSCheckPos).magnitude;
		if (magnitude < GetMinRangeBeforeBend() * Board.Get().squareSize)
		{
			return GetMinRangeBeforeBend();
		}
		if (magnitude > GetMaxRangeBeforeBend() * Board.Get().squareSize)
		{
			return GetMaxRangeBeforeBend();
		}
		return magnitude / Board.Get().squareSize;
	}

	private float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		Vector3 loSCheckPos = targetingActor.GetLoSCheckPos();
		float clampedRangeInSquares = GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = loSCheckPos + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
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
		int damage = GetLaserDamageAmount();
		bool flag = currentTargeterIndex == 0;
		AbilityUtil_Targeter.ActorTarget actorTarget = Targeters[currentTargeterIndex].GetActorsInRange().Find(t => t.m_actor == targetActor);
		if (actorTarget == null && currentTargeterIndex > 0)
		{
			actorTarget = Targeters[0].GetActorsInRange().Find(t => t.m_actor == targetActor);
			flag = actorTarget != null;
		}
		if (actorTarget != null && !actorTarget.m_subjectTypes.Contains(AbilityTooltipSubject.Near))
		{
			damage = GetDamageAfterPierce();
		}
		if (IsReactionHealTarget(targetActor))
		{
			damage += GetExtraDamageToHealingDebuffTarget();
			damage += m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
		}
		if (!flag)
		{
			damage += GetExtraDamageAfterBend();
		}
		dictionary[AbilityTooltipSymbol.Damage] = damage;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			foreach (AbilityUtil_Targeter.ActorTarget current in Targeters[i].GetActorsInRange())
			{
				if (IsReactionHealTarget(current.m_actor))
				{
					return m_healArrowAbility.GetTechPointsPerHeal();
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	private bool IsReactionHealTarget(ActorData targetActor)
	{
		if (m_syncComp.m_healReactionTargetActor == targetActor.ActorIndex
		    && !m_syncComp.ActorHasUsedHealReaction(ActorData))
		{
			return true;
		}

		if (m_healArrowActionType == AbilityData.ActionType.INVALID_ACTION
		    || m_actorTargeting == null)
		{
			return false;
		}
		List<AbilityTarget> abilityTargetsInRequest = m_actorTargeting.GetAbilityTargetsInRequest(m_healArrowActionType);
		if (abilityTargetsInRequest != null && abilityTargetsInRequest.Count > 0)
		{
			BoardSquare square = Board.Get().GetSquare(abilityTargetsInRequest[0].GridPos);
			ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(square, true, false, ActorData);
			if (targetableActorOnSquare == targetActor)
			{
				return true;
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, m_laserDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		switch (targetIndex)
		{
			case 0:
				min = GetMinRangeBeforeBend() * Board.Get().squareSize;
				max = GetMaxRangeBeforeBend() * Board.Get().squareSize;
				return true;
			case 1:
				min = 1f;
				max = 1f;
				return true;
			default:
				return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
		}
	}

	public override bool HasRestrictedFreeAimDegrees(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			Vector3 aimDirection = targetsSoFar[0].AimDirection;
			float num = VectorUtils.HorizontalAngle_Deg(aimDirection);
			min = num - GetMaxBendAngle();
			max = num + GetMaxBendAngle();
			return true;
		}
		return base.HasRestrictedFreeAimDegrees(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			AbilityTarget abilityTarget = targetsSoFar[0];
			Vector3 loSCheckPos = aimingActor.GetLoSCheckPos();
			float magnitude = (abilityTarget.FreePos - loSCheckPos).magnitude;
			float num;
			if (magnitude < GetMinRangeBeforeBend() * Board.Get().squareSize)
			{
				num = GetMinRangeBeforeBend() * Board.Get().squareSize;
			}
			else if (magnitude > GetMaxRangeBeforeBend() * Board.Get().squareSize)
			{
				num = GetMaxRangeBeforeBend() * Board.Get().squareSize;
			}
			else
			{
				num = magnitude;
			}
			overridePos = loSCheckPos + abilityTarget.AimDirection * num;
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherBendingArrow))
		{
			m_abilityMod = abilityMod as AbilityMod_ArcherBendingArrow;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		m_cachedLaserHitEffect = m_abilityMod != null
			? m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect;
		m_cachedEffectToHealingDebuffTarget = m_abilityMod != null
			? m_abilityMod.m_effectToHealingDebuffTarget.GetModifiedValue(null)
			: null;
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

	public StandardEffectInfo GetLaserHitEffect()
	{
		return m_cachedLaserHitEffect ?? m_laserHitEffect;
	}

	public int GetExtraAbsorbForShieldGeneratorPerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_nextShieldGeneratorExtraAbsorbPerHit.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraAbsorbForShieldGeneratorMax()
	{
		return m_abilityMod != null
			? m_abilityMod.m_nextShieldGeneratorExtraAbsorbMax.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraDamageToHealingDebuffTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageToHealingDebuffTarget.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraDamageAfterBend()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageAfterBend.GetModifiedValue(0)
			: 0;
	}

	public int GetDamageAfterPierce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAfterPiercingMod.GetModifiedValue(GetLaserDamageAmount())
			: GetLaserDamageAmount();
	}

	public StandardEffectInfo GetEffectToHealingDebuffTarget()
	{
		return m_cachedEffectToHealingDebuffTarget;
	}

	public int GetExtraHealingFromHealingDebuffTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealingFromHealingDebuffTarget.GetModifiedValue(0)
			: 0;
	}
	
#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_extraAbsorbForShieldGenerator = Mathf.Min(
				m_syncComp.m_extraAbsorbForShieldGenerator + GetExtraAbsorbForShieldGeneratorPerHit() * additionalData.m_abilityResults.HitActorList().Count,
				GetExtraAbsorbForShieldGeneratorMax());
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out List<Vector3> endPoints, out List<ActorData> actorsHitAfterBounce, nonActorTargetInfo);
		if (hitActors.Count > 1)
		{
			ActorData value = hitActors[0];
			hitActors[0] = hitActors[hitActors.Count - 1];
			hitActors[hitActors.Count - 1] = value;
		}
		BouncingShotSequence.ExtraParams extraParams = new BouncingShotSequence.ExtraParams();
		extraParams.segmentPts = new List<Vector3>();
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
			new Sequence.IExtraSequenceParams[]
			{
				extraParams
			});
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out List<Vector3> endPoints, out List<ActorData> actorsHitAfterBounce, nonActorTargetInfo);
		for (int i = 0; i < hitActors.Count; i++)
		{
			ActorData hitActor = hitActors[i];
			Vector3 loSCheckPos = caster.GetLoSCheckPos();
			bool flag = false;
			int num = i == 0
				? GetLaserDamageAmount()
				: GetDamageAfterPierce();
			if (actorsHitAfterBounce.Contains(hitActor))
			{
				loSCheckPos = endPoints[1];
				flag = true;
				if (Board.Get().GetSquareFromVec3(loSCheckPos) == hitActor.GetCurrentBoardSquare())
				{
					loSCheckPos = caster.GetLoSCheckPos();
				}
				num += GetExtraDamageAfterBend();
			}
			ActorHitParameters hitParams = new ActorHitParameters(hitActor, loSCheckPos);
			ActorHitResults actorHitResults = new ActorHitResults(GetLaserHitEffect(), hitParams);
			if (ServerEffectManager.Get().HasEffectByCaster(hitActor, caster, typeof(ArcherHealingReactionEffect))
			    && !m_syncComp.ActorHasUsedHealReaction(caster))
			{
				num += GetExtraDamageToHealingDebuffTarget();
				num += m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
				actorHitResults.AddStandardEffectInfo(GetEffectToHealingDebuffTarget());
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
		endPoints = new List<Vector3> { caster.GetLoSCheckPos() };
		float laserRangeInSquares = GetClampedRangeInSquares(caster, targets[0]);
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			endPoints[0],
			targets[0].AimDirection,
			laserRangeInSquares,
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
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, endPoints[0]);
		if (actorsInLaser.Count < GetMaxTargets()
		    && (laserEndPos - endPoints[0]).magnitude > laserRangeInSquares * Board.Get().squareSize - 0.1f)
		{
			laserRangeInSquares = GetDistanceRemaining(caster, targets[0], out Vector3 adjustedStartPosWithOffset);
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
			actorsHitAfterBounce = AreaEffectUtils.GetActorsInLaser(
				adjustedStartPosWithOffset,
				targeterClampedAimDirection,
				laserRangeInSquares,
				GetLaserWidth(),
				caster,
				relevantTeams,
				PenetrateLoS(),
				GetMaxTargets(),
				false,
				true,
				out Vector3 endPos,
				nonActorTargetInfo);
			TargeterUtils.SortActorsByDistanceToPos(ref actorsHitAfterBounce, endPos);
			for (int i = actorsHitAfterBounce.Count - 1; i >= 0; i--)
			{
				ActorData item = actorsHitAfterBounce[i];
				if (!actorsInLaser.Contains(item) && actorsInLaser.Count < GetMaxTargets())
				{
					actorsInLaser.Add(item);
				}
				else
				{
					actorsHitAfterBounce.Remove(item);
				}
			}
			endPoints.Add(endPos);
		}
		else
		{
			actorsHitAfterBounce = new List<ActorData>();
		}
		return actorsInLaser;
	}
#endif
}
