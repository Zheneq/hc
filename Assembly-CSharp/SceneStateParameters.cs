using System;

public class SceneStateParameters
{
	public UIManager.ClientState? NewClientGameState;

	public static bool IsHUDHidden
	{
		get
		{
			return UIScreenManager.Get().GetHideHUDCompletely();
		}
	}

	public static bool IsGroupLeader
	{
		get
		{
			if (ClientGameManager.Get().GroupInfo != null)
			{
				if (ClientGameManager.Get().GroupInfo.InAGroup)
				{
					return ClientGameManager.Get().GroupInfo.IsLeader;
				}
			}
			return false;
		}
	}

	public static bool IsGroupSubordinate
	{
		get
		{
			if (ClientGameManager.Get().GroupInfo != null)
			{
				if (ClientGameManager.Get().GroupInfo.InAGroup)
				{
					return !ClientGameManager.Get().GroupInfo.IsLeader;
				}
			}
			return false;
		}
	}

	public static bool IsInQueue
	{
		get
		{
			return GameManager.Get().QueueInfo != null;
		}
	}

	public static bool IsWaitingForGroup
	{
		get
		{
			if (GameManager.Get().QueueInfo == null)
			{
				if (GameManager.Get().GameStatus != GameStatus.LoadoutSelecting && GameManager.Get().GameStatus != GameStatus.FreelancerSelecting)
				{
					if (ClientGameManager.Get().GroupInfo != null)
					{
						if (ClientGameManager.Get().GroupInfo.InAGroup)
						{
							for (int i = 0; i < ClientGameManager.Get().GroupInfo.Members.Count; i++)
							{
								if (ClientGameManager.Get().GroupInfo.Members[i].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
								{
									if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
									{
										if (AppState_GroupCharacterSelect.Get().IsReady())
										{
											return true;
										}
									}
									if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
									{
										if (AppState_CharacterSelect.IsReady())
										{
											return true;
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
	}

	public static bool IsInGameLobby
	{
		get
		{
			if (GameManager.Get().GameInfo != null && GameManager.Get().PlayerInfo != null)
			{
				if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
				{
					return GameManager.Get().GameStatus != GameStatus.None;
				}
			}
			return false;
		}
	}

	public static bool IsInCustomGame
	{
		get
		{
			if (GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
			{
				if (GameManager.Get().GameInfo.GameConfig != null)
				{
					return GameManager.Get().GameInfo.GameConfig.GameType == GameType.Custom;
				}
			}
			return false;
		}
	}

	public static bool PracticeGameTypeSelectedForQueue
	{
		get
		{
			if (!SceneStateParameters.IsInGameLobby)
			{
				if (ClientGameManager.Get().GroupInfo != null)
				{
					return ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Practice;
				}
			}
			return false;
		}
	}

	public static TimeSpan TimeInQueue
	{
		get
		{
			return (!(ClientGameManager.Get().QueueEntryTime == DateTime.MinValue)) ? (DateTime.UtcNow - ClientGameManager.Get().QueueEntryTime) : TimeSpan.FromMinutes(0.0);
		}
	}

	public static CharacterType SelectedCharacterInGroup
	{
		get
		{
			if (ClientGameManager.Get().GroupInfo != null && ClientGameManager.Get().GroupInfo.InAGroup)
			{
				return ClientGameManager.Get().GroupInfo.ChararacterInfo.CharacterType;
			}
			return CharacterType.None;
		}
	}

	public static CharacterType SelectedCharacterFromPlayerData
	{
		get
		{
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				return ClientGameManager.Get().GetPlayerAccountData().AccountComponent.LastCharacter;
			}
			return CharacterType.None;
		}
	}

	public static CharacterType SelectedCharacterFromGameInfo
	{
		get
		{
			if (GameManager.Get().GameInfo != null)
			{
				if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
				{
					if (GameManager.Get().PlayerInfo != null)
					{
						return GameManager.Get().PlayerInfo.CharacterType;
					}
				}
			}
			return CharacterType.None;
		}
	}
}
