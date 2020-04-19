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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SceneStateParameters.get_IsGroupLeader()).MethodHandle;
				}
				if (ClientGameManager.Get().GroupInfo.InAGroup)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SceneStateParameters.get_IsGroupSubordinate()).MethodHandle;
				}
				if (ClientGameManager.Get().GroupInfo.InAGroup)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SceneStateParameters.get_IsWaitingForGroup()).MethodHandle;
				}
				if (GameManager.Get().GameStatus != GameStatus.LoadoutSelecting && GameManager.Get().GameStatus != GameStatus.FreelancerSelecting)
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
					if (ClientGameManager.Get().GroupInfo != null)
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
						if (ClientGameManager.Get().GroupInfo.InAGroup)
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
							for (int i = 0; i < ClientGameManager.Get().GroupInfo.Members.Count; i++)
							{
								if (ClientGameManager.Get().GroupInfo.Members[i].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
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
									if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
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
										if (AppState_GroupCharacterSelect.Get().IsReady())
										{
											return true;
										}
									}
									if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
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
										if (AppState_CharacterSelect.IsReady())
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
											return true;
										}
									}
								}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SceneStateParameters.get_IsInGameLobby()).MethodHandle;
				}
				if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SceneStateParameters.get_IsInCustomGame()).MethodHandle;
				}
				if (GameManager.Get().GameInfo.GameConfig != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SceneStateParameters.get_PracticeGameTypeSelectedForQueue()).MethodHandle;
				}
				if (ClientGameManager.Get().GroupInfo != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SceneStateParameters.get_SelectedCharacterInGroup()).MethodHandle;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SceneStateParameters.get_SelectedCharacterFromPlayerData()).MethodHandle;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SceneStateParameters.get_SelectedCharacterFromGameInfo()).MethodHandle;
				}
				if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
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
