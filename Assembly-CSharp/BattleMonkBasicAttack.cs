using System.Collections.Generic;
using UnityEngine;

public class BattleMonkBasicAttack : Ability
{
	[Space(10f)]
	public float m_coneWidthAngle = 270f;

	public float m_coneLength = 1.5f;

	public float m_coneBackwardOffset;

	public int m_damageAmount = 20;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets = 2;

	public int m_healAmountPerTargetHit;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_BattleMonkBasicAttack m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (4)
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
			m_abilityName = "Siphon Slash";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, ModdedConeAngle(), ModdedConeLength(), m_coneBackwardOffset, m_penetrateLineOfSight, true, true, false, ModdedHealPerTargetHit() > 0);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return ModdedConeLength();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkBasicAttack abilityMod_BattleMonkBasicAttack = modAsBase as AbilityMod_BattleMonkBasicAttack;
		int val = (!abilityMod_BattleMonkBasicAttack) ? m_damageAmount : abilityMod_BattleMonkBasicAttack.m_coneDamageMod.GetModifiedValue(m_damageAmount);
		tokens.Add(new TooltipTokenInt("Damage", "damage to enemies", val));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, ModdedConeDamage(1));
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, ModdedHealPerTargetHit());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				while (true)
				{
					switch (4)
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
				int num = ModdedHealPerTargetHit() * visibleActorsCountByTooltipSubject;
				dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt(num);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				dictionary[AbilityTooltipSymbol.Damage] = ModdedConeDamage(visibleActorsCountByTooltipSubject);
			}
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkBasicAttack))
		{
			m_abilityMod = (abilityMod as AbilityMod_BattleMonkBasicAttack);
			SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public float ModdedConeAngle()
	{
		float result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (6)
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
			result = m_coneWidthAngle;
		}
		else
		{
			result = m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		return result;
	}

	public float ModdedConeLength()
	{
		float result;
		if (m_abilityMod == null)
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
			result = m_coneLength;
		}
		else
		{
			result = m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength);
		}
		return result;
	}

	public int ModdedConeDamage(int numTargets)
	{
		int num = m_damageAmount;
		if (m_abilityMod != null)
		{
			num = m_abilityMod.m_coneDamageMod.GetModifiedValue(num);
			num += m_abilityMod.m_extraDamagePerTarget.GetModifiedValue(0) * (numTargets - 1);
		}
		return num;
	}

	public int ModdedHealPerTargetHit()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_healAmountPerTargetHit;
		}
		else
		{
			result = m_abilityMod.m_healPerTargetHitMod.GetModifiedValue(m_healAmountPerTargetHit);
		}
		return result;
	}
}
