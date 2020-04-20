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
			SinglePlayerManager.s_instance = singlePlayerManager;
		}
		return gameObject;
	}

	private static void OnDespawn(GameObject spawned)
	{
		if (SinglePlayerManager.s_instance == spawned)
		{
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
					if (hasTag != this.m_pausedTimer)
					{
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
				if (currentState2.m_markedForAdvanceState)
				{
					newIndex++;
				}
			}
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
			this.SetScriptIndex(value);
		}
	}

	public void SetCanEndTurn(bool canEnd)
	{
		if (NetworkServer.active)
		{
			this.Networkm_canEndTurn = canEnd;
		}
	}

	public bool GetCanEndTurnFlag()
	{
		if (NetworkServer.active)
		{
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
			if (SinglePlayerManager.Get().GetCurrentState() != null)
			{
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
			if (currentState.m_startTime != 0f)
			{
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
			if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnDecisionEnd))
			{
				this.AdvanceScript();
			}
		}
	}

	internal void OnResolutionEnd()
	{
		SinglePlayerState currentState = this.GetCurrentState();
		if (currentState != null)
		{
			if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnResolutionEnd))
			{
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
			SinglePlayerState[] script = SinglePlayerCoordinator.Get().m_script;
			if (scriptIndex >= 0)
			{
				if (scriptIndex < script.Length)
				{
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
			if (actor != null)
			{
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

	public void symbol_001D()
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
					return actorData;
				}
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
			if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnMoveEntered))
			{
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
			if (currentState != null)
			{
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnAbilitySelected))
				{
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
			if (currentState != null)
			{
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnAbilityTargeted))
				{
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
			if (currentState != null)
			{
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnLockInEntered))
				{
					this.AdvanceScript();
				}
			}
		}
	}

	public void OnTutorialQueueEmpty()
	{
		if (NetworkServer.active)
		{
			SinglePlayerState currentState = this.GetCurrentState();
			if (currentState != null)
			{
				if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnTutorialQueueEmpty))
				{
					this.AdvanceScript();
				}
			}
		}
		else
		{
			ActorData localPlayer = this.GetLocalPlayer();
			if (localPlayer != null)
			{
				PlayerData component = localPlayer.GetComponent<PlayerData>();
				if (component != null)
				{
					component.CallCmdTutorialQueueEmpty();
				}
			}
		}
	}

	private void UpdateDestinationHighlights()
	{
		if (this.m_advanceDestinationsHighlight != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m_advanceDestinationsHighlight);
			this.m_advanceDestinationsHighlight = null;
		}
		if (this.GetCurrentState() != null)
		{
			HashSet<BoardSquare> advanceDestinations = this.GetCurrentState().GetAdvanceDestinations();
			if (advanceDestinations != null)
			{
				if (advanceDestinations.Count > 0)
				{
					this.m_advanceDestinationsHighlight = HighlightUtils.Get().CreateBoundaryHighlight(advanceDestinations, Color.white, false, null, false);
					if (this.m_advanceDestinationsHighlight)
					{
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
			this.m_errorTriggered = false;
			this.RecalcCanEndTurn();
			if (!this.GetCanEndTurnFlag())
			{
				if (!this.IsMovementAllowed())
				{
					if (SinglePlayerCoordinator.Get() != null)
					{
						foreach (ActivatableObject activatableObject in SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath)
						{
							if (activatableObject == null)
							{
								Log.Error("Null activation-object in Activations On Forbidden Path.", new object[0]);
							}
							else if (activatableObject.m_sceneObject == null)
							{
								Log.Error("Activation-object has null scene-object in Activations On Forbidden Path.", new object[0]);
							}
							else
							{
								activatableObject.SetIsActive(true);
							}
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
			if (SinglePlayerCoordinator.Get() != null)
			{
				foreach (ActivatableObject activatableObject in SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath)
				{
					if (activatableObject != null && activatableObject.m_sceneObject != null)
					{
						activatableObject.SetIsActive(false);
					}
				}
				foreach (ActivatableObject activatableObject2 in SinglePlayerCoordinator.Get().m_activationsOnFailedToShootAndMove)
				{
					if (activatableObject2 != null && activatableObject2.m_sceneObject != null)
					{
						activatableObject2.SetIsActive(false);
					}
				}
				foreach (ActivatableObject activatableObject3 in SinglePlayerCoordinator.Get().m_activationsOnFailedToUseAllAbilities)
				{
					if (activatableObject3 != null)
					{
						if (activatableObject3.m_sceneObject != null)
						{
							activatableObject3.SetIsActive(false);
						}
					}
				}
			}
			if (!this.GetCanEndTurnFlag())
			{
				if (!this.IsMovementAllowed())
				{
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
								Log.Error("Activation-object has null scene-object in Activations On Forbidden Path.", new object[0]);
							}
							else
							{
								activatableObject4.SetIsActive(true);
							}
						}
					}
				}
				else if (!this.IsShootAndMoveAllowed())
				{
					if (SinglePlayerCoordinator.Get() != null)
					{
						foreach (ActivatableObject activatableObject5 in SinglePlayerCoordinator.Get().m_activationsOnFailedToShootAndMove)
						{
							if (activatableObject5 == null)
							{
								Log.Error("Null activation-object in Activations On Failed To Shoot And Move.", new object[0]);
							}
							else if (activatableObject5.m_sceneObject == null)
							{
								Log.Error("Activation-object has null scene-object in Activations On Failed To Shoot And Move.", new object[0]);
							}
							else
							{
								activatableObject5.SetIsActive(true);
							}
						}
					}
				}
				else if (!this.IsMultipleAbilitiesAllowed() && SinglePlayerCoordinator.Get() != null)
				{
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
				}
			}
		}
	}

	public static bool IsActionAllowed(ActorData caster, AbilityData.ActionType action)
	{
		if (SinglePlayerManager.s_instance == null)
		{
			return true;
		}
		if (SinglePlayerManager.s_instance.GetCurrentState() == null)
		{
			return true;
		}
		if (caster.GetIsHumanControlled())
		{
			bool result = false;
			SinglePlayerState currentState = SinglePlayerManager.s_instance.GetCurrentState();
			if (currentState.m_allowedAbilities.Length == 0)
			{
				result = true;
			}
			else
			{
				foreach (int num in currentState.m_allowedAbilities)
				{
					if (num == (int)action)
					{
						return true;
					}
				}
			}
			return result;
		}
		Ability abilityOfActionType = caster.GetAbilityData().GetAbilityOfActionType(action);
		if (abilityOfActionType == null)
		{
			return true;
		}
		if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.AutoQueueIfValid))
		{
			return SinglePlayerManager.Get().EnableAutoQueuedAbilitiesForNPCs();
		}
		return true;
	}

	public static bool IsDestinationAllowed(ActorData mover, BoardSquare square, bool settingWaypoints = true)
	{
		if (SinglePlayerManager.s_instance == null)
		{
			return true;
		}
		if (SinglePlayerManager.s_instance.GetCurrentState() == null)
		{
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
			flag2 = true;
		}
		else
		{
			flag2 = settingWaypoints;
		}
		bool result;
		if (flag)
		{
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
			return false;
		}
		if (SinglePlayerManager.s_instance.GetCurrentState() == null)
		{
			return false;
		}
		if (!mover.GetIsHumanControlled())
		{
			return false;
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
		bool flag = this.IsMovementAllowed();
		bool flag2 = this.IsShootAndMoveAllowed();
		bool flag3 = this.IsMultipleAbilitiesAllowed();
		bool flag4 = this.IsMoveToAdvanceScriptDestinationAllowed();
		bool flag5 = this.IsRequireDashOk();
		bool flag6 = !this.DisableAdvanceTurn();
		bool flag7;
		if (flag)
		{
			if (flag2)
			{
				if (flag3 && flag4)
				{
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
			if (this.GetCurrentState() != null)
			{
				if (this.GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
				{
					ActorData localPlayer = this.GetLocalPlayer();
					AbilityData abilityData = localPlayer.GetAbilityData();
					bool flag = abilityData.HasQueuedAbilities();
					if (flag)
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
			if (this.GetCurrentState() != null)
			{
				SinglePlayerState currentState = this.GetCurrentState();
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
							ActorData localPlayer = this.GetLocalPlayer();
							if (!NetworkServer.active && localPlayer.GetComponent<LineData>() != null)
							{
								flag = localPlayer.GetComponent<LineData>().GetIsChasing();
							}
							if (!flag)
							{
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
							if (localPlayer2.GetComponent<LineData>())
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
										GridPos gridPos = enumerator.Current;
										BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
										if (SinglePlayerCoordinator.Get().m_forbiddenSquares.GetSquaresInRegion().Contains(boardSquareSafe))
										{
											result = false;
											goto IL_1B0;
										}
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
				List<GridPos> gridPosPath = localPlayer.GetComponent<LineData>().GetGridPosPath();
				if (gridPosPath != null)
				{
					if (gridPosPath.Count > 0)
					{
						item = Board.Get().GetBoardSquareSafe(gridPosPath[gridPosPath.Count - 1]);
					}
				}
			}
			if (this.GetCurrentState().GetAdvanceDestinations() != null)
			{
				if (this.GetCurrentState().GetAdvanceDestinations().Contains(item))
				{
					return true;
				}
			}
			if (this.GetCurrentState().GetAdvanceDestinations() == null)
			{
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
			result = true;
		}
		else if (!this.GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireShootAndMove))
		{
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
				if (list.Count > 1)
				{
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
			result = true;
		}
		else if (!this.GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireMaxPossibleAbilities))
		{
			result = true;
		}
		else if (this.GetCurrentState().m_allowedAbilities.Length == 0)
		{
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
			return true;
		}
		if (!aimer.GetIsHumanControlled())
		{
			return true;
		}
		AbilityData component = aimer.GetComponent<AbilityData>();
		Ability selectedAbility = component.GetSelectedAbility();
		bool flag;
		bool flag2;
		if (selectedAbility == null)
		{
			flag = true;
			flag2 = true;
		}
		else
		{
			SinglePlayerState currentState = SinglePlayerManager.s_instance.GetCurrentState();
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
					flag3 = squaresInRegion.Contains(boardSquare);
				}
				else
				{
					flag3 = true;
				}
				flag2 = flag3;
				if (flag2 && currentState.m_mustTargetNearCenter)
				{
					AbilityTarget abilityTarget3 = AbilityTarget.CreateAbilityTargetFromInterface();
					Vector3 center = currentState.m_allowedTargets.GetCenter();
					float f = center.x - abilityTarget3.FreePos.x;
					float f2 = center.z - abilityTarget3.FreePos.z;
					float num = 0.45f * Board.Get().squareSize;
					if (Mathf.Abs(f) < num)
					{
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
			if (!(leftClick == null))
			{
				if (!(leftClickText == null) && !(shiftRightClick == null) && !(shiftRightClickText == null))
				{
					if (currentState != null)
					{
						if (currentState.m_leftClickHighlight != null)
						{
							List<BoardSquare> squaresInRegion = currentState.m_leftClickHighlight.GetSquaresInRegion();
							if (squaresInRegion.Count > 0)
							{
								if (!leftClick.activeSelf)
								{
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
								UIManager.SetGameObjectActive(leftClick, false, null);
							}
							List<BoardSquare> squaresInRegion2 = currentState.m_rightClickHighlight.GetSquaresInRegion();
							if (squaresInRegion2.Count > 0)
							{
								if (!rightClick.activeSelf)
								{
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
							bool flag;
							if (Options_UI.Get() != null)
							{
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
								if (flag2)
								{
									if (!shiftRightClick.activeSelf)
									{
										UIManager.SetGameObjectActive(shiftRightClick, true, null);
									}
									Canvas componentInParent3 = shiftRightClick.GetComponentInParent<Canvas>();
									RectTransform rectTransform3 = null;
									if (componentInParent3 != null)
									{
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
										UIManager.SetGameObjectActive(rightClick, true, null);
									}
									Canvas componentInParent4 = rightClick.GetComponentInParent<Canvas>();
									RectTransform rectTransform4 = null;
									if (componentInParent4 != null)
									{
										rectTransform4 = (componentInParent4.transform as RectTransform);
									}
									Vector3 position4 = new Vector3(squaresInRegion3[0].worldX, 1.5f + currentState.m_rightClickHeight, squaresInRegion3[0].worldY);
									Vector2 vector4 = Camera.main.WorldToViewportPoint(position4);
									Vector2 anchoredPosition4 = new Vector2(vector4.x * rectTransform4.sizeDelta.x - rectTransform4.sizeDelta.x * 0.5f, vector4.y * rectTransform4.sizeDelta.y - rectTransform4.sizeDelta.y * 0.5f);
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
									if (!flag2)
									{
										goto IL_7FA;
									}
								}
								if (rightClick.activeSelf)
								{
									UIManager.SetGameObjectActive(rightClick, false, null);
								}
							}
							IL_7FA:
							if (squaresInRegion3.Count != 0)
							{
								if (flag2)
								{
									return;
								}
							}
							if (shiftRightClick.activeSelf)
							{
								UIManager.SetGameObjectActive(shiftRightClick, false, null);
							}
							return;
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
			if (!(text == null))
			{
				SinglePlayerState currentState = this.GetCurrentState();
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
				if (this.m_errorTriggered)
				{
					if (text3 == string.Empty)
					{
					}
					else
					{
						UIManager.SetGameObjectActive(panel, true, null);
						if (text.text != text3)
						{
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
			if (!(panel == null))
			{
				if (!(text == null) && !(panel2 == null))
				{
					if (!(text2 == null))
					{
						if (!(panel3 == null))
						{
							if (!(text3 == null))
							{
								if (!(panelCameraMovement == null))
								{
									if (!(textCameraMovement == null) && !(panelCameraRotation == null))
									{
										if (textCameraRotation == null)
										{
										}
										else
										{
											if (Camera.main == null)
											{
												return;
											}
											bool flag = false;
											if (this.m_lastTutorialTextState != currentState.m_stateIndex)
											{
												flag = true;
											}
											if (currentState.m_tutorialBoxText.m_location == null)
											{
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
												UIManager.SetGameObjectActive(panel3, false, null);
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
												UIManager.SetGameObjectActive(panelCameraMovement, false, null);
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
				if (!base.syncVarHookGuard)
				{
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
			Debug.LogError("RPC RpcPlayScriptedChat called on server.");
			return;
		}
		((SinglePlayerManager)obj).RpcPlayScriptedChat(GeneratedNetworkCode._ReadSinglePlayerScriptedChat_None(reader));
	}

	public void CallRpcPlayScriptedChat(SinglePlayerScriptedChat chatText)
	{
		if (!NetworkServer.active)
		{
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_currentScriptIndex);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_canEndTurn);
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
