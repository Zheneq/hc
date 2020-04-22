using System;

public class AppState_FoundGame : AppState
{
	private static AppState_FoundGame s_instance;

	private UIOneButtonDialog m_messageBox;

	public static AppState_FoundGame Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_FoundGame>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		GameManager gameManager = GameManager.Get();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (gameManager.GameInfo == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					throw new Exception("GameInfo must be set before entering app state");
				}
			}
		}
		if (gameManager.GameInfo.AcceptTimeout == TimeSpan.Zero)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					throw new Exception("Accept timeout is zero");
				}
			}
		}
		WinUtils.FlashWindow();
		gameManager.OnGameSelecting += HandleGameSelecting;
		gameManager.OnGameStopped += HandleGameStopped;
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.FoundGame);
	}

	protected override void OnLeave()
	{
		if (m_messageBox != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_messageBox.Close();
			m_messageBox = null;
		}
		GameManager gameManager = GameManager.Get();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		gameManager.OnGameSelecting -= HandleGameSelecting;
		gameManager.OnGameStopped -= HandleGameStopped;
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None);
	}

	protected void Update()
	{
		if (base.Elapsed == 0f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (!((double)base.Elapsed > GameManager.Get().GameInfo.AcceptTimeout.TotalSeconds))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			OnCancelClicked();
			return;
		}
	}

	public void OnReadyClicked()
	{
		ClientGameManager.Get().UpdateReadyState(ReadyState.Accepted, null, null, null);
	}

	public void OnCancelClicked()
	{
		ClientGameManager.Get().UpdateReadyState(ReadyState.Declined, null, null, null);
	}

	private void HandleGameSelecting()
	{
		AppState_CharacterSelect.Get().Enter();
	}

	private void HandleGameStopped(GameResult gameResult)
	{
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage);
	}
}
