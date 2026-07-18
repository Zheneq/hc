// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// server-only, missing in reactor
#if SERVER
public class StandardActorEffect : Effect
{
	public StandardActorEffectData m_data;
	private bool m_invisibilityBroken;
	private bool m_invisibilityRemoved;
	private bool m_started;
	protected int m_perTurnHitDelay;
	private bool m_shouldPlaySequences = true;
	private List<StatusType> m_statusAdded = new List<StatusType>();
	private List<StatusType> m_statusesToAddOnTurnStart = new List<StatusType>();
	private int m_absorbToAddOnTurnStart;
	private bool m_canBeDispelledByStatus;

	public StandardActorEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, StandardActorEffectData data)
		: base(parent, targetSquare, target, caster)
	{
		InitFromData(data);
		m_canBeDispelledByStatus = base.CanBeDispelledByStatusImmunity();
	}

	public StandardActorEffect(ActorHitParameters hitParams, StandardActorEffectData data)
		: base(hitParams.GetEffectSource(), hitParams.Target.GetCurrentBoardSquare(), hitParams.Target, hitParams.Caster)
	{
		InitFromData(data);
	}

	private void InitFromData(StandardActorEffectData data)
	{
		m_data = data;
        HitPhase = AbilityPriority.Combat_Damage;
		m_time.duration = m_data.m_duration;
		m_effectName = m_data.m_effectName;
		m_perTurnHitDelay = m_data.m_perTurnHitDelayTurns;
		if (m_data.m_duration > 0 && m_data.m_maxStackSize > 0)
		{
            InitStacking(m_data.m_duration, m_data.m_maxStackSize, m_data.m_maxStackSize / m_data.m_duration);
		}
		foreach (EffectEndTag item in m_data.m_endTriggers)
		{
			m_endTags.Add(item);
		}
		if (m_perTurnHitDelay <= 0)
		{
            InitAbsorbtion(m_data.m_absorbAmount);
		}
	}

	public void SetDurationBeforeStart(int newDuration)
	{
		if (!m_started)
		{
			m_time.duration = newDuration;
		}
	}

	public void SetHitPhaseBeforeStart(AbilityPriority phase)
	{
		if (!m_started)
		{
            HitPhase = phase;
			return;
		}
		if (Application.isEditor)
		{
			Debug.LogError("Effect[ " + m_effectName + " ] trying to set hit phase after it has started");
		}
	}

	public void SetPlaySequenceBeforeStart(bool shouldPlay)
	{
		if (!m_started)
		{
			m_shouldPlaySequences = shouldPlay;
		}
	}

	public void SetPerTurnHitDelay(int delay)
	{
		if (!m_started)
		{
			m_perTurnHitDelay = delay;
		}
	}

	public void SetNextTurnAbsorbOverride(int nextTurnAbsorb)
	{
		if (!m_started)
		{
			// custom
			m_data.m_nextTurnAbsorbAmount = nextTurnAbsorb;
			// rogues - will be overriden OnStart in AddMods
			// m_absorbToAddOnTurnStart = nextTurnAbsorb;
		}
	}

	private CombatTextCategory GetGainTextCategory()
	{
		if (Caster.GetTeam() != Target.GetTeam())
		{
			return CombatTextCategory.DebuffGain;
		}
		return CombatTextCategory.BuffGain;
	}

	private CombatTextCategory GetLossTextCategory()
	{
		if (Caster.GetTeam() != Target.GetTeam())
		{
			return CombatTextCategory.DebuffLoss;
		}
		return CombatTextCategory.BuffLoss;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_shouldPlaySequences && m_data.m_sequencePrefabs.Length != 0)
		{
			GameObject[] sequencePrefabs = m_data.m_sequencePrefabs;
			for (int i = 0; i < sequencePrefabs.Length; i++)
			{
				ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(sequencePrefabs[i], TargetSquare, Target.AsArray(), Caster, SequenceSource, null);
				list.Add(item);
			}
		}
		return list;
	}

	public override void OnStart()
	{
		m_started = true;
		if (m_data.m_techPointChangeOnStart > 0)
		{
			ServerCombatManager.Get().TechPointGain(Parent, Caster, Target, m_data.m_techPointChangeOnStart, ServerCombatManager.TechPointChangeType.Effect);
		}
		if (m_data.m_techPointChangeOnStart < 0)
		{
			ServerCombatManager.Get().TechPointLoss(Parent, Caster, Target, Mathf.Abs(m_data.m_techPointChangeOnStart), ServerCombatManager.TechPointChangeType.Effect);
		}
		AddMods();
		if (Caster != null
		    && Caster.GetActorStats() != null
		    && !IgnoredForStatistics) // custom
		{
			int maxAbsorbAmount = GetMaxAbsorbAmount();
			GameplayMetricHelper.CollectPotentialAbsorbDealt(Caster, maxAbsorbAmount, Parent.Ability);
		}
	}

	public override void OnEnd()
	{
		if (!Stacking.stackCount.IsNullOrEmpty<int>())
		{
			int num = Stacking.stackCount[Stacking.stackCount.Length - 1];
			for (int i = 0; i < num; i++)
			{
				RemoveMods();
			}
		}

		// TODO EFFECTS might break stacking
		// custom -- statuses don't seem to be removed otherwise
		RemoveMods();
	}

	public override void OnStackCountChanged(int previousStackCount, int previousAge)
	{
		int num = Mathf.Abs(GetCurrentStackCount() - previousStackCount);
		bool flag = num > 0;
		for (int i = 0; i < num; i++)
		{
			if (flag)
			{
				AddMods();
			}
			else
			{
				RemoveMods();
			}
		}
	}

	public override void OnTurnStart()
	{
		ActorStatus actorStatus = Target.GetActorStatus();
		foreach (StatusType status in m_statusesToAddOnTurnStart)
		{
			HandleAddStatus(status, actorStatus, m_data.m_duration - 1);
		}
		m_statusesToAddOnTurnStart.Clear();
		if (m_absorbToAddOnTurnStart > 0)
		{
            InitAbsorbtion(m_absorbToAddOnTurnStart);
			if (CanAbsorb())
			{
				ServerEffectManager.Get().UpdateAbsorbPoints(this);
				if (!IgnoredForStatistics) // custom, unconditional in rogues
				{
					GameplayMetricHelper.CollectPotentialAbsorbDealt(Caster, GetMaxAbsorbAmount(), Parent.Ability);
				}
			}
		}
		m_absorbToAddOnTurnStart = 0;
		
		// custom
		int hot = GetExpectedHealOverTimeTotal();
		if (hot > 0 && Target != null && !Target.IsDead())
		{
			Target.ExpectedHoTTotal += hot;
			Target.ExpectedHoTThisTurn += GetExpectedHealOverTimeThisTurn();
			Target.SetDirtyBit(1);
		}
		// end custom
	}

	private void AddMods()
	{
		ActorStats actorStats = Target.GetActorStats();
		// rogues
		//EquipmentStats equipmentStats = Target.GetEquipmentStats();
		foreach (AbilityStatMod abilityStatMod in m_data.m_statMods)
		{
			if (abilityStatMod.stat != StatType.INVALID)
			{
				actorStats.AddStatMod(abilityStatMod.stat, abilityStatMod.modType, abilityStatMod.modValue);
			}
			// rogues
			//else if (abilityStatMod.gearStat != GearStatType.NUM)
			//{
			//	GearStatOp statOp = GearStatOp.Add;
			//	switch (abilityStatMod.modType)
			//	{
			//	case ModType.BaseAdd:
			//	case ModType.BonusAdd:
			//		statOp = GearStatOp.Raw;
			//		break;
			//	case ModType.PercentAdd:
			//		statOp = GearStatOp.Add;
			//		break;
			//	case ModType.Multiplier:
			//		statOp = GearStatOp.Multiply;
			//		break;
			//	}
			//	equipmentStats.AddActorStat(abilityStatMod.gearStat, abilityStatMod.modValue, statOp, 0, null);
			//}
		}
		List<StatusType> statusesToApplyOnEffectStart = GetStatusesToApplyOnEffectStart();
		List<StatusType> statusesOnTurnStart = GetStatusesOnTurnStart();
		ActorStatus actorStatus = Target.GetActorStatus();
		foreach (StatusType status in statusesToApplyOnEffectStart)
		{
			HandleAddStatus(status, actorStatus, m_data.m_duration);
		}
		foreach (StatusType item in statusesOnTurnStart)
		{
			m_statusesToAddOnTurnStart.Add(item);
		}
		m_absorbToAddOnTurnStart = m_data.m_nextTurnAbsorbAmount;
		if (CanApplyHealOverTime())
		{
			actorStatus.AddStatus(StatusType.HealingOverTime, m_data.m_duration);
		}
	}

	private void HandleAddStatus(StatusType status, ActorStatus targetStatus, int duration)
	{
		if (!targetStatus.IsMovementDebuffImmune(true) || !ActorStatus.IsDispellableMovementDebuff(status))
		{
			targetStatus.AddStatus(status, duration);
			m_statusAdded.Add(status);
			if (Target != null && Caster != null && Caster != Target && Target.GetActorBehavior() != null)
			{
				if (status == StatusType.Snared)
				{
                    Target.GetActorBehavior().AddSnareSourceActor(Caster);
				}
				else if (status == StatusType.Rooted)
				{
                    Target.GetActorBehavior().AddRootOrKnockbackSourceActor(Caster);
				}
			}
		}
	}

	public override void Refresh()
	{
		base.Refresh();
		ActorStatus actorStatus = Target.GetActorStatus();
		foreach (StatusType status in m_statusAdded)
		{
			actorStatus.UpdateStatusDuration(status, m_data.m_duration);
		}
	}

	private void RemoveMods()
	{
		if (NetworkServer.active)
		{
			ActorStats actorStats = Target.GetActorStats();
			// rogues
			//EquipmentStats equipmentStats = Target.GetEquipmentStats();
			foreach (AbilityStatMod abilityStatMod in m_data.m_statMods)
			{
				if (abilityStatMod.stat != StatType.INVALID)
				{
					actorStats.RemoveStatMod(abilityStatMod.stat, abilityStatMod.modType, abilityStatMod.modValue);
				}
				// rogues
				//else if (abilityStatMod.gearStat != GearStatType.NUM)
				//{
				//	GearStatOp statOp = GearStatOp.Add;
				//	switch (abilityStatMod.modType)
				//	{
				//	case ModType.BaseAdd:
				//	case ModType.BonusAdd:
				//		statOp = GearStatOp.Raw;
				//		break;
				//	case ModType.PercentAdd:
				//		statOp = GearStatOp.Add;
				//		break;
				//	case ModType.Multiplier:
				//		statOp = GearStatOp.Multiply;
				//		break;
				//	}
				//	equipmentStats.RemoveActorStat(abilityStatMod.gearStat, abilityStatMod.modValue, statOp);
				//}
			}
			ActorStatus component = Target.GetComponent<ActorStatus>();
			for (int j = 0; j < m_statusAdded.Count; j++)
			{
				component.RemoveStatus(m_statusAdded[j]);
			}
			if (CanApplyHealOverTime())
			{
				component.RemoveStatus(StatusType.HealingOverTime);
			}
		}
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		if (phase == AbilityPriority.Prep_Defense && m_time.duration > 0 && m_time.age == m_time.duration - 1)
		{
			if (m_data.m_removeInvisibilityOnLastResolveStart && IsInvisibilityEffect())
			{
				RemoveInvisibilityStatus();
			}
			if (m_data.m_removeRevealedOnLastResolveStart)
			{
				RemoveRevealedStatus();
			}
		}
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_effectResults.StoredHitForActor(Target) && m_data.m_tickSequencePrefab != null)
		{
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_data.m_tickSequencePrefab, Target.GetCurrentBoardSquare(), Target.AsArray(), Caster, SequenceSource, null);
			list.Add(item);
		}
		return list;
	}

	public override void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		int damagePerMoveSquare = GetDamagePerMoveSquare();
		int healPerMoveSquare = GetHealPerMoveSquare();
		int techPointLossPerMoveSquare = GetTechPointLossPerMoveSquare();
		int techPointGainPerMoveSquare = GetTechPointGainPerMoveSquare();
		if (damagePerMoveSquare > 0 || healPerMoveSquare > 0 || techPointLossPerMoveSquare > 0 || techPointGainPerMoveSquare > 0)
		{
			foreach (MovementInstance movementInstance in movement.m_movementInstances)
			{
				if (movementInstance.m_mover == Target && movementInstance.m_groundBased)
				{
					float num = movementInstance.m_path.FindMoveCostToEnd();
					if (num > 0f)
					{
						BoardSquare square = movementInstance.m_path.GetPathEndpoint().square;
						ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, square.ToVector3()));
						if (damagePerMoveSquare > 0 && healPerMoveSquare == 0)
						{
							actorHitResults.AddBaseDamage(Mathf.RoundToInt((float)damagePerMoveSquare * num));
						}
						if (healPerMoveSquare > 0 && damagePerMoveSquare == 0)
						{
							actorHitResults.AddBaseHealing(Mathf.RoundToInt((float)healPerMoveSquare * num));
						}
						if (techPointLossPerMoveSquare > 0 && techPointGainPerMoveSquare == 0)
						{
							actorHitResults.AddTechPointLoss(Mathf.RoundToInt((float)techPointLossPerMoveSquare * num));
						}
						if (techPointGainPerMoveSquare > 0 && techPointLossPerMoveSquare == 0)
						{
							actorHitResults.AddTechPointGain(Mathf.RoundToInt((float)techPointGainPerMoveSquare * num));
						}
						MovementResults item = new MovementResults(movement.m_movementStage, this, actorHitResults, Target, movementInstance.m_path.GetPathEndpoint(), null, square, SequenceSource, null);
						movementResultsList.Add(item);
					}
				}
			}
		}
	}

	public virtual int GetDamagePerMoveSquare()
	{
		return m_data.m_damagePerMoveSquare;
	}

	public virtual int GetHealPerMoveSquare()
	{
		return m_data.m_healPerMoveSquare;
	}

	public virtual int GetTechPointLossPerMoveSquare()
	{
		return m_data.m_techPointLossPerMoveSquare;
	}

	public virtual int GetTechPointGainPerMoveSquare()
	{
		return m_data.m_techPointGainPerMoveSquare;
	}

	public override void GatherMovementResultsFromSegment(ActorData mover, MovementInstance movementInstance, MovementStage movementStage, BoardSquarePathInfo sourcePath, BoardSquarePathInfo destPath, ref List<MovementResults> movementResultsList)
	{
		if (mover == Target && movementInstance.m_groundBased)
		{
			int damagePerMoveSquare = GetDamagePerMoveSquare();
			int healPerMoveSquare = GetHealPerMoveSquare();
			int techPointLossPerMoveSquare = GetTechPointLossPerMoveSquare();
			int techPointGainPerMoveSquare = GetTechPointGainPerMoveSquare();
			if (damagePerMoveSquare > 0 || healPerMoveSquare > 0 || techPointLossPerMoveSquare > 0 || techPointGainPerMoveSquare > 0)
			{
				float num = destPath.moveCost - sourcePath.moveCost;
				if (num > 0f)
				{
					BoardSquare square = destPath.square;
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, square.ToVector3()));
					if (damagePerMoveSquare > 0 && healPerMoveSquare == 0)
					{
						actorHitResults.AddBaseDamage(Mathf.RoundToInt((float)damagePerMoveSquare * num));
					}
					if (healPerMoveSquare > 0 && damagePerMoveSquare == 0)
					{
						actorHitResults.AddBaseHealing(Mathf.RoundToInt((float)healPerMoveSquare * num));
					}
					if (techPointLossPerMoveSquare > 0 && techPointGainPerMoveSquare == 0)
					{
						actorHitResults.AddTechPointLoss(Mathf.RoundToInt((float)techPointLossPerMoveSquare * num));
					}
					if (techPointGainPerMoveSquare > 0 && techPointLossPerMoveSquare == 0)
					{
						actorHitResults.AddTechPointGain(Mathf.RoundToInt((float)techPointGainPerMoveSquare * num));
					}
					MovementResults item = new MovementResults(movementStage, this, actorHitResults, Target, destPath, null, square, SequenceSource, null);
					movementResultsList.Add(item);
				}
			}
		}
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		ActorHitResults actorHitResults = BuildMainTargetHitResults();
		if (actorHitResults != null)
		{
			effectResults.StoreActorHit(actorHitResults);
		}
	}

	public virtual int GetDamagePerTurn()
	{
		return m_data.m_damagePerTurn;
	}

	public virtual int GetHealingPerTurn()
	{
		return m_data.m_healingPerTurn;
	}

	// rogues
	//public virtual float GetDamageCoeff()
	//{
	//	return m_data.m_damageCoeff;
	//}

	// rogues
	//public virtual float GetHealingCoeff()
	//{
	//	return m_data.m_healingCoeff;
	//}

	public virtual int GetTechPointGainPerTurn()
	{
		return m_data.m_techPointGainPerTurn;
	}

	public virtual int GetTechPointLossPerTurn()
	{
		return m_data.m_techPointLossPerTurn;
	}

	protected ActorHitResults BuildMainTargetHitResults()
	{
		int num = Mathf.Max(GetCurrentStackCount(), 1);
		ActorHitResults actorHitResults;
		if (m_time.age >= m_perTurnHitDelay
			&& (GetDamagePerTurn() > 0
				|| GetHealingPerTurn() > 0
				|| m_data.m_techPointGainPerTurn > 0
				|| m_data.m_techPointLossPerTurn > 0
				// rogues
				//|| GetDamageCoeff() > 0f
				//|| GetHealingCoeff() > 0f
				))
		{
			actorHitResults = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
			if (GetDamagePerTurn() > 0)
			{
				actorHitResults.AddBaseDamage(GetDamagePerTurn() * num);
			}
			else if (GetHealingPerTurn() > 0)
			{
				actorHitResults.AddBaseHealing(GetHealingPerTurn() * num);
				actorHitResults.SetIsPartOfHealOverTime(true);
			}

			// rogues
			//else if (GetDamageCoeff() > 0f)
			//{
			//	actorHitResults.ModifyDamageCoeff(GetDamageCoeff(), GetDamageCoeff());
			//}
			//else if (GetHealingCoeff() > 0f)
			//{
			//	actorHitResults.ModifyHealingCoeff(GetHealingCoeff(), GetHealingCoeff());
			//}

			if (GetTechPointGainPerTurn() > 0)
			{
				actorHitResults.AddTechPointGain(GetTechPointGainPerTurn() * num);
			}
			else if (GetTechPointLossPerTurn() > 0)
			{
				actorHitResults.AddTechPointLoss(GetTechPointLossPerTurn() * num);
			}
		}
		else
		{
			actorHitResults = null;
		}
		return actorHitResults;
	}

	public override int GetExpectedHealOverTimeTotal()
	{
		int result = 0;
		if (m_time.duration > 0)
		{
			int num = Mathf.Max(m_time.age, m_perTurnHitDelay);
			num = Mathf.Max(1, num);
			int num2 = m_time.duration - num;
			result = Mathf.Max(0, GetHealingPerTurn() * num2);
		}
		return result;
	}

	public override int GetExpectedHealOverTimeThisTurn()
	{
		int result = 0;
		if (m_time.duration > 0 && m_time.age >= m_perTurnHitDelay)
		{
			return Mathf.Max(0, GetHealingPerTurn());
		}
		return result;
	}

	public override bool CanStackWith(Effect otherEffect)
	{
		if (!base.CanStackWith(otherEffect))
		{
			return false;
		}
		if (otherEffect is StandardActorEffect)
		{
			StandardActorEffect standardActorEffect = otherEffect as StandardActorEffect;
			return m_data.Equals(standardActorEffect.m_data);
		}
		return false;
	}

	protected bool IsInvisibilityEffect()
	{
		bool result = false;
		foreach (StatusType statusType in m_data.m_statusChanges)
		{
			if (statusType == StatusType.InvisibleToEnemies
				|| statusType == StatusType.ProximityBasedInvisibility)
			{
				return true;
			}
        }
        return result;
	}

	public override void OnBreakInvisibility()
	{
		if (IsInvisibilityEffect())
		{
			if (m_data.m_invisBreakMode == StandardActorEffectData.InvisibilityBreakMode.RemoveInvisAndEndEarly)
			{
				if (!m_invisibilityRemoved)
				{
					for (int i = m_statusAdded.Count - 1; i >= 0; i--)
					{
						StatusType statusType = m_statusAdded[i];
						if (statusType == StatusType.InvisibleToEnemies)
						{
                            Target.GetActorStatus().RemoveStatus(statusType);
							m_statusAdded.RemoveAt(i);
						}
						else if (statusType == StatusType.ProximityBasedInvisibility)
						{
                            Target.GetActorStatus().RemoveStatus(statusType);
							m_statusAdded.RemoveAt(i);
						}
					}
					m_invisibilityBroken = true; // custom - do not consider invis broken if it's already removed
				}
				// m_invisibilityBroken = true; // rogues
				m_invisibilityRemoved = true;
			}
			else if (m_data.m_invisBreakMode == StandardActorEffectData.InvisibilityBreakMode.SuppressOnly && Target.GetAbilityData() != null)
			{
                Target.GetAbilityData().SuppressInvisibility();
			}
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Log.Warning("<color=green>Effect</color>: " + GetDebugIdentifier("yellow") + " breaking Invisibility");
			}
		}
	}

	protected void RemoveInvisibilityStatus()
	{
		if (!m_invisibilityRemoved)
		{
			for (int i = m_statusAdded.Count - 1; i >= 0; i--)
			{
				StatusType statusType = m_statusAdded[i];
				if (statusType == StatusType.InvisibleToEnemies)
				{
                    Target.GetActorStatus().RemoveStatus(statusType);
					m_statusAdded.RemoveAt(i);
				}
				else if (statusType == StatusType.ProximityBasedInvisibility)
				{
                    Target.GetActorStatus().RemoveStatus(statusType);
					m_statusAdded.RemoveAt(i);
				}
			}
			m_invisibilityRemoved = true;
		}
	}

	protected void RemoveRevealedStatus()
	{
		for (int i = m_statusAdded.Count - 1; i >= 0; i--)
		{
			StatusType statusType = m_statusAdded[i];
			if (statusType == StatusType.Revealed)
			{
                Target.GetActorStatus().RemoveStatus(statusType);
				m_statusAdded.RemoveAt(i);
			}
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || m_invisibilityBroken;
	}

	public override bool HasDispellableMovementDebuff()
	{
		foreach (AbilityStatMod abilityStatMod in m_data.m_statMods)
		{
			if (abilityStatMod.stat != StatType.Movement_Horizontal)
			{
				continue;
			}
			
			bool isDebuff = abilityStatMod.modType == ModType.Multiplier
				? abilityStatMod.modValue < 1f
				: abilityStatMod.modValue < 0f;

			if (isDebuff)
			{
				return true;
			}
		}
		
		foreach (StatusType statusChange in m_data.m_statusChanges)
		{
			if (ActorStatus.IsDispellableMovementDebuff(statusChange))
			{
				return true;
			}
		}
		
		return false;
	}

	public void OverrideCanBeDispelledByStatusImmunity(bool canBeDispelled)
	{
		if (m_canBeDispelledByStatus != canBeDispelled)
		{
			m_canBeDispelledByStatus = canBeDispelled;
		}
	}

	public override bool CanBeDispelledByStatusImmunity()
	{
		return m_canBeDispelledByStatus;
	}

	public override bool WillApplyStatus(StatusType status)
	{
		StatusType[] statusChanges = m_data.m_statusChanges;
		for (int i = 0; i < statusChanges.Length; i++)
		{
			if (statusChanges[i] == status)
			{
				return true;
			}
		}
		return false;
	}

	private bool CanDelayRelevantStatuses()
	{
		return false;
	}

	private List<StatusType> GetStatusesToApplyOnEffectStart()
	{
		List<StatusType> list = new List<StatusType>();
		if (m_data.m_statusChanges != null)
		{
			if (m_data.m_statusDelayMode == StandardActorEffectData.StatusDelayMode.AllStatusesDelayToTurnStart)
			{
				list.Clear();
			}
			else if (m_data.m_statusDelayMode == StandardActorEffectData.StatusDelayMode.NoStatusesDelayToTurnStart)
			{
				foreach (StatusType item in m_data.m_statusChanges)
				{
					list.Add(item);
				}
			}
			else if (m_data.m_statusDelayMode == StandardActorEffectData.StatusDelayMode.DefaultBehavior)
			{
				if (CanDelayRelevantStatuses())
				{
					List<StatusType> statusesToDelayFromCombatToNextTurn = GameWideData.Get().m_statusesToDelayFromCombatToNextTurn;
					foreach (StatusType item2 in m_data.m_statusChanges)
					{
						if (!statusesToDelayFromCombatToNextTurn.Contains(item2))
						{
							list.Add(item2);
						}
					}
				}
				else
				{
					foreach (StatusType item3 in m_data.m_statusChanges)
					{
						list.Add(item3);
					}
				}
			}
		}
		return list;
	}

	public override List<StatusType> GetStatuses()
	{
		List<StatusType> statusesToApplyOnEffectStart = GetStatusesToApplyOnEffectStart();
		if (CanApplyHealOverTime())
		{
			statusesToApplyOnEffectStart.Add(StatusType.HealingOverTime);
		}
		return statusesToApplyOnEffectStart;
	}

	public override List<StatusType> GetStatusesOnTurnStart()
	{
		List<StatusType> list = new List<StatusType>();
		if (m_data.m_statusChanges != null)
		{
			if (m_data.m_statusDelayMode == StandardActorEffectData.StatusDelayMode.AllStatusesDelayToTurnStart)
			{
				foreach (StatusType item in m_data.m_statusChanges)
				{
					list.Add(item);
				}
			}
			else if (m_data.m_statusDelayMode == StandardActorEffectData.StatusDelayMode.NoStatusesDelayToTurnStart)
			{
				list.Clear();
			}
			else if (m_data.m_statusDelayMode == StandardActorEffectData.StatusDelayMode.DefaultBehavior && CanDelayRelevantStatuses())
			{
				List<StatusType> statusesToDelayFromCombatToNextTurn = GameWideData.Get().m_statusesToDelayFromCombatToNextTurn;
				foreach (StatusType item2 in m_data.m_statusChanges)
				{
					if (statusesToDelayFromCombatToNextTurn.Contains(item2))
					{
						list.Add(item2);
					}
				}
			}
		}
		return list;
	}

	public virtual bool CanApplyHealOverTime()
	{
		return m_data.m_healingPerTurn > 0;
	}

	public string GetDebugHeaderString()
	{
		return "<color=orange>Effect[ " + m_effectName + " ]: </color>";
	}

	public override string GetInEditorDescription()
	{
		return m_data.GetInEditorDescription("", true, false, null);
	}
	
	// custom
	protected void EndAllEffectSequences(ActorHitResults actorHitResults)
	{
		if (m_data.m_sequencePrefabs != null && m_data.m_sequencePrefabs.Length != 0)
		{
			foreach (GameObject sequencePrefab in m_data.m_sequencePrefabs)
			{
				actorHitResults.AddEffectSequenceToEnd(sequencePrefab, m_guid);
			}
		}
	}
}
#endif
