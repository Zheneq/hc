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
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Hidden Trap";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		GroundEffectField trapFieldInfo = GetTrapFieldInfo();
		AbilityUtil_Targeter.AffectsActor affectsCaster = trapFieldInfo.IncludeAllies() ? AbilityUtil_Targeter.AffectsActor.Possible : AbilityUtil_Targeter.AffectsActor.Never;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, trapFieldInfo.shape, trapFieldInfo.penetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, trapFieldInfo.IncludeEnemies(), trapFieldInfo.IncludeAllies(), affectsCaster);
	}

	private void SetCachedFields()
	{
		GroundEffectField cachedTrapFieldInfo;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedTrapFieldInfo = m_abilityMod.m_trapFieldInfoMod.GetModifiedValue(m_trapFieldInfo);
		}
		else
		{
			cachedTrapFieldInfo = m_trapFieldInfo;
		}
		m_cachedTrapFieldInfo = cachedTrapFieldInfo;
	}

	public GroundEffectField GetTrapFieldInfo()
	{
		GroundEffectField result;
		if (m_cachedTrapFieldInfo != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedTrapFieldInfo;
		}
		else
		{
			result = m_trapFieldInfo;
		}
		return result;
	}

	public int GetExtraDamagePerTurn()
	{
		return (!m_abilityMod) ? m_extraDamagePerTurn : m_abilityMod.m_extraDamagePerTurnMod.GetModifiedValue(m_extraDamagePerTurn);
	}

	public int GetMaxExtraDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_maxExtraDamageMod.GetModifiedValue(m_maxExtraDamage);
		}
		else
		{
			result = m_maxExtraDamage;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		int damageAmount = GetTrapFieldInfo().damageAmount;
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, damageAmount);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefHiddenTrap abilityMod_ThiefHiddenTrap = modAsBase as AbilityMod_ThiefHiddenTrap;
		m_trapFieldInfo.AddTooltipTokens(tokens, "GroundEffect");
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ThiefHiddenTrap)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			val = abilityMod_ThiefHiddenTrap.m_extraDamagePerTurnMod.GetModifiedValue(m_extraDamagePerTurn);
		}
		else
		{
			val = m_extraDamagePerTurn;
		}
		AddTokenInt(tokens, "ExtraDamagePerTurn", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ThiefHiddenTrap)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_ThiefHiddenTrap.m_maxExtraDamageMod.GetModifiedValue(m_maxExtraDamage);
		}
		else
		{
			val2 = m_maxExtraDamage;
		}
		AddTokenInt(tokens, "MaxExtraDamage", empty2, val2);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefHiddenTrap))
		{
			m_abilityMod = (abilityMod as AbilityMod_ThiefHiddenTrap);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
