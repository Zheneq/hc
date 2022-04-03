using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
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

	public class ActivationInfo : GameEventArgs
	{
		public AbilityData.ActionType actionType;

		public bool active;
	}

	public class CharacterDeathEventArgs : GameEventArgs
	{
		public ActorData deadCharacter;
	}

	public class CharacterRagdollHitEventArgs : GameEventArgs
	{
		public ActorData m_ragdollingActor;

		public ActorData m_triggeringActor;
	}

	public class CharacterRespawnEventArgs : GameEventArgs
	{
		public ActorData respawningCharacter;
	}

	public class ActorHitHealthChangeArgs : GameEventArgs
	{
		public enum ChangeType
		{
			Damage,
			Healing,
			Absorb
		}

		public ChangeType m_type;

		public int m_amount;

		public ActorData m_target;

		public ActorData m_caster;

		public bool m_fromCharacterSpecificAbility;

		public ActorHitHealthChangeArgs(ChangeType type, int amount, ActorData target, ActorData caster, bool fromCharacterSpecificAbility)
		{
			m_type = type;
			m_amount = amount;
			m_target = target;
			m_caster = caster;
			m_fromCharacterSpecificAbility = fromCharacterSpecificAbility;
		}
	}

	public class ActorKnockback : GameEventArgs
	{
		public ActorData m_target;
	}

	internal class TheatricsAbilityHighlightStartArgs : GameEventArgs
	{
		internal HashSet<ActorData> m_casters = new HashSet<ActorData>();

		internal HashSet<ActorData> m_targets = new HashSet<ActorData>();
	}

	internal class NormalMovementStartAgs : GameEventArgs
	{
		internal List<ActorData> m_actorsBeingHitMidMovement = new List<ActorData>();
	}

	public class CharacterHealBuffArgs : GameEventArgs
	{
		public ActorData targetCharacter;

		public ActorData casterActor;

		public bool healed;
	}

	public class MatchEndedArgs : GameEventArgs
	{
		public GameResult result;
	}

	public class MatchObjectiveEventArgs : GameEventArgs
	{
		public enum ObjectiveType
		{
			CoinCollected,
			FlagPickedUp_Client,
			FlagTurnedIn_Client,
			ControlPointCaptured,
			ObjectivePointsGained,
			CasePickedUp_Client
		}

		public ObjectiveType objective;

		public Team team;

		public ActorData activatingActor;

		public ControlPoint controlPoint;
	}

	public class ActorPingEventArgs : GameEventArgs
	{
		public ActorData byActor;

		public ActorController.PingType pingType;
	}

	public class AbilityUseArgs : GameEventArgs
	{
		public ActorData userActor;

		public Ability ability;
	}

	public class PowerUpActivatedArgs : GameEventArgs
	{
		public ActorData byActor;

		public PowerUp powerUp;
	}

	public class CardUsedArgs : GameEventArgs
	{
		public ActorData userActor;
	}

	public class QueryAreaArgs : GameEventArgs
	{
		public ActorData characterActor;

		public QueryArea area;
	}

	public class WanderStateArgs : GameEventArgs
	{
		public ActorData characterActor;

		public float totalLengthTravelled;

		public float pathLength;

		public BoardSquare destinationSquare;

		public int turnsWandering;
	}

	public class PatrolPointArgs : GameEventArgs
	{
		public enum WhatHappenedType
		{
			PointReached,
			MovingToNextPoint
		}

		public ActorData characterActor;

		public WayPoint patrolPoint;

		public int patrolPointIndex;

		public WhatHappenedType whatHappened;

		public PatrolPath patrolPath;

		public bool destinationWasOccupied;

		public PatrolPointArgs(WhatHappenedType wht, ActorData ad, WayPoint pp, int index, PatrolPath inPath, bool dwo)
		{
			whatHappened = wht;
			characterActor = ad;
			patrolPoint = pp;
			patrolPointIndex = index;
			patrolPath = inPath;
			destinationWasOccupied = dwo;
		}
	}

	public class ScriptCommunicationArgs : GameEventArgs
	{
		public Transition TransistionMessage;

		public NPCBrain NextBrain;

		public bool popBrain;
	}

	public class ReplaceVFXPrefab : GameEventArgs
	{
		public Transform vfxRoot;

		public CharacterResourceLink characterResourceLink;

		public CharacterVisualInfo characterVisualInfo;

		public CharacterAbilityVfxSwapInfo characterAbilityVfxSwapInfo;
	}

	public class ReconnectReplayStateChangedArgs : GameEventArgs
	{
		public bool m_newReconnectReplayState;

		public ReconnectReplayStateChangedArgs(bool newReconnectReplayState)
		{
			m_newReconnectReplayState = newReconnectReplayState;
		}
	}

	public class TheatricsAbilityAnimationStartArgs : GameEventArgs
	{
		public bool lastInPhase;
	}

	private Dictionary<EventType, List<WeakReference>> m_listenersByEvent = new Dictionary<EventType, List<WeakReference>>();

	private List<ReferenceToRemove> m_referencesToRemove = new List<ReferenceToRemove>();

	private int m_firingEventsCount;

	private static GameEventManager s_instance;

	public static GameEventManager Get()
	{
		if (s_instance == null)
		{
			s_instance = new GameEventManager();
		}
		return s_instance;
	}

	~GameEventManager()
	{
		s_instance = null;
	}

	public void AddAllListenersTo(IGameEventListener whoTo)
	{
		PerformActionOnEvents(whoTo, delegate(IGameEventListener a, EventType b)
		{
			AddListener(a, b);
		});
	}

	public void RemoveAllListenersFrom(IGameEventListener whoTo)
	{
		PerformActionOnEvents(whoTo, delegate(IGameEventListener a, EventType b)
		{
			RemoveListener(a, b);
		});
	}

	private void PerformActionOnEvents(IGameEventListener whoTo, Action<IGameEventListener, EventType> action)
	{
		IEnumerator enumerator = Enum.GetValues(typeof(EventType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				EventType eventType = (EventType)enumerator.Current;
				if (eventType != 0)
				{
					action(whoTo, eventType);
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						disposable.Dispose();
						goto end_IL_0055;
					}
				}
			}
			end_IL_0055:;
		}
	}

	public void AddListener(IGameEventListener listener, EventType eventType)
	{
		WeakReference item = new WeakReference(listener, false);
		if (!m_listenersByEvent.TryGetValue(eventType, out List<WeakReference> value))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					List<WeakReference> list = new List<WeakReference>();
					list.Add(item);
					value = list;
					m_listenersByEvent.Add(eventType, value);
					return;
				}
				}
			}
		}
		if (value.Contains(item))
		{
			return;
		}
		while (true)
		{
			value.Add(item);
			return;
		}
	}

	public void RemoveListener(IGameEventListener listener, EventType eventType)
	{
		if (!m_listenersByEvent.TryGetValue(eventType, out List<WeakReference> value))
		{
			return;
		}
		int i = 0;
		for (int count = value.Count; i < count; i++)
		{
			WeakReference weakReference = value[i];
			if (!weakReference.IsAlive)
			{
				continue;
			}
			if (weakReference.Target != listener)
			{
				continue;
			}
			if (m_firingEventsCount == 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						value.Remove(weakReference);
						return;
					}
				}
			}
			m_referencesToRemove.Add(new ReferenceToRemove
			{
				referenceList = value,
				item = weakReference
			});
			return;
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public bool FireEvent(EventType eventType, GameEventArgs args)
	{
		Log.Info($"GameEventManager::FireEvent {eventType}");
		return FireEvent(eventType, args, null);
	}

	public bool FireEvent(EventType eventType, GameEventArgs args, IGameEventListener whoTo)
	{
		bool result = false;
		try
		{
			if (m_listenersByEvent.TryGetValue(eventType, out List<WeakReference> value))
			{
				m_firingEventsCount++;
				int i = 0;
				for (int count = value.Count; i < count; i++)
				{
					WeakReference weakReference = value[i];
					if (weakReference.IsAlive)
					{
						if (whoTo != null)
						{
							if (whoTo != weakReference.Target)
							{
								continue;
							}
						}
						IGameEventListener gameEventListener = (IGameEventListener)weakReference.Target;
						gameEventListener.OnGameEvent(eventType, args);
						result = true;
					}
					else
					{
						m_referencesToRemove.Add(new ReferenceToRemove
						{
							referenceList = value,
							item = weakReference
						});
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						m_firingEventsCount--;
						RemoveDeadReferences();
						return result;
					}
				}
			}
			return result;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			return result;
		}
	}

	private void RemoveDeadReferences()
	{
		if (m_firingEventsCount != 0)
		{
			return;
		}
		while (true)
		{
			int i = 0;
			for (int count = m_referencesToRemove.Count; i < count; i++)
			{
				ReferenceToRemove referenceToRemove = m_referencesToRemove[i];
				referenceToRemove.referenceList.Remove(referenceToRemove.item);
			}
			while (true)
			{
				m_referencesToRemove.Clear();
				return;
			}
		}
	}
}
