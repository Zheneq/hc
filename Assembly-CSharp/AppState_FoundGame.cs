using System;

public class AppState_FoundGame : AppState
{
	private static AppState_FoundGame s_instance;

	private UIOneButtonDialog m_messageBox;

	public static AppState_FoundGame Get()
	{
		return AppState_FoundGame.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_FoundGame>();
	}

	private void Awake()
	{
		AppState_FoundGame.s_instance = this;
	}

	protected override void OnEnter()
	{
		GameManager gameManager = GameManager.Get();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (gameManager.GameInfo == null)
		{
			throw new Exception("GameInfo must be set before entering app state");
		}
		if (gameManager.GameInfo.AcceptTimeout == TimeSpan.Zero)
		{
			throw new Exception("Accept timeout is zero");
		}
		WinUtils.FlashWindow();
		gameManager.OnGameSelecting += this.HandleGameSelecting;
		gameManager.OnGameStopped += this.HandleGameStopped;
		clientGameManager.OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.FoundGame, false);
	}

	protected override void OnLeave()
	{
		if (this.m_messageBox != null)
		{
			this.m_messageBox.Close();
			this.m_messageBox = null;
		}
		GameManager gameManager = GameManager.Get();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		gameManager.OnGameSelecting -= this.HandleGameSelecting;
		gameManager.OnGameStopped -= this.HandleGameStopped;
		clientGameManager.OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None, false);
	}

	protected void Update()
	{
		if (base.Elapsed == 0f)
		{
			return;
		}
		if ((double)base.Elapsed > GameManager.Get().GameInfo.AcceptTimeout.TotalSeconds)
		{
			this.OnCancelClicked();
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
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage, false);
	}
}
