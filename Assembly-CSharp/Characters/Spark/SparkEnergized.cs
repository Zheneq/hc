// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SparkEnergized : Ability
{
	[Header("-- Cast sequences (if Need To Select Target is true)")]
	public GameObject m_castOnAllySequence;
	public GameObject m_castOnEnemySequence;
	public GameObject m_castOnAllSequence;
	public StandardActorEffectData m_allyBuffEffect;
	public StandardActorEffectData m_enemyDebuffEffect;
	public int m_healAmtPerBeam;
	[Header("-- Whether to choose which target to select (need 1 TargetData entry if true, 0 otherwise)")]
	public bool m_needToSelectTarget = true;
	// removed in rogues
	[Separator("Effect and Boosted Heal/Damage when both tethers are attached", true)]
	public StandardEffectInfo m_bothTetherExtraEffectOnSelf;
	// removed in rogues
	public StandardEffectInfo m_bothTetherAllyEffect;
	// removed in rogues
	public StandardEffectInfo m_bothTetherEnemyEffect;
	// removed in rogues
	[Space(10f)]
	public int m_bothTetherExtraHeal;
	// removed in rogues
	public int m_bothTetherExtraDamage;

	private AbilityMod_SparkEnergized m_abilityMod;
	private SparkBasicAttack m_damageBeamAbility;
	private SparkHealingBeam m_healBeamAbility;
	private SparkBeamTrackerComponent m_beamSyncComp;
	// removed in rogues
	private bool m_cachedCanTargetSelf;
	// removed in rogues
	private TargetData[] m_emptyTargetData = new TargetData[0];
	private StandardActorEffectData m_cachedAllyBuffEffect;
	private StandardActorEffectData m_cachedEnemyDebuffEffect;
	// removed in rogues
	private StandardEffectInfo m_cachedBothTetherExtraEffectOnSelf;
	// removed in rogues
	private StandardEffectInfo m_cachedBothTetherAllyEffect;
	// removed in rogues
	private StandardEffectInfo m_cachedBothTetherEnemyEffect;
#if SERVER
	// added in rogues
	private List<ActorData> m_lastHitActors = new List<ActorData>();
#endif

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_damageBeamAbility = GetComponent<SparkBasicAttack>();
		m_healBeamAbility = GetComponent<SparkHealingBeam>();
		m_beamSyncComp = GetComponent<SparkBeamTrackerComponent>();
		if (NeedToChooseActor())
		{
			Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, true, true, AbilityUtil_Targeter.AffectsActor.Always);
			return;
		}

		// reactor
		AbilityUtil_Targeter_AllVisible abilityUtil_Targeter_AllVisible = new AbilityUtil_Targeter_AllVisible(this, true, true, false, AbilityUtil_Targeter_AllVisible.DamageOriginType.TargetPos);
		abilityUtil_Targeter_AllVisible.SetAffectedGroups(true, true, m_cachedCanTargetSelf);
		abilityUtil_Targeter_AllVisible.m_shouldAddActorDelegate = delegate (ActorData potentialTarget, ActorData caster)
		{
			if (m_beamSyncComp == null || potentialTarget == null)
			{
				return false;
			}
			if (potentialTarget == caster)
			{
				return m_cachedCanTargetSelf;
			}
			return m_beamSyncComp.IsActorIndexTracked(potentialTarget.ActorIndex);
		};
		Targeter = abilityUtil_Targeter_AllVisible;
		// rogues
		//      Targeter = new AbilityUtil_Targeter_AllVisible(this, true, true, false, AbilityUtil_Targeter_AllVisible.DamageOriginType.TargetPos)
		//{
		//	m_shouldAddActorDelegate = ((ActorData potentialTarget, ActorData caster) => m_beamSyncComp != null && potentialTarget != null && m_beamSyncComp.IsActorIndexTracked(potentialTarget.ActorIndex))
		//};
	}

	// removed in rogues
	public override TargetData[] GetTargetData()
	{
		if (NeedToChooseActor())
		{
			return base.GetTargetData();
		}
		return m_emptyTargetData;
	}

	private void SetCachedFields()
	{
		m_cachedAllyBuffEffect = m_abilityMod != null
			? m_abilityMod.m_allyBuffEffectMod.GetModifiedValue(m_allyBuffEffect)
			: m_allyBuffEffect;
		m_cachedEnemyDebuffEffect = m_abilityMod == null
			? m_enemyDebuffEffect
			: m_abilityMod.m_enemyDebuffEffectMod.GetModifiedValue(m_enemyDebuffEffect);
		m_cachedCanTargetSelf = false;

		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		if (GetHealAmtPerBeam() > 0
			|| moddedEffectForSelf != null
				&& moddedEffectForSelf.m_applyEffect
				&& moddedEffectForSelf.m_effectData.m_absorbAmount > 0)
		{
			m_cachedCanTargetSelf = true;
		}

		m_cachedBothTetherExtraEffectOnSelf = m_abilityMod != null
			? m_abilityMod.m_bothTetherExtraEffectOnSelfMod.GetModifiedValue(m_bothTetherExtraEffectOnSelf)
			: m_bothTetherExtraEffectOnSelf;
		m_cachedBothTetherAllyEffect = m_abilityMod != null
			? m_abilityMod.m_bothTetherAllyEffectMod.GetModifiedValue(m_bothTetherAllyEffect)
			: m_bothTetherAllyEffect;
		m_cachedBothTetherEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_bothTetherEnemyEffectMod.GetModifiedValue(m_bothTetherEnemyEffect)
			: m_bothTetherEnemyEffect;
	}

	public bool NeedToChooseActor()
	{
		return m_abilityMod != null
			? m_abilityMod.m_needToChooseTargetMod.GetModifiedValue(m_needToSelectTarget)
			: m_needToSelectTarget;
	}

	public int CalcHealOnSelfPerTurn(int baseAmount)
	{
		return m_abilityMod != null
			? m_abilityMod.m_healOnSelfFromTetherMod.GetModifiedValue(baseAmount)
			: baseAmount;
	}

	public int CalcEnergyOnSelfPerTurn(int baseAmount)
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyOnSelfFromTetherMod.GetModifiedValue(baseAmount)
			: baseAmount;
	}

	public int CalcAdditionalHealOnCast(int baseAmount)
	{
		return m_abilityMod != null
			? m_abilityMod.m_additionalHealMod.GetModifiedValue(baseAmount)
			: baseAmount;
	}

	public int CalcAdditonalDamageOnCast(int baseAmount)
	{
		return m_abilityMod != null
			? m_abilityMod.m_additionalDamageMod.GetModifiedValue(baseAmount)
			: baseAmount;
	}

	public bool HasEnemyEffectForTurnStart()
	{
		return m_abilityMod != null && m_abilityMod.m_effectOnEnemyOnNextTurn.m_applyEffect;
	}

	public StandardEffectInfo GetEnemyEffectForTurnStart()
	{
		return m_abilityMod?.m_effectOnEnemyOnNextTurn;
	}

	public StandardActorEffectData GetAllyBuffEffect()
	{
		return m_cachedAllyBuffEffect ?? m_allyBuffEffect;
	}

	public StandardActorEffectData GetEnemyDebuffEffect()
	{
		return m_cachedEnemyDebuffEffect ?? m_enemyDebuffEffect;
	}

	public int GetHealAmtPerBeam()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healAmtPerBeamMod.GetModifiedValue(m_healAmtPerBeam)
			: m_healAmtPerBeam;
	}

	// removed in rogues
	public StandardEffectInfo GetBothTetherExtraEffectOnSelf()
	{
		return m_cachedBothTetherExtraEffectOnSelf ?? m_bothTetherExtraEffectOnSelf;
	}

	// removed in rogues
	public StandardEffectInfo GetBothTetherAllyEffect()
	{
		return m_cachedBothTetherAllyEffect ?? m_bothTetherAllyEffect;
	}

	// removed in rogues
	public StandardEffectInfo GetBothTetherEnemyEffect()
	{
		return m_cachedBothTetherEnemyEffect ?? m_bothTetherEnemyEffect;
	}

	// removed in rogues
	public int GetBothTetherExtraHeal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bothTetherExtraHealMod.GetModifiedValue(m_bothTetherExtraHeal)
			: m_bothTetherExtraHeal;
	}

	// removed in rogues
	public int GetBothTetherExtraDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bothTetherExtraDamageMod.GetModifiedValue(m_bothTetherExtraDamage)
			: m_bothTetherExtraDamage;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return GetDamageBeamAbility() != null && animIndex == GetDamageBeamAbility().m_energizedPulseAnimIndex
			|| GetHealBeamAbility() != null && animIndex == GetHealBeamAbility().m_energizedPulseAnimIndex
			|| base.CanTriggerAnimAtIndexForTaunt(animIndex);
	}

	private SparkBasicAttack GetDamageBeamAbility()
	{
		if (m_damageBeamAbility == null)
		{
			m_damageBeamAbility = GetComponent<SparkBasicAttack>();
		}
		return m_damageBeamAbility;
	}

	private SparkHealingBeam GetHealBeamAbility()
	{
		if (m_healBeamAbility == null)
		{
			m_healBeamAbility = GetComponent<SparkHealingBeam>();
		}
		return m_healBeamAbility;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return caster.GetComponent<SparkBeamTrackerComponent>().BeamIsActive();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!NeedToChooseActor())
		{
			return true;
		}
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare != null && targetSquare.IsValidForGameplay())
		{
			List<ActorData> beamActors = caster.GetComponent<SparkBeamTrackerComponent>().GetBeamActors();
			List<BoardSquare> beamActorSquares = new List<BoardSquare>();
			foreach (ActorData beamActor in beamActors)
			{
				if (beamActor.GetCurrentBoardSquare() != null)
				{
					beamActorSquares.Add(beamActor.GetCurrentBoardSquare());
				}
			}
			return beamActorSquares.Contains(targetSquare);
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Ally, 1);
		AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Enemy, 1);
		GetAllyBuffEffect().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Ally);
		GetEnemyDebuffEffect().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Enemy);
		AppendTooltipNumbersFromBaseModEffects(ref number); // removed in rogues
		return number;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null
			&& m_beamSyncComp != null
			&& ActorData != null)
		{
			SparkHealingBeam healBeamAbility = GetHealBeamAbility();
			SparkBasicAttack damageBeamAbility = GetDamageBeamAbility();
			int tetherAgeOnActor = m_beamSyncComp.GetTetherAgeOnActor(targetActor.ActorIndex);
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				if (damageBeamAbility != null)
				{
					int perTurnDamage = damageBeamAbility.GetPerTurnDamage();
					perTurnDamage += CalcAdditonalDamageOnCast(damageBeamAbility.GetAdditionalDamageOnRadiated());
					perTurnDamage += damageBeamAbility.GetBonusDamageFromTetherAge(tetherAgeOnActor);
					// removed in rogues
					if (m_beamSyncComp.HasBothTethers())
					{
						perTurnDamage += GetBothTetherExtraDamage();
					}
					// end removed
					dictionary[AbilityTooltipSymbol.Damage] = perTurnDamage;
				}
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
			{
				if (healBeamAbility != null)
				{
					int healOnAllyPerTurn = healBeamAbility.GetHealOnAllyPerTurn();
					healOnAllyPerTurn += CalcAdditionalHealOnCast(healBeamAbility.GetAdditionalHealOnRadiated());
					healOnAllyPerTurn += healBeamAbility.GetBonusHealFromTetherAge(tetherAgeOnActor);
					// removed in rogues
					if (m_beamSyncComp.HasBothTethers())
					{
						healOnAllyPerTurn += GetBothTetherExtraHeal();
					}
					// end removed
					dictionary[AbilityTooltipSymbol.Healing] = healOnAllyPerTurn;
				}
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				List<int> beamActorIndices = m_beamSyncComp.GetBeamActorIndices();
				int allyNum = 0;
				int enemyNum = 0;
				foreach (int beamActorIndex in beamActorIndices)
				{
					ActorData beamActor = GameFlowData.Get().FindActorByActorIndex(beamActorIndex);
					if (beamActor != null)
					{
						if (beamActor.GetTeam() == ActorData.GetTeam())
						{
							allyNum++;
						}
						else
						{
							enemyNum++;
						}
					}
				}
				dictionary[AbilityTooltipSymbol.Healing] = (allyNum + enemyNum) * GetHealAmtPerBeam();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SparkEnergized abilityMod_SparkEnergized = modAsBase as AbilityMod_SparkEnergized;
		SparkBasicAttack basicAttack = GetComponent<SparkBasicAttack>();
		SparkHealingBeam healingBeam = GetComponent<SparkHealingBeam>();
		if (basicAttack != null)
		{
			AddTokenInt(tokens, "RadiatedDamage_Total", "", basicAttack.m_laserDamageAmount + basicAttack.m_additionalEnergizedDamage);
			AddTokenInt(tokens, "RadiatedDamage_Diff", "", basicAttack.m_additionalEnergizedDamage);
		}
		if (healingBeam != null)
		{
			AddTokenInt(tokens, "RadiatedHeal_Total", "", healingBeam.m_laserHealingAmount + healingBeam.m_additionalEnergizedHealing);
			AddTokenInt(tokens, "RadiatedHeal_Diff", "", healingBeam.m_additionalEnergizedHealing);
		}
		// reactor
		StandardActorEffectData allyBuffEffect = abilityMod_SparkEnergized != null
			? abilityMod_SparkEnergized.m_allyBuffEffectMod.GetModifiedValue(m_allyBuffEffect)
			: m_allyBuffEffect;
		allyBuffEffect.AddTooltipTokens(tokens, "AllyBuffEffect", abilityMod_SparkEnergized != null, m_allyBuffEffect);

		StandardActorEffectData enemyDebuffEffect = abilityMod_SparkEnergized != null
			? abilityMod_SparkEnergized.m_enemyDebuffEffectMod.GetModifiedValue(m_enemyDebuffEffect)
			: m_enemyDebuffEffect;
		enemyDebuffEffect.AddTooltipTokens(tokens, "EnemyDebuffEffect", abilityMod_SparkEnergized != null, m_enemyDebuffEffect);

		int healAmtPerBeam = abilityMod_SparkEnergized != null
			? abilityMod_SparkEnergized.m_healAmtPerBeamMod.GetModifiedValue(m_healAmtPerBeam)
			: m_healAmtPerBeam;
		AddTokenInt(tokens, "HealAmtPerBeam", "", healAmtPerBeam);
		AbilityMod.AddToken_EffectInfo(tokens, m_bothTetherExtraEffectOnSelf, "BothTetherExtraEffectOnSelf", m_bothTetherExtraEffectOnSelf);
		AbilityMod.AddToken_EffectInfo(tokens, m_bothTetherAllyEffect, "BothTetherAllyEffect", m_bothTetherAllyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_bothTetherEnemyEffect, "BothTetherEnemyEffect", m_bothTetherEnemyEffect);
		AddTokenInt(tokens, "BothTetherExtraHeal", "", m_bothTetherExtraHeal);
		AddTokenInt(tokens, "BothTetherExtraDamage", "", m_bothTetherExtraDamage);
		// rogues
		//(abilityMod_SparkEnergized ? abilityMod_SparkEnergized.m_allyBuffEffectMod.GetModifiedValue(m_allyBuffEffect) : m_allyBuffEffect).AddTooltipTokens(tokens, "AllyBuffEffect", abilityMod_SparkEnergized != null, m_allyBuffEffect);
		//(abilityMod_SparkEnergized ? abilityMod_SparkEnergized.m_enemyDebuffEffectMod.GetModifiedValue(m_enemyDebuffEffect) : m_enemyDebuffEffect).AddTooltipTokens(tokens, "EnemyDebuffEffect", abilityMod_SparkEnergized != null, m_enemyDebuffEffect);
		//      AddTokenInt(tokens, "HealAmtPerBeam", "", abilityMod_SparkEnergized ? abilityMod_SparkEnergized.m_healAmtPerBeamMod.GetModifiedValue(m_healAmtPerBeam) : m_healAmtPerBeam, false);
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		SparkBasicAttack basicAttack = GetComponent<SparkBasicAttack>();
		SparkHealingBeam healingBeam = GetComponent<SparkHealingBeam>();
		if (basicAttack != null)
		{
			list.Add(basicAttack.m_laserHitEffect.m_effectData.m_damagePerTurn + basicAttack.GetAdditionalDamageOnRadiated());
		}
		if (healingBeam != null)
		{
			list.Add(healingBeam.m_laserHitEffect.m_effectData.m_healingPerTurn + healingBeam.GetAdditionalHealOnRadiated());
		}
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkEnergized))
		{
			m_abilityMod = (abilityMod as AbilityMod_SparkEnergized);
			Setup();
			ReinitTetherAbilities();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
		ReinitTetherAbilities();
	}

	private void ReinitTetherAbilities()
	{
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			SparkBasicAttack sparkBasicAttack = abilityData.GetAbilityOfType(typeof(SparkBasicAttack)) as SparkBasicAttack;
			SparkHealingBeam sparkHealingBeam = abilityData.GetAbilityOfType(typeof(SparkHealingBeam)) as SparkHealingBeam;
			if (sparkBasicAttack != null)
			{
				sparkBasicAttack.Setup();
				sparkBasicAttack.ResetNameplateTargetingNumbers();
			}
			if (sparkHealingBeam != null)
			{
				sparkHealingBeam.Setup();
				sparkHealingBeam.ResetNameplateTargetingNumbers();
			}
		}
	}

	// added in rogues
