using System;
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
	public int m_middleDamage = 0x14;

	public StandardEffectInfo m_middleEnemyHitEffect;

	public int m_sideDamage = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Overhead Slam";
		}
		this.Setup();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedMiddleEnemyHitEffect;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.SetCachedFields()).MethodHandle;
			}
			cachedMiddleEnemyHitEffect = this.m_abilityMod.m_middleEnemyHitEffectMod.GetModifiedValue(this.m_middleEnemyHitEffect);
		}
		else
		{
			cachedMiddleEnemyHitEffect = this.m_middleEnemyHitEffect;
		}
		this.m_cachedMiddleEnemyHitEffect = cachedMiddleEnemyHitEffect;
		StandardEffectInfo cachedSideEnemyHitEffect;
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
			cachedSideEnemyHitEffect = this.m_abilityMod.m_sideEnemyHitEffectMod.GetModifiedValue(this.m_sideEnemyHitEffect);
		}
		else
		{
			cachedSideEnemyHitEffect = this.m_sideEnemyHitEffect;
		}
		this.m_cachedSideEnemyHitEffect = cachedSideEnemyHitEffect;
	}

	public float GetLaserRange()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetLaserRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
		}
		else
		{
			result = this.m_laserRange;
		}
		return result;
	}

	public float GetMidLaserWidth()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetMidLaserWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_midLaserWidthMod.GetModifiedValue(this.m_midLaserWidth);
		}
		else
		{
			result = this.m_midLaserWidth;
		}
		return result;
	}

	public float GetFullLaserWidth()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetFullLaserWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_fullLaserWidthMod.GetModifiedValue(this.m_fullLaserWidth);
		}
		else
		{
			result = this.m_fullLaserWidth;
		}
		return result;
	}

	public int GetLaserMaxTargets()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetLaserMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
		}
		else
		{
			result = this.m_laserMaxTargets;
		}
		return result;
	}

	public bool GetPenetrateLos()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLos : this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
	}

	public int GetMiddleDamage()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetMiddleDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_middleDamageMod.GetModifiedValue(this.m_middleDamage);
		}
		else
		{
			result = this.m_middleDamage;
		}
		return result;
	}

	public StandardEffectInfo GetMiddleEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedMiddleEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetMiddleEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedMiddleEnemyHitEffect;
		}
		else
		{
			result = this.m_middleEnemyHitEffect;
		}
		return result;
	}

	public int GetSideDamage()
	{
		return (!this.m_abilityMod) ? this.m_sideDamage : this.m_abilityMod.m_sideDamageMod.GetModifiedValue(this.m_sideDamage);
	}

	public StandardEffectInfo GetSideEnemyHitEffect()
	{
		return (this.m_cachedSideEnemyHitEffect == null) ? this.m_sideEnemyHitEffect : this.m_cachedSideEnemyHitEffect;
	}

	public int GetExtraSideDamagePerMiddleHit()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetExtraSideDamagePerMiddleHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraSideDamagePerMiddleHitMod.GetModifiedValue(this.m_extraSideDamagePerMiddleHit);
		}
		else
		{
			result = this.m_extraSideDamagePerMiddleHit;
		}
		return result;
	}

	public int GetExtraDamageOnLowHealthTarget()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetExtraDamageOnLowHealthTarget()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageOnLowHealthTargetMod.GetModifiedValue(this.m_extraDamageOnLowHealthTarget);
		}
		else
		{
			result = this.m_extraDamageOnLowHealthTarget;
		}
		return result;
	}

	public float GetLowHealthThreshold()
	{
		float result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetLowHealthThreshold()).MethodHandle;
			}
			result = this.m_abilityMod.m_lowHealthThresholdMod.GetModifiedValue(this.m_lowHealthThreshold);
		}
		else
		{
			result = this.m_lowHealthThreshold;
		}
		return result;
	}

	public int GetEnergyLossOnMidHit()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetEnergyLossOnMidHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyLossOnMidHitMod.GetModifiedValue(this.m_energyLossOnMidHit);
		}
		else
		{
			result = this.m_energyLossOnMidHit;
		}
		return result;
	}

	public int GetEnergyLossOnSideHit()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetEnergyLossOnSideHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyLossOnSideHitMod.GetModifiedValue(this.m_energyLossOnSideHit);
		}
		else
		{
			result = this.m_energyLossOnSideHit;
		}
		return result;
	}

	public int GetHealPerMidHit()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetHealPerMidHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_healPerMidHit.GetModifiedValue(0);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetHealPerSideHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_healPerSideHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Claymore_SyncComponent>();
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_ClaymoreKnockbackLaser(this, this.GetFullLaserWidth(), this.GetLaserRange(), this.GetPenetrateLos(), this.m_laserLengthIgnoreWorldGeo, 0, this.GetMidLaserWidth(), 0f, KnockbackType.AwayFromSource);
		AbilityUtil_Targeter targeter = base.Targeter;
		bool affectsEnemies = true;
		bool affectsAllies = false;
		bool affectsCaster;
		if (this.GetHealPerMidHit() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.Setup()).MethodHandle;
			}
			affectsCaster = (this.GetHealPerSideHit() > 0);
		}
		else
		{
			affectsCaster = true;
		}
		targeter.SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreSlam abilityMod_ClaymoreSlam = modAsBase as AbilityMod_ClaymoreSlam;
		string name = "LaserMaxTargets";
		string empty = string.Empty;
		int val;
		if (abilityMod_ClaymoreSlam)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_ClaymoreSlam.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
		}
		else
		{
			val = this.m_laserMaxTargets;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "MiddleDamage";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ClaymoreSlam)
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
			val2 = abilityMod_ClaymoreSlam.m_middleDamageMod.GetModifiedValue(this.m_middleDamage);
		}
		else
		{
			val2 = this.m_middleDamage;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_ClaymoreSlam)
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
			effectInfo = abilityMod_ClaymoreSlam.m_middleEnemyHitEffectMod.GetModifiedValue(this.m_middleEnemyHitEffect);
		}
		else
		{
			effectInfo = this.m_middleEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "MiddleEnemyHitEffect", this.m_middleEnemyHitEffect, true);
		string name3 = "SideDamage";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_ClaymoreSlam)
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
			val3 = abilityMod_ClaymoreSlam.m_sideDamageMod.GetModifiedValue(this.m_sideDamage);
		}
		else
		{
			val3 = this.m_sideDamage;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_ClaymoreSlam)
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
			effectInfo2 = abilityMod_ClaymoreSlam.m_sideEnemyHitEffectMod.GetModifiedValue(this.m_sideEnemyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_sideEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "SideEnemyHitEffect", this.m_sideEnemyHitEffect, true);
		string name4 = "ExtraSideDamagePerMiddleHit";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_ClaymoreSlam)
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
			val4 = abilityMod_ClaymoreSlam.m_extraSideDamagePerMiddleHitMod.GetModifiedValue(this.m_extraSideDamagePerMiddleHit);
		}
		else
		{
			val4 = this.m_extraSideDamagePerMiddleHit;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		string name5 = "ExtraDamageOnLowHealthTarget";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_ClaymoreSlam)
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
			val5 = abilityMod_ClaymoreSlam.m_extraDamageOnLowHealthTargetMod.GetModifiedValue(this.m_extraDamageOnLowHealthTarget);
		}
		else
		{
			val5 = this.m_extraDamageOnLowHealthTarget;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		string name6 = "EnergyLossOnMidHit";
		string empty6 = string.Empty;
		int val6;
		if (abilityMod_ClaymoreSlam)
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
			val6 = abilityMod_ClaymoreSlam.m_energyLossOnMidHitMod.GetModifiedValue(this.m_energyLossOnMidHit);
		}
		else
		{
			val6 = this.m_energyLossOnMidHit;
		}
		base.AddTokenInt(tokens, name6, empty6, val6, false);
		string name7 = "EnergyLossOnSideHit";
		string empty7 = string.Empty;
		int val7;
		if (abilityMod_ClaymoreSlam)
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
			val7 = abilityMod_ClaymoreSlam.m_energyLossOnSideHitMod.GetModifiedValue(this.m_energyLossOnSideHit);
		}
		else
		{
			val7 = this.m_energyLossOnSideHit;
		}
		base.AddTokenInt(tokens, name7, empty7, val7, false);
		string name8 = "HealPerMidHit";
		string empty8 = string.Empty;
		int val8;
		if (abilityMod_ClaymoreSlam)
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
			val8 = abilityMod_ClaymoreSlam.m_healPerMidHit.GetModifiedValue(0);
		}
		else
		{
			val8 = 0;
		}
		base.AddTokenInt(tokens, name8, empty8, val8, false);
		string name9 = "HealPerSideHit";
		string empty9 = string.Empty;
		int val9;
		if (abilityMod_ClaymoreSlam)
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
			val9 = abilityMod_ClaymoreSlam.m_healPerSideHit.GetModifiedValue(0);
		}
		else
		{
			val9 = 0;
		}
		base.AddTokenInt(tokens, name9, empty9, val9, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref list, AbilityTooltipSubject.Primary, this.GetMiddleDamage());
		this.m_middleEnemyHitEffect.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportEnergy(ref list, AbilityTooltipSubject.Primary, -1 * this.GetEnergyLossOnMidHit());
		AbilityTooltipHelper.ReportDamage(ref list, AbilityTooltipSubject.Secondary, this.GetSideDamage());
		this.m_sideEnemyHitEffect.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportEnergy(ref list, AbilityTooltipSubject.Secondary, -1 * this.GetEnergyLossOnSideHit());
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, 0));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreSlam.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Primary);
			int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Secondary);
			int value = 0;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				if (targetActor.\u0012() < this.GetLowHealthThreshold())
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
					value = this.GetExtraDamageOnLowHealthTarget();
				}
			}
			dictionary[AbilityTooltipSymbol.Damage] = value;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
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
				Dictionary<AbilityTooltipSymbol, int> dictionary2;
				(dictionary2 = dictionary)[AbilityTooltipSymbol.Damage] = dictionary2[AbilityTooltipSymbol.Damage] + this.GetMiddleDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
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
				int num = this.GetSideDamage();
				if (this.GetExtraSideDamagePerMiddleHit() > 0)
				{
					num += visibleActorsCountByTooltipSubject * this.GetExtraSideDamagePerMiddleHit();
				}
				Dictionary<AbilityTooltipSymbol, int> dictionary2;
				(dictionary2 = dictionary)[AbilityTooltipSymbol.Damage] = dictionary2[AbilityTooltipSymbol.Damage] + num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				int num2 = this.GetHealPerMidHit() * visibleActorsCountByTooltipSubject + this.GetHealPerSideHit() * visibleActorsCountByTooltipSubject2;
				if (num2 > 0)
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
					dictionary[AbilityTooltipSymbol.Healing] = num2;
				}
			}
		}
		return dictionary;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return (!(this.m_syncComp != null)) ? null : this.m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreSlam))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ClaymoreSlam);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
