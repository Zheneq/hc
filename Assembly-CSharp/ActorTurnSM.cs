// ROGUES
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorTurnSM : NetworkBehaviour
{
	public class ActionRequestForUndo
	{
		public UndoableRequestType m_type;
		public AbilityData.ActionType m_action;

		public ActionRequestForUndo(
			UndoableRequestType requestType,
			AbilityData.ActionType actionType = AbilityData.ActionType.INVALID_ACTION)
		{
			m_type = requestType;
			m_action = actionType;
		}
	}

	// TODO check server logic moved into these classes -- we might have missed something
	// rogues
	//private ActorTurnSMInternal m_fsmInternal; // they moved the state machine itself there in rogues
	private ActorData m_actorData;
	private bool m_firstUpdate;
	// removed in rogues
	private float m_timePingDown;
	// removed in rogues
	private Vector3 m_worldPositionPingDown;
	// removed in rogues
	private Vector3 m_mousePositionPingDown;

	// removed in rogues
	private const float c_advancedPingTime = 0.25f;

	// removed in rogues
	private bool m_abilitySelectorVisible;

	// removed in rogues
	private static Color s_chasingTextColor = new Color(0.3f, 0.75f, 0.75f);
	// removed in rogues
	private static Color s_movingTextColor = new Color(0.4f, 1f, 1f);
	// removed in rogues
	private static Color s_decidingTextColor = new Color(0.9f, 0.9f, 0.9f);

	private TurnStateEnum _NextState;

	// rogues
	//private SyncListBool m_pvePlayedAbilityFlags = new SyncListBool();
	//[SyncVar]
	//private float m_pveMoveCostUsedThisTurn;
	//[SyncVar]
	//private uint m_pveNumMoveActionsThisTurn;
	//[SyncVar]
	//private uint m_pveNumAbilityActionsThisTurn;
	//[SyncVar]
	//private uint m_pveNumQuickActionsThisTurn;
	//[SyncVar]
	//private uint m_pveNumFreeActionsThisTurn;
	//[SyncVar]
	//private uint m_pveMaxNumFreeActionsThisTurn;
	//[SyncVar]
	//private uint m_pveNumAbilityActionsPerTurn = 1U;
	//[SyncVar]
	//private bool m_hasStoredAbilityRequest;
	//[SyncVar]
	//private bool m_hasStoredMoveRequest;
	//[SyncVar]
	//private short m_numRespawnPicksThisTurn;
	//[SyncVar]
	//public int m_tauntRequestedForNextAbility = -1;

	private DateTime _TurnStart = DateTime.UtcNow;
	private DateTime _LockInTime = DateTime.MinValue;
	private List<AbilityData.ActionType> m_autoQueuedRequestActionTypes;
	private List<ActionRequestForUndo> m_requestStackForUndo;
	// removed in rogues
	private TurnState[] m_turnStates;
	private List<AbilityTarget> m_targets;

	// removed in rogues
	private static int kCmdCmdGUITurnMessage = -122150213;
	private static int kCmdCmdRequestCancelAction = 1831775955;
	private static int kCmdCmdChase = 1451912258;
	private static int kCmdCmdSetSquare = -1156253069;
	private static int kRpcRpcTurnMessage = -107921272;
	private static int kRpcRpcStoreAutoQueuedAbilityRequest = 675585254;


	// added in rogues
#if SERVER
	public ActorData Owner
	{
		get
		{
			return m_actorData;
		}
	}
#endif

	// removed in rogues
	public bool LockInBuffered { get; set; }
	public TurnStateEnum CurrentState { get; private set; }
	public TurnStateEnum PreviousState { get; private set; }

	public TurnStateEnum NextState
	{
		get
		{
			return _NextState;
		}
		set
		{
			if (value >= TurnStateEnum.CONFIRMED && _NextState < TurnStateEnum.CONFIRMED)
			{
				_LockInTime = DateTime.UtcNow;

			}
			else if (value < TurnStateEnum.CONFIRMED && _NextState >= TurnStateEnum.CONFIRMED)
			{
				_TurnStart = DateTime.UtcNow;
				_LockInTime = DateTime.MinValue;
			}
			_NextState = value;
		}
	}

	public TimeSpan TimeToLockIn
	{
		get
		{
			if (_LockInTime == DateTime.MinValue)
			{
				return TimeSpan.Zero;
			}
			return _LockInTime - _TurnStart;
		}
	}

	// removed in rogues
	public bool HandledSpaceInput;
	// removed in rogues
	public bool HandledMouseInput;

	internal int LastConfirmedCancelTurn { get; private set; }

	static ActorTurnSM()
	{
		// reactor
		RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdGUITurnMessage, InvokeCmdCmdGUITurnMessage);
		RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdRequestCancelAction, InvokeCmdCmdRequestCancelAction);
		RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdChase, InvokeCmdCmdChase);
		RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdSetSquare, InvokeCmdCmdSetSquare);
		RegisterRpcDelegate(typeof(ActorTurnSM), kRpcRpcTurnMessage, InvokeRpcRpcTurnMessage);
		RegisterRpcDelegate(typeof(ActorTurnSM), kRpcRpcStoreAutoQueuedAbilityRequest, InvokeRpcRpcStoreAutoQueuedAbilityRequest);
		NetworkCRC.RegisterBehaviour("ActorTurnSM", 0);
		// rogues
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), "CmdGUITurnMessage", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdGUITurnMessage));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), "CmdRequestCancelAction", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdRequestCancelAction));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), "CmdLockInRequested", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdLockInRequested));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), "CmdExecuteQueuedRequests", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdExecuteQueuedRequests));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), "CmdChase", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdChase));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), "CmdSetSquare", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdSetSquare));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), "CmdGroupMoveToSquare", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdGroupMoveToSquare));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTurnSM), "RpcTurnMessage", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeRpcRpcTurnMessage));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTurnSM), "RpcStoreAutoQueuedAbilityRequest", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeRpcRpcStoreAutoQueuedAbilityRequest));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTurnSM), "RpcSetNumRespawnPickInputs", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeRpcRpcSetNumRespawnPickInputs));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTurnSM), "RpcResetUsedAbilityAndMoveData", new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeRpcRpcResetUsedAbilityAndMoveData));
	}

	private void Awake()
	{

		// rogues
		//if (NetworkServer.active)
		//{
		//	for (int i = 0; i <= 9; i++)
		//	{
		//		m_pvePlayedAbilityFlags.Add(false);
		//	}
		//	Networkm_pveMoveCostUsedThisTurn = 0f;
		//}

		// reactor
		m_turnStates = new TurnState[11];
		m_turnStates[0] = new DecidingState(this);
		m_turnStates[1] = new ValidatingMoveRequestState(this);
		m_turnStates[2] = new TargetingActionState(this);
		m_turnStates[3] = new ValidatingActionRequestState(this);
		m_turnStates[4] = new DecidingMovementState(this);
		m_turnStates[5] = new ConfirmedState(this);
		m_turnStates[6] = new ResolvingState(this);
		m_turnStates[7] = new WaitingState(this);
		m_turnStates[8] = new RespawningState(this);
		m_turnStates[10] = new PickingRespawnState(this);
		m_turnStates[9] = new RespawningTakesActionState(this);
		// rogues
		//m_fsmInternal = new ActorTurnSMInternal_FCFS(this);

		CurrentState = PreviousState = NextState = TurnStateEnum.WAITING;
		m_targets = new List<AbilityTarget>();
		LastConfirmedCancelTurn = -1;
		m_requestStackForUndo = new List<ActionRequestForUndo>();
		m_autoQueuedRequestActionTypes = new List<AbilityData.ActionType>();
		m_actorData = GetComponent<ActorData>();
		m_firstUpdate = true;
	}

	// rogues
	//public uint NumAbilityActionsPerTurn
	//{
	//	get
	//	{
	//		return m_pveNumAbilityActionsPerTurn;
	//	}
	//	set
	//	{
	//		Networkm_pveNumAbilityActionsPerTurn = value;
	//	}
	//}

	// rogues
	//public void MarkPveAbilityFlagAtIndex(int index)
	//{
	//	if (index >= 0 && index < m_pvePlayedAbilityFlags.Count)
	//	{
	//		m_pvePlayedAbilityFlags[index] = true;
	//		Ability abilityOfActionType = Owner.GetAbilityData().GetAbilityOfActionType((AbilityData.ActionType)index);
	//		if (abilityOfActionType != null && !abilityOfActionType.IsFreeAction())
	//		{
	//			if (abilityOfActionType.m_quickAction)
	//			{
	//				IncrementPveNumQuickActions(1);
	//				return;
	//			}
	//			if (abilityOfActionType.m_freeAction)
	//			{
	//				IncrementPveNumFreeActions(1);
	//				return;
	//			}
	//			IncrementPveNumAbilityActions(1);
	//		}
	//	}
	//}

	// rogues
	//public bool PveIsAbilityAtIndexUsed(int index)
	//{
	//	return index >= 0 && index < m_pvePlayedAbilityFlags.Count && m_pvePlayedAbilityFlags[index];
	//}

	// rogues
	//public void IncrementPveMoveCostUsed(float increment)
	//{
	//	Networkm_pveMoveCostUsedThisTurn = m_pveMoveCostUsedThisTurn + increment;
	//}

	// rogues
	//public void IncrementPveNumMoveActions(int increment)
	//{
	//	Networkm_pveNumMoveActionsThisTurn = m_pveNumMoveActionsThisTurn + (uint)increment;
	//}

	// rogues
	//public void IncrementPveNumQuickActions(int increment)
	//{
	//	Networkm_pveNumQuickActionsThisTurn = m_pveNumQuickActionsThisTurn + (uint)increment;
	//}

	// rogues
	//public void IncrementPveNumFreeActions(int increment)
	//{
	//	Networkm_pveNumFreeActionsThisTurn = m_pveNumFreeActionsThisTurn + (uint)increment;
	//}

	// rogues
	//public void IncrementPveNumAbilityActions(int increment)
	//{
	//	Networkm_pveNumAbilityActionsThisTurn = m_pveNumAbilityActionsThisTurn + (uint)increment;
	//}

	// rogues
	//public float GetPveMovementCostUsed()
	//{
	//	return m_pveMoveCostUsedThisTurn;
	//}

	// rogues
	//public int GetPveNumMoveActionsUsed()
	//{
	//	return (int)m_pveNumMoveActionsThisTurn;
	//}

	// rogues
	//public void IncrementRespawnPickInput()
	//{
	//	Networkm_numRespawnPicksThisTurn = m_numRespawnPicksThisTurn + 1;
	//	if (NetworkServer.active)
	//	{
	//		CallRpcSetNumRespawnPickInputs((int)m_numRespawnPicksThisTurn);
	//	}
	//}

	// rogues
	//public int GetNumRespawnPickInputs()
	//{
	//	return (int)m_numRespawnPicksThisTurn;
	//}

	// rogues
	//public void ResetUsedAbilityAndMoveData()
	//{
	//       for (int i = 0; i < m_pvePlayedAbilityFlags.Count; i++)
	//       {
	//           if (m_pvePlayedAbilityFlags[i])
	//           {
	//               m_pvePlayedAbilityFlags[i] = false;
	//           }
	//       }
	//       Networkm_pveNumAbilityActionsThisTurn = 0U;
	//       Networkm_pveMoveCostUsedThisTurn = 0f;
	//       Networkm_pveNumMoveActionsThisTurn = 0U;
	//       Networkm_pveNumQuickActionsThisTurn = 0U;
	//       Networkm_pveNumFreeActionsThisTurn = 0U;
	//       Networkm_hasStoredAbilityRequest = false;
	//	Networkm_hasStoredMoveRequest = false;
	//	Networkm_numRespawnPicksThisTurn = 0;
	//	SetMaxFreeActionsThisTurn();
	//}

	// rogues
	//public bool UsedAllFullActions()
	//{
	//	return !HasRemainingAbilityUse(false) && !HasRemainingMovement() && !HasRemainingFreeActions();
	//}

	// rogues
	//public bool HasRemainingFreeActions()
	//{
	//	return m_pveNumFreeActionsThisTurn < m_pveMaxNumFreeActionsThisTurn;
	//}

	// rogues
	//public bool HasRemainingMovement()
	//{
	//	if (m_pveNumMoveActionsThisTurn >= 2U)
	//	{
	//		return false;
	//	}
	//	if (m_pveNumQuickActionsThisTurn >= 2U)
	//	{
	//		return false;
	//	}
	//	if (m_pveNumMoveActionsThisTurn >= 1U && m_pveNumAbilityActionsThisTurn >= m_pveNumAbilityActionsPerTurn)
	//	{
	//		return false;
	//	}
	//	if (m_pveNumMoveActionsThisTurn >= 1U && m_pveNumQuickActionsThisTurn >= 1U)
	//	{
	//		return false;
	//	}
	//	if (m_pveNumQuickActionsThisTurn >= 1U && m_pveNumAbilityActionsThisTurn >= m_pveNumAbilityActionsPerTurn)
	//	{
	//		return false;
	//	}
	//	AbilityData abilityData = Owner.GetAbilityData();
	//	return !(abilityData != null) || !abilityData.DidAnyExecutedAbilitiesUseAllMovement();
	//}

	// rogues
	//public bool HasRemainingAbilityUse(bool isQuickAction)
	//{
	//	return m_pveNumMoveActionsThisTurn < 2U && m_pveNumQuickActionsThisTurn < 2U && (m_pveNumMoveActionsThisTurn < 1U || m_pveNumQuickActionsThisTurn < 1U) && (m_pveNumAbilityActionsThisTurn < m_pveNumAbilityActionsPerTurn || isQuickAction) && (m_pveNumAbilityActionsThisTurn < m_pveNumAbilityActionsPerTurn || !isQuickAction || m_pveNumMoveActionsThisTurn < 1U);
	//}

	// rogues
	//public void UpdateHasStoredAbilityRequestFlag()
	//{
	//	if (NetworkServer.active)
	//	{
	//		bool flag = ServerActionBuffer.Get().HasPendingAbilityRequest(Owner, true);
	//		if (m_hasStoredAbilityRequest != flag)
	//		{
	//			Networkm_hasStoredAbilityRequest = flag;
	//		}
	//	}
	//}

	// rogues
	//public void UpdateHasStoredMoveRequestFlag()
	//{
	//	if (NetworkServer.active)
	//	{
	//		bool flag = ServerActionBuffer.Get().HasPendingMovementRequest(Owner);
	//		if (m_hasStoredMoveRequest != flag)
	//		{
	//			Networkm_hasStoredMoveRequest = flag;
	//		}
	//	}
	//}

	// rogues
	//public bool HasStoredAbilityOrMoveRequest()
	//{
	//	return m_hasStoredAbilityRequest || m_hasStoredMoveRequest;
	//}

	// rogues
	//public bool HasStoredAbilityRequest()
	//{
	//	return m_hasStoredAbilityRequest;
	//}

	// rogues
	//public bool HasStoredMoveRequest()
	//{
	//	return m_hasStoredMoveRequest;
	//}

	// rogues
	//private void SetMaxFreeActionsThisTurn()
	//{
	//	Networkm_pveMaxNumFreeActionsThisTurn = 0U;
	//	AbilityData abilityData = Owner.GetAbilityData();
	//	for (AbilityData.ActionType actionType = AbilityData.ActionType.ABILITY_0; actionType < AbilityData.ActionType.ABILITY_4; actionType++)
	//	{
	//		Ability abilityOfActionType = abilityData.GetAbilityOfActionType(actionType);
	//		if (abilityOfActionType != null && abilityOfActionType.m_freeAction && abilityOfActionType.GetRemainingCooldown() == 0)
	//		{
	//			Networkm_pveMaxNumFreeActionsThisTurn = m_pveMaxNumFreeActionsThisTurn + 1U;
	//		}
	//	}
	//}

	// removed in rogues
	public void OnSelect()
	{
		HandledSpaceInput = true;
	}

	public bool SelectTarget(AbilityTarget abilityTargetToUse, bool onLockIn = false)
	{
		bool result = false;
		ActorData component = GetComponent<ActorData>();
		if (!SinglePlayerManager.IsAbilitysCurrentAimingAllowed(component))
		{
			return false;
		}
		AbilityData abilityData = GetComponent<AbilityData>();
		Ability selectedAbility = abilityData.GetSelectedAbility();
		int targetersNum = 0;
		int expectedTargetersNum = 0;
		if (selectedAbility)
		{
			AbilityTarget abilityTarget = abilityTargetToUse != null
				? abilityTargetToUse.GetCopy()
				: AbilityTarget.CreateAbilityTargetFromInterface();
			AddAbilityTarget(abilityTarget);
			targetersNum = selectedAbility.GetNumTargets();
			expectedTargetersNum = selectedAbility.GetExpectedNumberOfTargeters();
			if (m_targets.Count > targetersNum || m_targets.Count > expectedTargetersNum)
			{
				Log.Error("SelectTarget has been called more times than there are targeters for the selected ability. " +
					"m_targets.Count {0}, numTargets {1}, numExpectedTargets {2}", m_targets.Count, targetersNum, expectedTargetersNum);
			}
			selectedAbility.Targeter.AbilityCasted(component.GetGridPos(), abilityTarget.GridPos);
		}
		if ((GetAbilityTargets().Count < targetersNum && GetAbilityTargets().Count < expectedTargetersNum) || targetersNum == 0)
		{
			if (selectedAbility != null && expectedTargetersNum > 1)
			{
				List<AbilityTarget> abilityTargets = GetAbilityTargets();
				for (int i = 0; i < abilityTargets.Count && i < selectedAbility.Targeters.Count; i++)
				{
					AbilityUtil_Targeter abilityUtil_Targeter = selectedAbility.Targeters[i];
					abilityUtil_Targeter.SetLastUpdateCursorState(abilityTargets[i]);
					abilityUtil_Targeter.UpdateHighlightPosAfterClick(abilityTargets[i], component, i, abilityTargets);
					abilityUtil_Targeter.SetupTargetingArc(component, false);

					// removed in rogues
					abilityUtil_Targeter.MarkedForForceUpdate = true;
					if (component == GameFlowData.Get().activeOwnedActorData)
					{
						abilityUtil_Targeter.HideAllSquareIndicators();
					}
				}
			}
		}
		else
		{
			AbilityData.ActionType selectedActionType = abilityData.GetSelectedActionType();
			OnQueueAbilityRequest(selectedActionType);
			result = true;
			// reactor
			if (NetworkClient.active && onLockIn)
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (selectedAbility != null && activeOwnedActorData != null && activeOwnedActorData == component)
				{
					selectedAbility.ResetAbilityTargeters();
				}
			}
			// rogues
			//if (NetworkClient.active && onLockIn && selectedAbility != null && IsClientActor())
			//{
			//    selectedAbility.ResetAbilityTargeters();
			//}

			if (!NetworkServer.active)
			{
				SendCastAbility(selectedActionType);
			}
#if SERVER
			else
			{
				// added in rogues
				abilityData.GetComponent<ServerActorController>().ProcessCastAbilityRequest(GetAbilityTargets(), selectedActionType, false);
			}
#endif
		}
		Board.Get()?.MarkForUpdateValidSquares();
		return result;
	}

	// removed in rogues
	private void CheckAbilityInput()
	{
		if (HUD_UI.Get() == null ||
			GameFlowData.Get().activeOwnedActorData != GetComponent<ActorData>() ||
			!GameFlowData.Get().IsInDecisionState())
		{
			return;
		}

		if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightTrigger) > 0f)
		{
			if (!m_abilitySelectorVisible)
			{
				AbilityData abilityData = GetComponent<ActorData>().GetComponent<AbilityData>();
				if (abilityData != null)
				{
					m_abilitySelectorVisible = true;
					HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel.Init(abilityData);
					UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel, true);
				}
			}
			if (m_abilitySelectorVisible)
			{
				float lineSize;
				if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY) == 0f &&
					ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX) == 0f)
				{
					lineSize = 0f;
				}
				else
				{
					lineSize = 200f;
				}

				RectTransform rectTransform = (HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel.GetComponent<UIAbilitySelectPanel>().m_line.transform as RectTransform);
				rectTransform.sizeDelta = new Vector2(lineSize, 2f);
				rectTransform.pivot = new Vector2(0f, 0.5f);
				rectTransform.anchoredPosition = Vector2.zero;
				float angle = Mathf.Atan2(ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY), ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX)) * 57.29578f;
				rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
				HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel.SelectAbilityButtonFromAngle(angle, lineSize);
			}
		}
		else if (m_abilitySelectorVisible)
		{
			m_abilitySelectorVisible = false;
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel, false);
			KeyPreference abilityHover = HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel.GetAbilityHover();
			if (HUD_UI.Get() != null && abilityHover != 0)
			{
				UIMainScreenPanel.Get().m_abilityBar.DoAbilityButtonClick(abilityHover);
			}
		}
	}

	// removed in rogues
	private void CheckPingInput()
	{
		if (GameFlowData.Get().activeOwnedActorData != m_actorData)
		{
			return;
		}
		if (CurrentState != TurnStateEnum.TARGETING_ACTION
			&& Input.GetMouseButtonDown(0)
			&& InterfaceManager.Get().ShouldHandleMouseClick()
			&& InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing))
		{
			if (HUD_UI.Get() != null && Board.Get().PlayerFreeSquare != null)
			{
				m_worldPositionPingDown = Board.Get().PlayerFreeSquare.ToVector3();
				m_mousePositionPingDown = Input.mousePosition;
				m_timePingDown = GameTime.time;
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel, true);
				Canvas componentInParent = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponentInParent<Canvas>();
				Vector2 vector = new Vector2(m_mousePositionPingDown.x / (float)Screen.width - 0.5f, m_mousePositionPingDown.y / (float)Screen.height - 0.5f);
				Vector2 sizeDelta = (componentInParent.transform as RectTransform).sizeDelta;
				Vector2 anchoredPosition = new Vector2(vector.x * sizeDelta.x, vector.y * sizeDelta.y);
				(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.transform as RectTransform).anchoredPosition = anchoredPosition;
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_closeButton, false);
			}
			return;
		}
		if (m_timePingDown == 0f)
		{
			return;
		}
		RectTransform rectTransform = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_line.transform as RectTransform;
		Canvas componentInParent2 = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponentInParent<Canvas>();
		Vector2 vector2 = new Vector2(m_mousePositionPingDown.x / (float)Screen.width, m_mousePositionPingDown.y / (float)Screen.height);
		Vector2 sizeDelta2 = (componentInParent2.transform as RectTransform).sizeDelta;
		Vector2 b = new Vector2(vector2.x * sizeDelta2.x, vector2.y * sizeDelta2.y);
		Vector3 mousePosition = Input.mousePosition;
		float x = mousePosition.x / (float)Screen.width;
		Vector3 mousePosition2 = Input.mousePosition;
		Vector2 vector3 = new Vector2(x, mousePosition2.y / (float)Screen.height);
		Vector2 a = new Vector2(vector3.x * sizeDelta2.x, vector3.y * sizeDelta2.y);
		Vector2 vector4 = a - b;
		rectTransform.sizeDelta = new Vector2(vector4.magnitude, 2f);
		rectTransform.pivot = new Vector2(0f, 0.5f);
		rectTransform.anchoredPosition = Vector2.zero;
		float z = Mathf.Atan2(vector4.y, vector4.x) * 57.29578f;
		rectTransform.rotation = Quaternion.Euler(0f, 0f, z);
		if (GameTime.time < m_timePingDown + 0.25f)
		{
			if (Input.GetMouseButtonUp(0))
			{
				BigPingPanel component = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>();
				ActorController.PingType pingType = component.GetPingType();
				UIManager.SetGameObjectActive(component, false);
				m_timePingDown = 0f;
				HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(m_worldPositionPingDown, pingType);
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			BigPingPanel component2 = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>();
			ActorController.PingType pingType2 = component2.GetPingType();
			UIManager.SetGameObjectActive(component2, false);
			m_timePingDown = 0f;
			if (pingType2 != 0)
			{
				HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(m_worldPositionPingDown, pingType2);
			}
		}
		else if (!HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_closeButton.activeSelf)
		{
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_closeButton, true);
		}
	}

	// removed in rogues
	private void CheckAbilityInputControlPad()
	{
		if (GameFlowData.Get().activeOwnedActorData != GetComponent<ActorData>()
			|| ControlpadGameplay.Get() == null
			|| !ControlpadGameplay.Get().UsingControllerInput)
		{
			return;
		}
		if (m_timePingDown == 0f
			&& ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftTrigger) > 0f)
		{
			if (HUD_UI.Get() != null && Board.Get().PlayerFreeSquare != null)
			{
				m_timePingDown = GameTime.time;
				HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.Init();
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad, true);
				m_worldPositionPingDown = Board.Get().PlayerFreeSquare.ToVector3();
				Canvas componentInParent = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponentInParent<Canvas>();
				Vector2 sizeDelta = (componentInParent.transform as RectTransform).sizeDelta;
				Vector3 position = Board.Get().PlayerClampedSquare.ToVector3();
				if (Board.Get().PlayerClampedSquare.height < 0)
				{
					position.y = Board.Get().BaselineHeight;
				}
				Canvas x = (!(HUD_UI.Get() != null)) ? null : HUD_UI.Get().GetTopLevelCanvas();
				if (x != null)
				{
					Vector2 vector = Camera.main.WorldToViewportPoint(position);
					Vector2 anchoredPosition = new Vector2(vector.x * sizeDelta.x - sizeDelta.x * 0.5f, vector.y * sizeDelta.y - sizeDelta.y * 0.5f);
					(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.transform as RectTransform).anchoredPosition = anchoredPosition;
				}
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_closeButton, false);
			}
		}
		else if (m_timePingDown != 0f)
		{
			RectTransform rectTransform = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_line.transform as RectTransform;
			float lineSize;
			if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY) == 0f && ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX) == 0f)
			{
				lineSize = 0f;
			}
			else
			{
				lineSize = 200f;
			}
			rectTransform.sizeDelta = new Vector2(lineSize, 2f);
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.anchoredPosition = Vector2.zero;
			float num3 = Mathf.Atan2(ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY), ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX)) * 57.29578f;
			rectTransform.rotation = Quaternion.Euler(0f, 0f, num3);
			HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.SelectAbilityButtonFromAngle(num3, lineSize);
			if (GameTime.time < m_timePingDown + 0.25f)
			{
				if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftTrigger) == 0f)
				{
					ActorController.PingType pingType = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetPingType();
					UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad, false);
					m_timePingDown = 0f;
					HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(m_worldPositionPingDown, pingType);
				}
			}
			else if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftTrigger) == 0f)
			{
				ActorController.PingType pingType2 = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetPingType();
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad, false);
				m_timePingDown = 0f;
				if (pingType2 != ActorController.PingType.Default)
				{
					HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(m_worldPositionPingDown, pingType2);
				}
			}
			else if (!HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_closeButton.activeSelf)
			{
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_closeButton, true);
			}
		}
	}

	private void Update()
	{
		UpdateStates();

		// removed in rogues
		UpdateCancelKey();
		if (LockInBuffered)
		{
			if (CheckStateForEndTurnRequestFromInput())
			{
				Log.Info("Buffered lock in at " + GameTime.time);
				UISounds.GetUISounds().Play("ui/ingame/v1/hud/lockin");
				RequestEndTurn();
				if (HUD_UI.Get() != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.LockedInClicked();
				}
				LockInBuffered = false;
			}
			if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
			{
				Log.Info("Stop lock in buffer during resolve");
				LockInBuffered = false;
			}
		}
		CheckAbilityInput();
		CheckPingInput();
		CheckAbilityInputControlPad();
		// end removed in rogues

		DisplayActorState();
		if (m_firstUpdate)
		{
			m_firstUpdate = false;
		}
	}

	// removed in rogues
	private void LateUpdate()
	{
		HandledSpaceInput = false;
		HandledMouseInput = false;
	}

	private void UpdateStates()
	{
		// if (!GameFlowData.Get().GetPause())  // check added in rogues
		// {
		do
		{
			SwitchToNewStates();
			GetState().Update();
		}
		while (NextState != CurrentState);
		// }
	}

	// added in rogues
