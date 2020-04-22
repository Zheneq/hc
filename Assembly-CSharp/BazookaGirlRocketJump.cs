using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlRocketJump : Ability
{
	public int m_damageAmount = 20;

	public bool m_penetrateLineOfSight;

	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;

	private AbilityMod_BazookaGirlRocketJump m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		StandardEffectInfo moddedEffectForAllies = GetModdedEffectForAllies();
		int num;
		if (moddedEffectForAllies != null)
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
			num = (moddedEffectForAllies.m_applyEffect ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool affectsAllies = (byte)num != 0;
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		int num2;
		if (moddedEffectForSelf != null)
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
			num2 = (moddedEffectForSelf.m_applyEffect ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool affectsCaster = (byte)num2 != 0;
		base.Targeter = new AbilityUtil_Targeter_RocketJump(this, m_shape, m_penetrateLineOfSight, 0f, affectsAllies);
		base.Targeter.SetAffectedGroups(true, affectsAllies, affectsCaster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount));
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmount();
						return dictionary;
					}
					}
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlRocketJump abilityMod_BazookaGirlRocketJump = modAsBase as AbilityMod_BazookaGirlRocketJump;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BazookaGirlRocketJump)
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
			val = abilityMod_BazookaGirlRocketJump.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlRocketJump))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_BazookaGirlRocketJump);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int GetDamageAmount()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_damageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		return result;
	}

	public bool ResetCooldownOnKill()
	{
		return !(m_abilityMod == null) && m_abilityMod.m_resetCooldownOnKill;
	}
}
