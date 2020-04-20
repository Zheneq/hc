using System;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreMultiRadiusCone : Ability
{
	[Header("-- Cone Targeting")]
	public float m_coneWidthAngle = 180f;

	public float m_coneBackwardOffset;

	public float m_coneLengthInner = 1.5f;

	public float m_coneLengthMiddle = 2.5f;

	public float m_coneLengthOuter = 3.5f;

	public bool m_penetrateLineOfSight;

	[Header("-- Base Damage")]
	public int m_damageAmountInner = 5;

	public int m_damageAmountMiddle = 4;

	public int m_damageAmountOuter = 3;

	[Header("-- Bonus Damage, (threshold value range 0 to 1)")]
	public int m_bonusDamageIfEnemyLowHealth;

	public float m_enemyHealthThreshForBonus = -1f;

	public int m_bonusDamageIfCasterLowHealth;

	public float m_casterHealthThreshForBonus = -1f;

	[Header("-- Hit Effects")]
	public StandardEffectInfo m_effectInner;

	public StandardEffectInfo m_effectMiddle;

	public StandardEffectInfo m_effectOuter;

	[Header("-- Energy Gain on Self for Hits")]
	public int m_tpGainInner;

	public int m_tpGainMiddle;

	public int m_tpGainOuter;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ClaymoreMultiRadiusCone m_abilityMod;

	private Claymore_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedEffectInner;

	private StandardEffectInfo m_cachedEffectMiddle;

	private StandardEffectInfo m_cachedEffectOuter;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Mountain Cleaver";
		}
		this.SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.ModdedOuterRadius();
	}

	private float ModdedConeAngle()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ModdedConeAngle()).MethodHandle;
			}
			result = this.m_coneWidthAngle;
		}
		else
		{
			result = this.m_abilityMod.m_coneAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		return result;
	}

	private float ModdedInnerRadius()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_coneInnerRadiusMod.GetModifiedValue(this.m_coneLengthInner) : this.m_coneLengthInner;
	}

	private float ModdedMiddleRadius()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ModdedMiddleRadius()).MethodHandle;
			}
			result = this.m_coneLengthMiddle;
		}
		else
		{
			result = this.m_abilityMod.m_coneMiddleRadiusMod.GetModifiedValue(this.m_coneLengthMiddle);
		}
		return result;
	}

	private float ModdedOuterRadius()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ModdedOuterRadius()).MethodHandle;
			}
			result = this.m_coneLengthOuter;
		}
		else
		{
			result = this.m_abilityMod.m_coneOuterRadiusMod.GetModifiedValue(this.m_coneLengthOuter);
		}
		return result;
	}

	private bool GetPenetrateLineOfSight()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.GetPenetrateLineOfSight()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		else
		{
			result = this.m_penetrateLineOfSight;
		}
		return result;
	}

	private int ModdedInnerDamage()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ModdedInnerDamage()).MethodHandle;
			}
			result = this.m_damageAmountInner;
		}
		else
		{
			result = this.m_abilityMod.m_innerDamageMod.GetModifiedValue(this.m_damageAmountInner);
		}
		return result;
	}

	private int ModdedMiddleDamage()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ModdedMiddleDamage()).MethodHandle;
			}
			result = this.m_damageAmountMiddle;
		}
		else
		{
			result = this.m_abilityMod.m_middleDamageMod.GetModifiedValue(this.m_damageAmountMiddle);
		}
		return result;
	}

	private int ModdedOuterDamage()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ModdedOuterDamage()).MethodHandle;
			}
			result = this.m_damageAmountOuter;
		}
		else
		{
			result = this.m_abilityMod.m_outerDamageMod.GetModifiedValue(this.m_damageAmountOuter);
		}
		return result;
	}

	private int ModdedInnerTpGain()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ModdedInnerTpGain()).MethodHandle;
			}
			result = this.m_tpGainInner;
		}
		else
		{
			result = this.m_abilityMod.m_innerTpGain.GetModifiedValue(this.m_tpGainInner);
		}
		return result;
	}

	private int ModdedMiddleTpGain()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ModdedMiddleTpGain()).MethodHandle;
			}
			result = this.m_tpGainMiddle;
		}
		else
		{
			result = this.m_abilityMod.m_middleTpGain.GetModifiedValue(this.m_tpGainMiddle);
		}
		return result;
	}

	private int ModdedOuterTpGain()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ModdedOuterTpGain()).MethodHandle;
			}
			result = this.m_tpGainOuter;
		}
		else
		{
			result = this.m_abilityMod.m_outerTpGain.GetModifiedValue(this.m_tpGainOuter);
		}
		return result;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectInner;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.SetCachedFields()).MethodHandle;
			}
			cachedEffectInner = this.m_abilityMod.m_effectInnerMod.GetModifiedValue(this.m_effectInner);
		}
		else
		{
			cachedEffectInner = this.m_effectInner;
		}
		this.m_cachedEffectInner = cachedEffectInner;
		StandardEffectInfo cachedEffectMiddle;
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
			cachedEffectMiddle = this.m_abilityMod.m_effectMiddleMod.GetModifiedValue(this.m_effectMiddle);
		}
		else
		{
			cachedEffectMiddle = this.m_effectMiddle;
		}
		this.m_cachedEffectMiddle = cachedEffectMiddle;
		StandardEffectInfo cachedEffectOuter;
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
			cachedEffectOuter = this.m_abilityMod.m_effectOuterMod.GetModifiedValue(this.m_effectOuter);
		}
		else
		{
			cachedEffectOuter = this.m_effectOuter;
		}
		this.m_cachedEffectOuter = cachedEffectOuter;
	}

	public StandardEffectInfo GetEffectInner()
	{
		return (this.m_cachedEffectInner == null) ? this.m_effectInner : this.m_cachedEffectInner;
	}

	public StandardEffectInfo GetEffectMiddle()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectMiddle != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.GetEffectMiddle()).MethodHandle;
			}
			result = this.m_cachedEffectMiddle;
		}
		else
		{
			result = this.m_effectMiddle;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOuter()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOuter != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.GetEffectOuter()).MethodHandle;
			}
			result = this.m_cachedEffectOuter;
		}
		else
		{
			result = this.m_effectOuter;
		}
		return result;
	}

	public int GetBonusDamageIfEnemyHealthBelow()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.GetBonusDamageIfEnemyHealthBelow()).MethodHandle;
			}
			result = this.m_abilityMod.m_bonusDamageIfEnemyLowHealthMod.GetModifiedValue(this.m_bonusDamageIfEnemyLowHealth);
		}
		else
		{
			result = this.m_bonusDamageIfEnemyLowHealth;
		}
		return result;
	}

	public float GetEnemyHealthThreshForBonus()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.GetEnemyHealthThreshForBonus()).MethodHandle;
			}
			result = this.m_abilityMod.m_enemyHealthThreshForBonusMod.GetModifiedValue(this.m_enemyHealthThreshForBonus);
		}
		else
		{
			result = this.m_enemyHealthThreshForBonus;
		}
		return result;
	}

	public int GetBonusDamageIfCasterHealthBelow()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.GetBonusDamageIfCasterHealthBelow()).MethodHandle;
			}
			result = this.m_abilityMod.m_bonusDamageIfCasterLowHealthMod.GetModifiedValue(this.m_bonusDamageIfCasterLowHealth);
		}
		else
		{
			result = this.m_bonusDamageIfCasterLowHealth;
		}
		return result;
	}

	public float GetCasterHealthThreshForBonus()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.GetCasterHealthThreshForBonus()).MethodHandle;
			}
			result = this.m_abilityMod.m_casterHealthThreshForBonusMod.GetModifiedValue(this.m_casterHealthThreshForBonus);
		}
		else
		{
			result = this.m_casterHealthThreshForBonus;
		}
		return result;
	}

	public bool ShouldApplyCasterBonusPerThresholdReached()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.ShouldApplyCasterBonusPerThresholdReached()).MethodHandle;
			}
			result = this.m_abilityMod.m_applyBonusPerThresholdReached;
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void SetupTargeter()
	{
		this.m_syncComp = base.GetComponent<Claymore_SyncComponent>();
		this.SetCachedFields();
		float angle = this.ModdedConeAngle();
		base.Targeter = new AbilityUtil_Targeter_MultipleCones(this, new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>
		{
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, this.ModdedInnerRadius()),
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, this.ModdedMiddleRadius()),
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, this.ModdedOuterRadius())
		}, this.m_coneBackwardOffset, this.GetPenetrateLineOfSight(), true, true, false, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreMultiRadiusCone abilityMod_ClaymoreMultiRadiusCone = modAsBase as AbilityMod_ClaymoreMultiRadiusCone;
		string name = "DamageAmountInner";
		string empty = string.Empty;
		int val;
		if (abilityMod_ClaymoreMultiRadiusCone)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_ClaymoreMultiRadiusCone.m_innerDamageMod.GetModifiedValue(this.m_damageAmountInner);
		}
		else
		{
			val = this.m_damageAmountInner;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "DamageAmountMiddle";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ClaymoreMultiRadiusCone)
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
			val2 = abilityMod_ClaymoreMultiRadiusCone.m_middleDamageMod.GetModifiedValue(this.m_damageAmountMiddle);
		}
		else
		{
			val2 = this.m_damageAmountMiddle;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "DamageAmountOuter";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_ClaymoreMultiRadiusCone)
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
			val3 = abilityMod_ClaymoreMultiRadiusCone.m_outerDamageMod.GetModifiedValue(this.m_damageAmountOuter);
		}
		else
		{
			val3 = this.m_damageAmountOuter;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "TpGainInner";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_ClaymoreMultiRadiusCone)
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
			val4 = abilityMod_ClaymoreMultiRadiusCone.m_innerTpGain.GetModifiedValue(this.m_tpGainInner);
		}
		else
		{
			val4 = this.m_tpGainInner;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		base.AddTokenInt(tokens, "TpGainMiddle", string.Empty, (!abilityMod_ClaymoreMultiRadiusCone) ? this.m_tpGainMiddle : abilityMod_ClaymoreMultiRadiusCone.m_middleTpGain.GetModifiedValue(this.m_tpGainMiddle), false);
		base.AddTokenInt(tokens, "TpGainOuter", string.Empty, (!abilityMod_ClaymoreMultiRadiusCone) ? this.m_tpGainOuter : abilityMod_ClaymoreMultiRadiusCone.m_outerTpGain.GetModifiedValue(this.m_tpGainOuter), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_ClaymoreMultiRadiusCone)
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
			effectInfo = abilityMod_ClaymoreMultiRadiusCone.m_effectInnerMod.GetModifiedValue(this.m_effectInner);
		}
		else
		{
			effectInfo = this.m_effectInner;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectInner", this.m_effectInner, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_ClaymoreMultiRadiusCone)
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
			effectInfo2 = abilityMod_ClaymoreMultiRadiusCone.m_effectMiddleMod.GetModifiedValue(this.m_effectMiddle);
		}
		else
		{
			effectInfo2 = this.m_effectMiddle;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectMiddle", this.m_effectMiddle, true);
		StandardEffectInfo effectInfo3;
		if (abilityMod_ClaymoreMultiRadiusCone)
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
			effectInfo3 = abilityMod_ClaymoreMultiRadiusCone.m_effectOuterMod.GetModifiedValue(this.m_effectOuter);
		}
		else
		{
			effectInfo3 = this.m_effectOuter;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "EffectOuter", this.m_effectOuter, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, this.m_damageAmountInner));
		this.GetEffectInner().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Near);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Midranged, this.m_damageAmountMiddle));
		this.GetEffectMiddle().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Midranged);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, this.m_damageAmountOuter));
		this.GetEffectOuter().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Far);
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			ActorData actorData = base.ActorData;
			int num = 0;
			if (this.GetBonusDamageIfCasterHealthBelow() > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
				}
				if (this.GetCasterHealthThreshForBonus() > 0f)
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
					float num2 = (float)actorData.HitPoints / (float)actorData.GetMaxHitPoints();
					if (this.ShouldApplyCasterBonusPerThresholdReached())
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
						int num3 = Mathf.FloorToInt((1f - num2) / this.GetCasterHealthThreshForBonus());
						num += this.GetBonusDamageIfCasterHealthBelow() * num3;
					}
					else if (num2 < this.GetCasterHealthThreshForBonus())
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
						num += this.GetBonusDamageIfCasterHealthBelow();
					}
				}
			}
			if (this.GetBonusDamageIfEnemyHealthBelow() > 0 && this.GetEnemyHealthThreshForBonus() > 0f)
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
				if ((float)targetActor.HitPoints / (float)targetActor.GetMaxHitPoints() < this.GetEnemyHealthThreshForBonus())
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
					num += this.GetBonusDamageIfEnemyHealthBelow();
				}
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Near))
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
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedInnerDamage() + num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Midranged))
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
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedMiddleDamage() + num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far))
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
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedOuterDamage() + num;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		if (this.ModdedInnerTpGain() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Near);
			num += visibleActorsInRangeByTooltipSubject.Count * this.ModdedInnerTpGain();
		}
		if (this.ModdedMiddleTpGain() > 0)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject2 = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Midranged);
			num += visibleActorsInRangeByTooltipSubject2.Count * this.ModdedMiddleTpGain();
		}
		if (this.ModdedOuterTpGain() > 0)
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
			List<ActorData> visibleActorsInRangeByTooltipSubject3 = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Far);
			num += visibleActorsInRangeByTooltipSubject3.Count * this.ModdedOuterTpGain();
		}
		return num;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject, ActorData, Vector3, ActorData)).MethodHandle;
			}
			if (subjectType != AbilityTooltipSubject.Midranged)
			{
				if (subjectType != AbilityTooltipSubject.Far)
				{
					return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		float num = this.ModdedInnerRadius() * Board.Get().squareSize;
		float num2 = this.ModdedMiddleRadius() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - damageOrigin;
		vector.y = 0f;
		float num3 = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			num3 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}
		bool flag = num3 <= num;
		bool flag2;
		if (!flag)
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
			flag2 = (num3 <= num2);
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		bool flag4;
		if (!flag)
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
			flag4 = !flag3;
		}
		else
		{
			flag4 = false;
		}
		bool flag5 = flag4;
		bool result;
		if (subjectType == AbilityTooltipSubject.Near)
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
			result = flag;
		}
		else if (subjectType == AbilityTooltipSubject.Midranged)
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
			result = flag3;
		}
		else
		{
			result = flag5;
		}
		return result;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return (!(this.m_syncComp != null)) ? null : this.m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreMultiRadiusCone))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreMultiRadiusCone.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ClaymoreMultiRadiusCone);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