#if SERVER
	public List<ActorData> GetLastHitActors()
	{
		return m_lastHitActors;
	}
#endif

	// added in rogues
#if SERVER
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		m_lastHitActors.Clear();
		foreach (ActorData item in additionalData.m_abilityResults.HitActorList())
		{
			m_lastHitActors.Add(item);
		}
	}
#endif

	// added in rogues
#if SERVER
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		GameObject prefab;
		if (NeedToChooseActor())
		{
			ActorData actorData = null;
			SparkBeamTrackerComponent component = caster.GetComponent<SparkBeamTrackerComponent>();
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square == null)
			{
				Debug.LogError("SparkEnergized is in NeedToChooseActor mode, but when it runs, the targeted square is null.");
			}
			else if (square.OccupantActor == null)
			{
				Debug.LogError("SparkEnergized is in NeedToChooseActor mode, but when it runs, the targeted square's actor is null.");
			}
			else if (!component.IsActorTracked(square.OccupantActor))
			{
				Debug.LogError("SparkEnergized is in NeedToChooseActor mode, but when it runs, the targeted square's actor is untracked.");
			}
			else
			{
				actorData = square.OccupantActor;
			}
			if (actorData == null)
			{
				prefab = AsEffectSource().GetSequencePrefab();
			}
			else if (actorData.GetTeam() == caster.GetTeam())
			{
				prefab = m_castOnAllySequence;
			}
			else
			{
				prefab = m_castOnEnemySequence;
			}
		}
		else
		{
			prefab = m_castOnAllSequence;
		}
		return new ServerClientUtils.SequenceStartData(prefab, caster.GetFreePos(), additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, null);
	}
