// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

// was empty in reactor
public class ServerEffectManager : MonoBehaviour
{
#if SERVER
	public static int s_nextEffectGuid;

	private SharedEffectBarrierManager m_sharedEffectBarrierManager;
	private Dictionary<ActorData, List<Effect>> m_actorEffects = new Dictionary<ActorData, List<Effect>>(8);
	private List<Effect> m_worldEffects = new List<Effect>();
	private List<ActorData> m_actorsAddedThisTurn = new List<ActorData>();
	private List<ActorData> m_actorsPendingDeath = new List<ActorData>();

	// rogues?
	//private Dictionary<ActorData, List<EffectTrigger>> m_actorTriggers = new Dictionary<ActorData, List<EffectTrigger>>();

	private static ServerEffectManager s_instance;

	public List<Effect> WorldEffects => m_worldEffects;

	// rogues?
	//public Dictionary<ActorData, List<EffectTrigger>> Triggers
	//{
	//	get
	//	{
	//		return this.m_actorTriggers;
	//	}
	//}

	public static ServerEffectManager Get()
	{
		return s_instance;
	}

	public static SharedEffectBarrierManager GetSharedEffectBarrierManager()
	{
		return s_instance.m_sharedEffectBarrierManager;
	}

	private void Awake()
	{
		s_instance = this;
		if (NetworkServer.active)
		{
			GameObject sharedEffectBarrierManagerPrefab = NetworkedSharedGameplayPrefabs.GetSharedEffectBarrierManagerPrefab();
			if (sharedEffectBarrierManagerPrefab != null)
			{
				GameObject barrierManagerGameObject = Instantiate(sharedEffectBarrierManagerPrefab, Vector3.zero, Quaternion.identity);
				NetworkServer.Spawn(barrierManagerGameObject);
				DontDestroyOnLoad(barrierManagerGameObject);
				m_sharedEffectBarrierManager = barrierManagerGameObject.GetComponent<SharedEffectBarrierManager>();
			}
		}
	}

	private void OnDestroy()
	{
		if (NetworkServer.active && m_sharedEffectBarrierManager != null)
		{
			NetworkServer.Destroy(m_sharedEffectBarrierManager.gameObject);
		}
		GameFlowData.s_onAddActor -= AddActor;
		GameFlowData.s_onRemoveActor -= RemoveReferencesToActor;
		s_instance = null;
	}

	private void Start()
	{
		if (GameFlowData.Get() != null && GameFlowData.Get().GetActors() != null)
		{
			foreach (ActorData actorToAdd in GameFlowData.Get().GetActors())
			{
				AddActor(actorToAdd);
			}
		}
		GameFlowData.s_onAddActor += AddActor;
		GameFlowData.s_onRemoveActor += RemoveReferencesToActor;
	}

	private void AddActor(ActorData actorToAdd)
	{
		if (m_actorEffects == null)
		{
			Log.Error("Trying to add actor {0} to the ServerEffectManager, but m_actorEffects is null", actorToAdd.DisplayName);
		}
		else if (m_actorEffects.ContainsKey(actorToAdd))
		{
			Log.Warning("Trying to add actor {0} to the ServerEffectManager, but he's already present", actorToAdd.DisplayName);
		}
		else
		{
			m_actorEffects.Add(actorToAdd, new List<Effect>());
			// rogues?
			//foreach (EffectTemplate effectTemplate in actorToAdd.m_onInitEffectTemplates)
			//{
			//	ActorData target = (effectTemplate.scope == EffectTemplate.Scope.Target) ? actorToAdd : null;
			//	EffectSystem.Effect effect = new EffectSystem.Effect(effectTemplate, effectTemplate, new EffectSource(effectTemplate.LocalizedName, null, null), null, target, actorToAdd);
			//	this.ApplyEffect(effect, 1);
			//}
			//this.m_actorTriggers.Add(actorToAdd, new List<EffectTrigger>());
		}

		// rogues
		//foreach (GearStatBlock gearStatBlock in actorToAdd.m_initialGearStatData)
		//{
		//	float rating = gearStatBlock.rating;
		//	GearStatDataTemplate gearStatDataTemplate = gearStatBlock.gearStatDataTemplate;
		//	EquipmentStats equipmentStats = actorToAdd.GetEquipmentStats();
		//	for (int i = 0; i < gearStatDataTemplate.infos.Length; i++)
		//	{
		//		GearStatTypeInfo gearStatTypeInfo = gearStatDataTemplate.infos[i];
		//		float value = gearStatTypeInfo.RatingToPercentageCurve.Evaluate(rating);
		//		if (gearStatDataTemplate.scope == GearStatScope.Actor)
		//		{
		//			equipmentStats.AddActorStat(gearStatTypeInfo.statType, value, gearStatTypeInfo.statOp, 0, null);
		//		}
		//	}
		//}
		//List<EffectTemplate> activeEffectTemplates = EscalationProducer.Get().GetActiveEffectTemplates();
		//if (activeEffectTemplates.Count > 0)
		//{
		//	foreach (EffectTemplate effectTemplate2 in activeEffectTemplates)
		//	{
		//		ActorData target2 = (effectTemplate2.scope == EffectTemplate.Scope.Target) ? actorToAdd : null;
		//		EffectSystem.Effect effect2 = new EffectSystem.Effect(effectTemplate2, effectTemplate2, new EffectSource(effectTemplate2.LocalizedName, null, null), null, target2, actorToAdd);
		//		this.ApplyEffect(effect2, 1);
		//	}
		//}
		//IEnumerable<Talent> talents = TalentManager.Get().GetTalents(actorToAdd.m_characterType);
		//if (talents != null && actorToAdd.GetTeam() == Team.TeamA)
		//{
		//	foreach (Talent talent in talents)
		//	{
		//		foreach (EffectTemplate effectTemplate3 in talent.m_effectTemplates)
		//		{
		//			ActorData target3 = (effectTemplate3.scope == EffectTemplate.Scope.Target) ? actorToAdd : null;
		//			EffectSystem.Effect effect3 = new EffectSystem.Effect(effectTemplate3, effectTemplate3, new EffectSource(effectTemplate3.LocalizedName, null, null), null, target3, actorToAdd);
		//			this.ApplyEffect(effect3, 1);
		//		}
		//	}
		//}

		m_actorsAddedThisTurn.Add(actorToAdd);
		foreach (Effect effect4 in m_actorEffects.SelectMany(kvp => kvp.Value).ToList())
		{
			effect4.OnActorAdded(actorToAdd);
		}
	}

	private void RemoveReferencesToActor(ActorData actor)
	{
		// rogues?
		//foreach (EffectTrigger effectTrigger in this.m_actorTriggers.SelectMany((KeyValuePair<ActorData, List<EffectTrigger>> kvp) => kvp.Value).ToList<EffectTrigger>())
		//{
		//	effectTrigger.EvaluateTick(TriggerEvent.ActorRemoved, new object[]
		//	{
		//		actor
		//	});
		//}

		RemoveEffectsOnActor(actor);
		RemoveEffectsCastedByActor(actor);
		if (m_actorEffects.ContainsKey(actor))
		{
			m_actorEffects.Remove(actor);
		}

		// rogues?
		//if (this.m_actorTriggers.ContainsKey(actor))
		//{
		//	this.m_actorTriggers.Remove(actor);
		//}
	}

