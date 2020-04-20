using System;
using System.Collections.Generic;
using UnityEngine;

public class ClericAreaBuff : Ability
{
	[Separator("Targeting", true)]
	public AbilityAreaShape m_shape;

	public bool m_penetrateLoS;

	public bool m_includeEnemies;

	public bool m_includeAllies;

	public bool m_includeCaster;

	public Color m_iconColorWhileActive = Color.cyan;

	[Separator("Misc - Energy, Cooldown, Animation", true)]
	public int m_extraTpCostPerTurnActive = 5;

	public int m_cooldownWhenBuffLapses = 2;

	[Separator("On Hit Heal/Damage/Effect", true)]
	public int m_effectDuration = 2;

	public int m_healAmount;

	public StandardEffectInfo m_effectOnCaster;

	public StandardEffectInfo m_effectOnAllies;

	[Header("-- Shielding on self override, if >= 0")]
	public int m_selfShieldingOverride = -1;

	public StandardEffectInfo m_effectOnEnemies;

	[Separator("Vision on Target Square", true)]
	public bool m_addVisionOnTargetSquare;

	public float m_visionRadius = 1.5f;

	public int m_visionDuration = 1;

	public VisionProviderInfo.BrushRevealType m_brushRevealType = VisionProviderInfo.BrushRevealType.Always;

	public bool m_visionAreaIgnoreLos = true;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_persistentSequencePrefab;

	public GameObject m_pulseSequencePrefab;

	public GameObject m_toggleOffSequencePrefab;

	private AbilityData.ActionType m_buffActionType;

	private Cleric_SyncComponent m_syncComp;

	private AbilityMod_ClericAreaBuff m_abilityMod;

	private StandardEffectInfo m_cachedEffectOnCaster;

	private StandardEffectInfo m_cachedEffectOnAllies;

	private StandardEffectInfo m_cachedFirstTurnEffectOnAllies;

