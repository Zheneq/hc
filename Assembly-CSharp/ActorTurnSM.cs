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

	private ActorData m_actorData;
	private bool m_firstUpdate;
	private float m_timePingDown;
	private Vector3 m_worldPositionPingDown;
	private Vector3 m_mousePositionPingDown;

	private const float c_advancedPingTime = 0.25f;

	private bool m_abilitySelectorVisible;

	private static Color s_chasingTextColor = new Color(0.3f, 0.75f, 0.75f);
	private static Color s_movingTextColor = new Color(0.4f, 1f, 1f);
	private static Color s_decidingTextColor = new Color(0.9f, 0.9f, 0.9f);

	private TurnStateEnum _NextState;
	private DateTime _TurnStart = DateTime.UtcNow;
	private DateTime _LockInTime = DateTime.MinValue;
	private List<AbilityData.ActionType> m_autoQueuedRequestActionTypes;
	private List<ActionRequestForUndo> m_requestStackForUndo;
	private TurnState[] m_turnStates;
	private List<AbilityTarget> m_targets;

	private static int kCmdCmdGUITurnMessage = -122150213;
	private static int kCmdCmdRequestCancelAction = 1831775955;
	private static int kCmdCmdChase = 1451912258;
	private static int kCmdCmdSetSquare = -1156253069;
	private static int kRpcRpcTurnMessage = -107921272;
	private static int kRpcRpcStoreAutoQueuedAbilityRequest = 675585254;

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

	public bool HandledSpaceInput;
	public bool HandledMouseInput;

	internal int LastConfirmedCancelTurn { get; private set; }

	static ActorTurnSM()
	{
		RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdGUITurnMessage, InvokeCmdCmdGUITurnMessage);
		RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdRequestCancelAction, InvokeCmdCmdRequestCancelAction);
		RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdChase, InvokeCmdCmdChase);
		RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdSetSquare, InvokeCmdCmdSetSquare);
		RegisterRpcDelegate(typeof(ActorTurnSM), kRpcRpcTurnMessage, InvokeRpcRpcTurnMessage);
		RegisterRpcDelegate(typeof(ActorTurnSM), kRpcRpcStoreAutoQueuedAbilityRequest, InvokeRpcRpcStoreAutoQueuedAbilityRequest);
		NetworkCRC.RegisterBehaviour("ActorTurnSM", 0);
	}

	private void Awake()
	{
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
		CurrentState = PreviousState = NextState = TurnStateEnum.WAITING;
		m_targets = new List<AbilityTarget>();
		LastConfirmedCancelTurn = -1;
		m_requestStackForUndo = new List<ActionRequestForUndo>();
		m_autoQueuedRequestActionTypes = new List<AbilityData.ActionType>();
		m_actorData = GetComponent<ActorData>();
		m_firstUpdate = true;
	}

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
			if (NetworkClient.active && onLockIn)
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (selectedAbility != null && activeOwnedActorData != null && activeOwnedActorData == component)
				{
					selectedAbility.ResetAbilityTargeters();
				}
			}
			if (!NetworkServer.active)
			{
				SendCastAbility(selectedActionType);
			}
		}
		Board.Get()?.MarkForUpdateValidSquares();
		return result;
	}

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
		DisplayActorState();
		if (m_firstUpdate)
		{
			m_firstUpdate = false;
		}
	}

	private void LateUpdate()
	{
		HandledSpaceInput = false;
		HandledMouseInput = false;
	}

	private void UpdateStates()
	{
		do
		{
			SwitchToNewStates();
			GetState().Update();
		}
		while (NextState != CurrentState);
	}

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
			case TurnStateEnum.DECIDING_MOVEMENT:
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
			m_actorData.OnClientQueuedActionChanged();
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
			CallCmdGUITurnMessage((int)TurnMessage.CANCEL_BUTTON_CLICKED, 0);
		}
	}

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
			CallCmdGUITurnMessage((int)TurnMessage.CANCEL_MOVEMENT, 0);
		}
		ActorData component = GetComponent<ActorData>();
		if (component == GameFlowData.Get().activeOwnedActorData
			&& component.IsActorInvisibleForRespawn()
			&& SpawnPointManager.Get() != null
			&& SpawnPointManager.Get().m_spawnInDuringMovement)
		{
			InterfaceManager.Get().DisplayAlert(StringUtil.TR("PostRespawnMovement", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true);
		}
		if (NetworkClient.active && component == GameFlowData.Get().activeOwnedActorData)
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
			CallCmdGUITurnMessage((int)TurnMessage.MOVE_BUTTON_CLICKED, 0);
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
		if (UIMainScreenPanel.Get() != null
			&& GameFlowData.Get().activeOwnedActorData != null
			&& GameFlowData.Get().activeOwnedActorData == m_actorData)
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

	public bool IsKeyDown(KeyCode keyCode)
	{
		return CameraControls.Get().Enabled && Input.GetKeyDown(keyCode);
	}

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
			m_actorData.OnClientQueuedActionChanged();
		}
	}

	private void CancelUndoableAbilityRequest(AbilityData.ActionType actionType)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
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
			m_actorData.OnClientQueuedActionChanged();
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

	private TurnState GetState()
	{
		return m_turnStates[(int)CurrentState];
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
			GetState().OnMsg(msg, extraData);
			UpdateStates();
		}
	}

	[Command]
	private void CmdGUITurnMessage(int msgEnum, int extraData)
	{
		Log.Info($"[JSON] {{\"CmdGUITurnMessage\":{{\"msgEnum\":{DefaultJsonSerializer.Serialize(msgEnum)},\"extraData\":{DefaultJsonSerializer.Serialize(extraData)}}}}}");
	}

	[Command]
	private void CmdRequestCancelAction(int action, bool hasIncomingRequest)
	{
		Log.Info($"[JSON] {{\"CmdRequestCancelAction\":{{\"action\":{DefaultJsonSerializer.Serialize(action)},\"hasIncomingRequest\":{DefaultJsonSerializer.Serialize(hasIncomingRequest)}}}}}");
	}

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
		bool isWaypoint = Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier);
		bool isExplicitWaypoint = Options_UI.Get().GetShiftClickForMovementWaypoints() && InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier);
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
					SelectMovementSquareForMovement(playerClampedSquare);
				}
			}
			else if (m_actorData.HasQueuedChase())
			{
				if (playerClampedSquare == m_actorData.GetQueuedChaseTarget().GetCurrentBoardSquare())
				{
					SelectMovementSquareForMovement(playerClampedSquare);
				}
				else if (!SelectMovementSquareForChasing(playerClampedSquare))
				{
					SelectMovementSquareForMovement(playerClampedSquare);
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
					SelectMovementSquareForMovement(playerClampedSquare);
				}
			}
		}
	}

	public bool SelectMovementSquareForChasing(BoardSquare selectedSquare)
	{
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
	}

	[Command]
	private void CmdChase(int selectedSquareX, int selectedSquareY)
	{
		Log.Info($"[JSON] {{\"CmdChase\":{{\"selectedSquareX\":{DefaultJsonSerializer.Serialize(selectedSquareX)},\"selectedSquareY\":{DefaultJsonSerializer.Serialize(selectedSquareY)}}}}}");
	}

	public void SelectMovementSquareForMovement(BoardSquare selectedSquare)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		list.Add(selectedSquare);
		SelectMovementSquaresForMovement(list);
	}

	public void SelectMovementSquaresForMovement(List<BoardSquare> selectedSquares)
	{
		ActorData actorData = GetComponent<ActorData>();
		if (GameFlowData.Get() == null || GameFlowData.Get().gameState != GameState.BothTeams_Decision)
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
				if (actorData == GameFlowData.Get().activeOwnedActorData
					&& num == 0
					&& actorData.GetActorMovement().SquaresCanMoveTo.Count > 0)
				{
					UISounds.GetUISounds().Play("ui/ingame/v1/move");
				}
				bool isWaypoint = Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier)
					&& FirstTurnMovement.CanWaypoint();
				StoreUndoableActionRequest(new ActionRequestForUndo(UndoableRequestType.MOVEMENT));
				CallCmdSetSquare(boardSquare.x, boardSquare.y, isWaypoint);
				flag = true;
			}
			num++;
		}
		if (flag)
		{
			NextState = TurnStateEnum.VALIDATING_MOVE_REQUEST;
			Log.Info(string.Concat("Setting State to ", NextState, " at ", GameTime.time));
			if (NetworkClient.active && actorData == GameFlowData.Get().activeOwnedActorData)
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
	private void CmdSetSquare(int x, int y, bool setWaypoint)
	{
		Log.Info($"[JSON] {{\"CmdSetSquare\":{{" +
			$"\"x\":{DefaultJsonSerializer.Serialize(x)}," +
			$"\"y\":{DefaultJsonSerializer.Serialize(y)}," +
			$"\"setWaypoint\":{DefaultJsonSerializer.Serialize(setWaypoint)}}}}}");
	}

	[ClientRpc]
	private void RpcTurnMessage(int msgEnum, int extraData)
	{
		Log.Info($"[JSON] {{\"RpcTurnMessage\":{{\"msgEnum\":{DefaultJsonSerializer.Serialize((TurnMessage)msgEnum)},\"extraData\":{DefaultJsonSerializer.Serialize(extraData)}}}}}");
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
		UpdateStates();
	}

	[ClientRpc]
	private void RpcStoreAutoQueuedAbilityRequest(int actionTypeInt)
	{
		Log.Info($"[JSON] {{\"RpcStoreAutoQueuedAbilityRequest\":{{\"actionTypeInt\":{DefaultJsonSerializer.Serialize((AbilityData.ActionType)actionTypeInt)}}}}}");
		if (!NetworkServer.active)
		{
			StoreAutoQueuedAbilityRequest((AbilityData.ActionType)actionTypeInt);
		}
	}

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
		if (activeOwnedActorData != null && activeOwnedActorData.GetActorTurnSM() == this)
		{
			AbilityData abilityData = activeOwnedActorData.GetAbilityData();
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
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.CONFIRMED && m_actorData.GetTimeBank().AllowUnconfirm();
	}

	public bool CanQueueSimpleAction()
	{
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.CONFIRMED && m_actorData.GetTimeBank().AllowUnconfirm();
	}

	public bool CanPickRespawnLocation()
	{
		return CurrentState == TurnStateEnum.PICKING_RESPAWN
			|| (CurrentState == TurnStateEnum.CONFIRMED && PreviousState == TurnStateEnum.PICKING_RESPAWN);
	}

	public bool AmDecidingMovement()
	{

		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.CONFIRMED && m_actorData.GetTimeBank().AllowUnconfirm();
	}

	public bool IsAbilityOrPingSelectorVisible()
	{
		return m_abilitySelectorVisible || m_timePingDown != 0f;
	}

	public static bool IsClientDecidingMovement()
	{
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
		{
			ActorTurnSM actorTurnSM = GameFlowData.Get().activeOwnedActorData.GetActorTurnSM();
			if (actorTurnSM != null)
			{
				return actorTurnSM.AmDecidingMovement();
			}
			return false;
		}
		return false;
	}

	public bool AmTargetingAction()
	{
		return CurrentState == TurnStateEnum.TARGETING_ACTION;
	}

	public bool AmStillDeciding()
	{
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.VALIDATING_MOVE_REQUEST
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST
			|| CurrentState == TurnStateEnum.PICKING_RESPAWN;
	}

	public bool ShouldShowGUIButtons()
	{
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.VALIDATING_MOVE_REQUEST
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST
			|| CurrentState == TurnStateEnum.CONFIRMED
			|| CurrentState == TurnStateEnum.PICKING_RESPAWN;
	}

	public bool ShouldEnableEndTurnButton()
	{
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.PICKING_RESPAWN;
	}

	public bool ShouldEnableMoveButton()
	{
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION;
	}

	public bool ShouldShowEndTurnButton()
	{
		return ShouldShowGUIButtons() && CurrentState != TurnStateEnum.CONFIRMED;
	}

	public bool ShouldEnableAbilityButton(bool isSimpleAction)
	{
		return CurrentState == TurnStateEnum.DECIDING
			|| CurrentState == TurnStateEnum.DECIDING_MOVEMENT
			|| CurrentState == TurnStateEnum.TARGETING_ACTION
			|| CurrentState == TurnStateEnum.CONFIRMED && isSimpleAction;
	}

	public void SetupForNewTurn()
	{
		ActorData component = GetComponent<ActorData>();
		if (HUD_UI.Get() != null && component == GameFlowData.Get().activeOwnedActorData)
		{
			HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay.Decision);
		}
		if (component == GameFlowData.Get().activeOwnedActorData)
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
			if (actorMovement && !GameplayUtils.IsMinion(this))
			{
				actorMovement.UpdateSquaresCanMoveTo();
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeCmdCmdGUITurnMessage(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdGUITurnMessage called on client.");
			return;
		}
		((ActorTurnSM)obj).CmdGUITurnMessage((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdRequestCancelAction(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRequestCancelAction called on client.");
			return;
		}
		((ActorTurnSM)obj).CmdRequestCancelAction((int)reader.ReadPackedUInt32(), reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdChase(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdChase called on client.");
			return;
		}
		((ActorTurnSM)obj).CmdChase((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdSetSquare(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetSquare called on client.");
			return;
		}
		((ActorTurnSM)obj).CmdSetSquare((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), reader.ReadBoolean());
	}

	public void CallCmdGUITurnMessage(int msgEnum, int extraData)
	{
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
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdGUITurnMessage);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)msgEnum);
		networkWriter.WritePackedUInt32((uint)extraData);
		SendCommandInternal(networkWriter, 0, "CmdGUITurnMessage");
	}

	public void CallCmdRequestCancelAction(int action, bool hasIncomingRequest)
	{
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
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdRequestCancelAction);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)action);
		networkWriter.Write(hasIncomingRequest);
		SendCommandInternal(networkWriter, 0, "CmdRequestCancelAction");
	}

	public void CallCmdChase(int selectedSquareX, int selectedSquareY)
	{
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
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdChase);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)selectedSquareX);
		networkWriter.WritePackedUInt32((uint)selectedSquareY);
		SendCommandInternal(networkWriter, 0, "CmdChase");
	}

	public void CallCmdSetSquare(int x, int y, bool setWaypoint)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSetSquare called on server.");
			return;
		}
		if (isServer)
		{
			CmdSetSquare(x, y, setWaypoint);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetSquare);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)x);
		networkWriter.WritePackedUInt32((uint)y);
		networkWriter.Write(setWaypoint);
		SendCommandInternal(networkWriter, 0, "CmdSetSquare");
	}

	protected static void InvokeRpcRpcTurnMessage(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcTurnMessage called on server.");
			return;
		}
		((ActorTurnSM)obj).RpcTurnMessage((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcStoreAutoQueuedAbilityRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcStoreAutoQueuedAbilityRequest called on server.");
			return;
		}
		((ActorTurnSM)obj).RpcStoreAutoQueuedAbilityRequest((int)reader.ReadPackedUInt32());
	}

	public void CallRpcTurnMessage(int msgEnum, int extraData)
	{
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
	}

	public void CallRpcStoreAutoQueuedAbilityRequest(int actionTypeInt)
	{
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
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
