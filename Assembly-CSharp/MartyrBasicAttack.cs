using System.Collections.Generic;
using UnityEngine;

public class MartyrBasicAttack : MartyrLaserBase
{
	[Header("-- Targeting")]
	public LaserTargetingInfo m_laserInfo;

	public StandardEffectInfo m_laserHitEffect;

	public float m_explosionRadius = 2.5f;

	public float m_additionalRadiusPerCrystalSpent = 0.25f;

	[Header("-- Damage & Crystal Bonuses")]
	public int m_baseLaserDamage = 20;

	public int m_baseExplosionDamage = 15;

	public int m_additionalDamagePerCrystalSpent;

	[Space(5f)]
	public int m_extraDamageIfSingleHit;

	[Header("-- Inner Ring Radius and Damage")]
	public float m_innerRingRadius;

	public float m_innerRingExtraRadiusPerCrystal;

	[Space(5f)]
	public int m_innerRingDamage = 20;

	public int m_innerRingDamagePerCrystal;

	public List<MartyrBasicAttackThreshold> m_thresholdBasedCrystalBonuses;

	[Header("-- Sequences")]
	public GameObject m_projectileSequence;

	private Martyr_SyncComponent m_syncComponent;

	private AbilityMod_MartyrBasicAttack m_abilityMod;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedLaserHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Martyr Hit";
		}
		Setup();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected void Setup()
	{
		SetCachedFields();
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		AbilityUtil_Targeter_MartyrLaser abilityUtil_Targeter_MartyrLaser = new AbilityUtil_Targeter_MartyrLaser(this, GetCurrentLaserWidth(), GetCurrentLaserRange(), GetCurrentLaserPenetrateLoS(), GetCurrentLaserMaxTargets(), true, false, false, true, false, GetCurrentExplosionRadius(), GetCurrentInnerExplosionRadius(), false, true, false);
		abilityUtil_Targeter_MartyrLaser.m_delegateLaserWidth = base.GetCurrentLaserWidth;
		abilityUtil_Targeter_MartyrLaser.m_delegateLaserRange = base.GetCurrentLaserRange;
		abilityUtil_Targeter_MartyrLaser.m_delegatePenetrateLos = base.GetCurrentLaserPenetrateLoS;
		abilityUtil_Targeter_MartyrLaser.m_delegateMaxTargets = base.GetCurrentLaserMaxTargets;
		abilityUtil_Targeter_MartyrLaser.m_delegateConeRadius = GetCurrentExplosionRadius;
		abilityUtil_Targeter_MartyrLaser.m_delegateInnerConeRadius = GetCurrentInnerExplosionRadius;
		base.Targeter = abilityUtil_Targeter_MartyrLaser;
		base.Targeter.SetShowArcToShape(true);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetCurrentLaserRange() + GetCurrentExplosionRadius();
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedLaserInfo = m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo);
		}
		else
		{
			cachedLaserInfo = m_laserInfo;
		}
		m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedLaserHitEffect;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedLaserHitEffect = m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = m_laserHitEffect;
		}
		m_cachedLaserHitEffect = cachedLaserHitEffect;
	}

	public override LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		return (m_cachedLaserHitEffect == null) ? m_laserHitEffect : m_cachedLaserHitEffect;
	}

	public float GetExplosionRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_explosionRadiusMod.GetModifiedValue(m_explosionRadius);
		}
		else
		{
			result = m_explosionRadius;
		}
		return result;
	}

	public int GetBaseLaserDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_baseLaserDamageMod.GetModifiedValue(m_baseLaserDamage);
		}
		else
		{
			result = m_baseLaserDamage;
		}
		return result;
	}

	public int GetBaseExplosionDamage()
	{
		return (!m_abilityMod) ? m_baseExplosionDamage : m_abilityMod.m_baseExplosionDamageMod.GetModifiedValue(m_baseExplosionDamage);
	}

	public int GetAdditionalDamagePerCrystalSpent()
	{
		return (!m_abilityMod) ? m_additionalDamagePerCrystalSpent : m_abilityMod.m_additionalDamagePerCrystalSpentMod.GetModifiedValue(m_additionalDamagePerCrystalSpent);
	}

	public float GetAdditionalRadiusPerCrystalSpent()
	{
		return (!m_abilityMod) ? m_additionalRadiusPerCrystalSpent : m_abilityMod.m_additionalRadiusPerCrystalSpentMod.GetModifiedValue(m_additionalRadiusPerCrystalSpent);
	}

	public int GetExtraDamageIfSingleHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_extraDamageIfSingleHitMod.GetModifiedValue(m_extraDamageIfSingleHit);
		}
		else
		{
			result = m_extraDamageIfSingleHit;
		}
		return result;
	}

	public float GetInnerRingRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_innerRingRadiusMod.GetModifiedValue(m_innerRingRadius);
		}
		else
		{
			result = m_innerRingRadius;
		}
		return result;
	}

	public float GetInnerRingExtraRadiusPerCrystal()
	{
		return (!m_abilityMod) ? m_innerRingExtraRadiusPerCrystal : m_abilityMod.m_innerRingExtraRadiusPerCrystalMod.GetModifiedValue(m_innerRingExtraRadiusPerCrystal);
	}

	public int GetInnerRingDamage()
	{
		return (!m_abilityMod) ? m_innerRingDamage : m_abilityMod.m_innerRingDamageMod.GetModifiedValue(m_innerRingDamage);
	}

	public int GetInnerRingDamagePerCrystal()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_innerRingDamagePerCrystalMod.GetModifiedValue(m_innerRingDamagePerCrystal);
		}
		else
		{
			result = m_innerRingDamagePerCrystal;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_MartyrBasicAttack abilityMod_MartyrBasicAttack = modAsBase as AbilityMod_MartyrBasicAttack;
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_MartyrBasicAttack)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			effectInfo = abilityMod_MartyrBasicAttack.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect);
		}
		else
		{
			effectInfo = m_laserHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "LaserHitEffect", m_laserHitEffect);
		AddTokenInt(tokens, "BaseLaserDamage", string.Empty, (!abilityMod_MartyrBasicAttack) ? m_baseLaserDamage : abilityMod_MartyrBasicAttack.m_baseLaserDamageMod.GetModifiedValue(m_baseLaserDamage));
		AddTokenInt(tokens, "BaseExplosionDamage", string.Empty, (!abilityMod_MartyrBasicAttack) ? m_baseExplosionDamage : abilityMod_MartyrBasicAttack.m_baseExplosionDamageMod.GetModifiedValue(m_baseExplosionDamage));
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_MartyrBasicAttack)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			val = abilityMod_MartyrBasicAttack.m_additionalDamagePerCrystalSpentMod.GetModifiedValue(m_additionalDamagePerCrystalSpent);
		}
		else
		{
			val = m_additionalDamagePerCrystalSpent;
		}
		AddTokenInt(tokens, "AdditionalDamagePerCrystalSpent", empty, val);
		string empty2 = string.Empty;
		float val2;
		if ((bool)abilityMod_MartyrBasicAttack)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_MartyrBasicAttack.m_additionalRadiusPerCrystalSpentMod.GetModifiedValue(m_additionalRadiusPerCrystalSpent);
		}
		else
		{
			val2 = m_additionalRadiusPerCrystalSpent;
		}
		AddTokenFloat(tokens, "AdditionalRadiusPerCrystalSpent", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_MartyrBasicAttack)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			val3 = abilityMod_MartyrBasicAttack.m_extraDamageIfSingleHitMod.GetModifiedValue(m_extraDamageIfSingleHit);
		}
		else
		{
			val3 = m_extraDamageIfSingleHit;
		}
		AddTokenInt(tokens, "ExtraDamageIfSingleHit", empty3, val3);
		AddTokenInt(tokens, "InnerRingDamage", string.Empty, (!abilityMod_MartyrBasicAttack) ? m_innerRingDamage : abilityMod_MartyrBasicAttack.m_innerRingDamageMod.GetModifiedValue(m_innerRingDamage));
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_MartyrBasicAttack)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val4 = abilityMod_MartyrBasicAttack.m_innerRingDamagePerCrystalMod.GetModifiedValue(m_innerRingDamagePerCrystal);
		}
		else
		{
			val4 = m_innerRingDamagePerCrystal;
		}
		AddTokenInt(tokens, "InnerRingDamagePerCrystal", empty4, val4);
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		using (List<MartyrBasicAttackThreshold>.Enumerator enumerator = m_thresholdBasedCrystalBonuses.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MartyrBasicAttackThreshold current = enumerator.Current;
				list.Add(current);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return list;
					}
					/*OpCode not supported: LdMemberToken*/;
					return list;
				}
			}
		}
	}

	private int GetCurrentLaserDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int num;
		if (martyrBasicAttackThreshold != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = martyrBasicAttackThreshold.m_additionalDamage;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return GetBaseLaserDamage() + m_syncComponent.SpentDamageCrystals(caster) * GetAdditionalDamagePerCrystalSpent() + num2;
	}

	private int GetCurrentExplosionDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int num;
		if (martyrBasicAttackThreshold != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = martyrBasicAttackThreshold.m_additionalDamage;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return GetBaseExplosionDamage() + m_syncComponent.SpentDamageCrystals(caster) * GetAdditionalDamagePerCrystalSpent() + num2;
	}

	public override float GetCurrentExplosionRadius()
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = GetCurrentPowerEntry(base.ActorData) as MartyrBasicAttackThreshold;
		float num;
		if (martyrBasicAttackThreshold != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = martyrBasicAttackThreshold.m_additionalRadius;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		return GetExplosionRadius() + (float)m_syncComponent.SpentDamageCrystals(base.ActorData) * GetAdditionalRadiusPerCrystalSpent() + num2;
	}

	public int GetCurrentInnerExplosionDamage(ActorData caster)
	{
		return GetInnerRingDamage() + m_syncComponent.SpentDamageCrystals(caster) * GetInnerRingDamagePerCrystal();
	}

	public override float GetCurrentInnerExplosionRadius()
	{
		return GetInnerRingRadius() + (float)m_syncComponent.SpentDamageCrystals(base.ActorData) * GetInnerRingExtraRadiusPerCrystal();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetBaseLaserDamage());
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetBaseExplosionDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		int num = 0;
		if (GetExtraDamageIfSingleHit() > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			if (visibleActorsCountByTooltipSubject == 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = GetExtraDamageIfSingleHit();
			}
		}
		Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, GetCurrentLaserDamage(base.ActorData) + num);
		ActorData actorData = base.ActorData;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag = false;
			float currentInnerExplosionRadius = GetCurrentInnerExplosionRadius();
			if (currentInnerExplosionRadius > 0f)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (base.Targeter is AbilityUtil_Targeter_MartyrLaser)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					AbilityUtil_Targeter_MartyrLaser abilityUtil_Targeter_MartyrLaser = base.Targeter as AbilityUtil_Targeter_MartyrLaser;
					flag = AreaEffectUtils.IsSquareInConeByActorRadius(targetActor.GetCurrentBoardSquare(), abilityUtil_Targeter_MartyrLaser.m_lastLaserEndPos, 0f, 360f, currentInnerExplosionRadius, 0f, true, actorData);
				}
			}
			int num2 = (!flag) ? GetCurrentExplosionDamage(actorData) : GetCurrentInnerExplosionDamage(actorData);
			symbolToValue[AbilityTooltipSymbol.Damage] = num2 + num;
		}
		return symbolToValue;
	}

	public override TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return TargetingParadigm.Position;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrBasicAttack))
		{
			m_abilityMod = (abilityMod as AbilityMod_MartyrBasicAttack);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
