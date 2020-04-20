using System;
using System.Diagnostics;
using UnityEngine;

public class ClientIdleTimer : MonoBehaviour
{
	private float m_timeSinceInput;

	private float m_timeSinceMatchStart;

	private Vector3 m_previousMousePosition;

	private UIDialogBox m_idleWarningDialog;

	private static ClientIdleTimer s_instance;

	public static ClientIdleTimer Get()
	{
		if (ClientIdleTimer.s_instance == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientIdleTimer.Get()).MethodHandle;
			}
			Log.Error("ClientIdleTimer component is not present on a bootstrap singleton!", new object[0]);
		}
		return ClientIdleTimer.s_instance;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		ClientIdleTimer.s_instance = this;
	}

	private void OnDestroy()
	{
		ClientIdleTimer.s_instance = null;
	}

	private void Update()
	{
		if (!HydrogenConfig.Get().EnableNoInputIdleDisconnect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientIdleTimer.Update()).MethodHandle;
			}
			return;
		}
		if (Application.isEditor)
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
			return;
		}
		if (Debugger.IsAttached)
		{
			return;
		}
		if (DebugParameters.Get() != null)
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
			if (DebugParameters.Get().GetParameterAsBool("DisableBotTakeover"))
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
		if (GameFlowData.Get() != null && GameFlowData.Get().GetPause())
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
		}
		else
		{
			if (GameManager.Get() != null)
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
				if (GameManager.Get().GameConfig != null)
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
					if (!GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.NoInputIdleDisconnect))
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
						return;
					}
				}
			}
			if (UILandingPageFullScreenMenus.Get() != null && UILandingPageFullScreenMenus.Get().IsVideoVisible())
			{
				return;
			}
			if (ClientGameManager.Get().IsFastForward)
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
				return;
			}
			if (ReplayPlayManager.Get() != null)
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
				if (ReplayPlayManager.Get().IsPlayback())
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
					return;
				}
			}
			bool flag = false;
			bool flag2 = false;
			if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
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
				if (AppState_GroupCharacterSelect.Get().InQueue())
				{
					flag = true;
					this.m_timeSinceMatchStart = 0f;
					goto IL_2BD;
				}
			}
			if (!(AppState.GetCurrent() == AppState_FoundGame.Get()))
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
				if (!(AppState.GetCurrent() == AppState_GameLoading.Get()))
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
					if (!(AppState.GetCurrent() == AppState_InGameStarting.Get()))
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
						if (!(AppState.GetCurrent() == AppState_InGameDeployment.Get()))
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
							if (AppState.GetCurrent() == AppState_WaitingForGame.Get())
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
							}
							else
							{
								if (!(AppState.GetCurrent() == AppState_InGameDecision.Get()))
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
									if (!(AppState.GetCurrent() == AppState_InGameResolve.Get()))
									{
										goto IL_2BD;
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
								if (GameFlowData.Get() != null)
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
									flag2 = true;
									this.m_timeSinceMatchStart += Time.deltaTime;
									goto IL_2BD;
								}
								goto IL_2BD;
							}
						}
					}
				}
			}
			flag = true;
			this.m_timeSinceMatchStart = 0f;
			IL_2BD:
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
					this.m_timeSinceInput = 0f;
					return;
				}
			}
			bool anyKey = Input.anyKey;
			bool flag3 = (this.m_previousMousePosition - Input.mousePosition).sqrMagnitude > 0.001f;
			this.m_previousMousePosition = Input.mousePosition;
			if (!anyKey)
			{
				if (!flag3)
				{
					goto IL_34A;
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
			if (AccountPreferences.DoesApplicationHaveFocus())
			{
				this.m_timeSinceInput = 0f;
				this.HideWarningDialog();
				return;
			}
			IL_34A:
			if (flag2)
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (GameFlowData.Get().gameState == GameState.BothTeams_Decision && activeOwnedActorData != null && activeOwnedActorData.GetActorTurnSM() != null)
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
					if (!activeOwnedActorData.GetActorTurnSM().GetRequestStackForUndo().IsNullOrEmpty<ActorTurnSM.ActionRequestForUndo>())
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
				if (activeOwnedActorData == null || activeOwnedActorData.IsDead())
				{
					return;
				}
			}
			this.m_timeSinceInput += Time.deltaTime;
			if (flag2)
			{
				if (GameFlowData.Get().CurrentTurn == 1)
				{
					if (this.m_timeSinceMatchStart > 12f)
					{
						if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
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
							if (this.m_timeSinceInput > HydrogenConfig.Get().NoInputIdleDisconnectTimeMatchStart)
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
								this.HideWarningDialog();
								ClientGameManager.Get().LeaveGame(false, GameResult.ClientIdleTimeout);
								this.m_timeSinceInput = 0f;
								goto IL_483;
							}
						}
						if (this.m_timeSinceInput > HydrogenConfig.Get().NoInputIdleWarningTimeMatchStart)
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
							this.ShowWarningDialog();
						}
					}
					IL_483:;
				}
				else if (this.m_timeSinceInput > HydrogenConfig.Get().NoInputIdleDisconnectTime)
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
					this.HideWarningDialog();
					ClientGameManager.Get().LeaveGame(false, GameResult.ClientIdleTimeout);
					this.m_timeSinceInput = 0f;
				}
				else if (this.m_timeSinceInput > HydrogenConfig.Get().NoInputIdleWarningTime)
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
					this.ShowWarningDialog();
				}
				else
				{
					this.HideWarningDialog();
				}
			}
			return;
		}
	}

	private void ShowWarningDialog()
	{
		if (this.m_idleWarningDialog == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientIdleTimer.ShowWarningDialog()).MethodHandle;
			}
			this.m_idleWarningDialog = UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("InactivityWarning", "Global"), StringUtil.TR("YouWillBeDisconnected", "Global"), StringUtil.TR("DontKickMe", "Global"), null, -1, false);
		}
	}

	private void HideWarningDialog()
	{
		if (this.m_idleWarningDialog != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientIdleTimer.HideWarningDialog()).MethodHandle;
			}
			UIDialogPopupManager.Get().CloseDialog(this.m_idleWarningDialog);
			this.m_idleWarningDialog = null;
		}
	}

	public void ResetIdleTimer()
	{
		this.m_timeSinceInput = 0f;
	}
}
