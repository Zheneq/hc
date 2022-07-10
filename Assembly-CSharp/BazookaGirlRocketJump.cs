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
		bool affectsAllies = moddedEffectForAllies != null && moddedEffectForAllies.m_applyEffect;
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		bool affectsCaster = moddedEffectForSelf != null && moddedEffectForSelf.m_applyEffect;
		Targeter = new AbilityUtil_Targeter_RocketJump(this, m_shape, m_penetrateLineOfSight, 0f, affectsAllies);
		Targeter.SetAffectedGroups(true, affectsAllies, affectsCaster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount)
		};
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null
		    || !tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			return null;
		}
		return new Dictionary<AbilityTooltipSymbol, int>
		{
			[AbilityTooltipSymbol.Damage] = GetDamageAmount()
		};
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlRocketJump mod = modAsBase as AbilityMod_BazookaGirlRocketJump;
		int damage = mod != null
			? mod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
		AddTokenInt(tokens, "DamageAmount", string.Empty, damage);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BazookaGirlRocketJump))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BazookaGirlRocketJump;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public bool ResetCooldownOnKill()
	{
		return m_abilityMod != null && m_abilityMod.m_resetCooldownOnKill;
	}
}
