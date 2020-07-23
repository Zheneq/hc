using System.Collections.Generic;
using UnityEngine;

public static class AbilityResultsUtils
{
	public static Dictionary<ActorData, ClientActorHitResults> DeSerializeActorHitResultsDictionaryFromStream(ref IBitStream stream)
	{
		Dictionary<ActorData, ClientActorHitResults> dictionary = new Dictionary<ActorData, ClientActorHitResults>();
		sbyte hitResultNum = 0;
		stream.Serialize(ref hitResultNum);
		for (int i = 0; i < hitResultNum; i++)
		{
			sbyte actorIndex = (sbyte)ActorData.s_invalidActorIndex;
			stream.Serialize(ref actorIndex);
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
			ClientActorHitResults hitResults = new ClientActorHitResults(ref stream);
			if (actorData != null)
			{
				dictionary.Add(actorData, hitResults);
			}
		}
		return dictionary;
	}

	public static Dictionary<Vector3, ClientPositionHitResults> DeSerializePositionHitResultsDictionaryFromStream(ref IBitStream stream)
	{
		sbyte hitResultNum = 0;
		stream.Serialize(ref hitResultNum);
		Dictionary<Vector3, ClientPositionHitResults> dictionary = new Dictionary<Vector3, ClientPositionHitResults>(hitResultNum);
		for (int i = 0; i < hitResultNum; i++)
		{
			Vector3 pos = Vector3.zero;
			stream.Serialize(ref pos);
			ClientPositionHitResults hitResults = new ClientPositionHitResults(ref stream);
			dictionary.Add(pos, hitResults);
		}
		return dictionary;
	}

	public static List<ClientEffectStartData> DeSerializeEffectsToStartFromStream(ref IBitStream stream)
	{
		sbyte effectStartNum = 0;
		stream.Serialize(ref effectStartNum);
		List<ClientEffectStartData> effectStartList = new List<ClientEffectStartData>(effectStartNum);
		for (int i = 0; i < effectStartNum; i++)
		{
			uint effectGUID = 0u;
			stream.Serialize(ref effectGUID);
			sbyte seqStartNum = 0;
			stream.Serialize(ref seqStartNum);
			List<ServerClientUtils.SequenceStartData> seqStartList = new List<ServerClientUtils.SequenceStartData>(seqStartNum);
			for (int j = 0; j < seqStartNum; j++)
			{
				ServerClientUtils.SequenceStartData seqStart = ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(ref stream);
				seqStartList.Add(seqStart);
			}
			sbyte casterActorIndex = (sbyte)ActorData.s_invalidActorIndex;
			stream.Serialize(ref casterActorIndex);
			ActorData caster = GameFlowData.Get().FindActorByActorIndex(casterActorIndex);
			sbyte targetActorIndex = (sbyte)ActorData.s_invalidActorIndex;
			stream.Serialize(ref targetActorIndex);
			ActorData effectTarget = GameFlowData.Get().FindActorByActorIndex(targetActorIndex);
			List<StatusType> statuses = new List<StatusType>();
			if (targetActorIndex != ActorData.s_invalidActorIndex)
			{
				sbyte statusNum = 0;
				stream.Serialize(ref statusNum);
				for (int k = 0; k < statusNum; k++)
				{
					byte statusType = 0;
					stream.Serialize(ref statusType);
					statuses.Add((StatusType)statusType);
				}
			}
			List<StatusType> statusesOnTurnStart = new List<StatusType>();
			if (targetActorIndex != ActorData.s_invalidActorIndex)
			{
				sbyte statusOnTurnStartNum = 0;
				stream.Serialize(ref statusOnTurnStartNum);
				for (int l = 0; l < statusOnTurnStartNum; l++)
				{
					byte statusType = 0;
					stream.Serialize(ref statusType);
					statusesOnTurnStart.Add((StatusType)statusType);
				}
			}
			bool isBuff = false;
			bool isDebuff = false;
			bool hasMovementDebuff = false;
			bool hasAbsorb = false;
			bool hasExpectedHoT = false;
			byte bitField = 0;
			stream.Serialize(ref bitField);
			ServerClientUtils.GetBoolsFromBitfield(bitField, out isBuff, out isDebuff, out hasMovementDebuff, out hasAbsorb, out hasExpectedHoT);
			short absorb = 0;
			if (hasAbsorb)
			{
				stream.Serialize(ref absorb);
			}
			short expectedHoT = 0;
			if (hasExpectedHoT)
			{
				stream.Serialize(ref expectedHoT);
			}
			ClientEffectStartData effectStart = new ClientEffectStartData(
				(int)effectGUID,
				seqStartList,
				effectTarget,
				caster,
				statuses,
				statusesOnTurnStart,
				absorb,
				expectedHoT,
				isBuff,
				isDebuff,
				hasMovementDebuff);
			effectStartList.Add(effectStart);
		}
		return effectStartList;
	}

