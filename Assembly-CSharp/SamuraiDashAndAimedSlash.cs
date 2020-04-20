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
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.Start()).MethodHandle;
			}
			this.m_abilityName = "SamuraiDashAndAimedSlash";
		}
		this.m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.ClearTargeters();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_DashAndAim abilityUtil_Targeter_DashAndAim = new AbilityUtil_Targeter_DashAndAim(this, 0f, false, this.GetLaserWidth(), this.GetLaserRange(), this.GetMaxAngleForLaser(), new AbilityUtil_Targeter_DashAndAim.GetClampedLaserDirection(this.GetClampedLaserDirection), false, false, this.GetMaxTargets());
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
		return this.m_canMoveAfterEvade;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = true;
		if (targetIndex == 0)
		{
			flag = (KnockbackUtils.BuildStraightLineChargePath(caster, Board.Get().GetBoardSquareSafe(target.GridPos), caster.GetCurrentBoardSquare(), false) != null);
		}
		return flag && base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
	}

	internal Vector3 GetClampedLaserDirection(AbilityTarget dashTarget, AbilityTarget shootTarget, Vector3 neutralDir)
	{
		Vector3 vector = shootTarget.FreePos - dashTarget.FreePos;
		vector.y = 0f;
		neutralDir.y = 0f;
		float num = Vector3.Angle(neutralDir, vector);
		if (num > this.GetMaxAngleForLaser())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.GetClampedLaserDirection(AbilityTarget, AbilityTarget, Vector3)).MethodHandle;
			}
			vector = Vector3.RotateTowards(vector, neutralDir, (num - this.GetMaxAngleForLaser()) * 0.0174532924f, 0f);
		}
		return vector.normalized;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTargetEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.SetCachedFields()).MethodHandle;
			}
			cachedTargetEffect = this.m_abilityMod.m_targetEffectMod.GetModifiedValue(this.m_targetEffect);
		}
		else
		{
			cachedTargetEffect = this.m_targetEffect;
		}
		this.m_cachedTargetEffect = cachedTargetEffect;
		StandardEffectInfo cachedEffectOnSelf;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEffectOnSelf = this.m_abilityMod.m_effectOnSelfMod.GetModifiedValue(this.m_effectOnSelf);
		}
		else
		{
			cachedEffectOnSelf = this.m_effectOnSelf;
		}
		this.m_cachedEffectOnSelf = cachedEffectOnSelf;
	}

	public float GetMaxAngleForLaser()
	{
		return (!this.m_abilityMod) ? this.m_maxAngleForLaser : this.m_abilityMod.m_maxAngleForLaserMod.GetModifiedValue(this.m_maxAngleForLaser);
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.GetLaserWidth()).MethodHandle;
			}
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
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.GetLaserRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
		}
		else
		{
			result = this.m_laserRange;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.GetMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_damageAmount : this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
	}

	public int GetExtraDamageIfSingleTarget()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.GetExtraDamageIfSingleTarget()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageIfSingleTargetMod.GetModifiedValue(this.m_extraDamageIfSingleTarget);
		}
		else
		{
			result = this.m_extraDamageIfSingleTarget;
		}
		return result;
	}

	public StandardEffectInfo GetTargetEffect()
	{
		return (this.m_cachedTargetEffect == null) ? this.m_targetEffect : this.m_cachedTargetEffect;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelf != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.GetEffectOnSelf()).MethodHandle;
			}
			result = this.m_cachedEffectOnSelf;
		}
		else
		{
			result = this.m_effectOnSelf;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "ExtraDamageIfSingleTarget", string.Empty, this.m_extraDamageIfSingleTarget, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_targetEffect, "TargetEffect", this.m_targetEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnSelf, "EffectOnSelf", this.m_effectOnSelf, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.GetDamageAmount()));
		this.GetEffectOnSelf().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Self);
		return list;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		AbilityUtil_Targeter_DashAndAim abilityUtil_Targeter_DashAndAim = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_DashAndAim;
		if (abilityUtil_Targeter_DashAndAim != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (abilityUtil_Targeter_DashAndAim.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
			{
				results.m_damage = this.GetDamageAmount();
				if (this.m_syncComponent != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					results.m_damage += this.m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
				}
				if (this.GetExtraDamageIfSingleTarget() > 0)
				{
					int visibleActorsCountByTooltipSubject = abilityUtil_Targeter_DashAndAim.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
					if (visibleActorsCountByTooltipSubject == 1)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						results.m_damage += this.GetExtraDamageIfSingleTarget();
					}
				}
			}
			else if (abilityUtil_Targeter_DashAndAim.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int absorb;
				if (this.GetEffectOnSelf().m_applyEffect)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					absorb = this.GetEffectOnSelf().m_effectData.m_absorbAmount;
				}
				else
				{
					absorb = 0;
				}
				results.m_absorb = absorb;
			}
			return true;
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiDashAndAimedSlash))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiDashAndAimedSlash.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SamuraiDashAndAimedSlash);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