#if SERVER
	public bool IsClientActor()
	{
		return Owner != null && Owner == GameFlowData.ClientActor;
	}
#endif

	[Client]
	internal void SendCastAbility(AbilityData.ActionType actionType)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ActorTurnSM::SendCastAbility(AbilityData/ActionType)' called on server");
			return;
		}
		GameFlow.Get().SendCastAbility(GetComponent<ActorData>(), actionType, GetAbilityTargets());
	}

	internal void RequestCancel(bool onlyCancelConfirm = false)
	{
		if (SinglePlayerManager.IsCancelDisabled())
		{
			return;
		}
		bool flag = false;
		switch (CurrentState)
		{
			case TurnStateEnum.DECIDING_MOVEMENT: // removed in rogues
			case TurnStateEnum.DECIDING:
				flag = true;
				break;
			case TurnStateEnum.CONFIRMED:
				if (m_actorData.GetTimeBank().AllowUnconfirm())
				{
					LastConfirmedCancelTurn = GameFlowData.Get().CurrentTurn;
					m_actorData.GetTimeBank().OnActionsUnconfirmed();
					BackToDecidingState();
					if (Options_UI.Get() != null && Options_UI.Get().ShouldCancelActionWhileConfirmed())
					{
						flag = true;
					}
				}
				break;
			default:
				BackToDecidingState();
				break;
		}
		if (!onlyCancelConfirm && flag && m_requestStackForUndo.Count != 0)
		{
			int index = m_requestStackForUndo.Count - 1;
			ActionRequestForUndo actionRequestForUndo = m_requestStackForUndo[index];
			m_requestStackForUndo.RemoveAt(index);
			m_actorData.OnClientQueuedActionChanged(); // removed in rogues
			UndoableRequestType type = actionRequestForUndo.m_type;
			if (type == UndoableRequestType.ABILITY_QUEUE)
			{
				RequestCancelAction(actionRequestForUndo.m_action, false);
				UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
			}
			else if (type == UndoableRequestType.MOVEMENT)
			{
				RequestCancelMovement();
				UISounds.GetUISounds().Play("ui/ingame/v1/move_undo");
			}
		}
		Board.Get().MarkForUpdateValidSquares();
	}

	public void BackToDecidingState()
	{
		if (NetworkServer.active || (NetworkClient.active && CurrentState == TurnStateEnum.TARGETING_ACTION))
		{
			OnMessage(TurnMessage.CANCEL_BUTTON_CLICKED, false);
		}
		if (!NetworkServer.active)
		{
			// reactor
			CallCmdGUITurnMessage((int)TurnMessage.CANCEL_BUTTON_CLICKED, 0);
			// rogues
			//CallCmdGUITurnMessage(2, 0);  // was 4 in reactor
		}
	}

	// removed in rogues
	private void UpdateCancelKey()
	{
		if (!(GameFlowData.Get().activeOwnedActorData == GetComponent<ActorData>()))
		{
			return;
		}
		ActorData component = GetComponent<ActorData>();
		bool hasQueuedMovement = component.HasQueuedMovement();
		bool hasQueuedAbilities = GetComponent<AbilityData>().HasQueuedAbilities();
		bool canCancel = true;
		if (InputManager.Get().IsKeyCodeMatchKeyBind(KeyPreference.CancelAction, KeyCode.Escape)
			&& Input.GetKeyDown(KeyCode.Escape))
		{
			if (UISystemEscapeMenu.Get() != null && UISystemEscapeMenu.Get().IsOpen())
			{
				canCancel = false;
				UISystemEscapeMenu.Get().OnToggleButtonClick(null);
			}
			else if (UIGameStatsWindow.Get() != null && UIGameStatsWindow.Get().m_container.gameObject.activeSelf)
			{
				canCancel = false;
				UIGameStatsWindow.Get().ToggleStatsWindow();
			}
			else if (Options_UI.Get().IsVisible())
			{
				canCancel = false;
				Options_UI.Get().HideOptions();
			}
			else if (KeyBinding_UI.Get().IsVisible())
			{
				canCancel = false;
				if (!KeyBinding_UI.Get().IsSettingKeybindCommand())
				{
					KeyBinding_UI.Get().HideKeybinds();
				}
			}
		}
		if ((hasQueuedMovement || hasQueuedAbilities || CurrentState != TurnStateEnum.DECIDING)
			&& canCancel
			&& InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CancelAction))
		{
			RequestCancel();
		}
	}

	public bool CheckStateForEndTurnRequestFromInput()
	{
		return CurrentState == TurnStateEnum.DECIDING && NextState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.PICKING_RESPAWN;
	}

	// removed in rogues
	public void UpdateEndTurnKey()
	{
		if (GameFlowData.Get().activeOwnedActorData == GetComponent<ActorData>()
			&& ShouldShowEndTurnButton()
			&& ShouldEnableEndTurnButton()
			&& InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.LockIn)
			&& !HandledSpaceInput
			&& !UITutorialFullscreenPanel.Get().IsAnyPanelVisible()
			&& AppState.GetCurrent() != AppState_InGameDeployment.Get())
		{
			if (CheckStateForEndTurnRequestFromInput())
			{
				HandledSpaceInput = true;
				UISounds.GetUISounds().Play("ui/ingame/v1/hud/lockin");
				Log.Info("Lock in request at " + GameTime.time);
				RequestEndTurn();
				if (HUD_UI.Get() != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.LockedInClicked();
				}
			}
			else if (CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST || CurrentState == TurnStateEnum.VALIDATING_MOVE_REQUEST)
			{
				LockInBuffered = true;
				Log.Info("Lockin to be buffered at " + GameTime.time);
			}
		}
	}

	// removed in rogues
	public void GetActionText(out string textStr, out Color textColor)
	{
		Ability ability = GetComponent<AbilityData>()?.GetSelectedAbility();
		if (CurrentState == TurnStateEnum.CONFIRMED)
		{
			if (ability != null)
			{
				textColor = Color.green;
				textStr = ability.m_abilityName;
			}
			else
			{
				ActorData actorData = GetComponent<ActorData>();
				bool hasQueuedMovement = actorData.HasQueuedMovement();
				if (actorData.HasQueuedChase())
				{
					textColor = s_chasingTextColor;
					textStr = "Chasing";
				}
				else if (hasQueuedMovement)
				{
					textColor = s_movingTextColor;
					textStr = "Moving";
				}
				else
				{
					textColor = s_movingTextColor;
					textStr = "Staying";
				}
			}
		}
		else if (CurrentState == TurnStateEnum.DECIDING || CurrentState == TurnStateEnum.DECIDING_MOVEMENT)
		{
			textColor = s_decidingTextColor;
			textStr = "(Deciding...)";
		}
		else
		{
			textColor = s_decidingTextColor;
			textStr = "";
		}
	}

	// reactor
	public void RequestEndTurn()
	{
		ActorData component = GetComponent<ActorData>();
		if (AmTargetingAction())
		{
			Ability selectedAbility = component.GetAbilityData().GetSelectedAbility();
			do
			{
				if (SelectTarget(null, true))
				{
					NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
				}
			} while (selectedAbility != null
			&& selectedAbility.ShouldAutoConfirmIfTargetingOnEndTurn()
			&& m_targets.Count < selectedAbility.GetExpectedNumberOfTargeters());
		}
		if (SinglePlayerManager.Get() != null)
		{
			SinglePlayerManager.Get().OnEndTurnRequested(component);
		}
		if (SinglePlayerManager.CanEndTurn(component))
		{
			if (NetworkServer.active)
			{
				OnMessage(TurnMessage.DONE_BUTTON_CLICKED);
			}
			else
			{
				CallCmdGUITurnMessage((int)TurnMessage.DONE_BUTTON_CLICKED, 0);
			}
			GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterLocked, null);
			if (component == GameFlowData.Get().activeOwnedActorData)
			{
				GameFlowData.Get().SetActiveNextNonConfirmedOwnedActorData();
			}
		}
		Board.Get().MarkForUpdateValidSquares();
	}

	// rogues
	//public void RequestEndTurn(bool pveAllDone)
	//{
	//	m_fsmInternal.RequestEndTurn(pveAllDone);
	//}

	// rogues
	//public void ExecuteQueuedRequests()
	//{
	//	m_fsmInternal.ExecuteQueuedRequests();
	//}

	// added in rogues
