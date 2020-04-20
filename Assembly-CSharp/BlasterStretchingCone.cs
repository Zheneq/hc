using System;
using System.Collections.Generic;
using UnityEngine;

public class BlasterStretchingCone : Ability
{
	[Header("-- Cone Limits")]
	public float m_minLength;

	public float m_maxLength;

	public float m_minAngle;

	public float m_maxAngle;

	public AreaEffectUtils.StretchConeStyle m_stretchStyle = AreaEffectUtils.StretchConeStyle.DistanceSquared;

	public float m_coneBackwardOffset;

	public bool m_penetrateLineOfSight;

	[Header("-- On Hit")]
	public int m_damageAmountNormal;

	public int m_extraDamageForSingleHit;

	public bool m_removeOverchargeEffectOnCast;

	[Header("-- Damage scaling by distance from enemy")]
	public float m_extraDamagePerSquareDistanceFromEnemy;

	[Header("-- Damage Change by Angle and distance")]
	public BlasterStretchingCone.DamageChangeMode m_angleDamageChangeMode;

	public int m_anglesPerDamageChange;

	public BlasterStretchingCone.DamageChangeMode m_distDamageChangeMode = BlasterStretchingCone.DamageChangeMode.IncreaseFromMax;

	public float m_distPerDamageChange;

	public int m_maxDamageChange;

	[Header("-- Effects On Hit")]
	public StandardEffectInfo m_normalEnemyEffect;

	public StandardEffectInfo m_overchargedEnemyEffect;

	public StandardEffectInfo m_singleEnemyHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_overchargedCastSequencePrefab;

	private AbilityMod_BlasterStretchingCone m_abilityMod;

	private BlasterOvercharge m_overchargeAbility;

	private BlasterDashAndBlast m_dashAndBlastAbility;

	private Blaster_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedNormalEnemyEffect;

	private StandardEffectInfo m_cachedOverchargedEnemyEffect;

