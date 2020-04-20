using System;
using System.Collections.Generic;
using UnityEngine;

public class FishManStaticCone : Ability
{
	[Header("-- Cone Data")]
	public float m_coneWidthAngle = 60f;

	public float m_coneWidthAngleMin = 5f;

	public float m_coneLength = 5f;

	public float m_coneBackwardOffset;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets;

	[Header("-- (for stretch cone only)")]
	public bool m_useDiscreteAngleChange;

	public float m_stretchInterpMinDist = 2.5f;

	public float m_stretchInterpRange = 4f;

	[Header("-- On Hit Target")]
	public int m_damageToEnemies;

	public int m_damageToEnemiesMax;

	public StandardEffectInfo m_effectToEnemies;

	[Space(10f)]
	public int m_healingToAllies = 0xF;

	public int m_healingToAlliesMax = 0x19;

	public StandardEffectInfo m_effectToAllies;

	public int m_extraAllyHealForSingleHit;

	public StandardEffectInfo m_extraEffectOnClosestAlly;

	[Header("-- Self-Healing")]
	public int m_healToCasterOnCast;

	public int m_healToCasterPerEnemyHit;

	public int m_healToCasterPerAllyHit;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_FishManStaticCone m_abilityMod;

	private FishMan_SyncComponent m_syncComp;

	private FishManCone m_damageConeAbility;

	private AreaEffectUtils.StretchConeStyle m_stretchConeStyle;

	private StandardEffectInfo m_cachedEffectToEnemies;

	private StandardEffectInfo m_cachedEffectToAllies;

	private StandardEffectInfo m_cachedExtraEffectOnClosestAlly;

	private void Start()
	{
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		if (this.m_syncComp == null)
		{
			this.m_syncComp = base.GetComponent<FishMan_SyncComponent>();
		}
		if (this.m_damageConeAbility == null)
		{
			AbilityData component = base.GetComponent<AbilityData>();
			if (component != null)
			{
				this.m_damageConeAbility = (component.GetAbilityOfType(typeof(FishManCone)) as FishManCone);
			}
		}
		base.Targeter = new AbilityUtil_Targeter_StretchCone(this, this.GetConeLength(), this.GetConeLength(), this.GetConeWidthAngleMin(), this.GetConeWidthAngle(), this.m_stretchConeStyle, this.GetConeBackwardOffset(), this.PenetrateLineOfSight())
		{
			m_includeEnemies = this.AffectsEnemies(),
			m_includeAllies = this.AffectsAllies(),
			m_includeCaster = this.AffectsCaster(),
			m_interpMinDistOverride = this.m_stretchInterpMinDist,
			m_interpRangeOverride = this.m_stretchInterpRange,
			m_discreteWidthAngleChange = this.m_useDiscreteAngleChange,
			m_numDiscreteWidthChanges = this.GetMaxHealingToAllies() - this.GetHealingToAllies()
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetConeLength();
	}

	private bool AffectsEnemies()
	{
		bool result;
		if (this.GetDamageToEnemies() <= 0)
		{
			result = this.GetEffectToEnemies().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool AffectsAllies()
	{
		bool result;
		if (this.GetHealingToAllies() <= 0)
		{
			result = this.GetEffectToAllies().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool AffectsCaster()
	{
		bool result;
		if (this.GetHealToCasterOnCast() <= 0 && this.GetHealToCasterPerAllyHit() <= 0)
		{
			result = (this.GetHealToCasterPerEnemyHit() > 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectToEnemies;
		if (this.m_abilityMod)
		{
			cachedEffectToEnemies = this.m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(this.m_effectToEnemies);
		}
		else
		{
			cachedEffectToEnemies = this.m_effectToEnemies;
		}
		this.m_cachedEffectToEnemies = cachedEffectToEnemies;
		StandardEffectInfo cachedEffectToAllies;
		if (this.m_abilityMod)
		{
			cachedEffectToAllies = this.m_abilityMod.m_effectToAlliesMod.GetModifiedValue(this.m_effectToAllies);
		}
		else
		{
			cachedEffectToAllies = this.m_effectToAllies;
		}
		this.m_cachedEffectToAllies = cachedEffectToAllies;
		StandardEffectInfo cachedExtraEffectOnClosestAlly;
		if (this.m_abilityMod)
		{
			cachedExtraEffectOnClosestAlly = this.m_abilityMod.m_extraEffectOnClosestAllyMod.GetModifiedValue(this.m_extraEffectOnClosestAlly);
		}
		else
		{
			cachedExtraEffectOnClosestAlly = this.m_extraEffectOnClosestAlly;
		}
		this.m_cachedExtraEffectOnClosestAlly = cachedExtraEffectOnClosestAlly;
	}

	public float GetConeWidthAngle()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		else
		{
			result = this.m_coneWidthAngle;
		}
		return result;
	}

	public float GetConeWidthAngleMin()
	{
		return (!this.m_abilityMod) ? this.m_coneWidthAngleMin : this.m_abilityMod.m_coneWidthAngleMinMod.GetModifiedValue(this.m_coneWidthAngleMin);
	}

	public float GetConeLength()
	{
		return (!this.m_abilityMod) ? this.m_coneLength : this.m_abilityMod.m_coneLengthMod.GetModifiedValue(this.m_coneLength);
	}

	public float GetConeBackwardOffset()
	{
		return (!this.m_abilityMod) ? this.m_coneBackwardOffset : this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		else
		{
			result = this.m_penetrateLineOfSight;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public int GetDamageToEnemies()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageToEnemiesMod.GetModifiedValue(this.m_damageToEnemies);
		}
		else
		{
			result = this.m_damageToEnemies;
		}
		return result;
	}

	public int GetMaxDamageToEnemies()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageToEnemiesMaxMod.GetModifiedValue(this.m_damageToEnemiesMax);
		}
		else
		{
			result = this.m_damageToEnemiesMax;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToEnemies()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectToEnemies != null)
		{
			result = this.m_cachedEffectToEnemies;
		}
		else
		{
			result = this.m_effectToEnemies;
		}
		return result;
	}

	public int GetHealingToAllies()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healingToAlliesMod.GetModifiedValue(this.m_healingToAllies);
		}
		else
		{
			result = this.m_healingToAllies;
		}
		return result;
	}

	public int GetMaxHealingToAllies()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healingToAlliesMaxMod.GetModifiedValue(this.m_healingToAlliesMax);
		}
		else
		{
			result = this.m_healingToAlliesMax;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToAllies()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectToAllies != null)
		{
			result = this.m_cachedEffectToAllies;
		}
		else
		{
			result = this.m_effectToAllies;
		}
		return result;
	}

	public int GetExtraAllyHealForSingleHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraAllyHealForSingleHitMod.GetModifiedValue(this.m_extraAllyHealForSingleHit);
		}
		else
		{
			result = this.m_extraAllyHealForSingleHit;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectOnClosestAlly()
	{
		return (this.m_cachedExtraEffectOnClosestAlly == null) ? this.m_extraEffectOnClosestAlly : this.m_cachedExtraEffectOnClosestAlly;
	}

	public int GetHealToCasterOnCast()
	{
		return (!this.m_abilityMod) ? this.m_healToCasterOnCast : this.m_abilityMod.m_healToCasterOnCastMod.GetModifiedValue(this.m_healToCasterOnCast);
	}

	public int GetHealToCasterPerEnemyHit()
	{
		return (!this.m_abilityMod) ? this.m_healToCasterPerEnemyHit : this.m_abilityMod.m_healToCasterPerEnemyHitMod.GetModifiedValue(this.m_healToCasterPerEnemyHit);
	}

	public int GetHealToCasterPerAllyHit()
	{
		return (!this.m_abilityMod) ? this.m_healToCasterPerAllyHit : this.m_abilityMod.m_healToCasterPerAllyHitMod.GetModifiedValue(this.m_healToCasterPerAllyHit);
	}

	public int GetExtraAllyHealFromBasicAttack()
	{
		if (this.m_damageConeAbility != null)
		{
			if (this.m_syncComp != null)
			{
				if ((int)this.m_syncComp.m_lastBasicAttackEnemyHitCount > 0)
				{
					if (this.m_damageConeAbility.GetExtraHealPerEnemyHitForNextHealCone() > 0)
					{
						return (int)this.m_syncComp.m_lastBasicAttackEnemyHitCount * this.m_damageConeAbility.GetExtraHealPerEnemyHitForNextHealCone();
					}
				}
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManStaticCone abilityMod_FishManStaticCone = modAsBase as AbilityMod_FishManStaticCone;
		base.AddTokenInt(tokens, "DamageToEnemies", string.Empty, (!abilityMod_FishManStaticCone) ? this.m_damageToEnemies : abilityMod_FishManStaticCone.m_damageToEnemiesMod.GetModifiedValue(this.m_damageToEnemies), false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManStaticCone) ? this.m_effectToEnemies : abilityMod_FishManStaticCone.m_effectToEnemiesMod.GetModifiedValue(this.m_effectToEnemies), "EffectToEnemies", this.m_effectToEnemies, true);
		string name = "HealingToAllies";
		string empty = string.Empty;
		int val;
		if (abilityMod_FishManStaticCone)
		{
			val = abilityMod_FishManStaticCone.m_healingToAlliesMod.GetModifiedValue(this.m_healingToAllies);
		}
		else
		{
			val = this.m_healingToAllies;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "HealingToAlliesMax", string.Empty, (!abilityMod_FishManStaticCone) ? this.m_healingToAlliesMax : abilityMod_FishManStaticCone.m_healingToAlliesMaxMod.GetModifiedValue(this.m_healingToAlliesMax), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_FishManStaticCone)
		{
			effectInfo = abilityMod_FishManStaticCone.m_effectToAlliesMod.GetModifiedValue(this.m_effectToAllies);
		}
		else
		{
			effectInfo = this.m_effectToAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectToAllies", this.m_effectToAllies, true);
		string name2 = "ExtraAllyHealForSingleHit";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_FishManStaticCone)
		{
			val2 = abilityMod_FishManStaticCone.m_extraAllyHealForSingleHitMod.GetModifiedValue(this.m_extraAllyHealForSingleHit);
		}
		else
		{
			val2 = this.m_extraAllyHealForSingleHit;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManStaticCone) ? this.m_extraEffectOnClosestAlly : abilityMod_FishManStaticCone.m_extraEffectOnClosestAllyMod.GetModifiedValue(this.m_extraEffectOnClosestAlly), "ExtraEffectOnClosestAlly", this.m_extraEffectOnClosestAlly, true);
		string name3 = "MaxTargets";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_FishManStaticCone)
		{
			val3 = abilityMod_FishManStaticCone.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			val3 = this.m_maxTargets;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		base.AddTokenInt(tokens, "HealToCasterOnCast", string.Empty, (!abilityMod_FishManStaticCone) ? this.m_healToCasterOnCast : abilityMod_FishManStaticCone.m_healToCasterOnCastMod.GetModifiedValue(this.m_healToCasterOnCast), false);
		string name4 = "HealToCasterPerEnemyHit";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_FishManStaticCone)
		{
			val4 = abilityMod_FishManStaticCone.m_healToCasterPerEnemyHitMod.GetModifiedValue(this.m_healToCasterPerEnemyHit);
		}
		else
		{
			val4 = this.m_healToCasterPerEnemyHit;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		string name5 = "HealToCasterPerAllyHit";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_FishManStaticCone)
		{
			val5 = abilityMod_FishManStaticCone.m_healToCasterPerAllyHitMod.GetModifiedValue(this.m_healToCasterPerAllyHit);
		}
		else
		{
			val5 = this.m_healToCasterPerAllyHit;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageToEnemies());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetHealingToAllies());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetHealToCasterOnCast() + this.GetHealToCasterPerEnemyHit() + this.GetHealToCasterPerAllyHit());
		this.GetEffectToAllies().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.GetEffectToEnemies().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = base.Targeter as AbilityUtil_Targeter_StretchCone;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				int visibleActorsCountByTooltipSubject = base.Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int visibleActorsCountByTooltipSubject2 = base.Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				int num = this.GetHealToCasterOnCast() + this.GetHealToCasterPerEnemyHit() * visibleActorsCountByTooltipSubject + this.GetHealToCasterPerAllyHit() * visibleActorsCountByTooltipSubject2;
				dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt((float)num);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				int damageForSweepAngle = this.GetDamageForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				dictionary[AbilityTooltipSymbol.Damage] = damageForSweepAngle;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
			{
				int visibleActorsCountByTooltipSubject3 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				int num2 = this.GetHealingForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				num2 += this.GetExtraAllyHealFromBasicAttack();
				if (visibleActorsCountByTooltipSubject3 == 1)
				{
					num2 += this.GetExtraAllyHealForSingleHit();
				}
				dictionary[AbilityTooltipSymbol.Healing] = num2;
			}
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManStaticCone))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_FishManStaticCone);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	private int GetDamageForSweepAngle(float sweepAngle)
	{
		float num = (float)(this.GetMaxDamageToEnemies() - this.GetDamageToEnemies());
		float num2 = this.GetConeWidthAngle() - this.GetConeWidthAngleMin();
		float num3;
		if (num2 > 0f)
		{
			num3 = 1f - (sweepAngle - this.GetConeWidthAngleMin()) / num2;
		}
		else
		{
			num3 = 1f;
		}
		float num4 = num3;
		num4 = Mathf.Clamp(num4, 0f, 1f);
		return this.GetDamageToEnemies() + Mathf.RoundToInt(num * num4);
	}

	private int GetHealingForSweepAngle(float sweepAngle)
	{
		float num = (float)(this.GetMaxHealingToAllies() - this.GetHealingToAllies());
		float num2 = this.GetConeWidthAngle() - this.GetConeWidthAngleMin();
		float num3 = (num2 <= 0f) ? 1f : (1f - (sweepAngle - this.GetConeWidthAngleMin()) / num2);
		num3 = Mathf.Clamp(num3, 0f, 1f);
		return this.GetHealingToAllies() + Mathf.RoundToInt(num * num3);
	}
}
