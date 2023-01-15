using System.Collections.Generic;
using UnityEngine;

public class RageBeastCharge : Ability
{
	[Space(10f)]
	public int m_damageAmount = 20;
	public int m_damageNearChargeEnd;
	public float m_damageRadius = 5f;
	public float m_radiusBehindStart;
	public float m_radiusBeyondEnd;
	public bool m_penetrateLineOfSight;
	public StandardEffectInfo m_enemyHitEffectNearChargeEnd;

	private AbilityMod_RageBeastCharge m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffectNearChargeEnd;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Pain Train";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_ChargeAoE(
			this,
			m_radiusBehindStart,
			ModdedChargeEndRadius(),
			ModdedChargeLineRadius(),
			-1,
			false,
			m_penetrateLineOfSight);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, ModdedDamage())
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null
		    && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far)
		    && ModdedDamageNearChargeEnd() > 0)
		{
			dictionary[AbilityTooltipSymbol.Damage] = ModdedDamageNearChargeEnd();
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastCharge abilityMod_RageBeastCharge = modAsBase as AbilityMod_RageBeastCharge;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_RageBeastCharge != null
			? abilityMod_RageBeastCharge.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "DamageNearChargeEnd", string.Empty, abilityMod_RageBeastCharge != null
			? abilityMod_RageBeastCharge.m_damageNearChargeEndMod.GetModifiedValue(m_damageNearChargeEnd)
			: m_damageNearChargeEnd);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffectNearChargeEnd, "EnemyHitEffectNearChargeEnd", m_enemyHitEffectNearChargeEnd);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
		       && targetSquare.IsValidForGameplay()
		       && targetSquare != caster.GetCurrentBoardSquare()
		       && KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare) != null;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastCharge))
		{
			m_abilityMod = abilityMod as AbilityMod_RageBeastCharge;
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

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffectNearChargeEnd = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectNearChargeEndMod.GetModifiedValue(m_enemyHitEffectNearChargeEnd)
			: m_enemyHitEffectNearChargeEnd;
	}

	public int ModdedDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int ModdedDamageNearChargeEnd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageNearChargeEndMod.GetModifiedValue(m_damageNearChargeEnd)
			: m_damageNearChargeEnd;
	}

	public float ModdedChargeLineRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargeLineRadiusMod.GetModifiedValue(m_damageRadius)
			: m_damageRadius;
	}

	public float ModdedChargeEndRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargeEndRadius.GetModifiedValue(m_radiusBeyondEnd)
			: m_radiusBeyondEnd;
	}

	public StandardEffectInfo GetEnemyHitEffectNearChargeEnd()
	{
		return m_cachedEnemyHitEffectNearChargeEnd ?? m_enemyHitEffectNearChargeEnd;
	}
}