	private void Update()
	{
		foreach (KeyValuePair<ActorData, List<Effect>> keyValuePair in m_actorEffects)
		{
			foreach (Effect effect in keyValuePair.Value)
			{
				effect.Update();
			}
		}
		foreach (Effect effect2 in m_worldEffects)
		{
			effect2.Update();
		}

		// rogues?
		//foreach (KeyValuePair<ActorData, List<EffectTrigger>> keyValuePair2 in this.m_actorTriggers)
		//{
		//	foreach (EffectTrigger effectTrigger in keyValuePair2.Value)
		//	{
		//		effectTrigger.Update();
		//	}
		//}
	}

	// added in rogues
	public bool HasEffect(ActorData target, Type effectType)
	{
		return GetEffect(target, effectType) != null;
	}

	// added in rogues
	public bool HasEffectByCaster(ActorData target, ActorData caster, Type effectType)
	{
		return GetEffectsOnTargetByCaster(target, caster, effectType).Count > 0;
	}

	public bool HasEffectRequiringAccuratePositionOnClients(ActorData actor)
	{
		if (actor == null)
		{
			return false;
		}
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			foreach (Effect effect in list)
			{
				if (effect.Caster == actor && effect.CasterMustHaveAccuratePositionOnClients())
				{
					return true;
				}
				if (effect.Target == actor && effect.TargetMustHaveAccuratePositionOnClients())
				{
					return true;
				}
			}
		}
		return false;
	}

	// rogues?
	//public void ApplyEffectTrigger(EffectTrigger effectTrigger)
	//{
	//	List<EffectTrigger> list;
	//	if (!this.m_actorTriggers.TryGetValue(effectTrigger.Caster, out list))
	//	{
	//		list = new List<EffectTrigger>();
	//		this.m_actorTriggers[effectTrigger.Caster] = list;
	//	}
	//	EffectTrigger effectTrigger2 = list.FirstOrDefault((EffectTrigger trigger) => trigger.EffectTriggerTemplate == effectTrigger.EffectTriggerTemplate && trigger.EffectTriggerTemplate.consolidateEffects);
	//	if (effectTrigger2 != null)
	//	{
	//		using (List<ActorData>.Enumerator enumerator = effectTrigger.Targets.GetEnumerator())
	//		{
	//			while (enumerator.MoveNext())
	//			{
	//				ActorData item = enumerator.Current;
	//				if (!effectTrigger2.Targets.Contains(item))
	//				{
	//					effectTrigger2.Targets.Add(item);
	//				}
	//			}
	//			return;
	//		}
	//	}
	//	list.Add(effectTrigger);
	//}


	// rogues?
	//public void RemoveEffectTrigger(EffectTrigger effectTrigger)
	//{
	//	foreach (KeyValuePair<ActorData, List<EffectTrigger>> keyValuePair in this.m_actorTriggers)
	//	{
	//		keyValuePair.Value.Remove(effectTrigger);
	//	}
	//}

	// added in rogues
	public Effect GetEffect(ActorData target, Type effectType)
	{
		List<Effect> effects = null;
		if (target != null && m_actorEffects.ContainsKey(target))
		{
			effects = m_actorEffects[target];
		}
		else if (target == null)
		{
			effects = m_worldEffects;
		}
		if (effects == null)
		{
			return null;
		}
		foreach (Effect effect in effects)
		{
			if (effect != null && effect.GetType() == effectType)
			{
				return effect;
			}
		}
		return null;
	}

	public Dictionary<ActorData, List<Effect>> GetAllActorEffects()
	{
		return m_actorEffects;
	}

	public List<Effect> GetActorEffects(ActorData actorData)
	{
		List<Effect> result = null;
		if (m_actorEffects.ContainsKey(actorData))
		{
			result = m_actorEffects[actorData];
		}
		return result;
	}

	// added in rogues
	public List<Effect> GetEffectsOnTargetByCaster(ActorData target, ActorData caster, Type effectType)
	{
		List<Effect> result = new List<Effect>();
		if (target != null && m_actorEffects.ContainsKey(target))
		{
			List<Effect> effectsOnTarget = m_actorEffects[target];
			if (effectsOnTarget != null)
			{
				foreach (Effect effect in effectsOnTarget)
				{
					if (effect != null && effect.GetType() == effectType && effect.Caster == caster)
					{
						result.Add(effect);
					}
				}
			}
		}
		return result;
	}

	public List<Effect> GetEffectsOnTargetGrantingStatus(ActorData target, StatusType status)
	{
		List<Effect> result = new List<Effect>();
		if (target != null && m_actorEffects.ContainsKey(target))
		{
			List<Effect> effectsOnTarget = m_actorEffects[target];
			if (effectsOnTarget != null)
			{
				foreach (Effect effect in effectsOnTarget)
				{
					if (effect != null)
					{
						List<StatusType> statuses = effect.GetStatuses();
						List<StatusType> statusesOnTurnStart = effect.GetStatusesOnTurnStart();
						if (statuses != null && statuses.Contains(status))
						{
							result.Add(effect);
						}
						else if (effect.m_time.age > 0 && statusesOnTurnStart != null && statusesOnTurnStart.Contains(status))
						{
							result.Add(effect);
						}
					}
				}
			}
		}
		return result;
	}

	public List<ActorData> GetCastersOfEffectsOnTargetGrantingStatus(ActorData target, StatusType status)
	{
		List<ActorData> result = new List<ActorData>();
		if (target != null && m_actorEffects.ContainsKey(target))
		{
			List<Effect> effectsOnTarget = m_actorEffects[target];
			if (effectsOnTarget != null)
			{
				foreach (Effect effect in effectsOnTarget)
				{
					if (effect != null && effect.Caster != null && !result.Contains(effect.Caster))
					{
						List<StatusType> statuses = effect.GetStatuses();
						List<StatusType> statusesOnTurnStart = effect.GetStatusesOnTurnStart();
						if (statuses != null && statuses.Contains(status))
						{
							result.Add(effect.Caster);
						}
						else if (effect.m_time.age > 0 && statusesOnTurnStart != null && statusesOnTurnStart.Contains(status))
						{
							result.Add(effect.Caster);
						}
					}
				}
			}
		}
		return result;
	}

	// added in rogues
	public List<Effect> GetAllActorEffectsByCaster(ActorData caster, Type effectType)
	{
		List<Effect> result = new List<Effect>();
		foreach (ActorData target in m_actorEffects.Keys)
		{
			result.AddRange(GetEffectsOnTargetByCaster(target, caster, effectType));
		}
		return result;
	}

	public List<Effect> GetWorldEffects()
	{
		return m_worldEffects;
	}

	public List<Effect> GetWorldEffectsByCaster(ActorData caster)
	{
		List<Effect> result = new List<Effect>();
		foreach (Effect effect in m_worldEffects)
		{
			if (effect.Caster == caster)
			{
				result.Add(effect);
			}
		}
		return result;
	}

	public List<Effect> GetWorldEffectsByCaster(ActorData caster, Type type)
	{
		List<Effect> result = new List<Effect>();
		foreach (Effect effect in m_worldEffects)
		{
			if (effect.Caster == caster && effect.GetType() == type)
			{
				result.Add(effect);
			}
		}
		return result;
	}

	private Effect GetEffectToStackWith(Effect newEffect, List<Effect> targetEffects)
	{
		if (!newEffect.CanStack())
		{
			return null;
		}
		foreach (Effect effect in targetEffects)
		{
			if (effect.CanStackWith(newEffect))
			{
				return effect;
			}
		}
		return null;
	}

	public void ApplyEffect(Effect effect, int stacks = 1)
	{
		bool canApply = true;
		if (effect.Target != null && effect.Caster != null)
		{
			ActorStatus component = effect.Target.GetComponent<ActorStatus>();
			if (component.HasStatus(StatusType.EffectImmune))
			{
				canApply = false;
			}
			else if (effect.Target != effect.Caster
				&& effect.Target.GetTeam() == effect.Caster.GetTeam()
				&& component.HasStatus(StatusType.CantBeHelpedByTeam))
			{
				canApply = false;
			}
			else if (component.HasStatus(StatusType.BuffImmune) && effect.IsBuff())
			{
				canApply = false;
			}
			else if (component.HasStatus(StatusType.DebuffImmune) && effect.IsDebuff())
			{
				canApply = false;
			}
		}
		// rogues?
		//EffectSystem.Effect effect2;
		//if ((effect2 = (effect as EffectSystem.Effect)) != null && !effect2.Evaluate<bool>(effect2.EffectTemplate.ShouldEvaluate, Array.Empty<object>()))
		//{
		//	canApply = false;
		//}
		if (!canApply)
		{
			m_sharedEffectBarrierManager.NotifyEffectEnded(effect.m_guid);
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Debug.LogWarning("<color=green>Effect</color>: " + effect.GetDebugIdentifier("yellow") + " Skipping SERVER effect application");
			}
			return;
		}
		bool isEffectStart = true;
		if (effect.Target == null)
		{
			effect.AddToStack(stacks);
			m_worldEffects.Add(effect);
		}
		else
		{
			if (!m_actorEffects.TryGetValue(effect.Target, out List<Effect> list))
			{
				list = new List<Effect>();
				m_actorEffects.Add(effect.Target, list);
			}
			Effect effectToStackWith = GetEffectToStackWith(effect, list);
			if (effectToStackWith == null)
			{
				list.Add(effect);
				effect.AddToStack(stacks);

				// rogues?
				//EffectSystem.Effect effect3;
				//if ((effect3 = (effect as EffectSystem.Effect)) != null)
				//{
				//	flag3 = (effect3.EffectLifecycle == EffectLifecycle.Invalid);
				//}
			}
			else
			{
				isEffectStart = false;
				effectToStackWith.AddToStack(stacks);

				// rogues?
				//EffectSystem.Effect effectSystemStackEffect;
				//if ((effectSystemStackEffect = (effectToStackWith as EffectSystem.Effect)) != null && effectSystemStackEffect.EffectTemplate.ConsolidateEffects)
				//{
				//	effectSystemStackEffect.targets.AddRange(from actorData in ((EffectSystem.Effect)effect).targets
				//	where !effectSystemStackEffect.targets.Contains(actorData)
				//	select actorData);
				//}

				m_sharedEffectBarrierManager.NotifyEffectEnded(effect.m_guid);
			}
		}
		if (isEffectStart)
		{
			effect.OnStart();
		}
		if (effect.Target != null)
		{
			if (effect.CanAbsorb())
			{
				UpdateAbsorbPoints(effect);
			}
			PassiveData component2 = effect.Target.GetComponent<PassiveData>();
			if (component2 != null)
			{
				component2.OnGainingEffect(effect);
			}
			ActorBehavior actorBehavior = effect.Target.GetActorBehavior();
			if (actorBehavior != null && actorBehavior.CurrentTurn != null)
			{
				actorBehavior.CurrentTurn.RecordEffectFromActor(effect.Caster);
			}
		}
		if (effect.Caster != null)
		{
			if (effect.Caster.GetComponent<PassiveData>())
			{
				effect.Caster.GetComponent<PassiveData>().OnAppliedEffectOnOther(effect);
			}
			if (effect.Parent.IsAbility() && BrushCoordinator.Get() != null)
			{
				BrushCoordinator.Get().OnEffect_HandleConcealment(effect);
			}
		}

		// rogues
		//effect.m_time.activeTeamAtApplication = GameFlowData.Get().ActingTeam;
		//if (effect.m_time.activeTeamAtApplication == Team.Invalid)
		//{
		//effect.m_time.activeTeamAtApplication = effect.Caster.GetTeam();
		//}

		OnGainingEffect(effect);
		foreach (ActorData actor in m_actorsAddedThisTurn)
		{
			effect.OnActorAdded(actor);
		}
		if (EffectDebugConfig.TracingAddAndRemove())
		{
			string text = (effect.Caster != null) ? effect.Caster.DebugNameString("yellow") : "[None]";
			string text2 = (effect.Target != null) ? effect.Target.DebugNameString("yellow") : "[None]";
			Log.Warning(string.Concat("<color=green>Effect</color>: ", effect.GetDebugIdentifier("yellow"), " APPLIED\nCaster: ", text, ", Target: ", text2));
		}
	}

	public void RemoveEffect(Effect effectToRemove, List<Effect> effectListToRemoveFrom)
	{
		effectToRemove.End();
		effectListToRemoveFrom.Remove(effectToRemove);
		NotifyLosingEffect(effectToRemove);
		if (EffectDebugConfig.TracingAddAndRemove())
		{
			string casterStr = (effectToRemove.Caster != null) ? effectToRemove.Caster.DebugNameString("yellow") : "[None]";
			string targetStr = (effectToRemove.Target != null) ? effectToRemove.Target.DebugNameString("yellow") : "[None]";
			Log.Warning(string.Concat("<color=green>Effect</color>: ", effectToRemove.GetDebugIdentifier("yellow"), " REMOVED\nCaster: ", casterStr, ", Target: ", targetStr));
		}
	}

	public void RemoveEffects(List<Effect> effectsToRemove, List<Effect> effectListToRemoveFrom)
	{
		foreach (Effect effect in effectsToRemove)
		{
			if (effectListToRemoveFrom.Remove(effect))
			{
				effect.End();
				NotifyLosingEffect(effect);
			}
		}
	}

	public void RemoveEffectsOnActor(ActorData actor)
	{
		if (actor != null && m_actorEffects.ContainsKey(actor))
		{
			List<Effect> list = new List<Effect>();
			List<Effect> list2 = m_actorEffects[actor];
			if (list2 != null)
			{
				foreach (Effect item in list2)
				{
					list.Add(item);
				}
			}
			foreach (Effect effectToRemove in list)
			{
				RemoveEffect(effectToRemove, list2);
			}
		}

		// rogues?
		//foreach (global::Effect effect in from e in this.m_actorEffects.SelectMany((KeyValuePair<ActorData, List<global::Effect>> kvp) => kvp.Value)
		//where e is EffectSystem.Effect
		//select e)
		//{
		//	((EffectSystem.Effect)effect).EffectGroups.Remove(actor);
		//}
	}

	public void RemoveEffectsCastedByActor(ActorData actor)
	{
		if (actor != null)
		{
			foreach (KeyValuePair<ActorData, List<Effect>> keyValuePair in m_actorEffects)
			{
				List<Effect> list = new List<Effect>();
				foreach (Effect effect in keyValuePair.Value)
				{
					if (effect.Caster == actor)
					{
						list.Add(effect);
					}
				}
				foreach (Effect effectToRemove in list)
				{
					RemoveEffect(effectToRemove, keyValuePair.Value);
				}
			}
			List<Effect> list2 = new List<Effect>();
			foreach (Effect effect2 in m_worldEffects)
			{
				if (effect2.Caster == actor)
				{
					list2.Add(effect2);
				}
			}
			foreach (Effect effectToRemove2 in list2)
			{
				RemoveEffect(effectToRemove2, m_worldEffects);
			}
		}
	}

	public void NotifyLosingEffect(Effect effectRemoved)
	{
		m_sharedEffectBarrierManager.NotifyEffectEnded(effectRemoved.m_guid);
		if (effectRemoved.Target != null)
		{
			PassiveData component = effectRemoved.Target.GetComponent<PassiveData>();
			if (component)
			{
				component.OnLosingEffect(effectRemoved);
			}
		}
		OnLosingEffect(effectRemoved);
	}

	public void OnTurnTick()
	{
		// rogues?
		//foreach (EffectSystem.Effect effect2 in (from effect in this.m_actorEffects.SelectMany((KeyValuePair<ActorData, List<global::Effect>> kvp) => kvp.Value)
		//where effect is EffectSystem.Effect
		//select (EffectSystem.Effect)effect).ToList<EffectSystem.Effect>())
		//{
		//	effect2.EvaluateTick(TriggerEvent.TurnTick, new object[]
		//	{
		//		GameFlowData.Get().CurrentTurn
		//	});
		//}
	}

	public void OnTurnStart()
	{
		ClearAllEffectResults();
		foreach (List<Effect> effectList in m_actorEffects.Values)
		{
			OnTurnStart(effectList);
		}
		OnTurnStart(m_worldEffects);
	}

	private void OnTurnStart(List<Effect> effectList)
	{
		List<Effect> effectsToRemove = new List<Effect>();
		foreach (Effect effect in effectList.ToList())
		{
			bool flag = ValidateEffectLifetime(effect);

			// rogues?
			//EffectSystem.Effect effect2 = effect as EffectSystem.Effect;
			//if (effect2 != null)
			//{
			//	flag |= (effect2.EffectLifecycle == EffectLifecycle.OnEnd && !effect2.pendingEffectResults.Any<EffectResults>());
			//}

			if (effect.ShouldEndEarly() || flag)
			{
				effectsToRemove.Add(effect);
			}
			else
			{
				effect.OnTurnStart();

				// rogues?
				//if (effect2 != null)
				//{
				//	flag = (effect2.EffectLifecycle == EffectLifecycle.OnEnd && !effect2.pendingEffectResults.Any<EffectResults>());
				//}

				if (effect.ShouldEndEarly())
				{
					effectsToRemove.Add(effect);
				}
			}
		}
		foreach (Effect effectToRemove in effectsToRemove)
		{
			RemoveEffect(effectToRemove, effectList);
		}
	}

	public void OnTurnEnd()  // Team nextTeam in rogues
	{
		foreach (List<Effect> effectList in m_actorEffects.Values)
		{
			OnTurnEnd(effectList); // , nextTeam in rogues
		}
		OnTurnEnd(m_worldEffects); // , nextTeam in rogues
	}

	// custom
	private bool ValidateEffectLifetime(Effect effect)
	{
		return effect.m_time.ReadyToEnd();
	}
	// rogues
	//private bool ValidateEffectLifetime(global::Effect effect, bool isEnd)
	//{
	//	Team actingTeam = GameFlowData.Get().ActingTeam;
	//	EffectDuration time = effect.m_time;
	//	EffectDuration.Termination termination = time.termination;
	//	bool flag2;
	//	if ((termination != EffectDuration.Termination.CasterTeamEnd || effect.Caster.GetTeam() != actingTeam || !isEnd) && (termination != EffectDuration.Termination.CasterTeamStart || effect.Caster.GetTeam() != actingTeam || isEnd))
	//	{
	//		bool flag;
	//		if (termination == EffectDuration.Termination.TargetTeamEnd)
	//		{
	//			ActorData target = effect.Target;
	//			flag = (target != null && target.GetTeam() == actingTeam);
	//		}
	//		else
	//		{
	//			flag = false;
	//		}
	//		if (!flag || !isEnd)
	//		{
	//			if (termination == EffectDuration.Termination.TargetTeamStart)
	//			{
	//				ActorData target2 = effect.Target;
	//				if (target2 != null && target2.GetTeam() == actingTeam && !isEnd)
	//				{
	//					goto IL_BB;
	//				}
	//			}
	//			if (termination != EffectDuration.Termination.ActiveTeamAtApplicationEnd || effect.m_time.activeTeamAtApplication != actingTeam || !isEnd)
	//			{
	//				flag2 = (termination == EffectDuration.Termination.ActiveTeamAtApplicationStart && effect.m_time.activeTeamAtApplication == actingTeam && !isEnd);
	//				goto IL_BC;
	//			}
	//		}
	//	}
	//	IL_BB:
	//	flag2 = true;
	//	IL_BC:
	//	if (flag2)
	//	{
	//		time.age++;
	//	}
	//	return time.ReadyToEnd();
	//}

	private void OnTurnEnd(List<Effect> effectList)  // , Team nextTeam in rogues
	{
		m_actorsAddedThisTurn.Clear();
		List<Effect> list = new List<Effect>();
		foreach (Effect effect in effectList.ToList())
		{
			// rogues?
			//EffectSystem.Effect effect2 = effect as EffectSystem.Effect;
			//if (effect2 != null)
			//{
			//	flag = (effect2.EffectLifecycle == EffectLifecycle.OnEnd && !effect2.pendingEffectResults.Any<EffectResults>());
			//}
			
			effect.m_time.age++; // custom
			// RageBeastPlasmaEffect expects age to be 1 by the fist OnTurnEnd call

			if (effect.ShouldEndEarly())
			{
				list.Add(effect);
			}
			else
			{
				effect.OnTurnEnd();

				// rogues?
				//if (effect2 != null)
				//{
				//	flag |= (effect2.EffectLifecycle == EffectLifecycle.OnEnd && !effect2.pendingEffectResults.Any<EffectResults>());
				//}

				if (effect.ShouldEndEarly())
				{
					list.Add(effect);
				}
			}
		}
		foreach (Effect effectToRemove in list)
		{
			RemoveEffect(effectToRemove, effectList);
		}
	}

	// custom
	public void OnAbilityPhaseStart(AbilityPriority phase)
	{
		foreach (List<Effect> effectList in m_actorEffects.Values)
		{
			OnAbilityPhaseStart(effectList, phase);
		}
		OnAbilityPhaseStart(m_worldEffects, phase);
	}

	// custom
	private void OnAbilityPhaseStart(List<Effect> effectList, AbilityPriority phase)
	{
		// NOTE: effects can execute results in OnAbilityPhaseStart (e.g. MantaDirtyFightingEffect)
		// and add new effect thus modifying the collection
		foreach (Effect effect in new List<Effect>(effectList))
		{
			effect.OnAbilityPhaseStart(phase);
		}
	}
	
	// custom
	public void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		foreach (List<Effect> effectList in m_actorEffects.Values)
		{
			OnAbilityPhaseEnd(effectList, phase);
		}
		OnAbilityPhaseEnd(m_worldEffects, phase);
	}

	// custom
	private void OnAbilityPhaseEnd(List<Effect> effectList, AbilityPriority phase)
	{
		List<Effect> effectsToRemove = new List<Effect>();
		foreach (Effect effect in new List<Effect>(effectList))
		{
			effect.OnAbilityPhaseEnd(phase);

			if (effect.ShouldEndEarly())
			{
				effectsToRemove.Add(effect);
			}
		}
		foreach (Effect effectToRemove in effectsToRemove)
		{
			RemoveEffect(effectToRemove, effectList);
		}
	}

	public int AdjustDamage(ActorData target, int damage)
	{
		int result = damage;
		List<Effect> absorbingEffects = new List<Effect>();
		if (m_actorEffects.TryGetValue(target, out List<Effect> actorEffects) && actorEffects != null)
		{
			foreach (Effect effect in actorEffects)
			{
				if (effect.CanAbsorb())
				{
					absorbingEffects.Add(effect);
				}
			}
		}
		absorbingEffects.Sort(delegate (Effect x, Effect y)
		{
			if (x == y)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			int xTimeRemaining = x.m_time.duration - x.m_time.age;
			if (x.m_time.duration <= 0)
			{
				xTimeRemaining = 10000;
			}
			int yTimeRemaining = y.m_time.duration - y.m_time.age;
			if (y.m_time.duration <= 0)
			{
				yTimeRemaining = 10000;
			}

			int timeRemainingCompare = xTimeRemaining.CompareTo(yTimeRemaining);
			
			// custom
			if (timeRemainingCompare == 0)
			{
				if (!x.IgnoredForStatistics && y.IgnoredForStatistics)
				{
					return -1;
				}
				if (x.IgnoredForStatistics && !y.IgnoredForStatistics)
				{
					return 1;
				}
			}
			// end custom
			
			return timeRemainingCompare;
		});
		foreach (Effect absorbingEffect in absorbingEffects)
		{
			absorbingEffect.AbsorbDamage(ref result);
		}
		return result;
	}

	public void UpdateAbsorbPoints(Effect effect)
	{
		int absorbAmount = effect.Absorbtion.m_absorbAmount;
		effect.Target.SetAbsorbPoints(CountAbsorbPoints(effect.Target));
		GameEventManager.ActorHitHealthChangeArgs args = new GameEventManager.ActorHitHealthChangeArgs(GameEventManager.ActorHitHealthChangeArgs.ChangeType.Absorb, absorbAmount, effect.Target, effect.Caster, effect.IsCharacterSpecificAbility());
		GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorDamaged_Server, args);
	}

	public int CountAbsorbPoints(ActorData actor)
	{
		int num = 0;
		if (m_actorEffects.ContainsKey(actor))
		{
			List<Effect> list = m_actorEffects[actor];
			if (list != null)
			{
				foreach (Effect effect in list)
				{
					if (effect.CanAbsorb())
					{
						num += effect.Absorbtion.m_absorbRemaining;
					}
				}
			}
		}
		return num;
	}

	public List<EffectResults> FindEffectsWithCasterAnimations(AbilityPriority phaseIndex)
	{
		return (from e in m_actorEffects.SelectMany(kvp => kvp.Value)
				where e.GetCasterAnimationIndex(phaseIndex) > 0
				select e).Concat(from e in m_worldEffects
								 where e.GetCasterAnimationIndex(phaseIndex) > 0
								 select e).SelectMany(delegate (Effect e)
								 {
									 // rogues?
									 //EffectSystem.Effect effect;
									 //if ((effect = (e as EffectSystem.Effect)) != null)
									 //{
									 //    return effect.pendingEffectResults;
									 //}

									 return new List<EffectResults>
				{
					e.GetResultsForPhase(phaseIndex, true)
				};
								 }).ToList();
	}

	public List<EffectResults> FindAnimlessEffectsWithHitsForPhase(AbilityPriority phaseIndex)
	{
		return (from e in m_actorEffects.SelectMany(kvp => kvp.Value)
				where ShouldAddActorAnimEntryAsAnimlessEffect(e, phaseIndex)
				select e).Concat(from e in m_worldEffects
								 where ShouldAddActorAnimEntryAsAnimlessEffect(e, phaseIndex)
								 select e).SelectMany(delegate (Effect e)
								 {
									 // rogues?
									 //EffectSystem.Effect effect;
									 //if ((effect = (e as EffectSystem.Effect)) != null)
									 //{
									 //    return effect.pendingEffectResults;
									 //}

									 return new List<EffectResults>
				{
					e.GetResultsForPhase(phaseIndex, true)
				};
								 }).ToList();
	}

	private bool ShouldAddActorAnimEntryAsAnimlessEffect(Effect effect, AbilityPriority phaseIndex)
	{
		EffectResults effectResults = (effect != null) ? effect.GetResultsForPhase(phaseIndex, true) : null;
		return effectResults != null && effect.GetCasterAnimationIndex(phaseIndex) <= 0 && effect.AddActorAnimEntryIfHasHits(phaseIndex) && (effectResults.m_actorToHitResults.Count > 0 || effectResults.m_positionToHitResults.Count > 0);
	}

	public void CollectGatheredOutgoingHitsSummary(AbilityPriority phase, ActorData caster, ServerActionBuffer.GatheredOutgoingHitsSummary summary)
	{
		if (summary == null)
		{
			return;
		}
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			foreach (Effect effect in list)
			{
				CollectEffectOutgoingHitSummary(effect, phase, caster, summary);
			}
		}
	}

	private void CollectEffectOutgoingHitSummary(Effect effect, AbilityPriority phase, ActorData caster, ServerActionBuffer.GatheredOutgoingHitsSummary summary)
	{
		if (effect != null && effect.Caster == caster && effect.HitPhase == phase && effect.GetResultsForPhase(phase, true).GatheredResults)
		{
			Dictionary<ActorData, int> damageResults = effect.GetResultsForPhase(phase, true).DamageResults;
			ActorData[] hitActors = effect.GetResultsForPhase(phase, true).HitActorsArray();
			summary.UpdateValuesForResult(caster, hitActors, damageResults);
		}
	}

	public void CountDamageAndHealFromGatheredResults(AbilityPriority phase, ActorData target, ref int damage, ref int healing)
	{
		for (int i = 0; i < m_worldEffects.Count; i++)
		{
			Effect effect = m_worldEffects[i];
			CountEffectDamageAndHealing(effect, phase, target, ref damage, ref healing);
		}
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				Effect effect2 = list[j];
				CountEffectDamageAndHealing(effect2, phase, target, ref damage, ref healing);
			}
		}
	}

	private void CountEffectDamageAndHealing(Effect effect, AbilityPriority phase, ActorData target, ref int damage, ref int healing)
	{
		if (effect != null && effect.HitPhase == phase && effect.GetResultsForPhase(phase, true).GatheredResults)
		{
			ServerGameplayUtils.CountDamageAndHeal(effect.GetResultsForPhase(phase, true).DamageResults, target, ref damage, ref healing);
		}
	}

	public void IntegrateHpDeltasForEffects(AbilityPriority phase, ref Dictionary<ActorData, int> actorToDeltaHP, bool ignoreEffectsWithTheatricsEntry)
	{
		for (int i = 0; i < m_worldEffects.Count; i++)
		{
			Effect effect = m_worldEffects[i];
			IntegrateHpDeltasForEffect(effect, phase, ref actorToDeltaHP, ignoreEffectsWithTheatricsEntry);
		}
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				Effect effect2 = list[j];
				IntegrateHpDeltasForEffect(effect2, phase, ref actorToDeltaHP, ignoreEffectsWithTheatricsEntry);
			}
		}
	}

	private void IntegrateHpDeltasForEffect(Effect effect, AbilityPriority phase, ref Dictionary<ActorData, int> actorToDeltaHP, bool ignoreEffectsWithTheatricsEntry)
	{
		if (effect != null && effect.HitPhase == phase && (!ignoreEffectsWithTheatricsEntry || (effect.GetCasterAnimationIndex(phase) == 0 && !effect.AddActorAnimEntryIfHasHits(phase))) && effect.GetResultsForPhase(phase, true).GatheredResults)
		{
			ServerGameplayUtils.IntegrateHpDeltas(effect.GetResultsForPhase(phase, true).DamageResults, ref actorToDeltaHP);
		}
	}

	public void IntegrateMovementDamageResults_Evasion(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		for (int i = 0; i < m_worldEffects.Count; i++)
		{
			m_worldEffects[i].IntegrateDamageResultsForEvasion(ref actorToDeltaHP);
		}
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				list[j].IntegrateDamageResultsForEvasion(ref actorToDeltaHP);
			}
		}
	}

	public void IntegrateMovementDamageResults_Knockback(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		for (int i = 0; i < m_worldEffects.Count; i++)
		{
			m_worldEffects[i].IntegrateDamageResultsForKnockback(ref actorToDeltaHP);
		}
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				list[j].IntegrateDamageResultsForKnockback(ref actorToDeltaHP);
			}
		}
	}

	public void GatherGrossDamageResults_Effects(AbilityPriority phase, ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, int> actorToGrossDamage_fake, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
	{
		for (int i = 0; i < m_worldEffects.Count; i++)
		{
			Effect effect = m_worldEffects[i];
			GatherGrossDamageResults_Effect(effect, phase, ref actorToGrossDamage_real, ref actorToGrossDamage_fake, ref stats);
		}
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				Effect effect2 = list[j];
				GatherGrossDamageResults_Effect(effect2, phase, ref actorToGrossDamage_real, ref actorToGrossDamage_fake, ref stats);
			}
		}
	}

	public void GatherGrossDamageResults_Effect(Effect effect, AbilityPriority phase, ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, int> actorToGrossDamage_fake, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
	{
		if (effect.HitPhase == phase && effect.GetResultsForPhase(phase, true).GatheredResults && effect.GetResultsForPhase(phase, false).GatheredResults)
		{
			Dictionary<ActorData, int> damageResults_Gross = effect.GetResultsForPhase(phase, true).DamageResults_Gross;
			Dictionary<ActorData, int> damageResults_Gross2 = effect.GetResultsForPhase(phase, false).DamageResults_Gross;
			ServerGameplayUtils.CalcDamageDodgedAndIntercepted(damageResults_Gross, damageResults_Gross2, ref stats);
			ServerGameplayUtils.IntegrateHpDeltas(damageResults_Gross, ref actorToGrossDamage_real);
			ServerGameplayUtils.IntegrateHpDeltas(damageResults_Gross2, ref actorToGrossDamage_fake);
		}
	}

	public void GatherGrossDamageResults_Effects_Evasion(ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
	{
		for (int i = 0; i < m_worldEffects.Count; i++)
		{
			Effect effect = m_worldEffects[i];
			GatherGrossDamageResults_Effect_Evasion(effect, ref actorToGrossDamage_real, ref stats);
		}
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				Effect effect2 = list[j];
				GatherGrossDamageResults_Effect_Evasion(effect2, ref actorToGrossDamage_real, ref stats);
			}
		}
	}

	public void GatherGrossDamageResults_Effect_Evasion(Effect effect, ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
	{
		Dictionary<ActorData, int> fakeDamageTaken = new Dictionary<ActorData, int>();
		foreach (MovementResults movementResults in effect.GetMovementResultsForMovementStage(MovementStage.Evasion))
		{
			Dictionary<ActorData, int> movementDamageResults_Gross = movementResults.GetMovementDamageResults_Gross();
			ServerGameplayUtils.CalcDamageDodgedAndIntercepted(movementDamageResults_Gross, fakeDamageTaken, ref stats);
			ServerGameplayUtils.IntegrateHpDeltas(movementDamageResults_Gross, ref actorToGrossDamage_real);
		}
	}

	public void NotifyBeforeGatherAllEffectResults(AbilityPriority phase)
	{
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			foreach (Effect effect in list)
			{
				effect.OnBeforeGatherEffectResults(phase);
			}
		}
		foreach (Effect effect2 in m_worldEffects)
		{
			effect2.OnBeforeGatherEffectResults(phase);
		}
	}

	public void ClearAllEffectResults()
	{
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			foreach (Effect effect in list)
			{
				effect.ClearEffectResults();
			}
		}
		foreach (Effect effect2 in m_worldEffects)
		{
			effect2.ClearEffectResults();
		}
	}

	public void ClearAllEffectResultsForNormalMovement()
	{
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			foreach (Effect effect in list)
			{
				effect.ClearNormalMovementResults();
			}
		}
		foreach (Effect effect2 in m_worldEffects)
		{
			effect2.ClearNormalMovementResults();
		}
	}

	public void GatherAllEffectResultsInResponseToEvades(MovementCollection evadeMovementCollection)
	{
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			foreach (Effect effect in list)
			{
				effect.GatherResultsInResponseToEvades(evadeMovementCollection);
			}
		}
		foreach (Effect effect2 in m_worldEffects)
		{
			effect2.GatherResultsInResponseToEvades(evadeMovementCollection);
		}
	}

	public void GatherAllEffectResultsInResponseToKnockbacks(MovementCollection knockbackCollection)
	{
		foreach (List<Effect> actorEffects in m_actorEffects.Values)
		{
			foreach (Effect effect in actorEffects)
			{
				effect.GatherResultsInResponseToKnockbacks(knockbackCollection);
			}
		}
		foreach (Effect effect in m_worldEffects)
		{
			effect.GatherResultsInResponseToKnockbacks(knockbackCollection);
		}
	}

	public void GatherEffectResultsInResponseToMovementSegment(ServerGameplayUtils.MovementGameplayData gameplayData, MovementStage movementStage, ref List<MovementResults> moveResultsForSegment)
	{
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			foreach (Effect effect in list)
			{
				List<MovementResults> list2 = new List<MovementResults>();
				effect.GatherMovementResultsFromSegment(gameplayData.Actor, gameplayData.m_movementInstance, movementStage, gameplayData.m_currentlyConsideredPath.prev, gameplayData.m_currentlyConsideredPath, ref list2);
				List<MovementResults> movementResultsForMovementStage = effect.GetMovementResultsForMovementStage(movementStage);
				for (int i = 0; i < list2.Count; i++)
				{
					if (list2[i].ShouldMovementHitUpdateTargetLastKnownPos(gameplayData.Actor))
					{
						gameplayData.m_currentlyConsideredPath.m_visibleToEnemies = true;
						gameplayData.m_currentlyConsideredPath.m_updateLastKnownPos = true;
						Log.Info($"UpdateLastKnownPos {list2[i].m_triggeringMover?.DisplayName} " +
						         $"{list2[i].m_triggeringPath.square?.GetGridPos()} updating for movement actor effect hit"); // custom debug
					}
					gameplayData.m_currentlyConsideredPath.m_moverHasGameplayHitHere = true;
					movementResultsForMovementStage.Add(list2[i]);
					moveResultsForSegment.Add(list2[i]);
				}
			}
		}
		foreach (Effect effect2 in m_worldEffects)
		{
			List<MovementResults> list3 = new List<MovementResults>();
			effect2.GatherMovementResultsFromSegment(gameplayData.Actor, gameplayData.m_movementInstance, movementStage, gameplayData.m_currentlyConsideredPath.prev, gameplayData.m_currentlyConsideredPath, ref list3);
			List<MovementResults> movementResultsForMovementStage2 = effect2.GetMovementResultsForMovementStage(movementStage);
			for (int j = 0; j < list3.Count; j++)
			{
				if (list3[j].ShouldMovementHitUpdateTargetLastKnownPos(gameplayData.Actor))
				{
					gameplayData.m_currentlyConsideredPath.m_visibleToEnemies = true;
					gameplayData.m_currentlyConsideredPath.m_updateLastKnownPos = true;
					Log.Info($"UpdateLastKnownPos {list3[j].m_triggeringMover?.DisplayName} " +
					         $"{list3[j].m_triggeringPath.square?.GetGridPos()} updating for movement world effect hit"); // custom debug
				}
				gameplayData.m_currentlyConsideredPath.m_moverHasGameplayHitHere = true;
				movementResultsForMovementStage2.Add(list3[j]);
				moveResultsForSegment.Add(list3[j]);
			}
		}
	}

	public List<Effect.EffectKnockbackTargets> FindKnockbackTargetSets()
	{
		List<Effect.EffectKnockbackTargets> list = new List<Effect.EffectKnockbackTargets>();
		foreach (List<Effect> list2 in m_actorEffects.Values)
		{
			foreach (Effect effect in list2)
			{
				Dictionary<ActorData, Vector2> effectKnockbackTargets = effect.GetEffectKnockbackTargets();
				Effect.EffectKnockbackTargets effectKnockbackTargets2 = new Effect.EffectKnockbackTargets(effect.Target, effectKnockbackTargets);
				if (effectKnockbackTargets2 != null && effectKnockbackTargets2.m_sourceActor != null && effectKnockbackTargets2.m_knockbackTargets != null && effectKnockbackTargets2.m_knockbackTargets.Count > 0)
				{
					list.Add(effectKnockbackTargets2);
				}
			}
		}
		foreach (Effect effect2 in m_worldEffects)
		{
			Dictionary<ActorData, Vector2> effectKnockbackTargets3 = effect2.GetEffectKnockbackTargets();
			Effect.EffectKnockbackTargets effectKnockbackTargets4 = new Effect.EffectKnockbackTargets(effect2.Caster, effectKnockbackTargets3);
			if (effectKnockbackTargets4 != null && effectKnockbackTargets4.m_sourceActor != null && effectKnockbackTargets4.m_knockbackTargets != null && effectKnockbackTargets4.m_knockbackTargets.Count > 0)
			{
				list.Add(effectKnockbackTargets4);
			}
		}
		return list;
	}

	public bool HitsDoneExecuting(AbilityPriority phase)
	{
		bool result = true;
		foreach (List<Effect> list in m_actorEffects.Values)
		{
			foreach (Effect effect in list)
			{
				if (!HitsDoneExecutingForEffect(effect, phase))
				{
					result = false;
					break;
				}
			}
		}
		foreach (Effect effect2 in m_worldEffects)
		{
			if (!HitsDoneExecutingForEffect(effect2, phase))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private bool HitsDoneExecutingForEffect(Effect effect, AbilityPriority phase)
	{
		// rogues?
		//EffectSystem.Effect effect2;
		//if ((effect2 = (effect as EffectSystem.Effect)) != null)
		//{
		//	return !effect2.GetExecutingResults().Any((EffectResults effectResults) => !effectResults.HitsDoneExecuting());
		//}

		EffectResults resultsForPhase = effect.GetResultsForPhase(phase, true);
		return (effect.HitPhase != phase || resultsForPhase == null || resultsForPhase.HitsDoneExecuting());
	}

	public void ExecuteUnexecutedHitsForAllEffects(AbilityPriority phase, bool asFailsafe)
	{
		foreach (List<Effect> collection in m_actorEffects.Values)
		{
			foreach (Effect effect in new List<Effect>(collection))
			{
				if (!HitsDoneExecutingForEffect(effect, phase))
				{
					ExecuteUnexecutedHitsForEffect(effect, phase, asFailsafe);
				}
			}
		}
		foreach (Effect effect2 in new List<Effect>(m_worldEffects))
		{
			if (!HitsDoneExecutingForEffect(effect2, phase))
			{
				ExecuteUnexecutedHitsForEffect(effect2, phase, asFailsafe);
			}
		}
	}

	private void ExecuteUnexecutedHitsForEffect(Effect effect, AbilityPriority phase, bool asFailsafe)
	{
		// rogues?
		//EffectSystem.Effect effect2;
		//if ((effect2 = (effect as EffectSystem.Effect)) != null)
		//{
		//	using (List<EffectResults>.Enumerator enumerator = (from effectResults in effect2.GetExecutingResults()
		//	where !effectResults.HitsDoneExecuting()
		//	select effectResults).ToList<EffectResults>().GetEnumerator())
		//	{
		//		while (enumerator.MoveNext())
		//		{
		//			EffectResults effectResults2 = enumerator.Current;
		//			effectResults2.ExecuteUnexecutedEffectHits(asFailsafe);
		//		}
		//		return;
		//	}
		//}

		EffectResults resultsForPhase = effect.GetResultsForPhase(phase, true);
		if (effect.HitPhase == phase && resultsForPhase != null && !resultsForPhase.HitsDoneExecuting())
		{
			resultsForPhase.ExecuteUnexecutedEffectHits(asFailsafe);
		}
	}

	// rogues?
	//private void ExecuteUnexecutedHitsForEffect(EffectSystem.Effect effect, AbilityPriority phase, bool asFailsafe)
	//{
	//	if (effect.HitPhase == phase)
	//	{
	//		foreach (EffectResults effectResults in (from results in effect.GetExecutingResults()
	//		where !results.HitsDoneExecuting()
	//		select results).ToList<EffectResults>())
	//		{
	//			effectResults.ExecuteUnexecutedEffectHits(asFailsafe);
	//		}
	//	}
	//}

	// TODO call it?
	public void ExecuteUnexecutedMovementHitsForAllEffects(MovementStage stage, bool asFailsafe)
	{
		foreach (List<Effect> collection in m_actorEffects.Values)
		{
			foreach (Effect effect in new List<Effect>(collection))
			{
				effect.ExecuteUnexecutedMovementResults_Effect(stage, asFailsafe);
			}
		}
		foreach (Effect effect2 in new List<Effect>(m_worldEffects))
		{
			effect2.ExecuteUnexecutedMovementResults_Effect(stage, asFailsafe);
		}
	}

	public void ExecuteUnexecutedMovementHitsForAllEffectsForDistance(float distance, MovementStage stage, bool asFailsafe, out bool stillHasUnexecutedHits, out float nextUnexecutedHitDistance)
	{
		stillHasUnexecutedHits = false;
		nextUnexecutedHitDistance = float.MaxValue;
		foreach (List<Effect> collection in m_actorEffects.Values)
		{
			foreach (Effect effect in new List<Effect>(collection))
			{
				bool flag;
				float num;
				effect.ExecuteUnexecutedMovementResultsForDistance_Effect(distance, stage, asFailsafe, out flag, out num);
				if (flag)
				{
					stillHasUnexecutedHits |= true;
					if (num < nextUnexecutedHitDistance)
					{
						nextUnexecutedHitDistance = num;
					}
				}
			}
		}
		foreach (Effect effect2 in new List<Effect>(m_worldEffects))
		{
			bool flag2;
			float num2;
			effect2.ExecuteUnexecutedMovementResultsForDistance_Effect(distance, stage, asFailsafe, out flag2, out num2);
			if (flag2)
			{
				stillHasUnexecutedHits |= true;
				if (num2 < nextUnexecutedHitDistance)
				{
					nextUnexecutedHitDistance = num2;
				}
			}
		}
	}

	public bool HasPendingActorDeaths()
	{
		return m_actorsPendingDeath.Count > 0;
	}

	public void RemoveEffectsFromActorDeath()
	{
		foreach (Effect effect in m_actorEffects.SelectMany(kvp => kvp.Value).ToList())
		{
			m_actorsPendingDeath.ForEach(actor => effect.OnActorRemoved(actor));
		}
		
		// custom
		m_actorsPendingDeath.ForEach(RemoveEffectsOnActor);
		// end custom
		
		m_actorsPendingDeath.Clear();
	}

	public void OnActorDeath(ActorData actorData)
	{
		m_actorsPendingDeath.Add(actorData);
	}

	public void OnUnresolvedDamage(ActorHitResults actorHitResults)
	{
		ActorData caster = actorHitResults.m_hitParameters.Caster;
		ActorData target = actorHitResults.m_hitParameters.Target;
		int finalDamage = actorHitResults.FinalDamage;
		DamageSource damageSource = actorHitResults.m_hitParameters.DamageSource;
		ServerCombatManager.DamageType damageType = actorHitResults.DamageType;
		bool targetInCoverWrtDamage = actorHitResults.TargetInCoverWrtDamage;
		//HitChanceBracket.HitType hitType = actorHitResults.m_hitType;  // rogues
		foreach (Effect effect in GetActorEffects(target).ToList())
		{
			effect.OnUnresolvedDamage(target, caster, damageSource, finalDamage, damageType, actorHitResults);
		}
		foreach (Effect effect2 in GetActorEffects(caster).ToList())
		{
			effect2.OnDealtUnresolvedDamage(target, caster, damageSource, finalDamage, damageType, actorHitResults);
		}
	}

	public void OnBreakInvisibility(ActorData actor)
	{
		foreach (Effect effect in GetActorEffects(actor))
		{
			effect.OnBreakInvisibility();
		}
	}

	public void OnDamaged(ActorData target, ActorData caster, DamageSource src, int damageAmount, ActorHitResults actorHitResults)
	{
		foreach (List<Effect> source in m_actorEffects.Values)
		{
			foreach (Effect effect in source.ToList())
			{
				effect.OnDamaged_Base(target, caster, src, damageAmount, actorHitResults);
			}
		}
		foreach (Effect effect2 in m_worldEffects.ToList())
		{
			effect2.OnDamaged_Base(target, caster, src, damageAmount, actorHitResults);
		}
	}

	public void OnHealed(ActorData target, ActorData caster, DamageSource src, int healAmount, ActorHitResults actorHitResults)
	{
		foreach (List<Effect> source in m_actorEffects.Values)
		{
			foreach (Effect effect in source.ToList())
			{
				effect.OnHealed_Base(target, caster, src, healAmount, src, actorHitResults);
			}
		}
		foreach (Effect effect2 in m_worldEffects.ToList())
		{
			effect2.OnHealed_Base(target, caster, src, healAmount, src, actorHitResults);
		}
	}

	public void OnGainingEffect(Effect newEffect)
	{
		foreach (List<Effect> source in m_actorEffects.Values)
		{
			foreach (Effect effect in source.ToList())
			{
				effect.OnGainingEffect(newEffect);
			}
		}
		foreach (Effect effect2 in m_worldEffects.ToList())
		{
			effect2.OnGainingEffect(newEffect);
		}

		// rogues?
		//foreach (EffectTrigger effectTrigger in this.m_actorTriggers.SelectMany((KeyValuePair<ActorData, List<EffectTrigger>> kvp) => kvp.Value).ToList<EffectTrigger>())
		//{
		//	effectTrigger.EvaluateTick(TriggerEvent.EffectAdded, new object[]
		//	{
		//		newEffect
		//	});
		//}
	}

	public void OnLosingEffect(Effect oldEffect)
	{
		foreach (List<Effect> source in m_actorEffects.Values)
		{
			foreach (Effect effect in source.ToList())
			{
				effect.OnLosingEffect(oldEffect);
			}
		}
		foreach (Effect effect2 in m_worldEffects.ToList())
		{
			effect2.OnLosingEffect(oldEffect);
		}

		// rogues?
		//foreach (EffectTrigger effectTrigger in this.m_actorTriggers.SelectMany((KeyValuePair<ActorData, List<EffectTrigger>> kvp) => kvp.Value).ToList<EffectTrigger>())
		//{
		//	effectTrigger.EvaluateTick(TriggerEvent.EffectRemoved, new object[]
		//	{
		//		oldEffect
		//	});
		//}
	}

	public void NotifyEffectsOnExecutedActorHit(ActorData hitCaster, ActorData hitTarget, ActorHitResults hitResults)
	{
		if (m_actorEffects.ContainsKey(hitTarget))
		{
			foreach (Effect effect in m_actorEffects[hitTarget].ToList())
			{
				effect.OnExecutedActorHitOnTarget(hitCaster, hitResults);
			}
		}
		if (m_actorEffects.ContainsKey(hitCaster))
		{
			foreach (Effect effect2 in m_actorEffects[hitCaster].ToList())
			{
				effect2.OnExecutedActorHitCastByTarget(hitTarget, hitResults);
			}
		}
	}

	public bool IsMovementBlockedOnEnterSquare(ActorData mover, BoardSquare movingFrom, BoardSquare movingTo)
	{
		if (mover.GetActorStatus() != null && !mover.GetActorStatus().HasStatus(StatusType.Unstoppable))
		{
			using (List<Effect>.Enumerator enumerator = m_worldEffects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsMovementBlockedOnEnterSquare(mover, movingFrom, movingTo))
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	public void CollectSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (squaresToAvoid != null)
		{
			foreach (List<Effect> list in m_actorEffects.Values)
			{
				foreach (Effect effect in list)
				{
					effect.AddToSquaresToAvoidForRespawn(squaresToAvoid, forActor);
				}
			}
			foreach (Effect effect2 in m_worldEffects)
			{
				effect2.AddToSquaresToAvoidForRespawn(squaresToAvoid, forActor);
			}
		}
	}

	private void OnDrawGizmos()
	{
	}
#endif
}
