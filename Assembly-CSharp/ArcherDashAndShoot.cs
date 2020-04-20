using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDashAndShoot : Ability
{
	[Header("-- Targeting")]
	public float m_maxAngleForLaser = 30f;

	public float m_laserWidth = 0.5f;

	public float m_laserRange = 5.5f;

	public float m_aoeRadius = 1f;

	public bool m_aoePenetratesLoS;

	[Header("-- Enemy hits")]
	public int m_directDamage;

	public int m_aoeDamage;

	public StandardEffectInfo m_directTargetEffect;

	public StandardEffectInfo m_aoeTargetEffect;

	[Header("-- Sequences")]
	public GameObject m_dashSequencePrefab;

	public GameObject m_arrowProjectileSequencePrefab;

	private AbilityMod_ArcherDashAndShoot m_abilityMod;

	private ArcherShieldGeneratorArrow m_shieldGenAbility;

	private ArcherHealingDebuffArrow m_healArrowAbility;

	private AbilityData.ActionType m_healArrowActionType = AbilityData.ActionType.INVALID_ACTION;

	private ActorTargeting m_actorTargeting;

	private Archer_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedDirectTargetEffect;

	private StandardEffectInfo m_cachedAoeTargetEffect;

	private StandardEffectInfo m_cachedHealingDebuffTargetEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "ArcherDashAndShoot";
		}
		this.m_shieldGenAbility = (base.GetAbilityOfType(typeof(ArcherShieldGeneratorArrow)) as ArcherShieldGeneratorArrow);
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
		this.m_syncComp = base.GetComponent<Archer_SyncComponent>();
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeters.Clear();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_DashAndAim abilityUtil_Targeter_DashAndAim = new AbilityUtil_Targeter_DashAndAim(this, this.GetAoeRadius(), this.AoePenetratesLoS(), this.GetLaserWidth(), this.GetLaserRange(), this.GetMaxAngleForLaser(), new AbilityUtil_Targeter_DashAndAim.GetClampedLaserDirection(this.GetClampedLaserDirection), true, this.GetMovementType() != ActorData.MovementType.Charge, 1);
			abilityUtil_Targeter_DashAndAim.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_DashAndAim);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	internal Vector3 GetClampedLaserDirection(AbilityTarget dashTarget, AbilityTarget shootTarget, Vector3 neutralDir)
	{
		Vector3 vector = (shootTarget.FreePos - dashTarget.FreePos).normalized;
		float num = Vector3.Angle(neutralDir, vector);
		if (num > this.GetMaxAngleForLaser())
		{
			vector = Vector3.RotateTowards(vector, neutralDir, (num - this.GetMaxAngleForLaser()) * 0.0174532924f, 0f);
		}
		return vector;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex != 0)
		{
			return true;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null && boardSquareSafe.IsBaselineHeight() && boardSquareSafe != caster.GetCurrentBoardSquare())
		{
			int num;
			return KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out num);
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_directDamage),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, this.m_aoeDamage)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (this.m_syncComp != null)
		{
			int num = this.GetDirectDamage();
			List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[1].GetTooltipSubjectTypes(targetActor);
			if (!tooltipSubjectTypes.IsNullOrEmpty<AbilityTooltipSubject>())
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
				{
					num = this.GetAoeDamage();
				}
			}
			if (this.IsReactionHealTarget(targetActor))
			{
				num += this.m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
			}
			dictionary[AbilityTooltipSymbol.Damage] = num;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
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
		base.AddTokenInt(tokens, "DirectDamage", string.Empty, this.m_directDamage, false);
		base.AddTokenInt(tokens, "AoeDamage", string.Empty, this.m_aoeDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_directTargetEffect, "DirectTargetEffect", this.m_directTargetEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_aoeTargetEffect, "AoeTargetEffect", this.m_aoeTargetEffect, true);
	}

	public unsafe override bool HasRestrictedFreeAimDegrees(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			Vector3 b;
			this.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out b);
			Vector3 vec = aimingActor.GetTravelBoardSquareWorldPosition() - b;
			vec.Normalize();
			float num = VectorUtils.HorizontalAngle_Deg(vec);
			min = num - this.GetMaxAngleForLaser();
			max = num + this.GetMaxAngleForLaser();
			return true;
		}
		return base.HasRestrictedFreeAimDegrees(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			min = 1f;
			max = 1f;
			return true;
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			AbilityTarget abilityTarget = targetsSoFar[0];
			overridePos = aimingActor.GetSquareWorldPosition(Board.Get().GetBoardSquareSafe(abilityTarget.GridPos));
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherDashAndShoot))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ArcherDashAndShoot);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	private void SetCachedFields()
	{
		this.m_cachedDirectTargetEffect = ((!this.m_abilityMod) ? this.m_directTargetEffect : this.m_abilityMod.m_directTargetEffectMod.GetModifiedValue(this.m_directTargetEffect));
		StandardEffectInfo cachedAoeTargetEffect;
		if (this.m_abilityMod)
		{
			cachedAoeTargetEffect = this.m_abilityMod.m_aoeTargetEffectMod.GetModifiedValue(this.m_aoeTargetEffect);
		}
		else
		{
			cachedAoeTargetEffect = this.m_aoeTargetEffect;
		}
		this.m_cachedAoeTargetEffect = cachedAoeTargetEffect;
		StandardEffectInfo cachedHealingDebuffTargetEffect;
		if (this.m_abilityMod)
		{
			cachedHealingDebuffTargetEffect = this.m_abilityMod.m_healingDebuffTargetEffect.GetModifiedValue(null);
		}
		else
		{
			cachedHealingDebuffTargetEffect = null;
		}
		this.m_cachedHealingDebuffTargetEffect = cachedHealingDebuffTargetEffect;
	}

	public float GetMaxAngleForLaser()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxAngleForLaserMod.GetModifiedValue(this.m_maxAngleForLaser);
		}
		else
		{
			result = this.m_maxAngleForLaser;
		}
		return result;
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

	public float GetLaserRange()
	{
		return (!this.m_abilityMod) ? this.m_laserRange : this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
	}

	public float GetAoeRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_aoeRadiusMod.GetModifiedValue(this.m_aoeRadius);
		}
		else
		{
			result = this.m_aoeRadius;
		}
		return result;
	}

	public bool AoePenetratesLoS()
	{
		return (!this.m_abilityMod) ? this.m_aoePenetratesLoS : this.m_abilityMod.m_aoePenetratesLoSMod.GetModifiedValue(this.m_aoePenetratesLoS);
	}

	public int GetDirectDamage()
	{
		return (!this.m_abilityMod) ? this.m_directDamage : this.m_abilityMod.m_directDamageMod.GetModifiedValue(this.m_directDamage);
	}

	public int GetAoeDamage()
	{
		return (!this.m_abilityMod) ? this.m_aoeDamage : this.m_abilityMod.m_aoeDamageMod.GetModifiedValue(this.m_aoeDamage);
	}

	public StandardEffectInfo GetDirectTargetEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedDirectTargetEffect != null)
		{
			result = this.m_cachedDirectTargetEffect;
		}
		else
		{
			result = this.m_directTargetEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAoeTargetEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAoeTargetEffect != null)
		{
			result = this.m_cachedAoeTargetEffect;
		}
		else
		{
			result = this.m_aoeTargetEffect;
		}
		return result;
	}

	public StandardEffectInfo GetHealingDebuffTargetEffect()
	{
		return this.m_cachedHealingDebuffTargetEffect;
	}

	public int GetCooldownAdjustmentEachTurnIfUnderHealthThreshold()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_cooldownAdjustmentEachTurnUnderHealthThreshold.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public float GetHealthThresholdForCooldownOverride()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healthThresholdForCooldownOverride.GetModifiedValue(0f);
		}
		else
		{
			result = 0f;
		}
		return result;
	}
}
