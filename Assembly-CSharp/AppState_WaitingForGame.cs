using System;
using LobbyGameClientMessages;

public class AppState_WaitingForGame : AppState
{
	private static AppState_WaitingForGame s_instance;

	private UIOneButtonDialog m_messageBox;

	private bool m_wasRequeued;

	public static AppState_WaitingForGame Get()
	{
		return AppState_WaitingForGame.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_WaitingForGame>();
	}

	public void Enter(bool wasRequeued)
	{
		this.m_wasRequeued = wasRequeued;
		base.Enter();
	}

	private void Awake()
	{
		AppState_WaitingForGame.s_instance = this;
	}

	private void Update()
	{
	}

	protected override void OnEnter()
	{
		GameManager gameManager = GameManager.Get();
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.WaitingForGame, false);
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnQueueStatusNotification += this.HandleQueueStatusNotification;
		clientGameManager.OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
		gameManager.OnGameAssembling += this.HandleGameAssembling;
		gameManager.OnGameSelecting += this.HandleGameSelecting;
		if (this.m_wasRequeued && this.m_messageBox == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_WaitingForGame.OnEnter()).MethodHandle;
			}
			this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("SomeoneDroppedReaddedQueue", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
		}
	}

	protected override void OnLeave()
	{
		if (this.m_messageBox != null)
		{
			this.m_messageBox.Close();
			this.m_messageBox = null;
		}
		this.m_wasRequeued = false;
		GameManager gameManager = GameManager.Get();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnQueueStatusNotification -= this.HandleQueueStatusNotification;
		clientGameManager.OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None, false);
		gameManager.OnGameAssembling -= this.HandleGameAssembling;
		gameManager.OnGameSelecting -= this.HandleGameSelecting;
	}

	public void HandleCancelClicked()
	{
		Log.Info("Clicked on cancel, leaving Queue", new object[0]);
		ClientGameManager clientGameManager = ClientGameManager.Get();
		LobbyGameClientInterface lobbyInterface = clientGameManager.LobbyInterface;
		if (AppState_WaitingForGame.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_WaitingForGame.HandleCancelClicked()).MethodHandle;
			}
			AppState_WaitingForGame.<>f__am$cache0 = delegate(LeaveMatchmakingQueueResponse response)
			{
				AppState_GameTypeSelect.Get().Enter();
			};
		}
		lobbyInterface.LeaveQueue(AppState_WaitingForGame.<>f__am$cache0);
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
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage, false);
	}
}
