using System;
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

	private static int kCmdCmdSelectAbilityCinematicRequest = 0x5C64F914;

	static ActorCinematicRequests()
	{
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorCinematicRequests), ActorCinematicRequests.kCmdCmdSelectAbilityCinematicRequest, new NetworkBehaviour.CmdDelegate(ActorCinematicRequests.InvokeCmdCmdSelectAbilityCinematicRequest));
		ActorCinematicRequests.kListm_abilityRequested = -0x54AB0BC;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorCinematicRequests), ActorCinematicRequests.kListm_abilityRequested, new NetworkBehaviour.CmdDelegate(ActorCinematicRequests.InvokeSyncListm_abilityRequested));
		ActorCinematicRequests.kListm_cinematicsPlayedThisMatch = 0x2EAA735E;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorCinematicRequests), ActorCinematicRequests.kListm_cinematicsPlayedThisMatch, new NetworkBehaviour.CmdDelegate(ActorCinematicRequests.InvokeSyncListm_cinematicsPlayedThisMatch));
		NetworkCRC.RegisterBehaviour("ActorCinematicRequests", 0);
	}

	private void Awake()
	{
		this.m_actorData = base.GetComponent<ActorData>();
		this.m_abilityRequested.InitializeBehaviour(this, ActorCinematicRequests.kListm_abilityRequested);
		this.m_cinematicsPlayedThisMatch.InitializeBehaviour(this, ActorCinematicRequests.kListm_cinematicsPlayedThisMatch);
	}

	public override void OnStartServer()
	{
		for (int i = 0; i < 0xE; i++)
		{
			this.m_abilityRequested.Add(false);
		}
	}

	public bool IsAbilityCinematicRequested(AbilityData.ActionType actionType)
	{
		return actionType < (AbilityData.ActionType)this.m_abilityRequested.Count && this.m_abilityRequested[(int)actionType];
	}

	[Server]
	public void OnTurnStart()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorCinematicRequests::OnTurnStart()' called on client");
			return;
		}
		int num = 0;
		for (int i = 0; i < this.m_abilityRequested.Count; i++)
		{
			if (this.m_abilityRequested[i])
			{
				num++;
				this.m_abilityRequested[i] = false;
			}
		}
		this.Networkm_numCinematicRequestsLeft = this.m_numCinematicRequestsLeft - num;
	}

	public int NumRequestsLeft(int tauntId)
	{
		if (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("NoCooldowns"))
		{
			if (GameManager.Get().GameConfig.GameType == GameType.Practice)
			{
			}
			else
			{
				bool flag = true;
				using (List<GameObject>.Enumerator enumerator = GameFlowData.Get().GetPlayers().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject = enumerator.Current;
						if (gameObject.GetComponent<BotController>() == null)
						{
							flag = false;
							goto IL_AC;
						}
					}
				}
				IL_AC:
				if (flag)
				{
					return 0xA;
				}
				if (this.m_cinematicsPlayedThisMatch.Contains(tauntId))
				{
					return 0;
				}
				int num = this.m_numCinematicRequestsLeft;
				for (int i = 0; i < this.m_abilityRequested.Count; i++)
				{
					if (this.m_abilityRequested[i])
					{
						num--;
					}
				}
				return num;
			}
		}
		return 0xA;
	}

	[Server]
	public void ProcessAbilityCinematicRequest(AbilityData.ActionType actionType, bool requested, int animTauntIndex, int tauntUniqueId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorCinematicRequests::ProcessAbilityCinematicRequest(AbilityData/ActionType,System.Boolean,System.Int32,System.Int32)' called on client");
			return;
		}
	}

	public void SendAbilityCinematicRequest(AbilityData.ActionType actionType, bool requested, int animTauntIndex, int tauntId)
	{
		if (NetworkServer.active)
		{
			this.ProcessAbilityCinematicRequest(actionType, requested, animTauntIndex, tauntId);
		}
		else
		{
			this.CallCmdSelectAbilityCinematicRequest((int)actionType, requested, animTauntIndex, tauntId);
		}
	}

	[Command]
	private void CmdSelectAbilityCinematicRequest(int actionType, bool requested, int animTauntIndex, int tauntId)
	{
		this.ProcessAbilityCinematicRequest((AbilityData.ActionType)actionType, requested, animTauntIndex, tauntId);
	}

	private void UNetVersion()
	{
	}

	public int Networkm_numCinematicRequestsLeft
	{
		get
		{
			return this.m_numCinematicRequestsLeft;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_numCinematicRequestsLeft, 2U);
		}
	}

	protected static void InvokeSyncListm_abilityRequested(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_abilityRequested called on server.");
			return;
		}
		((ActorCinematicRequests)obj).m_abilityRequested.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_cinematicsPlayedThisMatch(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_cinematicsPlayedThisMatch called on server.");
			return;
		}
		((ActorCinematicRequests)obj).m_cinematicsPlayedThisMatch.HandleMsg(reader);
	}

	protected static void InvokeCmdCmdSelectAbilityCinematicRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSelectAbilityCinematicRequest called on client.");
			return;
		}
		((ActorCinematicRequests)obj).CmdSelectAbilityCinematicRequest((int)reader.ReadPackedUInt32(), reader.ReadBoolean(), (int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	public void CallCmdSelectAbilityCinematicRequest(int actionType, bool requested, int animTauntIndex, int tauntId)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSelectAbilityCinematicRequest called on server.");
			return;
		}
		if (base.isServer)
		{
			this.CmdSelectAbilityCinematicRequest(actionType, requested, animTauntIndex, tauntId);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorCinematicRequests.kCmdCmdSelectAbilityCinematicRequest);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionType);
		networkWriter.Write(requested);
		networkWriter.WritePackedUInt32((uint)animTauntIndex);
		networkWriter.WritePackedUInt32((uint)tauntId);
		base.SendCommandInternal(networkWriter, 0, "CmdSelectAbilityCinematicRequest");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListBool.WriteInstance(writer, this.m_abilityRequested);
			writer.WritePackedUInt32((uint)this.m_numCinematicRequestsLeft);
			SyncListInt.WriteInstance(writer, this.m_cinematicsPlayedThisMatch);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListBool.WriteInstance(writer, this.m_abilityRequested);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_numCinematicRequestsLeft);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_cinematicsPlayedThisMatch);
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
			SyncListBool.ReadReference(reader, this.m_abilityRequested);
			this.m_numCinematicRequestsLeft = (int)reader.ReadPackedUInt32();
			SyncListInt.ReadReference(reader, this.m_cinematicsPlayedThisMatch);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListBool.ReadReference(reader, this.m_abilityRequested);
		}
		if ((num & 2) != 0)
		{
			this.m_numCinematicRequestsLeft = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			SyncListInt.ReadReference(reader, this.m_cinematicsPlayedThisMatch);
		}
	}
}
