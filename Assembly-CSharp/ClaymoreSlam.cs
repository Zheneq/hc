using System.Collections.Generic;
using UnityEngine;

public class ClaymoreSlam : Ability
{
	[Header("-- Laser Targeting")]
	public float m_laserRange = 4f;

	public float m_midLaserWidth = 1f;

	public float m_fullLaserWidth = 2f;

	public int m_laserMaxTargets;

	public bool m_penetrateLos;

	public bool m_laserLengthIgnoreWorldGeo = true;

	[Header("-- Normal Hit Damage/Effects")]
	public int m_middleDamage = 20;

	public StandardEffectInfo m_middleEnemyHitEffect;

	public int m_sideDamage = 10;

	public StandardEffectInfo m_sideEnemyHitEffect;

	[Header("-- Extra Damage on Side")]
	public int m_extraSideDamagePerMiddleHit;

	[Header("-- Extra Damage from Target Health Threshold (0 to 1) --")]
	public int m_extraDamageOnLowHealthTarget;

	public float m_lowHealthThreshold;

	[Header("-- Energy Damage")]
	public int m_energyLossOnMidHit;

	public int m_energyLossOnSideHit;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ClaymoreSlam m_abilityMod;

	private Claymore_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedMiddleEnemyHitEffect;

	private StandardEffectInfo m_cachedSideEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Overhead Slam";
		}
		Setup();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedMiddleEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedMiddleEnemyHitEffect = m_abilityMod.m_middleEnemyHitEffectMod.GetModifiedValue(m_middleEnemyHitEffect);
		}
		else
		{
			cachedMiddleEnemyHitEffect = m_middleEnemyHitEffect;
		}
		m_cachedMiddleEnemyHitEffect = cachedMiddleEnemyHitEffect;
		StandardEffectInfo cachedSideEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedSideEnemyHitEffect = m_abilityMod.m_sideEnemyHitEffectMod.GetModifiedValue(m_sideEnemyHitEffect);
		}
		else
		{
			cachedSideEnemyHitEffect = m_sideEnemyHitEffect;
		}
		m_cachedSideEnemyHitEffect = cachedSideEnemyHitEffect;
	}

	public float GetLaserRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
		}
		else
		{
			result = m_laserRange;
		}
		return result;
	}

	public float GetMidLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_midLaserWidthMod.GetModifiedValue(m_midLaserWidth);
		}
		else
		{
			result = m_midLaserWidth;
		}
		return result;
	}

	public float GetFullLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_fullLaserWidthMod.GetModifiedValue(m_fullLaserWidth);
		}
		else
		{
			result = m_fullLaserWidth;
		}
		return result;
	}

	public int GetLaserMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets);
		}
		else
		{
			result = m_laserMaxTargets;
		}
		return result;
	}

	public bool GetPenetrateLos()
	{
		return (!m_abilityMod) ? m_penetrateLos : m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
	}

	public int GetMiddleDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_middleDamageMod.GetModifiedValue(m_middleDamage);
		}
		else
		{
			result = m_middleDamage;
		}
		return result;
	}

	public StandardEffectInfo GetMiddleEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedMiddleEnemyHitEffect != null)
		{
			result = m_cachedMiddleEnemyHitEffect;
		}
		else
		{
			result = m_middleEnemyHitEffect;
		}
		return result;
	}

	public int GetSideDamage()
	{
		return (!m_abilityMod) ? m_sideDamage : m_abilityMod.m_sideDamageMod.GetModifiedValue(m_sideDamage);
	}

	public StandardEffectInfo GetSideEnemyHitEffect()
	{
		return (m_cachedSideEnemyHitEffect == null) ? m_sideEnemyHitEffect : m_cachedSideEnemyHitEffect;
	}

	public int GetExtraSideDamagePerMiddleHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraSideDamagePerMiddleHitMod.GetModifiedValue(m_extraSideDamagePerMiddleHit);
		}
		else
		{
			result = m_extraSideDamagePerMiddleHit;
		}
		return result;
	}

	public int GetExtraDamageOnLowHealthTarget()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageOnLowHealthTargetMod.GetModifiedValue(m_extraDamageOnLowHealthTarget);
		}
		else
		{
			result = m_extraDamageOnLowHealthTarget;
		}
		return result;
	}

	public float GetLowHealthThreshold()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_lowHealthThresholdMod.GetModifiedValue(m_lowHealthThreshold);
		}
		else
		{
			result = m_lowHealthThreshold;
		}
		return result;
	}

	public int GetEnergyLossOnMidHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyLossOnMidHitMod.GetModifiedValue(m_energyLossOnMidHit);
		}
		else
		{
			result = m_energyLossOnMidHit;
		}
		return result;
	}

	public int GetEnergyLossOnSideHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyLossOnSideHitMod.GetModifiedValue(m_energyLossOnSideHit);
		}
		else
		{
			result = m_energyLossOnSideHit;
		}
		return result;
	}

	public int GetHealPerMidHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healPerMidHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetHealPerSideHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healPerSideHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Claymore_SyncComponent>();
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_ClaymoreKnockbackLaser(this, GetFullLaserWidth(), GetLaserRange(), GetPenetrateLos(), m_laserLengthIgnoreWorldGeo, 0, GetMidLaserWidth(), 0f, KnockbackType.AwayFromSource);
		AbilityUtil_Targeter targeter = base.Targeter;
		int affectsCaster;
		if (GetHealPerMidHit() <= 0)
		{
			affectsCaster = ((GetHealPerSideHit() > 0) ? 1 : 0);
		}
		else
		{
			affectsCaster = 1;
		}
		targeter.SetAffectedGroups(true, false, (byte)affectsCaster != 0);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreSlam abilityMod_ClaymoreSlam = modAsBase as AbilityMod_ClaymoreSlam;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			val = abilityMod_ClaymoreSlam.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets);
		}
		else
		{
			val = m_laserMaxTargets;
		}
		AddTokenInt(tokens, "LaserMaxTargets", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			val2 = abilityMod_ClaymoreSlam.m_middleDamageMod.GetModifiedValue(m_middleDamage);
		}
		else
		{
			val2 = m_middleDamage;
		}
		AddTokenInt(tokens, "MiddleDamage", empty2, val2);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			effectInfo = abilityMod_ClaymoreSlam.m_middleEnemyHitEffectMod.GetModifiedValue(m_middleEnemyHitEffect);
		}
		else
		{
			effectInfo = m_middleEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "MiddleEnemyHitEffect", m_middleEnemyHitEffect);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			val3 = abilityMod_ClaymoreSlam.m_sideDamageMod.GetModifiedValue(m_sideDamage);
		}
		else
		{
			val3 = m_sideDamage;
		}
		AddTokenInt(tokens, "SideDamage", empty3, val3);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			effectInfo2 = abilityMod_ClaymoreSlam.m_sideEnemyHitEffectMod.GetModifiedValue(m_sideEnemyHitEffect);
		}
		else
		{
			effectInfo2 = m_sideEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "SideEnemyHitEffect", m_sideEnemyHitEffect);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			val4 = abilityMod_ClaymoreSlam.m_extraSideDamagePerMiddleHitMod.GetModifiedValue(m_extraSideDamagePerMiddleHit);
		}
		else
		{
			val4 = m_extraSideDamagePerMiddleHit;
		}
		AddTokenInt(tokens, "ExtraSideDamagePerMiddleHit", empty4, val4);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			val5 = abilityMod_ClaymoreSlam.m_extraDamageOnLowHealthTargetMod.GetModifiedValue(m_extraDamageOnLowHealthTarget);
		}
		else
		{
			val5 = m_extraDamageOnLowHealthTarget;
		}
		AddTokenInt(tokens, "ExtraDamageOnLowHealthTarget", empty5, val5);
		string empty6 = string.Empty;
		int val6;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			val6 = abilityMod_ClaymoreSlam.m_energyLossOnMidHitMod.GetModifiedValue(m_energyLossOnMidHit);
		}
		else
		{
			val6 = m_energyLossOnMidHit;
		}
		AddTokenInt(tokens, "EnergyLossOnMidHit", empty6, val6);
		string empty7 = string.Empty;
		int val7;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			val7 = abilityMod_ClaymoreSlam.m_energyLossOnSideHitMod.GetModifiedValue(m_energyLossOnSideHit);
		}
		else
		{
			val7 = m_energyLossOnSideHit;
		}
		AddTokenInt(tokens, "EnergyLossOnSideHit", empty7, val7);
		string empty8 = string.Empty;
		int val8;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			val8 = abilityMod_ClaymoreSlam.m_healPerMidHit.GetModifiedValue(0);
		}
		else
		{
			val8 = 0;
		}
		AddTokenInt(tokens, "HealPerMidHit", empty8, val8);
		string empty9 = string.Empty;
		int val9;
		if ((bool)abilityMod_ClaymoreSlam)
		{
			val9 = abilityMod_ClaymoreSlam.m_healPerSideHit.GetModifiedValue(0);
		}
		else
		{
			val9 = 0;
		}
		AddTokenInt(tokens, "HealPerSideHit", empty9, val9);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetMiddleDamage());
		m_middleEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Primary, -1 * GetEnergyLossOnMidHit());
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetSideDamage());
		m_sideEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Secondary, -1 * GetEnergyLossOnSideHit());
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, 0));
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Primary);
			int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Secondary);
			int value = 0;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				if (targetActor.GetHitPointPercent() < GetLowHealthThreshold())
				{
					value = GetExtraDamageOnLowHealthTarget();
				}
			}
			dictionary[AbilityTooltipSymbol.Damage] = value;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Damage] += GetMiddleDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				int num = GetSideDamage();
				if (GetExtraSideDamagePerMiddleHit() > 0)
				{
					num += visibleActorsCountByTooltipSubject * GetExtraSideDamagePerMiddleHit();
				}
				dictionary[AbilityTooltipSymbol.Damage] += num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				int num2 = GetHealPerMidHit() * visibleActorsCountByTooltipSubject + GetHealPerSideHit() * visibleActorsCountByTooltipSubject2;
				if (num2 > 0)
				{
					dictionary[AbilityTooltipSymbol.Healing] = num2;
				}
			}
		}
		return dictionary;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return (!(m_syncComp != null)) ? null : m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreSlam))
		{
			m_abilityMod = (abilityMod as AbilityMod_ClaymoreSlam);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
