using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamStatusDisplay : NetworkBehaviour
{
	private class TeamStatusEntry
	{
		public string m_text;
	}

	private static TeamStatusDisplay s_instance;

	private Dictionary<ActorData, TeamStatusEntry> m_playerIndexToTextMap;

	private static int kRpcRpcSetTeamStatus;

	static TeamStatusDisplay()
	{
		kRpcRpcSetTeamStatus = -1549624792;
		NetworkBehaviour.RegisterRpcDelegate(typeof(TeamStatusDisplay), kRpcRpcSetTeamStatus, InvokeRpcRpcSetTeamStatus);
		NetworkCRC.RegisterBehaviour("TeamStatusDisplay", 0);
	}

	public static TeamStatusDisplay GetTeamStatusDisplay()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_playerIndexToTextMap = new Dictionary<ActorData, TeamStatusEntry>();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void RebuildTeamDisplay()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if ((bool)activeOwnedActorData)
		{
			List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.GetTeam());
			foreach (ActorData current in playerAndBotTeamMembers)
			{
				TeamStatusEntry teamStatusEntry = new TeamStatusEntry();
				teamStatusEntry.m_text = string.Empty;
				m_playerIndexToTextMap[current] = teamStatusEntry;
			}
		}
	}

	public void SetStatusText(ActorData actorData, string text)
	{
		if (m_playerIndexToTextMap.ContainsKey(actorData))
		{
			m_playerIndexToTextMap[actorData].m_text = text;
		}
	}

	public void ClearStatusText()
	{
		foreach (var x in m_playerIndexToTextMap)
		{
			x.Value.m_text = string.Empty;
		}
	}

	public void ResetTeamStatus()
	{
		m_playerIndexToTextMap.Clear();
	}

	internal void SendSetTeamStatus(int playerIndex, string status)
	{
		Log.Warning("TeamStatusDisplay is not used, please remove it.");
	}

	[ClientRpc]
	private void RpcSetTeamStatus(int playerIndex, string status)
	{
		Log.Info($"[JSON] {{\"rpcSetTeamStatus\":{{\"playerIndex\":{DefaultJsonSerializer.Serialize(playerIndex)},\"status\":{DefaultJsonSerializer.Serialize(status)}}}}}");
		ActorData actorData = GameFlowData.Get().FindActorByPlayerIndex(playerIndex);
		if (!actorData)
		{
			return;
		}
		if (GameFlowData.Get().activeOwnedActorData == null)
		{
			return;
		}
		if (actorData.GetTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam())
		{
			SetStatusText(actorData, status);
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcSetTeamStatus(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetTeamStatus called on server.");
			return;
		}
		((TeamStatusDisplay)obj).RpcSetTeamStatus((int)reader.ReadPackedUInt32(), reader.ReadString());
	}

	public void CallRpcSetTeamStatus(int playerIndex, string status)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetTeamStatus called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetTeamStatus);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)playerIndex);
		networkWriter.Write(status);
		SendRPCInternal(networkWriter, 0, "RpcSetTeamStatus");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
