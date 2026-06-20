// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class SparkBasicAttackEffect : StandardActorEffect
{
	private float m_tetherDistance;
	private bool m_shouldEnd;
	private int m_healOnCasterOnTick;
	private int m_energyOnCasterPerTurn;
	private int m_additionalEnergizedDamage;
	private bool m_isEnergized;
	private int m_energizedTauntRequested = -1;
	private int m_pulseAnimIndex;
	private int m_energizedPulseAnimIndex;

	private Passive_Spark m_passive;
	private SparkBeamTrackerComponent m_syncComp;
	private SparkBasicAttack m_damageAbility;
	private SparkEnergized m_energizedAbility;
	private GameObject m_pulseSequencePrefab;
	private GameObject m_energizedPulseSequencePrefab;
	
	// custom 
	private int m_tetherDuration;

	public SparkBasicAttackEffect(
		EffectSource parent,
        BoardSquare targetSquare,
        ActorData target,
        ActorData caster,
        StandardActorEffectData standardActorEffectData,
        float tetherDistance,
		int tetherDuration, // custom
        int healOnCaster,
        int additionalDamage,
        int energyOnCasterPerTurn,
        int pulseAnimIndex,
        int energizedPulseAnimIndex,
        GameObject pulseSequencePrefab,
        GameObject energizedPulseSequencePrefab)
		: base(parent, targetSquare, target, caster, standardActorEffectData)
	{
        SetPerTurnHitDelay(1);
		m_shouldEnd = false;
		m_tetherDistance = tetherDistance;
		m_additionalEnergizedDamage = additionalDamage;
		m_healOnCasterOnTick = healOnCaster;
		m_energyOnCasterPerTurn = energyOnCasterPerTurn;
		m_pulseAnimIndex = pulseAnimIndex;
		m_energizedPulseAnimIndex = energizedPulseAnimIndex;
		m_pulseSequencePrefab = pulseSequencePrefab;
		m_energizedPulseSequencePrefab = energizedPulseSequencePrefab;
		m_passive = Caster.GetComponent<Passive_Spark>();
		m_damageAbility = (Caster.GetAbilityData().GetAbilityOfType(typeof(SparkBasicAttack)) as SparkBasicAttack);
		m_energizedAbility = (Caster.GetAbilityData().GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized);
		m_syncComp = Caster.GetComponent<SparkBeamTrackerComponent>();
		
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

	public void SetPulseAnimIndex(int pulseAnimIndex)
	{
		m_pulseAnimIndex = pulseAnimIndex;
	}

	public override int GetDamagePerTurn()
	{
		int damagePerTurn = base.GetDamagePerTurn();
		if (m_isEnergized)
		{
			int additionalDamage = m_additionalEnergizedDamage;
			if (m_energizedAbility != null)
			{
				additionalDamage = m_energizedAbility.CalcAdditonalDamageOnCast(m_additionalEnergizedDamage);
			}
			damagePerTurn += additionalDamage;
		}
		if (m_damageAbility != null && m_damageAbility.UseBonusDamageOverTime())
		{
			int bonusDamageFromTetherAge = m_damageAbility.GetBonusDamageFromTetherAge(m_time.age);
			damagePerTurn += bonusDamageFromTetherAge;
		}
		return damagePerTurn;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] array = m_effectResults.HitActorsArray();
		if (array.Length != 0)
		{
			GameObject prefab = m_isEnergized ? m_energizedPulseSequencePrefab : m_pulseSequencePrefab;
			foreach (ActorData actorData in array)
			{
				if (actorData != Caster)
				{
					ServerClientUtils.SequenceStartData data = new ServerClientUtils.SequenceStartData(prefab, actorData.GetCurrentBoardSquare(), actorData.AsArray(), Caster, SequenceSource, null);
					list.Add(data);
				}
				else
				{
					ServerClientUtils.SequenceStartData data = new ServerClientUtils.SequenceStartData(SequenceLookup.Get().GetSimpleHitSequencePrefab(), Caster.GetCurrentBoardSquare(), Caster.AsArray(), Caster, SequenceSource, null);
					list.Add(data);
				}
			}
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (IsAbilityQueued() || m_time.age < m_perTurnHitDelay)
		{
			Log.Info($"SPARK {Caster.DisplayName} not gathering effect results IsAbilityQueued()={IsAbilityQueued()} m_time.age={m_time.age} m_perTurnHitDelay={m_perTurnHitDelay}");
			return;
		}
		if (m_passive != null && m_passive.GetDamageBeamPulseAnimIndex() > 0)
		{
			if (m_pulseAnimIndex > 0)
			{
				foreach (Effect effect in ServerEffectManager.Get().GetAllActorEffectsByCaster(Caster, typeof(SparkBasicAttackEffect)))
				{
					SparkBasicAttackEffect sparkBasicAttackEffect = effect as SparkBasicAttackEffect;
					if (!sparkBasicAttackEffect.IsSkippingGatheringResults())
					{
						ActorHitResults actorHitResults = sparkBasicAttackEffect.BuildMainTargetHitResults();
						if (actorHitResults != null)
						{
							effectResults.StoreActorHit(actorHitResults);
						}
					}
					else
					{
						Log.Info($"SPARK {Caster.DisplayName} skipping gathering effect results");
					}
				}
			}
			else
			{
				Log.Info($"SPARK {Caster.DisplayName} not gathering effect results m_pulseAnimIndex={m_pulseAnimIndex}");
			}
		}
		else
		{
			Log.Info($"SPARK {Caster.DisplayName} not gathering effect results m_passive == null={m_passive == null} m_passive.GetDamageBeamPulseAnimIndex()={m_passive?.GetDamageBeamPulseAnimIndex()}");
			base.GatherEffectResults(ref effectResults, isReal);
		}
		int energyGainCyclePeriod = 1;  // TODO SPARK unused
		int techPointOnCasterOnTick = 0;
		if (m_damageAbility != null)
		{
			if (m_time.age > 0 && (m_time.age + 1) % energyGainCyclePeriod == 0)
			{
				energyGainCyclePeriod = Mathf.Max(1, m_damageAbility.GetEnergyGainCyclePeriod());
				techPointOnCasterOnTick = m_damageAbility.GetEnergyGainPerCycle();
			}
			int bonusEnergyFromGrowingGain = 0;
			int maxBonusEnergyFromGrowingGain = m_damageAbility.GetMaxBonusEnergyFromGrowingGain();
			int bonusEnergyGrowthRate = m_damageAbility.GetBonusEnergyGrowthRate();
			if (bonusEnergyGrowthRate > 0)
			{
				bonusEnergyFromGrowingGain = m_time.age * bonusEnergyGrowthRate;
			}
			if (maxBonusEnergyFromGrowingGain > 0)
			{
				bonusEnergyFromGrowingGain = Mathf.Min(bonusEnergyFromGrowingGain, maxBonusEnergyFromGrowingGain);
			}
			techPointOnCasterOnTick += bonusEnergyFromGrowingGain;
		}
		techPointOnCasterOnTick += m_energyOnCasterPerTurn;
		int healOnCasterOnTick = m_healOnCasterOnTick;
		if (healOnCasterOnTick > 0 || techPointOnCasterOnTick > 0)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
			if (healOnCasterOnTick > 0)
			{
				casterHitResults.SetBaseHealing(healOnCasterOnTick);
			}
			if (techPointOnCasterOnTick > 0)
			{
				casterHitResults.SetTechPointGain(techPointOnCasterOnTick);
			}
			effectResults.StoreActorHit(casterHitResults);
		}
	}

	public override void OnBeforeGatherEffectResults(AbilityPriority phase)
	{
		if (phase == HitPhase && m_passive != null && m_passive.GetDamageBeamPulseAnimIndex() > 0)
		{
			m_passive.SetPulseAnimIndexOnFirstBeams();
		}
	}

	public float GetTotalDistanceFromTarget()
	{
		return Target.GetCurrentBoardSquare().HorizontalDistanceInSquaresTo(Caster.GetCurrentBoardSquare());
	}

	public override void OnStart()
	{
		base.OnStart();
		if (Target != null)
		{
			m_syncComp.AddBeamActorByIndex(Target.ActorIndex);
		}
	}

	public bool IsAbilityQueued()
	{
		bool dashingAtCurrentTarget = ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(SparkDash))
		                              && ServerActionBuffer.Get().GetGatheredActorsOfStoredAbility(Caster, typeof(SparkDash)).Contains(Target);
		bool creatingNewTether = ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(SparkBasicAttack));
		Log.Info($"SPARK {Caster.DisplayName} dashingAtCurrentTarget={dashingAtCurrentTarget} creatingNewTether={creatingNewTether}");
        return dashingAtCurrentTarget || creatingNewTether;
	}

	public bool IsSkippingGatheringResults()
	{
		return IsAbilityQueued();
	}

	public override void OnEnd()
	{
		base.OnEnd();
		ActorData component = Target.GetComponent<ActorData>();
		if (component != null)
		{
			m_syncComp.RemoveBeamActorByIndex(component.ActorIndex);
			if (m_passive != null)
			{
				m_passive.SetPulseAnimIndexOnFirstBeams();
			}
		}
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

	public float GetMaxTetherDist()
	{
		return m_tetherDistance;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (Target != null)
		{
			m_syncComp.UpdateTetherDuration(Target.ActorIndex, m_time.age);
            Caster.GetComponent<FreelancerStats>().IncrementValueOfStat(FreelancerStats.SparkStats.TurnsTetheredToEnemies);
		}
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		CheckTetherDistance();
		
		// custom
		// TODO SPARK depending on how you define tether duration, might be off by one turn (is it even used?)
		if (m_tetherDuration > 0 && m_time.age >= m_tetherDuration)
		{
			m_shouldEnd = true;
		}
	}

	public void RemoveRevealedStatusForEvadeOutOfRange()
	{
        RemoveRevealedStatus();
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		CheckTetherDistance();
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
			|| phaseIndex != HitPhase)
		{
			return 0;
		}
		if (m_isEnergized)
		{
			return m_energizedPulseAnimIndex;
		}
		return m_pulseAnimIndex;
	}

	public override int GetCinematicRequested(AbilityPriority phaseIndex)
	{
		if (IsAbilityQueued()
			|| m_shouldEnd
			|| m_time.age < m_perTurnHitDelay
			|| phaseIndex != HitPhase
			|| !m_isEnergized
			|| m_energizedTauntRequested < 0)
		{
			return 0;
		}
		if (m_energizedAbility != null
			&& !m_energizedAbility.NeedToChooseActor()
			&& ServerEffectManager.Get().GetAllActorEffectsByCaster(Caster, typeof(SparkHealingBeamEffect)).Count > 0)
		{
			return 0;
		}
		return m_energizedTauntRequested;
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
