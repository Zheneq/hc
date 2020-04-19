using System;
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithWeaponsOfWar : Ability
{
	[Header("-- Targeting in Ability")]
	public bool m_canTargetEnemies = true;

	public bool m_canTargetAllies = true;

	[Header("-- Effect on Ability Target")]
	public StandardEffectInfo m_targetAllyOnHitEffect;

	public StandardEffectInfo m_targetEnemyOnHitEffect;

	[Header("-- Sweep Info")]
	public int m_sweepDamageAmount = 0xA;

	public int m_sweepDuration = 3;

	public int m_sweepDamageDelay;

	[Header("-- Sweep On Hit Effects")]
	public StandardEffectInfo m_enemySweepOnHitEffect;

	public StandardEffectInfo m_allySweepOnHitEffect;

	[Header("-- Sweep Targeting")]
	public AbilityAreaShape m_sweepShape = AbilityAreaShape.Three_x_Three;

	public bool m_sweepIncludeEnemies = true;

	public bool m_sweepIncludeAllies;

	public bool m_sweepPenetrateLineOfSight;

	public bool m_sweepIncludeTarget;

	[Header("-- Sequences -----------------------------")]
	public GameObject m_castSequencePrefab;

	public GameObject m_persistentSequencePrefab;

	public GameObject m_rangeIndicatorSequencePrefab;

	public GameObject m_bladeSequencePrefab;

	public GameObject m_shieldPerTurnSequencePrefab;

	private AbilityMod_NanoSmithWeaponsOfWar m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Weapons of War";
		}
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, this.m_sweepShape, this.m_sweepPenetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, AbilityUtil_Targeter.AffectsActor.Always);
		abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Secondary, AbilityTooltipSubject.None);
		base.Targeter = abilityUtil_Targeter_Shape;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetAllyTargetEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetSweepDamage());
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, this.m_canTargetEnemies, this.m_canTargetAllies, this.m_canTargetAllies, Ability.ValidateCheckPath.Ignore, true, true, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithWeaponsOfWar abilityMod_NanoSmithWeaponsOfWar = modAsBase as AbilityMod_NanoSmithWeaponsOfWar;
		StandardEffectInfo effectInfo;
		if (abilityMod_NanoSmithWeaponsOfWar)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithWeaponsOfWar.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_NanoSmithWeaponsOfWar.m_allyTargetEffectOverride.GetModifiedValue(this.m_targetAllyOnHitEffect);
		}
		else
		{
			effectInfo = this.m_targetAllyOnHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "TargetAllyOnHitEffect", this.m_targetAllyOnHitEffect, true);
		string name = "SweepDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_NanoSmithWeaponsOfWar)
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
			val = abilityMod_NanoSmithWeaponsOfWar.m_sweepDamageMod.GetModifiedValue(this.m_sweepDamageAmount);
		}
		else
		{
			val = this.m_sweepDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "SweepDuration", string.Empty, (!abilityMod_NanoSmithWeaponsOfWar) ? this.m_sweepDuration : abilityMod_NanoSmithWeaponsOfWar.m_sweepDurationMod.GetModifiedValue(this.m_sweepDuration), false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_NanoSmithWeaponsOfWar)
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
			effectInfo2 = abilityMod_NanoSmithWeaponsOfWar.m_enemySweepOnHitEffectOverride.GetModifiedValue(this.m_enemySweepOnHitEffect);
		}
		else
		{
			effectInfo2 = this.m_enemySweepOnHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemySweepOnHitEffect", this.m_enemySweepOnHitEffect, true);
		StandardEffectInfo effectInfo3;
		if (abilityMod_NanoSmithWeaponsOfWar)
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
			effectInfo3 = abilityMod_NanoSmithWeaponsOfWar.m_allySweepOnHitEffectOverride.GetModifiedValue(this.m_allySweepOnHitEffect);
		}
		else
		{
			effectInfo3 = this.m_allySweepOnHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "AllySweepOnHitEffect", this.m_allySweepOnHitEffect, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithWeaponsOfWar))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_NanoSmithWeaponsOfWar);
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
	}

	private int GetSweepDuration()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_sweepDurationMod.GetModifiedValue(this.m_sweepDuration) : this.m_sweepDuration;
	}

	private int GetSweepDamage()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithWeaponsOfWar.GetSweepDamage()).MethodHandle;
			}
			result = this.m_sweepDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_sweepDamageMod.GetModifiedValue(this.m_sweepDamageAmount);
		}
		return result;
	}

	private int GetShieldGainPerTurn()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithWeaponsOfWar.GetShieldGainPerTurn()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_shieldGainPerTurnMod.GetModifiedValue(0);
		}
		return result;
	}

	private StandardEffectInfo GetAllyTargetEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithWeaponsOfWar.GetAllyTargetEffect()).MethodHandle;
			}
			result = this.m_targetAllyOnHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_allyTargetEffectOverride.GetModifiedValue(this.m_targetAllyOnHitEffect);
		}
		return result;
	}

	private StandardEffectInfo GetAllySweepEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithWeaponsOfWar.GetAllySweepEffect()).MethodHandle;
			}
			result = this.m_allySweepOnHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_allySweepOnHitEffectOverride.GetModifiedValue(this.m_allySweepOnHitEffect);
		}
		return result;
	}

	private StandardEffectInfo GetEnemySweepEffect()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_enemySweepOnHitEffectOverride.GetModifiedValue(this.m_enemySweepOnHitEffect) : this.m_enemySweepOnHitEffect;
	}
}
