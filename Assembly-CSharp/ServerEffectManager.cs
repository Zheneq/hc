// ROGUES
// SERVER
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

	private Dictionary<ActorData, List<global::Effect>> m_actorEffects = new Dictionary<ActorData, List<global::Effect>>(8);

	private List<global::Effect> m_worldEffects = new List<global::Effect>();

	private List<ActorData> m_actorsAddedThisTurn = new List<ActorData>();

	private List<ActorData> m_actorsPendingDeath = new List<ActorData>();

	// rogues?
	//private Dictionary<ActorData, List<EffectTrigger>> m_actorTriggers = new Dictionary<ActorData, List<EffectTrigger>>();

	private static ServerEffectManager s_instance;

	public List<global::Effect> WorldEffects
	{
		get
		{
			return this.m_worldEffects;
		}
	}

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
		return ServerEffectManager.s_instance;
	}

	public static SharedEffectBarrierManager GetSharedEffectBarrierManager()
	{
		return ServerEffectManager.s_instance.m_sharedEffectBarrierManager;
	}

	private void Awake()
	{
		ServerEffectManager.s_instance = this;
		if (NetworkServer.active)
		{
			GameObject sharedEffectBarrierManagerPrefab = NetworkedSharedGameplayPrefabs.GetSharedEffectBarrierManagerPrefab();
			if (sharedEffectBarrierManagerPrefab != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(sharedEffectBarrierManagerPrefab, Vector3.zero, Quaternion.identity);
				NetworkServer.Spawn(gameObject);
				Object.DontDestroyOnLoad(gameObject);
				this.m_sharedEffectBarrierManager = gameObject.GetComponent<SharedEffectBarrierManager>();
			}
		}
	}

	private void OnDestroy()
	{
		if (NetworkServer.active && this.m_sharedEffectBarrierManager != null)
		{
			NetworkServer.Destroy(this.m_sharedEffectBarrierManager.gameObject);
		}
		GameFlowData.s_onAddActor -= this.AddActor;
		GameFlowData.s_onRemoveActor -= this.RemoveReferencesToActor;
		ServerEffectManager.s_instance = null;
	}

	private void Start()
	{
		if (GameFlowData.Get() != null && GameFlowData.Get().GetActors() != null)
		{
			foreach (ActorData actorToAdd in GameFlowData.Get().GetActors())
			{
				this.AddActor(actorToAdd);
			}
		}
		GameFlowData.s_onAddActor += this.AddActor;
		GameFlowData.s_onRemoveActor += this.RemoveReferencesToActor;
	}

	private void AddActor(ActorData actorToAdd)
	{
		if (this.m_actorEffects == null)
		{
			Log.Error("Trying to add actor {0} to the ServerEffectManager, but m_actorEffects is null", new object[]
			{
				actorToAdd.DisplayName
			});
		}
		else if (this.m_actorEffects.ContainsKey(actorToAdd))
		{
			Log.Warning("Trying to add actor {0} to the ServerEffectManager, but he's already present", new object[]
			{
				actorToAdd.DisplayName
			});
		}
		else
		{
			this.m_actorEffects.Add(actorToAdd, new List<global::Effect>());
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

		this.m_actorsAddedThisTurn.Add(actorToAdd);
		foreach (global::Effect effect4 in this.m_actorEffects.SelectMany((KeyValuePair<ActorData, List<global::Effect>> kvp) => kvp.Value).ToList<global::Effect>())
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

		this.RemoveEffectsOnActor(actor);
		this.RemoveEffectsCastedByActor(actor);
		if (this.m_actorEffects.ContainsKey(actor))
		{
			this.m_actorEffects.Remove(actor);
		}

		// rogues?
		//if (this.m_actorTriggers.ContainsKey(actor))
		//{
		//	this.m_actorTriggers.Remove(actor);
		//}
	}

	private void Update()
	{
		foreach (KeyValuePair<ActorData, List<global::Effect>> keyValuePair in this.m_actorEffects)
		{
			foreach (global::Effect effect in keyValuePair.Value)
			{
				effect.Update();
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects)
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
	public bool HasEffect(ActorData target, System.Type effectType)
	{
		return this.GetEffect(target, effectType) != null;
	}

	// added in rogues
	public bool HasEffectByCaster(ActorData target, ActorData caster, System.Type effectType)
	{
		return this.GetEffectsOnTargetByCaster(target, caster, effectType).Count > 0;
	}

	public bool HasEffectRequiringAccuratePositionOnClients(ActorData actor)
	{
		if (actor != null)
		{
			foreach (List<global::Effect> list in this.m_actorEffects.Values)
			{
				foreach (global::Effect effect in list)
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
	public global::Effect GetEffect(ActorData target, System.Type effectType)
	{
		global::Effect result = null;
		List<global::Effect> list = null;
		if (target != null && this.m_actorEffects.ContainsKey(target))
		{
			list = this.m_actorEffects[target];
		}
		else if (target == null)
		{
			list = this.m_worldEffects;
		}
		if (list != null)
		{
			foreach (global::Effect effect in list)
			{
				if (effect != null && effect.GetType() == effectType)
				{
					result = effect;
					break;
				}
			}
		}
		return result;
	}

	public Dictionary<ActorData, List<global::Effect>> GetAllActorEffects()
	{
		return this.m_actorEffects;
	}

	public List<global::Effect> GetActorEffects(ActorData actorData)
	{
		List<global::Effect> result = null;
		if (this.m_actorEffects.ContainsKey(actorData))
		{
			result = this.m_actorEffects[actorData];
		}
		return result;
	}

	// added in rogues
	public List<global::Effect> GetEffectsOnTargetByCaster(ActorData target, ActorData caster, System.Type effectType)
	{
		List<global::Effect> list = new List<global::Effect>();
		if (target != null && this.m_actorEffects.ContainsKey(target))
		{
			List<global::Effect> list2 = this.m_actorEffects[target];
			if (list2 != null)
			{
				foreach (global::Effect effect in list2)
				{
					if (effect != null && effect.GetType() == effectType && effect.Caster == caster)
					{
						list.Add(effect);
					}
				}
			}
		}
		return list;
	}

	public List<global::Effect> GetEffectsOnTargetGrantingStatus(ActorData target, StatusType status)
	{
		List<global::Effect> list = new List<global::Effect>();
		if (target != null && this.m_actorEffects.ContainsKey(target))
		{
			List<global::Effect> list2 = this.m_actorEffects[target];
			if (list2 != null)
			{
				foreach (global::Effect effect in list2)
				{
					if (effect != null)
					{
						List<StatusType> statuses = effect.GetStatuses();
						List<StatusType> statusesOnTurnStart = effect.GetStatusesOnTurnStart();
						if (statuses != null && statuses.Contains(status))
						{
							list.Add(effect);
						}
						else if (effect.m_time.age > 0 && statusesOnTurnStart != null && statusesOnTurnStart.Contains(status))
						{
							list.Add(effect);
						}
					}
				}
			}
		}
		return list;
	}

	public List<ActorData> GetCastersOfEffectsOnTargetGrantingStatus(ActorData target, StatusType status)
	{
		List<ActorData> list = new List<ActorData>();
		if (target != null && this.m_actorEffects.ContainsKey(target))
		{
			List<global::Effect> list2 = this.m_actorEffects[target];
			if (list2 != null)
			{
				foreach (global::Effect effect in list2)
				{
					if (effect != null && effect.Caster != null && !list.Contains(effect.Caster))
					{
						List<StatusType> statuses = effect.GetStatuses();
						List<StatusType> statusesOnTurnStart = effect.GetStatusesOnTurnStart();
						if (statuses != null && statuses.Contains(status))
						{
							list.Add(effect.Caster);
						}
						else if (effect.m_time.age > 0 && statusesOnTurnStart != null && statusesOnTurnStart.Contains(status))
						{
							list.Add(effect.Caster);
						}
					}
				}
			}
		}
		return list;
	}

	// added in rogues
	public List<global::Effect> GetAllActorEffectsByCaster(ActorData caster, System.Type effectType)
	{
		List<global::Effect> list = new List<global::Effect>();
		foreach (ActorData target in this.m_actorEffects.Keys)
		{
			list.AddRange(this.GetEffectsOnTargetByCaster(target, caster, effectType));
		}
		return list;
	}

	public List<global::Effect> GetWorldEffects()
	{
		return this.m_worldEffects;
	}

	public List<global::Effect> GetWorldEffectsByCaster(ActorData caster)
	{
		List<global::Effect> list = new List<global::Effect>();
		foreach (global::Effect effect in this.m_worldEffects)
		{
			if (effect.Caster == caster)
			{
				list.Add(effect);
			}
		}
		return list;
	}

	// rogues? what is Type?
	//public List<global::Effect> GetWorldEffectsByCaster(ActorData caster, Type type)
	//{
	//	List<global::Effect> list = new List<global::Effect>();
	//	foreach (global::Effect effect in this.m_worldEffects)
	//	{
	//		if (effect.Caster == caster && effect.GetType() == type)
	//		{
	//			list.Add(effect);
	//		}
	//	}
	//	return list;
	//}

	private global::Effect GetEffectToStackWith(global::Effect newEffect, List<global::Effect> targetEffects)
	{
		global::Effect result = null;
		if (newEffect.CanStack())
		{
			foreach (global::Effect effect in targetEffects)
			{
				if (effect.CanStackWith(newEffect))
				{
					result = effect;
					break;
				}
			}
		}
		return result;
	}

	public void ApplyEffect(global::Effect effect, int stacks = 1)
	{
		bool flag = true;
		if (effect.Target != null && effect.Caster != null)
		{
			bool flag2 = effect.Target.GetTeam() == effect.Caster.GetTeam();
			ActorStatus component = effect.Target.GetComponent<ActorStatus>();
			if (component.HasStatus(StatusType.EffectImmune, true))
			{
				flag = false;
			}
			else if (effect.Target != effect.Caster && flag2 && component.HasStatus(StatusType.CantBeHelpedByTeam, true))
			{
				flag = false;
			}
			else if (component.HasStatus(StatusType.BuffImmune, true) && effect.IsBuff())
			{
				flag = false;
			}
			else if (component.HasStatus(StatusType.DebuffImmune, true) && effect.IsDebuff())
			{
				flag = false;
			}
		}
		// rogues?
		//EffectSystem.Effect effect2;
		//if ((effect2 = (effect as EffectSystem.Effect)) != null && !effect2.Evaluate<bool>(effect2.EffectTemplate.ShouldEvaluate, Array.Empty<object>()))
		//{
		//	flag = false;
		//}
		if (!flag)
		{
			this.m_sharedEffectBarrierManager.NotifyEffectEnded(effect.m_guid);
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Debug.LogWarning("<color=green>Effect</color>: " + effect.GetDebugIdentifier("yellow") + " Skipping SERVER effect application");
			}
		}
		if (flag)
		{
			bool flag3 = true;
			if (effect.Target == null)
			{
				effect.AddToStack(stacks);
				this.m_worldEffects.Add(effect);
			}
			else
			{
				List<global::Effect> list;
				if (!this.m_actorEffects.TryGetValue(effect.Target, out list))
				{
					list = new List<global::Effect>();
					this.m_actorEffects.Add(effect.Target, list);
				}
				global::Effect effectToStackWith = this.GetEffectToStackWith(effect, list);
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
					flag3 = false;
					effectToStackWith.AddToStack(stacks);

					// rogues?
					//EffectSystem.Effect effectSystemStackEffect;
					//if ((effectSystemStackEffect = (effectToStackWith as EffectSystem.Effect)) != null && effectSystemStackEffect.EffectTemplate.ConsolidateEffects)
					//{
					//	effectSystemStackEffect.targets.AddRange(from actorData in ((EffectSystem.Effect)effect).targets
					//	where !effectSystemStackEffect.targets.Contains(actorData)
					//	select actorData);
					//}

					this.m_sharedEffectBarrierManager.NotifyEffectEnded(effect.m_guid);
				}
			}
			if (flag3)
			{
				effect.OnStart();
			}
			if (effect.Target != null)
			{
				if (effect.CanAbsorb())
				{
					this.UpdateAbsorbPoints(effect);
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

			this.OnGainingEffect(effect);
			foreach (ActorData actor in this.m_actorsAddedThisTurn)
			{
				effect.OnActorAdded(actor);
			}
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				string text = (effect.Caster != null) ? effect.Caster.DebugNameString("yellow") : "[None]";
				string text2 = (effect.Target != null) ? effect.Target.DebugNameString("yellow") : "[None]";
				Log.Warning(string.Concat(new string[]
				{
					"<color=green>Effect</color>: ",
					effect.GetDebugIdentifier("yellow"),
					" APPLIED\nCaster: ",
					text,
					", Target: ",
					text2
				}));
			}
		}
	}

	public void RemoveEffect(global::Effect effectToRemove, List<global::Effect> effectListToRemoveFrom)
	{
		effectToRemove.End();
		effectListToRemoveFrom.Remove(effectToRemove);
		this.NotifyLosingEffect(effectToRemove);
		if (EffectDebugConfig.TracingAddAndRemove())
		{
			string text = (effectToRemove.Caster != null) ? effectToRemove.Caster.DebugNameString("yellow") : "[None]";
			string text2 = (effectToRemove.Target != null) ? effectToRemove.Target.DebugNameString("yellow") : "[None]";
			Log.Warning(string.Concat(new string[]
			{
				"<color=green>Effect</color>: ",
				effectToRemove.GetDebugIdentifier("yellow"),
				" REMOVED\nCaster: ",
				text,
				", Target: ",
				text2
			}));
		}
	}

	public void RemoveEffects(List<global::Effect> effectsToRemove, List<global::Effect> effectListToRemoveFrom)
	{
		foreach (global::Effect effect in effectsToRemove)
		{
			if (effectListToRemoveFrom.Remove(effect))
			{
				effect.End();
				this.NotifyLosingEffect(effect);
			}
		}
	}

	public void RemoveEffectsOnActor(ActorData actor)
	{
		if (actor != null && this.m_actorEffects.ContainsKey(actor))
		{
			List<global::Effect> list = new List<global::Effect>();
			List<global::Effect> list2 = this.m_actorEffects[actor];
			if (list2 != null)
			{
				foreach (global::Effect item in list2)
				{
					list.Add(item);
				}
			}
			foreach (global::Effect effectToRemove in list)
			{
				this.RemoveEffect(effectToRemove, list2);
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
			foreach (KeyValuePair<ActorData, List<global::Effect>> keyValuePair in this.m_actorEffects)
			{
				List<global::Effect> list = new List<global::Effect>();
				foreach (global::Effect effect in keyValuePair.Value)
				{
					if (effect.Caster == actor)
					{
						list.Add(effect);
					}
				}
				foreach (global::Effect effectToRemove in list)
				{
					this.RemoveEffect(effectToRemove, keyValuePair.Value);
				}
			}
			List<global::Effect> list2 = new List<global::Effect>();
			foreach (global::Effect effect2 in this.m_worldEffects)
			{
				if (effect2.Caster == actor)
				{
					list2.Add(effect2);
				}
			}
			foreach (global::Effect effectToRemove2 in list2)
			{
				this.RemoveEffect(effectToRemove2, this.m_worldEffects);
			}
		}
	}

	public void NotifyLosingEffect(global::Effect effectRemoved)
	{
		this.m_sharedEffectBarrierManager.NotifyEffectEnded(effectRemoved.m_guid);
		if (effectRemoved.Target != null)
		{
			PassiveData component = effectRemoved.Target.GetComponent<PassiveData>();
			if (component)
			{
				component.OnLosingEffect(effectRemoved);
			}
		}
		this.OnLosingEffect(effectRemoved);
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
		this.ClearAllEffectResults();
		foreach (List<global::Effect> effectList in this.m_actorEffects.Values)
		{
			this.OnTurnStart(effectList);
		}
		this.OnTurnStart(this.m_worldEffects);
	}

	private void OnTurnStart(List<global::Effect> effectList)
	{
		List<global::Effect> list = new List<global::Effect>();
		foreach (global::Effect effect in effectList.ToList<global::Effect>())
		{
			bool flag = this.ValidateEffectLifetime(effect, false);

			// rogues?
			//EffectSystem.Effect effect2 = effect as EffectSystem.Effect;
			//if (effect2 != null)
			//{
			//	flag |= (effect2.EffectLifecycle == EffectLifecycle.OnEnd && !effect2.pendingEffectResults.Any<EffectResults>());
			//}

			if (effect.ShouldEndEarly() || flag)
			{
				list.Add(effect);
			}
			else
			{
				effect.OnTurnStart();

				// rogues?
				//if (effect2 != null)
				//{
				//	flag = (effect2.EffectLifecycle == EffectLifecycle.OnEnd && !effect2.pendingEffectResults.Any<EffectResults>());
				//}

				if (effect.ShouldEndEarly() || flag)
				{
					list.Add(effect);
				}
			}
		}
		foreach (global::Effect effectToRemove in list)
		{
			this.RemoveEffect(effectToRemove, effectList);
		}
	}

	public void OnTurnEnd()  // Team nextTeam in rogues
	{
		foreach (List<global::Effect> effectList in this.m_actorEffects.Values)
		{
			this.OnTurnEnd(effectList); // , nextTeam in rogues
		}
		this.OnTurnEnd(this.m_worldEffects); // , nextTeam in rogues
	}

	// rogues?
	//private bool ValidateEffectLifetime(global::Effect effect, bool isEnd)
	//{
	//	// TODO recheck ValidateEffectLifetime
	//	//Team actingTeam = GameFlowData.Get().ActingTeam;
	//	EffectDuration time = effect.m_time;
	//	EffectDuration.Termination termination = time.termination;
	//	bool flag2;
	//	if ((termination != EffectDuration.Termination.CasterTeamEnd
	//			//|| effect.Caster.GetTeam() != actingTeam
	//			|| !isEnd)
	//		&& (termination != EffectDuration.Termination.CasterTeamStart
	//		//|| effect.Caster.GetTeam() != actingTeam
	//		|| isEnd))
	//	{
	//		bool flag;
	//		if (termination == EffectDuration.Termination.TargetTeamEnd)
	//		{
	//			ActorData target = effect.Target;
	//			flag = (target != null
	//				//&& target.GetTeam() == actingTeam
	//				);
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
	//				if (target2 != null
	//					//&& target2.GetTeam() == actingTeam
	//					&& !isEnd)
	//				{
	//					goto IL_BB;
	//				}
	//			}
	//			if (termination != EffectDuration.Termination.ActiveTeamAtApplicationEnd
	//				//|| effect.m_time.activeTeamAtApplication != actingTeam
	//				|| !isEnd)
	//			{
	//				flag2 = (termination == EffectDuration.Termination.ActiveTeamAtApplicationStart
	//					//&& effect.m_time.activeTeamAtApplication == actingTeam
	//					&& !isEnd);
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

	private void OnTurnEnd(List<global::Effect> effectList)  // , Team nextTeam in rogues
	{
		this.m_actorsAddedThisTurn.Clear();
		List<global::Effect> list = new List<global::Effect>();
		foreach (global::Effect effect in effectList.ToList<global::Effect>())
		{
			bool flag = this.ValidateEffectLifetime(effect, true);

			// rogues?
			//EffectSystem.Effect effect2 = effect as EffectSystem.Effect;
			//if (effect2 != null)
			//{
			//	flag = (effect2.EffectLifecycle == EffectLifecycle.OnEnd && !effect2.pendingEffectResults.Any<EffectResults>());
			//}

			if (effect.ShouldEndEarly() || flag)
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

				if (effect.ShouldEndEarly() || flag)
				{
					list.Add(effect);
				}
			}
		}
		foreach (global::Effect effectToRemove in list)
		{
			this.RemoveEffect(effectToRemove, effectList);
		}
	}

	public int AdjustDamage(ActorData target, int damage)
	{
		int result = damage;
		List<global::Effect> list = new List<global::Effect>();
		if (this.m_actorEffects.ContainsKey(target))
		{
			List<global::Effect> list2 = this.m_actorEffects[target];
			if (list2 != null)
			{
				foreach (global::Effect effect in list2)
				{
					if (effect.CanAbsorb())
					{
						list.Add(effect);
					}
				}
			}
		}
		list.Sort(delegate (global::Effect x, global::Effect y)
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
			int num = x.m_time.duration - x.m_time.age;
			if (x.m_time.duration <= 0)
			{
				num = 10000;
			}
			int value = y.m_time.duration - y.m_time.age;
			if (y.m_time.duration <= 0)
			{
				value = 10000;
			}
			return num.CompareTo(value);
		});
		foreach (global::Effect effect2 in list)
		{
			effect2.AbsorbDamage(ref result);
		}
		return result;
	}

	public void UpdateAbsorbPoints(global::Effect effect)
	{
		int absorbAmount = effect.Absorbtion.m_absorbAmount;
		effect.Target.SetAbsorbPoints(this.CountAbsorbPoints(effect.Target));
		GameEventManager.ActorHitHealthChangeArgs args = new GameEventManager.ActorHitHealthChangeArgs(GameEventManager.ActorHitHealthChangeArgs.ChangeType.Absorb, absorbAmount, effect.Target, effect.Caster, effect.IsCharacterSpecificAbility());
		GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorDamaged_Server, args);
	}

	public int CountAbsorbPoints(ActorData actor)
	{
		int num = 0;
		if (this.m_actorEffects.ContainsKey(actor))
		{
			List<global::Effect> list = this.m_actorEffects[actor];
			if (list != null)
			{
				foreach (global::Effect effect in list)
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
		return (from e in this.m_actorEffects.SelectMany((KeyValuePair<ActorData, List<global::Effect>> kvp) => kvp.Value)
				where e.GetCasterAnimationIndex(phaseIndex) > 0
				select e).Concat(from e in this.m_worldEffects
								 where e.GetCasterAnimationIndex(phaseIndex) > 0
								 select e).SelectMany(delegate (global::Effect e)
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
								 }).ToList<EffectResults>();
	}

	public List<EffectResults> FindAnimlessEffectsWithHitsForPhase(AbilityPriority phaseIndex)
	{
		return (from e in this.m_actorEffects.SelectMany((KeyValuePair<ActorData, List<global::Effect>> kvp) => kvp.Value)
				where this.ShouldAddActorAnimEntryAsAnimlessEffect(e, phaseIndex)
				select e).Concat(from e in this.m_worldEffects
								 where this.ShouldAddActorAnimEntryAsAnimlessEffect(e, phaseIndex)
								 select e).SelectMany(delegate (global::Effect e)
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
								 }).ToList<EffectResults>();
	}

	private bool ShouldAddActorAnimEntryAsAnimlessEffect(global::Effect effect, AbilityPriority phaseIndex)
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
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in list)
			{
				this.CollectEffectOutgoingHitSummary(effect, phase, caster, summary);
			}
		}
	}

	private void CollectEffectOutgoingHitSummary(global::Effect effect, AbilityPriority phase, ActorData caster, ServerActionBuffer.GatheredOutgoingHitsSummary summary)
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
		for (int i = 0; i < this.m_worldEffects.Count; i++)
		{
			global::Effect effect = this.m_worldEffects[i];
			this.CountEffectDamageAndHealing(effect, phase, target, ref damage, ref healing);
		}
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				global::Effect effect2 = list[j];
				this.CountEffectDamageAndHealing(effect2, phase, target, ref damage, ref healing);
			}
		}
	}

	private void CountEffectDamageAndHealing(global::Effect effect, AbilityPriority phase, ActorData target, ref int damage, ref int healing)
	{
		if (effect != null && effect.HitPhase == phase && effect.GetResultsForPhase(phase, true).GatheredResults)
		{
			ServerGameplayUtils.CountDamageAndHeal(effect.GetResultsForPhase(phase, true).DamageResults, target, ref damage, ref healing);
		}
	}

	public void IntegrateHpDeltasForEffects(AbilityPriority phase, ref Dictionary<ActorData, int> actorToDeltaHP, bool ignoreEffectsWithTheatricsEntry)
	{
		for (int i = 0; i < this.m_worldEffects.Count; i++)
		{
			global::Effect effect = this.m_worldEffects[i];
			this.IntegrateHpDeltasForEffect(effect, phase, ref actorToDeltaHP, ignoreEffectsWithTheatricsEntry);
		}
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				global::Effect effect2 = list[j];
				this.IntegrateHpDeltasForEffect(effect2, phase, ref actorToDeltaHP, ignoreEffectsWithTheatricsEntry);
			}
		}
	}

	private void IntegrateHpDeltasForEffect(global::Effect effect, AbilityPriority phase, ref Dictionary<ActorData, int> actorToDeltaHP, bool ignoreEffectsWithTheatricsEntry)
	{
		if (effect != null && effect.HitPhase == phase && (!ignoreEffectsWithTheatricsEntry || (effect.GetCasterAnimationIndex(phase) == 0 && !effect.AddActorAnimEntryIfHasHits(phase))) && effect.GetResultsForPhase(phase, true).GatheredResults)
		{
			ServerGameplayUtils.IntegrateHpDeltas(effect.GetResultsForPhase(phase, true).DamageResults, ref actorToDeltaHP);
		}
	}

	public void IntegrateMovementDamageResults_Evasion(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		for (int i = 0; i < this.m_worldEffects.Count; i++)
		{
			this.m_worldEffects[i].IntegrateDamageResultsForEvasion(ref actorToDeltaHP);
		}
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				list[j].IntegrateDamageResultsForEvasion(ref actorToDeltaHP);
			}
		}
	}

	public void IntegrateMovementDamageResults_Knockback(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		for (int i = 0; i < this.m_worldEffects.Count; i++)
		{
			this.m_worldEffects[i].IntegrateDamageResultsForKnockback(ref actorToDeltaHP);
		}
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				list[j].IntegrateDamageResultsForKnockback(ref actorToDeltaHP);
			}
		}
	}

	public void GatherGrossDamageResults_Effects(AbilityPriority phase, ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, int> actorToGrossDamage_fake, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
	{
		for (int i = 0; i < this.m_worldEffects.Count; i++)
		{
			global::Effect effect = this.m_worldEffects[i];
			this.GatherGrossDamageResults_Effect(effect, phase, ref actorToGrossDamage_real, ref actorToGrossDamage_fake, ref stats);
		}
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				global::Effect effect2 = list[j];
				this.GatherGrossDamageResults_Effect(effect2, phase, ref actorToGrossDamage_real, ref actorToGrossDamage_fake, ref stats);
			}
		}
	}

	public void GatherGrossDamageResults_Effect(global::Effect effect, AbilityPriority phase, ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, int> actorToGrossDamage_fake, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
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
		for (int i = 0; i < this.m_worldEffects.Count; i++)
		{
			global::Effect effect = this.m_worldEffects[i];
			this.GatherGrossDamageResults_Effect_Evasion(effect, ref actorToGrossDamage_real, ref stats);
		}
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			for (int j = 0; j < list.Count; j++)
			{
				global::Effect effect2 = list[j];
				this.GatherGrossDamageResults_Effect_Evasion(effect2, ref actorToGrossDamage_real, ref stats);
			}
		}
	}

	public void GatherGrossDamageResults_Effect_Evasion(global::Effect effect, ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
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
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in list)
			{
				effect.OnBeforeGatherEffectResults(phase);
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects)
		{
			effect2.OnBeforeGatherEffectResults(phase);
		}
	}

	public void ClearAllEffectResults()
	{
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in list)
			{
				effect.ClearEffectResults();
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects)
		{
			effect2.ClearEffectResults();
		}
	}

	public void ClearAllEffectResultsForNormalMovement()
	{
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in list)
			{
				effect.ClearNormalMovementResults();
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects)
		{
			effect2.ClearNormalMovementResults();
		}
	}

	public void GatherAllEffectResultsInResponseToEvades(MovementCollection evadeMovementCollection)
	{
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in list)
			{
				effect.GatherResultsInResponseToEvades(evadeMovementCollection);
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects)
		{
			effect2.GatherResultsInResponseToEvades(evadeMovementCollection);
		}
	}

	public void GatherAllEffectResultsInResponseToKnockbacks(MovementCollection knockbackCollection)
	{
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in list)
			{
				effect.GatherResultsInResponseToKnockbacks(knockbackCollection);
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects)
		{
			effect2.GatherResultsInResponseToKnockbacks(knockbackCollection);
		}
	}

	public void GatherEffectResultsInResponseToMovementSegment(ServerGameplayUtils.MovementGameplayData gameplayData, MovementStage movementStage, ref List<MovementResults> moveResultsForSegment)
	{
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in list)
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
					}
					gameplayData.m_currentlyConsideredPath.m_moverHasGameplayHitHere = true;
					movementResultsForMovementStage.Add(list2[i]);
					moveResultsForSegment.Add(list2[i]);
				}
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects)
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
				}
				gameplayData.m_currentlyConsideredPath.m_moverHasGameplayHitHere = true;
				movementResultsForMovementStage2.Add(list3[j]);
				moveResultsForSegment.Add(list3[j]);
			}
		}
	}

	public List<global::Effect.EffectKnockbackTargets> FindKnockbackTargetSets()
	{
		List<global::Effect.EffectKnockbackTargets> list = new List<global::Effect.EffectKnockbackTargets>();
		foreach (List<global::Effect> list2 in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in list2)
			{
				Dictionary<ActorData, Vector2> effectKnockbackTargets = effect.GetEffectKnockbackTargets();
				global::Effect.EffectKnockbackTargets effectKnockbackTargets2 = new global::Effect.EffectKnockbackTargets(effect.Target, effectKnockbackTargets);
				if (effectKnockbackTargets2 != null && effectKnockbackTargets2.m_sourceActor != null && effectKnockbackTargets2.m_knockbackTargets != null && effectKnockbackTargets2.m_knockbackTargets.Count > 0)
				{
					list.Add(effectKnockbackTargets2);
				}
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects)
		{
			Dictionary<ActorData, Vector2> effectKnockbackTargets3 = effect2.GetEffectKnockbackTargets();
			global::Effect.EffectKnockbackTargets effectKnockbackTargets4 = new global::Effect.EffectKnockbackTargets(effect2.Caster, effectKnockbackTargets3);
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
		foreach (List<global::Effect> list in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in list)
			{
				if (!this.HitsDoneExecutingForEffect(effect, phase))
				{
					result = false;
					break;
				}
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects)
		{
			if (!this.HitsDoneExecutingForEffect(effect2, phase))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private bool HitsDoneExecutingForEffect(global::Effect effect, AbilityPriority phase)
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
		foreach (List<global::Effect> collection in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in new List<global::Effect>(collection))
			{
				if (!this.HitsDoneExecutingForEffect(effect, phase))
				{
					this.ExecuteUnexecutedHitsForEffect(effect, phase, asFailsafe);
				}
			}
		}
		foreach (global::Effect effect2 in new List<global::Effect>(this.m_worldEffects))
		{
			if (!this.HitsDoneExecutingForEffect(effect2, phase))
			{
				this.ExecuteUnexecutedHitsForEffect(effect2, phase, asFailsafe);
			}
		}
	}

	private void ExecuteUnexecutedHitsForEffect(global::Effect effect, AbilityPriority phase, bool asFailsafe)
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

	public void ExecuteUnexecutedMovementHitsForAllEffects(MovementStage stage, bool asFailsafe)
	{
		foreach (List<global::Effect> collection in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in new List<global::Effect>(collection))
			{
				effect.ExecuteUnexecutedMovementResults_Effect(stage, asFailsafe);
			}
		}
		foreach (global::Effect effect2 in new List<global::Effect>(this.m_worldEffects))
		{
			effect2.ExecuteUnexecutedMovementResults_Effect(stage, asFailsafe);
		}
	}

	public void ExecuteUnexecutedMovementHitsForAllEffectsForDistance(float distance, MovementStage stage, bool asFailsafe, out bool stillHasUnexecutedHits, out float nextUnexecutedHitDistance)
	{
		stillHasUnexecutedHits = false;
		nextUnexecutedHitDistance = float.MaxValue;
		foreach (List<global::Effect> collection in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in new List<global::Effect>(collection))
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
		foreach (global::Effect effect2 in new List<global::Effect>(this.m_worldEffects))
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
		return this.m_actorsPendingDeath.Count > 0;
	}

	public void RemoveEffectsFromActorDeath()
	{
		using (List<global::Effect>.Enumerator enumerator = this.m_actorEffects.SelectMany((KeyValuePair<ActorData, List<global::Effect>> kvp) => kvp.Value).ToList<global::Effect>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				global::Effect effect = enumerator.Current;
				this.m_actorsPendingDeath.ForEach(delegate (ActorData actor)
				{
					effect.OnActorRemoved(actor);
				});
			}
		}
		this.m_actorsPendingDeath.Clear();
	}

	public void OnActorDeath(ActorData actorData)
	{
		this.m_actorsPendingDeath.Add(actorData);
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
		foreach (global::Effect effect in this.GetActorEffects(target).ToList<global::Effect>())
		{
			effect.OnUnresolvedDamage(target, caster, damageSource, finalDamage, damageType, actorHitResults);
		}
		foreach (global::Effect effect2 in this.GetActorEffects(caster).ToList<global::Effect>())
		{
			effect2.OnDealtUnresolvedDamage(target, caster, damageSource, finalDamage, damageType, actorHitResults);
		}
	}

	public void OnBreakInvisibility(ActorData actor)
	{
		foreach (global::Effect effect in this.GetActorEffects(actor))
		{
			effect.OnBreakInvisibility();
		}
	}

	public void OnDamaged(ActorData target, ActorData caster, DamageSource src, int damageAmount, ActorHitResults actorHitResults)
	{
		foreach (List<global::Effect> source in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in source.ToList<global::Effect>())
			{
				effect.OnDamaged_Base(target, caster, src, damageAmount, actorHitResults);
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects.ToList<global::Effect>())
		{
			effect2.OnDamaged_Base(target, caster, src, damageAmount, actorHitResults);
		}
	}

	public void OnHealed(ActorData target, ActorData caster, DamageSource src, int healAmount, ActorHitResults actorHitResults)
	{
		foreach (List<global::Effect> source in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in source.ToList<global::Effect>())
			{
				effect.OnHealed_Base(target, caster, src, healAmount, src, actorHitResults);
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects.ToList<global::Effect>())
		{
			effect2.OnHealed_Base(target, caster, src, healAmount, src, actorHitResults);
		}
	}

	public void OnGainingEffect(global::Effect newEffect)
	{
		foreach (List<global::Effect> source in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in source.ToList<global::Effect>())
			{
				effect.OnGainingEffect(newEffect);
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects.ToList<global::Effect>())
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

	public void OnLosingEffect(global::Effect oldEffect)
	{
		foreach (List<global::Effect> source in this.m_actorEffects.Values)
		{
			foreach (global::Effect effect in source.ToList<global::Effect>())
			{
				effect.OnLosingEffect(oldEffect);
			}
		}
		foreach (global::Effect effect2 in this.m_worldEffects.ToList<global::Effect>())
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
		if (this.m_actorEffects.ContainsKey(hitTarget))
		{
			foreach (global::Effect effect in this.m_actorEffects[hitTarget].ToList<global::Effect>())
			{
				effect.OnExecutedActorHitOnTarget(hitCaster, hitResults);
			}
		}
		if (this.m_actorEffects.ContainsKey(hitCaster))
		{
			foreach (global::Effect effect2 in this.m_actorEffects[hitCaster].ToList<global::Effect>())
			{
				effect2.OnExecutedActorHitCastByTarget(hitTarget, hitResults);
			}
		}
	}

	public bool IsMovementBlockedOnEnterSquare(ActorData mover, BoardSquare movingFrom, BoardSquare movingTo)
	{
		if (mover.GetActorStatus() != null && !mover.GetActorStatus().HasStatus(StatusType.Unstoppable, true))
		{
			using (List<global::Effect>.Enumerator enumerator = this.m_worldEffects.GetEnumerator())
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
			foreach (List<global::Effect> list in this.m_actorEffects.Values)
			{
				foreach (global::Effect effect in list)
				{
					effect.AddToSquaresToAvoidForRespawn(squaresToAvoid, forActor);
				}
			}
			foreach (global::Effect effect2 in this.m_worldEffects)
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