	private StandardEffectInfo m_cachedEffectOnEnemies;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Area Buff";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		AbilityAreaShape shape = this.GetShape();
		bool penetrateLoS = this.PenetrateLoS();
		AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
		bool affectsEnemies = this.IncludeEnemies();
		bool affectsAllies = this.IncludeAllies();
		AbilityUtil_Targeter.AffectsActor affectsCaster;
		if (this.IncludeCaster())
		{
			affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		}
		else
		{
			affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		}
		base.Targeter = new AbilityUtil_Targeter_ClericAreaBuff(this, shape, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.SetShowArcToShape(false);
		this.m_syncComp = base.GetComponent<Cleric_SyncComponent>();
		this.m_buffActionType = base.GetActionTypeOfAbility(this);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericAreaBuff))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ClericAreaBuff);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnCaster;
		if (this.m_abilityMod)
		{
			cachedEffectOnCaster = this.m_abilityMod.m_effectOnCasterMod.GetModifiedValue(this.m_effectOnCaster);
		}
		else
		{
			cachedEffectOnCaster = this.m_effectOnCaster;
		}
		this.m_cachedEffectOnCaster = cachedEffectOnCaster;
		this.m_cachedEffectOnAllies = ((!this.m_abilityMod) ? this.m_effectOnAllies : this.m_abilityMod.m_effectOnAlliesMod.GetModifiedValue(this.m_effectOnAllies));
		this.m_cachedFirstTurnEffectOnAllies = ((!this.m_abilityMod) ? this.m_effectOnAllies : this.m_abilityMod.m_firstTurnOnlyEffectOnAlliesMod.GetModifiedValue(this.m_effectOnAllies));
		StandardEffectInfo cachedEffectOnEnemies;
		if (this.m_abilityMod)
		{
			cachedEffectOnEnemies = this.m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(this.m_effectOnEnemies);
		}
		else
		{
			cachedEffectOnEnemies = this.m_effectOnEnemies;
		}
		this.m_cachedEffectOnEnemies = cachedEffectOnEnemies;
	}

	public AbilityAreaShape GetShape()
	{
		return (!this.m_abilityMod) ? this.m_shape : this.m_abilityMod.m_shapeMod.GetModifiedValue(this.m_shape);
	}

	public bool PenetrateLoS()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_penetrateLoSMod.GetModifiedValue(this.m_penetrateLoS);
		}
		else
		{
			result = this.m_penetrateLoS;
		}
		return result;
	}

	public bool IncludeEnemies()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_includeEnemiesMod.GetModifiedValue(this.m_includeEnemies);
		}
		else
		{
			result = this.m_includeEnemies;
		}
		return result;
	}

	public bool IncludeAllies()
	{
		return (!this.m_abilityMod) ? this.m_includeAllies : this.m_abilityMod.m_includeAlliesMod.GetModifiedValue(this.m_includeAllies);
	}

	public bool IncludeCaster()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_includeCasterMod.GetModifiedValue(this.m_includeCaster);
		}
		else
		{
			result = this.m_includeCaster;
		}
		return result;
	}

	public int GetExtraTpCostPerTurnActive()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraTpCostPerTurnActiveMod.GetModifiedValue(this.m_extraTpCostPerTurnActive);
		}
		else
		{
			result = this.m_extraTpCostPerTurnActive;
		}
		return result;
	}

	public int GetCooldownWhenBuffLapses()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_cooldownWhenBuffLapsesMod.GetModifiedValue(this.m_cooldownWhenBuffLapses);
		}
		else
		{
			result = this.m_cooldownWhenBuffLapses;
		}
		return result;
	}

	public int GetEffectDuration()
	{
		return (!this.m_abilityMod) ? this.m_effectDuration : this.m_abilityMod.m_effectDurationMod.GetModifiedValue(this.m_effectDuration);
	}

	public int GetHealAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healAmountMod.GetModifiedValue(this.m_healAmount);
		}
		else
		{
			result = this.m_healAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnCaster()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnCaster != null)
		{
			result = this.m_cachedEffectOnCaster;
		}
		else
		{
			result = this.m_effectOnCaster;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnAllies()
	{
		if (this.m_syncComp != null)
		{
			if (this.m_syncComp.m_turnsAreaBuffActive == 0)
			{
				if (this.m_cachedFirstTurnEffectOnAllies != null && this.m_cachedFirstTurnEffectOnAllies.m_applyEffect)
				{
					return this.m_cachedFirstTurnEffectOnAllies;
				}
			}
		}
		return (this.m_cachedEffectOnAllies == null) ? this.m_effectOnAllies : this.m_cachedEffectOnAllies;
	}

	public int GetSelfShieldingOverride()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_selfShieldingOverrideMod.GetModifiedValue(this.m_selfShieldingOverride);
		}
		else
		{
			result = this.m_selfShieldingOverride;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnEnemies != null)
		{
			result = this.m_cachedEffectOnEnemies;
		}
		else
		{
			result = this.m_effectOnEnemies;
		}
		return result;
	}

	public bool AddVisionOnTargetSquare()
	{
		return (!this.m_abilityMod) ? this.m_addVisionOnTargetSquare : this.m_abilityMod.m_addVisionOnTargetSquareMod.GetModifiedValue(this.m_addVisionOnTargetSquare);
	}

	public float GetVisionRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_visionRadiusMod.GetModifiedValue(this.m_visionRadius);
		}
		else
		{
			result = this.m_visionRadius;
		}
		return result;
	}

	public int GetVisionDuration()
	{
		return (!this.m_abilityMod) ? this.m_visionDuration : this.m_abilityMod.m_visionDurationMod.GetModifiedValue(this.m_visionDuration);
	}

	public bool VisionAreaIgnoreLos()
	{
		return (!this.m_abilityMod) ? this.m_visionAreaIgnoreLos : this.m_abilityMod.m_visionAreaIgnoreLosMod.GetModifiedValue(this.m_visionAreaIgnoreLos);
	}

	public int GetExtraShieldsPerTurnActive()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraShieldsPerTurnActive.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetAllyTechPointGainPerTurnActive()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_allyTechPointGainPerTurnActive.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraSelfShieldsPerEnemyInShape()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraSelfShieldingPerEnemyInShape.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraHealForPurifyOnBuffedAllies()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraHealForPurifyOnBuffedAllies.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int CalculateShieldAmount(ActorData targetActor)
	{
		int num = this.GetEffectOnAllies().m_effectData.m_absorbAmount;
		if (targetActor == base.ActorData)
		{
			if (this.GetSelfShieldingOverride() > 0)
			{
				num = this.GetSelfShieldingOverride();
			}
			else if (this.GetEffectOnCaster() != null)
			{
				if (this.GetEffectOnCaster().m_applyEffect)
				{
					num = this.GetEffectOnCaster().m_effectData.m_absorbAmount;
				}
			}
			if (this.GetExtraSelfShieldsPerEnemyInShape() != 0)
			{
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.GetShape(), targetActor.GetTravelBoardSquareWorldPosition(), targetActor.GetCurrentBoardSquare(), true, targetActor, targetActor.GetOpposingTeam(), null);
				num += actorsInShape.Count * this.GetExtraSelfShieldsPerEnemyInShape();
			}
		}
		if (this.m_syncComp != null)
		{
			if (this.GetExtraShieldsPerTurnActive() != 0)
			{
				num += this.GetExtraShieldsPerTurnActive() * this.m_syncComp.m_turnsAreaBuffActive;
			}
		}
		return num;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "ExtraTpCostPerTurnActive", string.Empty, this.m_extraTpCostPerTurnActive, false);
		base.AddTokenInt(tokens, "CooldownWhenBuffLapses", string.Empty, this.m_cooldownWhenBuffLapses, false);
		base.AddTokenInt(tokens, "EffectDuration", string.Empty, this.m_effectDuration, false);
		base.AddTokenInt(tokens, "HealAmount", string.Empty, this.m_healAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnCaster, "EffectOnCaster", this.m_effectOnCaster, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnAllies, "EffectOnAllies", this.m_effectOnAllies, true);
		if (this.m_selfShieldingOverride >= 0)
		{
			base.AddTokenInt(tokens, "SelfShieldingOverride", string.Empty, this.m_selfShieldingOverride, false);
		}
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnEnemies, "EffectOnEnemies", this.m_effectOnEnemies, true);
		base.AddTokenInt(tokens, "VisionDuration", string.Empty, this.m_visionDuration, false);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		this.GetEffectOnAllies().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Ally);
		if (this.IncludeCaster())
		{
			if (this.m_selfShieldingOverride >= 0)
			{
				AbilityTooltipHelper.ReportAbsorb(ref list, AbilityTooltipSubject.Self, this.m_selfShieldingOverride);
			}
			else
			{
				this.GetEffectOnAllies().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Self);
			}
		}
		this.GetEffectOnEnemies().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Enemy);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, this.GetHealAmount()));
		if (this.IncludeCaster())
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, this.GetHealAmount()));
		}
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, 1));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		return new Dictionary<AbilityTooltipSymbol, int>
		{
			{
				AbilityTooltipSymbol.Absorb,
				this.CalculateShieldAmount(targetActor)
			},
			{
				AbilityTooltipSymbol.Energy,
				(!(targetActor != base.ActorData)) ? 0 : this.GetAllyTechPointGainPerTurnActive()
			}
		};
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (this.m_syncComp != null)
		{
			if (this.m_syncComp.m_turnsAreaBuffActive == 0)
			{
				return base.GetActionAnimType();
			}
		}
		return ActorModelData.ActionAnimationType.None;
	}

	public override bool ShouldAutoQueueIfValid()
	{
		if (this.m_syncComp != null)
		{
			if (this.m_syncComp.m_turnsAreaBuffActive > 0)
			{
				return true;
			}
		}
		return base.ShouldAutoQueueIfValid();
	}

	public override bool AllowCancelWhenAutoQueued()
	{
		return true;
	}

	public bool IsActorInBuffShape(ActorData targetActor)
	{
		if (base.ActorData.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.GetShape(), base.ActorData.GetTravelBoardSquareWorldPosition(), base.ActorData.GetCurrentBoardSquare());
			return AreaEffectUtils.GetActorsInShape(this.GetShape(), centerOfShape, base.ActorData.GetCurrentBoardSquare(), this.PenetrateLoS(), base.ActorData, base.ActorData.GetTeam(), null).Contains(targetActor);
		}
		return false;
	}

	public override bool UseCustomAbilityIconColor()
	{
		return this.m_syncComp != null && this.m_syncComp.m_turnsAreaBuffActive > 0;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		if (this.m_syncComp != null)
		{
			if (this.m_syncComp.m_turnsAreaBuffActive > 0)
			{
				return this.m_iconColorWhileActive;
			}
		}
		return base.GetCustomAbilityIconColor(actor);
	}

	public override bool ShouldShowPersistentAuraUI()
	{
		if (this.m_syncComp != null)
		{
			if (this.m_syncComp.m_turnsAreaBuffActive > 0)
			{
				AbilityData abilityData = base.ActorData.GetAbilityData();
				if (abilityData != null)
				{
					if (abilityData.HasQueuedAction(this.m_buffActionType))
					{
						if (this.GetPerTurnTechPointCost() <= base.ActorData.TechPoints)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public override int GetModdedCost()
	{
		if (this.m_syncComp != null && this.m_syncComp.m_turnsAreaBuffActive == 0)
		{
			return base.GetModdedCost();
		}
		return 0;
	}

	public int GetPerTurnTechPointCost()
	{
		int num = 0;
		if (this.m_syncComp != null)
		{
			num = this.m_syncComp.m_turnsAreaBuffActive * this.m_extraTpCostPerTurnActive;
		}
		return base.GetModdedCost() + num;
	}
}
