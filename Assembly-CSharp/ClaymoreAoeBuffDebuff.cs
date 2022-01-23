using System.Collections.Generic;
using UnityEngine;

public class ClaymoreAoeBuffDebuff : Ability
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
	public StandardEffectInfo m_selfHitEffect;

	public StandardEffectInfo m_allyHitEffect;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Energy Gain/Loss for hit actors")]
	public int m_allyEnergyGain;

	public int m_enemyEnergyLoss;

	public bool m_energyChangeOnlyIfHasAdjacent;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ClaymoreAoeBuffDebuff m_abilityMod;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Thundering Roar";
		}
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
		StandardEffectInfo cachedAllyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedAllyHitEffect = m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = m_allyHitEffect;
		}
		m_cachedAllyHitEffect = cachedAllyHitEffect;
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

	public AbilityAreaShape GetShape()
	{
		return (!m_abilityMod) ? m_shape : m_abilityMod.m_shapeMod.GetModifiedValue(m_shape);
	}

	public bool GetPenetrateLos()
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
		return (!m_abilityMod) ? m_baseSelfHeal : m_abilityMod.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal);
	}

	public int GetSelfHealAmountPerHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealAmountPerHitMod.GetModifiedValue(m_selfHealAmountPerHit);
		}
		else
		{
			result = m_selfHealAmountPerHit;
		}
		return result;
	}

	public bool GetSelfHealCountEnemyHit()
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

	public bool GetSelfHealCountAllyHit()
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

	public StandardEffectInfo GetSelfHitEffect()
	{
		return (m_cachedSelfHitEffect == null) ? m_selfHitEffect : m_cachedSelfHitEffect;
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
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public int GetAllyEnergyGain()
	{
		return (!m_abilityMod) ? m_allyEnergyGain : m_abilityMod.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain);
	}

	public int GetEnemyEnergyLoss()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_enemyEnergyLossMod.GetModifiedValue(m_enemyEnergyLoss);
		}
		else
		{
			result = m_enemyEnergyLoss;
		}
		return result;
	}

	public bool GetEnergyChangeOnlyIfHasAdjacent()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyChangeOnlyIfHasAdjacentMod.GetModifiedValue(m_energyChangeOnlyIfHasAdjacent);
		}
		else
		{
			result = m_energyChangeOnlyIfHasAdjacent;
		}
		return result;
	}

	public bool IncludeCaster()
	{
		int result;
		if (!GetSelfHitEffect().m_applyEffect && GetBaseSelfHeal() <= 0)
		{
			result = ((GetSelfHealAmountPerHit() > 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool IncludeAllies()
	{
		int result;
		if (!GetAllyHitEffect().m_applyEffect)
		{
			result = ((GetAllyEnergyGain() > 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool IncludeEnemies()
	{
		int result;
		if (!GetEnemyHitEffect().m_applyEffect)
		{
			result = ((GetEnemyEnergyLoss() > 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		if (!IncludeCaster())
		{
			affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, GetShape(), GetPenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, IncludeEnemies(), IncludeAllies(), affectsCaster);
		base.Targeter.ShowArcToShape = false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreAoeBuffDebuff abilityMod_ClaymoreAoeBuffDebuff = modAsBase as AbilityMod_ClaymoreAoeBuffDebuff;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ClaymoreAoeBuffDebuff)
		{
			val = abilityMod_ClaymoreAoeBuffDebuff.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal);
		}
		else
		{
			val = m_baseSelfHeal;
		}
		AddTokenInt(tokens, "BaseSelfHeal", empty, val);
		AddTokenInt(tokens, "SelfHealAmountPerHit", string.Empty, (!abilityMod_ClaymoreAoeBuffDebuff) ? m_selfHealAmountPerHit : abilityMod_ClaymoreAoeBuffDebuff.m_selfHealAmountPerHitMod.GetModifiedValue(m_selfHealAmountPerHit));
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_ClaymoreAoeBuffDebuff) ? m_selfHitEffect : abilityMod_ClaymoreAoeBuffDebuff.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect), "SelfHitEffect", m_selfHitEffect);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_ClaymoreAoeBuffDebuff)
		{
			effectInfo = abilityMod_ClaymoreAoeBuffDebuff.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_ClaymoreAoeBuffDebuff) ? m_enemyHitEffect : abilityMod_ClaymoreAoeBuffDebuff.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect), "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "AllyEnergyGain", string.Empty, (!abilityMod_ClaymoreAoeBuffDebuff) ? m_allyEnergyGain : abilityMod_ClaymoreAoeBuffDebuff.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain));
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ClaymoreAoeBuffDebuff)
		{
			val2 = abilityMod_ClaymoreAoeBuffDebuff.m_enemyEnergyLossMod.GetModifiedValue(m_enemyEnergyLoss);
		}
		else
		{
			val2 = m_enemyEnergyLoss;
		}
		AddTokenInt(tokens, "EnemyEnergyLoss", empty2, val2);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetBaseSelfHeal() + GetSelfHealAmountPerHit());
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Ally, GetAllyEnergyGain());
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Enemy, -1 * GetEnemyEnergyLoss());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (GetSelfHealAmountPerHit() <= 0)
		{
			if (GetBaseSelfHeal() <= 0)
			{
				return null;
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
			else if (GetEnergyChangeOnlyIfHasAdjacent())
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
				{
					if (GetEnemyEnergyLoss() > 0)
					{
						int value;
						if (AreaEffectUtils.HasAdjacentActorOfTeam(targetActor, targetActor.GetTeamAsList()))
						{
							value = -1 * GetEnemyEnergyLoss();
						}
						else
						{
							value = 0;
						}
						dictionary[AbilityTooltipSymbol.Energy] = value;
						goto IL_01ba;
					}
				}
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
				{
					if (GetAllyEnergyGain() > 0)
					{
						dictionary[AbilityTooltipSymbol.Energy] = (AreaEffectUtils.HasAdjacentActorOfTeam(targetActor, targetActor.GetTeamAsList()) ? GetAllyEnergyGain() : 0);
					}
				}
			}
		}
		goto IL_01ba;
		IL_01ba:
		return dictionary;
	}

	private int CalcSelfHealAmountFromHits(int allyHits, int enemyHits)
	{
		int result = 0;
		if (GetSelfHealAmountPerHit() <= 0)
		{
			if (GetBaseSelfHeal() <= 0)
			{
				goto IL_0077;
			}
		}
		int num = 0;
		if (GetSelfHealCountAllyHit())
		{
			num += allyHits;
		}
		if (GetSelfHealCountEnemyHit())
		{
			num += enemyHits;
		}
		result = GetBaseSelfHeal() + num * GetSelfHealAmountPerHit();
		goto IL_0077;
		IL_0077:
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ClaymoreAoeBuffDebuff))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ClaymoreAoeBuffDebuff);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
