using System.Collections.Generic;
using UnityEngine;

public class RampartMeleeBasicAttack : Ability
{
	[Header("-- Laser Targeting")]
	public float m_laserRange = 4f;

	public float m_laserWidth = 1f;

	public int m_laserMaxTargets;

	public bool m_penetrateLos;

	[Header("-- Cone Targeting")]
	public float m_coneWidthAngle = 90f;

	public float m_coneRange = 2.5f;

	[Header("-- Hit Damage/Effects")]
	public int m_laserDamage = 20;

	public StandardEffectInfo m_laserEnemyHitEffect;

	public int m_coneDamage = 10;

	public StandardEffectInfo m_coneEnemyHitEffect;

	public int m_bonusDamageForOverlap;

	[Header("-- Sequences")]
	public GameObject m_laserSequencePrefab;

	public GameObject m_coneSequencePrefab;

	private AbilityMod_RampartMeleeBasicAttack m_abilityMod;

	private StandardEffectInfo m_cachedLaserEnemyHitEffect;

	private StandardEffectInfo m_cachedConeEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Overhead Slam";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_ClaymoreSlam(this, GetLaserRange(), GetLaserWidth(), GetLaserMaxTargets(), GetConeAngle(), GetConeRange(), 0f, PenetrateLos(), true, false, false, GetBonusDamageForOverlap() > 0);
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
		m_cachedLaserEnemyHitEffect = ((!m_abilityMod) ? m_laserEnemyHitEffect : m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect));
		StandardEffectInfo cachedConeEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedConeEnemyHitEffect = m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(m_coneEnemyHitEffect);
		}
		else
		{
			cachedConeEnemyHitEffect = m_coneEnemyHitEffect;
		}
		m_cachedConeEnemyHitEffect = cachedConeEnemyHitEffect;
	}

	public float GetLaserRange()
	{
		return (!m_abilityMod) ? m_laserRange : m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
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

	public float GetConeAngle()
	{
		return (!m_abilityMod) ? m_coneWidthAngle : m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
	}

	public float GetConeRange()
	{
		return (!m_abilityMod) ? m_coneRange : m_abilityMod.m_coneRangeMod.GetModifiedValue(m_coneRange);
	}

	public int GetLaserDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamage);
		}
		else
		{
			result = m_laserDamage;
		}
		return result;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		return (m_cachedLaserEnemyHitEffect == null) ? m_laserEnemyHitEffect : m_cachedLaserEnemyHitEffect;
	}

	public int GetConeDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneDamageMod.GetModifiedValue(m_coneDamage);
		}
		else
		{
			result = m_coneDamage;
		}
		return result;
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		return (m_cachedConeEnemyHitEffect == null) ? m_coneEnemyHitEffect : m_cachedConeEnemyHitEffect;
	}

	public int GetBonusDamageForOverlap()
	{
		return (!m_abilityMod) ? m_bonusDamageForOverlap : m_abilityMod.m_bonusDamageForOverlapMod.GetModifiedValue(m_bonusDamageForOverlap);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartMeleeBasicAttack))
		{
			m_abilityMod = (abilityMod as AbilityMod_RampartMeleeBasicAttack);
		}
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetLaserDamage());
		m_laserEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			bool flag = tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary);
			bool flag2 = tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary);
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (flag)
			{
				if (flag2)
				{
					dictionary[AbilityTooltipSymbol.Damage] = GetLaserDamage() + GetBonusDamageForOverlap();
					goto IL_00ae;
				}
			}
			if (flag)
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetLaserDamage();
			}
			else if (flag2)
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetConeDamage();
			}
		}
		goto IL_00ae;
		IL_00ae:
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartMeleeBasicAttack abilityMod_RampartMeleeBasicAttack = modAsBase as AbilityMod_RampartMeleeBasicAttack;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RampartMeleeBasicAttack)
		{
			val = abilityMod_RampartMeleeBasicAttack.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets);
		}
		else
		{
			val = m_laserMaxTargets;
		}
		AddTokenInt(tokens, "LaserMaxTargets", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_RampartMeleeBasicAttack)
		{
			val2 = abilityMod_RampartMeleeBasicAttack.m_laserDamageMod.GetModifiedValue(m_laserDamage);
		}
		else
		{
			val2 = m_laserDamage;
		}
		AddTokenInt(tokens, "LaserDamage", empty2, val2);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_RampartMeleeBasicAttack)
		{
			effectInfo = abilityMod_RampartMeleeBasicAttack.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect);
		}
		else
		{
			effectInfo = m_laserEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "LaserEnemyHitEffect", m_laserEnemyHitEffect);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_RampartMeleeBasicAttack)
		{
			val3 = abilityMod_RampartMeleeBasicAttack.m_coneDamageMod.GetModifiedValue(m_coneDamage);
		}
		else
		{
			val3 = m_coneDamage;
		}
		AddTokenInt(tokens, "ConeDamage", empty3, val3);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_RampartMeleeBasicAttack)
		{
			effectInfo2 = abilityMod_RampartMeleeBasicAttack.m_coneEnemyHitEffectMod.GetModifiedValue(m_coneEnemyHitEffect);
		}
		else
		{
			effectInfo2 = m_coneEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "ConeEnemyHitEffect", m_coneEnemyHitEffect);
	}
}
