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
		base.Targeter = new AbilityUtil_Targeter_Shape(this, GetShape(), PenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, IncludeEnemies(), IncludeAllies(), affectsCaster);
		base.Targeter.ShowArcToShape = false;
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
		StandardEffectInfo cachedSelfHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedSelfHitEffect = m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect);
		}
		else
		{
			cachedSelfHitEffect = m_selfHitEffect;
		}
		m_cachedSelfHitEffect = cachedSelfHitEffect;
		m_cachedAllyHitEffect = ((!m_abilityMod) ? m_allyHitEffect : m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect));
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartAoeBuffDebuff abilityMod_RampartAoeBuffDebuff = modAsBase as AbilityMod_RampartAoeBuffDebuff;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RampartAoeBuffDebuff)
		{
			val = abilityMod_RampartAoeBuffDebuff.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal);
		}
		else
		{
			val = m_baseSelfHeal;
		}
		AddTokenInt(tokens, "BaseSelfHeal", empty, val);
		AddTokenInt(tokens, "SelfHealAmountPerHit", string.Empty, (!abilityMod_RampartAoeBuffDebuff) ? m_selfHealAmountPerHit : abilityMod_RampartAoeBuffDebuff.m_selfHealAmountPerHitMod.GetModifiedValue(m_selfHealAmountPerHit));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_RampartAoeBuffDebuff)
		{
			effectInfo = abilityMod_RampartAoeBuffDebuff.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect);
		}
		else
		{
			effectInfo = m_selfHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SelfHitEffect", m_selfHitEffect);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_RampartAoeBuffDebuff)
		{
			effectInfo2 = abilityMod_RampartAoeBuffDebuff.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo2 = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", m_allyHitEffect);
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_RampartAoeBuffDebuff)
		{
			effectInfo3 = abilityMod_RampartAoeBuffDebuff.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo3 = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "EnemyHitEffect", m_enemyHitEffect);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_RampartAoeBuffDebuff)
		{
			val2 = abilityMod_RampartAoeBuffDebuff.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies);
		}
		else
		{
			val2 = m_damageToEnemies;
		}
		AddTokenInt(tokens, "DamageToEnemies", empty2, val2);
		AddTokenInt(tokens, "HealingToAllies", string.Empty, (!abilityMod_RampartAoeBuffDebuff) ? m_healingToAllies : abilityMod_RampartAoeBuffDebuff.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies));
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					if (visibleActorsInRangeByTooltipSubject[i].GetTeam() != targetActor.GetTeam())
					{
						num++;
					}
					else if (visibleActorsInRangeByTooltipSubject[i] != targetActor)
					{
						num2++;
					}
				}
				int num4 = dictionary[AbilityTooltipSymbol.Healing] = CalcSelfHealAmountFromHits(num2, num);
			}
		}
		return dictionary;
	}

	private int CalcSelfHealAmountFromHits(int allyHits, int enemyHits)
	{
		int result = 0;
		if (GetSelfHealAmountPerHit() <= 0)
		{
			if (GetBaseSelfHeal() <= 0)
			{
				goto IL_0075;
			}
		}
		int num = 0;
		if (SelfHealCountAllyHit())
		{
			num += allyHits;
		}
		if (SelfHealCountEnemyHit())
		{
			num += enemyHits;
		}
		result = GetBaseSelfHeal() + num * GetSelfHealAmountPerHit();
		goto IL_0075;
		IL_0075:
		return result;
	}

	public bool IncludeCaster()
	{
		int result;
		if (!ModdedIncludeCaster())
		{
			if (GetSelfHealAmountPerHit() <= 0)
			{
				result = ((GetBaseSelfHeal() > 0) ? 1 : 0);
				goto IL_003e;
			}
		}
		result = 1;
		goto IL_003e;
		IL_003e:
		return (byte)result != 0;
	}

	public bool IncludeAllies()
	{
		int result;
		if (!GetAllyHitEffect().m_applyEffect)
		{
			result = ((GetHealingToAllies() > 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool IncludeEnemies()
	{
		return GetEnemyHitEffect().m_applyEffect || GetDamageToEnemies() > 0;
	}

	public AbilityAreaShape GetShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_shapeMod.GetModifiedValue(m_shape);
		}
		else
		{
			result = m_shape;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
		}
		else
		{
			result = m_penetrateLos;
		}
		return result;
	}

	public int GetBaseSelfHeal()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal);
		}
		else
		{
			result = m_baseSelfHeal;
		}
		return result;
	}

	public int GetSelfHealAmountPerHit()
	{
		return (!m_abilityMod) ? m_selfHealAmountPerHit : m_abilityMod.m_selfHealAmountPerHitMod.GetModifiedValue(m_selfHealAmountPerHit);
	}

	public bool SelfHealCountEnemyHit()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealCountEnemyHitMod.GetModifiedValue(m_selfHealCountEnemyHit);
		}
		else
		{
			result = m_selfHealCountEnemyHit;
		}
		return result;
	}

	public bool SelfHealCountAllyHit()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealCountAllyHitMod.GetModifiedValue(m_selfHealCountAllyHit);
		}
		else
		{
			result = m_selfHealCountAllyHit;
		}
		return result;
	}

	public bool ModdedIncludeCaster()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_includeCasterMod.GetModifiedValue(m_includeCaster);
		}
		else
		{
			result = m_includeCaster;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSelfHitEffect != null)
		{
			result = m_cachedSelfHitEffect;
		}
		else
		{
			result = m_selfHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyHitEffect != null)
		{
			result = m_cachedAllyHitEffect;
		}
		else
		{
			result = m_allyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return (m_cachedEnemyHitEffect == null) ? m_enemyHitEffect : m_cachedEnemyHitEffect;
	}

	public int GetDamageToEnemies()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies);
		}
		else
		{
			result = m_damageToEnemies;
		}
		return result;
	}

	public int GetHealingToAllies()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies);
		}
		else
		{
			result = m_healingToAllies;
		}
		return result;
	}
}
