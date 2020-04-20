using System;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityResultsUtils
{
	public unsafe static Dictionary<ActorData, ClientActorHitResults> DeSerializeActorHitResultsDictionaryFromStream(ref IBitStream stream)
	{
		Dictionary<ActorData, ClientActorHitResults> dictionary = new Dictionary<ActorData, ClientActorHitResults>();
		sbyte b = 0;
		stream.Serialize(ref b);
		for (int i = 0; i < (int)b; i++)
		{
			sbyte b2 = (sbyte)ActorData.s_invalidActorIndex;
			stream.Serialize(ref b2);
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex((int)b2);
			ClientActorHitResults value = new ClientActorHitResults(ref stream);
			if (actorData != null)
			{
				dictionary.Add(actorData, value);
			}
		}
		return dictionary;
	}

	public unsafe static Dictionary<Vector3, ClientPositionHitResults> DeSerializePositionHitResultsDictionaryFromStream(ref IBitStream stream)
	{
		sbyte b = 0;
		stream.Serialize(ref b);
		Dictionary<Vector3, ClientPositionHitResults> dictionary = new Dictionary<Vector3, ClientPositionHitResults>((int)b);
		for (int i = 0; i < (int)b; i++)
		{
			Vector3 zero = Vector3.zero;
			stream.Serialize(ref zero);
			ClientPositionHitResults value = new ClientPositionHitResults(ref stream);
			dictionary.Add(zero, value);
		}
		return dictionary;
	}

	public unsafe static List<ClientEffectStartData> DeSerializeEffectsToStartFromStream(ref IBitStream stream)
	{
		sbyte b = 0;
		stream.Serialize(ref b);
		List<ClientEffectStartData> list = new List<ClientEffectStartData>((int)b);
		for (int i = 0; i < (int)b; i++)
		{
			uint effectGUID = 0U;
			stream.Serialize(ref effectGUID);
			sbyte b2 = 0;
			stream.Serialize(ref b2);
			List<ServerClientUtils.SequenceStartData> list2 = new List<ServerClientUtils.SequenceStartData>((int)b2);
			for (int j = 0; j < (int)b2; j++)
			{
				ServerClientUtils.SequenceStartData item = ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(ref stream);
				list2.Add(item);
			}
			sbyte b3 = (sbyte)ActorData.s_invalidActorIndex;
			stream.Serialize(ref b3);
			ActorData caster = GameFlowData.Get().FindActorByActorIndex((int)b3);
			sbyte b4 = (sbyte)ActorData.s_invalidActorIndex;
			stream.Serialize(ref b4);
			ActorData effectTarget = GameFlowData.Get().FindActorByActorIndex((int)b4);
			List<StatusType> list3 = new List<StatusType>();
			List<StatusType> list4 = new List<StatusType>();
			if ((int)b4 != ActorData.s_invalidActorIndex)
			{
				sbyte b5 = 0;
				stream.Serialize(ref b5);
				for (int k = 0; k < (int)b5; k++)
				{
					byte item2 = 0;
					stream.Serialize(ref item2);
					list3.Add((StatusType)item2);
				}
			}
			if ((int)b4 != ActorData.s_invalidActorIndex)
			{
				sbyte b6 = 0;
				stream.Serialize(ref b6);
				for (int l = 0; l < (int)b6; l++)
				{
					byte item3 = 0;
					stream.Serialize(ref item3);
					list4.Add((StatusType)item3);
				}
			}
			bool isBuff = false;
			bool isDebuff = false;
			bool hasMovementDebuff = false;
			bool flag = false;
			bool flag2 = false;
			byte bitField = 0;
			stream.Serialize(ref bitField);
			ServerClientUtils.GetBoolsFromBitfield(bitField, out isBuff, out isDebuff, out hasMovementDebuff, out flag, out flag2);
			short absorb = 0;
			if (flag)
			{
				stream.Serialize(ref absorb);
			}
			short expectedHoT = 0;
			if (flag2)
			{
				stream.Serialize(ref expectedHoT);
			}
			ClientEffectStartData item4 = new ClientEffectStartData((int)effectGUID, list2, effectTarget, caster, list3, list4, (int)absorb, (int)expectedHoT, isBuff, isDebuff, hasMovementDebuff);
			list.Add(item4);
		}
		return list;
	}

	public unsafe static List<ClientBarrierStartData> DeSerializeBarriersToStartFromStream(ref IBitStream stream)
	{
		sbyte b = 0;
		stream.Serialize(ref b);
		List<ClientBarrierStartData> list = new List<ClientBarrierStartData>((int)b);
		for (int i = 0; i < (int)b; i++)
		{
			BarrierSerializeInfo barrierSerializeInfo = new BarrierSerializeInfo();
			BarrierSerializeInfo.SerializeBarrierInfo(stream, ref barrierSerializeInfo);
			int guid = barrierSerializeInfo.m_guid;
			sbyte b2 = 0;
			stream.Serialize(ref b2);
			List<ServerClientUtils.SequenceStartData> list2 = new List<ServerClientUtils.SequenceStartData>((int)b2);
			for (int j = 0; j < (int)b2; j++)
			{
				ServerClientUtils.SequenceStartData sequenceStartData = ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(ref stream);
				sequenceStartData.SetTargetPos(barrierSerializeInfo.m_center);
				list2.Add(sequenceStartData);
			}
			ClientBarrierStartData clientBarrierStartData = new ClientBarrierStartData(guid, list2, barrierSerializeInfo);
			list.Add(clientBarrierStartData);
			if (BarrierManager.Get() != null)
			{
				BarrierManager.Get().AddClientBarrierInfo(clientBarrierStartData.m_barrierGameplayInfo);
			}
		}
		return list;
	}

	public unsafe static List<int> DeSerializeEffectsForRemovalFromStream(ref IBitStream stream)
	{
		sbyte b = 0;
		stream.Serialize(ref b);
		List<int> list = new List<int>((int)b);
		for (int i = 0; i < (int)b; i++)
		{
			int item = -1;
			stream.Serialize(ref item);
			list.Add(item);
		}
		return list;
	}

	public unsafe static List<int> DeSerializeBarriersForRemovalFromStream(ref IBitStream stream)
	{
		sbyte b = 0;
		stream.Serialize(ref b);
		List<int> list = new List<int>((int)b);
		for (int i = 0; i < (int)b; i++)
		{
			int item = -1;
			stream.Serialize(ref item);
			list.Add(item);
		}
		return list;
	}

	public static List<ServerClientUtils.SequenceStartData> DeSerializeSequenceStartDataListFromStream(ref IBitStream stream)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		sbyte b = 0;
		stream.Serialize(ref b);
		for (int i = 0; i < (int)b; i++)
		{
			ServerClientUtils.SequenceStartData item = ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(ref stream);
			list.Add(item);
		}
		return list;
	}

	public unsafe static List<ServerClientUtils.SequenceEndData> DeSerializeSequenceEndDataListFromStream(ref IBitStream stream)
	{
		List<ServerClientUtils.SequenceEndData> list = new List<ServerClientUtils.SequenceEndData>();
		sbyte b = 0;
		stream.Serialize(ref b);
		for (int i = 0; i < (int)b; i++)
		{
			ServerClientUtils.SequenceEndData item = ServerClientUtils.SequenceEndData.SequenceEndData_DeserializeFromStream(ref stream);
			list.Add(item);
		}
		return list;
	}

	public static ClientAbilityResults DeSerializeClientAbilityResultsFromStream(ref IBitStream stream)
	{
		sbyte b = (sbyte)ActorData.s_invalidActorIndex;
		sbyte b2 = -1;
		stream.Serialize(ref b);
		stream.Serialize(ref b2);
		List<ServerClientUtils.SequenceStartData> seqStartDataList = AbilityResultsUtils.DeSerializeSequenceStartDataListFromStream(ref stream);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = AbilityResultsUtils.DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = AbilityResultsUtils.DeSerializePositionHitResultsDictionaryFromStream(ref stream);
		return new ClientAbilityResults((int)b, (int)b2, seqStartDataList, actorToHitResults, posToHitResults);
	}

	public static ClientEffectResults DeSerializeClientEffectResultsFromStream(ref IBitStream stream)
	{
		uint effectGUID = 0U;
		sbyte b = (sbyte)ActorData.s_invalidActorIndex;
		sbyte b2 = -1;
		stream.Serialize(ref effectGUID);
		stream.Serialize(ref b);
		stream.Serialize(ref b2);
		List<ServerClientUtils.SequenceStartData> seqStartDataList = AbilityResultsUtils.DeSerializeSequenceStartDataListFromStream(ref stream);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = AbilityResultsUtils.DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = AbilityResultsUtils.DeSerializePositionHitResultsDictionaryFromStream(ref stream);
		ActorData effectCaster = GameFlowData.Get().FindActorByActorIndex((int)b);
		AbilityData.ActionType sourceAbilityActionType = (AbilityData.ActionType)b2;
		return new ClientEffectResults((int)effectGUID, effectCaster, sourceAbilityActionType, seqStartDataList, actorToHitResults, posToHitResults);
	}

	public static ClientBarrierResults DeSerializeClientBarrierResultsFromStream(ref IBitStream stream)
	{
		int barrierGUID = -1;
		sbyte b = (sbyte)ActorData.s_invalidActorIndex;
		stream.Serialize(ref barrierGUID);
		stream.Serialize(ref b);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = AbilityResultsUtils.DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = AbilityResultsUtils.DeSerializePositionHitResultsDictionaryFromStream(ref stream);
		ActorData barrierCaster = GameFlowData.Get().FindActorByActorIndex((int)b);
		return new ClientBarrierResults(barrierGUID, barrierCaster, actorToHitResults, posToHitResults);
	}

	public unsafe static ClientMovementResults DeSerializeClientMovementResultsFromStream(ref IBitStream stream)
	{
		sbyte b = (sbyte)ActorData.s_invalidActorIndex;
		stream.Serialize(ref b);
		BoardSquarePathInfo triggeringPath = MovementUtils.DeSerializeLightweightPath(stream);
		List<ServerClientUtils.SequenceStartData> seqStartDataList = AbilityResultsUtils.DeSerializeSequenceStartDataListFromStream(ref stream);
		sbyte b2 = 0;
		stream.Serialize(ref b2);
		MovementResults_GameplayResponseType movementResults_GameplayResponseType = (MovementResults_GameplayResponseType)b2;
		ClientEffectResults effectResults = null;
		ClientBarrierResults barrierResults = null;
		ClientAbilityResults powerupResults = null;
		ClientAbilityResults gameModeResults = null;
		if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.Effect)
		{
			effectResults = AbilityResultsUtils.DeSerializeClientEffectResultsFromStream(ref stream);
		}
		else if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.Barrier)
		{
			barrierResults = AbilityResultsUtils.DeSerializeClientBarrierResultsFromStream(ref stream);
		}
		else if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.Powerup)
		{
			powerupResults = AbilityResultsUtils.DeSerializeClientAbilityResultsFromStream(ref stream);
		}
		else if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.GameMode)
		{
			gameModeResults = AbilityResultsUtils.DeSerializeClientAbilityResultsFromStream(ref stream);
		}
		ActorData triggeringMover = GameFlowData.Get().FindActorByActorIndex((int)b);
		return new ClientMovementResults(triggeringMover, triggeringPath, seqStartDataList, effectResults, barrierResults, powerupResults, gameModeResults);
	}

	public unsafe static List<ClientMovementResults> DeSerializeClientMovementResultsListFromStream(ref IBitStream stream)
	{
		List<ClientMovementResults> list = new List<ClientMovementResults>();
		sbyte b = 0;
		stream.Serialize(ref b);
		for (int i = 0; i < (int)b; i++)
		{
			ClientMovementResults item = AbilityResultsUtils.DeSerializeClientMovementResultsFromStream(ref stream);
			list.Add(item);
		}
		return list;
	}

	public unsafe static List<ClientReactionResults> DeSerializeClientReactionResultsFromStream(ref IBitStream stream)
	{
		List<ClientReactionResults> list = new List<ClientReactionResults>();
		sbyte b = 0;
		stream.Serialize(ref b);
		for (int i = 0; i < (int)b; i++)
		{
			List<ServerClientUtils.SequenceStartData> seqStartDataList = AbilityResultsUtils.DeSerializeSequenceStartDataListFromStream(ref stream);
			ClientEffectResults effectResults = AbilityResultsUtils.DeSerializeClientEffectResultsFromStream(ref stream);
			byte extraFlags = 0;
			stream.Serialize(ref extraFlags);
			ClientReactionResults item = new ClientReactionResults(effectResults, seqStartDataList, extraFlags);
			list.Add(item);
		}
		return list;
	}

	public unsafe static List<int> DeSerializePowerupsToRemoveFromStream(ref IBitStream stream)
	{
		List<int> list = new List<int>();
		sbyte b = 0;
		stream.Serialize(ref b);
		for (int i = 0; i < (int)b; i++)
		{
			int item = 0;
			stream.Serialize(ref item);
			list.Add(item);
		}
		return list;
	}

	public unsafe static List<ClientPowerupStealData> DeSerializePowerupsToStealFromStream(ref IBitStream stream)
	{
		sbyte b = 0;
		stream.Serialize(ref b);
		List<ClientPowerupStealData> list = new List<ClientPowerupStealData>((int)b);
		for (int i = 0; i < (int)b; i++)
		{
			int powerupGuid = -1;
			stream.Serialize(ref powerupGuid);
			ClientPowerupResults powerupResults = AbilityResultsUtils.DeSerializeClientPowerupResultsFromStream(ref stream);
			ClientPowerupStealData item = new ClientPowerupStealData(powerupGuid, powerupResults);
			list.Add(item);
		}
		return list;
	}

	public static ClientPowerupResults DeSerializeClientPowerupResultsFromStream(ref IBitStream stream)
	{
		List<ServerClientUtils.SequenceStartData> seqStartDataList = AbilityResultsUtils.DeSerializeSequenceStartDataListFromStream(ref stream);
		ClientAbilityResults clientAbilityResults = AbilityResultsUtils.DeSerializeClientAbilityResultsFromStream(ref stream);
		return new ClientPowerupResults(seqStartDataList, clientAbilityResults);
	}

	public unsafe static List<ClientGameModeEvent> DeSerializeClientGameModeEventListFromStream(ref IBitStream stream)
	{
		List<ClientGameModeEvent> list = new List<ClientGameModeEvent>();
		sbyte b = 0;
		stream.Serialize(ref b);
		for (int i = 0; i < (int)b; i++)
		{
			ClientGameModeEvent item = AbilityResultsUtils.DeSerializeClientGameModeEventFromStream(ref stream);
			list.Add(item);
		}
		return list;
	}

	public unsafe static ClientGameModeEvent DeSerializeClientGameModeEventFromStream(ref IBitStream stream)
	{
		sbyte b = 0;
		byte objectGuid = 0;
		sbyte b2 = 0;
		sbyte b3 = 0;
		sbyte b4 = -1;
		sbyte b5 = -1;
		int eventGuid = 0;
		stream.Serialize(ref b);
		stream.Serialize(ref objectGuid);
		stream.Serialize(ref b2);
		stream.Serialize(ref b3);
		stream.Serialize(ref b4);
		stream.Serialize(ref b5);
		stream.Serialize(ref eventGuid);
		GameModeEventType eventType = (GameModeEventType)b;
		BoardSquare square;
		if ((int)b4 == -1)
		{
			if ((int)b5 == -1)
			{
				square = null;
				goto IL_96;
			}
		}
		square = Board.Get().GetBoardSquare((int)b4, (int)b5);
		IL_96:
		ActorData primaryActor;
		if ((int)b2 == ActorData.s_invalidActorIndex)
		{
			primaryActor = null;
		}
		else
		{
			primaryActor = GameFlowData.Get().FindActorByActorIndex((int)b2);
		}
		ActorData secondaryActor;
		if ((int)b3 == ActorData.s_invalidActorIndex)
		{
			secondaryActor = null;
		}
		else
		{
			secondaryActor = GameFlowData.Get().FindActorByActorIndex((int)b3);
		}
		return new ClientGameModeEvent(eventType, objectGuid, square, primaryActor, secondaryActor, eventGuid);
	}

	public static List<int> DeSerializeClientOverconListFromStream(ref IBitStream stream)
	{
		List<int> list = new List<int>();
		sbyte b = 0;
		stream.Serialize(ref b);
		for (int i = 0; i < (int)b; i++)
		{
			int item = -1;
			stream.Serialize(ref item);
			list.Add(item);
		}
		return list;
	}
}
