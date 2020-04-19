using System;
using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlRocketJump : Ability
{
	public int m_damageAmount = 0x14;

	public bool m_penetrateLineOfSight;

	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;

	private AbilityMod_BazookaGirlRocketJump m_abilityMod;

	private void Start()
	{
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		StandardEffectInfo moddedEffectForAllies = base.GetModdedEffectForAllies();
		bool flag;
		if (moddedEffectForAllies != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlRocketJump.SetupTargeter()).MethodHandle;
			}
			flag = moddedEffectForAllies.m_applyEffect;
		}
		else
		{
			flag = false;
		}
		bool affectsAllies = flag;
		StandardEffectInfo moddedEffectForSelf = base.GetModdedEffectForSelf();
		bool flag2;
		if (moddedEffectForSelf != null)
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
			flag2 = moddedEffectForSelf.m_applyEffect;
		}
		else
		{
			flag2 = false;
		}
		bool affectsCaster = flag2;
		base.Targeter = new AbilityUtil_Targeter_RocketJump(this, this.m_shape, this.m_penetrateLineOfSight, 0f, affectsAllies);
		base.Targeter.SetAffectedGroups(true, affectsAllies, affectsCaster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_damageAmount));
		base.AppendTooltipNumbersFromBaseModEffects(ref list, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlRocketJump.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
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
				Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				dictionary[AbilityTooltipSymbol.Damage] = this.GetDamageAmount();
				return dictionary;
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlRocketJump abilityMod_BazookaGirlRocketJump = modAsBase as AbilityMod_BazookaGirlRocketJump;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_BazookaGirlRocketJump)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlRocketJump.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_BazookaGirlRocketJump.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlRocketJump))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlRocketJump.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_BazookaGirlRocketJump);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public int GetDamageAmount()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlRocketJump.GetDamageAmount()).MethodHandle;
			}
			result = this.m_damageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		return result;
	}

	public bool ResetCooldownOnKill()
	{
		return !(this.m_abilityMod == null) && this.m_abilityMod.m_resetCooldownOnKill;
	}
}
