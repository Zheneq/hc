using System.Collections.Generic;
using UnityEngine;

public class RampartAoeBuffDebuff : Ability
{
	[Header("-- Targeting")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;
	public bool m_penetrateLos;
	[Header("-- Self Heal Per Hit")]
	public int m_baseSelfHeal;
	public int m_selfHealAmountPerHit;
	public bool m_selfHealCountEnemyHit = true;
	public bool m_selfHealCountAllyHit = true;
	[Header("-- Normal Hit Effects")]
	public bool m_includeCaster = true;
	public StandardEffectInfo m_selfHitEffect;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_enemyHitEffect;
	public int m_damageToEnemies;
	public int m_healingToAllies;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_RampartAoeBuffDebuff m_abilityMod;
	private StandardEffectInfo m_cachedSelfHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Robotic Roar";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		if (!IncludeCaster())
		{
			affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		}
		Targeter = new AbilityUtil_Targeter_Shape(this, GetShape(), PenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, IncludeEnemies(), IncludeAllies(), affectsCaster)
		{
			ShowArcToShape = false
		};
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartAoeBuffDebuff))
		{
			m_abilityMod = (abilityMod as AbilityMod_RampartAoeBuffDebuff);
		}
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		m_cachedSelfHitEffect = m_abilityMod != null
			? m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartAoeBuffDebuff abilityMod_RampartAoeBuffDebuff = modAsBase as AbilityMod_RampartAoeBuffDebuff;
		AddTokenInt(tokens, "BaseSelfHeal", "", abilityMod_RampartAoeBuffDebuff != null
			? abilityMod_RampartAoeBuffDebuff.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal)
			: m_baseSelfHeal);
		AddTokenInt(tokens, "SelfHealAmountPerHit", "", abilityMod_RampartAoeBuffDebuff != null
			? abilityMod_RampartAoeBuffDebuff.m_selfHealAmountPerHitMod.GetModifiedValue(m_selfHealAmountPerHit)
			: m_selfHealAmountPerHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RampartAoeBuffDebuff != null
			? abilityMod_RampartAoeBuffDebuff.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect, "SelfHitEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RampartAoeBuffDebuff != null
			? abilityMod_RampartAoeBuffDebuff.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RampartAoeBuffDebuff != null
			? abilityMod_RampartAoeBuffDebuff.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "DamageToEnemies", "", abilityMod_RampartAoeBuffDebuff != null
			? abilityMod_RampartAoeBuffDebuff.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies)
			: m_damageToEnemies);
		AddTokenInt(tokens, "HealingToAllies", "", abilityMod_RampartAoeBuffDebuff != null
			? abilityMod_RampartAoeBuffDebuff.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies)
			: m_healingToAllies);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetBaseSelfHeal() + GetSelfHealAmountPerHit());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealingToAllies());
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageToEnemies());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (GetSelfHealAmountPerHit() <= 0 && GetBaseSelfHeal() <= 0)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
			int enemyHits = 0;
			int allyHits = 0;
			foreach (ActorData subject in visibleActorsInRangeByTooltipSubject)
			{
				if (subject.GetTeam() != targetActor.GetTeam())
				{
					enemyHits++;
				}
				else if (subject != targetActor)
				{
					allyHits++;
				}
			}
			dictionary[AbilityTooltipSymbol.Healing] = CalcSelfHealAmountFromHits(allyHits, enemyHits);
		}
		return dictionary;
	}

	private int CalcSelfHealAmountFromHits(int allyHits, int enemyHits)
	{
		if (GetSelfHealAmountPerHit() <= 0 && GetBaseSelfHeal() <= 0)
		{
			return 0;
		}
		int selfHeal = 0;
		if (SelfHealCountAllyHit())
		{
			selfHeal += allyHits;
		}
		if (SelfHealCountEnemyHit())
		{
			selfHeal += enemyHits;
		}
		return GetBaseSelfHeal() + selfHeal * GetSelfHealAmountPerHit();
	}

	public bool IncludeCaster()
	{
		return ModdedIncludeCaster() || GetSelfHealAmountPerHit() > 0 || GetBaseSelfHeal() > 0;
	}

	public bool IncludeAllies()
	{
		return GetAllyHitEffect().m_applyEffect || GetHealingToAllies() > 0;
	}

	public bool IncludeEnemies()
	{
		return GetEnemyHitEffect().m_applyEffect || GetDamageToEnemies() > 0;
	}

	public AbilityAreaShape GetShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shapeMod.GetModifiedValue(m_shape)
			: m_shape;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetBaseSelfHeal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal)
			: m_baseSelfHeal;
	}

	public int GetSelfHealAmountPerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealAmountPerHitMod.GetModifiedValue(m_selfHealAmountPerHit)
			: m_selfHealAmountPerHit;
	}

	public bool SelfHealCountEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealCountEnemyHitMod.GetModifiedValue(m_selfHealCountEnemyHit)
			: m_selfHealCountEnemyHit;
	}

	public bool SelfHealCountAllyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealCountAllyHitMod.GetModifiedValue(m_selfHealCountAllyHit)
			: m_selfHealCountAllyHit;
	}

	public bool ModdedIncludeCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_includeCasterMod.GetModifiedValue(m_includeCaster)
			: m_includeCaster;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		return m_cachedSelfHitEffect ?? m_selfHitEffect;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetDamageToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies)
			: m_damageToEnemies;
	}

	public int GetHealingToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies)
			: m_healingToAllies;
	}
}
