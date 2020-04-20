using System;
using System.Collections.Generic;
using UnityEngine;

public class FishManCone : Ability
{
	[Header("-- Cone Data")]
	public FishManCone.ConeTargetingMode m_coneMode = FishManCone.ConeTargetingMode.MultiClick;

	public float m_coneWidthAngle = 60f;

	public float m_coneWidthAngleMin = 5f;

	public float m_coneLength = 5f;

	public float m_coneBackwardOffset;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets;

	public float m_multiClickConeEdgeWidth = 0.2f;

	[Header("-- (for stretch cone only)")]
	public bool m_useDiscreteAngleChange;

	public float m_stretchInterpMinDist = 2.5f;

	public float m_stretchInterpRange = 4f;

	[Header("-- On Hit Target")]
	public int m_damageToEnemies;

	public int m_damageToEnemiesMax;

	public StandardEffectInfo m_effectToEnemies;

	[Header("-- Ally Healing")]
	public int m_healingToAllies = 0xF;

	public int m_healingToAlliesMax = 0x1E;

	public StandardEffectInfo m_effectToAllies;

	[Header("-- Self-Healing")]
	public int m_healToCasterOnCast;

	public int m_healToCasterPerEnemyHit;

	public int m_healToCasterPerAllyHit;

	[Header("-- Bonus Healing on Heal Cone ability")]
	public int m_extraHealPerEnemyHitForNextHealCone;

	[Header("-- Extra Energy")]
	public int m_extraEnergyForSingleEnemyHit;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_FishManCone m_abilityMod;

	private FishMan_SyncComponent m_syncComp;

	private AreaEffectUtils.StretchConeStyle m_stretchConeStyle;

	private StandardEffectInfo m_cachedEffectToEnemies;

	private StandardEffectInfo m_cachedEffectToAllies;

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
		if (this.m_coneMode == FishManCone.ConeTargetingMode.MultiClick)
		{
			base.ClearTargeters();
			for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
			{
				AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone = new AbilityUtil_Targeter_SweepMultiClickCone(this, this.GetConeWidthAngleMin(), this.GetConeWidthAngle(), this.GetConeLength(), this.GetConeBackwardOffset(), this.m_multiClickConeEdgeWidth, this.PenetrateLineOfSight(), this.GetMaxTargets());
				abilityUtil_Targeter_SweepMultiClickCone.SetAffectedGroups(this.AffectsEnemies(), this.AffectsAllies(), this.AffectsCaster());
				base.Targeters.Add(abilityUtil_Targeter_SweepMultiClickCone);
			}
		}
		else if (this.m_coneMode == FishManCone.ConeTargetingMode.Stretch)
		{
			base.Targeter = new AbilityUtil_Targeter_StretchCone(this, this.GetConeLength(), this.GetConeLength(), this.GetConeWidthAngleMin(), this.GetConeWidthAngle(), this.m_stretchConeStyle, this.GetConeBackwardOffset(), this.PenetrateLineOfSight())
			{
				m_includeEnemies = this.AffectsEnemies(),
				m_includeAllies = this.AffectsAllies(),
				m_includeCaster = this.AffectsCaster(),
				m_interpMinDistOverride = this.m_stretchInterpMinDist,
				m_interpRangeOverride = this.m_stretchInterpRange,
				m_discreteWidthAngleChange = this.m_useDiscreteAngleChange,
				m_numDiscreteWidthChanges = this.GetMaxDamageToEnemies() - this.GetDamageToEnemies()
			};
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, this.GetConeWidthAngle(), this.GetConeLength(), this.GetConeBackwardOffset(), this.PenetrateLineOfSight(), true, this.AffectsEnemies(), this.AffectsAllies(), this.AffectsCaster(), this.GetMaxTargets(), false);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result;
		if (this.m_coneMode == FishManCone.ConeTargetingMode.MultiClick)
		{
			result = 2;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetConeLength();
	}

