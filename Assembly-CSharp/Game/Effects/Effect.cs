// ROGUES
using System;
using System.Collections.Generic;
using System.Linq;
//using EffectSystem;
using UnityEngine;

// added in rogues
#if SERVER
public class Effect
{
	public string m_effectName = "Base Effect";

	public EffectDuration m_time;

	public int m_guid;

	private StackingData m_stacking;

	private AbsorbData m_absorbtion;

	public List<EffectEndTag> m_endTags;

	public List<EffectEndTag> m_reasonsToEndEarly;

	protected EffectResults m_effectResults;

	protected EffectResults m_effectResults_fake;

	protected List<MovementResults> m_evadeResults;

	protected List<MovementResults> m_knockbackResults;

	protected List<MovementResults> m_normalMovementResults;
	
	private bool m_ignoredForStatistics; // custom
	public bool IgnoredForStatistics => m_ignoredForStatistics;

	public Effect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster)
	{
		m_guid = ServerEffectManager.s_nextEffectGuid++;
		Parent = parent;
		TargetSquare = targetSquare;
		Target = target;
		Caster = caster;
		SequenceSource = new SequenceSource(new SequenceSource.ActorDelegate(OnHit_Base), new SequenceSource.Vector3Delegate(OnHit_Base), false, null, null);
		m_time = new EffectDuration();
		m_stacking = new StackingData
		{
			stackCount = new int[0],
			canStack = false,
			maxStackSize = 0,
			maxStackSizePerTurn = 0
		};
		m_absorbtion = default(AbsorbData);
		HitPhase = AbilityPriority.INVALID;
		if (caster != null)
		{
			CasterActorIndex = caster.ActorIndex;
		}
		else
		{
			CasterActorIndex = ActorData.s_invalidActorIndex;
		}
		if (target != null)
		{
			TargetActorIndex = target.ActorIndex;
		}
		else
		{
			TargetActorIndex = CasterActorIndex;
		}
		m_endTags = new List<EffectEndTag>();
		m_reasonsToEndEarly = new List<EffectEndTag>();
		m_effectResults = new EffectResults(this, null, true, false);
		m_effectResults_fake = new EffectResults(this, null, false, false);
		m_evadeResults = new List<MovementResults>();
		m_knockbackResults = new List<MovementResults>();
		m_normalMovementResults = new List<MovementResults>();
		if (ServerActionBuffer.Get() != null)
		{
			CreatedInAbilityPhase = ServerActionBuffer.Get().AbilityPhase;
			return;
		}
		CreatedInAbilityPhase = AbilityPriority.INVALID;
	}

	private Effect()
	{
		Log.Error("Code Error: unexpected default construction of Effect");
	}

	public ActorData Target { get; set; }

	public ActorData Caster { get; set; }

	public BoardSquare TargetSquare { get; set; }

	public EffectSource Parent { get; set; }

	public SequenceSource SequenceSource { get; protected set; }

	public AbilityPriority HitPhase { get; protected set; }

	protected AbilityPriority CreatedInAbilityPhase { get; set; }

	public int GetEffectGuid()
	{
		return m_guid;
	}

	public StackingData Stacking
	{
		get
		{
			return m_stacking;
		}
		private set
		{
			m_stacking = value;
		}
	}

	public AbsorbData Absorbtion
	{
		get
		{
			return m_absorbtion;
		}
		private set
		{
			m_absorbtion = value;
		}
	}

	public int TargetActorIndex { get; set; }

	public int CasterActorIndex { get; set; }

	// rogues
	// public virtual bool CanExecuteForTeam_FCFS(Team team)
	// {
	// 	return (Caster == null && team == Team.TeamB) || (Caster != null && Caster.GetTeam() == team) || (Caster != null && Caster.GetTeam() == Team.Objects);
	// }

	// custom
	public virtual int GetTheatricsSortPriority(AbilityData.ActionType actionType)
	{
		return 0;
	}

	public virtual bool ShouldForceReactToHit(ActorHitResults incomingHit)
	{
		return false;
	}

	public virtual bool CanReactToNormalMovementHit(ActorHitResults hit, bool isIncoming)
	{
		return false;
	}

	public virtual void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
	}

	public virtual void GatherResultsInResponseToOutgoingActorHit(ActorHitResults outgoingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
	}
	
	// custom
	public void SetIgnoredForStatistics(bool isIgnored)
	{
		m_ignoredForStatistics = isIgnored;
	}

	public void GatherResultsInResponseToEvades(MovementCollection collection)
	{
		m_evadeResults.Clear();
		GatherMovementResults(collection, ref m_evadeResults);
		foreach (MovementResults movementResults in m_evadeResults)
		{
			movementResults.m_triggeringPath.m_moverHasGameplayHitHere = true;
			movementResults.m_triggeringPath.m_updateLastKnownPos = movementResults.ShouldMovementHitUpdateTargetLastKnownPos(movementResults.m_triggeringMover);
			Log.Info($"UpdateLastKnownPos {movementResults.m_triggeringMover?.DisplayName} " +
			         $"{movementResults.m_triggeringPath.square?.GetGridPos()} " +
			         $"{(movementResults.m_triggeringPath.m_updateLastKnownPos ? "" : "NOT ")} updating for evade movement effect hit"); // custom debug
		}
	}

	public void GatherResultsInResponseToKnockbacks(MovementCollection collection)
	{
		m_knockbackResults.Clear();
		GatherMovementResults(collection, ref m_knockbackResults);
		for (int i = 0; i < m_knockbackResults.Count; i++)
		{
			m_knockbackResults[i].m_triggeringPath.m_moverHasGameplayHitHere = true;
			m_knockbackResults[i].m_triggeringPath.m_updateLastKnownPos = m_knockbackResults[i].ShouldMovementHitUpdateTargetLastKnownPos(m_knockbackResults[i].m_triggeringMover);
			TheatricsManager.Get().OnKnockbackMovementHitGathered(m_knockbackResults[i].GetTriggeringActor());
			Log.Info($"UpdateLastKnownPos {m_knockbackResults[i].m_triggeringMover?.DisplayName} " +
			         $"{m_knockbackResults[i].m_triggeringPath.square?.GetGridPos()} " +
			         $"{(m_knockbackResults[i].m_triggeringPath.m_updateLastKnownPos ? "" : "NOT ")} updating for knockback movement effect hit"); // custom debug
		}
	}

	public void ClearNormalMovementResults()
	{
		m_normalMovementResults.Clear();
	}

	public void IntegrateDamageResultsForEvasion(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		IntegrateDamageResultsForMovement(m_evadeResults, ref actorToDeltaHP);
	}

	public void IntegrateDamageResultsForKnockback(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		IntegrateDamageResultsForMovement(m_knockbackResults, ref actorToDeltaHP);
	}

	private void IntegrateDamageResultsForMovement(List<MovementResults> results, ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		for (int i = 0; i < results.Count; i++)
		{
			ServerGameplayUtils.IntegrateHpDeltas(results[i].GetMovementDamageResults(), ref actorToDeltaHP);
		}
	}

	public virtual void Resolve()
	{
		GatherEffectResults_Base(HitPhase, true);

		// rogues?
		//if (m_effectResults.GatheredResults && m_effectResults.m_actorToHitResults != null)
		//{
		//	foreach (ActorHitResults actorHitResults in m_effectResults.m_actorToHitResults.Values)
		//	{
		//		actorHitResults.ProcessEffectTemplates();
		//		if (actorHitResults.m_effectTriggers != null)
		//		{
		//			foreach (EffectTrigger effectTrigger in actorHitResults.m_effectTriggers)
		//			{
		//				ServerEffectManager.Get().ApplyEffectTrigger(effectTrigger);
		//			}
		//			actorHitResults.m_effectTriggers.Clear();
		//		}
		//	}
		//}
	}

	public void GatherEffectResults_Base(AbilityPriority phase, bool isReal)
	{
		if (phase == HitPhase)
		{
			if (m_effectResults.GatheredResults && isReal)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Gathering effect results in phase ",
					phase.ToString(),
					" for effect ",
					m_effectName,
					", but we already have results."
				}));
				return;
			}
			if (m_effectResults_fake.GatheredResults && !isReal)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Gathering fake effect results in phase ",
					phase.ToString(),
					" for effect ",
					m_effectName,
					", but we already have results."
				}));
				return;
			}
			if (isReal)
			{
				GatherEffectResults(ref m_effectResults, isReal);
				m_effectResults.FinalizeEffectResults();
				m_effectResults.GatheredResults = true;
		
				// custom
				Log.Info($"EFFECT RESULTS: {m_effectResults.Caster} with {m_effectResults.Effect.GetDebugIdentifier()}\n" +
				         $"ACTOR HIT RESULTS: {DefaultJsonSerializer.Serialize(m_effectResults.m_actorToHitResults)}\n" +
				         $"POSITION HIT RESULTS: {DefaultJsonSerializer.Serialize(m_effectResults.m_positionToHitResults)}");
				
				return;
			}
			GatherEffectResults(ref m_effectResults_fake, isReal);
			m_effectResults_fake.FinalizeEffectResults();
			m_effectResults_fake.GatheredResults = true;
		}
	}

	public virtual void OnBeforeGatherEffectResults(AbilityPriority phase)
	{
	}

	public virtual void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
	}

	public virtual void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
	}

	public virtual void GatherMovementResultsFromSegment(ActorData mover, MovementInstance movementInstance, MovementStage movementStage, BoardSquarePathInfo sourcePath, BoardSquarePathInfo destPath, ref List<MovementResults> movementResultsList)
	{
	}

	public virtual bool HitsCanBeReactedTo()
	{
		return false;
	}

	protected ActorHitResults MakeActorHitRes(ActorData target, Vector3 hitOrigin)
	{
		return new ActorHitResults(new ActorHitParameters(target, hitOrigin));
	}

	protected PositionHitResults MakePosHitRes(Vector3 position)
	{
		return new PositionHitResults(new PositionHitParameters(position));
	}

	public EffectResults GetResultsForPhase(AbilityPriority phase, bool isReal)
	{
		if (phase != HitPhase)
		{
			return null;
		}
		if (isReal)
		{
			return m_effectResults;
		}
		return m_effectResults_fake;
	}

	internal void OnHit_Base(ActorData target)
	{
		if (m_effectResults.GatheredResults)
		{
			m_effectResults.ExecuteForActor(target);
			return;
		}
		OnHit(target);
	}

	internal void OnHit_Base(Vector3 position)
	{
		if (m_effectResults.GatheredResults)
		{
			m_effectResults.ExecuteForPosition(position);
			return;
		}
		OnHit(position);
	}

	internal void ClearEffectResults()
	{
		m_effectResults.ClearResults();
		m_effectResults_fake.ClearResults();
	}

	public virtual Dictionary<ActorData, Vector2> GetEffectKnockbackTargets()
	{
		if (HitPhase == AbilityPriority.Combat_Knockback && m_effectResults.GatheredResults)
		{
			return m_effectResults.GetKnockbackTargets();
		}
		if (HitPhase == AbilityPriority.Combat_Knockback && (m_time.age != 0 || CreatedInAbilityPhase != AbilityPriority.Combat_Damage))
		{
			Debug.LogError("Trying to find knockback targets for knockback-phase effect " + m_effectName + ", but it hasn't gathered results.");
		}
		return null;
	}

	public virtual bool HasResolutionAction(AbilityPriority phase)
	{
		return phase == HitPhase && (m_effectResults.m_actorToHitResults.Count > 0 || m_effectResults.m_positionToHitResults.Count > 0 || m_effectResults.Effect.GetCasterAnimationIndex(phase) > 0);
	}

	public void ExecuteUnexecutedMovementResults_Effect(MovementStage movementStage, bool failsafe)
	{
		if (movementStage == MovementStage.Evasion)
		{
			MovementResults.ExecuteUnexecutedHits(m_evadeResults, failsafe);
			return;
		}
		if (movementStage == MovementStage.Knockback)
		{
			MovementResults.ExecuteUnexecutedHits(m_knockbackResults, failsafe);
			return;
		}
		if (movementStage == MovementStage.Normal)
		{
			MovementResults.ExecuteUnexecutedHits(m_normalMovementResults, failsafe);
		}
	}

	public void ExecuteUnexecutedMovementResultsForDistance_Effect(float distance, MovementStage movementStage, bool failsafe, out bool stillHasUnexecutedHits, out float nextUnexecutedHitDistance)
	{
		stillHasUnexecutedHits = false;
		nextUnexecutedHitDistance = -1f;
		if (movementStage == MovementStage.Evasion)
		{
			MovementResults.ExecuteUnexecutedHitsForDistance(m_evadeResults, distance, failsafe, out stillHasUnexecutedHits, out nextUnexecutedHitDistance);
			return;
		}
		if (movementStage == MovementStage.Knockback)
		{
			MovementResults.ExecuteUnexecutedHitsForDistance(m_knockbackResults, distance, failsafe, out stillHasUnexecutedHits, out nextUnexecutedHitDistance);
			return;
		}
		if (movementStage == MovementStage.Normal)
		{
			MovementResults.ExecuteUnexecutedHitsForDistance(m_normalMovementResults, distance, failsafe, out stillHasUnexecutedHits, out nextUnexecutedHitDistance);
		}
	}

	public List<MovementResults> GetMovementResultsForMovementStage(MovementStage movementStage)
	{
		if (movementStage == MovementStage.Evasion)
		{
			return m_evadeResults;
		}
		if (movementStage == MovementStage.Knockback)
		{
			return m_knockbackResults;
		}
		if (movementStage == MovementStage.Normal)
		{
			return m_normalMovementResults;
		}
		return null;
	}

	public virtual List<StatusType> GetStatuses()
	{
		return null;
	}

	public virtual List<StatusType> GetStatusesOnTurnStart()
	{
		return null;
	}

	public virtual ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return null;
	}

	public virtual List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ServerClientUtils.SequenceStartData effectStartSeqData = GetEffectStartSeqData();
		if (effectStartSeqData != null)
		{
			list.Add(effectStartSeqData);
		}
		return list;
	}

	public virtual ServerClientUtils.SequenceStartData GetEffectHitSeqData()
	{
		return null;
	}

	public virtual List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ServerClientUtils.SequenceStartData effectHitSeqData = GetEffectHitSeqData();
		if (effectHitSeqData != null)
		{
			list.Add(effectHitSeqData);
		}
		return list;
	}

	public virtual int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		return 0;
	}

	public virtual int GetCinematicRequested(AbilityPriority phaseIndex)
	{
		return -1;
	}

	public virtual bool IgnoreCameraFraming()
	{
		return false;
	}

	public virtual bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return false;
	}

	public virtual void OnActorAnimEntryPlay()
	{
	}

	public virtual Vector3 GetRotationTargetPos(AbilityPriority phaseIndex)
	{
		Vector3 result;
		if (Target != null)
		{
			result = Target.GetFreePos();
		}
		else if (TargetSquare != null)
		{
			result = TargetSquare.ToVector3();
		}
		else if (Caster != null)
		{
			result = Caster.GetFreePos();
		}
		else
		{
			result = Vector3.zero;
		}
		return result;
	}

	public virtual ActorData GetActorAnimationActor()
	{
		return Caster;
	}

	public virtual void Update()
	{
	}

	public virtual void OnStart()
	{
	}

	public virtual void OnEnd()
	{
	}

	public void End()
	{
		OnEnd();
		if (CanAbsorb())
		{
			m_absorbtion.m_absorbAmount = (m_absorbtion.m_absorbRemaining = 0);
			if (Target != null && ServerEffectManager.Get() != null)
			{
				Target.SetAbsorbPoints(ServerEffectManager.Get().CountAbsorbPoints(Target));
			}
		}
	}

	public virtual void OnTurnStart()
	{
	}

	public virtual void OnAbilityPhaseStart(AbilityPriority phase)
	{
	}

	public virtual void OnTurnEnd()
	{
		m_stacking.ShiftStackCount();
	}

	public virtual void OnAbilityPhaseEnd(AbilityPriority phase)
	{
	}

	// TODO EFFECT call it somewhere
	public virtual void OnResolvedHpAfterAbilities()
	{
	}

	// TODO EFFECT call it somewhere
	public virtual void OnAbilityAndMovementDone()
	{
	}

	public virtual void OnHit(ActorData target)
	{
	}

	public virtual void OnHit(Vector3 position)
	{
	}

	public virtual void OnUnresolvedDamage(ActorData target, ActorData caster, DamageSource src, int finalDamage, ServerCombatManager.DamageType damageType, ActorHitResults actorHitResults)
	{
	}

	public virtual void OnDealtUnresolvedDamage(ActorData target, ActorData caster, DamageSource src, int finalDamage, ServerCombatManager.DamageType damageType, ActorHitResults actorHitResults)
	{
	}

	public virtual void OnBreakInvisibility()
	{
	}

	public void OnDamaged_Base(ActorData damageTarget, ActorData damageCaster, DamageSource damageSource, int damageAmount, ActorHitResults actorHitResults)
	{
		if (damageCaster == Caster)
		{
			HandleEffectEndTagEvent(EffectEndTag.OnCasterDidDamage);
		}
		if (damageTarget == Caster)
		{
			HandleEffectEndTagEvent(EffectEndTag.OnCasterTookDamage);
		}
		if (damageCaster == Target)
		{
			HandleEffectEndTagEvent(EffectEndTag.OnTargetDidDamage);
		}
		if (damageTarget == Target)
		{
			HandleEffectEndTagEvent(EffectEndTag.OnTargetTookDamage);
		}
		OnDamaged(damageTarget, damageCaster, damageSource, damageAmount, actorHitResults);
	}

	protected virtual void OnDamaged(ActorData damageTarget, ActorData damageCaster, DamageSource damageSource, int damageAmount, ActorHitResults actorHitResults)
	{
	}

	public void OnHealed_Base(ActorData healTarget, ActorData healCaster, DamageSource healSource, int healAmount, DamageSource src, ActorHitResults actorHitResults)
	{
		if (healCaster == Caster)
		{
			HandleEffectEndTagEvent(EffectEndTag.OnCasterDidHealing);
		}
		if (healTarget == Caster)
		{
			HandleEffectEndTagEvent(EffectEndTag.OnCasterTookHealing);
		}
		if (healCaster == Target)
		{
			HandleEffectEndTagEvent(EffectEndTag.OnTargetDidHealing);
		}
		if (healTarget == Target)
		{
			HandleEffectEndTagEvent(EffectEndTag.OnTargetTookHealing);
		}
		OnHealed(healTarget, healCaster, healSource, healAmount, src, actorHitResults);
	}

	protected virtual void OnHealed(ActorData healTarget, ActorData healCaster, DamageSource healSource, int healAmount, DamageSource src, ActorHitResults actorHitResults)
	{
	}

	public virtual void OnGainingEffect(global::Effect effect)
	{
	}

	public virtual void OnLosingEffect(global::Effect effect)
	{
	}

	public virtual void OnActorAdded(ActorData actor)
	{
	}

	public virtual void OnActorRemoved(ActorData actor)
	{
	}

	public virtual void OnExecutedEffectResults(EffectResults effectResults)
	{
	}

	public virtual void OnExecutedActorHitOnTarget(ActorData hitCaster, ActorHitResults results)
	{
	}

	public virtual void OnExecutedActorHitCastByTarget(ActorData hitTarget, ActorHitResults results)
	{
	}

	public virtual bool CanStackWith(global::Effect other)
	{
		bool flag = false;
		if (other != null)
		{
			flag = (other.CanStack() && CanStack());
			flag = (flag && other.Caster == Caster);
			flag = (flag && other.GetType() == base.GetType());
		}
		return flag;
	}

	public virtual void OnStackCountChanged(int previousStackCount, int previousAge)
	{
	}

	public virtual string GetDisplayString()
	{
		string str = Parent.GetName();
		if (!m_stacking.stackCount.IsNullOrEmpty<int>())
		{
			for (int i = 0; i < m_stacking.stackCount.Length; i++)
			{
				str += string.Format(" x{0} for {1} turns ", m_stacking.stackCount[i], i + 1);
			}
		}
		str += "- ";
		return str + m_time.DisplayString();
	}

	public virtual bool ShouldEndEarly()
	{
		return (Absorbtion.m_absorbAmount > 0 && Absorbtion.m_absorbRemaining <= 0) || m_reasonsToEndEarly.Count > 0;
	}

	public virtual bool IsBuff()
	{
		return Target != null && Caster != null && Caster.GetTeam() == Target.GetTeam();
	}

	public virtual bool IsDebuff()
	{
		return Target != null && Caster != null && Caster.GetTeam() != Target.GetTeam();
	}

	public virtual bool HasDispellableMovementDebuff()
	{
		return false;
	}

	public virtual bool CanBeDispelledByStatusImmunity()
	{
		return true;
	}

	public virtual bool WillApplyStatus(StatusType status)
	{
		return false;
	}

	public virtual void OnCalculatedExtraDamageFromEmpoweredGrantedByThisEffect(ActorData empoweredActor, int extraDamage)
	{
		if (Parent.IsAbility())
		{
			Parent.Ability.OnCalculatedExtraDamageFromEmpoweredGrantedByMyEffect(Caster, empoweredActor, extraDamage);
		}
	}

	public virtual void OnCalculatedDamageReducedFromWeakenedGrantedByThisEffect(ActorData weakenedActor, int damageReduced)
	{
		if (Parent.IsAbility())
		{
			Parent.Ability.OnCalculatedDamageReducedFromWeakenedGrantedByMyEffect(Caster, weakenedActor, damageReduced);
		}
	}

	public virtual void OnCalculatedExtraDamageFromVulnerableGrantedByThisEffect(ActorData vulnerableActor, int extraDamage)
	{
		if (Parent.IsAbility())
		{
			Parent.Ability.OnCalculatedExtraDamageFromVulnerableGrantedByMyEffect(Caster, vulnerableActor, extraDamage);
		}
	}

	public virtual void OnCalculatedDamageReducedFromArmoredGrantedByThisEffect(ActorData armoredActor, int damageReduced)
	{
		if (Parent.IsAbility())
		{
			Parent.Ability.OnCalculatedDamageReducedFromArmoredGrantedByMyEffect(Caster, armoredActor, damageReduced);
		}
	}

	public virtual void OnAbsorbedDamage(int damageAbsorbed)
	{
		if (Parent.IsAbility())
		{
			Parent.Ability.OnEffectAbsorbedDamage(Caster, damageAbsorbed);
		}
	}

	public virtual bool CasterMustHaveAccuratePositionOnClients()
	{
		return false;
	}

	public virtual bool TargetMustHaveAccuratePositionOnClients()
	{
		return false;
	}

	public bool CanStack()
	{
		return Stacking.canStack;
	}

	public int GetMaxStackSize()
	{
		return m_stacking.maxStackSize;
	}

	public int GetCurrentStackCount()
	{
		int num = 0;
		for (int i = 0; i < m_stacking.stackCount.Length; i++)
		{
			num += m_stacking.stackCount[i];
		}
		return num;
	}

	public void InitStacking(int maxTurns, int maxStackSize, int maxStackSizePerTurn)
	{
		m_stacking.canStack = (maxStackSize >= 0);
		m_stacking.maxStackSize = maxStackSize;
		m_stacking.maxStackSizePerTurn = maxStackSizePerTurn;
		Array.Resize<int>(ref m_stacking.stackCount, maxTurns);
	}

	public void AddToStack(int stacks = 1)
	{
		int currentStackCount = GetCurrentStackCount();
		int age = m_time.age;
		if (!m_stacking.stackCount.IsNullOrEmpty<int>())
		{
			int i = 0;
			int num = currentStackCount;
			int delta = stacks;
			while (i < m_stacking.stackCount.Length)
			{
				int maxStackSizePerTurn = (m_stacking.maxStackSizePerTurn < 0) ? int.MaxValue : m_stacking.maxStackSizePerTurn;
				int maxStackSize = (m_stacking.maxStackSize == 0) ? int.MaxValue : m_stacking.maxStackSize;
				int maxDelta = maxStackSizePerTurn - m_stacking.stackCount[i];
				if (maxDelta <= 0)
				{
					i++;
				}
				else
				{
					int deltaClamped = Mathf.Min(maxDelta, delta);
					if (deltaClamped + num > maxStackSize)
					{
						deltaClamped = maxStackSize - num;
					}
					if (deltaClamped <= 0)
					{
						break;
					}
					m_stacking.stackCount[i] += deltaClamped;
					num += deltaClamped;
					delta -= deltaClamped;
					i++;
				}
				if (delta <= 0)
				{
					break;
				}
			}
		}
		m_time.age = 0;
		OnStackCountChanged(currentStackCount, age);
	}

	public virtual void Refresh()
	{
		m_stacking.Refresh();
		m_time.age = 0;
	}

	public bool CanAbsorb()
	{
		return Absorbtion.m_absorbAmount > 0 && Absorbtion.m_absorbRemaining > 0;
	}

	public int GetRemainingAbsorb()
	{
		if (Absorbtion.m_absorbAmount > 0)
		{
			return Absorbtion.m_absorbRemaining;
		}
		return 0;
	}

	public int GetMaxAbsorbAmount()
	{
		if (Absorbtion.m_absorbAmount > 0)
		{
			return Absorbtion.m_absorbAmount;
		}
		return 0;
	}

	public void InitAbsorbtion(int baseAbsorbAmount)
	{
		int num;
		if (Caster != null)
		{
			int num2;
			int num3;
			int num4;
			num = Caster.GetComponent<ActorStats>().CalculateOutgoingAbsorb(baseAbsorbAmount, out num2, out num3, out num4);
		}
		else
		{
			num = baseAbsorbAmount;
		}
		if (GameplayMutators.Get() != null)
		{
			num = Mathf.RoundToInt((float)num * GameplayMutators.GetAbsorbMultiplier());
		}
		m_absorbtion.m_absorbAmount = num;
		m_absorbtion.m_absorbRemaining = num;
	}

	public void AbsorbDamage(ref int damage)
	{
		int damageAbsorbed = ReduceAbsorptionRemaining(ref damage); // inlined in rogues
		if (damageAbsorbed > 0)
		{
			if (!IgnoredForStatistics) // custom, unconditional in rogues
			{
				GameplayMetricHelper.CollectAbsorbDealt(Caster, damageAbsorbed, Parent.Ability);
				GameplayMetricHelper.CollectAbsorbReceived(Target, damageAbsorbed);
			}
			OnAbsorbedDamage(damageAbsorbed);
		}
	}

	public int ReduceAbsorptionRemaining(ref int damage)
	{
		int damageAbsorbed;
		if (damage > m_absorbtion.m_absorbRemaining)
		{
			damageAbsorbed = m_absorbtion.m_absorbRemaining;
			damage -= m_absorbtion.m_absorbRemaining;
			m_absorbtion.m_absorbRemaining = 0;
		}
		else
		{
			damageAbsorbed = damage;
			m_absorbtion.m_absorbRemaining -= damage;
			damage = 0;
		}

		return damageAbsorbed;
	}

	public void RefillAbsorb(int restoreAmount)
	{
		if (m_absorbtion.m_absorbAmount > 0 && restoreAmount > 0 && m_absorbtion.m_absorbRemaining < m_absorbtion.m_absorbAmount)
		{
			int num = Mathf.Min(m_absorbtion.m_absorbRemaining + restoreAmount, m_absorbtion.m_absorbAmount);
			int amount = num - m_absorbtion.m_absorbRemaining;
			m_absorbtion.m_absorbRemaining = num;
			ServerEffectManager.Get().UpdateAbsorbPoints(this);

			if (!IgnoredForStatistics) // custom, unconditional in rogues
			{
				GameplayMetricHelper.CollectPotentialAbsorbDealt(Caster, amount, Parent.Ability);
			}
		}
	}

	public void HandleEffectEndTagEvent(EffectEndTag relevantTag)
	{
		if (m_endTags.Contains(relevantTag) && !m_reasonsToEndEarly.Contains(relevantTag))
		{
			m_reasonsToEndEarly.Add(relevantTag);
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Log.Warning(string.Concat(new string[]
				{
					"<color=green>Effect</color>: ",
					GetDebugIdentifier("yellow"),
					" got EndTag[ ",
					relevantTag.ToString(),
					" ]"
				}));
			}
		}
	}

	public virtual List<Vector3> CalcPointsOfInterestForCamera()
	{
		return null;
	}

	public bool CalcBoundsOfInterestForCamera(out Bounds bounds)
	{
		bounds = default(Bounds);
		List<Vector3> list = CalcPointsOfInterestForCamera();
		bool flag = list != null && list.Count > 0;
		if (flag)
		{
			bounds.center = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				bounds.Encapsulate(list[i]);
			}
		}
		return flag;
	}

	public virtual void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
	}

	public virtual bool IsMovementBlockedOnEnterSquare(ActorData mover, BoardSquare movingFrom, BoardSquare movingTo)
	{
		return false;
	}

	public virtual void OnMiscHitEventUpdate(List<MiscHitEventEffectUpdateParams> updateParams)
	{
	}

	public bool IsCharacterSpecificAbility()
	{
		return Parent.IsCharacterSpecificAbility(Caster);
	}

	public virtual int GetExpectedHealOverTimeTotal()
	{
		return 0;
	}

	public virtual int GetExpectedHealOverTimeThisTurn()
	{
		return 0;
	}

	public virtual void DrawGizmos()
	{
	}

	public string GetDebugIdentifier()
	{
		string text = (m_effectName != null && m_effectName.Length > 0) ? m_effectName : "NO_NAME";
		return string.Concat(new object[]
		{
			"[ ",
			text,
			" ] GUID ",
			m_guid
		});
	}

	public string GetDebugIdentifier(string colorStr)
	{
		return string.Concat(new string[]
		{
			"<color=",
			colorStr,
			">",
			GetDebugIdentifier(),
			"</color>"
		});
	}

	public virtual string GetInEditorDescription()
	{
		string str = (m_effectName.Length > 0) ? m_effectName : "No_Name";
		return "-- Effect [ " + str + " ] --\n" + ((m_time.duration > 0) ? ("Duration: " + m_time.duration + " turn(s).") : "WARNING: IS PERMANENT on Target (duration <= 0). Woof Woof Woof Woof");
	}

	public class EffectKnockbackTargets
	{
		public ActorData m_sourceActor;

		public Dictionary<ActorData, Vector2> m_knockbackTargets;

		public EffectKnockbackTargets(ActorData sourceActor, Dictionary<ActorData, Vector2> knockbackTargets)
		{
			m_sourceActor = sourceActor;
			m_knockbackTargets = knockbackTargets;
		}
	}
}
#endif
