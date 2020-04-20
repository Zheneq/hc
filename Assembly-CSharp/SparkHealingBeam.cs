using System;
using System.Collections.Generic;
using UnityEngine;

public class SparkHealingBeam : Ability
{
	[Header("-- Targeting")]
	public SparkHealingBeam.TargetingMode m_targetingMode;

	[Header("-- Targeting: If Using Laser targeting mode")]
	public LaserTargetingInfo m_laserInfo;

	public StandardEffectInfo m_laserHitEffect;

	[Separator("-- Tether --", true)]
	public float m_tetherDistance = 5f;

	public bool m_checkTetherRemovalBetweenPhases;

	[Header("-- Tether Duration")]
	public int m_tetherDuration;

	[Header("-- Healing")]
	public int m_laserHealingAmount;

	public int m_additionalEnergizedHealing = 2;

	public int m_healOnSelfOnTick;

	public bool m_healSelfOnInitialAttach;

	public AbilityPriority m_healingPhase = AbilityPriority.Prep_Offense;

	[Header("-- Energy on Caster Per Turn")]
	public int m_energyOnCasterPerTurn = 3;

	[Header("-- Animation on Pulse")]
	public int m_pulseAnimIndex;

	public int m_energizedPulseAnimIndex;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_pulseSequence;

	public GameObject m_energizedPulseSequence;

	public GameObject m_beamSequence;

	public GameObject m_targetPersistentSequence;

	private AbilityMod_SparkHealingBeam m_abilityMod;