	private StandardEffectInfo m_cachedSingleEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.Start()).MethodHandle;
			}
			this.m_abilityName = "Stretching Cone";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Blaster_SyncComponent>();
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.Setup()).MethodHandle;
			}
			this.m_overchargeAbility = (component.GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge);
			this.m_dashAndBlastAbility = (component.GetAbilityOfType(typeof(BlasterDashAndBlast)) as BlasterDashAndBlast);
		}
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_StretchCone(this, this.GetMinLength(), this.GetMaxLength(), this.GetMinAngle(), this.GetMaxAngle(), this.m_stretchStyle, this.GetConeBackwardOffset(), this.PenetrateLineOfSight());
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetMaxLength();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedNormalEnemyEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.SetCachedFields()).MethodHandle;
			}
			cachedNormalEnemyEffect = this.m_abilityMod.m_normalEnemyEffectMod.GetModifiedValue(this.m_normalEnemyEffect);
		}
		else
		{
			cachedNormalEnemyEffect = this.m_normalEnemyEffect;
		}
		this.m_cachedNormalEnemyEffect = cachedNormalEnemyEffect;
		this.m_cachedOverchargedEnemyEffect = ((!this.m_abilityMod) ? this.m_overchargedEnemyEffect : this.m_abilityMod.m_overchargedEnemyEffectMod.GetModifiedValue(this.m_overchargedEnemyEffect));
		StandardEffectInfo cachedSingleEnemyHitEffect;
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
			cachedSingleEnemyHitEffect = this.m_abilityMod.m_singleEnemyHitEffectMod.GetModifiedValue(this.m_singleEnemyHitEffect);
		}
		else
		{
			cachedSingleEnemyHitEffect = this.m_singleEnemyHitEffect;
		}
		this.m_cachedSingleEnemyHitEffect = cachedSingleEnemyHitEffect;
	}

	public float GetMinLength()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetMinLength()).MethodHandle;
			}
			result = this.m_abilityMod.m_minLengthMod.GetModifiedValue(this.m_minLength);
		}
		else
		{
			result = this.m_minLength;
		}
		return result;
	}

	public float GetMaxLength()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetMaxLength()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxLengthMod.GetModifiedValue(this.m_maxLength);
		}
		else
		{
			result = this.m_maxLength;
		}
		return result;
	}

	public float GetMinAngle()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetMinAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_minAngleMod.GetModifiedValue(this.m_minAngle);
		}
		else
		{
			result = this.m_minAngle;
		}
		return result;
	}

	public float GetMaxAngle()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetMaxAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxAngleMod.GetModifiedValue(this.m_maxAngle);
		}
		else
		{
			result = this.m_maxAngle;
		}
		return result;
	}

	public float GetConeBackwardOffset()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetConeBackwardOffset()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.PenetrateLineOfSight()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		else
		{
			result = this.m_penetrateLineOfSight;
		}
		return result;
	}

	public int GetDamageAmountNormal()
	{
		return (!this.m_abilityMod) ? this.m_damageAmountNormal : this.m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(this.m_damageAmountNormal);
	}

	public int GetExtraDamageForSingleHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetExtraDamageForSingleHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			result = this.m_extraDamageForSingleHit;
		}
		return result;
	}

	public float GetExtraDamagePerSquareDistanceFromEnemy()
	{
		float result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetExtraDamagePerSquareDistanceFromEnemy()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamagePerSquareDistanceFromEnemyMod.GetModifiedValue(this.m_extraDamagePerSquareDistanceFromEnemy);
		}
		else
		{
			result = this.m_extraDamagePerSquareDistanceFromEnemy;
		}
		return result;
	}

	public int GetAnglesPerDamageChange()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetAnglesPerDamageChange()).MethodHandle;
			}
			result = this.m_abilityMod.m_anglesPerDamageChangeMod.GetModifiedValue(this.m_anglesPerDamageChange);
		}
		else
		{
			result = this.m_anglesPerDamageChange;
		}
		return result;
	}

	public float GetDistPerDamageChange()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetDistPerDamageChange()).MethodHandle;
			}
			result = this.m_abilityMod.m_distPerDamageChangeMod.GetModifiedValue(this.m_distPerDamageChange);
		}
		else
		{
			result = this.m_distPerDamageChange;
		}
		return result;
	}

	public int GetMaxDamageChange()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetMaxDamageChange()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxDamageChangeMod.GetModifiedValue(this.m_maxDamageChange);
		}
		else
		{
			result = this.m_maxDamageChange;
		}
		return result;
	}

	public StandardEffectInfo GetNormalEnemyEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedNormalEnemyEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetNormalEnemyEffect()).MethodHandle;
			}
			result = this.m_cachedNormalEnemyEffect;
		}
		else
		{
			result = this.m_normalEnemyEffect;
		}
		return result;
	}

	public StandardEffectInfo GetOverchargedEnemyEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedOverchargedEnemyEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetOverchargedEnemyEffect()).MethodHandle;
			}
			result = this.m_cachedOverchargedEnemyEffect;
		}
		else
		{
			result = this.m_overchargedEnemyEffect;
		}
		return result;
	}

	public StandardEffectInfo GetSingleEnemyHitEffect()
	{
		return (this.m_cachedSingleEnemyHitEffect == null) ? this.m_singleEnemyHitEffect : this.m_cachedSingleEnemyHitEffect;
	}

	private bool AmOvercharged(ActorData caster)
	{
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.AmOvercharged(ActorData)).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Blaster_SyncComponent>();
		}
		return this.m_syncComp.m_overchargeBuffs > 0;
	}

	private int GetMultiStackOverchargeDamage()
	{
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetMultiStackOverchargeDamage()).MethodHandle;
			}
			if (this.m_syncComp.m_overchargeBuffs > 1)
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
				if (this.m_overchargeAbility != null && this.m_overchargeAbility.GetExtraDamageForMultiCast() > 0)
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
					return this.m_overchargeAbility.GetExtraDamageForMultiCast();
				}
			}
		}
		return 0;
	}

	public int GetCurrentModdedDamage()
	{
		if (this.AmOvercharged(base.ActorData))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetCurrentModdedDamage()).MethodHandle;
			}
			return this.GetDamageAmountNormal() + this.m_overchargeAbility.GetExtraDamage() + this.GetMultiStackOverchargeDamage();
		}
		return this.GetDamageAmountNormal();
	}

	public int GetExtraDamageFromAngle(float angleNow)
	{
		if (this.GetAnglesPerDamageChange() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetExtraDamageFromAngle(float)).MethodHandle;
			}
			int num;
			if (this.m_angleDamageChangeMode == BlasterStretchingCone.DamageChangeMode.IncreaseFromMin)
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
				num = Mathf.Max((int)(angleNow - this.GetMinAngle()), 0);
			}
			else
			{
				num = Mathf.Max((int)(this.GetMaxAngle() - angleNow), 0);
			}
			int num2 = num / this.GetAnglesPerDamageChange();
			if (this.GetMaxDamageChange() > 0)
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
				num2 = Mathf.Clamp(num2, 0, this.GetMaxDamageChange());
			}
			return num2;
		}
		return 0;
	}

	public int GetExtraDamageFromRadius(float radiusInSquares)
	{
		if (this.GetDistPerDamageChange() > 0.1f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetExtraDamageFromRadius(float)).MethodHandle;
			}
			float num;
			if (this.m_distDamageChangeMode == BlasterStretchingCone.DamageChangeMode.IncreaseFromMin)
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
				num = Mathf.Max(radiusInSquares - this.GetMinLength(), 0f);
			}
			else
			{
				num = Mathf.Max(this.GetMaxLength() - radiusInSquares, 0f);
			}
			int num2 = Mathf.RoundToInt(num / this.GetDistPerDamageChange());
			if (this.GetMaxDamageChange() > 0)
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
				num2 = Mathf.Clamp(num2, 0, this.GetMaxDamageChange());
			}
			return num2;
		}
		return 0;
	}

	public int GetExtraDamageForEnemy(ActorData caster, ActorData target)
	{
		if (this.GetExtraDamagePerSquareDistanceFromEnemy() > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetExtraDamageForEnemy(ActorData, ActorData)).MethodHandle;
			}
			float num = VectorUtils.HorizontalPlaneDistInSquares(caster.GetTravelBoardSquareWorldPosition(), target.GetTravelBoardSquareWorldPosition()) - 1.4f;
			return Mathf.RoundToInt(this.GetExtraDamagePerSquareDistanceFromEnemy() * num);
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterStretchingCone abilityMod_BlasterStretchingCone = modAsBase as AbilityMod_BlasterStretchingCone;
		base.AddTokenInt(tokens, "Damage", string.Empty, (!abilityMod_BlasterStretchingCone) ? this.m_damageAmountNormal : abilityMod_BlasterStretchingCone.m_damageAmountNormalMod.GetModifiedValue(this.m_damageAmountNormal), false);
		base.AddTokenFloat(tokens, "ExtraDamagePerSquareDistanceFromEnemy", string.Empty, this.m_extraDamagePerSquareDistanceFromEnemy, false);
		string name = "ExtraDamageForSingleHit";
		string empty = string.Empty;
		int val;
		if (abilityMod_BlasterStretchingCone)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_BlasterStretchingCone.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			val = this.m_extraDamageForSingleHit;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "AnglesPerDamageChange";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_BlasterStretchingCone)
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
			val2 = abilityMod_BlasterStretchingCone.m_anglesPerDamageChangeMod.GetModifiedValue(this.m_anglesPerDamageChange);
		}
		else
		{
			val2 = this.m_anglesPerDamageChange;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "MaxDamageChange";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_BlasterStretchingCone)
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
			val3 = abilityMod_BlasterStretchingCone.m_maxDamageChangeMod.GetModifiedValue(this.m_maxDamageChange);
		}
		else
		{
			val3 = this.m_maxDamageChange;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_BlasterStretchingCone) ? this.m_normalEnemyEffect : abilityMod_BlasterStretchingCone.m_normalEnemyEffectMod.GetModifiedValue(this.m_normalEnemyEffect), "NormalEnemyEffect", this.m_normalEnemyEffect, true);
		StandardEffectInfo effectInfo;
		if (abilityMod_BlasterStretchingCone)
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
			effectInfo = abilityMod_BlasterStretchingCone.m_overchargedEnemyEffectMod.GetModifiedValue(this.m_overchargedEnemyEffect);
		}
		else
		{
			effectInfo = this.m_overchargedEnemyEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "OverchargedEnemyEffect", this.m_overchargedEnemyEffect, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_BlasterStretchingCone)
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
			effectInfo2 = abilityMod_BlasterStretchingCone.m_singleEnemyHitEffectMod.GetModifiedValue(this.m_singleEnemyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_singleEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "SingleEnemyHitEffect", this.m_singleEnemyHitEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetCurrentModdedDamage());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = base.Targeter as AbilityUtil_Targeter_StretchCone;
			int num = 0;
			if (abilityUtil_Targeter_StretchCone != null)
			{
				num += this.GetExtraDamageFromAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				num += this.GetExtraDamageFromRadius(abilityUtil_Targeter_StretchCone.LastConeRadiusInSquares);
				num += this.GetExtraDamageForEnemy(base.ActorData, targetActor);
			}
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int num2 = this.GetCurrentModdedDamage() + num;
				if (visibleActorsCountByTooltipSubject == 1)
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
					num2 += this.GetExtraDamageForSingleHit();
				}
				dictionary[AbilityTooltipSymbol.Damage] = num2;
			}
		}
		return dictionary;
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = this.GetMinLength() * Board.Get().squareSize;
		max = this.GetMaxLength() * Board.Get().squareSize;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterStretchingCone))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_BlasterStretchingCone);
			this.Setup();
			if (this.m_dashAndBlastAbility != null)
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
				if (this.m_dashAndBlastAbility.m_useConeParamsFromPrimary)
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
					this.m_dashAndBlastAbility.OnPrimaryAttackModChange();
				}
			}
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
		if (this.m_dashAndBlastAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterStretchingCone.OnRemoveAbilityMod()).MethodHandle;
			}
			if (this.m_dashAndBlastAbility.m_useConeParamsFromPrimary)
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
				this.m_dashAndBlastAbility.OnPrimaryAttackModChange();
			}
		}
	}

	public enum DamageChangeMode
	{
		IncreaseFromMin,
		IncreaseFromMax
	}
}
