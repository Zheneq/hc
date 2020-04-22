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
				while (true)
				{
					switch (1)
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
				if (ClientGameManager.Get().GroupInfo.InAGroup)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
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
				while (true)
				{
					switch (2)
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
				if (ClientGameManager.Get().GroupInfo.InAGroup)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
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
				while (true)
				{
					switch (2)
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
				if (GameManager.Get().GameStatus != GameStatus.LoadoutSelecting && GameManager.Get().GameStatus != GameStatus.FreelancerSelecting)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (ClientGameManager.Get().GroupInfo != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (ClientGameManager.Get().GroupInfo.InAGroup)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							for (int i = 0; i < ClientGameManager.Get().GroupInfo.Members.Count; i++)
							{
								if (ClientGameManager.Get().GroupInfo.Members[i].AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
								{
									continue;
								}
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (AppState_GroupCharacterSelect.Get().IsReady())
									{
										return true;
									}
								}
								if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
								{
									continue;
								}
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!AppState_CharacterSelect.IsReady())
								{
									continue;
								}
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									return true;
								}
							}
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
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
				while (true)
				{
					switch (2)
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
				if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
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
				while (true)
				{
					switch (2)
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
				if (GameManager.Get().GameInfo.GameConfig != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
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
				while (true)
				{
					switch (2)
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
				if (ClientGameManager.Get().GroupInfo != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
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
				while (true)
				{
					switch (1)
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
				if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
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
