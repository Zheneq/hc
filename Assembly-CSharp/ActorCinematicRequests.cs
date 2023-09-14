// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
//using Mirror;
//using Talents;
using UnityEngine;
using UnityEngine.Networking;

public class ActorCinematicRequests : NetworkBehaviour
{
	internal SyncListBool m_abilityRequested = new SyncListBool();
	[SyncVar]
	private int m_numCinematicRequestsLeft = 2; // 99 in rogues
	private SyncListInt m_cinematicsPlayedThisMatch = new SyncListInt();
	private ActorData m_actorData;

	// removed in rogues
	private static int kListm_abilityRequested = -88780988;
	// removed in rogues
	private static int kListm_cinematicsPlayedThisMatch = 782922590;
	// removed in rogues
	private static int kCmdCmdSelectAbilityCinematicRequest = 1550121236;

	public int Networkm_numCinematicRequestsLeft
	{
		get
		{
			return m_numCinematicRequestsLeft;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_numCinematicRequestsLeft, 2u);  // 1UL in rogues
		}
	}

	static ActorCinematicRequests()
	{
		// reactor
		RegisterCommandDelegate(typeof(ActorCinematicRequests), kCmdCmdSelectAbilityCinematicRequest, InvokeCmdCmdSelectAbilityCinematicRequest);
		RegisterSyncListDelegate(typeof(ActorCinematicRequests), kListm_abilityRequested, InvokeSyncListm_abilityRequested);
		RegisterSyncListDelegate(typeof(ActorCinematicRequests), kListm_cinematicsPlayedThisMatch, InvokeSyncListm_cinematicsPlayedThisMatch);
		NetworkCRC.RegisterBehaviour("ActorCinematicRequests", 0);
		// rogues
		//RegisterCommandDelegate(typeof(ActorCinematicRequests), "CmdSelectAbilityCinematicRequest", new NetworkBehaviour.CmdDelegate(ActorCinematicRequests.InvokeCmdCmdSelectAbilityCinematicRequest));
		//RegisterRpcDelegate(typeof(ActorCinematicRequests), "RpcRemoveModForTurnStart", new NetworkBehaviour.CmdDelegate(ActorCinematicRequests.InvokeRpcRpcRemoveModForTurnStart));
	}

	private void Awake()
	{
		m_actorData = GetComponent<ActorData>();

		// removed in rogues
		m_abilityRequested.InitializeBehaviour(this, kListm_abilityRequested);
		m_cinematicsPlayedThisMatch.InitializeBehaviour(this, kListm_cinematicsPlayedThisMatch);

		// moved from OnStartServer in rogues
		//if (NetworkServer.active)
		//{
		//	for (int i = 0; i < AbilityData.NUM_ACTIONS; i++)
		//	{
		//		m_abilityRequested.Add(false);
		//	}
		//}
	}

	// moved to Awake in rogues
	public override void OnStartServer()
	{
		for (int i = 0; i < AbilityData.NUM_ACTIONS; i++)
		{
			m_abilityRequested.Add(false);
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
			Debug.LogWarning("[Server] function 'System.Void ActorCinematicRequests::OnTurnStart()' called on client");
			return;
		}
		int num = 0;
		for (int i = 0; i < m_abilityRequested.Count; i++)
		{
			if (m_abilityRequested[i])
			{
				// rogues?
				//Ability abilityOfActionType = GetComponent<AbilityData>().GetAbilityOfActionType((AbilityData.ActionType)i);
				//if (abilityOfActionType != null)
				//{
				//	abilityOfActionType.ClearAbilityMod(m_actorData);
				//	CallRpcRemoveModForTurnStart(i);
				//}

				num++;
				m_abilityRequested[i] = false;
			}
		}
		Networkm_numCinematicRequestsLeft = m_numCinematicRequestsLeft - num;
	}

	// rogues
	//[ClientRpc]
	//public void RpcRemoveModForTurnStart(int actionType)
	//{
	//	Ability abilityOfActionType = base.GetComponent<AbilityData>().GetAbilityOfActionType((AbilityData.ActionType)actionType);
	//	if (abilityOfActionType != null)
	//	{
	//		abilityOfActionType.ClearAbilityMod(m_actorData);
	//	}
	//}

	public int NumRequestsLeft(int tauntId)
	{
		if (DebugParameters.Get() != null
			&& DebugParameters.Get().GetParameterAsBool("NoCooldowns") || GameManager.Get().GameConfig.GameType == GameType.Practice)  // practive removed in rogues
		{
			return 10;
		}
		bool flag = true;
		foreach (GameObject gameObject in GameFlowData.Get().GetPlayers())
		{
			if (gameObject.GetComponent<BotController>() == null)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			return 10;
		}

		// reactor
		if (m_cinematicsPlayedThisMatch.Contains(tauntId))
		{
			return 0;
		}
		// rogues?
		//m_cinematicsPlayedThisMatch.Contains(tauntId);
		//CharacterTaunt characterTaunt = m_actorData.GetCharacterResourceLink().m_taunts.Find((CharacterTaunt t) => t.m_uniqueID == tauntId);
		//if (characterTaunt != null && characterTaunt.m_modToApplyOnTaunt >= 0 && base.GetComponent<AbilityData>().GetAbilityOfActionType(characterTaunt.m_actionForTaunt) != null)
		//{
		//	AbilityMod abilityMod = TalentManager.Get().GetAbilityMod(m_actorData.m_characterType, characterTaunt.m_actionForTaunt);
		//	if (abilityMod != null && abilityMod.m_techPointCostMod.GetModifiedValue(PveGameplayData.Get().m_tauntEnergyCost) > m_actorData.TechPoints)
		//	{
		//		return 0;
		//	}
		//	if (abilityMod != null && abilityMod.m_maxStocksMod.GetModifiedValue(1) <= m_cinematicsPlayedThisMatch.Count((int id) => id == tauntId))
		//	{
		//		return 0;
		//	}
		//}

		int num = m_numCinematicRequestsLeft;
		for (int i = 0; i < m_abilityRequested.Count; i++)
		{
			if (m_abilityRequested[i])
			{
				num--;
			}
		}
		return num;
	}

	[Server]
	public void ProcessAbilityCinematicRequest(AbilityData.ActionType actionType, bool requested, int animTauntIndex, int tauntUniqueId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorCinematicRequests::ProcessAbilityCinematicRequest(AbilityData/ActionType,System.Boolean,System.Int32,System.Int32)' called on client");
			return;
		}

		// TODO TAUNTS