	public static List<ClientBarrierStartData> DeSerializeBarriersToStartFromStream(ref IBitStream stream)
	{
		sbyte barrierStartNum = 0;
		stream.Serialize(ref barrierStartNum);
		List<ClientBarrierStartData> list = new List<ClientBarrierStartData>(barrierStartNum);
		
		for (int num = 0; num < barrierStartNum; num++)
		{
			BarrierSerializeInfo info = new BarrierSerializeInfo();
			BarrierSerializeInfo.SerializeBarrierInfo(stream, ref info);
			int barrierGUID = info.m_guid;
			sbyte seqenceStartNum = 0;
			stream.Serialize(ref seqenceStartNum);
			List<ServerClientUtils.SequenceStartData> sequenceStartList = new List<ServerClientUtils.SequenceStartData>(seqenceStartNum);
			for (int i = 0; i < seqenceStartNum; i++)
			{
				ServerClientUtils.SequenceStartData sequenceStartData = ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(ref stream);
				sequenceStartData.SetTargetPos(info.m_center);
				sequenceStartList.Add(sequenceStartData);
			}
			ClientBarrierStartData clientBarrierStartData = new ClientBarrierStartData(barrierGUID, sequenceStartList, info);
			list.Add(clientBarrierStartData);
			if (BarrierManager.Get() != null)
			{
				BarrierManager.Get().AddClientBarrierInfo(clientBarrierStartData.m_barrierGameplayInfo);
			}
			
		}
		return list;
	}

	public static List<int> DeSerializeEffectsForRemovalFromStream(ref IBitStream stream)
	{
		sbyte num = 0;
		stream.Serialize(ref num);
		List<int> list = new List<int>(num);
		for (int i = 0; i < num; i++)
		{
			int id = -1;
			stream.Serialize(ref id);
			list.Add(id);
		}
		return list;
	}

	public static List<int> DeSerializeBarriersForRemovalFromStream(ref IBitStream stream)
	{
		sbyte num = 0;
		stream.Serialize(ref num);
		List<int> list = new List<int>(num);
		for (int i = 0; i < num; i++)
		{
			int id = -1;
			stream.Serialize(ref id);
			list.Add(id);
		}
		return list;
	}

