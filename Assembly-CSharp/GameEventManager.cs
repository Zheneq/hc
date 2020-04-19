using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
	private Dictionary<GameEventManager.EventType, List<WeakReference>> m_listenersByEvent = new Dictionary<GameEventManager.EventType, List<WeakReference>>();

	private List<GameEventManager.ReferenceToRemove> m_referencesToRemove = new List<GameEventManager.ReferenceToRemove>();

	private int m_firingEventsCount;

	private static GameEventManager s_instance;

	public static GameEventManager Get()
	{
		if (GameEventManager.s_instance == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameEventManager.Get()).MethodHandle;
			}
			GameEventManager.s_instance = new GameEventManager();
		}
		return GameEventManager.s_instance;
	}

	~GameEventManager()
	{
		GameEventManager.s_instance = null;
	}

	public void AddAllListenersTo(IGameEventListener whoTo)
	{
		this.PerformActionOnEvents(whoTo, delegate(IGameEventListener a, GameEventManager.EventType b)
		{
			this.AddListener(a, b);
		});
	}

	public void RemoveAllListenersFrom(IGameEventListener whoTo)
	{
		this.PerformActionOnEvents(whoTo, delegate(IGameEventListener a, GameEventManager.EventType b)
		{
			this.RemoveListener(a, b);
		});
	}

	private void PerformActionOnEvents(IGameEventListener whoTo, Action<IGameEventListener, GameEventManager.EventType> action)
	{
		IEnumerator enumerator = Enum.GetValues(typeof(GameEventManager.EventType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				GameEventManager.EventType eventType = (GameEventManager.EventType)obj;
				if (eventType != GameEventManager.EventType.Invalid)
				{
					action(whoTo, eventType);
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameEventManager.PerformActionOnEvents(IGameEventListener, Action<IGameEventListener, GameEventManager.EventType>)).MethodHandle;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				disposable.Dispose();
			}
		}
	}

	public void AddListener(IGameEventListener listener, GameEventManager.EventType eventType)
	{
		WeakReference item = new WeakReference(listener, false);
		List<WeakReference> list;
		if (!this.m_listenersByEvent.TryGetValue(eventType, out list))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameEventManager.AddListener(IGameEventListener, GameEventManager.EventType)).MethodHandle;
			}
			list = new List<WeakReference>
			{
				item
			};
			this.m_listenersByEvent.Add(eventType, list);
		}
		else if (!list.Contains(item))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			list.Add(item);
		}
	}

	public void RemoveListener(IGameEventListener listener, GameEventManager.EventType eventType)
	{
		List<WeakReference> list;
		if (this.m_listenersByEvent.TryGetValue(eventType, out list))
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				WeakReference weakReference = list[i];
				if (weakReference.IsAlive)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameEventManager.RemoveListener(IGameEventListener, GameEventManager.EventType)).MethodHandle;
					}
					if (weakReference.Target == listener)
					{
						if (this.m_firingEventsCount == 0)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							list.Remove(weakReference);
						}
						else
						{
							this.m_referencesToRemove.Add(new GameEventManager.ReferenceToRemove
							{
								referenceList = list,
								item = weakReference
							});
						}
						return;
					}
				}
				i++;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public bool FireEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		return this.FireEvent(eventType, args, null);
	}

	public bool FireEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, IGameEventListener whoTo)
	{
		bool result = false;
		try
		{
			List<WeakReference> list;
			if (this.m_listenersByEvent.TryGetValue(eventType, out list))
			{
				this.m_firingEventsCount++;
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					WeakReference weakReference = list[i];
					if (weakReference.IsAlive)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(GameEventManager.FireEvent(GameEventManager.EventType, GameEventManager.GameEventArgs, IGameEventListener)).MethodHandle;
						}
						if (whoTo == null)
						{
							goto IL_75;
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (whoTo == weakReference.Target)
						{
							goto IL_75;
						}
						goto IL_B6;
						IL_75:
						IGameEventListener gameEventListener = (IGameEventListener)weakReference.Target;
						gameEventListener.OnGameEvent(eventType, args);
						result = true;
					}
					else
					{
						this.m_referencesToRemove.Add(new GameEventManager.ReferenceToRemove
						{
							referenceList = list,
							item = weakReference
						});
					}
					IL_B6:
					i++;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_firingEventsCount--;
				this.RemoveDeadReferences();
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return result;
	}

	private void RemoveDeadReferences()
	{
		if (this.m_firingEventsCount == 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameEventManager.RemoveDeadReferences()).MethodHandle;
			}
			int i = 0;
			int count = this.m_referencesToRemove.Count;
			while (i < count)
			{
				GameEventManager.ReferenceToRemove referenceToRemove = this.m_referencesToRemove[i];
				referenceToRemove.referenceList.Remove(referenceToRemove.item);
				i++;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_referencesToRemove.Clear();
		}
	}

	public enum EventType
	{
		Invalid,
		CharacterVisualDeath,
		ActorDamaged_Server,
		ActorDamaged_Client,
		ActorKnockback,
		CharacterHealedOrBuffed,
		TurnTick,
		CharacterLocked,
		CharacterRespawn,
		GameTeardown,
		GameObjectsDestroyed,
		VisualSceneLoaded,
		GameCameraCreatedPre,
		GameCameraCreated,
		GameCameraCreatedPost,
		TheatricsAbilityHighlightStart,
		TheatricsAbilitiesEnd,
		TheatricsEvasionMoveStart,
		TheatricsAbilityAnimationStart,
		ServerActionBufferPhaseStart,
		ServerActionBufferActionsDone,
		MatchEnded,
		MatchObjectiveEvent,
		AbilityUsed,
		PowerUpActivated,
		CardUsed,
		GraphicsQualityChanged,
		SystemEscapeMenuOnReturnToGameClick,
		BoardSquareVisibleShadeChanged,
		GameFlowDataStarted,
		NpcSpawned,
		PostCharacterDeath,
		CharacterEnteredQueryArea,
		CharacterExitedQueryArea,
		PatrolPointEvent,
		WanderStateEvent,
		ScriptCommunication,
		UIPhaseStartedPrep,
		UIPhaseStartedEvasion,
		UIPhaseStartedCombat,
		UIPhaseStartedMovement,
		UIPhaseStartedDecision,
		UITutorialHighlightChanged,
		ClientResolutionStarted,
		ReplaceVFXPrefab,
		NormalMovementStart,
		FrontEndSelectionChatterCue,
		FrontEndReady,
		FrontEndEquipMod,
		FrontEndEquipCatalyst,
		AppStateChanged,
		ActorHealed_Client,
		ActorHealed_Server,
		ActorGainedAbsorb_Client,
		ActorGainedAbsorb_Server,
		ActorPing,
		ReconnectReplayStateChanged,
		ReplayRestart,
		ReplaySeekFinished,
		GametimeScaleChange,
		ActiveControlChangedToEnemyTeam,
		ClientRagdollTriggerHit
	}

	private struct ReferenceToRemove
	{
		public List<WeakReference> referenceList;

		public WeakReference item;
	}

	public abstract class GameEventArgs
	{
	}

	public class ActivationInfo : GameEventManager.GameEventArgs
	{
		public AbilityData.ActionType actionType;

		public bool active;
	}

	public class CharacterDeathEventArgs : GameEventManager.GameEventArgs
	{
		public ActorData deadCharacter;
	}

	public class CharacterRagdollHitEventArgs : GameEventManager.GameEventArgs
	{
		public ActorData m_ragdollingActor;

		public ActorData m_triggeringActor;
	}

	public class CharacterRespawnEventArgs : GameEventManager.GameEventArgs
	{
		public ActorData respawningCharacter;
	}

	public class ActorHitHealthChangeArgs : GameEventManager.GameEventArgs
	{
		public GameEventManager.ActorHitHealthChangeArgs.ChangeType m_type;

		public int m_amount;

		public ActorData m_target;

		public ActorData m_caster;

		public bool m_fromCharacterSpecificAbility;

		public ActorHitHealthChangeArgs(GameEventManager.ActorHitHealthChangeArgs.ChangeType type, int amount, ActorData target, ActorData caster, bool fromCharacterSpecificAbility)
		{
			this.m_type = type;
			this.m_amount = amount;
			this.m_target = target;
			this.m_caster = caster;
			this.m_fromCharacterSpecificAbility = fromCharacterSpecificAbility;
		}

		public enum ChangeType
		{
			Damage,
			Healing,
			Absorb
		}
	}

	public class ActorKnockback : GameEventManager.GameEventArgs
	{
		public ActorData m_target;
	}

	internal class TheatricsAbilityHighlightStartArgs : GameEventManager.GameEventArgs
	{
		internal HashSet<ActorData> m_casters = new HashSet<ActorData>();

		internal HashSet<ActorData> m_targets = new HashSet<ActorData>();
	}

	internal class NormalMovementStartAgs : GameEventManager.GameEventArgs
	{
		internal List<ActorData> m_actorsBeingHitMidMovement = new List<ActorData>();
	}

	public class CharacterHealBuffArgs : GameEventManager.GameEventArgs
	{
		public ActorData targetCharacter;

		public ActorData casterActor;

		public bool healed;
	}

	public class MatchEndedArgs : GameEventManager.GameEventArgs
	{
		public GameResult result;
	}

	public class MatchObjectiveEventArgs : GameEventManager.GameEventArgs
	{
		public GameEventManager.MatchObjectiveEventArgs.ObjectiveType objective;

		public Team team;

		public ActorData activatingActor;

		public ControlPoint controlPoint;

		public enum ObjectiveType
		{
			CoinCollected,
			FlagPickedUp_Client,
			FlagTurnedIn_Client,
			ControlPointCaptured,
			ObjectivePointsGained,
			CasePickedUp_Client
		}
	}

	public class ActorPingEventArgs : GameEventManager.GameEventArgs
	{
		public ActorData byActor;

		public ActorController.PingType pingType;
	}

	public class AbilityUseArgs : GameEventManager.GameEventArgs
	{
		public ActorData userActor;

		public Ability ability;
	}

	public class PowerUpActivatedArgs : GameEventManager.GameEventArgs
	{
		public ActorData byActor;

		public PowerUp powerUp;
	}

	public class CardUsedArgs : GameEventManager.GameEventArgs
	{
		public ActorData userActor;
	}

	public class QueryAreaArgs : GameEventManager.GameEventArgs
	{
		public ActorData characterActor;

		public QueryArea area;
	}

	public class WanderStateArgs : GameEventManager.GameEventArgs
	{
		public ActorData characterActor;

		public float totalLengthTravelled;

		public float pathLength;

		public BoardSquare destinationSquare;

		public int turnsWandering;
	}

	public class PatrolPointArgs : GameEventManager.GameEventArgs
	{
		public ActorData characterActor;

		public WayPoint patrolPoint;

		public int patrolPointIndex;

		public GameEventManager.PatrolPointArgs.WhatHappenedType whatHappened;

		public PatrolPath patrolPath;

		public bool destinationWasOccupied;

		public PatrolPointArgs(GameEventManager.PatrolPointArgs.WhatHappenedType wht, ActorData ad, WayPoint pp, int index, PatrolPath inPath, bool dwo)
		{
			this.whatHappened = wht;
			this.characterActor = ad;
			this.patrolPoint = pp;
			this.patrolPointIndex = index;
			this.patrolPath = inPath;
			this.destinationWasOccupied = dwo;
		}

		public enum WhatHappenedType
		{
			PointReached,
			MovingToNextPoint
		}
	}

	public class ScriptCommunicationArgs : GameEventManager.GameEventArgs
	{
		public Transition TransistionMessage;

		public NPCBrain NextBrain;

		public bool popBrain;
	}

	public class ReplaceVFXPrefab : GameEventManager.GameEventArgs
	{
		public Transform vfxRoot;

		public CharacterResourceLink characterResourceLink;

		public CharacterVisualInfo characterVisualInfo;

		public CharacterAbilityVfxSwapInfo characterAbilityVfxSwapInfo;
	}

	public class ReconnectReplayStateChangedArgs : GameEventManager.GameEventArgs
	{
		public bool m_newReconnectReplayState;

		public ReconnectReplayStateChangedArgs(bool newReconnectReplayState)
		{
			this.m_newReconnectReplayState = newReconnectReplayState;
		}
	}

	public class TheatricsAbilityAnimationStartArgs : GameEventManager.GameEventArgs
	{
		public bool lastInPhase;
	}
}
