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

	private bool m_messageByMessage = false;

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
		for(int i = m_messages.Count - 1; i >= 0; i--)
        {
			Message message = m_messages[i];
			int bytesLeft = message.data.Length;
			int offset = 0;
			while (bytesLeft > 0)
			{
				uint index = UNetUtil.ReadUInt32(message.data, offset);
				ushort dataLen = UNetUtil.ReadUInt16(message.data, offset + 4);
				short type = UNetUtil.ReadInt16(message.data, offset + 6);
				int totalLen = 8 + dataLen;
				if (startSeqNum <= index && index <= endSeqNum)
				{
                    if (type != 37 && type != 51)
                    {
                        Message value = default(Message);
                        value.data = new byte[totalLen];
                        Buffer.BlockCopy(message.data, offset, value.data, 0, totalLen);
                        sortedList.Add(index, value);
                    }
                    num++;
				}
				offset += totalLen;
				bytesLeft -= totalLen;
			}
			if (num == endSeqNum - startSeqNum + 1)
			{
				break;
			}
		}
		return sortedList.Values.ToList();
	}

	public void StartPlayback()
	{
		LobbyGameInfo gameInfo = JsonUtility.FromJson<LobbyGameInfo>(m_gameInfo_Serialized);
		if (gameInfo.GameConfig.GameType == GameType.Tutorial)
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReplayIssue", "FrontEnd"), StringUtil.TR("InvalidReplay", "FrontEnd"), StringUtil.TR("Ok", "Global"));
			return;
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
				for (int i = 0; i < response.MatchData.Count; i++)
				{
					if (response.MatchData[i].GameServerProcessCode == gameInfo.GameServerProcessCode)
					{
						m_matchData = response.MatchData[i];
						return;
					}
				}
				return;
			}
		});
		Log.Info("Starting playback of replay, version: {0}", m_versionFull);
	}

	private void PlayMessage()
	{
		NetworkConnection connection = ClientGameManager.Get().Connection;
		Message message = m_messages[m_messageReadIndex];
		byte[] data = message.data;
		connection.TransportReceive(data, message.data.Length, 0);
		if (m_messageByMessage)
		{
			ReplayPlayManager.Get().Pause();
			UIChatBox.Get().m_chatBox.AddTextEntry($"Message {m_messageReadIndex}", Color.green, true);
		}
		m_messageReadIndex++;
		if (m_messageReadIndex == m_messages.Count)
		{
			UIGameOverScreen.Get().HandleMatchResultsNotification(new MatchResultsNotification());
		}
	}

	public void PlaybackUpdate()
	{
		if (ClientGameManager.Get() == null || ClientGameManager.Get().Connection == null || !AppState.IsInGame())
		{
			return;
		}
        if (m_initialMessageTimestamp == 0f)
		{
			m_initialMessageTimestamp = GameTime.time;
		}
		while (m_messageReadIndex < m_messages.Count)
		{
			if (m_messages[m_messageReadIndex].timestamp >= GameTime.time - m_initialMessageTimestamp)
			{
				return;
			}
			PlayMessage();
		}
	}

	public void PlaybackFastForward(ReplayTimestamp target)
	{
		while (m_messageReadIndex < m_messages.Count && ReplayTimestamp.Current() < target && !AsyncPump.Current.BreakRequested())
		{
			PlayMessage();
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
		ClientGameManager.Get().Replay_SetGameStatus(GameStatus.Loaded);
		ClientGameManager.Get().Replay_SetGameStatus(GameStatus.Started);
	}

	public PersistedCharacterMatchData GetMatchData()
	{
		return m_matchData;
	}
}
