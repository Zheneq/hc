using System;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDashAndAimedSlash : Ability
{
	[Separator("Targeting")]
	public float m_maxAngleForLaser = 30f;
	public float m_laserWidth = 0.5f;
	public float m_laserRange = 5.5f;
	public int m_maxTargets = 5;
	public bool m_canMoveAfterEvade;
	[Separator("Enemy hits")]
	public int m_damageAmount;
	public int m_extraDamageIfSingleTarget;
	public StandardEffectInfo m_targetEffect;
	[Separator("Self Hit")]
	public StandardEffectInfo m_effectOnSelf;
	[Separator("Sequences")]
	public GameObject m_dashSequencePrefab;
	public GameObject m_slashSequencePrefab;

	private AbilityMod_SamuraiDashAndAimedSlash m_abilityMod;
	private Samurai_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedTargetEffect;
	private StandardEffectInfo m_cachedEffectOnSelf;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SamuraiDashAndAimedSlash";
		}
		m_syncComponent = ActorData.GetComponent<Samurai_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		ClearTargeters();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_DashAndAim targeter = new AbilityUtil_Targeter_DashAndAim(
				this,
				0f,
				false,
				GetLaserWidth(),
				GetLaserRange(),
				GetMaxAngleForLaser(),
				GetClampedLaserDirection,
				false,
				false,
				GetMaxTargets());
			targeter.SetUseMultiTargetUpdate(true);
			targeter.SetAffectedGroups(true, false, true);
			targeter.AllowChargeThroughInvalidSquares = false;
			Targeters.Add(targeter);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public override bool ShouldAutoConfirmIfTargetingOnEndTurn()
	{
		return true;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return m_canMoveAfterEvade;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool isValidDash = true;
		if (targetIndex == 0)
		{
			BoardSquarePathInfo dashPath = KnockbackUtils.BuildStraightLineChargePath(
				caster,
				Board.Get().GetSquare(target.GridPos),
				caster.GetCurrentBoardSquare(),
				false);
			isValidDash = dashPath != null;
		}
		return isValidDash && base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
	}

	internal Vector3 GetClampedLaserDirection(AbilityTarget dashTarget, AbilityTarget shootTarget, Vector3 neutralDir)
	{
		Vector3 vector = shootTarget.FreePos - dashTarget.FreePos;
		vector.y = 0f;
		neutralDir.y = 0f;
		float num = Vector3.Angle(neutralDir, vector);
		if (num > GetMaxAngleForLaser())
		{
			vector = Vector3.RotateTowards(vector, neutralDir, (num - GetMaxAngleForLaser()) * ((float)Math.PI / 180f), 0f);
		}
		return vector.normalized;
	}

	private void SetCachedFields()
	{
		m_cachedTargetEffect = m_abilityMod != null
			? m_abilityMod.m_targetEffectMod.GetModifiedValue(m_targetEffect)
			: m_targetEffect;
		m_cachedEffectOnSelf = m_abilityMod != null
			? m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf)
			: m_effectOnSelf;
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

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetExtraDamageIfSingleTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageIfSingleTargetMod.GetModifiedValue(m_extraDamageIfSingleTarget)
			: m_extraDamageIfSingleTarget;
	}

	public StandardEffectInfo GetTargetEffect()
	{
		return m_cachedTargetEffect ?? m_targetEffect;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		return m_cachedEffectOnSelf ?? m_effectOnSelf;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "ExtraDamageIfSingleTarget", string.Empty, m_extraDamageIfSingleTarget);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetEffect, "TargetEffect", m_targetEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnSelf, "EffectOnSelf", m_effectOnSelf);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDamageAmount()));
		GetEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (!(Targeters[currentTargeterIndex] is AbilityUtil_Targeter_DashAndAim targeter))
		{
			return false;
		}
		if (targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			results.m_damage = GetDamageAmount();
			if (m_syncComponent != null)
			{
				results.m_damage += m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
			}
			if (GetExtraDamageIfSingleTarget() > 0 && targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 1)
			{
				results.m_damage += GetExtraDamageIfSingleTarget();
			}
		}
		else if (targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			results.m_absorb = GetEffectOnSelf().m_applyEffect
				? GetEffectOnSelf().m_effectData.m_absorbAmount
				: 0;
		}
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiDashAndAimedSlash))
		{
			m_abilityMod = abilityMod as AbilityMod_SamuraiDashAndAimedSlash;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
