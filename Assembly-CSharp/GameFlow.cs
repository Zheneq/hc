using System;
using System.Collections.Generic;
using Fabric;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class GameFlow : NetworkBehaviour
{
	private Dictionary<Player, PlayerDetails> m_playerDetails = new Dictionary<Player, PlayerDetails>(default(GameFlow.PlayerComparer));

	private static GameFlow s_instance;

	private static int kRpcRpcDisplayConsoleText = -0x2F0E5AE8;

	private static int kRpcRpcSetMatchTime;

	static GameFlow()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlow), GameFlow.kRpcRpcDisplayConsoleText, new NetworkBehaviour.CmdDelegate(GameFlow.InvokeRpcRpcDisplayConsoleText));
		GameFlow.kRpcRpcSetMatchTime = -0x2159A77A;
		NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlow), GameFlow.kRpcRpcSetMatchTime, new NetworkBehaviour.CmdDelegate(GameFlow.InvokeRpcRpcSetMatchTime));
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.SendCastAbility(ActorData, AbilityData.ActionType, List<AbilityTarget>)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void GameFlow::SendCastAbility(ActorData,AbilityData/ActionType,System.Collections.Generic.List`1<AbilityTarget>)' called on server");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.StartMessage(0x32);
		networkWriter.Write(caster.ActorIndex);
		networkWriter.Write((int)actionType);
		AbilityTarget.SerializeAbilityTargetList(targets, networkWriter);
		networkWriter.FinishMessage();
		ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
	}

	internal Dictionary<Player, PlayerDetails> playerDetails
	{
		get
		{
			return this.m_playerDetails;
		}
	}

	internal static GameFlow Get()
	{
		return GameFlow.s_instance;
	}

	private void Awake()
	{
		GameFlow.s_instance = this;
	}

	private void Start()
	{
		this.OnLoadedLevel();
		GameFlowData.s_onGameStateChanged += this.OnGameStateChanged;
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.Start()).MethodHandle;
			}
			if (GameFlowData.Get() != null)
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
				this.OnGameStateChanged(GameFlowData.Get().gameState);
			}
		}
	}

	private void OnLoadedLevel()
	{
		HighlightUtils.Get().HideCursor = false;
	}

	private void OnDestroy()
	{
		this.Client_OnDestroy();
		if (EventManager.Instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.OnDestroy()).MethodHandle;
			}
		}
		GameFlowData.s_onGameStateChanged -= this.OnGameStateChanged;
		GameFlow.s_instance = null;
	}

	private void OnGameStateChanged(GameState newState)
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
		{
			clientFog.MarkForRecalculateVisibility();
		}
		if (newState != GameState.BothTeams_Decision)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.OnGameStateChanged(GameState)).MethodHandle;
			}
			if (newState != GameState.BothTeams_Resolve)
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
			}
			else
			{
				AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_resolve", null);
				AudioManager.PostEvent("ui_resolution_cam_start", null);
				AudioManager.GetMixerSnapshotManager().SetMix_ResolveCam();
				if (GameEventManager.Get() != null)
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
					GameEventManager.Get().FireEvent(GameEventManager.EventType.ClientResolutionStarted, null);
				}
			}
		}
		else
		{
			AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_decision", null);
			AudioManager.GetMixerSnapshotManager().SetMix_DecisionCam();
		}
	}

	internal void CheckTutorialAutoselectCharacter()
	{
		if (this.m_playerDetails.Count == 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.CheckTutorialAutoselectCharacter()).MethodHandle;
			}
			if (GameFlowData.Get().GetNumAvailableCharacterResourceLinks() == 1)
			{
			}
		}
	}

	public Player GetPlayerFromConnectionId(int connectionId)
	{
		using (Dictionary<Player, PlayerDetails>.Enumerator enumerator = this.m_playerDetails.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Player, PlayerDetails> keyValuePair = enumerator.Current;
				if (keyValuePair.Key.m_connectionId == connectionId)
				{
					return keyValuePair.Key;
				}
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.GetPlayerFromConnectionId(int)).MethodHandle;
			}
		}
		return default(Player);
	}

	public string GetPlayerHandleFromConnectionId(int connectionId)
	{
		string empty = string.Empty;
		using (Dictionary<Player, PlayerDetails>.Enumerator enumerator = this.m_playerDetails.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Player, PlayerDetails> keyValuePair = enumerator.Current;
				if (keyValuePair.Key.m_connectionId == connectionId)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.GetPlayerHandleFromConnectionId(int)).MethodHandle;
					}
					return keyValuePair.Value.m_handle;
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
		return empty;
	}

	public string GetPlayerHandleFromAccountId(long accountId)
	{
		string empty = string.Empty;
		using (Dictionary<Player, PlayerDetails>.Enumerator enumerator = this.m_playerDetails.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Player, PlayerDetails> keyValuePair = enumerator.Current;
				if (keyValuePair.Key.m_accountId == accountId)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.GetPlayerHandleFromAccountId(long)).MethodHandle;
					}
					return keyValuePair.Value.m_handle;
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
		return empty;
	}

	[ClientRpc]
	private void RpcDisplayConsoleText(DisplayConsoleTextMessage message)
	{
		if (message.RestrictVisibiltyToTeam != Team.Invalid)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.RpcDisplayConsoleText(DisplayConsoleTextMessage)).MethodHandle;
			}
			if (!(GameFlowData.Get().activeOwnedActorData != null))
			{
				return;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameFlowData.Get().activeOwnedActorData.GetTeam() != message.RestrictVisibiltyToTeam)
			{
				return;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		string text = string.Empty;
		if (!message.Unlocalized.IsNullOrEmpty())
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
			text = message.Unlocalized;
		}
		else if (message.Token.IsNullOrEmpty())
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
		}, null);
	}

	[ClientRpc]
	private void RpcSetMatchTime(float timeSinceMatchStart)
	{
		UITimerPanel.Get().SetMatchTime(timeSinceMatchStart);
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		if (!initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		if (!initialState)
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
			if (base.syncVarDirtyBits == 0U)
			{
				return false;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		NetworkWriterAdapter networkWriterAdapter = new NetworkWriterAdapter(writer);
		int num = this.m_playerDetails.Count;
		networkWriterAdapter.Serialize(ref num);
		if (num >= 0)
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
			if (num <= 0x14)
			{
				goto IL_9F;
			}
		}
		Log.Error("Invalid number of players: " + num, new object[0]);
		num = Mathf.Clamp(num, 0, 0x14);
		IL_9F:
		using (Dictionary<Player, PlayerDetails>.Enumerator enumerator = this.m_playerDetails.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Player, PlayerDetails> keyValuePair = enumerator.Current;
				Player key = keyValuePair.Key;
				PlayerDetails value = keyValuePair.Value;
				bool flag = value != null;
				if (flag)
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
					key.OnSerializeHelper(networkWriterAdapter);
					value.OnSerializeHelper(networkWriterAdapter);
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return true;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			num = reader.ReadPackedUInt32();
		}
		if (num != 0U)
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
			NetworkReaderAdapter networkReaderAdapter = new NetworkReaderAdapter(reader);
			int num2 = this.m_playerDetails.Count;
			networkReaderAdapter.Serialize(ref num2);
			if (num2 >= 0)
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
				if (num2 <= 0x14)
				{
					goto IL_94;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			Log.Error("Invalid number of players: " + num2, new object[0]);
			num2 = Mathf.Clamp(num2, 0, 0x14);
			IL_94:
			this.m_playerDetails.Clear();
			for (int i = 0; i < num2; i++)
			{
				Player key = default(Player);
				PlayerDetails playerDetails = new PlayerDetails(PlayerGameAccountType.None);
				key.OnSerializeHelper(networkReaderAdapter);
				playerDetails.OnSerializeHelper(networkReaderAdapter);
				key.m_accountId = playerDetails.m_accountId;
				this.m_playerDetails[key] = playerDetails;
				if (GameFlowData.Get() && GameFlowData.Get().LocalPlayerData == null)
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
					GameFlowData.Get().SetLocalPlayerData();
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcDisplayConsoleText(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.InvokeRpcRpcDisplayConsoleText(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcDisplayConsoleText called on server.");
			return;
		}
		((GameFlow)obj).RpcDisplayConsoleText(GeneratedNetworkCode._ReadDisplayConsoleTextMessage_None(reader));
	}

	protected static void InvokeRpcRpcSetMatchTime(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.InvokeRpcRpcSetMatchTime(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcSetMatchTime called on server.");
			return;
		}
		((GameFlow)obj).RpcSetMatchTime(reader.ReadSingle());
	}

	public void CallRpcDisplayConsoleText(DisplayConsoleTextMessage message)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.CallRpcDisplayConsoleText(DisplayConsoleTextMessage)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcDisplayConsoleText called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)GameFlow.kRpcRpcDisplayConsoleText);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		GeneratedNetworkCode._WriteDisplayConsoleTextMessage_None(networkWriter, message);
		this.SendRPCInternal(networkWriter, 0, "RpcDisplayConsoleText");
	}

	public void CallRpcSetMatchTime(float timeSinceMatchStart)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameFlow.CallRpcSetMatchTime(float)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcSetMatchTime called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)GameFlow.kRpcRpcSetMatchTime);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(timeSinceMatchStart);
		this.SendRPCInternal(networkWriter, 0, "RpcSetMatchTime");
	}

	public class SetHumanInfoMessage : MessageBase
	{
		public string m_userName;

		public string m_buildVersion;

		public string m_accountIdString;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(this.m_userName);
			writer.Write(this.m_buildVersion);
			writer.Write(this.m_accountIdString);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.m_userName = reader.ReadString();
			this.m_buildVersion = reader.ReadString();
			this.m_accountIdString = reader.ReadString();
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
			writer.Write(this.m_characterName);
			writer.WritePackedUInt32((uint)this.m_skinIndex);
			writer.WritePackedUInt32((uint)this.m_patternIndex);
			writer.WritePackedUInt32((uint)this.m_colorIndex);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.m_characterName = reader.ReadString();
			this.m_skinIndex = (int)reader.ReadPackedUInt32();
			this.m_patternIndex = (int)reader.ReadPackedUInt32();
			this.m_colorIndex = (int)reader.ReadPackedUInt32();
		}
	}

	public class SetTeamFinalizedMessage : MessageBase
	{
		public int m_team;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)this.m_team);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.m_team = (int)reader.ReadPackedUInt32();
		}
	}

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
}
