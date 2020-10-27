using Fabric;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class GameFlow : NetworkBehaviour
{
	public class SetHumanInfoMessage : MessageBase
	{
		public string m_userName;
		public string m_buildVersion;
		public string m_accountIdString;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(m_userName);
			writer.Write(m_buildVersion);
			writer.Write(m_accountIdString);
		}

		public override void Deserialize(NetworkReader reader)
		{
			m_userName = reader.ReadString();
			m_buildVersion = reader.ReadString();
			m_accountIdString = reader.ReadString();
		}
	}

	public class SelectCharacterMessage : MessageBase
	{
		public string m_characterName;
		public int m_skinIndex;
		public int m_patternIndex;
		public int m_colorIndex;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(m_characterName);
			writer.WritePackedUInt32((uint)m_skinIndex);
			writer.WritePackedUInt32((uint)m_patternIndex);
			writer.WritePackedUInt32((uint)m_colorIndex);
		}

		public override void Deserialize(NetworkReader reader)
		{
			m_characterName = reader.ReadString();
			m_skinIndex = (int)reader.ReadPackedUInt32();
			m_patternIndex = (int)reader.ReadPackedUInt32();
			m_colorIndex = (int)reader.ReadPackedUInt32();
		}
	}

	public class SetTeamFinalizedMessage : MessageBase
	{
		public int m_team;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)m_team);
		}

		public override void Deserialize(NetworkReader reader)
		{
			m_team = (int)reader.ReadPackedUInt32();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PlayerComparer : IEqualityComparer<Player>
	{
		public bool Equals(Player x, Player y)
		{
			return x == y;
		}

		public int GetHashCode(Player obj)
		{
			return obj.GetHashCode();
		}
	}

	private Dictionary<Player, PlayerDetails> m_playerDetails = new Dictionary<Player, PlayerDetails>(default(PlayerComparer));
	private static GameFlow s_instance;

	private static int kRpcRpcDisplayConsoleText = -789469928;
	private static int kRpcRpcSetMatchTime = -559523706;

	internal Dictionary<Player, PlayerDetails> playerDetails => m_playerDetails;

	static GameFlow()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlow), kRpcRpcDisplayConsoleText, InvokeRpcRpcDisplayConsoleText);
		NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlow), kRpcRpcSetMatchTime, InvokeRpcRpcSetMatchTime);
		NetworkCRC.RegisterBehaviour("GameFlow", 0);
	}

	public override void OnStartClient()
	{
	}

	private void Client_OnDestroy()
	{
	}

	[Client]
	internal void SendCastAbility(ActorData caster, AbilityData.ActionType actionType, List<AbilityTarget> targets)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void GameFlow::SendCastAbility(ActorData,AbilityData/ActionType,System.Collections.Generic.List`1<AbilityTarget>)' called on server");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.StartMessage(50);
		networkWriter.Write(caster.ActorIndex);
		networkWriter.Write((int)actionType);
		AbilityTarget.SerializeAbilityTargetList(targets, networkWriter);
		networkWriter.FinishMessage();
		ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
	}

	internal static GameFlow Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void Start()
	{
		OnLoadedLevel();
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
		if (!NetworkServer.active && GameFlowData.Get() != null)
		{
			OnGameStateChanged(GameFlowData.Get().gameState);
		}
	}

	private void OnLoadedLevel()
	{
		HighlightUtils.Get().HideCursor = false;
	}

	private void OnDestroy()
	{
		Client_OnDestroy();
		// some broken code
		if (EventManager.Instance != null)
		{
		}
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
		s_instance = null;
	}

	private void OnGameStateChanged(GameState newState)
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
		{
			clientFog.MarkForRecalculateVisibility();
		}
		switch (newState)
		{
			case GameState.BothTeams_Decision:
				AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_decision");
				AudioManager.GetMixerSnapshotManager().SetMix_DecisionCam();
				break;
			case GameState.BothTeams_Resolve:
				AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_resolve");
				AudioManager.PostEvent("ui_resolution_cam_start");
				AudioManager.GetMixerSnapshotManager().SetMix_ResolveCam();
				if (GameEventManager.Get() != null)
				{
					GameEventManager.Get().FireEvent(GameEventManager.EventType.ClientResolutionStarted, null);
				}

				break;
		}
	}

	internal void CheckTutorialAutoselectCharacter()
	{
		// some broken code
		if (m_playerDetails.Count == 1 && GameFlowData.Get().GetNumAvailableCharacterResourceLinks() != 1)
		{
		}
	}

	public Player GetPlayerFromConnectionId(int connectionId)
	{
		foreach (Player key in m_playerDetails.Keys)
		{
			if (key.m_connectionId == connectionId)
			{
				return key;
			}
		}
		return default(Player);
	}

	public string GetPlayerHandleFromConnectionId(int connectionId)
	{
		foreach (KeyValuePair<Player, PlayerDetails> keyValuePair in m_playerDetails)
		{
			if (keyValuePair.Key.m_connectionId == connectionId)
			{
				return keyValuePair.Value.m_handle;
			}
		}
		return "";
	}

	public string GetPlayerHandleFromAccountId(long accountId)
	{
		foreach (KeyValuePair<Player, PlayerDetails> keyValuePair in m_playerDetails)
		{
			if (keyValuePair.Key.m_accountId == accountId)
			{
				return keyValuePair.Value.m_handle;
			}
		}
		return "";
	}

	[ClientRpc]
	private void RpcDisplayConsoleText(DisplayConsoleTextMessage message)
	{
		Log.Info($"[JSON] {{\"rpcDisplayConsoleText\":{DefaultJsonSerializer.Serialize(message)}}}");
		if (message.RestrictVisibiltyToTeam == Team.Invalid
			|| (GameFlowData.Get().activeOwnedActorData != null
				&& GameFlowData.Get().activeOwnedActorData.GetTeam() == message.RestrictVisibiltyToTeam))
		{
			string text;
			if (!message.Unlocalized.IsNullOrEmpty())
			{
				text = message.Unlocalized;
			}
			else if (message.Token.IsNullOrEmpty())
			{
				text = StringUtil.TR(message.Term, message.Context);
			}
			else
			{
				text = string.Format(StringUtil.TR(message.Term, message.Context), message.Token);
			}
			TextConsole.Get().Write(new TextConsole.Message
			{
				Text = text,
				MessageType = message.MessageType,
				RestrictVisibiltyToTeam = message.RestrictVisibiltyToTeam,
				SenderHandle = message.SenderHandle,
				CharacterType = message.CharacterType
			});
		}
	}

	[ClientRpc]
	private void RpcSetMatchTime(float timeSinceMatchStart)
	{
		Log.Info($"[JSON] {{\"rpcSetMatchTime\":{{\"timeSinceMatchStart\":{DefaultJsonSerializer.Serialize(timeSinceMatchStart)}}}}}");
		UITimerPanel.Get().SetMatchTime(timeSinceMatchStart);
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		if (!initialState)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		if (!initialState && syncVarDirtyBits == 0)
		{
			return false;
		}
		NetworkWriterAdapter networkWriterAdapter = new NetworkWriterAdapter(writer);
		int value = m_playerDetails.Count;
		networkWriterAdapter.Serialize(ref value);
		if (value < 0 || value > 20)
		{
			Log.Error("Invalid number of players: " + value);
			value = Mathf.Clamp(value, 0, 20);
		}
		foreach (var current in m_playerDetails)
		{
			Player player = current.Key;
			PlayerDetails details = current.Value;
			if (details != null)
			{
				player.OnSerializeHelper(networkWriterAdapter);
				details.OnSerializeHelper(networkWriterAdapter);
			}
		}

		return true;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		var jsonLog = new List<string>();
		uint num = uint.MaxValue;
		if (!initialState)
		{
			num = reader.ReadPackedUInt32();
		}
		if (num != 0)
		{
			NetworkReaderAdapter networkReaderAdapter = new NetworkReaderAdapter(reader);
			int value = m_playerDetails.Count;
			networkReaderAdapter.Serialize(ref value);
			if (value < 0 || value > 20)
			{
				Log.Error("Invalid number of players: " + value);
				value = Mathf.Clamp(value, 0, 20);
			}
			m_playerDetails.Clear();
			for (int i = 0; i < value; i++)
			{
				Player key = default(Player);
				PlayerDetails playerDetails = new PlayerDetails(PlayerGameAccountType.None);
				key.OnSerializeHelper(networkReaderAdapter);
				playerDetails.OnSerializeHelper(networkReaderAdapter);
				key.m_accountId = playerDetails.m_accountId;
				m_playerDetails[key] = playerDetails;
				if ((bool)GameFlowData.Get() && GameFlowData.Get().LocalPlayerData == null)
				{
					GameFlowData.Get().SetLocalPlayerData();
				}
			}
			jsonLog.Add($"\"playerDetails\":{DefaultJsonSerializer.Serialize(m_playerDetails)}");
		}
		Log.Info($"[JSON] {{\"gameFlow\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcDisplayConsoleText(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDisplayConsoleText called on server.");
			return;
		}
		((GameFlow)obj).RpcDisplayConsoleText(GeneratedNetworkCode._ReadDisplayConsoleTextMessage_None(reader));
	}

	protected static void InvokeRpcRpcSetMatchTime(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetMatchTime called on server.");
			return;
		}
		((GameFlow)obj).RpcSetMatchTime(reader.ReadSingle());
	}

	public void CallRpcDisplayConsoleText(DisplayConsoleTextMessage message)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcDisplayConsoleText called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcDisplayConsoleText);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		GeneratedNetworkCode._WriteDisplayConsoleTextMessage_None(networkWriter, message);
		SendRPCInternal(networkWriter, 0, "RpcDisplayConsoleText");
	}

	public void CallRpcSetMatchTime(float timeSinceMatchStart)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetMatchTime called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetMatchTime);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(timeSinceMatchStart);
		SendRPCInternal(networkWriter, 0, "RpcSetMatchTime");
	}
}