#if SERVER
	public void FillRemainingTargetDataOnLockIn()
	{
		if (AmTargetingAction())
		{
			Ability selectedAbility = m_actorData.GetAbilityData().GetSelectedAbility();
			do
			{
				if (SelectTarget(null, true))
				{
					NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
				}
			}
			while (selectedAbility != null
			&& selectedAbility.ShouldAutoConfirmIfTargetingOnEndTurn()
			&& GetAbilityTargets().Count < selectedAbility.GetExpectedNumberOfTargeters());
		}
	}
#endif

	public void RequestCancelMovement()
	{
		if (SinglePlayerManager.IsCancelDisabled())
		{
			return;
		}
		if (NetworkServer.active)
		{
			OnMessage(TurnMessage.CANCEL_MOVEMENT, 0);
		}
		else
		{
			// reactor
			CallCmdGUITurnMessage((int)TurnMessage.CANCEL_MOVEMENT, 0);
			// rogues
			//CallCmdGUITurnMessage(14, 0);  // 16 in reactor
		}
		ActorData component = GetComponent<ActorData>();
		if (component == GameFlowData.Get().activeOwnedActorData // IsClientActor() in rogues
			&& component.IsActorInvisibleForRespawn()
			&& SpawnPointManager.Get() != null
			// reactor
			&& SpawnPointManager.Get().m_spawnInDuringMovement)
		// rogues
		//&& SpawnPointManager.Get().SpawnInDuringMovement())
		{
			InterfaceManager.Get().DisplayAlert(StringUtil.TR("PostRespawnMovement", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true);
		}
		if (NetworkClient.active && component == GameFlowData.Get().activeOwnedActorData) // IsClientActor() in rogues
		{
			LineData component2 = component.GetComponent<LineData>();
			if (component2 != null)
			{
				component2.OnClientRequestedMovementChange();
			}
		}
	}

	public void RequestCancelAction(AbilityData.ActionType actionType, bool hasPendingStoreRequest)
	{
		if (SinglePlayerManager.IsCancelDisabled())
		{
			return;
		}
		CancelUndoableAbilityRequest(actionType);
		CancelAutoQueuedAbilityRequest(actionType);
		CallCmdRequestCancelAction((int)actionType, hasPendingStoreRequest);
	}

	public void RequestMove()
	{
		StoreUndoableActionRequest(new ActionRequestForUndo(UndoableRequestType.MOVEMENT));
		if (NetworkServer.active)
		{
			OnMessage(TurnMessage.MOVE_BUTTON_CLICKED);
		}
		else
		{
			// reactor
			CallCmdGUITurnMessage((int)TurnMessage.MOVE_BUTTON_CLICKED, 0);
			// rogues
			//CallCmdGUITurnMessage(11, 0);  // 13 in reactor
		}
	}

	private void DisplayActorState()
	{
	}

	private void SwitchToNewStates()
	{
		if (NextState == CurrentState)
		{
			return;
		}
		GetState().OnExit();
		PreviousState = CurrentState;
		CurrentState = NextState;
		// reactor
		if (UIMainScreenPanel.Get() != null
			&& GameFlowData.Get().activeOwnedActorData != null
			&& GameFlowData.Get().activeOwnedActorData == m_actorData)
		// rogues
		//if (UIMainScreenPanel.Get() != null && GameFlowData.ClientActor == m_actorData)
		{
			if (CurrentState == TurnStateEnum.TARGETING_ACTION)
			{
				UIMainScreenPanel.Get().m_targetingCursor.ShowTargetCursor();
			}
			else
			{
				UIMainScreenPanel.Get().m_targetingCursor.HideTargetCursor();
				Cursor.visible = true;
			}
		}
		GetState().OnEnter();
		if (CameraManager.Get() != null)
		{
			CameraManager.Get().OnNewTurnSMState();
		}
		if (Board.Get() != null)
		{
			Board.Get().MarkForUpdateValidSquares();
		}
	}

	public void ResetTurnStartNow()
	{
		_TurnStart = DateTime.UtcNow;
	}

	// removed in rogues
	public bool IsKeyDown(KeyCode keyCode)
	{
		return CameraControls.Get().Enabled && Input.GetKeyDown(keyCode);
	}

	// rogues
	//public void OnDeath()
	//{
	//	this.OnMessage(TurnMessage.DIED, true);
	//}

	public List<AbilityData.ActionType> GetAutoQueuedRequestActionTypes()
	{
		return m_autoQueuedRequestActionTypes;
	}

	public void StoreAutoQueuedAbilityRequest(AbilityData.ActionType actionType)
	{
		if (!m_autoQueuedRequestActionTypes.Contains(actionType))
		{
			m_autoQueuedRequestActionTypes.Add(actionType);
		}
	}

	private void CancelAutoQueuedAbilityRequest(AbilityData.ActionType actionType)
	{
		if (m_autoQueuedRequestActionTypes.Contains(actionType))
		{
			m_autoQueuedRequestActionTypes.Remove(actionType);
		}
	}

	public List<ActionRequestForUndo> GetRequestStackForUndo()
	{
		return m_requestStackForUndo;
	}

	private void StoreUndoableActionRequest(ActionRequestForUndo request)
	{
		bool flag = false;
		if (request.m_type == UndoableRequestType.MOVEMENT)
		{
			foreach (ActionRequestForUndo current in m_requestStackForUndo)
			{
				if (current.m_type == UndoableRequestType.MOVEMENT)
				{
					flag = true;
				}
			}
		}
		if (!flag)
		{
			m_requestStackForUndo.Add(request);
			m_actorData.OnClientQueuedActionChanged();  // removed in rogues
		}
	}

	private void CancelUndoableAbilityRequest(AbilityData.ActionType actionType)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData; // GameFlowData.ClientActor in rogues
		if (activeOwnedActorData != null
			&& actionType != AbilityData.ActionType.INVALID_ACTION
			&& activeOwnedActorData.GetComponent<ActorCinematicRequests>().IsAbilityCinematicRequested(actionType))
		{
			activeOwnedActorData.GetComponent<ActorCinematicRequests>().SendAbilityCinematicRequest(actionType, false, -1, -1);
		}
		int num = -1;
		for (int i = 0; i < m_requestStackForUndo.Count; i++)
		{
			if (m_requestStackForUndo[i].m_type == UndoableRequestType.ABILITY_QUEUE
				&& m_requestStackForUndo[i].m_action == actionType)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			m_requestStackForUndo.RemoveAt(num);
			m_actorData.OnClientQueuedActionChanged(); // removed in rogues
			if (HUD_UI.Get() != null)
			{
				HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.CancelAbilityRequest(actionType);
			}
		}
	}

	internal void OnQueueAbilityRequest(AbilityData.ActionType actionType)
	{
		AbilityData abilityData = GetComponent<AbilityData>();
		List<AbilityData.ActionType> actionsToCancel = null;
		bool cancelMovement = false;
		if (abilityData.GetActionsToCancelOnTargetingComplete(ref actionsToCancel, ref cancelMovement))
		{
			if (cancelMovement)
			{
				RequestCancelMovement();
			}
			if (actionsToCancel != null)
			{
				foreach (AbilityData.ActionType item in actionsToCancel)
				{
					RequestCancelAction(item, true);
				}
				if (actionsToCancel.Count > 0)
				{
					UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
				}
			}
			abilityData.ClearActionsToCancelOnTargetingComplete();
		}
		StoreUndoableActionRequest(new ActionRequestForUndo(UndoableRequestType.ABILITY_QUEUE, actionType));
	}

	[Server]
	internal void QueueAutoQueuedAbilityRequest(AbilityData.ActionType actionType)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorTurnSM::QueueAutoQueuedAbilityRequest(AbilityData/ActionType)' called on client");
			return;
		}
		StoreAutoQueuedAbilityRequest(actionType);
		CallRpcStoreAutoQueuedAbilityRequest((int)actionType);
	}

	// added in rogues
	private TurnState GetState()
	{
		// reactor
		return m_turnStates[(int)CurrentState];
		// rogues
		//return m_fsmInternal.GetState(CurrentState);
	}

	public void ClearAbilityTargets()
	{
		m_targets.Clear();
	}

	public void AddAbilityTarget(AbilityTarget newTarget)
	{
		m_targets.Add(newTarget);
	}

	public List<AbilityTarget> GetAbilityTargets()
	{
		return m_targets;
	}

	public void OnMessage(TurnMessage msg, bool ignoreClient = true)
	{
		OnMessage(msg, 0, ignoreClient);
	}

	public void OnMessage(TurnMessage msg, int extraData, bool ignoreClient = true)
	{
		if (NetworkServer.active)
		{
			CallRpcTurnMessage((int)msg, extraData);
		}
		if (NetworkServer.active
			|| NetworkClient.active && !ignoreClient)
		{
			// rogues
			//if (!m_fsmInternal.HandleMsg(msg, extraData))
			//{
			GetState().OnMsg(msg, extraData);
			//}
			UpdateStates();
		}
		
#if SERVER
		// custom
		if (NetworkServer.active
		    && (msg == TurnMessage.DONE_BUTTON_CLICKED
				|| msg == TurnMessage.CANCEL_BUTTON_CLICKED))
		{
			GameFlowData.Get().UpdateTimeRemainingOverflow();
		}
#endif
	}

	[Command]
	private void CmdGUITurnMessage(int msgEnum, int extraData)  // public in rogues
	{
#if SERVER
		if (msgEnum != (int)TurnMessage.MOVEMENT_RESOLVED
			&& msgEnum != (int)TurnMessage.PICKED_RESPAWN
			&& msgEnum != (int)TurnMessage.DONE_BUTTON_CLICKED
			&& msgEnum != (int)TurnMessage.MOVE_BUTTON_CLICKED
			&& msgEnum != (int)TurnMessage.CANCEL_BUTTON_CLICKED // custom
			&& msgEnum != (int)TurnMessage.CANCEL_MOVEMENT // custom
			&& msgEnum != (int)TurnMessage.CANCEL_SINGLE_ABILITY // custom
			&& msgEnum != (int)TurnMessage.PICK_RESPAWN)
		{
			Debug.LogError(string.Format("HACK ATTEMPT: Client sent invalid TurnMessage enum value for CmdGUITurnMessage - {0}", msgEnum));
		}
		OnMessage((TurnMessage)msgEnum, extraData, true);
#endif
	}

	[Command]
	private void CmdRequestCancelAction(int action, bool hasIncomingRequest)
	{
		// empty in reactor
#if SERVER
		if (NetworkServer.active
			&& (CurrentState == TurnStateEnum.DECIDING
				|| CurrentState == TurnStateEnum.TARGETING_ACTION))
		{
			base.GetComponent<ServerActorController>().ProcessCancelActionRequest((AbilityData.ActionType)action, hasIncomingRequest);
		}
#endif
	}

	// rogues
	//[Command]
	//public void CmdLockInRequested()
	//{
	//	if (NetworkServer.active)
	//	{
	//		m_fsmInternal.HandleLockInRequestedOnServer();
	//	}
	//}

	// rogues
	//[Command]
	//public void CmdExecuteQueuedRequests()
	//{
	//	if (NetworkServer.active)
	//	{
	//		m_fsmInternal.HandleExecuteQueuedRequestsOnServer();
	//	}
	//}

	public void OnActionsConfirmed()
	{
		if (m_actorData.GetTimeBank() != null)
		{
			m_actorData.GetTimeBank().OnActionsConfirmed();
		}
	}

	public void OnActionsUnconfirmed()
	{
		if (m_actorData.GetTimeBank() != null)
		{
			m_actorData.GetTimeBank().OnActionsUnconfirmed();
		}
	}

	public void OnSelectedAbilityChanged(Ability ability)
	{
		if (ability != null
			&& NetworkClient.active
			&& CanSelectAbility()
			&& !ability.IsAutoSelect())
		{
			OnMessage(TurnMessage.SELECTED_ABILITY, false);
		}
		GetState().OnSelectedAbilityChanged();
		if (Board.Get() != null)
		{
			Board.Get().MarkForUpdateValidSquares();
		}
	}

	public void SelectMovementSquare()
	{
		BoardSquare playerClampedSquare = Board.Get().PlayerClampedSquare;
		ActorData actorData = GetComponent<ActorData>();
		BoardSquare boardSquare = actorData?.MoveFromBoardSquare;

		// reactor
		bool isWaypoint = Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier);
		bool isExplicitWaypoint = Options_UI.Get().GetShiftClickForMovementWaypoints() && InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier);
		// rogues
		//bool isWaypoint = true;
		//bool isExplicitWaypoint = false;

		// added in rogues
		//bool forceDelayExecution = InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier);

		if (boardSquare != playerClampedSquare)
		{
			InterfaceManager.Get().CancelAlert(StringUtil.TR("PostRespawnMovement", "Global"));
		}
		if (playerClampedSquare != null && SinglePlayerManager.IsDestinationAllowed(actorData, playerClampedSquare, isWaypoint))
		{
			if (!m_actorData.HasQueuedMovement() && !m_actorData.HasQueuedChase())
			{
				if (isExplicitWaypoint || !SelectMovementSquareForChasing(playerClampedSquare))
				{
					SelectMovementSquareForMovement(playerClampedSquare);  // , forceDelayExecution in rogues
				}
			}
			else if (m_actorData.HasQueuedChase())
			{
				if (playerClampedSquare == m_actorData.GetQueuedChaseTarget().GetCurrentBoardSquare())
				{
					SelectMovementSquareForMovement(playerClampedSquare);  // , forceDelayExecution in rogues
				}
				else if (!SelectMovementSquareForChasing(playerClampedSquare))
				{
					SelectMovementSquareForMovement(playerClampedSquare);  // , forceDelayExecution in rogues
				}
			}
			else
			{
				bool flag3 = (!isWaypoint || !actorData.CanMoveToBoardSquare(playerClampedSquare)) && SelectMovementSquareForChasing(playerClampedSquare);
				if (playerClampedSquare == boardSquare || flag3)
				{
					SelectMovementSquareForChasing(playerClampedSquare);
				}
				else
				{
					SelectMovementSquareForMovement(playerClampedSquare);  // , forceDelayExecution in rogues
				}
			}
		}
	}

	public bool SelectMovementSquareForChasing(BoardSquare selectedSquare)
	{
		// reactor
		bool result = false;
		ActorData actor = GetComponent<ActorData>();
		if (actor.IsSquareChaseableByClient(selectedSquare))
		{
			ActorData occupant = selectedSquare.occupant.GetComponent<ActorData>();
			result = true;
			if (!m_actorData.HasQueuedChase() || m_actorData.GetQueuedChaseTarget() != occupant)
			{
				if (actor == GameFlowData.Get().activeOwnedActorData)
				{
					UISounds.GetUISounds().Play("ui/ingame/v1/teammember_move");
				}
				StoreUndoableActionRequest(new ActionRequestForUndo(UndoableRequestType.MOVEMENT));
				CallCmdChase(selectedSquare.x, selectedSquare.y);
				NextState = TurnStateEnum.VALIDATING_MOVE_REQUEST;
				Log.Info(string.Concat("Setting State to ", NextState, " at ", GameTime.time));
				if (NetworkClient.active && actor == GameFlowData.Get().activeOwnedActorData)
				{
					actor.GetComponent<LineData>()?.OnClientRequestedMovementChange();
				}
			}
		}
		return result;
		// rogues
		//return false;
	}

	[Command]
	private void CmdChase(int selectedSquareX, int selectedSquareY)
	{
		// empty in reactor
#if SERVER
		GetComponent<ServerActorController>().ProcessChaseRequest(selectedSquareX, selectedSquareY);
#endif
	}

	public void SelectMovementSquareForMovement(BoardSquare selectedSquare)  // , bool forceDelayExecution in rogues
	{
		// reactor
		List<BoardSquare> list = new List<BoardSquare>();
		list.Add(selectedSquare);
		SelectMovementSquaresForMovement(list);
		// rogues
		//SelectMovementSquaresForMovement(new List<BoardSquare>
		//{
		//	selectedSquare
		//}, forceDelayExecution);
	}

	public void SelectMovementSquaresForMovement(List<BoardSquare> selectedSquares)  // , bool forceDelayExecution in rogues
	{
		ActorData actorData = GetComponent<ActorData>();
		if (GameFlowData.Get() == null || !GameFlowData.Get().IsInDecisionState())
		{
			return;
		}
		if (SinglePlayerManager.Get() != null
			&& SinglePlayerManager.Get().GetCurrentState() != null
			&& actorData.IsHumanControlled()
			&& SinglePlayerManager.Get().GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
		{
			return;
		}
		bool flag = false;
		int num = 0;
		foreach (BoardSquare current in selectedSquares)
		{
			BoardSquare boardSquare = current;
			if (!actorData.CanMoveToBoardSquare(boardSquare))
			{
				boardSquare = actorData.GetActorMovement().GetClosestMoveableSquareTo(boardSquare, false);
			}
			if (boardSquare != null)
			{
				if (actorData == GameFlowData.Get().activeOwnedActorData // IsClientActor() in rogues
					&& num == 0
					&& actorData.GetActorMovement().SquaresCanMoveTo.Count > 0)
				{
					UISounds.GetUISounds().Play("ui/ingame/v1/move");
				}

				// reactor
				bool isWaypoint = Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier)
					&& FirstTurnMovement.CanWaypoint();
				// rogues
				//bool isWaypoint = true;

				StoreUndoableActionRequest(new ActionRequestForUndo(UndoableRequestType.MOVEMENT));
#if SERVER
				if (NetworkServer.active)
				{
					GetComponent<ServerActorController>().ProcessSetSquareRequest(boardSquare.x, boardSquare.y, isWaypoint);  // , forceDelayExecution in rogues
				}
				else if (NetworkClient.active)
				{
					CallCmdSetSquare(boardSquare.x, boardSquare.y, isWaypoint);  // , forceDelayExecution in rogues
					flag = true;
				}
#else
				CallCmdSetSquare(boardSquare.x, boardSquare.y, isWaypoint);
				flag = true;
#endif
			}
			num++;
		}
		if (flag)
		{
			NextState = TurnStateEnum.VALIDATING_MOVE_REQUEST;
			Log.Info(string.Concat("Setting State to ", NextState, " at ", GameTime.time));
			if (NetworkClient.active && actorData == GameFlowData.Get().activeOwnedActorData)  // IsClientActor() in rogues
			{
				LineData component2 = actorData.GetComponent<LineData>();
				if (component2 != null)
				{
					component2.OnClientRequestedMovementChange();
				}
			}
		}
		Board.Get().MarkForUpdateValidSquares();
	}

	[Command]
	private void CmdSetSquare(int x, int y, bool setWaypoint) // , bool forceDelayExecution in rogues
	{
		// empty in reactor
#if SERVER
		GetComponent<ServerActorController>().ProcessSetSquareRequest(x, y, setWaypoint);  // , forceDelayExecution in rogues
#endif
	}

	// rogues
	//public void ProcessGroupMoveRequestForOwnedActors(BoardSquare meetingSquare)
	//{
	//	if (meetingSquare != null && meetingSquare.IsValidForGameplay())
	//	{
	//		int x = meetingSquare.x;
	//		int y = meetingSquare.y;
	//		if (NetworkServer.active)
	//		{
	//			base.GetComponent<ServerActorController>().ProcessGroupMoveRequestForOwnedActors(m_actorData.ActorIndex, x, y);
	//			return;
	//		}
	//		CallCmdGroupMoveToSquare(m_actorData.ActorIndex, x, y);
	//	}
	//}

	// rogues
	//[Command]
	//private void CmdGroupMoveToSquare(int actorIndex, int x, int y)
	//{
	//	base.GetComponent<ServerActorController>().ProcessGroupMoveRequestForOwnedActors(actorIndex, x, y);
	//}

	[ClientRpc]
	private void RpcTurnMessage(int msgEnum, int extraData)
	{
		// reactor
		if (!m_actorData.HasBotController
			&& m_actorData == GameFlowData.Get().activeOwnedActorData
			&& !m_actorData.IsDead())
		{
			if (msgEnum == (int)TurnMessage.BEGIN_RESOLVE
				&& GetState() != m_turnStates[(int)TurnStateEnum.DECIDING]
				&& GetState() != m_turnStates[(int)TurnStateEnum.TARGETING_ACTION]
				&& GetState() != m_turnStates[(int)TurnStateEnum.CONFIRMED]
				&& GetState() != m_turnStates[(int)TurnStateEnum.WAITING])
			{
				if (m_requestStackForUndo.IsNullOrEmpty() && m_autoQueuedRequestActionTypes.IsNullOrEmpty())
				{
					int lastTargetIndex = -1;
					string text = "(none)";
					ActorController actorController = m_actorData.GetActorController();
					if (actorController != null)
					{
						Ability lastTargetedAbility = actorController.GetLastTargetedAbility(ref lastTargetIndex);
						if (lastTargetedAbility != null)
						{
							text = lastTargetedAbility.m_abilityName;
						}
					}
					Debug.LogError("Player " + m_actorData.DisplayName + " skipped turn (could be AFK) in client ActorTurnSM state " + GetState().GetType().ToString() + ". LastTargetedAbility: " + text + ", targeterIndex: " + lastTargetIndex + ". GuiUtility.hotControl: " + GUIUtility.hotControl);
				}
			}
			else if (msgEnum == (int)TurnMessage.TURN_START
				&& GetState() != m_turnStates[(int)TurnStateEnum.WAITING]
				&& GetState() != m_turnStates[(int)TurnStateEnum.DECIDING]
				&& GetState() != m_turnStates[(int)TurnStateEnum.RESPAWNING]
				&& GetState() != m_turnStates[(int)TurnStateEnum.CONFIRMED]
				&& GetState() != m_turnStates[(int)TurnStateEnum.RESOLVING])
			{
				Debug.LogError("Player " + m_actorData.DisplayName + " received TURN_START in client ActorTurnSM state " + GetState().GetType().ToString() + " which doesn't handle that transition.");
			}
		}
		GetState().OnMsg((TurnMessage)msgEnum, extraData);
		// rogues
		//if (!m_fsmInternal.HandleMsg((TurnMessage)msgEnum, extraData))
		//{
		//	GetState().OnMsg((TurnMessage)msgEnum, extraData);
		//}

		UpdateStates();
	}

	[ClientRpc]
	private void RpcStoreAutoQueuedAbilityRequest(int actionTypeInt)
	{
		if (!NetworkServer.active)
		{
			StoreAutoQueuedAbilityRequest((AbilityData.ActionType)actionTypeInt);
		}
	}

	// rogues
	//[ClientRpc]
	//private void RpcSetNumRespawnPickInputs(int count)
	//{
	//	Networkm_numRespawnPicksThisTurn = (short)count;
	//}

	// rogues
	//[ClientRpc]
	//public void RpcResetUsedAbilityAndMoveData()
	//{
	//	if (!NetworkServer.active)
	//	{
	//		ResetUsedAbilityAndMoveData();
	//	}
	//}

	public int GetTargetSelectionIndex()
	{
		int result = -1;
		TurnState state = GetState();
		if (state is TargetingActionState)
		{
			TargetingActionState targetingActionState = state as TargetingActionState;
			result = targetingActionState.TargetIndex;
		}
		return result;
	}

	internal Ability.TargetingParadigm GetSelectedTargetingParadigm()
	{
		Ability.TargetingParadigm result = Ability.TargetingParadigm.Position;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null && activeOwnedActorData.GetActorTurnSM() == this)  // if (IsClientActor()) in rogues
		{
			// reactor
			AbilityData abilityData = activeOwnedActorData.GetAbilityData();
			// rogues
			//AbilityData abilityData = Owner.GetAbilityData();
			Ability selectedAbility = abilityData.GetSelectedAbility();
			if (selectedAbility != null)
			{
				int targetSelectionIndex = GetTargetSelectionIndex();
				result = selectedAbility.GetTargetingParadigm(targetSelectionIndex);
			}
		}
		return result;
	}

	public bool CanSelectAbility()
	{
		// reactor
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.CONFIRMED && m_actorData.GetTimeBank().AllowUnconfirm();
		// rogues
		//return m_fsmInternal.CanSelectAbility();
	}

	public bool CanQueueSimpleAction()
	{
		// reactor
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.CONFIRMED && m_actorData.GetTimeBank().AllowUnconfirm();
		// rogues
		//return m_fsmInternal.CanQueueSimpleAction();
	}

	public bool CanPickRespawnLocation()
	{
		// reactor
		return CurrentState == TurnStateEnum.PICKING_RESPAWN
			|| CurrentState == TurnStateEnum.CONFIRMED && PreviousState == TurnStateEnum.PICKING_RESPAWN;
		// rogues
		//return m_fsmInternal.CanPickRespawnLocation();
	}

	public bool AmDecidingMovement()
	{
		// reactor
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.CONFIRMED && m_actorData.GetTimeBank().AllowUnconfirm();
		// rogues
		//return m_fsmInternal.AmDecidingMovement();
	}

	// removed in rogues
	public bool IsAbilityOrPingSelectorVisible()
	{
		return m_abilitySelectorVisible || m_timePingDown != 0f;
	}

	public static bool IsClientDecidingMovement()
	{
		// reactor
		ActorData clientActor = GameFlowData.Get()?.activeOwnedActorData;
		// rogues
		//ActorData clientActor = GameFlowData.ClientActor;
		if (clientActor != null)
		{
			ActorTurnSM actorTurnSM = clientActor.GetActorTurnSM();
			if (actorTurnSM != null)
			{
				return actorTurnSM.AmDecidingMovement();
			}
		}
		return false;
	}

	public bool AmTargetingAction()
	{
		// reactor
		return CurrentState == TurnStateEnum.TARGETING_ACTION;
		// rogues
		//return m_fsmInternal.AmTargetingAction();
	}

	public bool AmStillDeciding()
	{
		// reactor
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.VALIDATING_MOVE_REQUEST
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST
			|| CurrentState == TurnStateEnum.PICKING_RESPAWN;
		// rogues
		//return m_fsmInternal.AmStillDeciding();
	}

	public bool ShouldShowGUIButtons()
	{
		// reactor
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.VALIDATING_MOVE_REQUEST
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST
			|| CurrentState == TurnStateEnum.CONFIRMED
			|| CurrentState == TurnStateEnum.PICKING_RESPAWN;
		// rogues
		//return m_fsmInternal.ShouldShowGUIButtons();
	}

	public bool ShouldEnableEndTurnButton()
	{
		// reactor
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.PICKING_RESPAWN;
		// rogues
		//return m_fsmInternal.ShouldEnableEndTurnButton();
	}

	// removed in rogues
	public bool ShouldEnableMoveButton()
	{
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION;
	}

	public bool ShouldShowEndTurnButton()
	{
		// reactor
		return ShouldShowGUIButtons() && CurrentState != TurnStateEnum.CONFIRMED;
		// rogues
		//return m_fsmInternal.ShouldShowEndTurnButton();
	}

	public bool ShouldEnableAbilityButton(bool isSimpleAction)
	{
		// reactor
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.CONFIRMED && isSimpleAction;
		// rogues
		//return m_fsmInternal.ShouldEnableAbilityButton(isSimpleAction);
	}

	// rogues
	//public bool CanStillActInDecision()
	//{
	//       return m_fsmInternal.CanStillActInDecision();
	//   }

	public void SetupForNewTurn()
	{
		ActorData component = GetComponent<ActorData>();
		if (HUD_UI.Get() != null && component == GameFlowData.Get().activeOwnedActorData)  // IsClientActor() in rogues
		{
			HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay.Decision);
		}
		if (component == GameFlowData.Get().activeOwnedActorData)  // IsClientActor() in rogues
		{
			HighlightUtils.Get().SetCursorType(HighlightUtils.CursorType.MouseOverCursorType);
		}
		component.GetTimeBank().ResetTurn();
		ClearAbilityTargets();
		m_requestStackForUndo.Clear();
		m_autoQueuedRequestActionTypes.Clear();
		if (!NetworkServer.active)
		{
			ActorMovement actorMovement = component.GetActorMovement();
			if (actorMovement && !GameplayUtils.IsMinion(this))  // IsMinion removed in rogues
			{
				actorMovement.UpdateSquaresCanMoveTo();
			}
		}
	}

	// rogues
	//public string GetUsedActionsDebugString()
	//{
	//	string text = "UsedMovement= " + m_pveMoveCostUsedThisTurn + "\n";
	//	text += "Used Abilities:\n";
	//	for (int i = 0; i < m_pvePlayedAbilityFlags.Count; i++)
	//	{
	//		text = string.Concat(new object[]
	//		{
	//			text,
	//			"\tUsed Ability ",
	//			i,
	//			"? ",
	//			m_pvePlayedAbilityFlags[i].ToString(),
	//			"\n"
	//		});
	//	}
	//	return text;
	//}

	// added in rogues
