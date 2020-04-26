using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class ActorCinematicRequests : NetworkBehaviour
{
	internal SyncListBool m_abilityRequested = new SyncListBool();

	[SyncVar]
	private int m_numCinematicRequestsLeft = 2;

	private SyncListInt m_cinematicsPlayedThisMatch = new SyncListInt();

	private ActorData m_actorData;

	private static int kListm_abilityRequested;

	private static int kListm_cinematicsPlayedThisMatch;

	private static int kCmdCmdSelectAbilityCinematicRequest;

	public int Networkm_numCinematicRequestsLeft
	{
		get
		{
			return m_numCinematicRequestsLeft;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_numCinematicRequestsLeft, 2u);
		}
	}

	static ActorCinematicRequests()
	{
		kCmdCmdSelectAbilityCinematicRequest = 1550121236;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorCinematicRequests), kCmdCmdSelectAbilityCinematicRequest, InvokeCmdCmdSelectAbilityCinematicRequest);
		kListm_abilityRequested = -88780988;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorCinematicRequests), kListm_abilityRequested, InvokeSyncListm_abilityRequested);
		kListm_cinematicsPlayedThisMatch = 782922590;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorCinematicRequests), kListm_cinematicsPlayedThisMatch, InvokeSyncListm_cinematicsPlayedThisMatch);
		NetworkCRC.RegisterBehaviour("ActorCinematicRequests", 0);
	}

	private void Awake()
	{
		m_actorData = GetComponent<ActorData>();
		m_abilityRequested.InitializeBehaviour(this, kListm_abilityRequested);
		m_cinematicsPlayedThisMatch.InitializeBehaviour(this, kListm_cinematicsPlayedThisMatch);
	}

	public override void OnStartServer()
	{
		for (int i = 0; i < 14; i++)
		{
			m_abilityRequested.Add(false);
		}
		while (true)
		{
			return;
		}
	}

	public bool IsAbilityCinematicRequested(AbilityData.ActionType actionType)
	{
		return (int)actionType < m_abilityRequested.Count && m_abilityRequested[(int)actionType];
	}

	[Server]
	public void OnTurnStart()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void ActorCinematicRequests::OnTurnStart()' called on client");
					return;
				}
			}
		}
		int num = 0;
		for (int i = 0; i < m_abilityRequested.Count; i++)
		{
			if (m_abilityRequested[i])
			{
				num++;
				m_abilityRequested[i] = false;
			}
		}
		while (true)
		{
			Networkm_numCinematicRequestsLeft = m_numCinematicRequestsLeft - num;
			return;
		}
	}

	public int NumRequestsLeft(int tauntId)
	{
		if (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("NoCooldowns"))
		{
			if (GameManager.Get().GameConfig.GameType != GameType.Practice)
			{
				bool flag = true;
				using (List<GameObject>.Enumerator enumerator = GameFlowData.Get().GetPlayers().GetEnumerator())
				{
					while (true)
					{
						if (!enumerator.MoveNext())
						{
							break;
						}
						GameObject current = enumerator.Current;
						if (current.GetComponent<BotController>() == null)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return 10;
						}
					}
				}
				if (m_cinematicsPlayedThisMatch.Contains(tauntId))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return 0;
						}
					}
				}
				int num = m_numCinematicRequestsLeft;
				for (int i = 0; i < m_abilityRequested.Count; i++)
				{
					if (m_abilityRequested[i])
					{
						num--;
					}
				}
				while (true)
				{
					return num;
				}
			}
		}
		return 10;
	}

	[Server]
	public void ProcessAbilityCinematicRequest(AbilityData.ActionType actionType, bool requested, int animTauntIndex, int tauntUniqueId)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorCinematicRequests::ProcessAbilityCinematicRequest(AbilityData/ActionType,System.Boolean,System.Int32,System.Int32)' called on client");
			return;
		}
	}

	public void SendAbilityCinematicRequest(AbilityData.ActionType actionType, bool requested, int animTauntIndex, int tauntId)
	{
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					ProcessAbilityCinematicRequest(actionType, requested, animTauntIndex, tauntId);
					return;
				}
			}
		}
		CallCmdSelectAbilityCinematicRequest((int)actionType, requested, animTauntIndex, tauntId);
	}

	[Command]
	private void CmdSelectAbilityCinematicRequest(int actionType, bool requested, int animTauntIndex, int tauntId)
	{
		ProcessAbilityCinematicRequest((AbilityData.ActionType)actionType, requested, animTauntIndex, tauntId);
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_abilityRequested(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_abilityRequested called on server.");
					return;
				}
			}
		}
		((ActorCinematicRequests)obj).m_abilityRequested.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_cinematicsPlayedThisMatch(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_cinematicsPlayedThisMatch called on server.");
					return;
				}
			}
		}
		((ActorCinematicRequests)obj).m_cinematicsPlayedThisMatch.HandleMsg(reader);
	}

	protected static void InvokeCmdCmdSelectAbilityCinematicRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdSelectAbilityCinematicRequest called on client.");
					return;
				}
			}
		}
		((ActorCinematicRequests)obj).CmdSelectAbilityCinematicRequest((int)reader.ReadPackedUInt32(), reader.ReadBoolean(), (int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	public void CallCmdSelectAbilityCinematicRequest(int actionType, bool requested, int animTauntIndex, int tauntId)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdSelectAbilityCinematicRequest called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					CmdSelectAbilityCinematicRequest(actionType, requested, animTauntIndex, tauntId);
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSelectAbilityCinematicRequest);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionType);
		networkWriter.Write(requested);
		networkWriter.WritePackedUInt32((uint)animTauntIndex);
		networkWriter.WritePackedUInt32((uint)tauntId);
		SendCommandInternal(networkWriter, 0, "CmdSelectAbilityCinematicRequest");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					SyncListBool.WriteInstance(writer, m_abilityRequested);
					writer.WritePackedUInt32((uint)m_numCinematicRequestsLeft);
					SyncListInt.WriteInstance(writer, m_cinematicsPlayedThisMatch);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListBool.WriteInstance(writer, m_abilityRequested);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_numCinematicRequestsLeft);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_cinematicsPlayedThisMatch);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					SyncListBool.ReadReference(reader, m_abilityRequested);
					m_numCinematicRequestsLeft = (int)reader.ReadPackedUInt32();
					SyncListInt.ReadReference(reader, m_cinematicsPlayedThisMatch);
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListBool.ReadReference(reader, m_abilityRequested);
		}
		if ((num & 2) != 0)
		{
			m_numCinematicRequestsLeft = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			SyncListInt.ReadReference(reader, m_cinematicsPlayedThisMatch);
		}
	}
}
