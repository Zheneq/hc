using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class SinglePlayerManager : NetworkBehaviour
{
	private static string m_uniqueNetworkHash = "FFFFFF24601";

	private static SinglePlayerManager s_instance;

	[SyncVar(hook = "HookSetCurrentScriptIndex")]
	private int m_currentScriptIndex;

	[SyncVar(hook = "HookSetCanEndTurn")]
	private bool m_canEndTurn = true;

	private bool m_clientCanEndTurn = true;

	private GameObject m_advanceDestinationsHighlight;

	private bool m_pausedTimer;

	private bool m_decisionTimerForceOff;

	private bool m_lockInCancelButtonForceOff;

	private bool m_lockinPhaseDisplayForceOff;

	private bool m_lockinPhaseTextForceOff;

	private bool m_lockinPhaseColorForceOff;

	private bool m_notificationPanelForceOff;

	private bool[] m_teamPlayerIconForceOff = new bool[5];

	private bool[] m_enemyPlayerIconForceOff = new bool[5];

	[HideInInspector]
	private bool m_errorTriggered;

	private int m_lastTutorialTextState = -1;

	private int m_lastTutorialCameraState = -1;

	private static int kRpcRpcPlayScriptedChat = 0x34B13DB0;

	static SinglePlayerManager()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(SinglePlayerManager), SinglePlayerManager.kRpcRpcPlayScriptedChat, new NetworkBehaviour.CmdDelegate(SinglePlayerManager.InvokeRpcRpcPlayScriptedChat));
		NetworkCRC.RegisterBehaviour("SinglePlayerManager", 0);
	}

	private static GameObject OnSpawn(Vector3 position, NetworkHash128 assetId)
	{
		GameObject gameObject = new GameObject("SinglePlayerManagerGameObject");
		SinglePlayerManager singlePlayerManager = gameObject.AddComponent<SinglePlayerManager>();
		if (!SinglePlayerManager.s_instance)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnSpawn(Vector3, NetworkHash128)).MethodHandle;
			}
			SinglePlayerManager.s_instance = singlePlayerManager;
		}
		return gameObject;
	}

	private static void OnDespawn(GameObject spawned)
	{
		if (SinglePlayerManager.s_instance == spawned)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnDespawn(GameObject)).MethodHandle;
			}
			SinglePlayerManager.s_instance = null;
		}
	}

	public static void RegisterSpawnHandler()
	{
		NetworkHash128 assetId = NetworkHash128.Parse(SinglePlayerManager.m_uniqueNetworkHash);
		ClientScene.RegisterSpawnHandler(assetId, new SpawnDelegate(SinglePlayerManager.OnSpawn), new UnSpawnDelegate(SinglePlayerManager.OnDespawn));
	}

	public static void UnregisterSpawnHandler()
	{
		NetworkHash128 assetId = NetworkHash128.Parse(SinglePlayerManager.m_uniqueNetworkHash);
		ClientScene.UnregisterSpawnHandler(assetId);
	}

	public static SinglePlayerManager Get()
	{
		if (SinglePlayerCoordinator.Get() == null)
		{
			return null;
		}
		return SinglePlayerManager.s_instance;
	}

	public void SetScriptIndex(int newIndex)
	{
		while (newIndex > this.m_currentScriptIndex)
		{
			SinglePlayerState currentState = this.GetCurrentState();
			if (currentState != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.SetScriptIndex(int)).MethodHandle;
				}
				currentState.m_markedForAdvanceState = false;
				currentState.OnExitingState();
			}
			this.m_errorTriggered = false;
			this.Networkm_currentScriptIndex = newIndex;
			SinglePlayerState currentState2 = this.GetCurrentState();
			if (currentState2 != null)
			{
				currentState2.OnEnteringState();
				bool hasTag = currentState2.GetHasTag(SinglePlayerState.SinglePlayerTag.PauseTimer);
				if (NetworkServer.active)
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
					if (hasTag != this.m_pausedTimer)
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
						this.m_pausedTimer = hasTag;
						GameFlowData.Get().SetPausedForSinglePlayer(this.m_pausedTimer);
					}
				}
			}
			ActorData localPlayer = this.GetLocalPlayer();
			if (localPlayer)
			{
				ActorMovement actorMovement = localPlayer.GetActorMovement();
				if (actorMovement)
				{
					actorMovement.UpdateSquaresCanMoveTo();
				}
			}
			this.UpdateDestinationHighlights();
			this.RecalcCanEndTurn();
			if (currentState2 != null)
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
				if (currentState2.m_markedForAdvanceState)
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
					newIndex++;
				}
			}
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
	}

	public int GetCurrentScriptIndex()
	{
		return this.m_currentScriptIndex;
	}

	private void HookSetCurrentScriptIndex(int value)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.HookSetCurrentScriptIndex(int)).MethodHandle;
			}
			this.SetScriptIndex(value);
		}
	}

	public void SetCanEndTurn(bool canEnd)
	{
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.SetCanEndTurn(bool)).MethodHandle;
			}
			this.Networkm_canEndTurn = canEnd;
		}
	}

	public bool GetCanEndTurnFlag()
	{
		if (NetworkServer.active)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.GetCanEndTurnFlag()).MethodHandle;
			}
			return this.m_canEndTurn;
		}
		return this.m_clientCanEndTurn;
	}

	private void HookSetCanEndTurn(bool value)
	{
		this.Networkm_canEndTurn = value;
	}

	public void SetDecisionTimerForceOff(bool val)
	{
		this.m_decisionTimerForceOff = val;
	}

	public bool GetDecisionTimerForceOff()
	{
		return this.m_decisionTimerForceOff;
	}

	public void SetLockInCancelButtonForceOff(bool val)
	{
		this.m_lockInCancelButtonForceOff = val;
	}

	public bool GetLockinPhaseDisplayForceOff()
	{
		return this.m_lockinPhaseDisplayForceOff;
	}

	public void SetLockinPhaseDisplayForceOff(bool val)
	{
		this.m_lockinPhaseDisplayForceOff = val;
	}

	public bool GetLockinPhaseTextForceOff()
	{
		return this.m_lockinPhaseTextForceOff;
	}

	public void SetLockinPhaseTextForceOff(bool val)
	{
		this.m_lockinPhaseTextForceOff = val;
	}

	public bool GetLockinPhaseColorForceOff()
	{
		return this.m_lockinPhaseColorForceOff;
	}

	public void SetLockinPhaseColorForceOff(bool val)
	{
		this.m_lockinPhaseColorForceOff = val;
	}

	public bool GetLockInCancelButtonForceOff()
	{
		return this.m_lockInCancelButtonForceOff;
	}

	public void SetNotificationPanelForceOff(bool val)
	{
		this.m_notificationPanelForceOff = val;
	}

	public bool GetNotificationPanelForceOff()
	{
		return this.m_notificationPanelForceOff;
	}

	public void SetTeamPlayerIconForceOff(int index, bool val)
	{
		this.m_teamPlayerIconForceOff[index] = val;
	}

	public bool GetTeamPlayerIconForceOff(int index)
	{
		return this.m_teamPlayerIconForceOff[index];
	}

	public void SetEnemyPlayerIconForceOff(int index, bool val)
	{
		this.m_enemyPlayerIconForceOff[index] = val;
	}

	public bool GetEnemyPlayerIconForceOff(int index)
	{
		return this.m_enemyPlayerIconForceOff[index];
	}

	public static bool IsCancelDisabled()
	{
		if (SinglePlayerManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsCancelDisabled()).MethodHandle;
			}
			if (SinglePlayerManager.Get().GetCurrentState() != null)
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
				if (SinglePlayerManager.Get().GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.DisableCancel))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void Awake()
	{
		this.Networkm_currentScriptIndex = -1;
		this.Networkm_canEndTurn = true;
		this.m_clientCanEndTurn = true;
	}

	private void OnDestroy()
	{
		SinglePlayerManager.s_instance = null;
	}

	private void Update()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		if (currentState != null && currentState.m_advanceAfterSeconds != 0f)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.Update()).MethodHandle;
			}
			if (currentState.m_startTime != 0f)
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
				if (currentState.GetDuration() > currentState.m_advanceAfterSeconds / Time.timeScale)
				{
					this.AdvanceScript();
				}
			}
		}
	}

	internal void OnDecisionEnd()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		if (currentState != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnDecisionEnd()).MethodHandle;
			}
			if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnDecisionEnd))
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
				this.AdvanceScript();
			}
		}
	}

	internal void OnResolutionEnd()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		if (currentState != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnResolutionEnd()).MethodHandle;
			}
			if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnResolutionEnd))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				this.AdvanceScript();
			}
		}
	}

	public SinglePlayerState GetCurrentState()
	{
		return this.GetState(this.m_currentScriptIndex);
	}

	private SinglePlayerState GetState(int scriptIndex)
	{
		if (SinglePlayerCoordinator.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.GetState(int)).MethodHandle;
			}
			SinglePlayerState[] script = SinglePlayerCoordinator.Get().m_script;
			if (scriptIndex >= 0)
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
				if (scriptIndex < script.Length)
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
					return script[scriptIndex];
				}
			}
		}
		return null;
	}

	public static bool CanEndTurn(ActorData actor)
	{
		if (SinglePlayerManager.s_instance)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.CanEndTurn(ActorData)).MethodHandle;
			}
			if (actor != null)
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
				if (actor.GetIsHumanControlled())
				{
					return SinglePlayerManager.s_instance.GetCanEndTurnFlag();
				}
			}
		}
		return true;
	}

	public bool EnableChatter()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		bool result;
		if (currentState != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.EnableChatter()).MethodHandle;
			}
			result = currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableChatter);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool EnableAutoQueuedAbilitiesForNPCs()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		bool result;
		if (currentState != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.EnableAutoQueuedAbilitiesForNPCs()).MethodHandle;
			}
			result = currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableAutoQueuedAbilitiesForNpcs);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool EnableHiddenMovementText()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		return currentState == null || currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableHiddenMovementText);
	}

	public bool EnableBrush()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		return currentState == null || currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableBrush);
	}

	public bool EnableCooldownIndicators()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		return currentState == null || currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableCooldownIndicators);
	}

	public bool DisableAdvanceTurn()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		return currentState == null || currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.DisableAdvanceTurn);
	}

	public void OnTurnTick()
	{
	}

	public void \u001D()
	{
		this.AdvanceScript();
		UITutorialFullscreenPanel.Get().ClearAllPanels();
		UITutorialPanel.Get().ClearAll();
	}

	private void AdvanceScript()
	{
		this.SetScriptIndex(this.m_currentScriptIndex + 1);
	}

	public ActorData GetLocalPlayer()
	{
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData.GetIsHumanControlled())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.GetLocalPlayer()).MethodHandle;
					}
					return actorData;
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
		}
		return null;
	}

	public void OnActorDeath(ActorData actor)
	{
		bool flag = false;
		SinglePlayerState currentState = this.GetCurrentState();
		if (currentState != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnActorDeath(ActorData)).MethodHandle;
			}
			ActorData localPlayer = this.GetLocalPlayer();
			bool flag2;
			switch (currentState.m_advanceScriptIfActorDies)
			{
			case SinglePlayerState.ActorDiesTrigger.Never:
				flag2 = false;
				break;
			case SinglePlayerState.ActorDiesTrigger.AnyActor:
				flag2 = true;
				break;
			case SinglePlayerState.ActorDiesTrigger.SpawnedNPCs:
				flag2 = NPCCoordinator.IsSpawnedNPC(actor);
				break;
			case SinglePlayerState.ActorDiesTrigger.Players:
				flag2 = GameplayUtils.IsPlayerControlled(actor);
				break;
			case SinglePlayerState.ActorDiesTrigger.ClientActor:
			{
				bool flag3;
				if (!(localPlayer == null))
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
					flag3 = (localPlayer == actor);
				}
				else
				{
					flag3 = true;
				}
				flag2 = flag3;
				break;
			}
			case SinglePlayerState.ActorDiesTrigger.ClientAlly:
			{
				bool flag4;
				if (!(localPlayer == null))
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
					flag4 = (localPlayer.GetTeam() == actor.GetTeam());
				}
				else
				{
					flag4 = true;
				}
				flag2 = flag4;
				break;
			}
			case SinglePlayerState.ActorDiesTrigger.ClientEnemy:
			{
				bool flag5;
				if (!(localPlayer == null))
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
					flag5 = (localPlayer.GetTeam() != actor.GetTeam());
				}
				else
				{
					flag5 = true;
				}
				flag2 = flag5;
				break;
			}
			default:
				flag2 = false;
				break;
			}
			if (flag2)
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
				currentState.m_actorDeaths++;
				if (currentState.m_actorDeaths >= currentState.m_advanceScriptIfActorDiesCount)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.AdvanceScript();
		}
	}

	public void OnActorMoveEntered(ActorData actor)
	{
		SinglePlayerState currentState = this.GetCurrentState();
		ActorData localPlayer = this.GetLocalPlayer();
		if (localPlayer == actor && currentState != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnActorMoveEntered(ActorData)).MethodHandle;
			}
			if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnMoveEntered))
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
				this.AdvanceScript();
			}
		}
	}

	public void OnActorAbilitySelected(ActorData actor)
	{
		SinglePlayerState currentState = this.GetCurrentState();
		ActorData localPlayer = this.GetLocalPlayer();
		if (localPlayer == actor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnActorAbilitySelected(ActorData)).MethodHandle;
			}
			if (currentState != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnAbilitySelected))
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
					this.AdvanceScript();
				}
			}
		}
	}

	public void OnActorAbilityTargeted(ActorData actor)
	{
		SinglePlayerState currentState = this.GetCurrentState();
		ActorData localPlayer = this.GetLocalPlayer();
		if (localPlayer == actor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnActorAbilityTargeted(ActorData)).MethodHandle;
			}
			if (currentState != null)
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
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnAbilityTargeted))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.AdvanceScript();
				}
			}
		}
	}

	public void OnActorLockInEntered(ActorData actor)
	{
		SinglePlayerState currentState = this.GetCurrentState();
		ActorData localPlayer = this.GetLocalPlayer();
		if (localPlayer == actor)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnActorLockInEntered(ActorData)).MethodHandle;
			}
			if (currentState != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnLockInEntered))
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
					this.AdvanceScript();
				}
			}
		}
	}

	public void OnTutorialQueueEmpty()
	{
		if (NetworkServer.active)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnTutorialQueueEmpty()).MethodHandle;
			}
			SinglePlayerState currentState = this.GetCurrentState();
			if (currentState != null)
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
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnTutorialQueueEmpty))
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
					this.AdvanceScript();
				}
			}
		}
		else
		{
			ActorData localPlayer = this.GetLocalPlayer();
			if (localPlayer != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				PlayerData component = localPlayer.GetComponent<PlayerData>();
				if (component != null)
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
					component.CallCmdTutorialQueueEmpty();
				}
			}
		}
	}

	private void UpdateDestinationHighlights()
	{
		if (this.m_advanceDestinationsHighlight != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.UpdateDestinationHighlights()).MethodHandle;
			}
			UnityEngine.Object.DestroyImmediate(this.m_advanceDestinationsHighlight);
			this.m_advanceDestinationsHighlight = null;
		}
		if (this.GetCurrentState() != null)
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
			HashSet<BoardSquare> advanceDestinations = this.GetCurrentState().GetAdvanceDestinations();
			if (advanceDestinations != null)
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
				if (advanceDestinations.Count > 0)
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
					this.m_advanceDestinationsHighlight = HighlightUtils.Get().CreateBoundaryHighlight(advanceDestinations, Color.white, false, null, false);
					if (this.m_advanceDestinationsHighlight)
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
						this.m_advanceDestinationsHighlight.AddComponent<HighlightParent>();
					}
				}
			}
		}
	}

	public void OnActorMovementChanged(ActorData actor)
	{
		if (actor == this.GetLocalPlayer())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnActorMovementChanged(ActorData)).MethodHandle;
			}
			this.m_errorTriggered = false;
			this.RecalcCanEndTurn();
			if (!this.GetCanEndTurnFlag())
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
				if (!this.IsMovementAllowed())
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
					if (SinglePlayerCoordinator.Get() != null)
					{
						foreach (ActivatableObject activatableObject in SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath)
						{
							if (activatableObject == null)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								Log.Error("Null activation-object in Activations On Forbidden Path.", new object[0]);
							}
							else if (activatableObject.m_sceneObject == null)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								Log.Error("Activation-object has null scene-object in Activations On Forbidden Path.", new object[0]);
							}
							else
							{
								activatableObject.SetIsActive(true);
							}
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
						if (actor.GetCurrentBoardSquare() != actor.MoveFromBoardSquare)
						{
							this.m_errorTriggered = true;
						}
					}
				}
			}
		}
	}

	public void OnEndTurnRequested(ActorData requestingActor)
	{
		this.RecalcCanEndTurn();
		if (requestingActor.GetIsHumanControlled())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnEndTurnRequested(ActorData)).MethodHandle;
			}
			if (SinglePlayerCoordinator.Get() != null)
			{
				foreach (ActivatableObject activatableObject in SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath)
				{
					if (activatableObject != null && activatableObject.m_sceneObject != null)
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
						activatableObject.SetIsActive(false);
					}
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
				foreach (ActivatableObject activatableObject2 in SinglePlayerCoordinator.Get().m_activationsOnFailedToShootAndMove)
				{
					if (activatableObject2 != null && activatableObject2.m_sceneObject != null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						activatableObject2.SetIsActive(false);
					}
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
				foreach (ActivatableObject activatableObject3 in SinglePlayerCoordinator.Get().m_activationsOnFailedToUseAllAbilities)
				{
					if (activatableObject3 != null)
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
						if (activatableObject3.m_sceneObject != null)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							activatableObject3.SetIsActive(false);
						}
					}
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
			}
			if (!this.GetCanEndTurnFlag())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.IsMovementAllowed())
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (SinglePlayerCoordinator.Get() != null)
					{
						foreach (ActivatableObject activatableObject4 in SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath)
						{
							if (activatableObject4 == null)
							{
								Log.Error("Null activation-object in Activations On Forbidden Path.", new object[0]);
							}
							else if (activatableObject4.m_sceneObject == null)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								Log.Error("Activation-object has null scene-object in Activations On Forbidden Path.", new object[0]);
							}
							else
							{
								activatableObject4.SetIsActive(true);
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
					}
				}
				else if (!this.IsShootAndMoveAllowed())
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (SinglePlayerCoordinator.Get() != null)
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
						foreach (ActivatableObject activatableObject5 in SinglePlayerCoordinator.Get().m_activationsOnFailedToShootAndMove)
						{
							if (activatableObject5 == null)
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
								Log.Error("Null activation-object in Activations On Failed To Shoot And Move.", new object[0]);
							}
							else if (activatableObject5.m_sceneObject == null)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								Log.Error("Activation-object has null scene-object in Activations On Failed To Shoot And Move.", new object[0]);
							}
							else
							{
								activatableObject5.SetIsActive(true);
							}
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				else if (!this.IsMultipleAbilitiesAllowed() && SinglePlayerCoordinator.Get() != null)
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
					foreach (ActivatableObject activatableObject6 in SinglePlayerCoordinator.Get().m_activationsOnFailedToUseAllAbilities)
					{
						if (activatableObject6 == null)
						{
							Log.Error("Null activation-object in Activations On Failed To Use All Abilities.", new object[0]);
						}
						else if (activatableObject6.m_sceneObject == null)
						{
							Log.Error("Activation-object has null scene-object in Activations On Failed To Use All Abilities.", new object[0]);
						}
						else
						{
							activatableObject6.SetIsActive(true);
						}
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
	}

	public static bool IsActionAllowed(ActorData caster, AbilityData.ActionType action)
	{
		if (SinglePlayerManager.s_instance == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsActionAllowed(ActorData, AbilityData.ActionType)).MethodHandle;
			}
			return true;
		}
		if (SinglePlayerManager.s_instance.GetCurrentState() == null)
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
			return true;
		}
		if (caster.GetIsHumanControlled())
		{
			bool result = false;
			SinglePlayerState currentState = SinglePlayerManager.s_instance.GetCurrentState();
			if (currentState.m_allowedAbilities.Length == 0)
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
				result = true;
			}
			else
			{
				foreach (int num in currentState.m_allowedAbilities)
				{
					if (num == (int)action)
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
						return true;
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
			}
			return result;
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		Ability abilityOfActionType = caster.GetAbilityData().GetAbilityOfActionType(action);
		if (abilityOfActionType == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			return true;
		}
		if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.AutoQueueIfValid))
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
			return SinglePlayerManager.Get().EnableAutoQueuedAbilitiesForNPCs();
		}
		return true;
	}

	public static bool IsDestinationAllowed(ActorData mover, BoardSquare square, bool settingWaypoints = true)
	{
		if (SinglePlayerManager.s_instance == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsDestinationAllowed(ActorData, BoardSquare, bool)).MethodHandle;
			}
			return true;
		}
		if (SinglePlayerManager.s_instance.GetCurrentState() == null)
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
			return true;
		}
		if (mover.SpawnerId != -1)
		{
			return true;
		}
		SinglePlayerState currentState = SinglePlayerManager.s_instance.GetCurrentState();
		bool flag = currentState.m_allowedDestinations.m_quads.Length == 0 || currentState.m_allowedDestinations.GetSquaresInRegion().Contains(square);
		bool flag2;
		if (!currentState.m_onlyAllowWaypointMovement)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			flag2 = true;
		}
		else
		{
			flag2 = settingWaypoints;
		}
		bool result;
		if (flag)
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
			result = flag2;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public static bool IsSquareForbidden(ActorData mover, BoardSquare square)
	{
		if (SinglePlayerManager.s_instance == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsSquareForbidden(ActorData, BoardSquare)).MethodHandle;
			}
			return false;
		}
		if (SinglePlayerManager.s_instance.GetCurrentState() == null)
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
			return false;
		}
		if (!mover.GetIsHumanControlled())
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
			return false;
		}
		bool result = false;
		if (SinglePlayerCoordinator.Get() != null)
		{
			if (SinglePlayerCoordinator.Get().m_forbiddenSquares.m_quads.Length == 0)
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
				result = false;
			}
			else
			{
				result = SinglePlayerCoordinator.Get().m_forbiddenSquares.GetSquaresInRegion().Contains(square);
			}
		}
		return result;
	}

	public void RecalcCanEndTurn()
	{
		bool flag = this.IsMovementAllowed();
		bool flag2 = this.IsShootAndMoveAllowed();
		bool flag3 = this.IsMultipleAbilitiesAllowed();
		bool flag4 = this.IsMoveToAdvanceScriptDestinationAllowed();
		bool flag5 = this.IsRequireDashOk();
		bool flag6 = !this.DisableAdvanceTurn();
		bool flag7;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.RecalcCanEndTurn()).MethodHandle;
			}
			if (flag2)
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
				if (flag3 && flag4)
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
					if (flag5)
					{
						flag7 = flag6;
						goto IL_75;
					}
				}
			}
		}
		flag7 = false;
		IL_75:
		bool flag8 = flag7;
		if (NetworkServer.active)
		{
			this.Networkm_canEndTurn = flag8;
		}
		else
		{
			this.m_clientCanEndTurn = flag8;
		}
	}

	private bool IsRequireDashOk()
	{
		bool result = true;
		if (SinglePlayerCoordinator.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsRequireDashOk()).MethodHandle;
			}
			if (this.GetCurrentState() != null)
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
				if (this.GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
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
					ActorData localPlayer = this.GetLocalPlayer();
					AbilityData abilityData = localPlayer.GetAbilityData();
					bool flag = abilityData.HasQueuedAbilities();
					if (flag)
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
						if (abilityData.GetQueuedAbilitiesAllowMovement())
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
							result = false;
						}
					}
					else
					{
						result = false;
					}
				}
			}
		}
		return result;
	}

	private bool IsMovementAllowed()
	{
		bool result = true;
		if (SinglePlayerCoordinator.Get() != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsMovementAllowed()).MethodHandle;
			}
			if (this.GetCurrentState() != null)
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
				SinglePlayerState currentState = this.GetCurrentState();
				if (currentState != null && currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
				{
					result = true;
				}
				else
				{
					if (currentState != null)
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
						if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.RequireChasing))
						{
							bool flag = false;
							ActorData localPlayer = this.GetLocalPlayer();
							if (!NetworkServer.active && localPlayer.GetComponent<LineData>() != null)
							{
								flag = localPlayer.GetComponent<LineData>().GetIsChasing();
							}
							if (!flag)
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
								result = false;
							}
							return result;
						}
					}
					if (SinglePlayerCoordinator.Get().m_forbiddenSquares.m_quads.Length == 0)
					{
						result = true;
					}
					else
					{
						ActorData localPlayer2 = this.GetLocalPlayer();
						bool flag2 = false;
						List<GridPos> list = null;
						if (!NetworkServer.active)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (localPlayer2.GetComponent<LineData>())
							{
								list = localPlayer2.GetComponent<LineData>().GetGridPosPath();
							}
						}
						if (list != null)
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
							result = true;
							if (!flag2)
							{
								using (List<GridPos>.Enumerator enumerator = list.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										GridPos gridPos = enumerator.Current;
										BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
										if (SinglePlayerCoordinator.Get().m_forbiddenSquares.GetSquaresInRegion().Contains(boardSquareSafe))
										{
											for (;;)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
											result = false;
											goto IL_1B0;
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
								}
							}
							IL_1B0:;
						}
						else
						{
							result = !SinglePlayerCoordinator.Get().m_forbiddenSquares.GetSquaresInRegion().Contains(localPlayer2.GetCurrentBoardSquare());
						}
					}
				}
			}
		}
		return result;
	}

	private bool IsMoveToAdvanceScriptDestinationAllowed()
	{
		bool result;
		if (this.GetCurrentState() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsMoveToAdvanceScriptDestinationAllowed()).MethodHandle;
			}
			result = true;
		}
		else if (!this.GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireMoveToAdvanceScriptDestination))
		{
			result = true;
		}
		else
		{
			BoardSquare item = null;
			ActorData localPlayer = this.GetLocalPlayer();
			if (!NetworkServer.active)
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
				List<GridPos> gridPosPath = localPlayer.GetComponent<LineData>().GetGridPosPath();
				if (gridPosPath != null)
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
					if (gridPosPath.Count > 0)
					{
						item = Board.Get().GetBoardSquareSafe(gridPosPath[gridPosPath.Count - 1]);
					}
				}
			}
			if (this.GetCurrentState().GetAdvanceDestinations() != null)
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
				if (this.GetCurrentState().GetAdvanceDestinations().Contains(item))
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
					return true;
				}
			}
			if (this.GetCurrentState().GetAdvanceDestinations() == null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Error("We have a single player state {0} with the tag RequireMoveToAdvanceScriptDestination and no advance destinations!  Error!  Tell Colin!", new object[]
				{
					this.GetCurrentScriptIndex()
				});
			}
			result = false;
		}
		return result;
	}

	private bool IsShootAndMoveAllowed()
	{
		bool result;
		if (this.GetCurrentState() == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsShootAndMoveAllowed()).MethodHandle;
			}
			result = true;
		}
		else if (!this.GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireShootAndMove))
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
			result = true;
		}
		else
		{
			ActorData localPlayer = this.GetLocalPlayer();
			bool flag = false;
			List<GridPos> list = null;
			if (!NetworkServer.active)
			{
				list = localPlayer.GetComponent<LineData>().GetGridPosPath();
				flag = localPlayer.GetComponent<LineData>().GetIsChasing();
			}
			bool flag2;
			if (list != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (list.Count > 1)
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
					flag2 = !flag;
					goto IL_A0;
				}
			}
			flag2 = false;
			IL_A0:
			bool flag3 = flag2;
			AbilityData abilityData = localPlayer.GetAbilityData();
			bool flag4 = abilityData.HasQueuedAbilities();
			result = (flag3 && flag4);
		}
		return result;
	}

	private bool IsMultipleAbilitiesAllowed()
	{
		bool result;
		if (this.GetCurrentState() == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsMultipleAbilitiesAllowed()).MethodHandle;
			}
			result = true;
		}
		else if (!this.GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireMaxPossibleAbilities))
		{
			result = true;
		}
		else if (this.GetCurrentState().m_allowedAbilities.Length == 0)
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
			Log.Warning("State " + this.m_currentScriptIndex + " has RequireMaxPossibleAbilities but no specified allowed abilities.  Ignoring RequireMaxPossibleAbilities...", new object[0]);
			result = true;
		}
		else
		{
			int num = this.GetCurrentState().m_allowedAbilities.Length;
			ActorData localPlayer = this.GetLocalPlayer();
			AbilityData abilityData = localPlayer.GetAbilityData();
			int numQueuedAbilities = abilityData.GetNumQueuedAbilities();
			result = (num == numQueuedAbilities);
		}
		return result;
	}

	public static bool IsAbilitysCurrentAimingAllowed(ActorData aimer)
	{
		if (SinglePlayerManager.s_instance == null)
		{
			return true;
		}
		if (SinglePlayerManager.s_instance.GetCurrentState() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.IsAbilitysCurrentAimingAllowed(ActorData)).MethodHandle;
			}
			return true;
		}
		if (!aimer.GetIsHumanControlled())
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
			return true;
		}
		AbilityData component = aimer.GetComponent<AbilityData>();
		Ability selectedAbility = component.GetSelectedAbility();
		bool flag;
		bool flag2;
		if (selectedAbility == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			flag = true;
			flag2 = true;
		}
		else
		{
			SinglePlayerState currentState = SinglePlayerManager.s_instance.GetCurrentState();
			if (currentState.m_minAbilityTargetsForAiming == 0)
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
				flag = true;
			}
			else
			{
				if (!(selectedAbility == null))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (selectedAbility.Targeter != null)
					{
						if (selectedAbility.Targeter.GetNumActorsInRange() >= currentState.m_minAbilityTargetsForAiming)
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
							flag = true;
							goto IL_117;
						}
						flag = false;
						goto IL_117;
					}
				}
				Log.Warning("Single Player state " + currentState.m_stateIndex + " cares about MinAbilityTargetsForAiming for a targeter-less ability.  Suggest RequireMaxPossibleAbilities instead.", new object[0]);
				flag = (currentState.m_minAbilityTargetsForAiming <= 1);
			}
			IL_117:
			if (currentState.m_allowedTargets.m_quads.Length == 0)
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
				flag2 = true;
			}
			else
			{
				ActorTurnSM component2 = aimer.GetComponent<ActorTurnSM>();
				int targetSelectionIndex = component2.GetTargetSelectionIndex();
				List<BoardSquare> squaresInRegion = currentState.m_allowedTargets.GetSquaresInRegion();
				Ability.TargetingParadigm targetingParadigm = selectedAbility.GetTargetingParadigm(targetSelectionIndex);
				BoardSquare boardSquare;
				if (targetingParadigm == Ability.TargetingParadigm.BoardSquare)
				{
					AbilityTarget abilityTarget = AbilityTarget.CreateAbilityTargetFromInterface();
					boardSquare = Board.Get().GetBoardSquareSafe(abilityTarget.GridPos);
				}
				else if (targetingParadigm == Ability.TargetingParadigm.Position)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					AbilityTarget abilityTarget2 = AbilityTarget.CreateAbilityTargetFromInterface();
					boardSquare = Board.Get().GetBoardSquare(abilityTarget2.FreePos);
				}
				else
				{
					boardSquare = null;
				}
				bool flag3;
				if (!(boardSquare == null))
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
					flag3 = squaresInRegion.Contains(boardSquare);
				}
				else
				{
					flag3 = true;
				}
				flag2 = flag3;
				if (flag2 && currentState.m_mustTargetNearCenter)
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
					AbilityTarget abilityTarget3 = AbilityTarget.CreateAbilityTargetFromInterface();
					Vector3 center = currentState.m_allowedTargets.GetCenter();
					float f = center.x - abilityTarget3.FreePos.x;
					float f2 = center.z - abilityTarget3.FreePos.z;
					float num = 0.45f * Board.Get().squareSize;
					if (Mathf.Abs(f) < num)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (Mathf.Abs(f2) < num)
						{
							goto IL_28E;
						}
					}
					flag2 = false;
				}
			}
		}
		IL_28E:
		bool result;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			result = flag2;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void UpdateRightAndLeftClickElements(GameObject rightClick, TextMeshProUGUI rightClickText, GameObject leftClick, TextMeshProUGUI leftClickText, GameObject shiftRightClick, TextMeshProUGUI shiftRightClickText)
	{
		SinglePlayerState currentState = this.GetCurrentState();
		if (!(rightClick == null) && !(rightClickText == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.UpdateRightAndLeftClickElements(GameObject, TextMeshProUGUI, GameObject, TextMeshProUGUI, GameObject, TextMeshProUGUI)).MethodHandle;
			}
			if (!(leftClick == null))
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
				if (!(leftClickText == null) && !(shiftRightClick == null) && !(shiftRightClickText == null))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (currentState != null)
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
						if (currentState.m_leftClickHighlight != null)
						{
							List<BoardSquare> squaresInRegion = currentState.m_leftClickHighlight.GetSquaresInRegion();
							if (squaresInRegion.Count > 0)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!leftClick.activeSelf)
								{
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									UIManager.SetGameObjectActive(leftClick, true, null);
								}
								Canvas componentInParent = leftClick.GetComponentInParent<Canvas>();
								RectTransform rectTransform = null;
								if (componentInParent != null)
								{
									rectTransform = (componentInParent.transform as RectTransform);
								}
								Vector3 position = new Vector3(squaresInRegion[0].worldX, 1.5f + currentState.m_leftClickHeight, squaresInRegion[0].worldY);
								Vector2 vector = Camera.main.WorldToViewportPoint(position);
								Vector2 anchoredPosition = new Vector2(vector.x * rectTransform.sizeDelta.x - rectTransform.sizeDelta.x * 0.5f, vector.y * rectTransform.sizeDelta.y - rectTransform.sizeDelta.y * 0.5f);
								(leftClick.transform as RectTransform).anchoredPosition = anchoredPosition;
								string leftClickText2 = currentState.GetLeftClickText();
								if (leftClickText2 != string.Empty)
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
									if (leftClickText.text != leftClickText2)
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
										leftClickText.text = leftClickText2;
									}
								}
								else if (leftClickText.text != StringUtil.TR("LeftClick", "Tutorial"))
								{
									leftClickText.text = StringUtil.TR("LeftClick", "Tutorial");
								}
							}
							else if (leftClick.activeSelf)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								UIManager.SetGameObjectActive(leftClick, false, null);
							}
							List<BoardSquare> squaresInRegion2 = currentState.m_rightClickHighlight.GetSquaresInRegion();
							if (squaresInRegion2.Count > 0)
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
								if (!rightClick.activeSelf)
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
									UIManager.SetGameObjectActive(rightClick, true, null);
								}
								Canvas componentInParent2 = rightClick.GetComponentInParent<Canvas>();
								RectTransform rectTransform2 = null;
								if (componentInParent2 != null)
								{
									rectTransform2 = (componentInParent2.transform as RectTransform);
								}
								Vector3 position2 = new Vector3(squaresInRegion2[0].worldX, 1.5f + currentState.m_rightClickHeight, squaresInRegion2[0].worldY);
								Vector2 vector2 = Camera.main.WorldToViewportPoint(position2);
								Vector2 anchoredPosition2 = new Vector2(vector2.x * rectTransform2.sizeDelta.x - rectTransform2.sizeDelta.x * 0.5f, vector2.y * rectTransform2.sizeDelta.y - rectTransform2.sizeDelta.y * 0.5f);
								(rightClick.transform as RectTransform).anchoredPosition = anchoredPosition2;
								string rightClickText2 = currentState.GetRightClickText();
								if (rightClickText2 != string.Empty)
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
									if (rightClickText.text != rightClickText2)
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										rightClickText.text = rightClickText2;
									}
								}
								else if (rightClickText.text != StringUtil.TR("RightClick", "Tutorial"))
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
									rightClickText.text = StringUtil.TR("RightClick", "Tutorial");
								}
							}
							bool flag;
							if (Options_UI.Get() != null)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (Options_UI.Get().GetShiftClickForMovementWaypoints())
								{
									flag = true;
									goto IL_442;
								}
							}
							flag = false;
							IL_442:
							bool flag2 = flag;
							List<BoardSquare> squaresInRegion3 = currentState.m_shiftRightClickHighlight.GetSquaresInRegion();
							if (squaresInRegion3.Count > 0)
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
								if (flag2)
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
									if (!shiftRightClick.activeSelf)
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
										UIManager.SetGameObjectActive(shiftRightClick, true, null);
									}
									Canvas componentInParent3 = shiftRightClick.GetComponentInParent<Canvas>();
									RectTransform rectTransform3 = null;
									if (componentInParent3 != null)
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
										rectTransform3 = (componentInParent3.transform as RectTransform);
									}
									Vector3 position3 = new Vector3(squaresInRegion3[0].worldX, 1.5f + currentState.m_shiftRightClickHeight, squaresInRegion3[0].worldY);
									Vector2 vector3 = Camera.main.WorldToViewportPoint(position3);
									Vector2 anchoredPosition3 = new Vector2(vector3.x * rectTransform3.sizeDelta.x - rectTransform3.sizeDelta.x * 0.5f, vector3.y * rectTransform3.sizeDelta.y - rectTransform3.sizeDelta.y * 0.5f);
									(shiftRightClick.transform as RectTransform).anchoredPosition = anchoredPosition3;
									string shiftRightClickText2 = currentState.GetShiftRightClickText();
									if (shiftRightClickText2 != string.Empty)
									{
										if (shiftRightClickText.text != shiftRightClickText2)
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
											shiftRightClickText.text = shiftRightClickText2;
										}
									}
									else if (shiftRightClickText.text != StringUtil.TR("RightClick", "Tutorial"))
									{
										shiftRightClickText.text = StringUtil.TR("RightClick", "Tutorial");
									}
								}
								else
								{
									if (!rightClick.activeSelf)
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
										UIManager.SetGameObjectActive(rightClick, true, null);
									}
									Canvas componentInParent4 = rightClick.GetComponentInParent<Canvas>();
									RectTransform rectTransform4 = null;
									if (componentInParent4 != null)
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
										rectTransform4 = (componentInParent4.transform as RectTransform);
									}
									Vector3 position4 = new Vector3(squaresInRegion3[0].worldX, 1.5f + currentState.m_rightClickHeight, squaresInRegion3[0].worldY);
									Vector2 vector4 = Camera.main.WorldToViewportPoint(position4);
									Vector2 anchoredPosition4 = new Vector2(vector4.x * rectTransform4.sizeDelta.x - rectTransform4.sizeDelta.x * 0.5f, vector4.y * rectTransform4.sizeDelta.y - rectTransform4.sizeDelta.y * 0.5f);
									(rightClick.transform as RectTransform).anchoredPosition = anchoredPosition4;
									string rightClickText3 = currentState.GetRightClickText();
									if (rightClickText3 != string.Empty)
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
										if (rightClickText.text != rightClickText3)
										{
											for (;;)
											{
												switch (2)
												{
												case 0:
													continue;
												}
												break;
											}
											rightClickText.text = rightClickText3;
										}
									}
									else if (rightClickText.text != StringUtil.TR("RightClick", "Tutorial"))
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										rightClickText.text = StringUtil.TR("RightClick", "Tutorial");
									}
								}
							}
							if (squaresInRegion2.Count == 0)
							{
								if (squaresInRegion3.Count != 0)
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
									if (!flag2)
									{
										goto IL_7FA;
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
								}
								if (rightClick.activeSelf)
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
									UIManager.SetGameObjectActive(rightClick, false, null);
								}
							}
							IL_7FA:
							if (squaresInRegion3.Count != 0)
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
								if (flag2)
								{
									return;
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
							if (shiftRightClick.activeSelf)
							{
								UIManager.SetGameObjectActive(shiftRightClick, false, null);
							}
							return;
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
					}
				}
			}
		}
	}

	public void UpdateTutorialError(GameObject panel, TextMeshProUGUI text)
	{
		if (!(panel == null))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.UpdateTutorialError(GameObject, TextMeshProUGUI)).MethodHandle;
			}
			if (!(text == null))
			{
				SinglePlayerState currentState = this.GetCurrentState();
				string text2;
				if (currentState != null)
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
					text2 = currentState.GetErrorStringOnForbiddenPath();
				}
				else
				{
					text2 = string.Empty;
				}
				string text3 = text2;
				if (this.m_errorTriggered)
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
					if (text3 == string.Empty)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					else
					{
						UIManager.SetGameObjectActive(panel, true, null);
						if (text.text != text3)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							text.text = text3;
							return;
						}
						return;
					}
				}
				UIManager.SetGameObjectActive(panel, false, null);
				return;
			}
		}
	}

	public void UpdateTutorialTextElements(GameObject panel, TextMeshProUGUI text, GameObject panel2, TextMeshProUGUI text2, GameObject panel3, TextMeshProUGUI text3, GameObject panelCameraMovement, TextMeshProUGUI textCameraMovement, GameObject panelCameraRotation, TextMeshProUGUI textCameraRotation)
	{
		SinglePlayerState currentState = this.GetCurrentState();
		if (currentState != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.UpdateTutorialTextElements(GameObject, TextMeshProUGUI, GameObject, TextMeshProUGUI, GameObject, TextMeshProUGUI, GameObject, TextMeshProUGUI, GameObject, TextMeshProUGUI)).MethodHandle;
			}
			if (!(panel == null))
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
				if (!(text == null) && !(panel2 == null))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(text2 == null))
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
						if (!(panel3 == null))
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
							if (!(text3 == null))
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
								if (!(panelCameraMovement == null))
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
									if (!(textCameraMovement == null) && !(panelCameraRotation == null))
									{
										if (textCameraRotation == null)
										{
											for (;;)
											{
												switch (2)
												{
												case 0:
													continue;
												}
												break;
											}
										}
										else
										{
											if (Camera.main == null)
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
												return;
											}
											bool flag = false;
											if (this.m_lastTutorialTextState != currentState.m_stateIndex)
											{
												for (;;)
												{
													switch (5)
													{
													case 0:
														continue;
													}
													break;
												}
												flag = true;
											}
											if (currentState.m_tutorialBoxText.m_location == null)
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
												UIManager.SetGameObjectActive(panel, false, null);
											}
											else
											{
												Canvas componentInParent = panel.GetComponentInParent<Canvas>();
												RectTransform rectTransform = null;
												if (componentInParent != null)
												{
													rectTransform = (componentInParent.transform as RectTransform);
												}
												if (flag)
												{
													this.m_lastTutorialTextState = currentState.m_stateIndex;
													text.text = currentState.GetTutorialBoxText();
													UIManager.SetGameObjectActive(panel, true, null);
												}
												Vector3 position = currentState.m_tutorialBoxText.m_location.transform.position;
												Vector2 vector = Camera.main.WorldToViewportPoint(position);
												Vector2 anchoredPosition = new Vector2(vector.x * rectTransform.sizeDelta.x - rectTransform.sizeDelta.x * 0.5f, vector.y * rectTransform.sizeDelta.y - rectTransform.sizeDelta.y * 0.5f);
												(panel.transform as RectTransform).anchoredPosition = anchoredPosition;
											}
											if (currentState.m_tutorialBoxText2.m_location == null)
											{
												for (;;)
												{
													switch (2)
													{
													case 0:
														continue;
													}
													break;
												}
												UIManager.SetGameObjectActive(panel2, false, null);
											}
											else
											{
												Canvas componentInParent2 = panel2.GetComponentInParent<Canvas>();
												RectTransform rectTransform2 = null;
												if (componentInParent2 != null)
												{
													rectTransform2 = (componentInParent2.transform as RectTransform);
												}
												if (flag)
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
													this.m_lastTutorialTextState = currentState.m_stateIndex;
													text2.text = currentState.GetTutorialBoxText2();
													UIManager.SetGameObjectActive(panel2, true, null);
												}
												Vector3 position2 = currentState.m_tutorialBoxText2.m_location.transform.position;
												Vector2 vector2 = Camera.main.WorldToViewportPoint(position2);
												Vector2 anchoredPosition2 = new Vector2(vector2.x * rectTransform2.sizeDelta.x - rectTransform2.sizeDelta.x * 0.5f, vector2.y * rectTransform2.sizeDelta.y - rectTransform2.sizeDelta.y * 0.5f);
												(panel2.transform as RectTransform).anchoredPosition = anchoredPosition2;
											}
											if (currentState.m_tutorialBoxText3.m_location == null)
											{
												for (;;)
												{
													switch (2)
													{
													case 0:
														continue;
													}
													break;
												}
												UIManager.SetGameObjectActive(panel3, false, null);
											}
											else
											{
												Canvas componentInParent3 = panel3.GetComponentInParent<Canvas>();
												RectTransform rectTransform3 = null;
												if (componentInParent3 != null)
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
													rectTransform3 = (componentInParent3.transform as RectTransform);
												}
												if (flag)
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
													this.m_lastTutorialTextState = currentState.m_stateIndex;
													text3.text = currentState.GetTutorialBoxText3();
													UIManager.SetGameObjectActive(panel3, true, null);
												}
												Vector3 position3 = currentState.m_tutorialBoxText3.m_location.transform.position;
												Vector2 vector3 = Camera.main.WorldToViewportPoint(position3);
												Vector2 anchoredPosition3 = new Vector2(vector3.x * rectTransform3.sizeDelta.x - rectTransform3.sizeDelta.x * 0.5f, vector3.y * rectTransform3.sizeDelta.y - rectTransform3.sizeDelta.y * 0.5f);
												(panel3.transform as RectTransform).anchoredPosition = anchoredPosition3;
											}
											if (currentState.m_tutorialCameraMovementText.m_location == null)
											{
												for (;;)
												{
													switch (5)
													{
													case 0:
														continue;
													}
													break;
												}
												UIManager.SetGameObjectActive(panelCameraMovement, false, null);
											}
											else
											{
												Canvas componentInParent4 = panel3.GetComponentInParent<Canvas>();
												RectTransform rectTransform4 = null;
												if (componentInParent4 != null)
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
													rectTransform4 = (componentInParent4.transform as RectTransform);
												}
												if (flag)
												{
													this.m_lastTutorialTextState = currentState.m_stateIndex;
													textCameraMovement.text = currentState.GetTutorialCameraMovementText();
													UIManager.SetGameObjectActive(panelCameraMovement, true, null);
												}
												Vector3 position4 = currentState.m_tutorialCameraMovementText.m_location.transform.position;
												Vector2 vector4 = Camera.main.WorldToViewportPoint(position4);
												Vector2 anchoredPosition4 = new Vector2(vector4.x * rectTransform4.sizeDelta.x - rectTransform4.sizeDelta.x * 0.5f, vector4.y * rectTransform4.sizeDelta.y - rectTransform4.sizeDelta.y * 0.5f);
												(panelCameraMovement.transform as RectTransform).anchoredPosition = anchoredPosition4;
											}
											if (currentState.m_tutorialCameraRotationText.m_location == null)
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
												UIManager.SetGameObjectActive(panelCameraRotation, false, null);
											}
											else
											{
												Canvas componentInParent5 = panel3.GetComponentInParent<Canvas>();
												RectTransform rectTransform5 = null;
												if (componentInParent5 != null)
												{
													rectTransform5 = (componentInParent5.transform as RectTransform);
												}
												if (flag)
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
													this.m_lastTutorialTextState = currentState.m_stateIndex;
													textCameraRotation.text = currentState.GetTutorialCameraRotationText();
													UIManager.SetGameObjectActive(panelCameraRotation, true, null);
												}
												Vector3 position5 = currentState.m_tutorialCameraRotationText.m_location.transform.position;
												Vector2 vector5 = Camera.main.WorldToViewportPoint(position5);
												Vector2 anchoredPosition5 = new Vector2(vector5.x * rectTransform5.sizeDelta.x - rectTransform5.sizeDelta.x * 0.5f, vector5.y * rectTransform5.sizeDelta.y - rectTransform5.sizeDelta.y * 0.5f);
												(panelCameraRotation.transform as RectTransform).anchoredPosition = anchoredPosition5;
											}
											return;
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public bool HasPendingCameraUpdate()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		bool flag = false;
		if (currentState != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.HasPendingCameraUpdate()).MethodHandle;
			}
			if (this.m_lastTutorialCameraState != currentState.m_stateIndex)
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.m_lastTutorialCameraState = currentState.m_stateIndex;
			if (currentState.m_cameraRotationTarget == null)
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
				flag = false;
			}
		}
		return flag;
	}

	public static void ResetUIActivations()
	{
		ActivatableUI activatableUI = new ActivatableUI();
		activatableUI.m_activation = ActivatableUI.ActivationAction.SetActive;
		activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.Taunt;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelBackground;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelCenterPiece;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus1;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus2;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus3;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus4;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus5;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.ObjectivePanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.NotificationPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.BuffList;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.LockInButtonTutorialTipImage;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.LockInButtonTutorialTipText;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.FadeOutPanel;
		activatableUI.Activate();
		activatableUI.m_activation = ActivatableUI.ActivationAction.ClearActive;
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow1;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow2;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow3;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow4;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip1;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip2;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip3;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip4;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.LockInButtonTutorialTip;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.CameraControlsTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.EnergyGlow;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.EnergyArrows;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.CombatPhaseTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.DashPhaseTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.PrepPhaseTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.FullScreenPrepPhaseTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.FullScreenDashPhaseTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.FullScreenCombatPhaseTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.CardButtonTutorialTip;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.CardButtonTutorialTip1;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.CardButtonTutorialTip2;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.StatusEffectTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.TeammateTargetingTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.EnergyAndUltimatesTutorialPanel;
		activatableUI.Activate();
		activatableUI.m_uiElement = ActivatableUI.UIElement.FadeOutPanel;
		activatableUI.Activate();
		UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar, false, null);
		UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText, false, null);
	}

	[ClientRpc]
	public void RpcPlayScriptedChat(SinglePlayerScriptedChat chatText)
	{
		UITutorialPanel.Get().QueueDialogue(chatText.m_text, chatText.m_audioEvent, chatText.m_displaySeconds, chatText.m_sender);
	}

	private void UNetVersion()
	{
	}

	public int Networkm_currentScriptIndex
	{
		get
		{
			return this.m_currentScriptIndex;
		}
		[param: In]
		set
		{
			uint dirtyBit = 1U;
			if (NetworkServer.localClientActive)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.set_Networkm_currentScriptIndex(int)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
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
					base.syncVarHookGuard = true;
					this.HookSetCurrentScriptIndex(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_currentScriptIndex, dirtyBit);
		}
	}

	public bool Networkm_canEndTurn
	{
		get
		{
			return this.m_canEndTurn;
		}
		[param: In]
		set
		{
			uint dirtyBit = 2U;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.set_Networkm_canEndTurn(bool)).MethodHandle;
				}
				base.syncVarHookGuard = true;
				this.HookSetCanEndTurn(value);
				base.syncVarHookGuard = false;
			}
			base.SetSyncVar<bool>(value, ref this.m_canEndTurn, dirtyBit);
		}
	}

	protected static void InvokeRpcRpcPlayScriptedChat(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.InvokeRpcRpcPlayScriptedChat(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcPlayScriptedChat called on server.");
			return;
		}
		((SinglePlayerManager)obj).RpcPlayScriptedChat(GeneratedNetworkCode._ReadSinglePlayerScriptedChat_None(reader));
	}

	public void CallRpcPlayScriptedChat(SinglePlayerScriptedChat chatText)
	{
		if (!NetworkServer.active)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.CallRpcPlayScriptedChat(SinglePlayerScriptedChat)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcPlayScriptedChat called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)SinglePlayerManager.kRpcRpcPlayScriptedChat);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		GeneratedNetworkCode._WriteSinglePlayerScriptedChat_None(networkWriter, chatText);
		this.SendRPCInternal(networkWriter, 0, "RpcPlayScriptedChat");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_currentScriptIndex);
			writer.Write(this.m_canEndTurn);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnSerialize(NetworkWriter, bool)).MethodHandle;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_currentScriptIndex);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_canEndTurn);
		}
		if (!flag)
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerManager.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_currentScriptIndex = (int)reader.ReadPackedUInt32();
			this.m_canEndTurn = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.HookSetCurrentScriptIndex((int)reader.ReadPackedUInt32());
		}
		if ((num & 2) != 0)
		{
			this.HookSetCanEndTurn(reader.ReadBoolean());
		}
	}
}