	public static List<ServerClientUtils.SequenceStartData> DeSerializeSequenceStartDataListFromStream(ref IBitStream stream)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		sbyte seqStartNum = 0;
		stream.Serialize(ref seqStartNum);
		for (int i = 0; i < seqStartNum; i++)
		{
			ServerClientUtils.SequenceStartData item = ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(ref stream);
			list.Add(item);
		}
		return list;
	}

	public static List<ServerClientUtils.SequenceEndData> DeSerializeSequenceEndDataListFromStream(ref IBitStream stream)
	{
		List<ServerClientUtils.SequenceEndData> list = new List<ServerClientUtils.SequenceEndData>();
		sbyte num = 0;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			ServerClientUtils.SequenceEndData item = ServerClientUtils.SequenceEndData.SequenceEndData_DeserializeFromStream(ref stream);
			list.Add(item);
		}
		return list;
	}

	public static ClientAbilityResults DeSerializeClientAbilityResultsFromStream(ref IBitStream stream)
	{
		sbyte casterActorIndex = (sbyte)ActorData.s_invalidActorIndex;
		sbyte abilityAction = -1;
		stream.Serialize(ref casterActorIndex);
		stream.Serialize(ref abilityAction);
		List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = DeSerializePositionHitResultsDictionaryFromStream(ref stream);
		return new ClientAbilityResults(casterActorIndex, abilityAction, seqStartDataList, actorToHitResults, posToHitResults);
	}

	public static ClientEffectResults DeSerializeClientEffectResultsFromStream(ref IBitStream stream)
	{
		uint effectGUID = 0u;
		sbyte casterActorIndex = (sbyte)ActorData.s_invalidActorIndex;
		sbyte sourceAbilityActionType = -1;
		stream.Serialize(ref effectGUID);
		stream.Serialize(ref casterActorIndex);
		stream.Serialize(ref sourceAbilityActionType);
		List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = DeSerializePositionHitResultsDictionaryFromStream(ref stream);
		ActorData effectCaster = GameFlowData.Get().FindActorByActorIndex(casterActorIndex);
		return new ClientEffectResults(
			(int)effectGUID,
			effectCaster,
			(AbilityData.ActionType)sourceAbilityActionType,
			seqStartDataList,
			actorToHitResults,
			posToHitResults);
	}

	public static ClientBarrierResults DeSerializeClientBarrierResultsFromStream(ref IBitStream stream)
	{
		int barrierGUID = -1;
		sbyte casterIndex = (sbyte)ActorData.s_invalidActorIndex;
		stream.Serialize(ref barrierGUID);
		stream.Serialize(ref casterIndex);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = DeSerializePositionHitResultsDictionaryFromStream(ref stream);
		ActorData barrierCaster = GameFlowData.Get().FindActorByActorIndex(casterIndex);
		return new ClientBarrierResults(barrierGUID, barrierCaster, actorToHitResults, posToHitResults);
	}

	public static ClientMovementResults DeSerializeClientMovementResultsFromStream(ref IBitStream stream)
	{
		sbyte triggeringMoverActorIndex = (sbyte)ActorData.s_invalidActorIndex;
		stream.Serialize(ref triggeringMoverActorIndex);
		BoardSquarePathInfo triggeringPath = MovementUtils.DeSerializeLightweightPath(stream);
		List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
		sbyte responseType = 0;
		stream.Serialize(ref responseType);
		MovementResults_GameplayResponseType movementResults_GameplayResponseType = (MovementResults_GameplayResponseType)responseType;
		ClientEffectResults effectResults = null;
		ClientBarrierResults barrierResults = null;
		ClientAbilityResults powerupResults = null;
		ClientAbilityResults gameModeResults = null;
		if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.Effect)
		{
			effectResults = DeSerializeClientEffectResultsFromStream(ref stream);
		}
		else if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.Barrier)
		{
			barrierResults = DeSerializeClientBarrierResultsFromStream(ref stream);
		}
		else if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.Powerup)
		{
			powerupResults = DeSerializeClientAbilityResultsFromStream(ref stream);
		}
		else if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.GameMode)
		{
			gameModeResults = DeSerializeClientAbilityResultsFromStream(ref stream);
		}
		ActorData triggeringMover = GameFlowData.Get().FindActorByActorIndex(triggeringMoverActorIndex);
		return new ClientMovementResults(triggeringMover, triggeringPath, seqStartDataList, effectResults, barrierResults, powerupResults, gameModeResults);
	}

	public static List<ClientMovementResults> DeSerializeClientMovementResultsListFromStream(ref IBitStream stream)
	{
		List<ClientMovementResults> list = new List<ClientMovementResults>();
		sbyte num = 0;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			ClientMovementResults item = DeSerializeClientMovementResultsFromStream(ref stream);
			list.Add(item);
		}
		return list;
	}

	public static List<ClientReactionResults> DeSerializeClientReactionResultsFromStream(ref IBitStream stream)
	{
		List<ClientReactionResults> list = new List<ClientReactionResults>();
		sbyte num = 0;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
			ClientEffectResults effectResults = DeSerializeClientEffectResultsFromStream(ref stream);
			byte extraFlags = 0;
			stream.Serialize(ref extraFlags);
			ClientReactionResults item = new ClientReactionResults(effectResults, seqStartDataList, extraFlags);
			list.Add(item);
		}
		return list;
	}

	public static List<int> DeSerializePowerupsToRemoveFromStream(ref IBitStream stream)
	{
		List<int> list = new List<int>();
		sbyte num = 0;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			int id = 0;
			stream.Serialize(ref id);
			list.Add(id);
		}
		return list;
	}

	public static List<ClientPowerupStealData> DeSerializePowerupsToStealFromStream(ref IBitStream stream)
	{
		sbyte num = 0;
		stream.Serialize(ref num);
		List<ClientPowerupStealData> list = new List<ClientPowerupStealData>(num);
		for (int i = 0; i < num; i++)
		{
			int powerupGUID = -1;
			stream.Serialize(ref powerupGUID);
			ClientPowerupResults powerupResults = DeSerializeClientPowerupResultsFromStream(ref stream);
			ClientPowerupStealData item = new ClientPowerupStealData(powerupGUID, powerupResults);
			list.Add(item);
		}
		return list;
	}

	public static ClientPowerupResults DeSerializeClientPowerupResultsFromStream(ref IBitStream stream)
	{
		List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
		ClientAbilityResults clientAbilityResults = DeSerializeClientAbilityResultsFromStream(ref stream);
		return new ClientPowerupResults(seqStartDataList, clientAbilityResults);
	}

	public static List<ClientGameModeEvent> DeSerializeClientGameModeEventListFromStream(ref IBitStream stream)
	{
		List<ClientGameModeEvent> list = new List<ClientGameModeEvent>();
		sbyte num = 0;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			ClientGameModeEvent item = DeSerializeClientGameModeEventFromStream(ref stream);
			list.Add(item);
		}
		return list;
	}

	public static ClientGameModeEvent DeSerializeClientGameModeEventFromStream(ref IBitStream stream)
	{
		sbyte value = 0;
		byte objectGUID = 0;
		sbyte primaryActorIndex = 0;
		sbyte secondaryActorIndex = 0;
		sbyte x = -1;
		sbyte y = -1;
		int eventGUID = 0;
		stream.Serialize(ref value);
		stream.Serialize(ref objectGUID);
		stream.Serialize(ref primaryActorIndex);
		stream.Serialize(ref secondaryActorIndex);
		stream.Serialize(ref x);
		stream.Serialize(ref y);
		stream.Serialize(ref eventGUID);
		GameModeEventType eventType = (GameModeEventType)value;
		BoardSquare square = (x == -1 && y == -1) ? null : Board.Get().GetSquare(x, y);
		ActorData primaryActor = primaryActorIndex != ActorData.s_invalidActorIndex ? GameFlowData.Get().FindActorByActorIndex(primaryActorIndex) : null;
		ActorData secondaryActor = secondaryActorIndex != ActorData.s_invalidActorIndex ? GameFlowData.Get().FindActorByActorIndex(secondaryActorIndex) : null;
		return new ClientGameModeEvent(eventType, objectGUID, square, primaryActor, secondaryActor, eventGUID);
	}

	public static List<int> DeSerializeClientOverconListFromStream(ref IBitStream stream)
	{
		List<int> list = new List<int>();
		sbyte num = 0;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			int id = -1;
			stream.Serialize(ref id);
			list.Add(id);
		}
		return list;
	}

	public static void SerializeIntListToStream(ref IBitStream stream, List<int> list)
	{
		sbyte num = (sbyte)list.Count;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			int id = list[i];
			stream.Serialize(ref id);
		}
	}

	public static void SerializeEffectsToStartToStream(ref IBitStream stream, List<ClientEffectStartData> effectStartList)
	{
		sbyte effectStartNum = (sbyte)effectStartList.Count;
		stream.Serialize(ref effectStartNum);
		for (int i = 0; i < effectStartNum; i++)
		{
			ClientEffectStartData effectStart = effectStartList[i];

			uint effectGUID = (uint)effectStart.m_effectGUID;
			stream.Serialize(ref effectGUID);
			List<ServerClientUtils.SequenceStartData> seqStartList = effectStart.m_sequenceStartDataList;
			sbyte seqStartNum = (sbyte)seqStartList.Count;
			stream.Serialize(ref seqStartNum);
			for (int j = 0; j < seqStartNum; j++)
			{
				ServerClientUtils.SequenceStartData seqStart = seqStartList[j];
				ServerClientUtils.SequenceStartData.Serialize(ref stream, ref seqStart);
			}
			sbyte casterActorIndex = (sbyte)(effectStart.m_caster?.ActorIndex ?? ActorData.s_invalidActorIndex);
			stream.Serialize(ref casterActorIndex);
			sbyte targetActorIndex = (sbyte)(effectStart.m_effectTarget?.ActorIndex ?? ActorData.s_invalidActorIndex);
			stream.Serialize(ref targetActorIndex);
			if (targetActorIndex != ActorData.s_invalidActorIndex)
			{
				sbyte statusNum = (sbyte)effectStart.m_statuses.Count;
				stream.Serialize(ref statusNum);
				for (int j = 0; j < statusNum; j++)
				{
					byte statusType = (byte)effectStart.m_statuses[j];
					stream.Serialize(ref statusType);
				}
			}
			if (targetActorIndex != ActorData.s_invalidActorIndex)
			{
				sbyte statusOnTurnStartNum = (sbyte)effectStart.m_statusesOnTurnStart.Count;
				stream.Serialize(ref statusOnTurnStartNum);
				for (int j = 0; j < statusOnTurnStartNum; j++)
				{
					byte statusType = (byte)effectStart.m_statusesOnTurnStart[j];
					stream.Serialize(ref statusType);
				}
			}
			bool hasAbsorb = effectStart.m_absorb != 0;
			bool hasExpectedHoT = effectStart.m_expectedHoT != 0;
			byte bitField = ServerClientUtils.CreateBitfieldFromBools(
				effectStart.m_isBuff,
				effectStart.m_isDebuff,
				effectStart.m_hasMovementDebuff,
				hasAbsorb,
				hasExpectedHoT,
				false, false, false);
			stream.Serialize(ref bitField);
			if (hasAbsorb)
			{
				short absorb = (short)effectStart.m_absorb;
				stream.Serialize(ref absorb);
			}
			if (hasExpectedHoT)
			{
				short expectedHoT = (short)effectStart.m_expectedHoT;
				stream.Serialize(ref expectedHoT);
			}
		}
	}

	public static void SerializeBarriersToStartToStream(ref IBitStream stream, List<ClientBarrierStartData> barrierStartList)
	{
		sbyte barrierStartNum = (sbyte)barrierStartList.Count;
		stream.Serialize(ref barrierStartNum);

		for (int i = 0; i < barrierStartNum; i++)
		{
			ClientBarrierStartData clientBarrierStartData = barrierStartList[i];
			BarrierSerializeInfo info = clientBarrierStartData.m_barrierGameplayInfo;
			BarrierSerializeInfo.SerializeBarrierInfo(stream, ref info);
			sbyte seqenceStartNum = (sbyte)clientBarrierStartData.m_sequenceStartDataList.Count;
			stream.Serialize(ref seqenceStartNum);
			for (int j = 0; j < seqenceStartNum; j++)
			{
				ServerClientUtils.SequenceStartData seqStart = clientBarrierStartData.m_sequenceStartDataList[j];
				ServerClientUtils.SequenceStartData.Serialize(ref stream, ref seqStart);
			}
		}
	}

	public static void SerializeSequenceEndDataListToStream(ref IBitStream stream, List<ServerClientUtils.SequenceEndData> list)
	{
		sbyte num = (sbyte)list.Count;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			list[i].SequenceEndData_SerializeToStream(ref stream);
		}
	}

	public static void SerializeClientReactionResultsToStream(ref IBitStream stream, List<ClientReactionResults> list)
	{
		sbyte num = (sbyte)list.Count;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			list[i].SerializeToStream(ref stream);
		}
	}

	public static void SerializeSequenceStartDataListToStream(ref IBitStream stream, List<ServerClientUtils.SequenceStartData> list)
	{
		sbyte seqStartNum = (sbyte)list.Count;
		stream.Serialize(ref seqStartNum);
		for (int i = 0; i < seqStartNum; i++)
		{
			ServerClientUtils.SequenceStartData seqStart = list[i];
			ServerClientUtils.SequenceStartData.Serialize(ref stream, ref seqStart);
		}
	}

	public static void SerializeActorHitResultsDictionaryToStream(ref IBitStream stream, Dictionary<ActorData, ClientActorHitResults> dictionary)
	{
		sbyte hitResultNum = (sbyte)dictionary.Count;
		stream.Serialize(ref hitResultNum);
		foreach (var e in dictionary)
		{
			sbyte actorIndex = (sbyte)e.Key.ActorIndex;
			stream.Serialize(ref actorIndex);
			e.Value.SerializeToStream(ref stream);
		}
	}

	public static void SerializePositionHitResultsDictionaryToStream(ref IBitStream stream, Dictionary<Vector3, ClientPositionHitResults> dictionary)
	{
		sbyte hitResultNum = (sbyte)dictionary.Count;
		stream.Serialize(ref hitResultNum);
		foreach (var e in dictionary)
		{
			Vector3 pos = e.Key;
			stream.Serialize(ref pos);
			e.Value.SerializeToStream(ref stream);
		}
	}

	public static void SerializeClientMovementResultsListToStream(ref IBitStream stream, List<ClientMovementResults> list)
	{
		sbyte num = (sbyte)list.Count;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			list[i].SerializeToStream(ref stream);
		}
	}

	public static void SerializePowerupsToStealToStream(ref IBitStream stream, List<ClientPowerupStealData> list)
	{
		sbyte num = (sbyte)list.Count;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			int powerupGUID = list[i].m_powerupGuid;
			stream.Serialize(ref powerupGUID);
			list[i].m_powerupResults.SerializeToStream(ref stream);
		}
	}

	public static void SerializeClientGameModeEventListToStream(ref IBitStream stream, List<ClientGameModeEvent> list)
	{
		sbyte num = (sbyte)list.Count;
		stream.Serialize(ref num);
		for (int i = 0; i < num; i++)
		{
			SerializeClientGameModeEventToStream(ref stream, list[i]);
		}
	}

	public static void SerializeClientGameModeEventToStream(ref IBitStream stream, ClientGameModeEvent gameModeEvent)
	{
		sbyte eventTypeId = (sbyte)gameModeEvent.m_eventType;
		byte objectGUID = gameModeEvent.m_objectGuid;
		sbyte primaryActorIndex = (sbyte)(gameModeEvent.m_primaryActor?.ActorIndex ?? ActorData.s_invalidActorIndex);
		sbyte secondaryActorIndex = (sbyte)(gameModeEvent.m_secondaryActor?.ActorIndex ?? ActorData.s_invalidActorIndex);
		sbyte x = (sbyte)(gameModeEvent.m_square?.x ?? -1);
		sbyte y = (sbyte)(gameModeEvent.m_square?.y ?? -1);
		int eventGUID = gameModeEvent.m_eventGuid;
		stream.Serialize(ref eventTypeId);
		stream.Serialize(ref objectGUID);
		stream.Serialize(ref primaryActorIndex);
		stream.Serialize(ref secondaryActorIndex);
		stream.Serialize(ref x);
		stream.Serialize(ref y);
		stream.Serialize(ref eventGUID);
	}
}