#if SERVER
		// server-only below
		ActorData actorData = m_actorData;
		ActorTurnSM component = base.GetComponent<ActorTurnSM>();
		Ability abilityOfActionType = base.GetComponent<AbilityData>().GetAbilityOfActionType(actionType);

		// TODO LOW check taunt activation
		// custom
		bool flag = false;
		// rogues
		//bool flag = component.m_tauntRequestedForNextAbility == (int)actionType || (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("AlwaysTauntAutomatically"));

		if (((GameManager.Get() != null
		      && GameManager.Get().GameplayOverrides.IsTauntAllowed(actorData.m_characterType, (int)actionType, tauntUniqueId)) || flag)
		    && actorData != null
		    && component != null
		    && abilityOfActionType != null
		    && actionType >= AbilityData.ActionType.ABILITY_0
		    && actionType < (AbilityData.ActionType)m_abilityRequested.Count
		    && requested != m_abilityRequested[(int)actionType]
		    && (NumRequestsLeft(tauntUniqueId) > 0 || !requested))
		{
			if (requested)
			{
				if (actorData.HasBotController || actorData.m_availableTauntIDs.Contains(tauntUniqueId) || flag)
				{
					if (actorData.GetCharacterResourceLink().m_taunts.Find((CharacterTaunt t) => t.m_actionForTaunt == actionType && t.m_uniqueID == tauntUniqueId) == null)
					{
						Log.Warning($"Taunt entry not found {animTauntIndex} uniqueId: {tauntUniqueId}");
					}
					else if (ServerActionBuffer.Get().AbilityCinematicRequest(actorData, abilityOfActionType, requested, animTauntIndex, tauntUniqueId))
					{
						m_abilityRequested[(int)actionType] = requested;
					}
				}
			}
			else if (ServerActionBuffer.Get().AbilityCinematicRequest(actorData, abilityOfActionType, requested, animTauntIndex, tauntUniqueId))
			{
				m_abilityRequested[(int)actionType] = requested;
			}
		}
#endif
	}

	public void SendAbilityCinematicRequest(AbilityData.ActionType actionType, bool requested, int animTauntIndex, int tauntId)
	{
		if (NetworkServer.active)
		{
			ProcessAbilityCinematicRequest(actionType, requested, animTauntIndex, tauntId);
			return;
		}
		CallCmdSelectAbilityCinematicRequest((int)actionType, requested, animTauntIndex, tauntId);
	}

	[Command]
	private void CmdSelectAbilityCinematicRequest(int actionType, bool requested, int animTauntIndex, int tauntId)
	{
		ProcessAbilityCinematicRequest((AbilityData.ActionType)actionType, requested, animTauntIndex, tauntId);
	}

	// removed in rogues
	private void UNetVersion()
	{
	}

	// added in rogues
#if SERVER
	public void AddUsedUniqueTauntId(int tauntUniqueId)
	{
		if (tauntUniqueId >= 0)
		{
			m_cinematicsPlayedThisMatch.Add(tauntUniqueId);
		}
	}
