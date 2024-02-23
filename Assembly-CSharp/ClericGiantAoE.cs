using System.Collections.Generic;
using UnityEngine;

public class ClericGiantAoE : Ability
{
	[Header("-- Targeting")]
	public float m_aoeRadius = 6f;
	public bool m_penetrateLoS;
	public int m_maxTargets = -1;
	[Header("-- On Hit Damage/Heal/Effect")]
	public int m_damageAmount = 30;
	public float m_damageDecreasePerSquare = 2f;
	public int m_healAmount = 30;
	public float m_healDecreasePerSquare = 2f;
	public StandardEffectInfo m_enemyHitEffect;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Giant AoE";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, GetAoeRadius(), PenetrateLoS(), true, true, GetMaxTargets());
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_enemyHitEffect;
		m_cachedAllyHitEffect = m_allyHitEffect;
	}

	public float GetAoeRadius()
	{
		return m_aoeRadius;
	}

	public bool PenetrateLoS()
	{
		return m_penetrateLoS;
	}

	public int GetMaxTargets()
	{
		return m_maxTargets;
	}

	public int GetDamageAmount()
	{
		return m_damageAmount;
	}

	public float GetDamageDecreasePerSquare()
	{
		return m_damageDecreasePerSquare;
	}

	public int GetHealAmount()
	{
		return m_healAmount;
	}

	public float GetHealDecreasePerSquare()
	{
		return m_healDecreasePerSquare;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healAmount)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		int dist = Mathf.RoundToInt((targetActor.GetLoSCheckPos() - ActorData.GetLoSCheckPos()).magnitude / Board.Get().squareSize);
		dist = Mathf.Max(0, dist - 1);
		int damage = GetDamageAmount() - Mathf.RoundToInt(dist * GetDamageDecreasePerSquare());
		int healing = GetHealAmount() - Mathf.RoundToInt(dist * GetHealDecreasePerSquare());
		var ints = new Dictionary<AbilityTooltipSymbol, int>();
		ints[AbilityTooltipSymbol.Healing] = healing;
		ints[AbilityTooltipSymbol.Damage] = damage;
		return ints;
	}
}
