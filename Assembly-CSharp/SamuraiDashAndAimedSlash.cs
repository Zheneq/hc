using System;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDashAndAimedSlash : Ability
{
	[Separator("Targeting", true)]
	public float m_maxAngleForLaser = 30f;

	public float m_laserWidth = 0.5f;

	public float m_laserRange = 5.5f;

	public int m_maxTargets = 5;

	public bool m_canMoveAfterEvade;

	[Separator("Enemy hits", true)]
	public int m_damageAmount;

	public int m_extraDamageIfSingleTarget;

	public StandardEffectInfo m_targetEffect;

	[Separator("Self Hit", true)]
	public StandardEffectInfo m_effectOnSelf;

	[Separator("Sequences", true)]
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
		m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		ClearTargeters();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_DashAndAim abilityUtil_Targeter_DashAndAim = new AbilityUtil_Targeter_DashAndAim(this, 0f, false, GetLaserWidth(), GetLaserRange(), GetMaxAngleForLaser(), GetClampedLaserDirection, false, false, GetMaxTargets());
			abilityUtil_Targeter_DashAndAim.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_DashAndAim.SetAffectedGroups(true, false, true);
			abilityUtil_Targeter_DashAndAim.AllowChargeThroughInvalidSquares = false;
			base.Targeters.Add(abilityUtil_Targeter_DashAndAim);
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
		bool flag = true;
		if (targetIndex == 0)
		{
			flag = (KnockbackUtils.BuildStraightLineChargePath(caster, Board.Get().GetSquare(target.GridPos), caster.GetCurrentBoardSquare(), false) != null);
		}
		return flag && base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
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
		StandardEffectInfo cachedTargetEffect;
		if ((bool)m_abilityMod)
		{
			cachedTargetEffect = m_abilityMod.m_targetEffectMod.GetModifiedValue(m_targetEffect);
		}
		else
		{
			cachedTargetEffect = m_targetEffect;
		}
		m_cachedTargetEffect = cachedTargetEffect;
		StandardEffectInfo cachedEffectOnSelf;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnSelf = m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf);
		}
		else
		{
			cachedEffectOnSelf = m_effectOnSelf;
		}
		m_cachedEffectOnSelf = cachedEffectOnSelf;
	}

	public float GetMaxAngleForLaser()
	{
		return (!m_abilityMod) ? m_maxAngleForLaser : m_abilityMod.m_maxAngleForLaserMod.GetModifiedValue(m_maxAngleForLaser);
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public float GetLaserRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
		}
		else
		{
			result = m_laserRange;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!m_abilityMod) ? m_damageAmount : m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
	}

	public int GetExtraDamageIfSingleTarget()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageIfSingleTargetMod.GetModifiedValue(m_extraDamageIfSingleTarget);
		}
		else
		{
			result = m_extraDamageIfSingleTarget;
		}
		return result;
	}

	public StandardEffectInfo GetTargetEffect()
	{
		return (m_cachedTargetEffect == null) ? m_targetEffect : m_cachedTargetEffect;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelf != null)
		{
			result = m_cachedEffectOnSelf;
		}
		else
		{
			result = m_effectOnSelf;
		}
		return result;
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
		AbilityUtil_Targeter_DashAndAim abilityUtil_Targeter_DashAndAim = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_DashAndAim;
		if (abilityUtil_Targeter_DashAndAim != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (abilityUtil_Targeter_DashAndAim.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
					{
						results.m_damage = GetDamageAmount();
						if (m_syncComponent != null)
						{
							results.m_damage += m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
						}
						if (GetExtraDamageIfSingleTarget() > 0)
						{
							int visibleActorsCountByTooltipSubject = abilityUtil_Targeter_DashAndAim.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
							if (visibleActorsCountByTooltipSubject == 1)
							{
								results.m_damage += GetExtraDamageIfSingleTarget();
							}
						}
					}
					else if (abilityUtil_Targeter_DashAndAim.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
					{
						int absorb;
						if (GetEffectOnSelf().m_applyEffect)
						{
							absorb = GetEffectOnSelf().m_effectData.m_absorbAmount;
						}
						else
						{
							absorb = 0;
						}
						results.m_absorb = absorb;
					}
					return true;
				}
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SamuraiDashAndAimedSlash))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_SamuraiDashAndAimedSlash);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
