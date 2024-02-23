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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ArcherDashAndShoot";
		}
		m_shieldGenAbility = GetAbilityOfType(typeof(ArcherShieldGeneratorArrow)) as ArcherShieldGeneratorArrow;
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
		m_syncComp = GetComponent<Archer_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeters.Clear();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_DashAndAim abilityUtil_Targeter_DashAndAim = new AbilityUtil_Targeter_DashAndAim(
				this,
				GetAoeRadius(),
				AoePenetratesLoS(),
				GetLaserWidth(),
				GetLaserRange(),
				GetMaxAngleForLaser(),
				GetClampedLaserDirection,
				true,
				GetMovementType() != ActorData.MovementType.Charge,
				1);
			abilityUtil_Targeter_DashAndAim.SetUseMultiTargetUpdate(true);
			Targeters.Add(abilityUtil_Targeter_DashAndAim);
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
		if (num > GetMaxAngleForLaser())
		{
			vector = Vector3.RotateTowards(vector, neutralDir, (num - GetMaxAngleForLaser()) * ((float)Math.PI / 180f), 0f);
		}
		return vector;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex != 0)
		{
			return true;
		}
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare != null
		    && targetSquare.IsValidForGameplay()
		    && targetSquare != caster.GetCurrentBoardSquare())
		{
			int foo;
			return KnockbackUtils.CanBuildStraightLineChargePath(caster, targetSquare, caster.GetCurrentBoardSquare(), false, out foo);
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_directDamage),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, m_aoeDamage)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (m_syncComp != null)
		{
			int damage = GetDirectDamage();
			List<AbilityTooltipSubject> tooltipSubjectTypes = Targeters[1].GetTooltipSubjectTypes(targetActor);
			if (!tooltipSubjectTypes.IsNullOrEmpty() && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				damage = GetAoeDamage();
			}
			if (IsReactionHealTarget(targetActor))
			{
				damage += m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
			}
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = Targeters[currentTargeterIndex].GetActorsInRange();
		foreach (AbilityUtil_Targeter.ActorTarget actor in actorsInRange)
		{
			if (IsReactionHealTarget(actor.m_actor))
			{
				return m_healArrowAbility.GetTechPointsPerHeal();
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	private bool IsReactionHealTarget(ActorData targetActor)
	{
		if (m_syncComp.m_healReactionTargetActor == targetActor.ActorIndex && !m_syncComp.ActorHasUsedHealReaction(ActorData))
		{
			return true;
		}
		if (m_healArrowActionType == AbilityData.ActionType.INVALID_ACTION || m_actorTargeting == null)
		{
			return false;
		}
		List<AbilityTarget> abilityTargetsInRequest = m_actorTargeting.GetAbilityTargetsInRequest(m_healArrowActionType);
		if (abilityTargetsInRequest != null && abilityTargetsInRequest.Count > 0)
		{
			BoardSquare targetSquare = Board.Get().GetSquare(abilityTargetsInRequest[0].GridPos);
			ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(targetSquare, true, false, ActorData);
			if (targetableActorOnSquare == targetActor)
			{
				return true;
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DirectDamage", string.Empty, m_directDamage);
		AddTokenInt(tokens, "AoeDamage", string.Empty, m_aoeDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_directTargetEffect, "DirectTargetEffect", m_directTargetEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_aoeTargetEffect, "AoeTargetEffect", m_aoeTargetEffect);
	}

	public override bool HasRestrictedFreeAimDegrees(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			Vector3 overridePos;
			HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
			Vector3 vec = aimingActor.GetFreePos() - overridePos;
			vec.Normalize();
			float num = VectorUtils.HorizontalAngle_Deg(vec);
			min = num - GetMaxAngleForLaser();
			max = num + GetMaxAngleForLaser();
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
			overridePos = aimingActor.GetFreePos(Board.Get().GetSquare(abilityTarget.GridPos));
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherDashAndShoot))
		{
			m_abilityMod = abilityMod as AbilityMod_ArcherDashAndShoot;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void SetCachedFields()
	{
		m_cachedDirectTargetEffect = m_abilityMod != null
			? m_abilityMod.m_directTargetEffectMod.GetModifiedValue(m_directTargetEffect)
			: m_directTargetEffect;
		m_cachedAoeTargetEffect = m_abilityMod != null
			? m_abilityMod.m_aoeTargetEffectMod.GetModifiedValue(m_aoeTargetEffect)
			: m_aoeTargetEffect;
		m_cachedHealingDebuffTargetEffect = m_abilityMod != null
			? m_abilityMod.m_healingDebuffTargetEffect.GetModifiedValue(null)
			: null;
	}

	public float GetMaxAngleForLaser()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxAngleForLaserMod.GetModifiedValue(m_maxAngleForLaser)
			: m_maxAngleForLaser;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange)
			: m_laserRange;
	}

	public float GetAoeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeRadiusMod.GetModifiedValue(m_aoeRadius)
			: m_aoeRadius;
	}

	public bool AoePenetratesLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoePenetratesLoSMod.GetModifiedValue(m_aoePenetratesLoS)
			: m_aoePenetratesLoS;
	}

	public int GetDirectDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_directDamageMod.GetModifiedValue(m_directDamage)
			: m_directDamage;
	}

	public int GetAoeDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeDamageMod.GetModifiedValue(m_aoeDamage)
			: m_aoeDamage;
	}

	public StandardEffectInfo GetDirectTargetEffect()
	{
		return m_cachedDirectTargetEffect ?? m_directTargetEffect;
	}

	public StandardEffectInfo GetAoeTargetEffect()
	{
		return m_cachedAoeTargetEffect ?? m_aoeTargetEffect;
	}

	public StandardEffectInfo GetHealingDebuffTargetEffect()
	{
		return m_cachedHealingDebuffTargetEffect;
	}

	public int GetCooldownAdjustmentEachTurnIfUnderHealthThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownAdjustmentEachTurnUnderHealthThreshold.GetModifiedValue(0)
			: 0;
	}

	public float GetHealthThresholdForCooldownOverride()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healthThresholdForCooldownOverride.GetModifiedValue(0f)
			: 0f;
	}
}
