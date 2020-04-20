using System;
using System.Collections.Generic;
using UnityEngine;

public class MartyrHealOverTime : Ability
{
	[Header("-- Targeting --")]
	public bool m_canTargetAlly = true;

	public bool m_targetingPenetrateLos;

	public int m_healBase = 5;

	public int m_healPerCrystal = 3;

	[Header("  (( base effect data for healing, no need to specify healing here ))")]
	public StandardActorEffectData m_healEffectData;

	[Header("-- Extra healing if has Aoe on React effect")]
	public int m_extraHealingIfHasAoeOnReact;

	[Header("-- Extra Effect for low health --")]
	public bool m_onlyAddExtraEffecForFirstTurn;

	public float m_lowHealthThreshold;

	public StandardEffectInfo m_extraEffectForLowHealth;

	[Header("-- Heal/Effect on Caster if targeting Ally")]
	public int m_baseSelfHealIfTargetAlly;

	public int m_selfHealPerCrystalIfTargetAlly;

	public bool m_addHealEffectOnSelfIfTargetAlly;

	public StandardActorEffectData m_healEffectOnSelfIfTargetAlly;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private Martyr_SyncComponent m_syncComponent;

	private AbilityMod_MartyrHealOverTime m_abilityMod;

	private StandardActorEffectData m_cachedHealEffectData;

	private StandardEffectInfo m_cachedExtraEffectForLowHealth;