#endif

	// removed in rogues
	protected static void InvokeSyncListm_abilityRequested(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_abilityRequested called on server.");
			return;
		}
		((ActorCinematicRequests)obj).m_abilityRequested.HandleMsg(reader);
	}

	// removed in rogues
	protected static void InvokeSyncListm_cinematicsPlayedThisMatch(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_cinematicsPlayedThisMatch called on server.");
			return;
		}
		((ActorCinematicRequests)obj).m_cinematicsPlayedThisMatch.HandleMsg(reader);
	}

	// rogues
	//public ActorCinematicRequests()
	//{
	//	base.InitSyncObject(m_abilityRequested);
	//	base.InitSyncObject(m_cinematicsPlayedThisMatch);
	//}

	// rogues
	//private void MirrorProcessed()
	//{
	//}

	protected static void InvokeCmdCmdSelectAbilityCinematicRequest(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSelectAbilityCinematicRequest called on client.");
			return;
		}
		// reactor
		((ActorCinematicRequests)obj).CmdSelectAbilityCinematicRequest((int)reader.ReadPackedUInt32(), reader.ReadBoolean(), (int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
		// rogues
		//((ActorCinematicRequests)obj).CmdSelectAbilityCinematicRequest(reader.ReadPackedInt32(), reader.ReadBoolean(), reader.ReadPackedInt32(), reader.ReadPackedInt32());
	}

	public void CallCmdSelectAbilityCinematicRequest(int actionType, bool requested, int animTauntIndex, int tauntId)
	{
		// removed in rogues
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSelectAbilityCinematicRequest called on server.");
			return;
		}
		if (isServer)
		{
			CmdSelectAbilityCinematicRequest(actionType, requested, animTauntIndex, tauntId);
			return;
		}

		// reactor
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
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(actionType);
		//networkWriter.Write(requested);
		//networkWriter.WritePackedInt32(animTauntIndex);
		//networkWriter.WritePackedInt32(tauntId);
		//base.SendCommandInternal(typeof(ActorCinematicRequests), "CmdSelectAbilityCinematicRequest", networkWriter, 0);
	}

	// rogues
	//protected static void InvokeRpcRpcRemoveModForTurnStart(NetworkBehaviour obj, NetworkReader reader)
	//{
	//	if (!NetworkClient.active)
	//	{
	//		Debug.LogError("RPC RpcRemoveModForTurnStart called on server.");
	//		return;
	//	}
	//	((ActorCinematicRequests)obj).RpcRemoveModForTurnStart(reader.ReadPackedInt32());
	//}

	// rogues
	//public void CallRpcRemoveModForTurnStart(int actionType)
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32(actionType);
	//	this.SendRPCInternal(typeof(ActorCinematicRequests), "RpcRemoveModForTurnStart", networkWriter, 0);
	//}

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListBool.WriteInstance(writer, m_abilityRequested);
			writer.WritePackedUInt32((uint)m_numCinematicRequestsLeft);
			SyncListInt.WriteInstance(writer, m_cinematicsPlayedThisMatch);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListBool.WriteInstance(writer, m_abilityRequested);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_numCinematicRequestsLeft);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_cinematicsPlayedThisMatch);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// rogues
	//public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	//{
	//	bool result = base.OnSerialize(writer, forceAll);
	//	if (forceAll)
	//	{
	//		writer.WritePackedInt32(m_numCinematicRequestsLeft);
	//		return true;
	//	}
	//	writer.WritePackedUInt64(base.syncVarDirtyBits);
	//	if ((base.syncVarDirtyBits & 1UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_numCinematicRequestsLeft);
	//		result = true;
	//	}
	//	return result;
	//}

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListBool.ReadReference(reader, m_abilityRequested);
			m_numCinematicRequestsLeft = (int)reader.ReadPackedUInt32();
			SyncListInt.ReadReference(reader, m_cinematicsPlayedThisMatch);
			return;
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

	// rogues
	//public override void OnDeserialize(NetworkReader reader, bool initialState)
	//{
	//	base.OnDeserialize(reader, initialState);
	//	if (initialState)
	//	{
	//		int networkm_numCinematicRequestsLeft = reader.ReadPackedInt32();
	//		Networkm_numCinematicRequestsLeft = networkm_numCinematicRequestsLeft;
	//		return;
	//	}
	//	long num = (long)reader.ReadPackedUInt64();
	//	if ((num & 1L) != 0L)
	//	{
	//		int networkm_numCinematicRequestsLeft2 = reader.ReadPackedInt32();
	//		Networkm_numCinematicRequestsLeft = networkm_numCinematicRequestsLeft2;
	//	}
	//}
	
#if SERVER
	// custom
	public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
	{
		return TeamSensitiveUtils.OnRebuildObservers_NotForReconnection(observers, this);
	}

	// custom
	public override bool OnCheckObserver(NetworkConnection conn)
	{
		return TeamSensitiveUtils.OnCheckObserver_NotForReconnection(conn, this);
	}
#endif
}
