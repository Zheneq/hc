// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only missing in reactor
#if SERVER
public class SparkHealingBeamEffect : StandardActorEffect
{
	private float m_tetherDistance;
	private bool m_checkTetherRemovalBetweenPhases;
	private bool m_shouldEnd;
	private int m_healingOnCaster;
	private int m_additionalEnergizedHealing;
	private int m_energyOnCasterPerTurn;
	private bool m_isEnergized;
	private int m_energizedTauntRequested = -1;
	private int m_pulseAnimIndex;
	private int m_energizedPulseAnimIndex;
	private Passive_Spark m_passive;
	private SparkHealingBeam m_healAbility;
	private SparkEnergized m_energizedAbility;
	private GameObject m_pulseSequencePrefab;
	private GameObject m_energizedPulseSequencePrefab;
	
	// custom 
	private int m_tetherDuration;

	public SparkHealingBeamEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData standardActorEffectData,
		AbilityPriority hitPhase,
		float tetherDistance,
		int tetherDuration, // custom
		bool checkTetherRemovalBetweenPhases,
		int healingOnCaster,
		int additionalHealing,
		int energyOnCasterPerTurn,
		int pulseAnimIndex,
		int energizedPulseAnimIndex,
		GameObject pulseSequencePrefab,
		GameObject energizedPulseSequencePrefab) : base(parent,
		targetSquare,
		target,
		caster,
		standardActorEffectData)
	{
        HitPhase = hitPhase;
        SetPerTurnHitDelay(1);
		m_shouldEnd = false;
		m_tetherDistance = tetherDistance;
		m_checkTetherRemovalBetweenPhases = checkTetherRemovalBetweenPhases;
		m_healingOnCaster = healingOnCaster;
		m_additionalEnergizedHealing = additionalHealing;
		m_energyOnCasterPerTurn = energyOnCasterPerTurn;
		m_pulseAnimIndex = pulseAnimIndex;
		m_energizedPulseAnimIndex = energizedPulseAnimIndex;
		m_pulseSequencePrefab = pulseSequencePrefab;
		m_energizedPulseSequencePrefab = energizedPulseSequencePrefab;
		m_passive = Caster.GetComponent<Passive_Spark>();
		m_healAbility = (Caster.GetAbilityData().GetAbilityOfType(typeof(SparkHealingBeam)) as SparkHealingBeam);
		m_energizedAbility = (Caster.GetAbilityData().GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized);
		
		// custom
		m_tetherDuration = tetherDuration;
	}

	public void SetEnergized(bool energized)
	{
		m_isEnergized = energized;
	}

	public void SetEnergizedTauntRequested(int tauntNumber)
	{
		m_energizedTauntRequested = tauntNumber;
	}

	public bool IsEnergized()
	{
		if (HitPhase >= AbilityPriority.Evasion || !(m_energizedAbility != null) || !ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(SparkEnergized)))
		{
			return m_isEnergized;
		}
		if (m_energizedAbility.NeedToChooseActor())
		{
			List<AbilityTarget> targetingDataOfStoredAbility = ServerActionBuffer.Get().GetTargetingDataOfStoredAbility(Caster, typeof(SparkEnergized));
			if (targetingDataOfStoredAbility != null && targetingDataOfStoredAbility.Count > 0)
			{
				BoardSquare square = Board.Get().GetSquare(targetingDataOfStoredAbility[0].GridPos);
				if (square != null && square == Target.GetCurrentBoardSquare())
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public void SetPulseAnimIndex(int pulseAnimIndex)
	{
		m_pulseAnimIndex = pulseAnimIndex;
	}

	public override int GetHealingPerTurn()
	{
		int num = base.GetHealingPerTurn();
		if (IsEnergized())
		{
			int num2 = m_additionalEnergizedHealing;
			if (m_energizedAbility != null)
			{
				num2 = m_energizedAbility.CalcAdditionalHealOnCast(m_additionalEnergizedHealing);
			}
			num += num2;
		}
		return num;
	}

	public override int GetExpectedHealOverTimeTotal()
	{
		return 0;
	}

	public override int GetExpectedHealOverTimeThisTurn()
	{
		return 0;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] array = m_effectResults.HitActorsArray();
		if (array.Length != 0)
		{
			GameObject prefab = IsEnergized() ? m_energizedPulseSequencePrefab : m_pulseSequencePrefab;
			foreach (ActorData actorData in array)
			{
				if (actorData != Caster)
				{
					ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(prefab, actorData.GetCurrentBoardSquare(), actorData.AsArray(), Caster, SequenceSource, null);
					list.Add(item);
				}
				else
				{
					ServerClientUtils.SequenceStartData item2 = new ServerClientUtils.SequenceStartData(SequenceLookup.Get().GetSimpleHitSequencePrefab(), Caster.GetCurrentBoardSquare(), Caster.AsArray(), Caster, SequenceSource, null);
					list.Add(item2);
				}
			}
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!IsAbilityQueued() && m_time.age >= m_perTurnHitDelay)
		{
			int num = 0;
			if (m_healAbility != null && m_healAbility.UseBonusHealing())
			{
				num = m_healAbility.GetBonusHealFromTetherAge(m_time.age);
			}
			if (m_passive != null && m_passive.GetHealBeamPulseAnimIndex() > 0)
			{
				if (m_pulseAnimIndex > 0)
				{
					foreach (Effect effect in ServerEffectManager.Get().GetAllActorEffectsByCaster(Caster, typeof(SparkHealingBeamEffect)))
					{
						SparkHealingBeamEffect sparkHealingBeamEffect = effect as SparkHealingBeamEffect;
						if (!sparkHealingBeamEffect.IsSkippingGatheringResults())
						{
							ActorHitResults actorHitResults = sparkHealingBeamEffect.BuildMainTargetHitResults();
							if (actorHitResults != null)
							{
								if (num > 0)
								{
									actorHitResults.AddBaseHealing(num);
								}
								effectResults.StoreActorHit(actorHitResults);
							}
						}
					}
				}
			}
			else
			{
				ActorHitResults actorHitResults2 = BuildMainTargetHitResults();
				if (actorHitResults2 != null)
				{
					if (num > 0)
					{
						actorHitResults2.AddBaseHealing(num);
					}
					effectResults.StoreActorHit(actorHitResults2);
				}
			}
			int healingOnCaster = m_healingOnCaster;
			int energyOnCasterPerTurn = m_energyOnCasterPerTurn;
			if (healingOnCaster > 0 || energyOnCasterPerTurn > 0)
			{
				ActorHitResults actorHitResults3 = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
				actorHitResults3.SetBaseHealing(healingOnCaster);
				actorHitResults3.SetTechPointGain(energyOnCasterPerTurn);
				effectResults.StoreActorHit(actorHitResults3);
			}
		}
	}

	public override void OnBeforeGatherEffectResults(AbilityPriority phase)
	{
		if (phase == HitPhase
			&& m_passive != null
			&& m_passive.GetHealBeamPulseAnimIndex() > 0)
		{
			m_passive.SetPulseAnimIndexOnFirstBeams();
		}
	}

	public float GetTotalDistanceFromTarget()
	{
		return Target.GetCurrentBoardSquare().HorizontalDistanceInSquaresTo(Caster.GetCurrentBoardSquare());
	}

	private void CheckTetherDistance()
	{
		if (Target.IsDead()
			|| Target.GetCurrentBoardSquare() == null
			|| Caster.IsDead()
			|| Caster.GetCurrentBoardSquare() == null
			|| GetTotalDistanceFromTarget() > m_tetherDistance)
		{
			m_shouldEnd = true;
		}
	}

	public bool IsAbilityQueued()
	{
		return ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(SparkHealingBeam));
	}

	public bool IsSkippingGatheringResults()
	{
		return IsAbilityQueued();
	}

	public override void OnStart()
	{
		base.OnStart();
		ActorData component = Target.GetComponent<ActorData>();
		if (component != null)
		{
            Caster.GetComponent<SparkBeamTrackerComponent>().AddBeamActorByIndex(component.ActorIndex);
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		ActorData component = Target.GetComponent<ActorData>();
		if (component != null)
		{
            Caster.GetComponent<SparkBeamTrackerComponent>().RemoveBeamActorByIndex(component.ActorIndex);
			if (m_passive != null)
			{
				m_passive.SetPulseAnimIndexOnFirstBeams();
			}
		}
		
		// custom
		// TODO SPARK depending on how you define tether duration, might be off by one turn (is it even used?)
		if (m_tetherDuration > 0 && m_time.age >= m_tetherDuration)
		{
			m_shouldEnd = true;
		}
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (Target != null)
		{
            Caster.GetComponent<SparkBeamTrackerComponent>().UpdateTetherDuration(Target.ActorIndex, m_time.age);
            Caster.GetComponent<FreelancerStats>().IncrementValueOfStat(FreelancerStats.SparkStats.TurnsTetheredToAllies);
		}
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		CheckTetherDistance();
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		if (m_checkTetherRemovalBetweenPhases)
		{
			CheckTetherDistance();
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || m_shouldEnd;
	}

	public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		if (IsAbilityQueued()
			|| m_shouldEnd
			|| m_time.age < m_perTurnHitDelay 
			| phaseIndex != HitPhase)
		{
			return 0;
		}
		if (IsEnergized())
		{
			return m_energizedPulseAnimIndex;
		}
		return m_pulseAnimIndex;
	}

	public override int GetCinematicRequested(AbilityPriority phaseIndex)
	{
		if (!IsAbilityQueued()
			&& !m_shouldEnd
			&& m_time.age >= m_perTurnHitDelay
			&& phaseIndex == HitPhase
			&& IsEnergized()
			&& m_energizedTauntRequested >= 0)
		{
			return m_energizedTauntRequested;
		}
		return 0;
	}

	public override bool CasterMustHaveAccuratePositionOnClients()
	{
		return true;
	}

	public override bool TargetMustHaveAccuratePositionOnClients()
	{
		return true;
	}
}
#endif