	public unsafe override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (this.m_coneMode == FishManCone.ConeTargetingMode.Stretch)
		{
			min = this.m_stretchInterpMinDist * Board.Get().squareSize;
			max = (this.m_stretchInterpMinDist + this.m_stretchInterpRange) * Board.Get().squareSize;
			return true;
		}
		return base.HasRestrictedFreeAimDegrees(aimingActor, targetIndex, targetsSoFar, out min, out max);
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
		return this.GetHealingToAllies() > 0 || this.GetEffectToAllies().m_applyEffect;
	}

	private bool AffectsCaster()
	{
		return this.GetHealToCasterOnCast() > 0 || this.GetHealToCasterPerAllyHit() > 0 || this.GetHealToCasterPerEnemyHit() > 0;
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
		this.m_cachedEffectToAllies = ((!this.m_abilityMod) ? this.m_effectToAllies : this.m_abilityMod.m_effectToAlliesMod.GetModifiedValue(this.m_effectToAllies));
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
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneWidthAngleMinMod.GetModifiedValue(this.m_coneWidthAngleMin);
		}
		else
		{
			result = this.m_coneWidthAngleMin;
		}
		return result;
	}

	public float GetConeLength()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneLengthMod.GetModifiedValue(this.m_coneLength);
		}
		else
		{
			result = this.m_coneLength;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if (this.m_abilityMod)
		{
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
		return (!this.m_abilityMod) ? this.m_damageToEnemies : this.m_abilityMod.m_damageToEnemiesMod.GetModifiedValue(this.m_damageToEnemies);
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
		return (this.m_cachedEffectToEnemies == null) ? this.m_effectToEnemies : this.m_cachedEffectToEnemies;
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

	public int GetHealToCasterOnCast()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healToCasterOnCastMod.GetModifiedValue(this.m_healToCasterOnCast);
		}
		else
		{
			result = this.m_healToCasterOnCast;
		}
		return result;
	}

	public int GetHealToCasterPerEnemyHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healToCasterPerEnemyHitMod.GetModifiedValue(this.m_healToCasterPerEnemyHit);
		}
		else
		{
			result = this.m_healToCasterPerEnemyHit;
		}
		return result;
	}

	public int GetHealToCasterPerAllyHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healToCasterPerAllyHitMod.GetModifiedValue(this.m_healToCasterPerAllyHit);
		}
		else
		{
			result = this.m_healToCasterPerAllyHit;
		}
		return result;
	}

	public int GetExtraHealPerEnemyHitForNextHealCone()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraHealPerEnemyHitForNextHealConeMod.GetModifiedValue(this.m_extraHealPerEnemyHitForNextHealCone);
		}
		else
		{
			result = this.m_extraHealPerEnemyHitForNextHealCone;
		}
		return result;
	}

	public int GetExtraEnergyForSingleEnemyHit()
	{
		return (!this.m_abilityMod) ? this.m_extraEnergyForSingleEnemyHit : this.m_abilityMod.m_extraEnergyForSingleEnemyHitMod.GetModifiedValue(this.m_extraEnergyForSingleEnemyHit);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManCone))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_FishManCone);
			this.Setup();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManCone abilityMod_FishManCone = modAsBase as AbilityMod_FishManCone;
		string name = "DamageToEnemies";
		string empty = string.Empty;
		int val;
		if (abilityMod_FishManCone)
		{
			val = abilityMod_FishManCone.m_damageToEnemiesMod.GetModifiedValue(this.m_damageToEnemies);
		}
		else
		{
			val = this.m_damageToEnemies;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "DamageToEnemiesMax";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_FishManCone)
		{
			val2 = abilityMod_FishManCone.m_damageToEnemiesMaxMod.GetModifiedValue(this.m_damageToEnemiesMax);
		}
		else
		{
			val2 = this.m_damageToEnemiesMax;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_FishManCone)
		{
			effectInfo = abilityMod_FishManCone.m_effectToEnemiesMod.GetModifiedValue(this.m_effectToEnemies);
		}
		else
		{
			effectInfo = this.m_effectToEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectToEnemies", this.m_effectToEnemies, true);
		string name3 = "HealingToAllies";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_FishManCone)
		{
			val3 = abilityMod_FishManCone.m_healingToAlliesMod.GetModifiedValue(this.m_healingToAllies);
		}
		else
		{
			val3 = this.m_healingToAllies;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_FishManCone)
		{
			effectInfo2 = abilityMod_FishManCone.m_effectToAlliesMod.GetModifiedValue(this.m_effectToAllies);
		}
		else
		{
			effectInfo2 = this.m_effectToAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectToAllies", this.m_effectToAllies, true);
		string name4 = "MaxTargets";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_FishManCone)
		{
			val4 = abilityMod_FishManCone.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			val4 = this.m_maxTargets;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		string name5 = "HealToCasterOnCast";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_FishManCone)
		{
			val5 = abilityMod_FishManCone.m_healToCasterOnCastMod.GetModifiedValue(this.m_healToCasterOnCast);
		}
		else
		{
			val5 = this.m_healToCasterOnCast;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		string name6 = "HealToCasterPerEnemyHit";
		string empty6 = string.Empty;
		int val6;
		if (abilityMod_FishManCone)
		{
			val6 = abilityMod_FishManCone.m_healToCasterPerEnemyHitMod.GetModifiedValue(this.m_healToCasterPerEnemyHit);
		}
		else
		{
			val6 = this.m_healToCasterPerEnemyHit;
		}
		base.AddTokenInt(tokens, name6, empty6, val6, false);
		string name7 = "HealToCasterPerAllyHit";
		string empty7 = string.Empty;
		int val7;
		if (abilityMod_FishManCone)
		{
			val7 = abilityMod_FishManCone.m_healToCasterPerAllyHitMod.GetModifiedValue(this.m_healToCasterPerAllyHit);
		}
		else
		{
			val7 = this.m_healToCasterPerAllyHit;
		}
		base.AddTokenInt(tokens, name7, empty7, val7, false);
		string name8 = "ExtraHealPerEnemyHitForNextHealCone";
		string empty8 = string.Empty;
		int val8;
		if (abilityMod_FishManCone)
		{
			val8 = abilityMod_FishManCone.m_extraHealPerEnemyHitForNextHealConeMod.GetModifiedValue(this.m_extraHealPerEnemyHitForNextHealCone);
		}
		else
		{
			val8 = this.m_extraHealPerEnemyHitForNextHealCone;
		}
		base.AddTokenInt(tokens, name8, empty8, val8, false);
		string name9 = "ExtraEnergyForSingleEnemyHit";
		string empty9 = string.Empty;
		int val9;
		if (abilityMod_FishManCone)
		{
			val9 = abilityMod_FishManCone.m_extraEnergyForSingleEnemyHitMod.GetModifiedValue(this.m_extraEnergyForSingleEnemyHit);
		}
		else
		{
			val9 = this.m_extraEnergyForSingleEnemyHit;
		}
		base.AddTokenInt(tokens, name9, empty9, val9, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.GetDamageToEnemies()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, this.GetHealingToAllies())
		};
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageToEnemies());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetHealingToAllies());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetHealToCasterOnCast() + this.GetHealToCasterPerEnemyHit() + this.GetHealToCasterPerAllyHit());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (currentTargeterIndex <= 0)
		{
			if (this.m_coneMode == FishManCone.ConeTargetingMode.MultiClick)
			{
				return dictionary;
			}
		}
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
				int value = this.GetDamageToEnemies();
				if (this.m_coneMode == FishManCone.ConeTargetingMode.MultiClick)
				{
					AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SweepMultiClickCone;
					value = this.GetDamageForSweepAngle(abilityUtil_Targeter_SweepMultiClickCone.sweepAngle);
				}
				else if (this.m_coneMode == FishManCone.ConeTargetingMode.Stretch)
				{
					if (abilityUtil_Targeter_StretchCone != null)
					{
						value = this.GetDamageForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
					}
				}
				dictionary[AbilityTooltipSymbol.Damage] = value;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
			{
				int value2 = this.GetHealingToAllies();
				if (this.m_coneMode == FishManCone.ConeTargetingMode.MultiClick)
				{
					AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone2 = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SweepMultiClickCone;
					value2 = this.GetHealingForSweepAngle(abilityUtil_Targeter_SweepMultiClickCone2.sweepAngle);
				}
				else if (this.m_coneMode == FishManCone.ConeTargetingMode.Stretch && abilityUtil_Targeter_StretchCone != null)
				{
					value2 = this.GetHealingForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				}
				dictionary[AbilityTooltipSymbol.Healing] = value2;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.GetExtraEnergyForSingleEnemyHit() > 0 && (currentTargeterIndex > 0 || this.m_coneMode != FishManCone.ConeTargetingMode.MultiClick))
		{
			AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[currentTargeterIndex];
			if (abilityUtil_Targeter != null)
			{
				int visibleActorsCountByTooltipSubject = abilityUtil_Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				if (visibleActorsCountByTooltipSubject == 1)
				{
					return this.GetExtraEnergyForSingleEnemyHit();
				}
			}
		}
		return 0;
	}

	private unsafe Vector3 GetTargeterClampedAimDirection(Vector3 startAimDirection, Vector3 endAimDirection, out float sweepAngle, out float coneCenterDegrees)
	{
		float num = VectorUtils.HorizontalAngle_Deg(startAimDirection);
		sweepAngle = Vector3.Angle(startAimDirection, endAimDirection);
		float coneWidthAngle = this.GetConeWidthAngle();
		float coneWidthAngleMin = this.GetConeWidthAngleMin();
		if (coneWidthAngle > 0f)
		{
			if (sweepAngle > coneWidthAngle)
			{
				endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, 0.0174532924f * (sweepAngle - coneWidthAngle), 0f);
				sweepAngle = coneWidthAngle;
				goto IL_A3;
			}
		}
		if (coneWidthAngleMin > 0f && sweepAngle < coneWidthAngleMin)
		{
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, 0.0174532924f * (sweepAngle - coneWidthAngleMin), 0f);
			sweepAngle = coneWidthAngleMin;
		}
		IL_A3:
		coneCenterDegrees = num;
		if (Vector3.Cross(startAimDirection, endAimDirection).y > 0f)
		{
			coneCenterDegrees -= sweepAngle * 0.5f;
		}
		else
		{
			coneCenterDegrees += sweepAngle * 0.5f;
		}
		return endAimDirection;
	}

	public override Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		if (this.m_coneMode == FishManCone.ConeTargetingMode.MultiClick)
		{
			float coneWidthAngleMin = this.GetConeWidthAngleMin();
			float num = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
			float angle = num;
			if (targets.Count > 1)
			{
				this.GetTargeterClampedAimDirection(targets[0].AimDirection, targets[targets.Count - 1].AimDirection, out coneWidthAngleMin, out angle);
			}
			return caster.GetTravelBoardSquareWorldPosition() + VectorUtils.AngleDegreesToVector(angle);
		}
		return base.GetRotateToTargetPos(targets, caster);
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
		return this.GetHealingToAllies() + Mathf.RoundToInt(num * num4);
	}

	public enum ConeTargetingMode
	{
		Static,
		MultiClick,
		Stretch
	}
}
