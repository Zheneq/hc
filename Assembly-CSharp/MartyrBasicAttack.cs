using System;
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
	public int m_baseLaserDamage = 0x14;

	public int m_baseExplosionDamage = 0xF;

	public int m_additionalDamagePerCrystalSpent;

	[Space(5f)]
	public int m_extraDamageIfSingleHit;

	[Header("-- Inner Ring Radius and Damage")]
	public float m_innerRingRadius;

	public float m_innerRingExtraRadiusPerCrystal;

	[Space(5f)]
	public int m_innerRingDamage = 0x14;

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
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.Start()).MethodHandle;
			}
			this.m_abilityName = "Martyr Hit";
		}
		this.Setup();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return this.m_syncComponent;
	}

	protected void Setup()
	{
		this.SetCachedFields();
		this.m_syncComponent = base.GetComponent<Martyr_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_MartyrLaser(this, base.GetCurrentLaserWidth(), base.GetCurrentLaserRange(), base.GetCurrentLaserPenetrateLoS(), base.GetCurrentLaserMaxTargets(), true, false, false, true, false, this.GetCurrentExplosionRadius(), this.GetCurrentInnerExplosionRadius(), false, true, false)
		{
			m_delegateLaserWidth = new AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate(base.GetCurrentLaserWidth),
			m_delegateLaserRange = new AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate(base.GetCurrentLaserRange),
			m_delegatePenetrateLos = new AbilityUtil_Targeter_MartyrLaser.CustomBoolValueDelegate(base.GetCurrentLaserPenetrateLoS),
			m_delegateMaxTargets = new AbilityUtil_Targeter_MartyrLaser.CustomIntValueDelegate(base.GetCurrentLaserMaxTargets),
			m_delegateConeRadius = new AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate(this.GetCurrentExplosionRadius),
			m_delegateInnerConeRadius = new AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate(this.GetCurrentInnerExplosionRadius)
		};
		base.Targeter.SetShowArcToShape(true);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetCurrentLaserRange() + this.GetCurrentExplosionRadius();
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.SetCachedFields()).MethodHandle;
			}
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedLaserHitEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedLaserHitEffect = this.m_abilityMod.m_laserHitEffectMod.GetModifiedValue(this.m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = this.m_laserHitEffect;
		}
		this.m_cachedLaserHitEffect = cachedLaserHitEffect;
	}

	public override LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetLaserInfo()).MethodHandle;
			}
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		return (this.m_cachedLaserHitEffect == null) ? this.m_laserHitEffect : this.m_cachedLaserHitEffect;
	}

	public float GetExplosionRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetExplosionRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_explosionRadiusMod.GetModifiedValue(this.m_explosionRadius);
		}
		else
		{
			result = this.m_explosionRadius;
		}
		return result;
	}

	public int GetBaseLaserDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetBaseLaserDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_baseLaserDamageMod.GetModifiedValue(this.m_baseLaserDamage);
		}
		else
		{
			result = this.m_baseLaserDamage;
		}
		return result;
	}

	public int GetBaseExplosionDamage()
	{
		return (!this.m_abilityMod) ? this.m_baseExplosionDamage : this.m_abilityMod.m_baseExplosionDamageMod.GetModifiedValue(this.m_baseExplosionDamage);
	}

	public int GetAdditionalDamagePerCrystalSpent()
	{
		return (!this.m_abilityMod) ? this.m_additionalDamagePerCrystalSpent : this.m_abilityMod.m_additionalDamagePerCrystalSpentMod.GetModifiedValue(this.m_additionalDamagePerCrystalSpent);
	}

	public float GetAdditionalRadiusPerCrystalSpent()
	{
		return (!this.m_abilityMod) ? this.m_additionalRadiusPerCrystalSpent : this.m_abilityMod.m_additionalRadiusPerCrystalSpentMod.GetModifiedValue(this.m_additionalRadiusPerCrystalSpent);
	}

	public int GetExtraDamageIfSingleHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetExtraDamageIfSingleHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageIfSingleHitMod.GetModifiedValue(this.m_extraDamageIfSingleHit);
		}
		else
		{
			result = this.m_extraDamageIfSingleHit;
		}
		return result;
	}

	public float GetInnerRingRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetInnerRingRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_innerRingRadiusMod.GetModifiedValue(this.m_innerRingRadius);
		}
		else
		{
			result = this.m_innerRingRadius;
		}
		return result;
	}

	public float GetInnerRingExtraRadiusPerCrystal()
	{
		return (!this.m_abilityMod) ? this.m_innerRingExtraRadiusPerCrystal : this.m_abilityMod.m_innerRingExtraRadiusPerCrystalMod.GetModifiedValue(this.m_innerRingExtraRadiusPerCrystal);
	}

	public int GetInnerRingDamage()
	{
		return (!this.m_abilityMod) ? this.m_innerRingDamage : this.m_abilityMod.m_innerRingDamageMod.GetModifiedValue(this.m_innerRingDamage);
	}

	public int GetInnerRingDamagePerCrystal()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetInnerRingDamagePerCrystal()).MethodHandle;
			}
			result = this.m_abilityMod.m_innerRingDamagePerCrystalMod.GetModifiedValue(this.m_innerRingDamagePerCrystal);
		}
		else
		{
			result = this.m_innerRingDamagePerCrystal;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_MartyrBasicAttack abilityMod_MartyrBasicAttack = modAsBase as AbilityMod_MartyrBasicAttack;
		StandardEffectInfo effectInfo;
		if (abilityMod_MartyrBasicAttack)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_MartyrBasicAttack.m_laserHitEffectMod.GetModifiedValue(this.m_laserHitEffect);
		}
		else
		{
			effectInfo = this.m_laserHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "LaserHitEffect", this.m_laserHitEffect, true);
		base.AddTokenInt(tokens, "BaseLaserDamage", string.Empty, (!abilityMod_MartyrBasicAttack) ? this.m_baseLaserDamage : abilityMod_MartyrBasicAttack.m_baseLaserDamageMod.GetModifiedValue(this.m_baseLaserDamage), false);
		base.AddTokenInt(tokens, "BaseExplosionDamage", string.Empty, (!abilityMod_MartyrBasicAttack) ? this.m_baseExplosionDamage : abilityMod_MartyrBasicAttack.m_baseExplosionDamageMod.GetModifiedValue(this.m_baseExplosionDamage), false);
		string name = "AdditionalDamagePerCrystalSpent";
		string empty = string.Empty;
		int val;
		if (abilityMod_MartyrBasicAttack)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			val = abilityMod_MartyrBasicAttack.m_additionalDamagePerCrystalSpentMod.GetModifiedValue(this.m_additionalDamagePerCrystalSpent);
		}
		else
		{
			val = this.m_additionalDamagePerCrystalSpent;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "AdditionalRadiusPerCrystalSpent";
		string empty2 = string.Empty;
		float val2;
		if (abilityMod_MartyrBasicAttack)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_MartyrBasicAttack.m_additionalRadiusPerCrystalSpentMod.GetModifiedValue(this.m_additionalRadiusPerCrystalSpent);
		}
		else
		{
			val2 = this.m_additionalRadiusPerCrystalSpent;
		}
		base.AddTokenFloat(tokens, name2, empty2, val2, false);
		string name3 = "ExtraDamageIfSingleHit";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_MartyrBasicAttack)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			val3 = abilityMod_MartyrBasicAttack.m_extraDamageIfSingleHitMod.GetModifiedValue(this.m_extraDamageIfSingleHit);
		}
		else
		{
			val3 = this.m_extraDamageIfSingleHit;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		base.AddTokenInt(tokens, "InnerRingDamage", string.Empty, (!abilityMod_MartyrBasicAttack) ? this.m_innerRingDamage : abilityMod_MartyrBasicAttack.m_innerRingDamageMod.GetModifiedValue(this.m_innerRingDamage), false);
		string name4 = "InnerRingDamagePerCrystal";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_MartyrBasicAttack)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val4 = abilityMod_MartyrBasicAttack.m_innerRingDamagePerCrystalMod.GetModifiedValue(this.m_innerRingDamagePerCrystal);
		}
		else
		{
			val4 = this.m_innerRingDamagePerCrystal;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		using (List<MartyrBasicAttackThreshold>.Enumerator enumerator = this.m_thresholdBasedCrystalBonuses.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MartyrBasicAttackThreshold item = enumerator.Current;
				list.Add(item);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetThresholdBasedCrystalBonusList()).MethodHandle;
			}
		}
		return list;
	}

	private int GetCurrentLaserDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = base.GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int num;
		if (martyrBasicAttackThreshold != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetCurrentLaserDamage(ActorData)).MethodHandle;
			}
			num = martyrBasicAttackThreshold.m_additionalDamage;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseLaserDamage() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetAdditionalDamagePerCrystalSpent() + num2;
	}

	private int GetCurrentExplosionDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = base.GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int num;
		if (martyrBasicAttackThreshold != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetCurrentExplosionDamage(ActorData)).MethodHandle;
			}
			num = martyrBasicAttackThreshold.m_additionalDamage;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseExplosionDamage() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetAdditionalDamagePerCrystalSpent() + num2;
	}

	public override float GetCurrentExplosionRadius()
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = base.GetCurrentPowerEntry(base.ActorData) as MartyrBasicAttackThreshold;
		float num;
		if (martyrBasicAttackThreshold != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetCurrentExplosionRadius()).MethodHandle;
			}
			num = martyrBasicAttackThreshold.m_additionalRadius;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		return this.GetExplosionRadius() + (float)this.m_syncComponent.SpentDamageCrystals(base.ActorData) * this.GetAdditionalRadiusPerCrystalSpent() + num2;
	}

	public int GetCurrentInnerExplosionDamage(ActorData caster)
	{
		return this.GetInnerRingDamage() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetInnerRingDamagePerCrystal();
	}

	public override float GetCurrentInnerExplosionRadius()
	{
		return this.GetInnerRingRadius() + (float)this.m_syncComponent.SpentDamageCrystals(base.ActorData) * this.GetInnerRingExtraRadiusPerCrystal();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetBaseLaserDamage());
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.GetBaseExplosionDamage());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = 0;
		if (this.GetExtraDamageIfSingleHit() > 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrBasicAttack.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			if (visibleActorsCountByTooltipSubject == 1)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.GetExtraDamageIfSingleHit();
			}
		}
		Ability.AddNameplateValueForSingleHit(ref dictionary, base.Targeter, targetActor, this.GetCurrentLaserDamage(base.ActorData) + num, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		ActorData actorData = base.ActorData;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag = false;
			float currentInnerExplosionRadius = this.GetCurrentInnerExplosionRadius();
			if (currentInnerExplosionRadius > 0f)
			{
				for (;;)
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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					AbilityUtil_Targeter_MartyrLaser abilityUtil_Targeter_MartyrLaser = base.Targeter as AbilityUtil_Targeter_MartyrLaser;
					flag = AreaEffectUtils.IsSquareInConeByActorRadius(targetActor.GetCurrentBoardSquare(), abilityUtil_Targeter_MartyrLaser.m_lastLaserEndPos, 0f, 360f, currentInnerExplosionRadius, 0f, true, actorData, false, default(Vector3));
				}
			}
			int num2;
			if (flag)
			{
				num2 = this.GetCurrentInnerExplosionDamage(actorData);
			}
			else
			{
				num2 = this.GetCurrentExplosionDamage(actorData);
			}
			dictionary[AbilityTooltipSymbol.Damage] = num2 + num;
		}
		return dictionary;
	}

	public override Ability.TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return Ability.TargetingParadigm.Position;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrBasicAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_MartyrBasicAttack);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
