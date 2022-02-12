// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SparkHealingBeam : Ability
{
	public enum TargetingMode
	{
		BoardSquare,
		Laser
	}

	[Header("-- Targeting")]
	public TargetingMode m_targetingMode;
	[Header("-- Targeting: If Using Laser targeting mode")]
	public LaserTargetingInfo m_laserInfo;
	public StandardEffectInfo m_laserHitEffect;
	// reactor
	[Separator("-- Tether --", true)]
	// rogues
	//[Header("-- Tether --")]
	public float m_tetherDistance = 5f;
	public bool m_checkTetherRemovalBetweenPhases;
	// removed in rogues
	// TODO GAMEPLAY check this works on server (tether duration mod?)
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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Spark Healing Beam";
		}
		Setup();
	}

	public void Setup()
	{
		SetCachedFields();
		if (m_energizedAbility == null)
		{
			AbilityData abilityData = GetComponent<AbilityData>();
			if (abilityData != null)
			{
				m_energizedAbility = abilityData.GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized;
			}
		}
		if (Targeter != null)
		{
			Targeter.ResetTargeter(true);  // no param in rogues
		}
		bool affectsCaster = m_healSelfOnInitialAttach && GetHealOnSelfPerTurn() > 0;
		if (m_targetingMode == TargetingMode.Laser)
		{
			AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, GetLaserInfo());
			abilityUtil_Targeter_Laser.SetAffectedGroups(false, true, affectsCaster);
			abilityUtil_Targeter_Laser.m_affectCasterDelegate = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
			Targeter = abilityUtil_Targeter_Laser;
		}
		if (m_targetingMode == TargetingMode.BoardSquare)
		{
			AbilityUtil_Targeter.AffectsActor affectsCasterEnum = affectsCaster
				? AbilityUtil_Targeter.AffectsActor.Possible
				: AbilityUtil_Targeter.AffectsActor.Never;
			AbilityUtil_Targeter_Shape targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, affectsCasterEnum);
			targeter.SetAffectedGroups(false, true, affectsCaster);
			targeter.m_affectCasterDelegate = (ActorData caster, List<ActorData> actorsSoFar, bool casterInShape) => actorsSoFar.Count > 0;
			Targeter = targeter;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	public int GetHealOnAllyPerTurn()
	{
		return GetAllyTetherEffectData().m_healingPerTurn;
	}

	public int GetHealingOnAttach()
	{
		return m_abilityMod != null
			? m_abilityMod.m_initialHealingMod.GetModifiedValue(m_laserHealingAmount)
			: m_laserHealingAmount;
	}

	public int GetAdditionalHealOnRadiated()
	{
		return m_abilityMod != null
			? m_abilityMod.m_additionalHealOnRadiatedMod.GetModifiedValue(m_additionalEnergizedHealing)
			: m_additionalEnergizedHealing;
	}

	public int GetEnergyOnCasterPerTurn()
	{
		int num = m_abilityMod != null
			? m_abilityMod.m_energyOnCasterPerTurnMod.GetModifiedValue(m_energyOnCasterPerTurn)
			: m_energyOnCasterPerTurn;
		if (m_energizedAbility != null)
		{
			num = m_energizedAbility.CalcEnergyOnSelfPerTurn(num);
		}
		return num;
	}

	public int GetHealOnSelfPerTurn()
	{
		int num = m_abilityMod != null
			? m_abilityMod.m_healOnCasterOnTickMod.GetModifiedValue(m_healOnSelfOnTick)
			: m_healOnSelfOnTick;
		if (m_energizedAbility != null)
		{
			num = m_energizedAbility.CalcHealOnSelfPerTurn(num);
		}
		return num;
	}

	public float GetTetherDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_tetherDistanceMod.GetModifiedValue(m_tetherDistance)
			: m_tetherDistance;
	}

	// removed in rogues
	public int GetTetherDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_tetherDurationMod.GetModifiedValue(m_tetherDuration)
			: m_tetherDuration;
	}

	public bool UseBonusHealing()
	{
		return m_abilityMod != null && m_abilityMod.m_useBonusHealOverTime;
	}

	public int GetBonusHealGrowRate()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_bonusAllyHealIncreaseRate.GetModifiedValue(0)
			: 0;
	}

	public int GetMaxBonusHealing()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxAllyBonusHealAmount.GetModifiedValue(0)
			: 0;
	}

	public int GetBonusHealFromTetherAge(int age)
	{
		int num = 0;
		if (UseBonusHealing())
		{
			int maxBonusHealing = GetMaxBonusHealing();
			int bonusHealGrowRate = GetBonusHealGrowRate();
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
		return GetXDamageThreshold() > 0 && GetTargetEffectForXDamage() != null;
	}

	public int GetXDamageThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_xDamageThreshold
			: -1;
	}

	public StandardEffectInfo GetTargetEffectForXDamage()
	{
		return m_abilityMod?.m_effectOnTargetForTakingXDamage;
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_laserInfo.GetModifiedCopy(m_abilityMod?.m_laserInfoMod);
		StandardEffectInfo standardEffectInfo = m_abilityMod != null
			? m_abilityMod.m_tetherBaseEffectOverride.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect.GetShallowCopy();
		m_cachedAllyEffect = standardEffectInfo.m_effectData;
		m_cachedAllyEffect.m_sequencePrefabs = new GameObject[2]
		{
			m_targetPersistentSequence,
			m_beamSequence
		};
	}

	public StandardActorEffectData GetAllyTetherEffectData()
	{
		return m_cachedAllyEffect ?? m_laserHitEffect.m_effectData;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (m_targetingMode == TargetingMode.Laser)
		{
			return true;
		}
		ActorData targetActor = Board.Get().GetSquare(target.GridPos)?.OccupantActor;
		return CanTargetActorInDecision(caster, targetActor, false, true, false, ValidateCheckPath.Ignore, true, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_targetingMode == TargetingMode.Laser)
		{
			return true;
		}
		TargetingParadigm targetingParadigm = GetTargetingParadigm(0);
		if (targetingParadigm != TargetingParadigm.BoardSquare && targetingParadigm != TargetingParadigm.Position)
		{
			return true;
		}
		return HasTargetableActorsInDecision(caster, false, true, false, ValidateCheckPath.Ignore, true, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Heal_FirstTurn", "damage on initial attach", m_laserHealingAmount);
		AddTokenInt(tokens, "Heal_PerTurnAfterFirst", "damage on subsequent turns", m_laserHitEffect.m_effectData.m_healingPerTurn);
		AddTokenInt(tokens, "Heal_AdditionalOnRadiated", "additional damage on radiated", m_additionalEnergizedHealing);
		AddTokenInt(tokens, "Heal_OnCasterPerTurn", "heal on caster per turn", m_healOnSelfOnTick);
		AddTokenInt(tokens, "EnergyOnCasterPerTurn", string.Empty, m_energyOnCasterPerTurn);
		AddTokenInt(tokens, "TetherDuration", string.Empty, m_tetherDuration);  // removed in rogues
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Ally, GetHealingOnAttach());
		if (m_healSelfOnInitialAttach && GetHealOnSelfPerTurn() > 0)
		{
			AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, GetHealOnSelfPerTurn());
		}
		return number;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int visibleActorsCountByTooltipSubject = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
		return visibleActorsCountByTooltipSubject > 0 ? GetEnergyOnCasterPerTurn() : 0;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		list.Add(m_laserHitEffect.m_effectData.m_healingPerTurn);
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkHealingBeam))
		{
			m_abilityMod = (abilityMod as AbilityMod_SparkHealingBeam);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	// server-only
#if SERVER
	public SparkHealingBeamEffect CreateHealTetherEffect(ActorData caster, ActorData hitActor)
	{
		StandardActorEffectData allyTetherEffectData = GetAllyTetherEffectData();
		return new SparkHealingBeamEffect(AsEffectSource(), caster.GetCurrentBoardSquare(), hitActor, caster, allyTetherEffectData, m_healingPhase, GetTetherDistance(), m_checkTetherRemovalBetweenPhases, GetHealOnSelfPerTurn(), GetAdditionalHealOnRadiated(), GetEnergyOnCasterPerTurn(), m_pulseAnimIndex, m_energizedPulseAnimIndex, m_pulseSequence, m_energizedPulseSequence);
	}
#endif

	// server-only
#if SERVER
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		VectorUtils.LaserCoords laserCoords;
		GetHitActors(targets, caster, out laserCoords, null);
		return new ServerClientUtils.SequenceStartData(m_castSequence, laserCoords.end, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, null);
	}