	private SparkEnergized m_energizedAbility;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardActorEffectData m_cachedAllyEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Spark Healing Beam";
		}
		this.Setup();
	}

	public void Setup()
	{
		this.SetCachedFields();
		if (this.m_energizedAbility == null)
		{
			AbilityData component = base.GetComponent<AbilityData>();
			if (component != null)
			{
				this.m_energizedAbility = (component.GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized);
			}
		}
		if (base.Targeter != null)
		{
			base.Targeter.ResetTargeter(true);
		}
		bool flag = this.m_healSelfOnInitialAttach && this.GetHealOnSelfPerTurn() > 0;
		if (this.m_targetingMode == SparkHealingBeam.TargetingMode.Laser)
		{
			AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, this.GetLaserInfo());
			abilityUtil_Targeter_Laser.SetAffectedGroups(false, true, flag);
			abilityUtil_Targeter_Laser.m_affectCasterDelegate = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
			base.Targeter = abilityUtil_Targeter_Laser;
		}
		if (this.m_targetingMode == SparkHealingBeam.TargetingMode.BoardSquare)
		{
			AbilityUtil_Targeter.AffectsActor affectsActor;
			if (flag)
			{
				affectsActor = AbilityUtil_Targeter.AffectsActor.Possible;
			}
			else
			{
				affectsActor = AbilityUtil_Targeter.AffectsActor.Never;
			}
			AbilityUtil_Targeter.AffectsActor affectsCaster = affectsActor;
			AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
			abilityUtil_Targeter_Shape.SetAffectedGroups(false, true, flag);
			abilityUtil_Targeter_Shape.m_affectCasterDelegate = ((ActorData caster, List<ActorData> actorsSoFar, bool casterInShape) => actorsSoFar.Count > 0);
			base.Targeter = abilityUtil_Targeter_Shape;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserInfo().range;
	}

	public int GetHealOnAllyPerTurn()
	{
		return this.GetAllyTetherEffectData().m_healingPerTurn;
	}

	public int GetHealingOnAttach()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_initialHealingMod.GetModifiedValue(this.m_laserHealingAmount);
		}
		else
		{
			result = this.m_laserHealingAmount;
		}
		return result;
	}

	public int GetAdditionalHealOnRadiated()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_additionalHealOnRadiatedMod.GetModifiedValue(this.m_additionalEnergizedHealing);
		}
		else
		{
			result = this.m_additionalEnergizedHealing;
		}
		return result;
	}

	public int GetEnergyOnCasterPerTurn()
	{
		int num;
		if (this.m_abilityMod)
		{
			num = this.m_abilityMod.m_energyOnCasterPerTurnMod.GetModifiedValue(this.m_energyOnCasterPerTurn);
		}
		else
		{
			num = this.m_energyOnCasterPerTurn;
		}
		int num2 = num;
		if (this.m_energizedAbility != null)
		{
			num2 = this.m_energizedAbility.CalcEnergyOnSelfPerTurn(num2);
		}
		return num2;
	}

	public int GetHealOnSelfPerTurn()
	{
		int num = (!this.m_abilityMod) ? this.m_healOnSelfOnTick : this.m_abilityMod.m_healOnCasterOnTickMod.GetModifiedValue(this.m_healOnSelfOnTick);
		if (this.m_energizedAbility != null)
		{
			num = this.m_energizedAbility.CalcHealOnSelfPerTurn(num);
		}
		return num;
	}

	public float GetTetherDistance()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_tetherDistanceMod.GetModifiedValue(this.m_tetherDistance);
		}
		else
		{
			result = this.m_tetherDistance;
		}
		return result;
	}

	public int GetTetherDuration()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_tetherDurationMod.GetModifiedValue(this.m_tetherDuration);
		}
		else
		{
			result = this.m_tetherDuration;
		}
		return result;
	}

	public bool UseBonusHealing()
	{
		return this.m_abilityMod && this.m_abilityMod.m_useBonusHealOverTime;
	}

	public int GetBonusHealGrowRate()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_bonusAllyHealIncreaseRate.GetModifiedValue(0);
	}

	public int GetMaxBonusHealing()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxAllyBonusHealAmount.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetBonusHealFromTetherAge(int age)
	{
		int num = 0;
		if (this.UseBonusHealing())
		{
			int maxBonusHealing = this.GetMaxBonusHealing();
			int bonusHealGrowRate = this.GetBonusHealGrowRate();
			if (bonusHealGrowRate > 0)
			{
				num = age * bonusHealGrowRate;
			}
			if (maxBonusHealing > 0)
			{
				num = Mathf.Min(maxBonusHealing, num);
			}
		}
		return num;
	}

	public bool ShouldApplyTargetEffectForXDamage()
	{
		bool result;
		if (this.GetXDamageThreshold() > 0)
		{
			result = (this.GetTargetEffectForXDamage() != null);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int GetXDamageThreshold()
	{
		return (!this.m_abilityMod) ? -1 : this.m_abilityMod.m_xDamageThreshold;
	}

	public StandardEffectInfo GetTargetEffectForXDamage()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_effectOnTargetForTakingXDamage;
		}
		else
		{
			result = null;
		}
		return result;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo laserInfo = this.m_laserInfo;
		AbilityModPropertyLaserInfo mod;
		if (this.m_abilityMod)
		{
			mod = this.m_abilityMod.m_laserInfoMod;
		}
		else
		{
			mod = null;
		}
		this.m_cachedLaserInfo = laserInfo.GetModifiedCopy(mod);
		StandardEffectInfo standardEffectInfo = (!this.m_abilityMod) ? this.m_laserHitEffect.GetShallowCopy() : this.m_abilityMod.m_tetherBaseEffectOverride.GetModifiedValue(this.m_laserHitEffect);
		this.m_cachedAllyEffect = standardEffectInfo.m_effectData;
		this.m_cachedAllyEffect.m_sequencePrefabs = new GameObject[]
		{
			this.m_targetPersistentSequence,
			this.m_beamSequence
		};
	}

	public StandardActorEffectData GetAllyTetherEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedAllyEffect != null)
		{
			result = this.m_cachedAllyEffect;
		}
		else
		{
			result = this.m_laserHitEffect.m_effectData;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
		{
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.m_targetingMode == SparkHealingBeam.TargetingMode.Laser)
		{
			return true;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		ActorData actorData;
		if (boardSquareSafe)
		{
			actorData = boardSquareSafe.OccupantActor;
		}
		else
		{
			actorData = null;
		}
		ActorData targetActor = actorData;
		return base.CanTargetActorInDecision(caster, targetActor, false, true, false, Ability.ValidateCheckPath.Ignore, true, false, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_targetingMode == SparkHealingBeam.TargetingMode.Laser)
		{
			return true;
		}
		Ability.TargetingParadigm targetingParadigm = base.GetTargetingParadigm(0);
		if (targetingParadigm != Ability.TargetingParadigm.BoardSquare)
		{
			if (targetingParadigm != Ability.TargetingParadigm.Position)
			{
				return true;
			}
		}
		return base.HasTargetableActorsInDecision(caster, false, true, false, Ability.ValidateCheckPath.Ignore, true, false, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Heal_FirstTurn", "damage on initial attach", this.m_laserHealingAmount, false);
		base.AddTokenInt(tokens, "Heal_PerTurnAfterFirst", "damage on subsequent turns", this.m_laserHitEffect.m_effectData.m_healingPerTurn, false);
		base.AddTokenInt(tokens, "Heal_AdditionalOnRadiated", "additional damage on radiated", this.m_additionalEnergizedHealing, false);
		base.AddTokenInt(tokens, "Heal_OnCasterPerTurn", "heal on caster per turn", this.m_healOnSelfOnTick, false);
		base.AddTokenInt(tokens, "EnergyOnCasterPerTurn", string.Empty, this.m_energyOnCasterPerTurn, false);
		base.AddTokenInt(tokens, "TetherDuration", string.Empty, this.m_tetherDuration, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		int healingOnAttach = this.GetHealingOnAttach();
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, healingOnAttach);
		if (this.m_healSelfOnInitialAttach)
		{
			if (this.GetHealOnSelfPerTurn() > 0)
			{
				AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetHealOnSelfPerTurn());
			}
		}
		return result;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
		int result;
		if (visibleActorsCountByTooltipSubject > 0)
		{
			result = this.GetEnergyOnCasterPerTurn();
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public override List<int> symbol_001D()
	{
		List<int> list = base.symbol_001D();
		list.Add(this.m_laserHitEffect.m_effectData.m_healingPerTurn);
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkHealingBeam))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SparkHealingBeam);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public enum TargetingMode
	{
		BoardSquare,
		Laser
	}
}
