using System;
using System.Collections.Generic;
using UnityEngine;

public class ClericGiantAoE : Ability
{
	[Header("-- Targeting")]
	public float m_aoeRadius = 6f;

	public bool m_penetrateLoS;

	public int m_maxTargets = -1;

	[Header("-- On Hit Damage/Heal/Effect")]
	public int m_damageAmount = 0x1E;

	public float m_damageDecreasePerSquare = 2f;

	public int m_healAmount = 0x1E;

	public float m_healDecreasePerSquare = 2f;

	public StandardEffectInfo m_enemyHitEffect;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Giant AoE";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, this.GetAoeRadius(), this.PenetrateLoS(), true, true, this.GetMaxTargets());
	}

	private void SetCachedFields()
	{
		this.m_cachedEnemyHitEffect = this.m_enemyHitEffect;
		this.m_cachedAllyHitEffect = this.m_allyHitEffect;
	}

	public float GetAoeRadius()
	{
		return this.m_aoeRadius;
	}

	public bool PenetrateLoS()
	{
		return this.m_penetrateLoS;
	}

	public int GetMaxTargets()
	{
		return this.m_maxTargets;
	}

	public int GetDamageAmount()
	{
		return this.m_damageAmount;
	}

	public float GetDamageDecreasePerSquare()
	{
		return this.m_damageDecreasePerSquare;
	}

	public int GetHealAmount()
	{
		return this.m_healAmount;
	}

	public float GetHealDecreasePerSquare()
	{
		return this.m_healDecreasePerSquare;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
		{
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
		{
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "HealAmount", string.Empty, this.m_healAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyHitEffect", this.m_allyHitEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damageAmount),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, this.m_healAmount)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = Mathf.RoundToInt((targetActor.GetTravelBoardSquareWorldPositionForLos() - base.ActorData.GetTravelBoardSquareWorldPositionForLos()).magnitude / Board.Get().squareSize);
		num = Mathf.Max(0, num - 1);
		int value = this.GetDamageAmount() - Mathf.RoundToInt((float)num * this.GetDamageDecreasePerSquare());
		int value2 = this.GetHealAmount() - Mathf.RoundToInt((float)num * this.GetHealDecreasePerSquare());
		dictionary[AbilityTooltipSymbol.Damage] = value;
		dictionary[AbilityTooltipSymbol.Healing] = value2;
		return dictionary;
	}
}