	private StandardActorEffectData m_cachedHealEffectOnSelfIfTargetAlly;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "MartyrHealOverTime";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_syncComponent == null)
		{
			this.m_syncComponent = base.GetComponent<Martyr_SyncComponent>();
		}
		this.SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsActor;
		if (this.HasSelfHitIfTargetingAlly())
		{
			affectsActor = AbilityUtil_Targeter.AffectsActor.Always;
		}
		else
		{
			affectsActor = AbilityUtil_Targeter.AffectsActor.Possible;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = affectsActor;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, this.CanTargetAlly(), affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedHealEffectData;
		if (this.m_abilityMod)
		{
			cachedHealEffectData = this.m_abilityMod.m_healEffectDataMod.GetModifiedValue(this.m_healEffectData);
		}
		else
		{
			cachedHealEffectData = this.m_healEffectData;
		}
		this.m_cachedHealEffectData = cachedHealEffectData;
		StandardEffectInfo cachedExtraEffectForLowHealth;
		if (this.m_abilityMod)
		{
			cachedExtraEffectForLowHealth = this.m_abilityMod.m_extraEffectForLowHealthMod.GetModifiedValue(this.m_extraEffectForLowHealth);
		}
		else
		{
			cachedExtraEffectForLowHealth = this.m_extraEffectForLowHealth;
		}
		this.m_cachedExtraEffectForLowHealth = cachedExtraEffectForLowHealth;
		this.m_cachedHealEffectOnSelfIfTargetAlly = ((!this.m_abilityMod) ? this.m_healEffectOnSelfIfTargetAlly : this.m_abilityMod.m_healEffectOnSelfIfTargetAllyMod.GetModifiedValue(this.m_healEffectOnSelfIfTargetAlly));
	}

	public bool CanTargetAlly()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canTargetAllyMod.GetModifiedValue(this.m_canTargetAlly);
		}
		else
		{
			result = this.m_canTargetAlly;
		}
		return result;
	}

	public bool TargetingPenetrateLos()
	{
		return (!this.m_abilityMod) ? this.m_targetingPenetrateLos : this.m_abilityMod.m_targetingPenetrateLosMod.GetModifiedValue(this.m_targetingPenetrateLos);
	}

	public StandardActorEffectData GetHealEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedHealEffectData != null)
		{
			result = this.m_cachedHealEffectData;
		}
		else
		{
			result = this.m_healEffectData;
		}
		return result;
	}

	public int GetHealBase()
	{
		return (!this.m_abilityMod) ? this.m_healBase : this.m_abilityMod.m_healBaseMod.GetModifiedValue(this.m_healBase);
	}

	public int GetHealPerCrystal()
	{
		return (!this.m_abilityMod) ? this.m_healPerCrystal : this.m_abilityMod.m_healPerCrystalMod.GetModifiedValue(this.m_healPerCrystal);
	}

	public int GetExtraHealingIfHasAoeOnReact()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraHealingIfHasAoeOnReactMod.GetModifiedValue(this.m_extraHealingIfHasAoeOnReact);
		}
		else
		{
			result = this.m_extraHealingIfHasAoeOnReact;
		}
		return result;
	}

	public bool OnlyAddExtraEffecForFirstTurn()
	{
		return (!this.m_abilityMod) ? this.m_onlyAddExtraEffecForFirstTurn : this.m_abilityMod.m_onlyAddExtraEffecForFirstTurnMod.GetModifiedValue(this.m_onlyAddExtraEffecForFirstTurn);
	}

	public float GetLowHealthThreshold()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_lowHealthThresholdMod.GetModifiedValue(this.m_lowHealthThreshold);
		}
		else
		{
			result = this.m_lowHealthThreshold;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectForLowHealth()
	{
		StandardEffectInfo result;
		if (this.m_cachedExtraEffectForLowHealth != null)
		{
			result = this.m_cachedExtraEffectForLowHealth;
		}
		else
		{
			result = this.m_extraEffectForLowHealth;
		}
		return result;
	}

	public int GetBaseSelfHealIfTargetAlly()
	{
		return (!this.m_abilityMod) ? this.m_baseSelfHealIfTargetAlly : this.m_abilityMod.m_baseSelfHealIfTargetAllyMod.GetModifiedValue(this.m_baseSelfHealIfTargetAlly);
	}

	public int GetSelfHealPerCrystalIfTargetAlly()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_selfHealPerCrystalIfTargetAllyMod.GetModifiedValue(this.m_selfHealPerCrystalIfTargetAlly);
		}
		else
		{
			result = this.m_selfHealPerCrystalIfTargetAlly;
		}
		return result;
	}

	public bool AddHealEffectOnSelfIfTargetAlly()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_addHealEffectOnSelfIfTargetAllyMod.GetModifiedValue(this.m_addHealEffectOnSelfIfTargetAlly);
		}
		else
		{
			result = this.m_addHealEffectOnSelfIfTargetAlly;
		}
		return result;
	}

	public StandardActorEffectData GetHealEffectOnSelfIfTargetAlly()
	{
		StandardActorEffectData result;
		if (this.m_cachedHealEffectOnSelfIfTargetAlly != null)
		{
			result = this.m_cachedHealEffectOnSelfIfTargetAlly;
		}
		else
		{
			result = this.m_healEffectOnSelfIfTargetAlly;
		}
		return result;
	}

	public int GetCurrentHealing(ActorData caster)
	{
		return this.GetHealBase() + this.GetHealPerCrystal() * this.m_syncComponent.SpentDamageCrystals(caster);
	}

	public int GetSelfHealingIfTargetingAlly(ActorData caster)
	{
		int num = this.GetBaseSelfHealIfTargetAlly();
		if (this.GetSelfHealPerCrystalIfTargetAlly() > 0)
		{
			num += this.GetSelfHealPerCrystalIfTargetAlly() * this.m_syncComponent.SpentDamageCrystals(caster);
		}
		return num;
	}

	public bool HasSelfHitIfTargetingAlly()
	{
		if (this.GetBaseSelfHealIfTargetAlly() <= 0)
		{
			if (this.GetSelfHealPerCrystalIfTargetAlly() <= 0)
			{
				return this.AddHealEffectOnSelfIfTargetAlly();
			}
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, false, this.CanTargetAlly(), true, Ability.ValidateCheckPath.Ignore, this.TargetingPenetrateLos(), true, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Primary, 1);
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Primary, 1);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			ActorData actorData = base.ActorData;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			if (actorData != null)
			{
				if (actorData == targetActor)
				{
					if (visibleActorsCountByTooltipSubject > 0)
					{
						int num = this.GetSelfHealingIfTargetingAlly(actorData);
						if (this.m_syncComponent != null)
						{
							if (this.m_syncComponent.ActorHasAoeOnReactEffect(targetActor))
							{
								if (this.GetExtraHealingIfHasAoeOnReact() > 0)
								{
									num += this.GetExtraHealingIfHasAoeOnReact();
								}
							}
						}
						dictionary[AbilityTooltipSymbol.Healing] = num;
						dictionary[AbilityTooltipSymbol.Absorb] = 0;
						return dictionary;
					}
				}
			}
			int num2 = this.GetCurrentHealing(actorData);
			if (this.m_syncComponent != null)
			{
				if (this.m_syncComponent.ActorHasAoeOnReactEffect(targetActor))
				{
					if (this.GetExtraHealingIfHasAoeOnReact() > 0)
					{
						num2 += this.GetExtraHealingIfHasAoeOnReact();
					}
				}
			}
			dictionary[AbilityTooltipSymbol.Healing] = num2;
			dictionary[AbilityTooltipSymbol.Absorb] = 0;
			if (this.GetLowHealthThreshold() > 0f)
			{
				if (targetActor.GetHitPointShareOfMax() <= this.GetLowHealthThreshold())
				{
					StandardEffectInfo extraEffectForLowHealth = this.GetExtraEffectForLowHealth();
					if (extraEffectForLowHealth.m_applyEffect)
					{
						dictionary[AbilityTooltipSymbol.Absorb] = extraEffectForLowHealth.m_effectData.m_absorbAmount;
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_MartyrHealOverTime abilityMod_MartyrHealOverTime = modAsBase as AbilityMod_MartyrHealOverTime;
		string name = "HealBase";
		string empty = string.Empty;
		int val;
		if (abilityMod_MartyrHealOverTime)
		{
			val = abilityMod_MartyrHealOverTime.m_healBaseMod.GetModifiedValue(this.m_healBase);
		}
		else
		{
			val = this.m_healBase;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "HealPerCrystal";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_MartyrHealOverTime)
		{
			val2 = abilityMod_MartyrHealOverTime.m_healPerCrystalMod.GetModifiedValue(this.m_healPerCrystal);
		}
		else
		{
			val2 = this.m_healPerCrystal;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardActorEffectData standardActorEffectData;
		if (abilityMod_MartyrHealOverTime)
		{
			standardActorEffectData = abilityMod_MartyrHealOverTime.m_healEffectDataMod.GetModifiedValue(this.m_healEffectData);
		}
		else
		{
			standardActorEffectData = this.m_healEffectData;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "HealEffectData", abilityMod_MartyrHealOverTime != null, this.m_healEffectData);
		base.AddTokenInt(tokens, "ExtraHealingIfHasAoeOnReact", string.Empty, (!abilityMod_MartyrHealOverTime) ? this.m_extraHealingIfHasAoeOnReact : abilityMod_MartyrHealOverTime.m_extraHealingIfHasAoeOnReactMod.GetModifiedValue(this.m_extraHealingIfHasAoeOnReact), false);
		base.AddTokenFloatAsPct(tokens, "LowHealthThreshold_Pct", string.Empty, (!abilityMod_MartyrHealOverTime) ? this.m_lowHealthThreshold : abilityMod_MartyrHealOverTime.m_lowHealthThresholdMod.GetModifiedValue(this.m_lowHealthThreshold), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_MartyrHealOverTime)
		{
			effectInfo = abilityMod_MartyrHealOverTime.m_extraEffectForLowHealthMod.GetModifiedValue(this.m_extraEffectForLowHealth);
		}
		else
		{
			effectInfo = this.m_extraEffectForLowHealth;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "ExtraEffectForLowHealth", this.m_extraEffectForLowHealth, true);
		base.AddTokenInt(tokens, "BaseSelfHealIfTargetAlly", string.Empty, (!abilityMod_MartyrHealOverTime) ? this.m_baseSelfHealIfTargetAlly : abilityMod_MartyrHealOverTime.m_baseSelfHealIfTargetAllyMod.GetModifiedValue(this.m_baseSelfHealIfTargetAlly), false);
		string name3 = "SelfHealPerCrystalIfTargetAlly";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_MartyrHealOverTime)
		{
			val3 = abilityMod_MartyrHealOverTime.m_selfHealPerCrystalIfTargetAllyMod.GetModifiedValue(this.m_selfHealPerCrystalIfTargetAlly);
		}
		else
		{
			val3 = this.m_selfHealPerCrystalIfTargetAlly;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		StandardActorEffectData standardActorEffectData3;
		if (abilityMod_MartyrHealOverTime)
		{
			standardActorEffectData3 = abilityMod_MartyrHealOverTime.m_healEffectOnSelfIfTargetAllyMod.GetModifiedValue(this.m_healEffectOnSelfIfTargetAlly);
		}
		else
		{
			standardActorEffectData3 = this.m_healEffectOnSelfIfTargetAlly;
		}
		StandardActorEffectData standardActorEffectData4 = standardActorEffectData3;
		standardActorEffectData4.AddTooltipTokens(tokens, "HealEffectOnSelfIfTargetAlly", abilityMod_MartyrHealOverTime != null, this.m_healEffectOnSelfIfTargetAlly);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrHealOverTime))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_MartyrHealOverTime);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
