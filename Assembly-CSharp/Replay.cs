using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Replay
{
	[Serializable]
	public struct Message
	{
		public float timestamp;

		public byte[] data;
	}

	public string m_gameplayOverrides_Serialized;

	public string m_gameInfo_Serialized;

	public string m_teamInfo_Serialized;

	public string m_versionMini;

	public string m_versionFull;

	public int m_playerInfo_Index = -1;

	public List<Message> m_messages = new List<Message>();

	private float m_initialMessageTimestamp;

	private int m_messageReadIndex;

	private PersistedCharacterMatchData m_matchData;

	public void RecordRawNetworkMessage(byte[] data, int dataSize)
	{
		if (m_initialMessageTimestamp == 0f)
		{
			m_initialMessageTimestamp = GameTime.time;
		}
		Message item = default(Message);
		item.timestamp = GameTime.time - m_initialMessageTimestamp;
		item.data = new byte[dataSize];
		Array.Copy(data, item.data, dataSize);
		m_messages.Add(item);
	}

	public List<Message> GetRawNetworkMessages(uint startSeqNum, uint endSeqNum)
	{
		int num = 0;
		SortedList<uint, Message> sortedList = new SortedList<uint, Message>();
		int num2 = m_messages.Count - 1;
		while (true)
		{
			if (num2 >= 0)
			{
				Message message = m_messages[num2];
				int num3 = message.data.Length;
				int num4 = 0;
				while (num3 > 0)
				{
					uint num5 = UNetUtil.ReadUInt32(message.data, num4);
					ushort num6 = UNetUtil.ReadUInt16(message.data, num4 + 4);
					short num7 = UNetUtil.ReadInt16(message.data, num4 + 6);
					int num8 = 8 + num6;
					if (startSeqNum <= num5 && num5 <= endSeqNum)
					{
						if (num7 != 37)
						{
							if (num7 != 51)
							{
								Message value = default(Message);
								value.data = new byte[num8];
								Buffer.BlockCopy(message.data, num4, value.data, 0, num8);
								sortedList.Add(num5, value);
							}
						}
						num++;
					}
					num4 += num8;
					num3 -= num8;
				}
				if (num == endSeqNum - startSeqNum + 1)
				{
					break;
				}
				num2--;
				continue;
			}
			break;
		}
		return sortedList.Values.ToList();
	}

	public void StartPlayback()
	{
		LobbyGameInfo gameInfo = JsonUtility.FromJson<LobbyGameInfo>(m_gameInfo_Serialized);
		if (gameInfo.GameConfig.GameType == GameType.Tutorial)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReplayIssue", "FrontEnd"), StringUtil.TR("InvalidReplay", "FrontEnd"), StringUtil.TR("Ok", "Global"));
					return;
				}
			}
		}
		GameFlowData.s_onGameStateChanged += PlaybackOnGameStateChanged;
		GameManager.Get().SetGameplayOverridesForCurrentGame(JsonUtility.FromJson<LobbyGameplayOverrides>(m_gameplayOverrides_Serialized));
		GameManager.Get().SetGameInfo(gameInfo);
		GameManager.Get().SetTeamInfo(JsonUtility.FromJson<LobbyTeamInfo>(m_teamInfo_Serialized));
		GameManager.Get().SetPlayerInfo(GameManager.Get().TeamInfo.TeamPlayerInfo[m_playerInfo_Index]);
		ClientGameManager.Get().Replay_SetGameStatus(GameStatus.Launched);
		ClientGameManager.Get().QueryPlayerMatchData(delegate(PlayerMatchDataResponse response)
		{
			if (response.Success)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						for (int i = 0; i < response.MatchData.Count; i++)
						{
							if (response.MatchData[i].GameServerProcessCode == gameInfo.GameServerProcessCode)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										m_matchData = response.MatchData[i];
										return;
									}
								}
							}
						}
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
					}
				}
			}
		});
		Log.Info("Starting playback of replay, version: {0}", m_versionFull);
	}

	private void PlayMessage()
	{
		NetworkConnection connection = ClientGameManager.Get().Connection;
		Message message = m_messages[m_messageReadIndex];
		byte[] data = message.data;
		Message message2 = m_messages[m_messageReadIndex];
		connection.TransportReceive(data, message2.data.Length, 0);
		m_messageReadIndex++;
		if (m_messageReadIndex == m_messages.Count)
		{
			UIGameOverScreen.Get().HandleMatchResultsNotification(new MatchResultsNotification());
		}
	}

	public void PlaybackUpdate()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get().Connection == null)
			{
				return;
			}
			while (true)
			{
				if (!AppState.IsInGame())
				{
					return;
				}
				if (m_initialMessageTimestamp == 0f)
				{
					m_initialMessageTimestamp = GameTime.time;
				}
				while (true)
				{
					if (m_messageReadIndex < m_messages.Count)
					{
						Message message = m_messages[m_messageReadIndex];
						if (!(message.timestamp < GameTime.time - m_initialMessageTimestamp))
						{
							break;
						}
						PlayMessage();
						continue;
					}
					return;
				}
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
		}
	}

	public void PlaybackFastForward(ReplayTimestamp target)
	{
		while (m_messageReadIndex < m_messages.Count && ReplayTimestamp.Current() < target)
		{
			if (!AsyncPump.Current.BreakRequested())
			{
				PlayMessage();
				continue;
			}
			break;
		}
		float time = GameTime.time;
		Message message = m_messages[m_messageReadIndex - 1];
		m_initialMessageTimestamp = time - message.timestamp;
	}

	public void PlaybackRestart()
	{
		m_messageReadIndex = 0;
		m_initialMessageTimestamp = GameTime.time;
	}

	public void FinishPlayback()
	{
		GameFlowData.s_onGameStateChanged -= PlaybackOnGameStateChanged;
	}

	private void PlaybackOnGameStateChanged(GameState newState)
	{
		if (newState != GameState.StartingGame)
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().Replay_SetGameStatus(GameStatus.Loaded);
			ClientGameManager.Get().Replay_SetGameStatus(GameStatus.Started);
			return;
		}
	}

	public PersistedCharacterMatchData GetMatchData()
	{
		return m_matchData;
	}
}
