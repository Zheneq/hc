using System.Collections.Generic;
using UnityEngine;

public class GrydKnockbackTrap : Ability
{
	[Header("-- Trap Ground Field")]
	public GroundEffectField m_trapFieldInfo;

	[Header("-- Extra Damage")]
	public int m_extraDamagePerTurn;

	public int m_maxExtraDamage;

	public int m_knockbackAmount = 2;

	public bool m_lockToCardinalDirs = true;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private GroundEffectField m_cachedTrapFieldInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Knockback Trap";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		GroundEffectField trapFieldInfo = GetTrapFieldInfo();
		int num;
		if (trapFieldInfo.IncludeAllies())
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
			num = 1;
		}
		else
		{
			num = 0;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = (AbilityUtil_Targeter.AffectsActor)num;
		base.Targeters.Clear();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_KnockbackAoE abilityUtil_Targeter_KnockbackAoE = new AbilityUtil_Targeter_KnockbackAoE(this, trapFieldInfo.shape, trapFieldInfo.penetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, trapFieldInfo.IncludeEnemies(), trapFieldInfo.IncludeAllies(), affectsCaster, AbilityUtil_Targeter.AffectsActor.Never, m_knockbackAmount, KnockbackType.ForwardAlongAimDir);
			abilityUtil_Targeter_KnockbackAoE.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_KnockbackAoE.m_lockToCardinalDirs = m_lockToCardinalDirs;
			abilityUtil_Targeter_KnockbackAoE.m_showArrowHighlight = true;
			base.Targeters.Add(abilityUtil_Targeter_KnockbackAoE);
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	private void SetCachedFields()
	{
		m_cachedTrapFieldInfo = m_trapFieldInfo;
	}

	public GroundEffectField GetTrapFieldInfo()
	{
		GroundEffectField result;
		if (m_cachedTrapFieldInfo != null)
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
		return m_extraDamagePerTurn;
	}

	public int GetMaxExtraDamage()
	{
		return m_maxExtraDamage;
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
		AddTokenInt(tokens, "ExtraDamagePerTurn", string.Empty, (!abilityMod_ThiefHiddenTrap) ? m_extraDamagePerTurn : abilityMod_ThiefHiddenTrap.m_extraDamagePerTurnMod.GetModifiedValue(m_extraDamagePerTurn));
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ThiefHiddenTrap)
		{
			while (true)
			{
				switch (5)
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
			val = abilityMod_ThiefHiddenTrap.m_maxExtraDamageMod.GetModifiedValue(m_maxExtraDamage);
		}
		else
		{
			val = m_maxExtraDamage;
		}
		AddTokenInt(tokens, "MaxExtraDamage", empty, val);
	}
}
