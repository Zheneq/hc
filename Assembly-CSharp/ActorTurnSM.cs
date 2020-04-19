using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorTurnSM : NetworkBehaviour
{
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

	private List<ActorTurnSM.ActionRequestForUndo> m_requestStackForUndo;

	private TurnState[] m_turnStates;

	private List<AbilityTarget> m_targets;

	private static int kCmdCmdGUITurnMessage = -0x747DD45;

	private static int kCmdCmdRequestCancelAction;

	private static int kCmdCmdChase;

	private static int kCmdCmdSetSquare;

	private static int kRpcRpcTurnMessage;

	private static int kRpcRpcStoreAutoQueuedAbilityRequest;

	static ActorTurnSM()
	{
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), ActorTurnSM.kCmdCmdGUITurnMessage, new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdGUITurnMessage));
		ActorTurnSM.kCmdCmdRequestCancelAction = 0x6D2EAED3;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), ActorTurnSM.kCmdCmdRequestCancelAction, new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdRequestCancelAction));
		ActorTurnSM.kCmdCmdChase = 0x568A6C42;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), ActorTurnSM.kCmdCmdChase, new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdChase));
		ActorTurnSM.kCmdCmdSetSquare = -0x44EB058D;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorTurnSM), ActorTurnSM.kCmdCmdSetSquare, new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeCmdCmdSetSquare));
		ActorTurnSM.kRpcRpcTurnMessage = -0x66EBF78;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTurnSM), ActorTurnSM.kRpcRpcTurnMessage, new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeRpcRpcTurnMessage));
		ActorTurnSM.kRpcRpcStoreAutoQueuedAbilityRequest = 0x28449CE6;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorTurnSM), ActorTurnSM.kRpcRpcStoreAutoQueuedAbilityRequest, new NetworkBehaviour.CmdDelegate(ActorTurnSM.InvokeRpcRpcStoreAutoQueuedAbilityRequest));
		NetworkCRC.RegisterBehaviour("ActorTurnSM", 0);
	}

	private void Awake()
	{
		this.m_turnStates = new TurnState[0xB];
		this.m_turnStates[0] = new DecidingState(this);
		this.m_turnStates[1] = new ValidatingMoveRequestState(this);
		this.m_turnStates[2] = new TargetingActionState(this);
		this.m_turnStates[3] = new ValidatingActionRequestState(this);
		this.m_turnStates[4] = new DecidingMovementState(this);
		this.m_turnStates[5] = new ConfirmedState(this);
		this.m_turnStates[6] = new ResolvingState(this);
		this.m_turnStates[7] = new WaitingState(this);
		this.m_turnStates[8] = new RespawningState(this);
		this.m_turnStates[0xA] = new PickingRespawnState(this);
		this.m_turnStates[9] = new RespawningTakesActionState(this);
		TurnStateEnum turnStateEnum = TurnStateEnum.WAITING;
		this.NextState = turnStateEnum;
		turnStateEnum = turnStateEnum;
		this.PreviousState = turnStateEnum;
		this.CurrentState = turnStateEnum;
		this.m_targets = new List<AbilityTarget>();
		this.LastConfirmedCancelTurn = -1;
		this.m_requestStackForUndo = new List<ActorTurnSM.ActionRequestForUndo>();
		this.m_autoQueuedRequestActionTypes = new List<AbilityData.ActionType>();
		this.m_actorData = base.GetComponent<ActorData>();
		this.m_firstUpdate = true;
	}

	public void OnSelect()
	{
		this.HandledSpaceInput = true;
	}

	public bool SelectTarget(AbilityTarget abilityTargetToUse, bool onLockIn = false)
	{
		bool result = false;
		ActorData component = base.GetComponent<ActorData>();
		if (!SinglePlayerManager.IsAbilitysCurrentAimingAllowed(component))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.SelectTarget(AbilityTarget, bool)).MethodHandle;
			}
			return result;
		}
		AbilityData component2 = base.GetComponent<AbilityData>();
		Ability selectedAbility = component2.GetSelectedAbility();
		int num = 0;
		int num2 = 0;
		if (selectedAbility)
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
			AbilityTarget abilityTarget;
			if (abilityTargetToUse != null)
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
				abilityTarget = abilityTargetToUse.GetCopy();
			}
			else
			{
				abilityTarget = AbilityTarget.CreateAbilityTargetFromInterface();
			}
			AbilityTarget abilityTarget2 = abilityTarget;
			this.AddAbilityTarget(abilityTarget2);
			num = selectedAbility.GetNumTargets();
			num2 = selectedAbility.GetExpectedNumberOfTargeters();
			if (this.m_targets.Count <= num)
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
				if (this.m_targets.Count <= num2)
				{
					goto IL_F2;
				}
			}
			Log.Error("SelectTarget has been called more times than there are targeters for the selected ability. m_targets.Count {0}, numTargets {1}, numExpectedTargets {2}", new object[]
			{
				this.m_targets.Count,
				num,
				num2
			});
			IL_F2:
			selectedAbility.Targeter.AbilityCasted(component.\u000E(), abilityTarget2.GridPos);
		}
		if (this.GetAbilityTargets().Count < num)
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
			if (this.GetAbilityTargets().Count < num2)
			{
				goto IL_1E5;
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
		if (num != 0)
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
			AbilityData.ActionType selectedActionType = component2.GetSelectedActionType();
			this.OnQueueAbilityRequest(selectedActionType);
			result = true;
			if (NetworkClient.active && onLockIn)
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
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (selectedAbility != null && activeOwnedActorData != null)
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
					if (activeOwnedActorData == component)
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
						selectedAbility.ResetAbilityTargeters();
					}
				}
			}
			if (!NetworkServer.active)
			{
				this.SendCastAbility(selectedActionType);
			}
			goto IL_2BB;
		}
		IL_1E5:
		if (selectedAbility != null)
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
			if (num2 > 1)
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
				List<AbilityTarget> abilityTargets = this.GetAbilityTargets();
				int num3 = 0;
				while (num3 < abilityTargets.Count && num3 < selectedAbility.Targeters.Count)
				{
					AbilityUtil_Targeter abilityUtil_Targeter = selectedAbility.Targeters[num3];
					abilityUtil_Targeter.SetLastUpdateCursorState(abilityTargets[num3]);
					abilityUtil_Targeter.UpdateHighlightPosAfterClick(abilityTargets[num3], component, num3, abilityTargets);
					abilityUtil_Targeter.SetupTargetingArc(component, false);
					abilityUtil_Targeter.MarkedForForceUpdate = true;
					if (component == GameFlowData.Get().activeOwnedActorData)
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
						abilityUtil_Targeter.HideAllSquareIndicators();
					}
					num3++;
				}
			}
		}
		IL_2BB:
		if (Board.\u000E() != null)
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
			Board.\u000E().MarkForUpdateValidSquares(true);
		}
		return result;
	}

	private void CheckAbilityInput()
	{
		if (HUD_UI.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CheckAbilityInput()).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData == base.GetComponent<ActorData>())
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
				if (GameFlowData.Get().IsInDecisionState())
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
					if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightTrigger) > 0f)
					{
						if (!this.m_abilitySelectorVisible)
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
							ActorData component = base.GetComponent<ActorData>();
							AbilityData component2 = component.GetComponent<AbilityData>();
							if (component2 != null)
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
								this.m_abilitySelectorVisible = true;
								HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel.Init(component2);
								UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel, true, null);
							}
						}
						if (this.m_abilitySelectorVisible)
						{
							RectTransform rectTransform = HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel.GetComponent<UIAbilitySelectPanel>().m_line.transform as RectTransform;
							float num;
							if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY) == 0f)
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
								if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX) == 0f)
								{
									num = 0f;
									goto IL_16F;
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
							num = 200f;
							IL_16F:
							float num2 = num;
							rectTransform.sizeDelta = new Vector2(num2, 2f);
							rectTransform.pivot = new Vector2(0f, 0.5f);
							rectTransform.anchoredPosition = Vector2.zero;
							float num3 = Mathf.Atan2(ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY), ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX)) * 57.29578f;
							rectTransform.rotation = Quaternion.Euler(0f, 0f, num3);
							HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel.SelectAbilityButtonFromAngle(num3, num2);
						}
					}
					else if (this.m_abilitySelectorVisible)
					{
						this.m_abilitySelectorVisible = false;
						UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel, false, null);
						KeyPreference abilityHover = HUD_UI.Get().m_mainScreenPanel.m_abilitySelectPanel.GetAbilityHover();
						if (HUD_UI.Get() != null)
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
							if (abilityHover != KeyPreference.NullPreference)
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
								UIMainScreenPanel.Get().m_abilityBar.DoAbilityButtonClick(abilityHover);
							}
						}
					}
				}
			}
		}
	}

	private void CheckPingInput()
	{
		if (GameFlowData.Get().activeOwnedActorData == this.m_actorData)
		{
			if (this.CurrentState != TurnStateEnum.TARGETING_ACTION)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CheckPingInput()).MethodHandle;
				}
				if (Input.GetMouseButtonDown(0) && InterfaceManager.Get().ShouldHandleMouseClick())
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
					if (InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing))
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
						if (HUD_UI.Get() != null)
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
							if (Board.\u000E().PlayerFreeSquare != null)
							{
								this.m_worldPositionPingDown = Board.\u000E().PlayerFreeSquare.ToVector3();
								this.m_mousePositionPingDown = Input.mousePosition;
								this.m_timePingDown = GameTime.time;
								UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel, true, null);
								Canvas componentInParent = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponentInParent<Canvas>();
								Vector2 vector = new Vector2(this.m_mousePositionPingDown.x / (float)Screen.width - 0.5f, this.m_mousePositionPingDown.y / (float)Screen.height - 0.5f);
								Vector2 sizeDelta = (componentInParent.transform as RectTransform).sizeDelta;
								Vector2 anchoredPosition = new Vector2(vector.x * sizeDelta.x, vector.y * sizeDelta.y);
								(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.transform as RectTransform).anchoredPosition = anchoredPosition;
								UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_closeButton, false, null);
							}
						}
						return;
					}
				}
			}
			if (this.m_timePingDown != 0f)
			{
				RectTransform rectTransform = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_line.transform as RectTransform;
				Canvas componentInParent2 = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponentInParent<Canvas>();
				Vector2 vector2 = new Vector2(this.m_mousePositionPingDown.x / (float)Screen.width, this.m_mousePositionPingDown.y / (float)Screen.height);
				Vector2 sizeDelta2 = (componentInParent2.transform as RectTransform).sizeDelta;
				Vector2 b = new Vector2(vector2.x * sizeDelta2.x, vector2.y * sizeDelta2.y);
				Vector2 vector3 = new Vector2(Input.mousePosition.x / (float)Screen.width, Input.mousePosition.y / (float)Screen.height);
				Vector2 a = new Vector2(vector3.x * sizeDelta2.x, vector3.y * sizeDelta2.y);
				Vector2 vector4 = a - b;
				rectTransform.sizeDelta = new Vector2(vector4.magnitude, 2f);
				rectTransform.pivot = new Vector2(0f, 0.5f);
				rectTransform.anchoredPosition = Vector2.zero;
				float z = Mathf.Atan2(vector4.y, vector4.x) * 57.29578f;
				rectTransform.rotation = Quaternion.Euler(0f, 0f, z);
				if (GameTime.time < this.m_timePingDown + 0.25f)
				{
					if (Input.GetMouseButtonUp(0))
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
						BigPingPanel component = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>();
						ActorController.PingType pingType = component.GetPingType();
						UIManager.SetGameObjectActive(component, false, null);
						this.m_timePingDown = 0f;
						HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(this.m_worldPositionPingDown, pingType);
					}
				}
				else if (Input.GetMouseButtonUp(0))
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
					BigPingPanel component2 = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>();
					ActorController.PingType pingType2 = component2.GetPingType();
					UIManager.SetGameObjectActive(component2, false, null);
					this.m_timePingDown = 0f;
					if (pingType2 != ActorController.PingType.Default)
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
						HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(this.m_worldPositionPingDown, pingType2);
					}
				}
				else if (!HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_closeButton.activeSelf)
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
					UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanel.GetComponent<BigPingPanel>().m_closeButton, true, null);
				}
			}
		}
	}

	private void CheckAbilityInputControlPad()
	{
		if (GameFlowData.Get().activeOwnedActorData == base.GetComponent<ActorData>() && ControlpadGameplay.Get() != null && ControlpadGameplay.Get().UsingControllerInput)
		{
			if (this.m_timePingDown == 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CheckAbilityInputControlPad()).MethodHandle;
				}
				if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftTrigger) > 0f)
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
					if (HUD_UI.Get() != null && Board.\u000E().PlayerFreeSquare != null)
					{
						this.m_timePingDown = GameTime.time;
						HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.Init();
						UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad, true, null);
						this.m_worldPositionPingDown = Board.\u000E().PlayerFreeSquare.ToVector3();
						Canvas componentInParent = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponentInParent<Canvas>();
						Vector2 sizeDelta = (componentInParent.transform as RectTransform).sizeDelta;
						Vector3 position = Board.\u000E().PlayerClampedSquare.ToVector3();
						if (Board.\u000E().PlayerClampedSquare.height < 0)
						{
							position.y = (float)Board.\u000E().BaselineHeight;
						}
						Canvas x = (!(HUD_UI.Get() != null)) ? null : HUD_UI.Get().GetTopLevelCanvas();
						if (x != null)
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
							Vector2 vector = Camera.main.WorldToViewportPoint(position);
							Vector2 anchoredPosition = new Vector2(vector.x * sizeDelta.x - sizeDelta.x * 0.5f, vector.y * sizeDelta.y - sizeDelta.y * 0.5f);
							(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.transform as RectTransform).anchoredPosition = anchoredPosition;
						}
						UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_closeButton, false, null);
					}
					return;
				}
			}
			if (this.m_timePingDown != 0f)
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
				RectTransform rectTransform = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_line.transform as RectTransform;
				float num;
				if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY) == 0f)
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
					if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX) == 0f)
					{
						num = 0f;
						goto IL_2E0;
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
				num = 200f;
				IL_2E0:
				float num2 = num;
				rectTransform.sizeDelta = new Vector2(num2, 2f);
				rectTransform.pivot = new Vector2(0f, 0.5f);
				rectTransform.anchoredPosition = Vector2.zero;
				float num3 = Mathf.Atan2(ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickY), ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftStickX)) * 57.29578f;
				rectTransform.rotation = Quaternion.Euler(0f, 0f, num3);
				HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.SelectAbilityButtonFromAngle(num3, num2);
				if (GameTime.time < this.m_timePingDown + 0.25f)
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
					if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftTrigger) == 0f)
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
						ActorController.PingType pingType = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetPingType();
						UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad, false, null);
						this.m_timePingDown = 0f;
						HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(this.m_worldPositionPingDown, pingType);
					}
				}
				else if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.LeftTrigger) == 0f)
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
					ActorController.PingType pingType2 = HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetPingType();
					UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad, false, null);
					this.m_timePingDown = 0f;
					if (pingType2 != ActorController.PingType.Default)
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
						HUD_UI.Get().m_mainScreenPanel.m_minimap.SendMiniMapPing(this.m_worldPositionPingDown, pingType2);
					}
				}
				else if (!HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_closeButton.activeSelf)
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
					UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_bigPingPanelControlpad.GetComponent<BigPingPanelControlpad>().m_closeButton, true, null);
				}
			}
		}
	}

	private void Update()
	{
		this.UpdateStates();
		this.UpdateCancelKey();
		if (this.LockInBuffered)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.Update()).MethodHandle;
			}
			if (this.CheckStateForEndTurnRequestFromInput())
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
				Log.Info("Buffered lock in at " + GameTime.time, new object[0]);
				UISounds.GetUISounds().Play("ui/ingame/v1/hud/lockin");
				this.RequestEndTurn();
				if (HUD_UI.Get() != null)
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
					HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.LockedInClicked();
				}
				this.LockInBuffered = false;
			}
			if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
			{
				Log.Info("Stop lock in buffer during resolve", new object[0]);
				this.LockInBuffered = false;
			}
		}
		this.CheckAbilityInput();
		this.CheckPingInput();
		this.CheckAbilityInputControlPad();
		this.DisplayActorState();
		if (this.m_firstUpdate)
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
			this.m_firstUpdate = false;
		}
	}

	private void LateUpdate()
	{
		this.HandledSpaceInput = false;
		this.HandledMouseInput = false;
	}

	private void UpdateStates()
	{
		do
		{
			this.SwitchToNewStates();
			this.GetState().Update();
		}
		while (this.NextState != this.CurrentState);
	}

	[Client]
	internal void SendCastAbility(AbilityData.ActionType actionType)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.SendCastAbility(AbilityData.ActionType)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void ActorTurnSM::SendCastAbility(AbilityData/ActionType)' called on server");
			return;
		}
		GameFlow.Get().SendCastAbility(base.GetComponent<ActorData>(), actionType, this.GetAbilityTargets());
	}

	internal void RequestCancel(bool onlyCancelConfirm = false)
	{
		if (SinglePlayerManager.IsCancelDisabled())
		{
			return;
		}
		bool flag = false;
		TurnStateEnum currentState = this.CurrentState;
		if (currentState != TurnStateEnum.DECIDING_MOVEMENT)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.RequestCancel(bool)).MethodHandle;
			}
			if (currentState == TurnStateEnum.CONFIRMED)
			{
				if (this.m_actorData.\u000E().AllowUnconfirm())
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
					this.LastConfirmedCancelTurn = GameFlowData.Get().CurrentTurn;
					this.m_actorData.\u000E().OnActionsUnconfirmed();
					this.BackToDecidingState();
					if (Options_UI.Get() != null)
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
						if (Options_UI.Get().ShouldCancelActionWhileConfirmed())
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
							flag = true;
						}
					}
				}
				goto IL_D0;
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
			if (currentState != TurnStateEnum.DECIDING)
			{
				this.BackToDecidingState();
				goto IL_D0;
			}
		}
		flag = true;
		IL_D0:
		if (!onlyCancelConfirm)
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
			if (flag)
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
				if (this.m_requestStackForUndo.Count != 0)
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
					int index = this.m_requestStackForUndo.Count - 1;
					ActorTurnSM.ActionRequestForUndo actionRequestForUndo = this.m_requestStackForUndo[index];
					this.m_requestStackForUndo.RemoveAt(index);
					this.m_actorData.OnClientQueuedActionChanged();
					UndoableRequestType type = actionRequestForUndo.m_type;
					if (type != UndoableRequestType.MOVEMENT)
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
						if (type == UndoableRequestType.ABILITY_QUEUE)
						{
							this.RequestCancelAction(actionRequestForUndo.m_action, false);
							UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
						}
					}
					else
					{
						this.RequestCancelMovement();
						UISounds.GetUISounds().Play("ui/ingame/v1/move_undo");
					}
				}
			}
		}
		Board.\u000E().MarkForUpdateValidSquares(true);
	}

	public void BackToDecidingState()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.BackToDecidingState()).MethodHandle;
			}
			if (!NetworkClient.active)
			{
				goto IL_42;
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
			if (this.CurrentState != TurnStateEnum.TARGETING_ACTION)
			{
				goto IL_42;
			}
		}
		this.OnMessage(TurnMessage.CANCEL_BUTTON_CLICKED, false);
		IL_42:
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
			this.CallCmdGUITurnMessage(4, 0);
		}
	}

	private void UpdateCancelKey()
	{
		if (GameFlowData.Get().activeOwnedActorData == base.GetComponent<ActorData>())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.UpdateCancelKey()).MethodHandle;
			}
			ActorData component = base.GetComponent<ActorData>();
			bool flag = component.HasQueuedMovement();
			AbilityData component2 = base.GetComponent<AbilityData>();
			bool flag2 = component2.HasQueuedAbilities();
			bool flag3 = true;
			if (InputManager.Get().IsKeyCodeMatchKeyBind(KeyPreference.CancelAction, KeyCode.Escape))
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
				if (Input.GetKeyDown(KeyCode.Escape))
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
					if (UISystemEscapeMenu.Get() != null)
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
						if (UISystemEscapeMenu.Get().IsOpen())
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
							flag3 = false;
							UISystemEscapeMenu.Get().OnToggleButtonClick(null);
							goto IL_18C;
						}
					}
					if (UIGameStatsWindow.Get() != null)
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
						if (UIGameStatsWindow.Get().m_container.gameObject.activeSelf)
						{
							flag3 = false;
							UIGameStatsWindow.Get().ToggleStatsWindow();
							goto IL_18C;
						}
					}
					if (Options_UI.Get().IsVisible())
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
						flag3 = false;
						Options_UI.Get().HideOptions();
					}
					else if (KeyBinding_UI.Get().IsVisible())
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
						flag3 = false;
						if (!KeyBinding_UI.Get().IsSettingKeybindCommand())
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
							KeyBinding_UI.Get().HideKeybinds();
						}
					}
				}
			}
			IL_18C:
			if (!flag)
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
				if (!flag2)
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
					if (this.CurrentState == TurnStateEnum.DECIDING)
					{
						return;
					}
				}
			}
			if (flag3)
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
				if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CancelAction))
				{
					this.RequestCancel(false);
				}
			}
		}
	}

	public bool CheckStateForEndTurnRequestFromInput()
	{
		if (this.CurrentState == TurnStateEnum.DECIDING)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CheckStateForEndTurnRequestFromInput()).MethodHandle;
			}
			if (this.NextState == TurnStateEnum.DECIDING)
			{
				return true;
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
		return this.CurrentState == TurnStateEnum.PICKING_RESPAWN;
	}

	public void UpdateEndTurnKey()
	{
		if (GameFlowData.Get().activeOwnedActorData == base.GetComponent<ActorData>() && this.ShouldShowEndTurnButton())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.UpdateEndTurnKey()).MethodHandle;
			}
			if (this.ShouldEnableEndTurnButton() && InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.LockIn) && !this.HandledSpaceInput)
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
				if (!UITutorialFullscreenPanel.Get().IsAnyPanelVisible())
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
					if (AppState.GetCurrent() != AppState_InGameDeployment.Get())
					{
						if (this.CheckStateForEndTurnRequestFromInput())
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
							this.HandledSpaceInput = true;
							UISounds.GetUISounds().Play("ui/ingame/v1/hud/lockin");
							Log.Info("Lock in request at " + GameTime.time, new object[0]);
							this.RequestEndTurn();
							if (HUD_UI.Get() != null)
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
								HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.LockedInClicked();
							}
						}
						else
						{
							if (this.CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
							{
								if (this.CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
								{
									return;
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
							this.LockInBuffered = true;
							Log.Info("Lockin to be buffered at " + GameTime.time, new object[0]);
						}
					}
				}
			}
		}
	}

	public unsafe void GetActionText(out string textStr, out Color textColor)
	{
		AbilityData component = base.GetComponent<AbilityData>();
		Ability ability = null;
		if (component)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.GetActionText(string*, Color*)).MethodHandle;
			}
			ability = component.GetSelectedAbility();
		}
		if (this.CurrentState == TurnStateEnum.CONFIRMED)
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
			if (ability != null)
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
				textColor = Color.green;
				textStr = ability.m_abilityName;
			}
			else
			{
				ActorData component2 = base.GetComponent<ActorData>();
				bool flag = component2.HasQueuedMovement();
				bool flag2 = component2.HasQueuedChase();
				if (flag2)
				{
					textColor = ActorTurnSM.s_chasingTextColor;
					textStr = "Chasing";
				}
				else if (flag)
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
					textColor = ActorTurnSM.s_movingTextColor;
					textStr = "Moving";
				}
				else
				{
					textColor = ActorTurnSM.s_movingTextColor;
					textStr = "Staying";
				}
			}
		}
		else if (this.CurrentState == TurnStateEnum.DECIDING || this.CurrentState == TurnStateEnum.DECIDING_MOVEMENT)
		{
			textColor = ActorTurnSM.s_decidingTextColor;
			textStr = "(Deciding...)";
		}
		else
		{
			textColor = ActorTurnSM.s_decidingTextColor;
			textStr = string.Empty;
		}
	}

	public void RequestEndTurn()
	{
		ActorData component = base.GetComponent<ActorData>();
		if (this.AmTargetingAction())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.RequestEndTurn()).MethodHandle;
			}
			Ability selectedAbility = component.\u000E().GetSelectedAbility();
			do
			{
				if (this.SelectTarget(null, true))
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
					this.NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
				}
				if (!(selectedAbility != null) || !selectedAbility.ShouldAutoConfirmIfTargetingOnEndTurn())
				{
					goto IL_92;
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
			while (this.m_targets.Count < selectedAbility.GetExpectedNumberOfTargeters());
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
		IL_92:
		if (SinglePlayerManager.Get() != null)
		{
			SinglePlayerManager.Get().OnEndTurnRequested(component);
		}
		if (SinglePlayerManager.CanEndTurn(component))
		{
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
				this.OnMessage(TurnMessage.DONE_BUTTON_CLICKED, true);
			}
			else
			{
				this.CallCmdGUITurnMessage(0xE, 0);
			}
			GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterLocked, null);
			if (component == GameFlowData.Get().activeOwnedActorData)
			{
				GameFlowData.Get().SetActiveNextNonConfirmedOwnedActorData();
			}
		}
		Board.\u000E().MarkForUpdateValidSquares(true);
	}

	public void RequestCancelMovement()
	{
		if (SinglePlayerManager.IsCancelDisabled())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.RequestCancelMovement()).MethodHandle;
			}
			return;
		}
		if (NetworkServer.active)
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
			this.OnMessage(TurnMessage.CANCEL_MOVEMENT, 0, true);
		}
		else
		{
			this.CallCmdGUITurnMessage(0x10, 0);
		}
		ActorData component = base.GetComponent<ActorData>();
		if (component == GameFlowData.Get().activeOwnedActorData)
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
			if (component.\u0015())
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
				if (SpawnPointManager.Get() != null)
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
					if (SpawnPointManager.Get().m_spawnInDuringMovement)
					{
						InterfaceManager.Get().DisplayAlert(StringUtil.TR("PostRespawnMovement", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true, 0);
					}
				}
			}
		}
		if (NetworkClient.active)
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
			if (component == GameFlowData.Get().activeOwnedActorData)
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
				LineData component2 = component.GetComponent<LineData>();
				if (component2 != null)
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
					component2.OnClientRequestedMovementChange();
				}
			}
		}
	}

	public void RequestCancelAction(AbilityData.ActionType actionType, bool hasPendingStoreRequest)
	{
		if (SinglePlayerManager.IsCancelDisabled())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.RequestCancelAction(AbilityData.ActionType, bool)).MethodHandle;
			}
			return;
		}
		this.CancelUndoableAbilityRequest(actionType);
		this.CancelAutoQueuedAbilityRequest(actionType);
		this.CallCmdRequestCancelAction((int)actionType, hasPendingStoreRequest);
	}

	public void RequestMove()
	{
		this.StoreUndoableActionRequest(new ActorTurnSM.ActionRequestForUndo(UndoableRequestType.MOVEMENT, AbilityData.ActionType.INVALID_ACTION));
		if (NetworkServer.active)
		{
			this.OnMessage(TurnMessage.MOVE_BUTTON_CLICKED, true);
		}
		else
		{
			this.CallCmdGUITurnMessage(0xD, 0);
		}
	}

	private void DisplayActorState()
	{
	}

	private void SwitchToNewStates()
	{
		if (this.NextState != this.CurrentState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.SwitchToNewStates()).MethodHandle;
			}
			this.GetState().OnExit();
			this.PreviousState = this.CurrentState;
			this.CurrentState = this.NextState;
			if (UIMainScreenPanel.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
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
				if (GameFlowData.Get().activeOwnedActorData == this.m_actorData)
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
					if (this.CurrentState == TurnStateEnum.TARGETING_ACTION)
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
						UIMainScreenPanel.Get().m_targetingCursor.ShowTargetCursor();
					}
					else
					{
						UIMainScreenPanel.Get().m_targetingCursor.HideTargetCursor();
						Cursor.visible = true;
					}
				}
			}
			this.GetState().OnEnter();
			if (CameraManager.Get() != null)
			{
				CameraManager.Get().OnNewTurnSMState();
			}
			if (Board.\u000E() != null)
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
				Board.\u000E().MarkForUpdateValidSquares(true);
			}
		}
	}

	public bool LockInBuffered { get; set; }

	public TurnStateEnum CurrentState { get; private set; }

	public TurnStateEnum PreviousState { get; private set; }

	public TurnStateEnum NextState
	{
		get
		{
			return this._NextState;
		}
		set
		{
			if (value >= TurnStateEnum.CONFIRMED)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.set_NextState(TurnStateEnum)).MethodHandle;
				}
				if (this._NextState < TurnStateEnum.CONFIRMED)
				{
					this._LockInTime = DateTime.UtcNow;
					goto IL_5C;
				}
			}
			if (value < TurnStateEnum.CONFIRMED && this._NextState >= TurnStateEnum.CONFIRMED)
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
				this._TurnStart = DateTime.UtcNow;
				this._LockInTime = DateTime.MinValue;
			}
			IL_5C:
			this._NextState = value;
		}
	}

	public TimeSpan TimeToLockIn
	{
		get
		{
			if (this._LockInTime == DateTime.MinValue)
			{
				return TimeSpan.Zero;
			}
			return this._LockInTime - this._TurnStart;
		}
	}

	public void ResetTurnStartNow()
	{
		this._TurnStart = DateTime.UtcNow;
	}

	public bool HandledSpaceInput { get; set; }

	public bool HandledMouseInput { get; set; }

	internal int LastConfirmedCancelTurn { get; private set; }

	public bool IsKeyDown(KeyCode keyCode)
	{
		bool result;
		if (CameraControls.Get().Enabled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.IsKeyDown(KeyCode)).MethodHandle;
			}
			result = Input.GetKeyDown(keyCode);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public List<AbilityData.ActionType> GetAutoQueuedRequestActionTypes()
	{
		return this.m_autoQueuedRequestActionTypes;
	}

	public void StoreAutoQueuedAbilityRequest(AbilityData.ActionType actionType)
	{
		if (!this.m_autoQueuedRequestActionTypes.Contains(actionType))
		{
			this.m_autoQueuedRequestActionTypes.Add(actionType);
		}
	}

	private void CancelAutoQueuedAbilityRequest(AbilityData.ActionType actionType)
	{
		if (this.m_autoQueuedRequestActionTypes.Contains(actionType))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CancelAutoQueuedAbilityRequest(AbilityData.ActionType)).MethodHandle;
			}
			this.m_autoQueuedRequestActionTypes.Remove(actionType);
		}
	}

	public List<ActorTurnSM.ActionRequestForUndo> GetRequestStackForUndo()
	{
		return this.m_requestStackForUndo;
	}

	private void StoreUndoableActionRequest(ActorTurnSM.ActionRequestForUndo request)
	{
		bool flag = false;
		if (request.m_type == UndoableRequestType.MOVEMENT)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.StoreUndoableActionRequest(ActorTurnSM.ActionRequestForUndo)).MethodHandle;
			}
			using (List<ActorTurnSM.ActionRequestForUndo>.Enumerator enumerator = this.m_requestStackForUndo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorTurnSM.ActionRequestForUndo actionRequestForUndo = enumerator.Current;
					if (actionRequestForUndo.m_type == UndoableRequestType.MOVEMENT)
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
		if (!flag)
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
			this.m_requestStackForUndo.Add(request);
			this.m_actorData.OnClientQueuedActionChanged();
		}
	}

	private void CancelUndoableAbilityRequest(AbilityData.ActionType actionType)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CancelUndoableAbilityRequest(AbilityData.ActionType)).MethodHandle;
			}
			if (actionType != AbilityData.ActionType.INVALID_ACTION)
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
				ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
				if (component.IsAbilityCinematicRequested(actionType))
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
					activeOwnedActorData.GetComponent<ActorCinematicRequests>().SendAbilityCinematicRequest(actionType, false, -1, -1);
				}
			}
		}
		int num = -1;
		for (int i = 0; i < this.m_requestStackForUndo.Count; i++)
		{
			if (this.m_requestStackForUndo[i].m_type == UndoableRequestType.ABILITY_QUEUE)
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
				if (this.m_requestStackForUndo[i].m_action == actionType)
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
					num = i;
					break;
				}
			}
		}
		if (num != -1)
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
			this.m_requestStackForUndo.RemoveAt(num);
			this.m_actorData.OnClientQueuedActionChanged();
			if (HUD_UI.Get() != null)
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
				HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.CancelAbilityRequest(actionType);
			}
		}
	}

	internal void OnQueueAbilityRequest(AbilityData.ActionType actionType)
	{
		AbilityData component = base.GetComponent<AbilityData>();
		List<AbilityData.ActionType> list = null;
		bool flag = false;
		if (component.GetActionsToCancelOnTargetingComplete(ref list, ref flag))
		{
			if (flag)
			{
				this.RequestCancelMovement();
			}
			if (list != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.OnQueueAbilityRequest(AbilityData.ActionType)).MethodHandle;
				}
				foreach (AbilityData.ActionType actionType2 in list)
				{
					this.RequestCancelAction(actionType2, true);
				}
				if (list.Count > 0)
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
					UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
				}
			}
			component.ClearActionsToCancelOnTargetingComplete();
		}
		this.StoreUndoableActionRequest(new ActorTurnSM.ActionRequestForUndo(UndoableRequestType.ABILITY_QUEUE, actionType));
	}

	[Server]
	internal void QueueAutoQueuedAbilityRequest(AbilityData.ActionType actionType)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.QueueAutoQueuedAbilityRequest(AbilityData.ActionType)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ActorTurnSM::QueueAutoQueuedAbilityRequest(AbilityData/ActionType)' called on client");
			return;
		}
		if (NetworkServer.active)
		{
			this.StoreAutoQueuedAbilityRequest(actionType);
			this.CallRpcStoreAutoQueuedAbilityRequest((int)actionType);
		}
	}

	private TurnState GetState()
	{
		return this.m_turnStates[(int)this.CurrentState];
	}

	public void ClearAbilityTargets()
	{
		this.m_targets.Clear();
	}

	public void AddAbilityTarget(AbilityTarget newTarget)
	{
		this.m_targets.Add(newTarget);
	}

	public List<AbilityTarget> GetAbilityTargets()
	{
		return this.m_targets;
	}

	public void OnMessage(TurnMessage msg, bool ignoreClient = true)
	{
		this.OnMessage(msg, 0, ignoreClient);
	}

	public void OnMessage(TurnMessage msg, int extraData, bool ignoreClient = true)
	{
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.OnMessage(TurnMessage, int, bool)).MethodHandle;
			}
			this.CallRpcTurnMessage((int)msg, extraData);
		}
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
			if (!NetworkClient.active)
			{
				return;
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
			if (ignoreClient)
			{
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
		this.GetState().OnMsg(msg, extraData);
		this.UpdateStates();
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
		if (this.m_actorData.\u000E() != null)
		{
			this.m_actorData.\u000E().OnActionsConfirmed();
		}
	}

	public void OnActionsUnconfirmed()
	{
		if (this.m_actorData.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.OnActionsUnconfirmed()).MethodHandle;
			}
			this.m_actorData.\u000E().OnActionsUnconfirmed();
		}
	}

	public void OnSelectedAbilityChanged(Ability ability)
	{
		if (ability != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.OnSelectedAbilityChanged(Ability)).MethodHandle;
			}
			if (NetworkClient.active && this.CanSelectAbility())
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
				if (!ability.IsAutoSelect())
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
					this.OnMessage(TurnMessage.SELECTED_ABILITY, false);
				}
			}
		}
		this.GetState().OnSelectedAbilityChanged();
		if (Board.\u000E() != null)
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
			Board.\u000E().MarkForUpdateValidSquares(true);
		}
	}

	public void SelectMovementSquare()
	{
		BoardSquare playerClampedSquare = Board.\u000E().PlayerClampedSquare;
		ActorData component = base.GetComponent<ActorData>();
		BoardSquare boardSquare = null;
		if (component != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.SelectMovementSquare()).MethodHandle;
			}
			boardSquare = component.MoveFromBoardSquare;
		}
		bool flag = Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier);
		bool flag2 = Options_UI.Get().GetShiftClickForMovementWaypoints() && InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier);
		if (boardSquare != playerClampedSquare)
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
			InterfaceManager.Get().CancelAlert(StringUtil.TR("PostRespawnMovement", "Global"));
		}
		if (playerClampedSquare != null)
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
			if (SinglePlayerManager.IsDestinationAllowed(component, playerClampedSquare, flag))
			{
				if (!this.m_actorData.HasQueuedMovement())
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
					if (!this.m_actorData.HasQueuedChase())
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
						bool flag3;
						if (!flag2)
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
							flag3 = this.SelectMovementSquareForChasing(playerClampedSquare);
						}
						else
						{
							flag3 = false;
						}
						if (!flag3)
						{
							this.SelectMovementSquareForMovement(playerClampedSquare);
						}
						return;
					}
				}
				if (this.m_actorData.HasQueuedChase())
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
					if (playerClampedSquare == this.m_actorData.\u0012().\u0012())
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
						this.SelectMovementSquareForMovement(playerClampedSquare);
					}
					else if (!this.SelectMovementSquareForChasing(playerClampedSquare))
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
						this.SelectMovementSquareForMovement(playerClampedSquare);
					}
				}
				else
				{
					bool flag4;
					if (flag)
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
						if (component.CanMoveToBoardSquare(playerClampedSquare))
						{
							flag4 = false;
							goto IL_1DB;
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
					flag4 = this.SelectMovementSquareForChasing(playerClampedSquare);
					IL_1DB:
					bool flag5 = flag4;
					if (!(playerClampedSquare == boardSquare))
					{
						if (!flag5)
						{
							this.SelectMovementSquareForMovement(playerClampedSquare);
							return;
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
					this.SelectMovementSquareForChasing(playerClampedSquare);
				}
			}
		}
	}

	public bool SelectMovementSquareForChasing(BoardSquare selectedSquare)
	{
		bool result = false;
		ActorData component = base.GetComponent<ActorData>();
		if (component.\u0012(selectedSquare))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.SelectMovementSquareForChasing(BoardSquare)).MethodHandle;
			}
			ActorData component2 = selectedSquare.occupant.GetComponent<ActorData>();
			result = true;
			bool flag;
			if (this.m_actorData.HasQueuedChase())
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
				flag = (this.m_actorData.\u0012() == component2);
			}
			else
			{
				flag = false;
			}
			if (!flag)
			{
				if (component == GameFlowData.Get().activeOwnedActorData)
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
					UISounds.GetUISounds().Play("ui/ingame/v1/teammember_move");
				}
				this.StoreUndoableActionRequest(new ActorTurnSM.ActionRequestForUndo(UndoableRequestType.MOVEMENT, AbilityData.ActionType.INVALID_ACTION));
				this.CallCmdChase(selectedSquare.x, selectedSquare.y);
				this.NextState = TurnStateEnum.VALIDATING_MOVE_REQUEST;
				Log.Info(string.Concat(new object[]
				{
					"Setting State to ",
					this.NextState,
					" at ",
					GameTime.time
				}), new object[0]);
				if (NetworkClient.active && component == GameFlowData.Get().activeOwnedActorData)
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
					LineData component3 = component.GetComponent<LineData>();
					if (component3 != null)
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
		this.SelectMovementSquaresForMovement(new List<BoardSquare>
		{
			selectedSquare
		});
	}

	public void SelectMovementSquaresForMovement(List<BoardSquare> selectedSquares)
	{
		ActorData component = base.GetComponent<ActorData>();
		if (!(GameFlowData.Get() == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.SelectMovementSquaresForMovement(List<BoardSquare>)).MethodHandle;
			}
			if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
			{
				if (SinglePlayerManager.Get() != null)
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
					if (SinglePlayerManager.Get().GetCurrentState() != null)
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
						if (component.\u0019() && SinglePlayerManager.Get().GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
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
							return;
						}
					}
				}
				bool flag = false;
				int num = 0;
				using (List<BoardSquare>.Enumerator enumerator = selectedSquares.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardSquare boardSquare = enumerator.Current;
						BoardSquare boardSquare2 = boardSquare;
						if (!component.CanMoveToBoardSquare(boardSquare2))
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
							boardSquare2 = component.\u000E().GetClosestMoveableSquareTo(boardSquare2, false);
						}
						if (boardSquare2 != null)
						{
							if (component == GameFlowData.Get().activeOwnedActorData)
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
								if (num == 0)
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
									if (component.\u000E().SquaresCanMoveTo.Count > 0)
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
										UISounds.GetUISounds().Play("ui/ingame/v1/move");
									}
								}
							}
							bool flag2;
							if (Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier))
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
								flag2 = FirstTurnMovement.CanWaypoint();
							}
							else
							{
								flag2 = false;
							}
							bool setWaypoint = flag2;
							this.StoreUndoableActionRequest(new ActorTurnSM.ActionRequestForUndo(UndoableRequestType.MOVEMENT, AbilityData.ActionType.INVALID_ACTION));
							this.CallCmdSetSquare(boardSquare2.x, boardSquare2.y, setWaypoint);
							flag = true;
						}
						num++;
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
					this.NextState = TurnStateEnum.VALIDATING_MOVE_REQUEST;
					Log.Info(string.Concat(new object[]
					{
						"Setting State to ",
						this.NextState,
						" at ",
						GameTime.time
					}), new object[0]);
					if (NetworkClient.active)
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
						if (component == GameFlowData.Get().activeOwnedActorData)
						{
							LineData component2 = component.GetComponent<LineData>();
							if (component2 != null)
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
								component2.OnClientRequestedMovementChange();
							}
						}
					}
				}
				Board.\u000E().MarkForUpdateValidSquares(true);
				return;
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
	}

	[Command]
	private void CmdSetSquare(int x, int y, bool setWaypoint)
	{
	}

	[ClientRpc]
	private void RpcTurnMessage(int msgEnum, int extraData)
	{
		if (!this.m_actorData.HasBotController)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.RpcTurnMessage(int, int)).MethodHandle;
			}
			if (this.m_actorData == GameFlowData.Get().activeOwnedActorData && !this.m_actorData.\u000E())
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
				if (msgEnum == 1)
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
					if (this.GetState() != this.m_turnStates[0])
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
						if (this.GetState() != this.m_turnStates[2])
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
							if (this.GetState() != this.m_turnStates[5])
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
								if (this.GetState() != this.m_turnStates[7])
								{
									if (this.m_requestStackForUndo.IsNullOrEmpty<ActorTurnSM.ActionRequestForUndo>())
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
										if (this.m_autoQueuedRequestActionTypes.IsNullOrEmpty<AbilityData.ActionType>())
										{
											int num = -1;
											string text = "(none)";
											ActorController actorController = this.m_actorData.\u000E();
											if (actorController != null)
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
												Ability lastTargetedAbility = actorController.GetLastTargetedAbility(ref num);
												if (lastTargetedAbility != null)
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
													text = lastTargetedAbility.m_abilityName;
												}
											}
											Debug.LogError(string.Concat(new string[]
											{
												"Player ",
												this.m_actorData.DisplayName,
												" skipped turn (could be AFK) in client ActorTurnSM state ",
												this.GetState().GetType().ToString(),
												". LastTargetedAbility: ",
												text,
												", targeterIndex: ",
												num.ToString(),
												". GuiUtility.hotControl: ",
												GUIUtility.hotControl.ToString()
											}));
										}
									}
									goto IL_2D4;
								}
							}
						}
					}
				}
				if (msgEnum == 0)
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
					if (this.GetState() != this.m_turnStates[7])
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
						if (this.GetState() != this.m_turnStates[0])
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
							if (this.GetState() != this.m_turnStates[8])
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
								if (this.GetState() != this.m_turnStates[5] && this.GetState() != this.m_turnStates[6])
								{
									Debug.LogError(string.Concat(new string[]
									{
										"Player ",
										this.m_actorData.DisplayName,
										" received TURN_START in client ActorTurnSM state ",
										this.GetState().GetType().ToString(),
										" which doesn't handle that transition."
									}));
								}
							}
						}
					}
				}
			}
		}
		IL_2D4:
		this.GetState().OnMsg((TurnMessage)msgEnum, extraData);
		this.UpdateStates();
	}

	[ClientRpc]
	private void RpcStoreAutoQueuedAbilityRequest(int actionTypeInt)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.RpcStoreAutoQueuedAbilityRequest(int)).MethodHandle;
			}
			this.StoreAutoQueuedAbilityRequest((AbilityData.ActionType)actionTypeInt);
		}
	}

	public int GetTargetSelectionIndex()
	{
		int result = -1;
		TurnState state = this.GetState();
		if (state is TargetingActionState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.GetTargetSelectionIndex()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.GetSelectedTargetingParadigm()).MethodHandle;
			}
			if (activeOwnedActorData.\u000E() == this)
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
				AbilityData abilityData = activeOwnedActorData.\u000E();
				Ability selectedAbility = abilityData.GetSelectedAbility();
				if (selectedAbility != null)
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
					int targetSelectionIndex = this.GetTargetSelectionIndex();
					result = selectedAbility.GetTargetingParadigm(targetSelectionIndex);
				}
			}
		}
		return result;
	}

	public bool CanSelectAbility()
	{
		int result;
		if (this.CurrentState != TurnStateEnum.DECIDING)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CanSelectAbility()).MethodHandle;
			}
			if (this.CurrentState != TurnStateEnum.DECIDING_MOVEMENT && this.CurrentState != TurnStateEnum.TARGETING_ACTION)
			{
				if (this.CurrentState == TurnStateEnum.CONFIRMED)
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
					result = (this.m_actorData.\u000E().AllowUnconfirm() ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return result != 0;
			}
		}
		result = 1;
		return result != 0;
	}

	public bool CanQueueSimpleAction()
	{
		int result;
		if (this.CurrentState != TurnStateEnum.DECIDING && this.CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CanQueueSimpleAction()).MethodHandle;
			}
			if (this.CurrentState != TurnStateEnum.TARGETING_ACTION)
			{
				if (this.CurrentState == TurnStateEnum.CONFIRMED)
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
					result = (this.m_actorData.\u000E().AllowUnconfirm() ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return result != 0;
			}
		}
		result = 1;
		return result != 0;
	}

	public bool CanPickRespawnLocation()
	{
		bool result;
		if (this.CurrentState != TurnStateEnum.PICKING_RESPAWN)
		{
			if (this.CurrentState == TurnStateEnum.CONFIRMED)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CanPickRespawnLocation()).MethodHandle;
				}
				result = (this.PreviousState == TurnStateEnum.PICKING_RESPAWN);
			}
			else
			{
				result = false;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool AmDecidingMovement()
	{
		int result;
		if (this.CurrentState != TurnStateEnum.DECIDING)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.AmDecidingMovement()).MethodHandle;
			}
			if (this.CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
			{
				if (this.CurrentState == TurnStateEnum.CONFIRMED)
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
					result = (this.m_actorData.\u000E().AllowUnconfirm() ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return result != 0;
			}
		}
		result = 1;
		return result != 0;
	}

	public bool IsAbilityOrPingSelectorVisible()
	{
		return this.m_abilitySelectorVisible || this.m_timePingDown != 0f;
	}

	public static bool IsClientDecidingMovement()
	{
		bool result;
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.IsClientDecidingMovement()).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != null)
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
				ActorTurnSM actorTurnSM = GameFlowData.Get().activeOwnedActorData.\u000E();
				if (actorTurnSM != null)
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
					result = actorTurnSM.AmDecidingMovement();
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
		result = false;
		return result;
	}

	public bool AmTargetingAction()
	{
		return this.CurrentState == TurnStateEnum.TARGETING_ACTION;
	}

	public bool AmStillDeciding()
	{
		if (this.CurrentState != TurnStateEnum.DECIDING && this.CurrentState != TurnStateEnum.DECIDING_MOVEMENT && this.CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.AmStillDeciding()).MethodHandle;
			}
			if (this.CurrentState != TurnStateEnum.TARGETING_ACTION)
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
				if (this.CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
				{
					return this.CurrentState == TurnStateEnum.PICKING_RESPAWN;
				}
			}
		}
		return true;
	}

	public bool ShouldShowGUIButtons()
	{
		if (this.CurrentState != TurnStateEnum.DECIDING)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.ShouldShowGUIButtons()).MethodHandle;
			}
			if (this.CurrentState != TurnStateEnum.DECIDING_MOVEMENT && this.CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
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
				if (this.CurrentState != TurnStateEnum.TARGETING_ACTION && this.CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST && this.CurrentState != TurnStateEnum.CONFIRMED)
				{
					return this.CurrentState == TurnStateEnum.PICKING_RESPAWN;
				}
			}
		}
		return true;
	}

	public bool ShouldEnableEndTurnButton()
	{
		if (this.CurrentState != TurnStateEnum.DECIDING)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.ShouldEnableEndTurnButton()).MethodHandle;
			}
			if (this.CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
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
				if (this.CurrentState != TurnStateEnum.TARGETING_ACTION)
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
					return this.CurrentState == TurnStateEnum.PICKING_RESPAWN;
				}
			}
		}
		return true;
	}

	public bool ShouldEnableMoveButton()
	{
		if (this.CurrentState != TurnStateEnum.DECIDING)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.ShouldEnableMoveButton()).MethodHandle;
			}
			if (this.CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
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
				return this.CurrentState == TurnStateEnum.TARGETING_ACTION;
			}
		}
		return true;
	}

	public bool ShouldShowEndTurnButton()
	{
		bool result;
		if (this.ShouldShowGUIButtons())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.ShouldShowEndTurnButton()).MethodHandle;
			}
			result = (this.CurrentState != TurnStateEnum.CONFIRMED);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool ShouldEnableAbilityButton(bool isSimpleAction)
	{
		int result;
		if (this.CurrentState != TurnStateEnum.DECIDING)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.ShouldEnableAbilityButton(bool)).MethodHandle;
			}
			if (this.CurrentState != TurnStateEnum.DECIDING_MOVEMENT)
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
				if (this.CurrentState != TurnStateEnum.TARGETING_ACTION)
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
					if (this.CurrentState == TurnStateEnum.CONFIRMED)
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
						result = (isSimpleAction ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return result != 0;
				}
			}
		}
		result = 1;
		return result != 0;
	}

	public void SetupForNewTurn()
	{
		ActorData component = base.GetComponent<ActorData>();
		if (HUD_UI.Get() != null && component == GameFlowData.Get().activeOwnedActorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.SetupForNewTurn()).MethodHandle;
			}
			HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay.Decision);
		}
		if (component == GameFlowData.Get().activeOwnedActorData)
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
			HighlightUtils.Get().SetCursorType(HighlightUtils.CursorType.MouseOverCursorType);
		}
		component.\u000E().ResetTurn();
		this.ClearAbilityTargets();
		this.m_requestStackForUndo.Clear();
		this.m_autoQueuedRequestActionTypes.Clear();
		if (!NetworkServer.active)
		{
			ActorMovement actorMovement = component.\u000E();
			if (actorMovement && !GameplayUtils.IsMinion(this))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.InvokeCmdCmdGUITurnMessage(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdGUITurnMessage called on client.");
			return;
		}
		((ActorTurnSM)obj).CmdGUITurnMessage((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdRequestCancelAction(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.InvokeCmdCmdRequestCancelAction(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdRequestCancelAction called on client.");
			return;
		}
		((ActorTurnSM)obj).CmdRequestCancelAction((int)reader.ReadPackedUInt32(), reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdChase(NetworkBehaviour obj, NetworkReader reader)
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.InvokeCmdCmdChase(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdChase called on client.");
			return;
		}
		((ActorTurnSM)obj).CmdChase((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdSetSquare(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.InvokeCmdCmdSetSquare(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdSetSquare called on client.");
			return;
		}
		((ActorTurnSM)obj).CmdSetSquare((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), reader.ReadBoolean());
	}

	public void CallCmdGUITurnMessage(int msgEnum, int extraData)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CallCmdGUITurnMessage(int, int)).MethodHandle;
			}
			Debug.LogError("Command function CmdGUITurnMessage called on server.");
			return;
		}
		if (base.isServer)
		{
			this.CmdGUITurnMessage(msgEnum, extraData);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorTurnSM.kCmdCmdGUITurnMessage);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)msgEnum);
		networkWriter.WritePackedUInt32((uint)extraData);
		base.SendCommandInternal(networkWriter, 0, "CmdGUITurnMessage");
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
			this.CmdRequestCancelAction(action, hasIncomingRequest);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorTurnSM.kCmdCmdRequestCancelAction);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)action);
		networkWriter.Write(hasIncomingRequest);
		base.SendCommandInternal(networkWriter, 0, "CmdRequestCancelAction");
	}

	public void CallCmdChase(int selectedSquareX, int selectedSquareY)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CallCmdChase(int, int)).MethodHandle;
			}
			Debug.LogError("Command function CmdChase called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdChase(selectedSquareX, selectedSquareY);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorTurnSM.kCmdCmdChase);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)selectedSquareX);
		networkWriter.WritePackedUInt32((uint)selectedSquareY);
		base.SendCommandInternal(networkWriter, 0, "CmdChase");
	}

	public void CallCmdSetSquare(int x, int y, bool setWaypoint)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CallCmdSetSquare(int, int, bool)).MethodHandle;
			}
			Debug.LogError("Command function CmdSetSquare called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdSetSquare(x, y, setWaypoint);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorTurnSM.kCmdCmdSetSquare);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)x);
		networkWriter.WritePackedUInt32((uint)y);
		networkWriter.Write(setWaypoint);
		base.SendCommandInternal(networkWriter, 0, "CmdSetSquare");
	}

	protected static void InvokeRpcRpcTurnMessage(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.InvokeRpcRpcTurnMessage(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcTurnMessage called on server.");
			return;
		}
		((ActorTurnSM)obj).RpcTurnMessage((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcStoreAutoQueuedAbilityRequest(NetworkBehaviour obj, NetworkReader reader)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.InvokeRpcRpcStoreAutoQueuedAbilityRequest(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcStoreAutoQueuedAbilityRequest called on server.");
			return;
		}
		((ActorTurnSM)obj).RpcStoreAutoQueuedAbilityRequest((int)reader.ReadPackedUInt32());
	}

	public void CallRpcTurnMessage(int msgEnum, int extraData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorTurnSM.CallRpcTurnMessage(int, int)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcTurnMessage called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorTurnSM.kRpcRpcTurnMessage);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)msgEnum);
		networkWriter.WritePackedUInt32((uint)extraData);
		this.SendRPCInternal(networkWriter, 0, "RpcTurnMessage");
	}

	public void CallRpcStoreAutoQueuedAbilityRequest(int actionTypeInt)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcStoreAutoQueuedAbilityRequest called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorTurnSM.kRpcRpcStoreAutoQueuedAbilityRequest);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionTypeInt);
		this.SendRPCInternal(networkWriter, 0, "RpcStoreAutoQueuedAbilityRequest");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result;
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}

	public class ActionRequestForUndo
	{
		public UndoableRequestType m_type;

		public AbilityData.ActionType m_action;

		public ActionRequestForUndo(UndoableRequestType requestType, AbilityData.ActionType actionType = AbilityData.ActionType.INVALID_ACTION)
		{
			this.m_type = requestType;
			this.m_action = actionType;
		}
	}
}
