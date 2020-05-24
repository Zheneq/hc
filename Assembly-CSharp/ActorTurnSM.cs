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

		public ActionRequestForUndo(UndoableRequestType requestType, AbilityData.ActionType actionType = AbilityData.ActionType.INVALID_ACTION)
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

	public bool LockInBuffered
	{
		get;
		set;
	}

	public TurnStateEnum CurrentState
	{
		get;
		private set;
	}

	public TurnStateEnum PreviousState
	{
		get;
		private set;
	}

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

	internal int LastConfirmedCancelTurn
	{
		get;
		private set;
	}

	static ActorTurnSM()
	{
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdGUITurnMessage, InvokeCmdCmdGUITurnMessage);
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdRequestCancelAction, InvokeCmdCmdRequestCancelAction);
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdChase, InvokeCmdCmdChase);
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), kCmdCmdSetSquare, InvokeCmdCmdSetSquare);
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTurnSM), kRpcRpcTurnMessage, InvokeRpcRpcTurnMessage);
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTurnSM), kRpcRpcStoreAutoQueuedAbilityRequest, InvokeRpcRpcStoreAutoQueuedAbilityRequest);
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
			selectedAbility.Targeter.AbilityCasted(component.GetGridPosWithIncrementedHeight(), abilityTarget.GridPos);
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
			if (!m_abilitySelectorVisible)
			{
				return;
			}

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
			return;
		}
		if (!m_abilitySelectorVisible)
		{
			return;
		}
		m_abilitySelectorVisible = false;
		UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel, false);
		KeyPreference abilityHover = HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel.GetAbilityHover();
		if (HUD_UI.Get() == null)
		{
			return;
		}
		if (abilityHover != 0)
		{
			UIMainScreenPanel.Get().m_abilityBar.DoAbilityButtonClick(abilityHover);
		}
	}

	private void CheckPingInput()
	{
		if (!(GameFlowData.Get().activeOwnedActorData == m_actorData))
		{
			return;
		}
		if (CurrentState != TurnStateEnum.TARGETING_ACTION)
		{
			if (Input.GetMouseButtonDown(0) && InterfaceManager.Get().ShouldHandleMouseClick())
			{
				if (InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							if (HUD_UI.Get() != null)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										if (Board.Get().PlayerFreeSquare != null)
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
								}
							}
							return;
						}
					}
				}
			}
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
			if (!Input.GetMouseButtonUp(0))
			{
				return;
			}
			while (true)
			{
				BigPingPanel component = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>();
				ActorController.PingType pingType = component.GetPingType();
				UIManager.SetGameObjectActive(component, false);
				m_timePingDown = 0f;
				HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(m_worldPositionPingDown, pingType);
				return;
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					BigPingPanel component2 = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>();
					ActorController.PingType pingType2 = component2.GetPingType();
					UIManager.SetGameObjectActive(component2, false);
					m_timePingDown = 0f;
					if (pingType2 != 0)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(m_worldPositionPingDown, pingType2);
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		if (HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_closeButton.activeSelf)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_closeButton, true);
			return;
		}
	}

	private void CheckAbilityInputControlPad()
	{
		if (!(GameFlowData.Get().activeOwnedActorData == GetComponent<ActorData>()) || !(ControlpadGameplay.Get() != null) || !ControlpadGameplay.Get().UsingControllerInput)
		{
			return;
		}
		if (m_timePingDown == 0f)
		{
			if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftTrigger) > 0f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
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
						return;
					}
				}
			}
		}
		if (m_timePingDown == 0f)
		{
			return;
		}
		while (true)
		{
			RectTransform rectTransform = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_line.transform as RectTransform;
			float num;
			if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY) == 0f)
			{
				if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX) == 0f)
				{
					num = 0f;
					goto IL_02e0;
				}
			}
			num = 200f;
			goto IL_02e0;
			IL_02e0:
			float num2 = num;
			rectTransform.sizeDelta = new Vector2(num2, 2f);
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.anchoredPosition = Vector2.zero;
			float num3 = Mathf.Atan2(ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY), ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX)) * 57.29578f;
			rectTransform.rotation = Quaternion.Euler(0f, 0f, num3);
			HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.SelectAbilityButtonFromAngle(num3, num2);
			if (GameTime.time < m_timePingDown + 0.25f)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftTrigger) == 0f)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
								{
									ActorController.PingType pingType = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetPingType();
									UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad, false);
									m_timePingDown = 0f;
									HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(m_worldPositionPingDown, pingType);
									return;
								}
								}
							}
						}
						return;
					}
				}
			}
			if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftTrigger) == 0f)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						ActorController.PingType pingType2 = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetPingType();
						UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad, false);
						m_timePingDown = 0f;
						if (pingType2 != 0)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(m_worldPositionPingDown, pingType2);
									return;
								}
							}
						}
						return;
					}
					}
				}
			}
			if (!HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_closeButton.activeSelf)
			{
				while (true)
				{
					UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_closeButton, true);
					return;
				}
			}
			return;
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
		if (CurrentState == TurnStateEnum.DECIDING_MOVEMENT)
		{
			flag = true;
		}
		else if (CurrentState != TurnStateEnum.CONFIRMED)
		{
			if (CurrentState == TurnStateEnum.DECIDING)
			{
				flag = true;
			}
			else
			{
				BackToDecidingState();
			}
		}
		else if (m_actorData.GetTimeBank().AllowUnconfirm())
		{
			LastConfirmedCancelTurn = GameFlowData.Get().CurrentTurn;
			m_actorData.GetTimeBank().OnActionsUnconfirmed();
			BackToDecidingState();
			if (Options_UI.Get()?.ShouldCancelActionWhileConfirmed() ?? false)
			{
				flag = true;
			}
		}
		if (!onlyCancelConfirm && flag && m_requestStackForUndo.Count != 0)
		{
			int index = m_requestStackForUndo.Count - 1;
			ActionRequestForUndo actionRequestForUndo = m_requestStackForUndo[index];
			m_requestStackForUndo.RemoveAt(index);
			m_actorData.OnClientQueuedActionChanged();
			UndoableRequestType type = actionRequestForUndo.m_type;
			if (type != UndoableRequestType.MOVEMENT && type == UndoableRequestType.ABILITY_QUEUE)
			{
				RequestCancelAction(actionRequestForUndo.m_action, false);
				UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
			}
			else
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
		if (NetworkServer.active)
		{
			return;
		}
		CallCmdGUITurnMessage(4, 0);
	}

	private void UpdateCancelKey()
	{
		if (!(GameFlowData.Get().activeOwnedActorData == GetComponent<ActorData>()))
		{
			return;
		}
		while (true)
		{
			ActorData component = GetComponent<ActorData>();
			bool flag = component.HasQueuedMovement();
			AbilityData component2 = GetComponent<AbilityData>();
			bool flag2 = component2.HasQueuedAbilities();
			bool flag3 = true;
			if (InputManager.Get().IsKeyCodeMatchKeyBind(KeyPreference.CancelAction, KeyCode.Escape))
			{
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					if (UISystemEscapeMenu.Get() != null)
					{
						if (UISystemEscapeMenu.Get().IsOpen())
						{
							flag3 = false;
							UISystemEscapeMenu.Get().OnToggleButtonClick(null);
							goto IL_018c;
						}
					}
					if (UIGameStatsWindow.Get() != null)
					{
						if (UIGameStatsWindow.Get().m_container.gameObject.activeSelf)
						{
							flag3 = false;
							UIGameStatsWindow.Get().ToggleStatsWindow();
							goto IL_018c;
						}
					}
					if (Options_UI.Get().IsVisible())
					{
						flag3 = false;
						Options_UI.Get().HideOptions();
					}
					else if (KeyBinding_UI.Get().IsVisible())
					{
						flag3 = false;
						if (!KeyBinding_UI.Get().IsSettingKeybindCommand())
						{
							KeyBinding_UI.Get().HideKeybinds();
						}
					}
				}
			}
			goto IL_018c;
			IL_018c:
			if (!flag)
			{
				if (!flag2)
				{
					if (CurrentState == TurnStateEnum.DECIDING)
					{
						return;
					}
				}
			}
			if (!flag3)
			{
				return;
			}
			while (true)
			{
				if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CancelAction))
				{
					RequestCancel();
				}
				return;
			}
		}
	}

	public bool CheckStateForEndTurnRequestFromInput()
	{
		if (CurrentState == TurnStateEnum.DECIDING && NextState == TurnStateEnum.DECIDING)
		{
			return true;
		}
		return CurrentState == TurnStateEnum.PICKING_RESPAWN;
	}

	public void UpdateEndTurnKey()
	{
		if (!(GameFlowData.Get().activeOwnedActorData == GetComponent<ActorData>()) || !ShouldShowEndTurnButton())
		{
			return;
		}
		while (true)
		{
			if (!ShouldEnableEndTurnButton() || !InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.LockIn) || HandledSpaceInput)
			{
				return;
			}
			while (true)
			{
				if (UITutorialFullscreenPanel.Get().IsAnyPanelVisible())
				{
					return;
				}
				while (true)
				{
					if (!(AppState.GetCurrent() != AppState_InGameDeployment.Get()))
					{
						return;
					}
					if (CheckStateForEndTurnRequestFromInput())
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								HandledSpaceInput = true;
								UISounds.GetUISounds().Play("ui/ingame/v1/hud/lockin");
								Log.Info("Lock in request at " + GameTime.time);
								RequestEndTurn();
								if (HUD_UI.Get() != null)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.LockedInClicked();
											return;
										}
									}
								}
								return;
							}
						}
					}
					if (CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
					{
						if (CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
						{
							return;
						}
					}
					LockInBuffered = true;
					Log.Info("Lockin to be buffered at " + GameTime.time);
					return;
				}
			}
		}
	}

	public void GetActionText(out string textStr, out Color textColor)
	{
		AbilityData component = GetComponent<AbilityData>();
		Ability ability = null;
		if ((bool)component)
		{
			ability = component.GetSelectedAbility();
		}
		if (CurrentState == TurnStateEnum.CONFIRMED)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (ability != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								textColor = Color.green;
								textStr = ability.m_abilityName;
								return;
							}
						}
					}
					ActorData component2 = GetComponent<ActorData>();
					bool flag = component2.HasQueuedMovement();
					if (component2.HasQueuedChase())
					{
						textColor = s_chasingTextColor;
						textStr = "Chasing";
					}
					else
					{
						if (flag)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									textColor = s_movingTextColor;
									textStr = "Moving";
									return;
								}
							}
						}
						textColor = s_movingTextColor;
						textStr = "Staying";
					}
					return;
				}
				}
			}
		}
		if (CurrentState == TurnStateEnum.DECIDING || CurrentState == TurnStateEnum.DECIDING_MOVEMENT)
		{
			textColor = s_decidingTextColor;
			textStr = "(Deciding...)";
		}
		else
		{
			textColor = s_decidingTextColor;
			textStr = string.Empty;
		}
	}

	public void RequestEndTurn()
	{
		ActorData component = GetComponent<ActorData>();
		if (AmTargetingAction())
		{
			Ability selectedAbility = component.GetAbilityData().GetSelectedAbility();
			while (true)
			{
				if (SelectTarget(null, true))
				{
					NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
				}
				if (!(selectedAbility != null) || !selectedAbility.ShouldAutoConfirmIfTargetingOnEndTurn())
				{
					break;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						goto end_IL_0067;
					}
					continue;
					end_IL_0067:
					break;
				}
				if (m_targets.Count >= selectedAbility.GetExpectedNumberOfTargeters())
				{
					break;
				}
			}
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
				CallCmdGUITurnMessage(14, 0);
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (NetworkServer.active)
		{
			OnMessage(TurnMessage.CANCEL_MOVEMENT, 0);
		}
		else
		{
			CallCmdGUITurnMessage(16, 0);
		}
		ActorData component = GetComponent<ActorData>();
		if (component == GameFlowData.Get().activeOwnedActorData)
		{
			if (component.ShouldPickRespawn_zq())
			{
				if (SpawnPointManager.Get() != null)
				{
					if (SpawnPointManager.Get().m_spawnInDuringMovement)
					{
						InterfaceManager.Get().DisplayAlert(StringUtil.TR("PostRespawnMovement", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true);
					}
				}
			}
		}
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			if (!(component == GameFlowData.Get().activeOwnedActorData))
			{
				return;
			}
			while (true)
			{
				LineData component2 = component.GetComponent<LineData>();
				if (component2 != null)
				{
					while (true)
					{
						component2.OnClientRequestedMovementChange();
						return;
					}
				}
				return;
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
		while (true)
		{
			GetState().OnExit();
			PreviousState = CurrentState;
			CurrentState = NextState;
			if (UIMainScreenPanel.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
			{
				if (GameFlowData.Get().activeOwnedActorData == m_actorData)
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
			}
			GetState().OnEnter();
			if (CameraManager.Get() != null)
			{
				CameraManager.Get().OnNewTurnSMState();
			}
			if (Board.Get() != null)
			{
				while (true)
				{
					Board.Get().MarkForUpdateValidSquares();
					return;
				}
			}
			return;
		}
	}

	public void ResetTurnStartNow()
	{
		_TurnStart = DateTime.UtcNow;
	}

	public bool IsKeyDown(KeyCode keyCode)
	{
		int result;
		if (CameraControls.Get().Enabled)
		{
			result = (Input.GetKeyDown(keyCode) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
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
		if (!m_autoQueuedRequestActionTypes.Contains(actionType))
		{
			return;
		}
		while (true)
		{
			m_autoQueuedRequestActionTypes.Remove(actionType);
			return;
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
			using (List<ActionRequestForUndo>.Enumerator enumerator = m_requestStackForUndo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActionRequestForUndo current = enumerator.Current;
					if (current.m_type == UndoableRequestType.MOVEMENT)
					{
						flag = true;
					}
				}
			}
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			m_requestStackForUndo.Add(request);
			m_actorData.OnClientQueuedActionChanged();
			return;
		}
	}

	private void CancelUndoableAbilityRequest(AbilityData.ActionType actionType)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			if (actionType != AbilityData.ActionType.INVALID_ACTION)
			{
				ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
				if (component.IsAbilityCinematicRequested(actionType))
				{
					activeOwnedActorData.GetComponent<ActorCinematicRequests>().SendAbilityCinematicRequest(actionType, false, -1, -1);
				}
			}
		}
		int num = -1;
		for (int i = 0; i < m_requestStackForUndo.Count; i++)
		{
			if (m_requestStackForUndo[i].m_type != 0)
			{
				continue;
			}
			if (m_requestStackForUndo[i].m_action == actionType)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			return;
		}
		while (true)
		{
			m_requestStackForUndo.RemoveAt(num);
			m_actorData.OnClientQueuedActionChanged();
			if (HUD_UI.Get() != null)
			{
				while (true)
				{
					HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.CancelAbilityRequest(actionType);
					return;
				}
			}
			return;
		}
	}

	internal void OnQueueAbilityRequest(AbilityData.ActionType actionType)
	{
		AbilityData component = GetComponent<AbilityData>();
		List<AbilityData.ActionType> actionsToCancel = null;
		bool cancelMovement = false;
		if (component.GetActionsToCancelOnTargetingComplete(ref actionsToCancel, ref cancelMovement))
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
			component.ClearActionsToCancelOnTargetingComplete();
		}
		StoreUndoableActionRequest(new ActionRequestForUndo(UndoableRequestType.ABILITY_QUEUE, actionType));
	}

	[Server]
	internal void QueueAutoQueuedAbilityRequest(AbilityData.ActionType actionType)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void ActorTurnSM::QueueAutoQueuedAbilityRequest(AbilityData/ActionType)' called on client");
					return;
				}
			}
		}
		if (NetworkServer.active)
		{
			StoreAutoQueuedAbilityRequest(actionType);
			CallRpcStoreAutoQueuedAbilityRequest((int)actionType);
		}
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
		if (!NetworkServer.active)
		{
			if (!NetworkClient.active)
			{
				return;
			}
			if (ignoreClient)
			{
				return;
			}
		}
		GetState().OnMsg(msg, extraData);
		UpdateStates();
	}

	[Command]
	private void CmdGUITurnMessage(int msgEnum, int extraData)
	{
	}

	[Command]
	private void CmdRequestCancelAction(int action, bool hasIncomingRequest)
	{
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
		if (!(m_actorData.GetTimeBank() != null))
		{
			return;
		}
		while (true)
		{
			m_actorData.GetTimeBank().OnActionsUnconfirmed();
			return;
		}
	}

	public void OnSelectedAbilityChanged(Ability ability)
	{
		if (ability != null)
		{
			if (NetworkClient.active && CanSelectAbility())
			{
				if (!ability.IsAutoSelect())
				{
					OnMessage(TurnMessage.SELECTED_ABILITY, false);
				}
			}
		}
		GetState().OnSelectedAbilityChanged();
		if (!(Board.Get() != null))
		{
			return;
		}
		while (true)
		{
			Board.Get().MarkForUpdateValidSquares();
			return;
		}
	}

	public void SelectMovementSquare()
	{
		BoardSquare playerClampedSquare = Board.Get().PlayerClampedSquare;
		ActorData component = GetComponent<ActorData>();
		BoardSquare boardSquare = null;
		if (component != null)
		{
			boardSquare = component.MoveFromBoardSquare;
		}
		bool flag = false;
		bool flag2 = false;
		flag = (Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier));
		flag2 = (Options_UI.Get().GetShiftClickForMovementWaypoints() && InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier));
		if (boardSquare != playerClampedSquare)
		{
			InterfaceManager.Get().CancelAlert(StringUtil.TR("PostRespawnMovement", "Global"));
		}
		if (!(playerClampedSquare != null))
		{
			return;
		}
		while (true)
		{
			if (!SinglePlayerManager.IsDestinationAllowed(component, playerClampedSquare, flag))
			{
				return;
			}
			if (!m_actorData.HasQueuedMovement())
			{
				if (!m_actorData.HasQueuedChase())
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
						{
							int num;
							if (!flag2)
							{
								num = (SelectMovementSquareForChasing(playerClampedSquare) ? 1 : 0);
							}
							else
							{
								num = 0;
							}
							if (num == 0)
							{
								SelectMovementSquareForMovement(playerClampedSquare);
							}
							return;
						}
						}
					}
				}
			}
			if (m_actorData.HasQueuedChase())
			{
				while (true)
				{
					if (playerClampedSquare == m_actorData.GetQueuedChaseTarget().GetCurrentBoardSquare())
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								SelectMovementSquareForMovement(playerClampedSquare);
								return;
							}
						}
					}
					if (!SelectMovementSquareForChasing(playerClampedSquare))
					{
						while (true)
						{
							SelectMovementSquareForMovement(playerClampedSquare);
							return;
						}
					}
					return;
				}
			}
			int num2;
			if (flag)
			{
				if (component.CanMoveToBoardSquare(playerClampedSquare))
				{
					num2 = 0;
					goto IL_01db;
				}
			}
			num2 = (SelectMovementSquareForChasing(playerClampedSquare) ? 1 : 0);
			goto IL_01db;
			IL_01db:
			bool flag3 = (byte)num2 != 0;
			if (!(playerClampedSquare == boardSquare))
			{
				if (!flag3)
				{
					SelectMovementSquareForMovement(playerClampedSquare);
					return;
				}
			}
			SelectMovementSquareForChasing(playerClampedSquare);
			return;
		}
	}

	public bool SelectMovementSquareForChasing(BoardSquare selectedSquare)
	{
		bool result = false;
		ActorData component = GetComponent<ActorData>();
		if (component._0012(selectedSquare))
		{
			ActorData component2 = selectedSquare.occupant.GetComponent<ActorData>();
			result = true;
			int num;
			if (m_actorData.HasQueuedChase())
			{
				num = ((m_actorData.GetQueuedChaseTarget() == component2) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num == 0)
			{
				if (component == GameFlowData.Get().activeOwnedActorData)
				{
					UISounds.GetUISounds().Play("ui/ingame/v1/teammember_move");
				}
				StoreUndoableActionRequest(new ActionRequestForUndo(UndoableRequestType.MOVEMENT));
				CallCmdChase(selectedSquare.x, selectedSquare.y);
				NextState = TurnStateEnum.VALIDATING_MOVE_REQUEST;
				Log.Info(string.Concat("Setting State to ", NextState, " at ", GameTime.time));
				if (NetworkClient.active && component == GameFlowData.Get().activeOwnedActorData)
				{
					LineData component3 = component.GetComponent<LineData>();
					if (component3 != null)
					{
						component3.OnClientRequestedMovementChange();
					}
				}
			}
		}
		return result;
	}

	[Command]
	private void CmdChase(int selectedSquareX, int selectedSquareY)
	{
	}

	public void SelectMovementSquareForMovement(BoardSquare selectedSquare)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		list.Add(selectedSquare);
		SelectMovementSquaresForMovement(list);
	}

	public void SelectMovementSquaresForMovement(List<BoardSquare> selectedSquares)
	{
		ActorData component = GetComponent<ActorData>();
		if (GameFlowData.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get().gameState != GameState.BothTeams_Decision)
			{
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			if (SinglePlayerManager.Get() != null)
			{
				if (SinglePlayerManager.Get().GetCurrentState() != null)
				{
					if (component.GetIsHumanControlled() && SinglePlayerManager.Get().GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
					{
						while (true)
						{
							switch (3)
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
			bool flag = false;
			int num = 0;
			using (List<BoardSquare>.Enumerator enumerator = selectedSquares.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare current = enumerator.Current;
					BoardSquare boardSquare = current;
					if (!component.CanMoveToBoardSquare(boardSquare))
					{
						boardSquare = component.GetActorMovement().GetClosestMoveableSquareTo(boardSquare, false);
					}
					if (boardSquare != null)
					{
						if (component == GameFlowData.Get().activeOwnedActorData)
						{
							if (num == 0)
							{
								if (component.GetActorMovement().SquaresCanMoveTo.Count > 0)
								{
									UISounds.GetUISounds().Play("ui/ingame/v1/move");
								}
							}
						}
						bool flag2 = false;
						int num2;
						if (Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier))
						{
							num2 = (FirstTurnMovement.CanWaypoint() ? 1 : 0);
						}
						else
						{
							num2 = 0;
						}
						flag2 = ((byte)num2 != 0);
						StoreUndoableActionRequest(new ActionRequestForUndo(UndoableRequestType.MOVEMENT));
						CallCmdSetSquare(boardSquare.x, boardSquare.y, flag2);
						flag = true;
					}
					num++;
				}
			}
			if (flag)
			{
				NextState = TurnStateEnum.VALIDATING_MOVE_REQUEST;
				Log.Info(string.Concat("Setting State to ", NextState, " at ", GameTime.time));
				if (NetworkClient.active)
				{
					if (component == GameFlowData.Get().activeOwnedActorData)
					{
						LineData component2 = component.GetComponent<LineData>();
						if (component2 != null)
						{
							component2.OnClientRequestedMovementChange();
						}
					}
				}
			}
			Board.Get().MarkForUpdateValidSquares();
			return;
		}
	}

	[Command]
	private void CmdSetSquare(int x, int y, bool setWaypoint)
	{
	}

	[ClientRpc]
	private void RpcTurnMessage(int msgEnum, int extraData)
	{
		if (!m_actorData.HasBotController)
		{
			if (m_actorData == GameFlowData.Get().activeOwnedActorData && !m_actorData.IsDead())
			{
				if (msgEnum == 1)
				{
					if (GetState() != m_turnStates[0])
					{
						if (GetState() != m_turnStates[2])
						{
							if (GetState() != m_turnStates[5])
							{
								if (GetState() != m_turnStates[7])
								{
									if (m_requestStackForUndo.IsNullOrEmpty())
									{
										if (m_autoQueuedRequestActionTypes.IsNullOrEmpty())
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
									goto IL_02d4;
								}
							}
						}
					}
				}
				if (msgEnum == 0)
				{
					if (GetState() != m_turnStates[7])
					{
						if (GetState() != m_turnStates[0])
						{
							if (GetState() != m_turnStates[8])
							{
								if (GetState() != m_turnStates[5] && GetState() != m_turnStates[6])
								{
									Debug.LogError("Player " + m_actorData.DisplayName + " received TURN_START in client ActorTurnSM state " + GetState().GetType().ToString() + " which doesn't handle that transition.");
								}
							}
						}
					}
				}
			}
		}
		goto IL_02d4;
		IL_02d4:
		GetState().OnMsg((TurnMessage)msgEnum, extraData);
		UpdateStates();
	}

	[ClientRpc]
	private void RpcStoreAutoQueuedAbilityRequest(int actionTypeInt)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			StoreAutoQueuedAbilityRequest((AbilityData.ActionType)actionTypeInt);
			return;
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
		if (activeOwnedActorData != null)
		{
			if (activeOwnedActorData.GetActorTurnSM() == this)
			{
				AbilityData abilityData = activeOwnedActorData.GetAbilityData();
				Ability selectedAbility = abilityData.GetSelectedAbility();
				if (selectedAbility != null)
				{
					int targetSelectionIndex = GetTargetSelectionIndex();
					result = selectedAbility.GetTargetingParadigm(targetSelectionIndex);
				}
			}
		}
		return result;
	}

	public bool CanSelectAbility()
	{
		int result;
		if (CurrentState != 0)
		{
			if (CurrentState != TurnStateEnum.DECIDING_MOVEMENT && CurrentState != TurnStateEnum.TARGETING_ACTION)
			{
				if (CurrentState == TurnStateEnum.CONFIRMED)
				{
					result = (m_actorData.GetTimeBank().AllowUnconfirm() ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				goto IL_0060;
			}
		}
		result = 1;
		goto IL_0060;
		IL_0060:
		return (byte)result != 0;
	}

	public bool CanQueueSimpleAction()
	{
		int result;
		if (CurrentState != 0 && CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
		{
			if (CurrentState != TurnStateEnum.TARGETING_ACTION)
			{
				if (CurrentState == TurnStateEnum.CONFIRMED)
				{
					result = (m_actorData.GetTimeBank().AllowUnconfirm() ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				goto IL_005e;
			}
		}
		result = 1;
		goto IL_005e;
		IL_005e:
		return (byte)result != 0;
	}

	public bool CanPickRespawnLocation()
	{
		int result;
		if (CurrentState != TurnStateEnum.PICKING_RESPAWN)
		{
			if (CurrentState == TurnStateEnum.CONFIRMED)
			{
				result = ((PreviousState == TurnStateEnum.PICKING_RESPAWN) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool AmDecidingMovement()
	{
		int result;
		if (CurrentState != 0)
		{
			if (CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
			{
				if (CurrentState == TurnStateEnum.CONFIRMED)
				{
					result = (m_actorData.GetTimeBank().AllowUnconfirm() ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				goto IL_0055;
			}
		}
		result = 1;
		goto IL_0055;
		IL_0055:
		return (byte)result != 0;
	}

	public bool IsAbilityOrPingSelectorVisible()
	{
		return m_abilitySelectorVisible || m_timePingDown != 0f;
	}

	public static bool IsClientDecidingMovement()
	{
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						ActorTurnSM actorTurnSM = GameFlowData.Get().activeOwnedActorData.GetActorTurnSM();
						if (actorTurnSM != null)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									return actorTurnSM.AmDecidingMovement();
								}
							}
						}
						return false;
					}
					}
				}
			}
		}
		return false;
	}

	public bool AmTargetingAction()
	{
		return CurrentState == TurnStateEnum.TARGETING_ACTION;
	}

	public bool AmStillDeciding()
	{
		int result;
		if (CurrentState != 0 && CurrentState != TurnStateEnum.DECIDING_MOVEMENT && CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
		{
			if (CurrentState != TurnStateEnum.TARGETING_ACTION)
			{
				if (CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
				{
					result = ((CurrentState == TurnStateEnum.PICKING_RESPAWN) ? 1 : 0);
					goto IL_005e;
				}
			}
		}
		result = 1;
		goto IL_005e;
		IL_005e:
		return (byte)result != 0;
	}

	public bool ShouldShowGUIButtons()
	{
		int result;
		if (CurrentState != 0)
		{
			if (CurrentState != TurnStateEnum.DECIDING_MOVEMENT && CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
			{
				if (CurrentState != TurnStateEnum.TARGETING_ACTION && CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST && CurrentState != TurnStateEnum.CONFIRMED)
				{
					result = ((CurrentState == TurnStateEnum.PICKING_RESPAWN) ? 1 : 0);
					goto IL_0067;
				}
			}
		}
		result = 1;
		goto IL_0067;
		IL_0067:
		return (byte)result != 0;
	}

	public bool ShouldEnableEndTurnButton()
	{
		int result;
		if (CurrentState != 0)
		{
			if (CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
			{
				if (CurrentState != TurnStateEnum.TARGETING_ACTION)
				{
					result = ((CurrentState == TurnStateEnum.PICKING_RESPAWN) ? 1 : 0);
					goto IL_0054;
				}
			}
		}
		result = 1;
		goto IL_0054;
		IL_0054:
		return (byte)result != 0;
	}

	public bool ShouldEnableMoveButton()
	{
		int result;
		if (CurrentState != 0)
		{
			if (CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
			{
				result = ((CurrentState == TurnStateEnum.TARGETING_ACTION) ? 1 : 0);
				goto IL_003c;
			}
		}
		result = 1;
		goto IL_003c;
		IL_003c:
		return (byte)result != 0;
	}

	public bool ShouldShowEndTurnButton()
	{
		int result;
		if (ShouldShowGUIButtons())
		{
			result = ((CurrentState != TurnStateEnum.CONFIRMED) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool ShouldEnableAbilityButton(bool isSimpleAction)
	{
		int result;
		if (CurrentState != 0)
		{
			if (CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
			{
				if (CurrentState != TurnStateEnum.TARGETING_ACTION)
				{
					if (CurrentState == TurnStateEnum.CONFIRMED)
					{
						result = (isSimpleAction ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					goto IL_0061;
				}
			}
		}
		result = 1;
		goto IL_0061;
		IL_0061:
		return (byte)result != 0;
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
		if (NetworkServer.active)
		{
			return;
		}
		ActorMovement actorMovement = component.GetActorMovement();
		if (!actorMovement || GameplayUtils.IsMinion(this))
		{
			return;
		}
		while (true)
		{
			actorMovement.UpdateSquaresCanMoveTo();
			return;
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeCmdCmdGUITurnMessage(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdGUITurnMessage called on client.");
					return;
				}
			}
		}
		((ActorTurnSM)obj).CmdGUITurnMessage((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdRequestCancelAction(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdRequestCancelAction called on client.");
					return;
				}
			}
		}
		((ActorTurnSM)obj).CmdRequestCancelAction((int)reader.ReadPackedUInt32(), reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdChase(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdChase called on client.");
					return;
				}
			}
		}
		((ActorTurnSM)obj).CmdChase((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdSetSquare(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdSetSquare called on client.");
					return;
				}
			}
		}
		((ActorTurnSM)obj).CmdSetSquare((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), reader.ReadBoolean());
	}

	public void CallCmdGUITurnMessage(int msgEnum, int extraData)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdGUITurnMessage called on server.");
					return;
				}
			}
		}
		if (base.isServer)
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
		if (base.isServer)
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdChase called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					CmdChase(selectedSquareX, selectedSquareY);
					return;
				}
			}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdSetSquare called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					CmdSetSquare(x, y, setWaypoint);
					return;
				}
			}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcTurnMessage called on server.");
					return;
				}
			}
		}
		((ActorTurnSM)obj).RpcTurnMessage((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcStoreAutoQueuedAbilityRequest(NetworkBehaviour obj, NetworkReader reader)
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
					Debug.LogError("RPC RpcStoreAutoQueuedAbilityRequest called on server.");
					return;
				}
			}
		}
		((ActorTurnSM)obj).RpcStoreAutoQueuedAbilityRequest((int)reader.ReadPackedUInt32());
	}

	public void CallRpcTurnMessage(int msgEnum, int extraData)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcTurnMessage called on client.");
					return;
				}
			}
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
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
