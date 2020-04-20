using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamStatusDisplay : NetworkBehaviour
{
	private static TeamStatusDisplay s_instance;

	private Dictionary<ActorData, TeamStatusDisplay.TeamStatusEntry> m_playerIndexToTextMap;

	private static int kRpcRpcSetTeamStatus = -0x5C5D65D8;

	static TeamStatusDisplay()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(TeamStatusDisplay), TeamStatusDisplay.kRpcRpcSetTeamStatus, new NetworkBehaviour.CmdDelegate(TeamStatusDisplay.InvokeRpcRpcSetTeamStatus));
		NetworkCRC.RegisterBehaviour("TeamStatusDisplay", 0);
	}

	public static TeamStatusDisplay GetTeamStatusDisplay()
	{
		return TeamStatusDisplay.s_instance;
	}

	private void Awake()
	{
		TeamStatusDisplay.s_instance = this;
		this.m_playerIndexToTextMap = new Dictionary<ActorData, TeamStatusDisplay.TeamStatusEntry>();
	}

	private void OnDestroy()
	{
		TeamStatusDisplay.s_instance = null;
	}

	public void RebuildTeamDisplay()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData)
		{
			List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.GetTeam());
			using (List<ActorData>.Enumerator enumerator = playerAndBotTeamMembers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData key = enumerator.Current;
					TeamStatusDisplay.TeamStatusEntry teamStatusEntry = new TeamStatusDisplay.TeamStatusEntry();
					teamStatusEntry.m_text = string.Empty;
					this.m_playerIndexToTextMap[key] = teamStatusEntry;
				}
			}
		}
	}

	public void SetStatusText(ActorData actorData, string text)
	{
		if (this.m_playerIndexToTextMap.ContainsKey(actorData))
		{
			this.m_playerIndexToTextMap[actorData].m_text = text;
		}
	}

	public void ClearStatusText()
	{
		using (Dictionary<ActorData, TeamStatusDisplay.TeamStatusEntry>.Enumerator enumerator = this.m_playerIndexToTextMap.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, TeamStatusDisplay.TeamStatusEntry> keyValuePair = enumerator.Current;
				keyValuePair.Value.m_text = string.Empty;
			}
		}
	}

	public void ResetTeamStatus()
	{
		this.m_playerIndexToTextMap.Clear();
	}

	internal void SendSetTeamStatus(int playerIndex, string status)
	{
		Log.Warning("TeamStatusDisplay is not used, please remove it.", new object[0]);
	}

	[ClientRpc]
	private void RpcSetTeamStatus(int playerIndex, string status)
	{
		ActorData actorData = GameFlowData.Get().FindActorByPlayerIndex(playerIndex);
		if (actorData)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				if (actorData.GetTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam())
				{
					this.SetStatusText(actorData, status);
				}
			}
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
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)TeamStatusDisplay.kRpcRpcSetTeamStatus);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)playerIndex);
		networkWriter.Write(status);
		this.SendRPCInternal(networkWriter, 0, "RpcSetTeamStatus");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}

	private class TeamStatusEntry
	{
		public string m_text;
	}
}
