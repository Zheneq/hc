using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using UnityEngine;

[Serializable]
public class Replay
{
	public string m_gameplayOverrides_Serialized;

	public string m_gameInfo_Serialized;

	public string m_teamInfo_Serialized;

	public string m_versionMini;

	public string m_versionFull;

	public int m_playerInfo_Index = -1;

	public List<Replay.Message> m_messages = new List<Replay.Message>();

	private float m_initialMessageTimestamp;

	private int m_messageReadIndex;

	private PersistedCharacterMatchData m_matchData;

	public void RecordRawNetworkMessage(byte[] data, int dataSize)
	{
		if (this.m_initialMessageTimestamp == 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Replay.RecordRawNetworkMessage(byte[], int)).MethodHandle;
			}
			this.m_initialMessageTimestamp = GameTime.time;
		}
		Replay.Message item = default(Replay.Message);
		item.timestamp = GameTime.time - this.m_initialMessageTimestamp;
		item.data = new byte[dataSize];
		Array.Copy(data, item.data, dataSize);
		this.m_messages.Add(item);
	}

	public List<Replay.Message> GetRawNetworkMessages(uint startSeqNum, uint endSeqNum)
	{
		int num = 0;
		SortedList<uint, Replay.Message> sortedList = new SortedList<uint, Replay.Message>();
		for (int i = this.m_messages.Count - 1; i >= 0; i--)
		{
			Replay.Message message = this.m_messages[i];
			int j = message.data.Length;
			int num2 = 0;
			while (j > 0)
			{
				uint num3 = UNetUtil.ReadUInt32(message.data, num2);
				ushort num4 = UNetUtil.ReadUInt16(message.data, num2 + 4);
				short num5 = UNetUtil.ReadInt16(message.data, num2 + 6);
				int num6 = (int)(8 + num4);
				if (startSeqNum <= num3 && num3 <= endSeqNum)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(Replay.GetRawNetworkMessages(uint, uint)).MethodHandle;
					}
					if (num5 != 0x25)
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
						if (num5 != 0x33)
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
							Replay.Message value = default(Replay.Message);
							value.data = new byte[num6];
							Buffer.BlockCopy(message.data, num2, value.data, 0, num6);
							sortedList.Add(num3, value);
						}
					}
					num++;
				}
				num2 += num6;
				j -= num6;
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
			if ((long)num == (long)((ulong)(endSeqNum - startSeqNum + 1U)))
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
				IL_146:
				return sortedList.Values.ToList<Replay.Message>();
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			goto IL_146;
		}
	}

	public void StartPlayback()
	{
		LobbyGameInfo gameInfo = JsonUtility.FromJson<LobbyGameInfo>(this.m_gameInfo_Serialized);
		if (gameInfo.GameConfig.GameType == GameType.Tutorial)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Replay.StartPlayback()).MethodHandle;
			}
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReplayIssue", "FrontEnd"), StringUtil.TR("InvalidReplay", "FrontEnd"), StringUtil.TR("Ok", "Global"), null, -1, false);
			return;
		}
		GameFlowData.s_onGameStateChanged += this.PlaybackOnGameStateChanged;
		GameManager.Get().SetGameplayOverridesForCurrentGame(JsonUtility.FromJson<LobbyGameplayOverrides>(this.m_gameplayOverrides_Serialized));
		GameManager.Get().SetGameInfo(gameInfo);
		GameManager.Get().SetTeamInfo(JsonUtility.FromJson<LobbyTeamInfo>(this.m_teamInfo_Serialized));
		GameManager.Get().SetPlayerInfo(GameManager.Get().TeamInfo.TeamPlayerInfo[this.m_playerInfo_Index]);
		ClientGameManager.Get().Replay_SetGameStatus(GameStatus.Launched);
		ClientGameManager.Get().QueryPlayerMatchData(delegate(PlayerMatchDataResponse response)
		{
			if (response.Success)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(Replay.<StartPlayback>c__AnonStorey0.<>m__0(PlayerMatchDataResponse)).MethodHandle;
				}
				for (int i = 0; i < response.MatchData.Count; i++)
				{
					if (response.MatchData[i].GameServerProcessCode == gameInfo.GameServerProcessCode)
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
						this.m_matchData = response.MatchData[i];
						return;
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		});
		Log.Info("Starting playback of replay, version: {0}", new object[]
		{
			this.m_versionFull
		});
	}

	private void PlayMessage()
	{
		ClientGameManager.Get().Connection.TransportReceive(this.m_messages[this.m_messageReadIndex].data, this.m_messages[this.m_messageReadIndex].data.Length, 0);
		this.m_messageReadIndex++;
		if (this.m_messageReadIndex == this.m_messages.Count)
		{
			UIGameOverScreen.Get().HandleMatchResultsNotification(new MatchResultsNotification());
		}
	}

	public void PlaybackUpdate()
	{
		if (ClientGameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Replay.PlaybackUpdate()).MethodHandle;
			}
			if (ClientGameManager.Get().Connection != null)
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
				if (AppState.IsInGame())
				{
					if (this.m_initialMessageTimestamp == 0f)
					{
						this.m_initialMessageTimestamp = GameTime.time;
					}
					while (this.m_messageReadIndex < this.m_messages.Count)
					{
						if (this.m_messages[this.m_messageReadIndex].timestamp >= GameTime.time - this.m_initialMessageTimestamp)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								return;
							}
						}
						else
						{
							this.PlayMessage();
						}
					}
				}
			}
		}
	}

	public void PlaybackFastForward(ReplayTimestamp target)
	{
		while (this.m_messageReadIndex < this.m_messages.Count && ReplayTimestamp.Current() < target)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Replay.PlaybackFastForward(ReplayTimestamp)).MethodHandle;
			}
			if (AsyncPump.Current.BreakRequested())
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					goto IL_59;
				}
			}
			else
			{
				this.PlayMessage();
			}
		}
		IL_59:
		this.m_initialMessageTimestamp = GameTime.time - this.m_messages[this.m_messageReadIndex - 1].timestamp;
	}

	public void PlaybackRestart()
	{
		this.m_messageReadIndex = 0;
		this.m_initialMessageTimestamp = GameTime.time;
	}

	public void FinishPlayback()
	{
		GameFlowData.s_onGameStateChanged -= this.PlaybackOnGameStateChanged;
	}

	private void PlaybackOnGameStateChanged(GameState newState)
	{
		if (newState == GameState.StartingGame)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Replay.PlaybackOnGameStateChanged(GameState)).MethodHandle;
			}
			ClientGameManager.Get().Replay_SetGameStatus(GameStatus.Loaded);
			ClientGameManager.Get().Replay_SetGameStatus(GameStatus.Started);
		}
	}

	public PersistedCharacterMatchData GetMatchData()
	{
		return this.m_matchData;
	}

	[Serializable]
	public struct Message
	{
		public float timestamp;

		public byte[] data;
	}
}
