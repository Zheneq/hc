using System;
using System.Collections.Generic;
using UnityEngine;

public class ThiefHiddenTrap : Ability
{
	[Header("-- Trap Ground Field")]
	public GroundEffectField m_trapFieldInfo;

	[Header("-- Extra Damage")]
	public int m_extraDamagePerTurn;

	public int m_maxExtraDamage;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ThiefHiddenTrap m_abilityMod;

	private GroundEffectField m_cachedTrapFieldInfo;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefHiddenTrap.Start()).MethodHandle;
			}
			this.m_abilityName = "Hidden Trap";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		GroundEffectField trapFieldInfo = this.GetTrapFieldInfo();
		AbilityUtil_Targeter.AffectsActor affectsCaster = (!trapFieldInfo.IncludeAllies()) ? AbilityUtil_Targeter.AffectsActor.Never : AbilityUtil_Targeter.AffectsActor.Possible;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, trapFieldInfo.shape, trapFieldInfo.penetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, trapFieldInfo.IncludeEnemies(), trapFieldInfo.IncludeAllies(), affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	private void SetCachedFields()
	{
		GroundEffectField cachedTrapFieldInfo;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefHiddenTrap.SetCachedFields()).MethodHandle;
			}
			cachedTrapFieldInfo = this.m_abilityMod.m_trapFieldInfoMod.GetModifiedValue(this.m_trapFieldInfo);
		}
		else
		{
			cachedTrapFieldInfo = this.m_trapFieldInfo;
		}
		this.m_cachedTrapFieldInfo = cachedTrapFieldInfo;
	}

	public GroundEffectField GetTrapFieldInfo()
	{
		GroundEffectField result;
		if (this.m_cachedTrapFieldInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefHiddenTrap.GetTrapFieldInfo()).MethodHandle;
			}
			result = this.m_cachedTrapFieldInfo;
		}
		else
		{
			result = this.m_trapFieldInfo;
		}
		return result;
	}

	public int GetExtraDamagePerTurn()
	{
		return (!this.m_abilityMod) ? this.m_extraDamagePerTurn : this.m_abilityMod.m_extraDamagePerTurnMod.GetModifiedValue(this.m_extraDamagePerTurn);
	}

	public int GetMaxExtraDamage()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefHiddenTrap.GetMaxExtraDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxExtraDamageMod.GetModifiedValue(this.m_maxExtraDamage);
		}
		else
		{
			result = this.m_maxExtraDamage;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		int damageAmount = this.GetTrapFieldInfo().damageAmount;
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, damageAmount);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefHiddenTrap abilityMod_ThiefHiddenTrap = modAsBase as AbilityMod_ThiefHiddenTrap;
		this.m_trapFieldInfo.AddTooltipTokens(tokens, "GroundEffect", false, null);
		string name = "ExtraDamagePerTurn";
		string empty = string.Empty;
		int val;
		if (abilityMod_ThiefHiddenTrap)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefHiddenTrap.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_ThiefHiddenTrap.m_extraDamagePerTurnMod.GetModifiedValue(this.m_extraDamagePerTurn);
		}
		else
		{
			val = this.m_extraDamagePerTurn;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "MaxExtraDamage";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ThiefHiddenTrap)
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
			val2 = abilityMod_ThiefHiddenTrap.m_maxExtraDamageMod.GetModifiedValue(this.m_maxExtraDamage);
		}
		else
		{
			val2 = this.m_maxExtraDamage;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefHiddenTrap))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ThiefHiddenTrap);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