#endif

	// added in rogues
#if SERVER
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<ActorData> list = new List<ActorData>();
		SparkBeamTrackerComponent component = caster.GetComponent<SparkBeamTrackerComponent>();
		if (NeedToChooseActor())
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null && square.OccupantActor != null && component.IsActorTracked(square.OccupantActor))
			{
				list.Add(square.OccupantActor);
			}
		}
		else
		{
			foreach (int actorIndex in component.GetBeamActorIndices())
			{
				ActorData actorOfActorIndex = GameplayUtils.GetActorOfActorIndex(actorIndex);
				if (actorOfActorIndex != null)
				{
					list.Add(actorOfActorIndex);
				}
			}
		}

		bool bothTethers = m_beamSyncComp.HasBothTethers(); // custom
		
		foreach (ActorData actorData in list)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			if (actorData.GetTeam() == caster.GetTeam() && GetAllyBuffEffect() != null)
			{
				actorHitResults.AddEffect(new SparkEnergizedEffect(
					AsEffectSource(),
					caster.GetCurrentBoardSquare(),
					actorData,
					caster,
					GetAllyBuffEffect(),
					abilityResults.CinematicRequested));
			
				// custom
				if (bothTethers)
				{
					actorHitResults.AddStandardEffectInfo(GetBothTetherAllyEffect());
					actorHitResults.AddBaseHealing(GetBothTetherExtraHeal());
				}
				// end custom
			}
			else if (actorData.GetTeam() != caster.GetTeam() && GetEnemyDebuffEffect() != null)
			{
				actorHitResults.AddEffect(new SparkEnergizedEffect(
					AsEffectSource(),
					caster.GetCurrentBoardSquare(),
					actorData,
					caster,
					GetEnemyDebuffEffect(),
					abilityResults.CinematicRequested));
			
				// custom
				if (bothTethers)
				{
					actorHitResults.AddStandardEffectInfo(GetBothTetherEnemyEffect());
					actorHitResults.AddBaseHealing(GetBothTetherExtraDamage());
				}
				// end custom
			}
			
			abilityResults.StoreActorHit(actorHitResults);
		}
		
		ActorHitParameters hitParams = new ActorHitParameters(caster, caster.GetFreePos());
		ActorHitResults hitResults = new ActorHitResults(GetHealAmtPerBeam() * component.GetNumTethers(), HitActionType.Healing, hitParams);
		
		// custom
		if (bothTethers)
		{
			hitResults.AddStandardEffectInfo(GetBothTetherExtraEffectOnSelf());
		}
		// end custom
		
		abilityResults.StoreActorHit(hitResults);
	}
#endif
}
