using LobbyGameClientMessages;
using System;

public class AppState_WaitingForGame : AppState
{
	private static AppState_WaitingForGame s_instance;

	private UIOneButtonDialog m_messageBox;

	private bool m_wasRequeued;

	public static AppState_WaitingForGame Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_WaitingForGame>();
	}

	public void Enter(bool wasRequeued)
	{
		m_wasRequeued = wasRequeued;
		base.Enter();
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void Update()
	{
	}

	protected override void OnEnter()
	{
		GameManager gameManager = GameManager.Get();
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.WaitingForGame);
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnQueueStatusNotification += HandleQueueStatusNotification;
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		gameManager.OnGameAssembling += HandleGameAssembling;
		gameManager.OnGameSelecting += HandleGameSelecting;
		if (!m_wasRequeued || !(m_messageBox == null))
		{
			return;
		}
		while (true)
		{
			m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("SomeoneDroppedReaddedQueue", "Global"), StringUtil.TR("Ok", "Global"));
			return;
		}
	}

	protected override void OnLeave()
	{
		if (m_messageBox != null)
		{
			m_messageBox.Close();
			m_messageBox = null;
		}
		m_wasRequeued = false;
		GameManager gameManager = GameManager.Get();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnQueueStatusNotification -= HandleQueueStatusNotification;
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None);
		gameManager.OnGameAssembling -= HandleGameAssembling;
		gameManager.OnGameSelecting -= HandleGameSelecting;
	}

	public void HandleCancelClicked()
	{
		Log.Info("Clicked on cancel, leaving Queue");
		ClientGameManager clientGameManager = ClientGameManager.Get();
		LobbyGameClientInterface lobbyInterface = clientGameManager.LobbyInterface;
		
		lobbyInterface.LeaveQueue(delegate
			{
				AppState_GameTypeSelect.Get().Enter();
			});
	}

	public void HandleQueueStatusNotification(MatchmakingQueueStatusNotification notification)
	{
	}

	private void HandleGameAssembling()
	{
		if (GameManager.Get().GameInfo.AcceptTimeout != TimeSpan.Zero)
		{
			AppState_FoundGame.Get().Enter();
		}
	}

	private void HandleGameSelecting()
	{
		AppState_CharacterSelect.Get().Enter();
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage);
	}
}
