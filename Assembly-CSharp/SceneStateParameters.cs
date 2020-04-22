using System;

public class SceneStateParameters
{
	public UIManager.ClientState? NewClientGameState;

	public static bool IsHUDHidden => UIScreenManager.Get().GetHideHUDCompletely();

	public static bool IsGroupLeader
	{
		get
		{
			int result;
			if (ClientGameManager.Get().GroupInfo != null)
			{
				if (ClientGameManager.Get().GroupInfo.InAGroup)
				{
					result = (ClientGameManager.Get().GroupInfo.IsLeader ? 1 : 0);
					goto IL_0055;
				}
			}
			result = 0;
			goto IL_0055;
			IL_0055:
			return (byte)result != 0;
		}
	}

	public static bool IsGroupSubordinate
	{
		get
		{
			int result;
			if (ClientGameManager.Get().GroupInfo != null)
			{
				if (ClientGameManager.Get().GroupInfo.InAGroup)
				{
					result = ((!ClientGameManager.Get().GroupInfo.IsLeader) ? 1 : 0);
					goto IL_0059;
				}
			}
			result = 0;
			goto IL_0059;
			IL_0059:
			return (byte)result != 0;
		}
	}

	public static bool IsInQueue => GameManager.Get().QueueInfo != null;

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
								if (ClientGameManager.Get().GroupInfo.Members[i].AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
								{
									continue;
								}
								if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
								{
									if (AppState_GroupCharacterSelect.Get().IsReady())
									{
										return true;
									}
								}
								if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
								{
									continue;
								}
								if (!AppState_CharacterSelect.IsReady())
								{
									continue;
								}
								while (true)
								{
									return true;
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
			int result;
			if (GameManager.Get().GameInfo != null && GameManager.Get().PlayerInfo != null)
			{
				if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
				{
					result = ((GameManager.Get().GameStatus != GameStatus.None) ? 1 : 0);
					goto IL_0065;
				}
			}
			result = 0;
			goto IL_0065;
			IL_0065:
			return (byte)result != 0;
		}
	}

	public static bool IsInCustomGame
	{
		get
		{
			int result;
			if (GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
			{
				if (GameManager.Get().GameInfo.GameConfig != null)
				{
					result = ((GameManager.Get().GameInfo.GameConfig.GameType == GameType.Custom) ? 1 : 0);
					goto IL_0073;
				}
			}
			result = 0;
			goto IL_0073;
			IL_0073:
			return (byte)result != 0;
		}
	}

	public static bool PracticeGameTypeSelectedForQueue
	{
		get
		{
			int result;
			if (!IsInGameLobby)
			{
				if (ClientGameManager.Get().GroupInfo != null)
				{
					result = ((ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Practice) ? 1 : 0);
					goto IL_004d;
				}
			}
			result = 0;
			goto IL_004d;
			IL_004d:
			return (byte)result != 0;
		}
	}

	public static TimeSpan TimeInQueue => (!(ClientGameManager.Get().QueueEntryTime == DateTime.MinValue)) ? (DateTime.UtcNow - ClientGameManager.Get().QueueEntryTime) : TimeSpan.FromMinutes(0.0);

	public static CharacterType SelectedCharacterInGroup
	{
		get
		{
			if (ClientGameManager.Get().GroupInfo != null && ClientGameManager.Get().GroupInfo.InAGroup)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return ClientGameManager.Get().GroupInfo.ChararacterInfo.CharacterType;
					}
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return ClientGameManager.Get().GetPlayerAccountData().AccountComponent.LastCharacter;
					}
				}
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
