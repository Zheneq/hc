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
		if (s_instance == null)
		{
			Log.Error("ClientIdleTimer component is not present on a bootstrap singleton!");
		}
		return s_instance;
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Update()
	{
		if (!HydrogenConfig.Get().EnableNoInputIdleDisconnect)
		{
			while (true)
			{
				return;
			}
		}
		if (Application.isEditor)
		{
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
		if (Debugger.IsAttached)
		{
			return;
		}
		if (DebugParameters.Get() != null)
		{
			if (DebugParameters.Get().GetParameterAsBool("DisableBotTakeover"))
			{
				return;
			}
		}
		if (GameFlowData.Get() != null && GameFlowData.Get().GetPause())
		{
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
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameConfig != null)
			{
				if (!GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.NoInputIdleDisconnect))
				{
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
			}
		}
		if (UILandingPageFullScreenMenus.Get() != null && UILandingPageFullScreenMenus.Get().IsVideoVisible())
		{
			return;
		}
		if (ClientGameManager.Get().IsFastForward)
		{
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
		if (ReplayPlayManager.Get() != null)
		{
			if (ReplayPlayManager.Get().IsPlayback())
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
		}
		bool flag = false;
		bool flag2 = false;
		if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
		{
			if (AppState_GroupCharacterSelect.Get().InQueue())
			{
				flag = true;
				m_timeSinceMatchStart = 0f;
				goto IL_02bd;
			}
		}
		if (!(AppState.GetCurrent() == AppState_FoundGame.Get()))
		{
			if (!(AppState.GetCurrent() == AppState_GameLoading.Get()))
			{
				if (!(AppState.GetCurrent() == AppState_InGameStarting.Get()))
				{
					if (!(AppState.GetCurrent() == AppState_InGameDeployment.Get()))
					{
						if (!(AppState.GetCurrent() == AppState_WaitingForGame.Get()))
						{
							if (!(AppState.GetCurrent() == AppState_InGameDecision.Get()))
							{
								if (!(AppState.GetCurrent() == AppState_InGameResolve.Get()))
								{
									goto IL_02bd;
								}
							}
							if (GameFlowData.Get() != null)
							{
								flag2 = true;
								m_timeSinceMatchStart += Time.deltaTime;
							}
							goto IL_02bd;
						}
					}
				}
			}
		}
		flag = true;
		m_timeSinceMatchStart = 0f;
		goto IL_02bd;
		IL_034a:
		if (flag2)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (GameFlowData.Get().gameState == GameState.BothTeams_Decision && activeOwnedActorData != null && activeOwnedActorData.GetActorTurnSM() != null)
			{
				if (!activeOwnedActorData.GetActorTurnSM().GetRequestStackForUndo().IsNullOrEmpty())
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
			if (activeOwnedActorData == null || activeOwnedActorData.IsDead())
			{
				return;
			}
		}
		m_timeSinceInput += Time.deltaTime;
		if (!flag2)
		{
			return;
		}
		if (GameFlowData.Get().CurrentTurn == 1)
		{
			if (!(m_timeSinceMatchStart > 12f))
			{
				return;
			}
			if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
			{
				if (m_timeSinceInput > HydrogenConfig.Get().NoInputIdleDisconnectTimeMatchStart)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							HideWarningDialog();
							ClientGameManager.Get().LeaveGame(false, GameResult.ClientIdleTimeout);
							m_timeSinceInput = 0f;
							return;
						}
					}
				}
			}
			if (!(m_timeSinceInput > HydrogenConfig.Get().NoInputIdleWarningTimeMatchStart))
			{
				return;
			}
			while (true)
			{
				ShowWarningDialog();
				return;
			}
		}
		if (m_timeSinceInput > HydrogenConfig.Get().NoInputIdleDisconnectTime)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					HideWarningDialog();
					ClientGameManager.Get().LeaveGame(false, GameResult.ClientIdleTimeout);
					m_timeSinceInput = 0f;
					return;
				}
			}
		}
		if (m_timeSinceInput > HydrogenConfig.Get().NoInputIdleWarningTime)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					ShowWarningDialog();
					return;
				}
			}
		}
		HideWarningDialog();
		return;
		IL_02bd:
		if (!flag)
		{
			if (!flag2)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_timeSinceInput = 0f;
						return;
					}
				}
			}
		}
		bool anyKey = Input.anyKey;
		bool flag3 = (m_previousMousePosition - Input.mousePosition).sqrMagnitude > 0.001f;
		m_previousMousePosition = Input.mousePosition;
		if (!anyKey)
		{
			if (!flag3)
			{
				goto IL_034a;
			}
		}
		if (AccountPreferences.DoesApplicationHaveFocus())
		{
			m_timeSinceInput = 0f;
			HideWarningDialog();
			return;
		}
		goto IL_034a;
	}

	private void ShowWarningDialog()
	{
		if (!(m_idleWarningDialog == null))
		{
			return;
		}
		while (true)
		{
			m_idleWarningDialog = UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("InactivityWarning", "Global"), StringUtil.TR("YouWillBeDisconnected", "Global"), StringUtil.TR("DontKickMe", "Global"));
			return;
		}
	}

	private void HideWarningDialog()
	{
		if (!(m_idleWarningDialog != null))
		{
			return;
		}
		while (true)
		{
			UIDialogPopupManager.Get().CloseDialog(m_idleWarningDialog);
			m_idleWarningDialog = null;
			return;
		}
	}

	public void ResetIdleTimer()
	{
		m_timeSinceInput = 0f;
	}
}
