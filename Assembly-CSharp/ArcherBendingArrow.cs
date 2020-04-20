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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Bending Arrow";
		}
		this.m_syncComp = base.GetComponent<Archer_SyncComponent>();
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			this.m_healArrowAbility = (base.GetAbilityOfType(typeof(ArcherHealingDebuffArrow)) as ArcherHealingDebuffArrow);
			if (this.m_healArrowAbility != null)
			{
				this.m_healArrowActionType = component.GetActionTypeOfAbility(this.m_healArrowAbility);
			}
		}
		this.m_actorTargeting = base.GetComponent<ActorTargeting>();
		base.Targeters.Clear();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = new AbilityUtil_Targeter_BendingLaser(this, this.GetLaserWidth(), this.GetMinRangeBeforeBend(), this.GetMaxRangeBeforeBend(), this.GetMaxTotalRange(), this.GetMaxBendAngle(), this.PenetrateLoS(), this.GetMaxTargets(), false, false);
			abilityUtil_Targeter_BendingLaser.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_BendingLaser.m_startFadeAtActorRadius = this.m_startTargeterFadeAtActorRadius;
			base.Targeters.Add(abilityUtil_Targeter_BendingLaser);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (!base.Targeters.IsNullOrEmpty<AbilityUtil_Targeter>())
		{
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = base.Targeters[0] as AbilityUtil_Targeter_BendingLaser;
			if (abilityUtil_Targeter_BendingLaser.DidStopShort())
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
		return this.GetMaxTotalRange();
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, List<AbilityTarget> targets)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxBendAngle = this.GetMaxBendAngle();
		Vector3 aimDirection = targets[0].AimDirection;
		if (maxBendAngle > 0f && maxBendAngle < 360f)
		{
			aimDir = Vector3.RotateTowards(aimDirection, aimDir, 0.0174532924f * maxBendAngle, 0f);
		}
		return aimDir;
	}

	private float GetClampedRangeInSquares(ActorData targetingActor, AbilityTarget currentTarget)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float magnitude = (currentTarget.FreePos - travelBoardSquareWorldPositionForLos).magnitude;
		if (magnitude < this.GetMinRangeBeforeBend() * Board.Get().squareSize)
		{
			return this.GetMinRangeBeforeBend();
		}
		if (magnitude > this.GetMaxRangeBeforeBend() * Board.Get().squareSize)
		{
			return this.GetMaxRangeBeforeBend();
		}
		return magnitude / Board.Get().squareSize;
	}

	private float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float clampedRangeInSquares = this.GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = travelBoardSquareWorldPositionForLos + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
		return this.GetMaxTotalRange() - clampedRangeInSquares;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_laserDamageAmount > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_laserDamageAmount);
		}
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = this.GetLaserDamageAmount();
		bool flag = currentTargeterIndex == 0;
		AbilityUtil_Targeter.ActorTarget actorTarget = base.Targeters[currentTargeterIndex].GetActorsInRange().Find((AbilityUtil_Targeter.ActorTarget t) => t.m_actor == targetActor);
		if (actorTarget == null)
		{
			if (currentTargeterIndex > 0)
			{
				actorTarget = base.Targeters[0].GetActorsInRange().Find((AbilityUtil_Targeter.ActorTarget t) => t.m_actor == targetActor);
				flag = (actorTarget != null);
			}
		}
		if (actorTarget != null && !actorTarget.m_subjectTypes.Contains(AbilityTooltipSubject.Near))
		{
			num = this.GetDamageAfterPierce();
		}
		if (this.IsReactionHealTarget(targetActor))
		{
			num += this.GetExtraDamageToHealingDebuffTarget();
			num += this.m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
		}
		if (!flag)
		{
			num += this.GetExtraDamageAfterBend();
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[i].GetActorsInRange();
			using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
					if (this.IsReactionHealTarget(actorTarget.m_actor))
					{
						return this.m_healArrowAbility.GetTechPointsPerHeal();
					}
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	private bool IsReactionHealTarget(ActorData targetActor)
	{
		if (this.m_syncComp.m_healReactionTargetActor == targetActor.ActorIndex)
		{
			if (!this.m_syncComp.ActorHasUsedHealReaction(base.ActorData))
			{
				return true;
			}
		}
		if (this.m_healArrowActionType != AbilityData.ActionType.INVALID_ACTION)
		{
			if (this.m_actorTargeting != null)
			{
				List<AbilityTarget> abilityTargetsInRequest = this.m_actorTargeting.GetAbilityTargetsInRequest(this.m_healArrowActionType);
				if (abilityTargetsInRequest != null)
				{
					if (abilityTargetsInRequest.Count > 0)
					{
						BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(abilityTargetsInRequest[0].GridPos);
						ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(boardSquareSafe, true, false, base.ActorData);
						if (targetableActorOnSquare == targetActor)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "LaserDamageAmount", string.Empty, this.m_laserDamageAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserHitEffect, "LaserHitEffect", this.m_laserHitEffect, true);
	}

	public unsafe override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 0)
		{
			min = this.GetMinRangeBeforeBend() * Board.Get().squareSize;
			max = this.GetMaxRangeBeforeBend() * Board.Get().squareSize;
			return true;
		}
		if (targetIndex == 1)
		{
			min = 1f;
			max = 1f;
			return true;
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public unsafe override bool HasRestrictedFreeAimDegrees(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			Vector3 aimDirection = targetsSoFar[0].AimDirection;
			float num = VectorUtils.HorizontalAngle_Deg(aimDirection);
			min = num - this.GetMaxBendAngle();
			max = num + this.GetMaxBendAngle();
			return true;
		}
		return base.HasRestrictedFreeAimDegrees(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public unsafe override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			AbilityTarget abilityTarget = targetsSoFar[0];
			Vector3 travelBoardSquareWorldPositionForLos = aimingActor.GetTravelBoardSquareWorldPositionForLos();
			float magnitude = (abilityTarget.FreePos - travelBoardSquareWorldPositionForLos).magnitude;
			float d;
			if (magnitude < this.GetMinRangeBeforeBend() * Board.Get().squareSize)
			{
				d = this.GetMinRangeBeforeBend() * Board.Get().squareSize;
			}
			else if (magnitude > this.GetMaxRangeBeforeBend() * Board.Get().squareSize)
			{
				d = this.GetMaxRangeBeforeBend() * Board.Get().squareSize;
			}
			else
			{
				d = magnitude;
			}
			Vector3 vector = travelBoardSquareWorldPositionForLos + abilityTarget.AimDirection * d;
			overridePos = vector;
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherBendingArrow))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ArcherBendingArrow);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedLaserHitEffect;
		if (this.m_abilityMod)
		{
			cachedLaserHitEffect = this.m_abilityMod.m_laserHitEffectMod.GetModifiedValue(this.m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = this.m_laserHitEffect;
		}
		this.m_cachedLaserHitEffect = cachedLaserHitEffect;
		StandardEffectInfo cachedEffectToHealingDebuffTarget;
		if (this.m_abilityMod)
		{
			cachedEffectToHealingDebuffTarget = this.m_abilityMod.m_effectToHealingDebuffTarget.GetModifiedValue(null);
		}
		else
		{
			cachedEffectToHealingDebuffTarget = null;
		}
		this.m_cachedEffectToHealingDebuffTarget = cachedEffectToHealingDebuffTarget;
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public float GetMinRangeBeforeBend()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_minRangeBeforeBendMod.GetModifiedValue(this.m_minRangeBeforeBend);
		}
		else
		{
			result = this.m_minRangeBeforeBend;
		}
		return result;
	}

	public float GetMaxRangeBeforeBend()
	{
		return (!this.m_abilityMod) ? this.m_maxRangeBeforeBend : this.m_abilityMod.m_maxRangeBeforeBendMod.GetModifiedValue(this.m_maxRangeBeforeBend);
	}

	public float GetMaxTotalRange()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTotalRangeMod.GetModifiedValue(this.m_maxTotalRange);
		}
		else
		{
			result = this.m_maxTotalRange;
		}
		return result;
	}

	public float GetMaxBendAngle()
	{
		return (!this.m_abilityMod) ? this.m_maxBendAngle : this.m_abilityMod.m_maxBendAngleMod.GetModifiedValue(this.m_maxBendAngle);
	}

	public bool PenetrateLoS()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLoS : this.m_abilityMod.m_penetrateLoSMod.GetModifiedValue(this.m_penetrateLoS);
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public int GetLaserDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_laserDamageAmount : this.m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserHitEffect != null)
		{
			result = this.m_cachedLaserHitEffect;
		}
		else
		{
			result = this.m_laserHitEffect;
		}
		return result;
	}

	public int GetExtraAbsorbForShieldGeneratorPerHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_nextShieldGeneratorExtraAbsorbPerHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraAbsorbForShieldGeneratorMax()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_nextShieldGeneratorExtraAbsorbMax.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraDamageToHealingDebuffTarget()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageToHealingDebuffTarget.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraDamageAfterBend()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageAfterBend.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetDamageAfterPierce()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageAfterPiercingMod.GetModifiedValue(this.GetLaserDamageAmount());
		}
		else
		{
			result = this.GetLaserDamageAmount();
		}
		return result;
	}

	public StandardEffectInfo GetEffectToHealingDebuffTarget()
	{
		return this.m_cachedEffectToHealingDebuffTarget;
	}

	public int GetExtraHealingFromHealingDebuffTarget()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraHealingFromHealingDebuffTarget.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}
}