#if SERVER
	public static string GetDebugStateName(TurnStateEnum stateEnum)
	{
		if (Application.isEditor)
		{
			return "<color=white>[ " + stateEnum.ToString() + " ]</color>";
		}
		return "[ " + stateEnum.ToString() + " ]";
	}
#endif

	// added in rogues
#if SERVER
	public static string GetDebugMsgName(TurnMessage msg)
	{
		if (Application.isEditor)
		{
			return "<color=cyan>( " + msg.ToString() + " )</color>";
		}
		return "( " + msg.ToString() + " )";
	}
#endif

	// rogues
	//public ActorTurnSM()
	//{
	//	base.InitSyncObject(m_pvePlayedAbilityFlags);
	//}

	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// rogues
	//public float Networkm_pveMoveCostUsedThisTurn
	//{
	//	get
	//	{
	//		return m_pveMoveCostUsedThisTurn;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<float>(value, ref m_pveMoveCostUsedThisTurn, 1UL);
	//	}
	//}

	// rogues
	//public uint Networkm_pveNumMoveActionsThisTurn
	//{
	//	get
	//	{
	//		return m_pveNumMoveActionsThisTurn;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<uint>(value, ref m_pveNumMoveActionsThisTurn, 2UL);
	//	}
	//}

	// rogues
	//public uint Networkm_pveNumAbilityActionsThisTurn
	//{
	//	get
	//	{
	//		return m_pveNumAbilityActionsThisTurn;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<uint>(value, ref m_pveNumAbilityActionsThisTurn, 4UL);
	//	}
	//}

	// rogues
	//public uint Networkm_pveNumQuickActionsThisTurn
	//{
	//	get
	//	{
	//		return m_pveNumQuickActionsThisTurn;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<uint>(value, ref m_pveNumQuickActionsThisTurn, 8UL);
	//	}
	//}

	// rogues
	//public uint Networkm_pveNumFreeActionsThisTurn
	//{
	//	get
	//	{
	//		return m_pveNumFreeActionsThisTurn;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<uint>(value, ref m_pveNumFreeActionsThisTurn, 16UL);
	//	}
	//}

	// rogues
	//public uint Networkm_pveMaxNumFreeActionsThisTurn
	//{
	//	get
	//	{
	//		return m_pveMaxNumFreeActionsThisTurn;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<uint>(value, ref m_pveMaxNumFreeActionsThisTurn, 32UL);
	//	}
	//}

	// rogues
	//public uint Networkm_pveNumAbilityActionsPerTurn
	//{
	//	get
	//	{
	//		return m_pveNumAbilityActionsPerTurn;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<uint>(value, ref m_pveNumAbilityActionsPerTurn, 64UL);
	//	}
	//}

	// rogues
	//public bool Networkm_hasStoredAbilityRequest
	//{
	//	get
	//	{
	//		return m_hasStoredAbilityRequest;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<bool>(value, ref m_hasStoredAbilityRequest, 128UL);
	//	}
	//}

	// rogues
	//public bool Networkm_hasStoredMoveRequest
	//{
	//	get
	//	{
	//		return m_hasStoredMoveRequest;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<bool>(value, ref m_hasStoredMoveRequest, 256UL);
	//	}
	//}

	// rogues
	//public short Networkm_numRespawnPicksThisTurn
	//{
	//	get
	//	{
	//		return m_numRespawnPicksThisTurn;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<short>(value, ref m_numRespawnPicksThisTurn, 512UL);
	//	}
	//}

	// rogues
	//public int Networkm_tauntRequestedForNextAbility
	//{
	//	get
	//	{
	//		return m_tauntRequestedForNextAbility;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<int>(value, ref m_tauntRequestedForNextAbility, 1024UL);
	//	}
	//}

	protected static void InvokeCmdCmdGUITurnMessage(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdGUITurnMessage called on client.");
			return;
		}
		// reactor
		((ActorTurnSM)obj).CmdGUITurnMessage((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
		// rogues
		// ((ActorTurnSM)obj).CmdGUITurnMessage(reader.ReadPackedInt32(), reader.ReadPackedInt32());
	}

	protected static void InvokeCmdCmdRequestCancelAction(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRequestCancelAction called on client.");
			return;
		}

		// reactor
		((ActorTurnSM)obj).CmdRequestCancelAction((int)reader.ReadPackedUInt32(), reader.ReadBoolean());
		// rogues
		// ((ActorTurnSM)obj).CmdRequestCancelAction(reader.ReadPackedInt32(), reader.ReadBoolean());
	}

	// rogues
	//protected static void InvokeCmdCmdLockInRequested(NetworkBehaviour obj, NetworkReader reader)
	//{
	//	if (!NetworkServer.active)
	//	{
	//		Debug.LogError("Command CmdLockInRequested called on client.");
	//		return;
	//	}
	//	((ActorTurnSM)obj).CmdLockInRequested();
	//}

	// rogues
	//protected static void InvokeCmdCmdExecuteQueuedRequests(NetworkBehaviour obj, NetworkReader reader)
	//{
	//	if (!NetworkServer.active)
	//	{
	//		Debug.LogError("Command CmdExecuteQueuedRequests called on client.");
	//		return;
	//	}
	//	((ActorTurnSM)obj).CmdExecuteQueuedRequests();
	//}

	protected static void InvokeCmdCmdChase(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdChase called on client.");
			return;
		}

		// reactor
		((ActorTurnSM)obj).CmdChase((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
		// rogues
		// ((ActorTurnSM)obj).CmdChase(reader.ReadPackedInt32(), reader.ReadPackedInt32());
	}

	protected static void InvokeCmdCmdSetSquare(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetSquare called on client.");
			return;
		}

		// reactor
		((ActorTurnSM)obj).CmdSetSquare((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), reader.ReadBoolean());
		// rogues
		// ((ActorTurnSM)obj).CmdSetSquare(reader.ReadPackedInt32(), reader.ReadPackedInt32(), reader.ReadBoolean(), reader.ReadBoolean());
	}

	// rogues
	//protected static void InvokeCmdCmdGroupMoveToSquare(NetworkBehaviour obj, NetworkReader reader)
	//{
	//	if (!NetworkServer.active)
	//	{
	//		Debug.LogError("Command CmdGroupMoveToSquare called on client.");
	//		return;
	//	}
	//	((ActorTurnSM)obj).CmdGroupMoveToSquare(reader.ReadPackedInt32(), reader.ReadPackedInt32(), reader.ReadPackedInt32());
	//}

	public void CallCmdGUITurnMessage(int msgEnum, int extraData)
	{
		// removed in rogues
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdGUITurnMessage called on server.");
			return;
		}

		if (isServer)
		{
			CmdGUITurnMessage(msgEnum, extraData);
			return;
		}
		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdGUITurnMessage);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)msgEnum);
		networkWriter.WritePackedUInt32((uint)extraData);
		SendCommandInternal(networkWriter, 0, "CmdGUITurnMessage");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(msgEnum);
		//networkWriter.WritePackedInt32(extraData);
		//base.SendCommandInternal(typeof(ActorTurnSM), "CmdGUITurnMessage", networkWriter, 0);
	}

	public void CallCmdRequestCancelAction(int action, bool hasIncomingRequest)
	{
		// removed in rogues
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdRequestCancelAction called on server.");
			return;
		}

		if (isServer)
		{
			CmdRequestCancelAction(action, hasIncomingRequest);
			return;
		}

		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdRequestCancelAction);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)action);
		networkWriter.Write(hasIncomingRequest);
		SendCommandInternal(networkWriter, 0, "CmdRequestCancelAction");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(action);
		//networkWriter.Write(hasIncomingRequest);
		//base.SendCommandInternal(typeof(ActorTurnSM), "CmdRequestCancelAction", networkWriter, 0);
	}

	// rogues
	//public void CallCmdLockInRequested()
	//{
	//	if (base.isServer)
	//	{
	//		CmdLockInRequested();
	//		return;
	//	}
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	base.SendCommandInternal(typeof(ActorTurnSM), "CmdLockInRequested", networkWriter, 0);
	//}

	// rogues
	//public void CallCmdExecuteQueuedRequests()
	//{
	//	if (base.isServer)
	//	{
	//		CmdExecuteQueuedRequests();
	//		return;
	//	}
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	base.SendCommandInternal(typeof(ActorTurnSM), "CmdExecuteQueuedRequests", networkWriter, 0);
	//}

	public void CallCmdChase(int selectedSquareX, int selectedSquareY)
	{
		// removed in rogues
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdChase called on server.");
			return;
		}

		if (isServer)
		{
			CmdChase(selectedSquareX, selectedSquareY);
			return;
		}
		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdChase);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)selectedSquareX);
		networkWriter.WritePackedUInt32((uint)selectedSquareY);
		SendCommandInternal(networkWriter, 0, "CmdChase");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(selectedSquareX);
		//networkWriter.WritePackedInt32(selectedSquareY);
		//base.SendCommandInternal(typeof(ActorTurnSM), "CmdChase", networkWriter, 0);
	}

	public void CallCmdSetSquare(int x, int y, bool setWaypoint) // , bool forceDelayExecution in rogues
	{
		// removed in rogues
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSetSquare called on server.");
			return;
		}

		if (isServer)
		{
			CmdSetSquare(x, y, setWaypoint); // , bool forceDelayExecution in rogues
			return;
		}
		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetSquare);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)x);
		networkWriter.WritePackedUInt32((uint)y);
		networkWriter.Write(setWaypoint);
		SendCommandInternal(networkWriter, 0, "CmdSetSquare");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(x);
		//networkWriter.WritePackedInt32(y);
		//networkWriter.Write(setWaypoint);
		//networkWriter.Write(forceDelayExecution);
		//base.SendCommandInternal(typeof(ActorTurnSM), "CmdSetSquare", networkWriter, 0);
	}

	// rogues
	//public void CallCmdGroupMoveToSquare(int actorIndex, int x, int y)
	//{
	//	if (base.isServer)
	//	{
	//		CmdGroupMoveToSquare(actorIndex, x, y);
	//		return;
	//	}
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32(actorIndex);
	//	networkWriter.WritePackedInt32(x);
	//	networkWriter.WritePackedInt32(y);
	//	base.SendCommandInternal(typeof(ActorTurnSM), "CmdGroupMoveToSquare", networkWriter, 0);
	//}

	protected static void InvokeRpcRpcTurnMessage(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcTurnMessage called on server.");
			return;
		}
		// reactor
		((ActorTurnSM)obj).RpcTurnMessage((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
		// rogues
		//((ActorTurnSM)obj).RpcTurnMessage(reader.ReadPackedInt32(), reader.ReadPackedInt32());
	}

	protected static void InvokeRpcRpcStoreAutoQueuedAbilityRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcStoreAutoQueuedAbilityRequest called on server.");
			return;
		}
		// reactor
		((ActorTurnSM)obj).RpcStoreAutoQueuedAbilityRequest((int)reader.ReadPackedUInt32());
		// rogues
		//((ActorTurnSM)obj).RpcStoreAutoQueuedAbilityRequest(reader.ReadPackedInt32());
	}

	// rogues
	//protected static void InvokeRpcRpcSetNumRespawnPickInputs(NetworkBehaviour obj, NetworkReader reader)
	//{
	//	if (!NetworkClient.active)
	//	{
	//		Debug.LogError("RPC RpcSetNumRespawnPickInputs called on server.");
	//		return;
	//	}
	//	((ActorTurnSM)obj).RpcSetNumRespawnPickInputs(reader.ReadPackedInt32());
	//}

	// rogues
	//protected static void InvokeRpcRpcResetUsedAbilityAndMoveData(NetworkBehaviour obj, NetworkReader reader)
	//{
	//	if (!NetworkClient.active)
	//	{
	//		Debug.LogError("RPC RpcResetUsedAbilityAndMoveData called on server.");
	//		return;
	//	}
	//	((ActorTurnSM)obj).RpcResetUsedAbilityAndMoveData();
	//}

	public void CallRpcTurnMessage(int msgEnum, int extraData)
	{
		// reactor
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcTurnMessage called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcTurnMessage);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)msgEnum);
		networkWriter.WritePackedUInt32((uint)extraData);
		SendRPCInternal(networkWriter, 0, "RpcTurnMessage");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(msgEnum);
		//networkWriter.WritePackedInt32(extraData);
		//this.SendRPCInternal(typeof(ActorTurnSM), "RpcTurnMessage", networkWriter, 0);
	}

	public void CallRpcStoreAutoQueuedAbilityRequest(int actionTypeInt)
	{
		// reactor
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcStoreAutoQueuedAbilityRequest called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcStoreAutoQueuedAbilityRequest);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionTypeInt);
		SendRPCInternal(networkWriter, 0, "RpcStoreAutoQueuedAbilityRequest");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(actionTypeInt);
		//this.SendRPCInternal(typeof(ActorTurnSM), "RpcStoreAutoQueuedAbilityRequest", networkWriter, 0);
	}

	// rogues
	//public void CallRpcSetNumRespawnPickInputs(int count)
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32(count);
	//	this.SendRPCInternal(typeof(ActorTurnSM), "RpcSetNumRespawnPickInputs", networkWriter, 0);
	//}

	// rogues
	//public void CallRpcResetUsedAbilityAndMoveData()
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	this.SendRPCInternal(typeof(ActorTurnSM), "RpcResetUsedAbilityAndMoveData", networkWriter, 0);
	//}


	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	// rogues
	//public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	//{
	//	bool result = base.OnSerialize(writer, forceAll);
	//	if (forceAll)
	//	{
	//		writer.Write(m_pveMoveCostUsedThisTurn);
	//		writer.WritePackedUInt32(m_pveNumMoveActionsThisTurn);
	//		writer.WritePackedUInt32(m_pveNumAbilityActionsThisTurn);
	//		writer.WritePackedUInt32(m_pveNumQuickActionsThisTurn);
	//		writer.WritePackedUInt32(m_pveNumFreeActionsThisTurn);
	//		writer.WritePackedUInt32(m_pveMaxNumFreeActionsThisTurn);
	//		writer.WritePackedUInt32(m_pveNumAbilityActionsPerTurn);
	//		writer.Write(m_hasStoredAbilityRequest);
	//		writer.Write(m_hasStoredMoveRequest);
	//		writer.Write(m_numRespawnPicksThisTurn);
	//		writer.WritePackedInt32(m_tauntRequestedForNextAbility);
	//		return true;
	//	}
	//	writer.WritePackedUInt64(base.syncVarDirtyBits);
	//	if ((base.syncVarDirtyBits & 1UL) != 0UL)
	//	{
	//		writer.Write(m_pveMoveCostUsedThisTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 2UL) != 0UL)
	//	{
	//		writer.WritePackedUInt32(m_pveNumMoveActionsThisTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 4UL) != 0UL)
	//	{
	//		writer.WritePackedUInt32(m_pveNumAbilityActionsThisTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 8UL) != 0UL)
	//	{
	//		writer.WritePackedUInt32(m_pveNumQuickActionsThisTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 16UL) != 0UL)
	//	{
	//		writer.WritePackedUInt32(m_pveNumFreeActionsThisTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 32UL) != 0UL)
	//	{
	//		writer.WritePackedUInt32(m_pveMaxNumFreeActionsThisTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 64UL) != 0UL)
	//	{
	//		writer.WritePackedUInt32(m_pveNumAbilityActionsPerTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 128UL) != 0UL)
	//	{
	//		writer.Write(m_hasStoredAbilityRequest);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 256UL) != 0UL)
	//	{
	//		writer.Write(m_hasStoredMoveRequest);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 512UL) != 0UL)
	//	{
	//		writer.Write(m_numRespawnPicksThisTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 1024UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_tauntRequestedForNextAbility);
	//		result = true;
	//	}
	//	return result;
	//}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}

	// rogues
	//public override void OnDeserialize(NetworkReader reader, bool initialState)
	//{
	//	base.OnDeserialize(reader, initialState);
	//	if (initialState)
	//	{
	//		float networkm_pveMoveCostUsedThisTurn = reader.ReadSingle();
	//		Networkm_pveMoveCostUsedThisTurn = networkm_pveMoveCostUsedThisTurn;
	//		uint networkm_pveNumMoveActionsThisTurn = reader.ReadPackedUInt32();
	//		Networkm_pveNumMoveActionsThisTurn = networkm_pveNumMoveActionsThisTurn;
	//		uint networkm_pveNumAbilityActionsThisTurn = reader.ReadPackedUInt32();
	//		Networkm_pveNumAbilityActionsThisTurn = networkm_pveNumAbilityActionsThisTurn;
	//		uint networkm_pveNumQuickActionsThisTurn = reader.ReadPackedUInt32();
	//		Networkm_pveNumQuickActionsThisTurn = networkm_pveNumQuickActionsThisTurn;
	//		uint networkm_pveNumFreeActionsThisTurn = reader.ReadPackedUInt32();
	//		Networkm_pveNumFreeActionsThisTurn = networkm_pveNumFreeActionsThisTurn;
	//		uint networkm_pveMaxNumFreeActionsThisTurn = reader.ReadPackedUInt32();
	//		Networkm_pveMaxNumFreeActionsThisTurn = networkm_pveMaxNumFreeActionsThisTurn;
	//		uint networkm_pveNumAbilityActionsPerTurn = reader.ReadPackedUInt32();
	//		Networkm_pveNumAbilityActionsPerTurn = networkm_pveNumAbilityActionsPerTurn;
	//		bool networkm_hasStoredAbilityRequest = reader.ReadBoolean();
	//		Networkm_hasStoredAbilityRequest = networkm_hasStoredAbilityRequest;
	//		bool networkm_hasStoredMoveRequest = reader.ReadBoolean();
	//		Networkm_hasStoredMoveRequest = networkm_hasStoredMoveRequest;
	//		short networkm_numRespawnPicksThisTurn = reader.ReadInt16();
	//		Networkm_numRespawnPicksThisTurn = networkm_numRespawnPicksThisTurn;
	//		int networkm_tauntRequestedForNextAbility = reader.ReadPackedInt32();
	//		Networkm_tauntRequestedForNextAbility = networkm_tauntRequestedForNextAbility;
	//		return;
	//	}
	//	long num = (long)reader.ReadPackedUInt64();
	//	if ((num & 1L) != 0L)
	//	{
	//		float networkm_pveMoveCostUsedThisTurn2 = reader.ReadSingle();
	//		Networkm_pveMoveCostUsedThisTurn = networkm_pveMoveCostUsedThisTurn2;
	//	}
	//	if ((num & 2L) != 0L)
	//	{
	//		uint networkm_pveNumMoveActionsThisTurn2 = reader.ReadPackedUInt32();
	//		Networkm_pveNumMoveActionsThisTurn = networkm_pveNumMoveActionsThisTurn2;
	//	}
	//	if ((num & 4L) != 0L)
	//	{
	//		uint networkm_pveNumAbilityActionsThisTurn2 = reader.ReadPackedUInt32();
	//		Networkm_pveNumAbilityActionsThisTurn = networkm_pveNumAbilityActionsThisTurn2;
	//	}
	//	if ((num & 8L) != 0L)
	//	{
	//		uint networkm_pveNumQuickActionsThisTurn2 = reader.ReadPackedUInt32();
	//		Networkm_pveNumQuickActionsThisTurn = networkm_pveNumQuickActionsThisTurn2;
	//	}
	//	if ((num & 16L) != 0L)
	//	{
	//		uint networkm_pveNumFreeActionsThisTurn2 = reader.ReadPackedUInt32();
	//		Networkm_pveNumFreeActionsThisTurn = networkm_pveNumFreeActionsThisTurn2;
	//	}
	//	if ((num & 32L) != 0L)
	//	{
	//		uint networkm_pveMaxNumFreeActionsThisTurn2 = reader.ReadPackedUInt32();
	//		Networkm_pveMaxNumFreeActionsThisTurn = networkm_pveMaxNumFreeActionsThisTurn2;
	//	}
	//	if ((num & 64L) != 0L)
	//	{
	//		uint networkm_pveNumAbilityActionsPerTurn2 = reader.ReadPackedUInt32();
	//		Networkm_pveNumAbilityActionsPerTurn = networkm_pveNumAbilityActionsPerTurn2;
	//	}
	//	if ((num & 128L) != 0L)
	//	{
	//		bool networkm_hasStoredAbilityRequest2 = reader.ReadBoolean();
	//		Networkm_hasStoredAbilityRequest = networkm_hasStoredAbilityRequest2;
	//	}
	//	if ((num & 256L) != 0L)
	//	{
	//		bool networkm_hasStoredMoveRequest2 = reader.ReadBoolean();
	//		Networkm_hasStoredMoveRequest = networkm_hasStoredMoveRequest2;
	//	}
	//	if ((num & 512L) != 0L)
	//	{
	//		short networkm_numRespawnPicksThisTurn2 = reader.ReadInt16();
	//		Networkm_numRespawnPicksThisTurn = networkm_numRespawnPicksThisTurn2;
	//	}
	//	if ((num & 1024L) != 0L)
	//	{
	//		int networkm_tauntRequestedForNextAbility2 = reader.ReadPackedInt32();
	//		Networkm_tauntRequestedForNextAbility = networkm_tauntRequestedForNextAbility2;
	//	}
	//}
}
