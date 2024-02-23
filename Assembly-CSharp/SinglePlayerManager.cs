using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class SinglePlayerManager : NetworkBehaviour
{
	private static string m_uniqueNetworkHash;

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

	private static int kRpcRpcPlayScriptedChat;

	public int Networkm_currentScriptIndex
	{
		get
		{
			return m_currentScriptIndex;
		}
		[param: In]
		set
		{
			ref int currentScriptIndex = ref m_currentScriptIndex;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetCurrentScriptIndex(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref currentScriptIndex, 1u);
		}
	}

	public bool Networkm_canEndTurn
	{
		get
		{
			return m_canEndTurn;
		}
		[param: In]
		set
		{
			ref bool canEndTurn = ref m_canEndTurn;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
			{
				base.syncVarHookGuard = true;
				HookSetCanEndTurn(value);
				base.syncVarHookGuard = false;
			}
			SetSyncVar(value, ref canEndTurn, 2u);
		}
	}

	static SinglePlayerManager()
	{
		m_uniqueNetworkHash = "FFFFFF24601";
		kRpcRpcPlayScriptedChat = 884030896;
		NetworkBehaviour.RegisterRpcDelegate(typeof(SinglePlayerManager), kRpcRpcPlayScriptedChat, InvokeRpcRpcPlayScriptedChat);
		NetworkCRC.RegisterBehaviour("SinglePlayerManager", 0);
	}

	private static GameObject OnSpawn(Vector3 position, NetworkHash128 assetId)
	{
		GameObject gameObject = new GameObject("SinglePlayerManagerGameObject");
		SinglePlayerManager singlePlayerManager = gameObject.AddComponent<SinglePlayerManager>();
		if (!s_instance)
		{
			s_instance = singlePlayerManager;
		}
		return gameObject;
	}

	private static void OnDespawn(GameObject spawned)
	{
		if (!(s_instance == spawned))
		{
			return;
		}
		while (true)
		{
			s_instance = null;
			return;
		}
	}

	public static void RegisterSpawnHandler()
	{
		NetworkHash128 assetId = NetworkHash128.Parse(m_uniqueNetworkHash);
		ClientScene.RegisterSpawnHandler(assetId, OnSpawn, OnDespawn);
	}

	public static void UnregisterSpawnHandler()
	{
		NetworkHash128 assetId = NetworkHash128.Parse(m_uniqueNetworkHash);
		ClientScene.UnregisterSpawnHandler(assetId);
	}

	public static SinglePlayerManager Get()
	{
		if (SinglePlayerCoordinator.Get() == null)
		{
			return null;
		}
		return s_instance;
	}

	public void SetScriptIndex(int newIndex)
	{
		while (newIndex > m_currentScriptIndex)
		{
			SinglePlayerState currentState = GetCurrentState();
			if (currentState != null)
			{
				currentState.m_markedForAdvanceState = false;
				currentState.OnExitingState();
			}
			m_errorTriggered = false;
			Networkm_currentScriptIndex = newIndex;
			SinglePlayerState currentState2 = GetCurrentState();
			if (currentState2 != null)
			{
				currentState2.OnEnteringState();
				bool hasTag = currentState2.GetHasTag(SinglePlayerState.SinglePlayerTag.PauseTimer);
				if (NetworkServer.active)
				{
					if (hasTag != m_pausedTimer)
					{
						m_pausedTimer = hasTag;
						GameFlowData.Get().SetPausedForSinglePlayer(m_pausedTimer);
					}
				}
			}
			ActorData localPlayer = GetLocalPlayer();
			if ((bool)localPlayer)
			{
				ActorMovement actorMovement = localPlayer.GetActorMovement();
				if ((bool)actorMovement)
				{
					actorMovement.UpdateSquaresCanMoveTo();
				}
			}
			UpdateDestinationHighlights();
			RecalcCanEndTurn();
			if (currentState2 == null)
			{
				continue;
			}
			if (currentState2.m_markedForAdvanceState)
			{
				newIndex++;
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public int GetCurrentScriptIndex()
	{
		return m_currentScriptIndex;
	}

	private void HookSetCurrentScriptIndex(int value)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			SetScriptIndex(value);
			return;
		}
	}

	public void SetCanEndTurn(bool canEnd)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			Networkm_canEndTurn = canEnd;
			return;
		}
	}

	public bool GetCanEndTurnFlag()
	{
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_canEndTurn;
				}
			}
		}
		return m_clientCanEndTurn;
	}

	private void HookSetCanEndTurn(bool value)
	{
		Networkm_canEndTurn = value;
	}

	public void SetDecisionTimerForceOff(bool val)
	{
		m_decisionTimerForceOff = val;
	}

	public bool GetDecisionTimerForceOff()
	{
		return m_decisionTimerForceOff;
	}

	public void SetLockInCancelButtonForceOff(bool val)
	{
		m_lockInCancelButtonForceOff = val;
	}

	public bool GetLockinPhaseDisplayForceOff()
	{
		return m_lockinPhaseDisplayForceOff;
	}

	public void SetLockinPhaseDisplayForceOff(bool val)
	{
		m_lockinPhaseDisplayForceOff = val;
	}

	public bool GetLockinPhaseTextForceOff()
	{
		return m_lockinPhaseTextForceOff;
	}

	public void SetLockinPhaseTextForceOff(bool val)
	{
		m_lockinPhaseTextForceOff = val;
	}

	public bool GetLockinPhaseColorForceOff()
	{
		return m_lockinPhaseColorForceOff;
	}

	public void SetLockinPhaseColorForceOff(bool val)
	{
		m_lockinPhaseColorForceOff = val;
	}

	public bool GetLockInCancelButtonForceOff()
	{
		return m_lockInCancelButtonForceOff;
	}

	public void SetNotificationPanelForceOff(bool val)
	{
		m_notificationPanelForceOff = val;
	}

	public bool GetNotificationPanelForceOff()
	{
		return m_notificationPanelForceOff;
	}

	public void SetTeamPlayerIconForceOff(int index, bool val)
	{
		m_teamPlayerIconForceOff[index] = val;
	}

	public bool GetTeamPlayerIconForceOff(int index)
	{
		return m_teamPlayerIconForceOff[index];
	}

	public void SetEnemyPlayerIconForceOff(int index, bool val)
	{
		m_enemyPlayerIconForceOff[index] = val;
	}

	public bool GetEnemyPlayerIconForceOff(int index)
	{
		return m_enemyPlayerIconForceOff[index];
	}

	public static bool IsCancelDisabled()
	{
		if (Get() != null)
		{
			if (Get().GetCurrentState() != null)
			{
				if (Get().GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.DisableCancel))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void Awake()
	{
		Networkm_currentScriptIndex = -1;
		Networkm_canEndTurn = true;
		m_clientCanEndTurn = true;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Update()
	{
		SinglePlayerState currentState = GetCurrentState();
		if (currentState == null || currentState.m_advanceAfterSeconds == 0f)
		{
			return;
		}
		while (true)
		{
			if (currentState.m_startTime == 0f)
			{
				return;
			}
			while (true)
			{
				if (currentState.GetDuration() > currentState.m_advanceAfterSeconds / Time.timeScale)
				{
					AdvanceScript();
				}
				return;
			}
		}
	}

	internal void OnDecisionEnd()
	{
		SinglePlayerState currentState = GetCurrentState();
		if (currentState == null)
		{
			return;
		}
		while (true)
		{
			if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnDecisionEnd))
			{
				while (true)
				{
					AdvanceScript();
					return;
				}
			}
			return;
		}
	}

	internal void OnResolutionEnd()
	{
		SinglePlayerState currentState = GetCurrentState();
		if (currentState == null)
		{
			return;
		}
		while (true)
		{
			if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnResolutionEnd))
			{
				while (true)
				{
					AdvanceScript();
					return;
				}
			}
			return;
		}
	}

	public SinglePlayerState GetCurrentState()
	{
		return GetState(m_currentScriptIndex);
	}

	private SinglePlayerState GetState(int scriptIndex)
	{
		if (SinglePlayerCoordinator.Get() != null)
		{
			SinglePlayerState[] script = SinglePlayerCoordinator.Get().m_script;
			if (scriptIndex >= 0)
			{
				if (scriptIndex < script.Length)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return script[scriptIndex];
						}
					}
				}
			}
		}
		return null;
	}

	public static bool CanEndTurn(ActorData actor)
	{
		if ((bool)s_instance)
		{
			if (actor != null)
			{
				if (actor.IsHumanControlled())
				{
					return s_instance.GetCanEndTurnFlag();
				}
			}
		}
		return true;
	}

	public bool EnableChatter()
	{
		SinglePlayerState currentState = GetCurrentState();
		int result;
		if (currentState != null)
		{
			result = (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableChatter) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool EnableAutoQueuedAbilitiesForNPCs()
	{
		SinglePlayerState currentState = GetCurrentState();
		int result;
		if (currentState != null)
		{
			result = (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableAutoQueuedAbilitiesForNpcs) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool EnableHiddenMovementText()
	{
		return GetCurrentState()?.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableHiddenMovementText) ?? true;
	}

	public bool EnableBrush()
	{
		return GetCurrentState()?.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableBrush) ?? true;
	}

	public bool EnableCooldownIndicators()
	{
		return GetCurrentState()?.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableCooldownIndicators) ?? true;
	}

	public bool DisableAdvanceTurn()
	{
		return GetCurrentState()?.GetHasTag(SinglePlayerState.SinglePlayerTag.DisableAdvanceTurn) ?? true;
	}

	public void OnTurnTick()
	{
	}

	public void _001D()
	{
		AdvanceScript();
		UITutorialFullscreenPanel.Get().ClearAllPanels();
		UITutorialPanel.Get().ClearAll();
	}

	private void AdvanceScript()
	{
		SetScriptIndex(m_currentScriptIndex + 1);
	}

	public ActorData GetLocalPlayer()
	{
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current.IsHumanControlled())
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
			}
		}
		return null;
	}

	public void OnActorDeath(ActorData actor)
	{
		bool flag = false;
		SinglePlayerState currentState = GetCurrentState();
		if (currentState != null)
		{
			ActorData localPlayer = GetLocalPlayer();
			bool flag2 = false;
			switch (currentState.m_advanceScriptIfActorDies)
			{
			case SinglePlayerState.ActorDiesTrigger.AnyActor:
				flag2 = true;
				break;
			case SinglePlayerState.ActorDiesTrigger.ClientActor:
			{
				int num3;
				if (!(localPlayer == null))
				{
					num3 = ((localPlayer == actor) ? 1 : 0);
				}
				else
				{
					num3 = 1;
				}
				flag2 = ((byte)num3 != 0);
				break;
			}
			case SinglePlayerState.ActorDiesTrigger.ClientAlly:
			{
				int num2;
				if (!(localPlayer == null))
				{
					num2 = ((localPlayer.GetTeam() == actor.GetTeam()) ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				flag2 = ((byte)num2 != 0);
				break;
			}
			case SinglePlayerState.ActorDiesTrigger.ClientEnemy:
			{
				int num;
				if (!(localPlayer == null))
				{
					num = ((localPlayer.GetTeam() != actor.GetTeam()) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				flag2 = ((byte)num != 0);
				break;
			}
			case SinglePlayerState.ActorDiesTrigger.Never:
				flag2 = false;
				break;
			case SinglePlayerState.ActorDiesTrigger.SpawnedNPCs:
				flag2 = NPCCoordinator.IsSpawnedNPC(actor);
				break;
			case SinglePlayerState.ActorDiesTrigger.Players:
				flag2 = GameplayUtils.IsPlayerControlled(actor);
				break;
			default:
				flag2 = false;
				break;
			}
			if (flag2)
			{
				currentState.m_actorDeaths++;
				if (currentState.m_actorDeaths >= currentState.m_advanceScriptIfActorDiesCount)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			AdvanceScript();
		}
	}

	public void OnActorMoveEntered(ActorData actor)
	{
		SinglePlayerState currentState = GetCurrentState();
		ActorData localPlayer = GetLocalPlayer();
		if (!(localPlayer == actor) || currentState == null)
		{
			return;
		}
		while (true)
		{
			if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnMoveEntered))
			{
				while (true)
				{
					AdvanceScript();
					return;
				}
			}
			return;
		}
	}

	public void OnActorAbilitySelected(ActorData actor)
	{
		SinglePlayerState currentState = GetCurrentState();
		ActorData localPlayer = GetLocalPlayer();
		if (!(localPlayer == actor))
		{
			return;
		}
		while (true)
		{
			if (currentState == null)
			{
				return;
			}
			while (true)
			{
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnAbilitySelected))
				{
					while (true)
					{
						AdvanceScript();
						return;
					}
				}
				return;
			}
		}
	}

	public void OnActorAbilityTargeted(ActorData actor)
	{
		SinglePlayerState currentState = GetCurrentState();
		ActorData localPlayer = GetLocalPlayer();
		if (!(localPlayer == actor))
		{
			return;
		}
		while (true)
		{
			if (currentState == null)
			{
				return;
			}
			while (true)
			{
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnAbilityTargeted))
				{
					while (true)
					{
						AdvanceScript();
						return;
					}
				}
				return;
			}
		}
	}

	public void OnActorLockInEntered(ActorData actor)
	{
		SinglePlayerState currentState = GetCurrentState();
		ActorData localPlayer = GetLocalPlayer();
		if (!(localPlayer == actor))
		{
			return;
		}
		while (true)
		{
			if (currentState == null)
			{
				return;
			}
			while (true)
			{
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnLockInEntered))
				{
					while (true)
					{
						AdvanceScript();
						return;
					}
				}
				return;
			}
		}
	}

	public void OnTutorialQueueEmpty()
	{
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					SinglePlayerState currentState = GetCurrentState();
					if (currentState != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnTutorialQueueEmpty))
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											AdvanceScript();
											return;
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		ActorData localPlayer = GetLocalPlayer();
		if (!(localPlayer != null))
		{
			return;
		}
		while (true)
		{
			PlayerData component = localPlayer.GetComponent<PlayerData>();
			if (component != null)
			{
				while (true)
				{
					component.CallCmdTutorialQueueEmpty();
					return;
				}
			}
			return;
		}
	}

	private void UpdateDestinationHighlights()
	{
		if (m_advanceDestinationsHighlight != null)
		{
			Object.DestroyImmediate(m_advanceDestinationsHighlight);
			m_advanceDestinationsHighlight = null;
		}
		if (GetCurrentState() == null)
		{
			return;
		}
		while (true)
		{
			HashSet<BoardSquare> advanceDestinations = GetCurrentState().GetAdvanceDestinations();
			if (advanceDestinations == null)
			{
				return;
			}
			while (true)
			{
				if (advanceDestinations.Count <= 0)
				{
					return;
				}
				while (true)
				{
					m_advanceDestinationsHighlight = HighlightUtils.Get().CreateBoundaryHighlight(advanceDestinations, Color.white);
					if ((bool)m_advanceDestinationsHighlight)
					{
						while (true)
						{
							m_advanceDestinationsHighlight.AddComponent<HighlightParent>();
							return;
						}
					}
					return;
				}
			}
		}
	}

	public void OnActorMovementChanged(ActorData actor)
	{
		if (!(actor == GetLocalPlayer()))
		{
			return;
		}
		while (true)
		{
			m_errorTriggered = false;
			RecalcCanEndTurn();
			if (GetCanEndTurnFlag())
			{
				return;
			}
			while (true)
			{
				if (IsMovementAllowed())
				{
					return;
				}
				while (true)
				{
					if (!(SinglePlayerCoordinator.Get() != null))
					{
						return;
					}
					ActivatableObject[] activationsOnForbiddenPath = SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath;
					foreach (ActivatableObject activatableObject in activationsOnForbiddenPath)
					{
						if (activatableObject == null)
						{
							Log.Error("Null activation-object in Activations On Forbidden Path.");
						}
						else if (activatableObject.m_sceneObject == null)
						{
							Log.Error("Activation-object has null scene-object in Activations On Forbidden Path.");
						}
						else
						{
							activatableObject.SetIsActive(true);
						}
					}
					while (true)
					{
						if (actor.GetCurrentBoardSquare() != actor.MoveFromBoardSquare)
						{
							m_errorTriggered = true;
						}
						return;
					}
				}
			}
		}
	}

	public void OnEndTurnRequested(ActorData requestingActor)
	{
		RecalcCanEndTurn();
		if (!requestingActor.IsHumanControlled())
		{
			return;
		}
		while (true)
		{
			if (SinglePlayerCoordinator.Get() != null)
			{
				ActivatableObject[] activationsOnForbiddenPath = SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath;
				foreach (ActivatableObject activatableObject in activationsOnForbiddenPath)
				{
					if (activatableObject != null && activatableObject.m_sceneObject != null)
					{
						activatableObject.SetIsActive(false);
					}
				}
				ActivatableObject[] activationsOnFailedToShootAndMove = SinglePlayerCoordinator.Get().m_activationsOnFailedToShootAndMove;
				foreach (ActivatableObject activatableObject2 in activationsOnFailedToShootAndMove)
				{
					if (activatableObject2 != null && activatableObject2.m_sceneObject != null)
					{
						activatableObject2.SetIsActive(false);
					}
				}
				ActivatableObject[] activationsOnFailedToUseAllAbilities = SinglePlayerCoordinator.Get().m_activationsOnFailedToUseAllAbilities;
				foreach (ActivatableObject activatableObject3 in activationsOnFailedToUseAllAbilities)
				{
					if (activatableObject3 == null)
					{
						continue;
					}
					if (activatableObject3.m_sceneObject != null)
					{
						activatableObject3.SetIsActive(false);
					}
				}
			}
			if (GetCanEndTurnFlag())
			{
				return;
			}
			while (true)
			{
				if (!IsMovementAllowed())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (SinglePlayerCoordinator.Get() != null)
							{
								ActivatableObject[] activationsOnForbiddenPath2 = SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath;
								foreach (ActivatableObject activatableObject4 in activationsOnForbiddenPath2)
								{
									if (activatableObject4 == null)
									{
										Log.Error("Null activation-object in Activations On Forbidden Path.");
									}
									else if (activatableObject4.m_sceneObject == null)
									{
										Log.Error("Activation-object has null scene-object in Activations On Forbidden Path.");
									}
									else
									{
										activatableObject4.SetIsActive(true);
									}
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
							return;
						}
					}
				}
				if (!IsShootAndMoveAllowed())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (SinglePlayerCoordinator.Get() != null)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
									{
										ActivatableObject[] activationsOnFailedToShootAndMove2 = SinglePlayerCoordinator.Get().m_activationsOnFailedToShootAndMove;
										foreach (ActivatableObject activatableObject5 in activationsOnFailedToShootAndMove2)
										{
											if (activatableObject5 == null)
											{
												Log.Error("Null activation-object in Activations On Failed To Shoot And Move.");
											}
											else if (activatableObject5.m_sceneObject == null)
											{
												Log.Error("Activation-object has null scene-object in Activations On Failed To Shoot And Move.");
											}
											else
											{
												activatableObject5.SetIsActive(true);
											}
										}
										while (true)
										{
											switch (5)
											{
											default:
												return;
											case 0:
												break;
											}
										}
									}
									}
								}
							}
							return;
						}
					}
				}
				if (IsMultipleAbilitiesAllowed() || !(SinglePlayerCoordinator.Get() != null))
				{
					return;
				}
				while (true)
				{
					ActivatableObject[] activationsOnFailedToUseAllAbilities2 = SinglePlayerCoordinator.Get().m_activationsOnFailedToUseAllAbilities;
					foreach (ActivatableObject activatableObject6 in activationsOnFailedToUseAllAbilities2)
					{
						if (activatableObject6 == null)
						{
							Log.Error("Null activation-object in Activations On Failed To Use All Abilities.");
						}
						else if (activatableObject6.m_sceneObject == null)
						{
							Log.Error("Activation-object has null scene-object in Activations On Failed To Use All Abilities.");
						}
						else
						{
							activatableObject6.SetIsActive(true);
						}
					}
					while (true)
					{
						switch (2)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
		}
	}

	public static bool IsActionAllowed(ActorData caster, AbilityData.ActionType action)
	{
		if (s_instance == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (s_instance.GetCurrentState() == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (!caster.IsHumanControlled())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					Ability abilityOfActionType = caster.GetAbilityData().GetAbilityOfActionType(action);
					if (abilityOfActionType == null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
					if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.AutoQueueIfValid))
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return Get().EnableAutoQueuedAbilitiesForNPCs();
							}
						}
					}
					return true;
				}
				}
			}
		}
		bool result = false;
		SinglePlayerState currentState = s_instance.GetCurrentState();
		if (currentState.m_allowedAbilities.Length == 0)
		{
			result = true;
		}
		else
		{
			int[] allowedAbilities = currentState.m_allowedAbilities;
			int num = 0;
			while (true)
			{
				if (num < allowedAbilities.Length)
				{
					int num2 = allowedAbilities[num];
					if (num2 == (int)action)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		return result;
	}

	public static bool IsDestinationAllowed(ActorData mover, BoardSquare square, bool settingWaypoints = true)
	{
		if (s_instance == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (s_instance.GetCurrentState() == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (mover.SpawnerId != -1)
		{
			return true;
		}
		SinglePlayerState currentState = s_instance.GetCurrentState();
		bool flag = currentState.m_allowedDestinations.m_quads.Length == 0 || currentState.m_allowedDestinations.GetSquaresInRegion().Contains(square);
		bool flag2;
		if (!currentState.m_onlyAllowWaypointMovement)
		{
			flag2 = true;
		}
		else
		{
			flag2 = settingWaypoints;
		}
		int result;
		if (flag)
		{
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public static bool IsSquareForbidden(ActorData mover, BoardSquare square)
	{
		if (s_instance == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (s_instance.GetCurrentState() == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!mover.IsHumanControlled())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		bool result = false;
		if (SinglePlayerCoordinator.Get() != null)
		{
			if (SinglePlayerCoordinator.Get().m_forbiddenSquares.m_quads.Length == 0)
			{
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
		bool flag = IsMovementAllowed();
		bool flag2 = IsShootAndMoveAllowed();
		bool flag3 = IsMultipleAbilitiesAllowed();
		bool flag4 = IsMoveToAdvanceScriptDestinationAllowed();
		bool flag5 = IsRequireDashOk();
		bool flag6 = !DisableAdvanceTurn();
		int num;
		if (flag)
		{
			if (flag2)
			{
				if (flag3 && flag4)
				{
					if (flag5)
					{
						num = (flag6 ? 1 : 0);
						goto IL_0075;
					}
				}
			}
		}
		num = 0;
		goto IL_0075;
		IL_0075:
		bool flag7 = (byte)num != 0;
		if (NetworkServer.active)
		{
			Networkm_canEndTurn = flag7;
		}
		else
		{
			m_clientCanEndTurn = flag7;
		}
	}

	private bool IsRequireDashOk()
	{
		bool result = true;
		if (SinglePlayerCoordinator.Get() != null)
		{
			if (GetCurrentState() != null)
			{
				if (GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
				{
					ActorData localPlayer = GetLocalPlayer();
					AbilityData abilityData = localPlayer.GetAbilityData();
					if (abilityData.HasQueuedAbilities())
					{
						if (abilityData.GetQueuedAbilitiesAllowMovement())
						{
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
			if (GetCurrentState() != null)
			{
				SinglePlayerState currentState = GetCurrentState();
				if (currentState != null && currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
				{
					result = true;
				}
				else
				{
					if (currentState != null)
					{
						if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.RequireChasing))
						{
							bool flag = false;
							ActorData localPlayer = GetLocalPlayer();
							if (!NetworkServer.active && localPlayer.GetComponent<LineData>() != null)
							{
								flag = localPlayer.GetComponent<LineData>().GetIsChasing();
							}
							if (!flag)
							{
								result = false;
							}
							goto IL_01d7;
						}
					}
					if (SinglePlayerCoordinator.Get().m_forbiddenSquares.m_quads.Length == 0)
					{
						result = true;
					}
					else
					{
						ActorData localPlayer2 = GetLocalPlayer();
						bool flag2 = false;
						List<GridPos> list = null;
						if (!NetworkServer.active)
						{
							if ((bool)localPlayer2.GetComponent<LineData>())
							{
								list = localPlayer2.GetComponent<LineData>().GetGridPosPath();
							}
						}
						if (list != null)
						{
							result = true;
							if (!flag2)
							{
								using (List<GridPos>.Enumerator enumerator = list.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										GridPos current = enumerator.Current;
										BoardSquare boardSquareSafe = Board.Get().GetSquare(current);
										if (SinglePlayerCoordinator.Get().m_forbiddenSquares.GetSquaresInRegion().Contains(boardSquareSafe))
										{
											while (true)
											{
												switch (5)
												{
												case 0:
													break;
												default:
													return false;
												}
											}
										}
									}
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											return result;
										}
									}
								}
							}
						}
						else
						{
							result = !SinglePlayerCoordinator.Get().m_forbiddenSquares.GetSquaresInRegion().Contains(localPlayer2.GetCurrentBoardSquare());
						}
					}
				}
			}
		}
		goto IL_01d7;
		IL_01d7:
		return result;
	}

	private bool IsMoveToAdvanceScriptDestinationAllowed()
	{
		if (GetCurrentState() == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (!GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireMoveToAdvanceScriptDestination))
		{
			return true;
		}
		BoardSquare item = null;
		ActorData localPlayer = GetLocalPlayer();
		if (!NetworkServer.active)
		{
			List<GridPos> gridPosPath = localPlayer.GetComponent<LineData>().GetGridPosPath();
			if (gridPosPath != null)
			{
				if (gridPosPath.Count > 0)
				{
					item = Board.Get().GetSquare(gridPosPath[gridPosPath.Count - 1]);
				}
			}
		}
		if (GetCurrentState().GetAdvanceDestinations() != null)
		{
			if (GetCurrentState().GetAdvanceDestinations().Contains(item))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		if (GetCurrentState().GetAdvanceDestinations() == null)
		{
			Log.Error("We have a single player state {0} with the tag RequireMoveToAdvanceScriptDestination and no advance destinations!  Error!  Tell Colin!", GetCurrentScriptIndex());
		}
		return false;
	}

	private bool IsShootAndMoveAllowed()
	{
		if (GetCurrentState() == null)
		{
			while (true)
			{
				return true;
			}
		}
		if (!GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireShootAndMove))
		{
			while (true)
			{
				return true;
			}
		}
		ActorData localPlayer = GetLocalPlayer();
		bool flag = false;
		List<GridPos> list = null;
		if (!NetworkServer.active)
		{
			list = localPlayer.GetComponent<LineData>().GetGridPosPath();
			flag = localPlayer.GetComponent<LineData>().GetIsChasing();
		}
		int num;
		if (list != null)
		{
			if (list.Count > 1)
			{
				num = ((!flag) ? 1 : 0);
				goto IL_00a0;
			}
		}
		num = 0;
		goto IL_00a0;
		IL_00a0:
		bool flag2 = (byte)num != 0;
		AbilityData abilityData = localPlayer.GetAbilityData();
		bool flag3 = abilityData.HasQueuedAbilities();
		return flag2 && flag3;
	}

	private bool IsMultipleAbilitiesAllowed()
	{
		if (GetCurrentState() == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (!GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireMaxPossibleAbilities))
		{
			return true;
		}
		if (GetCurrentState().m_allowedAbilities.Length == 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Log.Warning(new StringBuilder().Append("State ").Append(m_currentScriptIndex).Append(" has RequireMaxPossibleAbilities but no specified allowed abilities.  Ignoring RequireMaxPossibleAbilities...").ToString());
					return true;
				}
			}
		}
		int num = GetCurrentState().m_allowedAbilities.Length;
		ActorData localPlayer = GetLocalPlayer();
		AbilityData abilityData = localPlayer.GetAbilityData();
		int numQueuedAbilities = abilityData.GetNumQueuedAbilities();
		return num == numQueuedAbilities;
	}

	public static bool IsAbilitysCurrentAimingAllowed(ActorData aimer)
	{
		if (s_instance == null)
		{
			return true;
		}
		if (s_instance.GetCurrentState() == null)
		{
			while (true)
			{
				return true;
			}
		}
		if (!aimer.IsHumanControlled())
		{
			while (true)
			{
				return true;
			}
		}
		AbilityData component = aimer.GetComponent<AbilityData>();
		Ability selectedAbility = component.GetSelectedAbility();
		bool flag;
		bool flag2;
		if (selectedAbility == null)
		{
			flag = true;
			flag2 = true;
			goto IL_028e;
		}
		SinglePlayerState currentState = s_instance.GetCurrentState();
		if (currentState.m_minAbilityTargetsForAiming == 0)
		{
			flag = true;
		}
		else
		{
			if (!(selectedAbility == null))
			{
				if (selectedAbility.Targeter != null)
				{
					if (selectedAbility.Targeter.GetNumActorsInRange() >= currentState.m_minAbilityTargetsForAiming)
					{
						flag = true;
					}
					else
					{
						flag = false;
					}
					goto IL_0117;
				}
			}
			Log.Warning(new StringBuilder().Append("Single Player state ").Append(currentState.m_stateIndex).Append(" cares about MinAbilityTargetsForAiming for a targeter-less ability.  Suggest RequireMaxPossibleAbilities instead.").ToString());
			flag = (currentState.m_minAbilityTargetsForAiming <= 1);
		}
		goto IL_0117;
		IL_028e:
		int result;
		if (flag)
		{
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
		IL_0117:
		if (currentState.m_allowedTargets.m_quads.Length == 0)
		{
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
				boardSquare = Board.Get().GetSquare(abilityTarget.GridPos);
			}
			else if (targetingParadigm == Ability.TargetingParadigm.Position)
			{
				AbilityTarget abilityTarget2 = AbilityTarget.CreateAbilityTargetFromInterface();
				boardSquare = Board.Get().GetSquareFromVec3(abilityTarget2.FreePos);
			}
			else
			{
				boardSquare = null;
			}
			int num;
			if (!(boardSquare == null))
			{
				num = (squaresInRegion.Contains(boardSquare) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			flag2 = ((byte)num != 0);
			if (flag2 && currentState.m_mustTargetNearCenter)
			{
				AbilityTarget abilityTarget3 = AbilityTarget.CreateAbilityTargetFromInterface();
				Vector3 center = currentState.m_allowedTargets.GetCenter();
				float x = center.x;
				Vector3 freePos = abilityTarget3.FreePos;
				float f = x - freePos.x;
				float z = center.z;
				Vector3 freePos2 = abilityTarget3.FreePos;
				float f2 = z - freePos2.z;
				float num2 = 0.45f * Board.Get().squareSize;
				if (!(Mathf.Abs(f) >= num2))
				{
					if (!(Mathf.Abs(f2) >= num2))
					{
						goto IL_028e;
					}
				}
				flag2 = false;
			}
		}
		goto IL_028e;
	}

	public void UpdateRightAndLeftClickElements(GameObject rightClick, TextMeshProUGUI rightClickText, GameObject leftClick, TextMeshProUGUI leftClickText, GameObject shiftRightClick, TextMeshProUGUI shiftRightClickText)
	{
		SinglePlayerState currentState = GetCurrentState();
		if (rightClick == null || rightClickText == null)
		{
			return;
		}
		while (true)
		{
			if (leftClick == null)
			{
				return;
			}
			while (true)
			{
				if (leftClickText == null || shiftRightClick == null || shiftRightClickText == null)
				{
					return;
				}
				while (true)
				{
					if (currentState == null)
					{
						return;
					}
					while (true)
					{
						if (currentState.m_leftClickHighlight == null)
						{
							while (true)
							{
								switch (1)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
						List<BoardSquare> squaresInRegion = currentState.m_leftClickHighlight.GetSquaresInRegion();
						if (squaresInRegion.Count > 0)
						{
							if (!leftClick.activeSelf)
							{
								UIManager.SetGameObjectActive(leftClick, true);
							}
							Canvas componentInParent = leftClick.GetComponentInParent<Canvas>();
							RectTransform rectTransform = null;
							if (componentInParent != null)
							{
								rectTransform = (componentInParent.transform as RectTransform);
							}
							Vector3 position = new Vector3(squaresInRegion[0].worldX, 1.5f + currentState.m_leftClickHeight, squaresInRegion[0].worldY);
							Vector2 vector = Camera.main.WorldToViewportPoint(position);
							float x = vector.x;
							Vector2 sizeDelta = rectTransform.sizeDelta;
							float num = x * sizeDelta.x;
							Vector2 sizeDelta2 = rectTransform.sizeDelta;
							float x2 = num - sizeDelta2.x * 0.5f;
							float y = vector.y;
							Vector2 sizeDelta3 = rectTransform.sizeDelta;
							float num2 = y * sizeDelta3.y;
							Vector2 sizeDelta4 = rectTransform.sizeDelta;
							Vector2 anchoredPosition = new Vector2(x2, num2 - sizeDelta4.y * 0.5f);
							(leftClick.transform as RectTransform).anchoredPosition = anchoredPosition;
							string leftClickText2 = currentState.GetLeftClickText();
							if (leftClickText2 != string.Empty)
							{
								if (leftClickText.text != leftClickText2)
								{
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
							UIManager.SetGameObjectActive(leftClick, false);
						}
						List<BoardSquare> squaresInRegion2 = currentState.m_rightClickHighlight.GetSquaresInRegion();
						if (squaresInRegion2.Count > 0)
						{
							if (!rightClick.activeSelf)
							{
								UIManager.SetGameObjectActive(rightClick, true);
							}
							Canvas componentInParent2 = rightClick.GetComponentInParent<Canvas>();
							RectTransform rectTransform2 = null;
							if (componentInParent2 != null)
							{
								rectTransform2 = (componentInParent2.transform as RectTransform);
							}
							Vector3 position2 = new Vector3(squaresInRegion2[0].worldX, 1.5f + currentState.m_rightClickHeight, squaresInRegion2[0].worldY);
							Vector2 vector2 = Camera.main.WorldToViewportPoint(position2);
							float x3 = vector2.x;
							Vector2 sizeDelta5 = rectTransform2.sizeDelta;
							float num3 = x3 * sizeDelta5.x;
							Vector2 sizeDelta6 = rectTransform2.sizeDelta;
							float x4 = num3 - sizeDelta6.x * 0.5f;
							float y2 = vector2.y;
							Vector2 sizeDelta7 = rectTransform2.sizeDelta;
							float num4 = y2 * sizeDelta7.y;
							Vector2 sizeDelta8 = rectTransform2.sizeDelta;
							Vector2 anchoredPosition2 = new Vector2(x4, num4 - sizeDelta8.y * 0.5f);
							(rightClick.transform as RectTransform).anchoredPosition = anchoredPosition2;
							string rightClickText2 = currentState.GetRightClickText();
							if (rightClickText2 != string.Empty)
							{
								if (rightClickText.text != rightClickText2)
								{
									rightClickText.text = rightClickText2;
								}
							}
							else if (rightClickText.text != StringUtil.TR("RightClick", "Tutorial"))
							{
								rightClickText.text = StringUtil.TR("RightClick", "Tutorial");
							}
						}
						bool flag = true;
						int num5;
						if (Options_UI.Get() != null)
						{
							if (Options_UI.Get().GetShiftClickForMovementWaypoints())
							{
								num5 = 1;
								goto IL_0442;
							}
						}
						num5 = 0;
						goto IL_0442;
						IL_0442:
						flag = ((byte)num5 != 0);
						List<BoardSquare> squaresInRegion3 = currentState.m_shiftRightClickHighlight.GetSquaresInRegion();
						if (squaresInRegion3.Count > 0)
						{
							if (flag)
							{
								if (!shiftRightClick.activeSelf)
								{
									UIManager.SetGameObjectActive(shiftRightClick, true);
								}
								Canvas componentInParent3 = shiftRightClick.GetComponentInParent<Canvas>();
								RectTransform rectTransform3 = null;
								if (componentInParent3 != null)
								{
									rectTransform3 = (componentInParent3.transform as RectTransform);
								}
								Vector3 position3 = new Vector3(squaresInRegion3[0].worldX, 1.5f + currentState.m_shiftRightClickHeight, squaresInRegion3[0].worldY);
								Vector2 vector3 = Camera.main.WorldToViewportPoint(position3);
								float x5 = vector3.x;
								Vector2 sizeDelta9 = rectTransform3.sizeDelta;
								float num6 = x5 * sizeDelta9.x;
								Vector2 sizeDelta10 = rectTransform3.sizeDelta;
								float x6 = num6 - sizeDelta10.x * 0.5f;
								float y3 = vector3.y;
								Vector2 sizeDelta11 = rectTransform3.sizeDelta;
								float num7 = y3 * sizeDelta11.y;
								Vector2 sizeDelta12 = rectTransform3.sizeDelta;
								Vector2 anchoredPosition3 = new Vector2(x6, num7 - sizeDelta12.y * 0.5f);
								(shiftRightClick.transform as RectTransform).anchoredPosition = anchoredPosition3;
								string shiftRightClickText2 = currentState.GetShiftRightClickText();
								if (shiftRightClickText2 != string.Empty)
								{
									if (shiftRightClickText.text != shiftRightClickText2)
									{
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
									UIManager.SetGameObjectActive(rightClick, true);
								}
								Canvas componentInParent4 = rightClick.GetComponentInParent<Canvas>();
								RectTransform rectTransform4 = null;
								if (componentInParent4 != null)
								{
									rectTransform4 = (componentInParent4.transform as RectTransform);
								}
								Vector3 position4 = new Vector3(squaresInRegion3[0].worldX, 1.5f + currentState.m_rightClickHeight, squaresInRegion3[0].worldY);
								Vector2 vector4 = Camera.main.WorldToViewportPoint(position4);
								float x7 = vector4.x;
								Vector2 sizeDelta13 = rectTransform4.sizeDelta;
								float num8 = x7 * sizeDelta13.x;
								Vector2 sizeDelta14 = rectTransform4.sizeDelta;
								float x8 = num8 - sizeDelta14.x * 0.5f;
								float y4 = vector4.y;
								Vector2 sizeDelta15 = rectTransform4.sizeDelta;
								float num9 = y4 * sizeDelta15.y;
								Vector2 sizeDelta16 = rectTransform4.sizeDelta;
								Vector2 anchoredPosition4 = new Vector2(x8, num9 - sizeDelta16.y * 0.5f);
								(rightClick.transform as RectTransform).anchoredPosition = anchoredPosition4;
								string rightClickText3 = currentState.GetRightClickText();
								if (rightClickText3 != string.Empty)
								{
									if (rightClickText.text != rightClickText3)
									{
										rightClickText.text = rightClickText3;
									}
								}
								else if (rightClickText.text != StringUtil.TR("RightClick", "Tutorial"))
								{
									rightClickText.text = StringUtil.TR("RightClick", "Tutorial");
								}
							}
						}
						if (squaresInRegion2.Count == 0)
						{
							if (squaresInRegion3.Count != 0)
							{
								if (!flag)
								{
									goto IL_07fa;
								}
							}
							if (rightClick.activeSelf)
							{
								UIManager.SetGameObjectActive(rightClick, false);
							}
						}
						goto IL_07fa;
						IL_07fa:
						if (squaresInRegion3.Count != 0)
						{
							if (flag)
							{
								return;
							}
						}
						if (shiftRightClick.activeSelf)
						{
							UIManager.SetGameObjectActive(shiftRightClick, false);
						}
						return;
					}
				}
			}
		}
	}

	public void UpdateTutorialError(GameObject panel, TextMeshProUGUI text)
	{
		if (panel == null)
		{
			return;
		}
		while (true)
		{
			if (text == null)
			{
				return;
			}
			SinglePlayerState currentState = GetCurrentState();
			string text2;
			if (currentState != null)
			{
				text2 = currentState.GetErrorStringOnForbiddenPath();
			}
			else
			{
				text2 = string.Empty;
			}
			string text3 = text2;
			if (m_errorTriggered)
			{
				if (!(text3 == string.Empty))
				{
					UIManager.SetGameObjectActive(panel, true);
					if (text.text != text3)
					{
						while (true)
						{
							text.text = text3;
							return;
						}
					}
					return;
				}
			}
			UIManager.SetGameObjectActive(panel, false);
			return;
		}
	}

	public void UpdateTutorialTextElements(GameObject panel, TextMeshProUGUI text, GameObject panel2, TextMeshProUGUI text2, GameObject panel3, TextMeshProUGUI text3, GameObject panelCameraMovement, TextMeshProUGUI textCameraMovement, GameObject panelCameraRotation, TextMeshProUGUI textCameraRotation)
	{
		SinglePlayerState currentState = GetCurrentState();
		if (currentState == null)
		{
			return;
		}
		while (true)
		{
			if (panel == null)
			{
				return;
			}
			while (true)
			{
				if (text == null || panel2 == null)
				{
					return;
				}
				while (true)
				{
					if (text2 == null)
					{
						return;
					}
					while (true)
					{
						if (panel3 == null)
						{
							return;
						}
						while (true)
						{
							if (text3 == null)
							{
								return;
							}
							while (true)
							{
								if (panelCameraMovement == null)
								{
									return;
								}
								while (true)
								{
									if (textCameraMovement == null || panelCameraRotation == null)
									{
										return;
									}
									if (textCameraRotation == null)
									{
										while (true)
										{
											switch (2)
											{
											default:
												return;
											case 0:
												break;
											}
										}
									}
									if (Camera.main == null)
									{
										while (true)
										{
											switch (1)
											{
											default:
												return;
											case 0:
												break;
											}
										}
									}
									bool flag = false;
									if (m_lastTutorialTextState != currentState.m_stateIndex)
									{
										flag = true;
									}
									if (currentState.m_tutorialBoxText.m_location == null)
									{
										UIManager.SetGameObjectActive(panel, false);
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
											m_lastTutorialTextState = currentState.m_stateIndex;
											text.text = currentState.GetTutorialBoxText();
											UIManager.SetGameObjectActive(panel, true);
										}
										Vector3 position = currentState.m_tutorialBoxText.m_location.transform.position;
										Vector2 vector = Camera.main.WorldToViewportPoint(position);
										float x = vector.x;
										Vector2 sizeDelta = rectTransform.sizeDelta;
										float num = x * sizeDelta.x;
										Vector2 sizeDelta2 = rectTransform.sizeDelta;
										float x2 = num - sizeDelta2.x * 0.5f;
										float y = vector.y;
										Vector2 sizeDelta3 = rectTransform.sizeDelta;
										float num2 = y * sizeDelta3.y;
										Vector2 sizeDelta4 = rectTransform.sizeDelta;
										Vector2 anchoredPosition = new Vector2(x2, num2 - sizeDelta4.y * 0.5f);
										(panel.transform as RectTransform).anchoredPosition = anchoredPosition;
									}
									if (currentState.m_tutorialBoxText2.m_location == null)
									{
										UIManager.SetGameObjectActive(panel2, false);
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
											m_lastTutorialTextState = currentState.m_stateIndex;
											text2.text = currentState.GetTutorialBoxText2();
											UIManager.SetGameObjectActive(panel2, true);
										}
										Vector3 position2 = currentState.m_tutorialBoxText2.m_location.transform.position;
										Vector2 vector2 = Camera.main.WorldToViewportPoint(position2);
										float x3 = vector2.x;
										Vector2 sizeDelta5 = rectTransform2.sizeDelta;
										float num3 = x3 * sizeDelta5.x;
										Vector2 sizeDelta6 = rectTransform2.sizeDelta;
										float x4 = num3 - sizeDelta6.x * 0.5f;
										float y2 = vector2.y;
										Vector2 sizeDelta7 = rectTransform2.sizeDelta;
										float num4 = y2 * sizeDelta7.y;
										Vector2 sizeDelta8 = rectTransform2.sizeDelta;
										Vector2 anchoredPosition2 = new Vector2(x4, num4 - sizeDelta8.y * 0.5f);
										(panel2.transform as RectTransform).anchoredPosition = anchoredPosition2;
									}
									if (currentState.m_tutorialBoxText3.m_location == null)
									{
										UIManager.SetGameObjectActive(panel3, false);
									}
									else
									{
										Canvas componentInParent3 = panel3.GetComponentInParent<Canvas>();
										RectTransform rectTransform3 = null;
										if (componentInParent3 != null)
										{
											rectTransform3 = (componentInParent3.transform as RectTransform);
										}
										if (flag)
										{
											m_lastTutorialTextState = currentState.m_stateIndex;
											text3.text = currentState.GetTutorialBoxText3();
											UIManager.SetGameObjectActive(panel3, true);
										}
										Vector3 position3 = currentState.m_tutorialBoxText3.m_location.transform.position;
										Vector2 vector3 = Camera.main.WorldToViewportPoint(position3);
										float x5 = vector3.x;
										Vector2 sizeDelta9 = rectTransform3.sizeDelta;
										float num5 = x5 * sizeDelta9.x;
										Vector2 sizeDelta10 = rectTransform3.sizeDelta;
										float x6 = num5 - sizeDelta10.x * 0.5f;
										float y3 = vector3.y;
										Vector2 sizeDelta11 = rectTransform3.sizeDelta;
										float num6 = y3 * sizeDelta11.y;
										Vector2 sizeDelta12 = rectTransform3.sizeDelta;
										Vector2 anchoredPosition3 = new Vector2(x6, num6 - sizeDelta12.y * 0.5f);
										(panel3.transform as RectTransform).anchoredPosition = anchoredPosition3;
									}
									if (currentState.m_tutorialCameraMovementText.m_location == null)
									{
										UIManager.SetGameObjectActive(panelCameraMovement, false);
									}
									else
									{
										Canvas componentInParent4 = panel3.GetComponentInParent<Canvas>();
										RectTransform rectTransform4 = null;
										if (componentInParent4 != null)
										{
											rectTransform4 = (componentInParent4.transform as RectTransform);
										}
										if (flag)
										{
											m_lastTutorialTextState = currentState.m_stateIndex;
											textCameraMovement.text = currentState.GetTutorialCameraMovementText();
											UIManager.SetGameObjectActive(panelCameraMovement, true);
										}
										Vector3 position4 = currentState.m_tutorialCameraMovementText.m_location.transform.position;
										Vector2 vector4 = Camera.main.WorldToViewportPoint(position4);
										float x7 = vector4.x;
										Vector2 sizeDelta13 = rectTransform4.sizeDelta;
										float num7 = x7 * sizeDelta13.x;
										Vector2 sizeDelta14 = rectTransform4.sizeDelta;
										float x8 = num7 - sizeDelta14.x * 0.5f;
										float y4 = vector4.y;
										Vector2 sizeDelta15 = rectTransform4.sizeDelta;
										float num8 = y4 * sizeDelta15.y;
										Vector2 sizeDelta16 = rectTransform4.sizeDelta;
										Vector2 anchoredPosition4 = new Vector2(x8, num8 - sizeDelta16.y * 0.5f);
										(panelCameraMovement.transform as RectTransform).anchoredPosition = anchoredPosition4;
									}
									if (currentState.m_tutorialCameraRotationText.m_location == null)
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
												UIManager.SetGameObjectActive(panelCameraRotation, false);
												return;
											}
										}
									}
									Canvas componentInParent5 = panel3.GetComponentInParent<Canvas>();
									RectTransform rectTransform5 = null;
									if (componentInParent5 != null)
									{
										rectTransform5 = (componentInParent5.transform as RectTransform);
									}
									if (flag)
									{
										m_lastTutorialTextState = currentState.m_stateIndex;
										textCameraRotation.text = currentState.GetTutorialCameraRotationText();
										UIManager.SetGameObjectActive(panelCameraRotation, true);
									}
									Vector3 position5 = currentState.m_tutorialCameraRotationText.m_location.transform.position;
									Vector2 vector5 = Camera.main.WorldToViewportPoint(position5);
									float x9 = vector5.x;
									Vector2 sizeDelta17 = rectTransform5.sizeDelta;
									float num9 = x9 * sizeDelta17.x;
									Vector2 sizeDelta18 = rectTransform5.sizeDelta;
									float x10 = num9 - sizeDelta18.x * 0.5f;
									float y5 = vector5.y;
									Vector2 sizeDelta19 = rectTransform5.sizeDelta;
									float num10 = y5 * sizeDelta19.y;
									Vector2 sizeDelta20 = rectTransform5.sizeDelta;
									Vector2 anchoredPosition5 = new Vector2(x10, num10 - sizeDelta20.y * 0.5f);
									(panelCameraRotation.transform as RectTransform).anchoredPosition = anchoredPosition5;
									return;
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
		SinglePlayerState currentState = GetCurrentState();
		bool flag = false;
		if (currentState != null)
		{
			if (m_lastTutorialCameraState != currentState.m_stateIndex)
			{
				flag = true;
			}
		}
		if (flag)
		{
			m_lastTutorialCameraState = currentState.m_stateIndex;
			if (currentState.m_cameraRotationTarget == null)
			{
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
		UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar, false);
		UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText, false);
	}

	[ClientRpc]
	public void RpcPlayScriptedChat(SinglePlayerScriptedChat chatText)
	{
		UITutorialPanel.Get().QueueDialogue(chatText.m_text, chatText.m_audioEvent, chatText.m_displaySeconds, chatText.m_sender);
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcPlayScriptedChat(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcPlayScriptedChat called on server.");
					return;
				}
			}
		}
		((SinglePlayerManager)obj).RpcPlayScriptedChat(GeneratedNetworkCode._ReadSinglePlayerScriptedChat_None(reader));
	}

	public void CallRpcPlayScriptedChat(SinglePlayerScriptedChat chatText)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcPlayScriptedChat called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcPlayScriptedChat);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		GeneratedNetworkCode._WriteSinglePlayerScriptedChat_None(networkWriter, chatText);
		SendRPCInternal(networkWriter, 0, "RpcPlayScriptedChat");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_currentScriptIndex);
			writer.Write(m_canEndTurn);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_currentScriptIndex);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_canEndTurn);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_currentScriptIndex = (int)reader.ReadPackedUInt32();
					m_canEndTurn = reader.ReadBoolean();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			HookSetCurrentScriptIndex((int)reader.ReadPackedUInt32());
		}
		if ((num & 2) != 0)
		{
			HookSetCanEndTurn(reader.ReadBoolean());
		}
	}
}