#endif

	// server-only
#if SERVER
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		VectorUtils.LaserCoords laserCoords;
		List<ActorData> hitActors = GetHitActors(targets, caster, out laserCoords, nonActorTargetInfo);
		foreach (ActorData actorData in hitActors)
		{
			int healingOnAttach = GetHealingOnAttach();
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			actorHitResults.SetBaseHealing(healingOnAttach);
			actorHitResults.SetTechPointGainOnCaster(GetEnergyOnCasterPerTurn());
			SetExistingEffectsForRemoval(caster, actorHitResults);
			SparkHealingBeamEffect effect = CreateHealTetherEffect(caster, actorData);
			actorHitResults.AddEffect(effect);
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (hitActors.Count > 0 && m_healSelfOnInitialAttach && GetHealOnSelfPerTurn() > 0)
		{
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			actorHitResults2.SetBaseHealing(GetHealOnSelfPerTurn());
			abilityResults.StoreActorHit(actorHitResults2);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif

	// server-only
#if SERVER
	protected virtual List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out VectorUtils.LaserCoords endPoints, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> result;
		if (m_targetingMode == TargetingMode.Laser)
		{
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, GetLaserInfo().affectsAllies, GetLaserInfo().affectsEnemies);
			result = AreaEffectUtils.GetActorsInLaser(laserCoords.start, targets[0].AimDirection, GetLaserInfo().range, GetLaserInfo().width, caster, relevantTeams, GetLaserInfo().penetrateLos, GetLaserInfo().maxTargets, false, true, out laserCoords.end, nonActorTargetInfo, null, false, true);
		}
		else
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null && square.OccupantActor != null && !square.OccupantActor.IgnoreForAbilityHits)
			{
				laserCoords.end = square.OccupantActor.GetLoSCheckPos();
				result = square.OccupantActor.AsList();
			}
			else
			{
				laserCoords.end = laserCoords.start;
				result = new List<ActorData>();
			}
		}
		endPoints = laserCoords;
		return result;
	}
#endif

	// server-only
#if SERVER
	public void SetExistingEffectsForRemoval(ActorData caster, ActorHitResults hitResult)
	{
		foreach (int actorIndex in caster.GetComponent<SparkBeamTrackerComponent>().GetBeamActorIndices())
		{
			ActorData actorOfActorIndex = GameplayUtils.GetActorOfActorIndex(actorIndex);
			if (actorOfActorIndex != null)
			{
				foreach (Effect effect in ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorOfActorIndex, caster, typeof(SparkHealingBeamEffect)))
				{
					SparkHealingBeamEffect effect2 = effect as SparkHealingBeamEffect;
					hitResult.AddEffectForRemoval(effect2, ServerEffectManager.Get().GetActorEffects(actorOfActorIndex));
				}
			}
		}
	}
#endif
}
