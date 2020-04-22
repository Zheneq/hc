using System.Collections.Generic;
using UnityEngine;

public static class AbilityResultsUtils
{
	public static Dictionary<ActorData, ClientActorHitResults> DeSerializeActorHitResultsDictionaryFromStream(ref IBitStream stream)
	{
		Dictionary<ActorData, ClientActorHitResults> dictionary = new Dictionary<ActorData, ClientActorHitResults>();
		sbyte value = 0;
		stream.Serialize(ref value);
		for (int i = 0; i < value; i++)
		{
			sbyte value2 = (sbyte)ActorData.s_invalidActorIndex;
			stream.Serialize(ref value2);
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(value2);
			ClientActorHitResults value3 = new ClientActorHitResults(ref stream);
			if (actorData != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				dictionary.Add(actorData, value3);
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return dictionary;
		}
	}

	public static Dictionary<Vector3, ClientPositionHitResults> DeSerializePositionHitResultsDictionaryFromStream(ref IBitStream stream)
	{
		sbyte value = 0;
		stream.Serialize(ref value);
		Dictionary<Vector3, ClientPositionHitResults> dictionary = new Dictionary<Vector3, ClientPositionHitResults>(value);
		for (int i = 0; i < value; i++)
		{
			Vector3 value2 = Vector3.zero;
			stream.Serialize(ref value2);
			ClientPositionHitResults value3 = new ClientPositionHitResults(ref stream);
			dictionary.Add(value2, value3);
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return dictionary;
		}
	}

	public static List<ClientEffectStartData> DeSerializeEffectsToStartFromStream(ref IBitStream stream)
	{
		sbyte value = 0;
		stream.Serialize(ref value);
		List<ClientEffectStartData> list = new List<ClientEffectStartData>(value);
		for (int i = 0; i < value; i++)
		{
			uint value2 = 0u;
			stream.Serialize(ref value2);
			sbyte value3 = 0;
			stream.Serialize(ref value3);
			List<ServerClientUtils.SequenceStartData> list2 = new List<ServerClientUtils.SequenceStartData>(value3);
			for (int j = 0; j < value3; j++)
			{
				ServerClientUtils.SequenceStartData item = ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(ref stream);
				list2.Add(item);
			}
			sbyte value4 = (sbyte)ActorData.s_invalidActorIndex;
			stream.Serialize(ref value4);
			ActorData caster = GameFlowData.Get().FindActorByActorIndex(value4);
			sbyte value5 = (sbyte)ActorData.s_invalidActorIndex;
			stream.Serialize(ref value5);
			ActorData effectTarget = GameFlowData.Get().FindActorByActorIndex(value5);
			List<StatusType> list3 = new List<StatusType>();
			List<StatusType> list4 = new List<StatusType>();
			if (value5 != ActorData.s_invalidActorIndex)
			{
				sbyte value6 = 0;
				stream.Serialize(ref value6);
				for (int k = 0; k < value6; k++)
				{
					byte value7 = 0;
					stream.Serialize(ref value7);
					list3.Add((StatusType)value7);
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
			if (value5 != ActorData.s_invalidActorIndex)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				sbyte value8 = 0;
				stream.Serialize(ref value8);
				for (int l = 0; l < value8; l++)
				{
					byte value9 = 0;
					stream.Serialize(ref value9);
					list4.Add((StatusType)value9);
				}
			}
			bool @out = false;
			bool out2 = false;
			bool out3 = false;
			bool out4 = false;
			bool out5 = false;
			byte value10 = 0;
			stream.Serialize(ref value10);
			ServerClientUtils.GetBoolsFromBitfield(value10, out @out, out out2, out out3, out out4, out out5);
			short value11 = 0;
			if (out4)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				stream.Serialize(ref value11);
			}
			short value12 = 0;
			if (out5)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				stream.Serialize(ref value12);
			}
			ClientEffectStartData item2 = new ClientEffectStartData((int)value2, list2, effectTarget, caster, list3, list4, value11, value12, @out, out2, out3);
			list.Add(item2);
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	public static List<ClientBarrierStartData> DeSerializeBarriersToStartFromStream(ref IBitStream stream)
	{
		sbyte value = 0;
		stream.Serialize(ref value);
		List<ClientBarrierStartData> list = new List<ClientBarrierStartData>(value);
		int num = 0;
		while (num < value)
		{
			int num2 = -1;
			BarrierSerializeInfo info = new BarrierSerializeInfo();
			BarrierSerializeInfo.SerializeBarrierInfo(stream, ref info);
			num2 = info.m_guid;
			sbyte value2 = 0;
			stream.Serialize(ref value2);
			List<ServerClientUtils.SequenceStartData> list2 = new List<ServerClientUtils.SequenceStartData>(value2);
			for (int i = 0; i < value2; i++)
			{
				ServerClientUtils.SequenceStartData sequenceStartData = ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(ref stream);
				sequenceStartData.SetTargetPos(info.m_center);
				list2.Add(sequenceStartData);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				ClientBarrierStartData clientBarrierStartData = new ClientBarrierStartData(num2, list2, info);
				list.Add(clientBarrierStartData);
				if (BarrierManager.Get() != null)
				{
					BarrierManager.Get().AddClientBarrierInfo(clientBarrierStartData.m_barrierGameplayInfo);
				}
				num++;
				goto IL_00ca;
			}
			IL_00ca:;
		}
		return list;
	}

	public static List<int> DeSerializeEffectsForRemovalFromStream(ref IBitStream stream)
	{
		sbyte value = 0;
		stream.Serialize(ref value);
		List<int> list = new List<int>(value);
		for (int i = 0; i < value; i++)
		{
			int value2 = -1;
			stream.Serialize(ref value2);
			list.Add(value2);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public static List<int> DeSerializeBarriersForRemovalFromStream(ref IBitStream stream)
	{
		sbyte value = 0;
		stream.Serialize(ref value);
		List<int> list = new List<int>(value);
		for (int i = 0; i < value; i++)
		{
			int value2 = -1;
			stream.Serialize(ref value2);
			list.Add(value2);
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public static List<ServerClientUtils.SequenceStartData> DeSerializeSequenceStartDataListFromStream(ref IBitStream stream)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		sbyte value = 0;
		stream.Serialize(ref value);
		for (int i = 0; i < value; i++)
		{
			ServerClientUtils.SequenceStartData item = ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(ref stream);
			list.Add(item);
		}
		return list;
	}

	public static List<ServerClientUtils.SequenceEndData> DeSerializeSequenceEndDataListFromStream(ref IBitStream stream)
	{
		List<ServerClientUtils.SequenceEndData> list = new List<ServerClientUtils.SequenceEndData>();
		sbyte value = 0;
		stream.Serialize(ref value);
		for (int i = 0; i < value; i++)
		{
			ServerClientUtils.SequenceEndData item = ServerClientUtils.SequenceEndData.SequenceEndData_DeserializeFromStream(ref stream);
			list.Add(item);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public static ClientAbilityResults DeSerializeClientAbilityResultsFromStream(ref IBitStream stream)
	{
		sbyte value = (sbyte)ActorData.s_invalidActorIndex;
		sbyte value2 = -1;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = DeSerializePositionHitResultsDictionaryFromStream(ref stream);
		return new ClientAbilityResults(value, value2, seqStartDataList, actorToHitResults, posToHitResults);
	}

	public static ClientEffectResults DeSerializeClientEffectResultsFromStream(ref IBitStream stream)
	{
		uint value = 0u;
		sbyte value2 = (sbyte)ActorData.s_invalidActorIndex;
		sbyte value3 = -1;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		stream.Serialize(ref value3);
		List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = DeSerializePositionHitResultsDictionaryFromStream(ref stream);
		ActorData effectCaster = GameFlowData.Get().FindActorByActorIndex(value2);
		AbilityData.ActionType sourceAbilityActionType = (AbilityData.ActionType)value3;
		return new ClientEffectResults((int)value, effectCaster, sourceAbilityActionType, seqStartDataList, actorToHitResults, posToHitResults);
	}

	public static ClientBarrierResults DeSerializeClientBarrierResultsFromStream(ref IBitStream stream)
	{
		int value = -1;
		sbyte value2 = (sbyte)ActorData.s_invalidActorIndex;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = DeSerializePositionHitResultsDictionaryFromStream(ref stream);
		ActorData barrierCaster = GameFlowData.Get().FindActorByActorIndex(value2);
		return new ClientBarrierResults(value, barrierCaster, actorToHitResults, posToHitResults);
	}

	public static ClientMovementResults DeSerializeClientMovementResultsFromStream(ref IBitStream stream)
	{
		sbyte value = (sbyte)ActorData.s_invalidActorIndex;
		stream.Serialize(ref value);
		BoardSquarePathInfo triggeringPath = MovementUtils.DeSerializeLightweightPath(stream);
		List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
		sbyte value2 = 0;
		stream.Serialize(ref value2);
		MovementResults_GameplayResponseType movementResults_GameplayResponseType = (MovementResults_GameplayResponseType)value2;
		ClientEffectResults effectResults = null;
		ClientBarrierResults barrierResults = null;
		ClientAbilityResults powerupResults = null;
		ClientAbilityResults gameModeResults = null;
		if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.Effect)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			effectResults = DeSerializeClientEffectResultsFromStream(ref stream);
		}
		else if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.Barrier)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			barrierResults = DeSerializeClientBarrierResultsFromStream(ref stream);
		}
		else if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.Powerup)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			powerupResults = DeSerializeClientAbilityResultsFromStream(ref stream);
		}
		else if (movementResults_GameplayResponseType == MovementResults_GameplayResponseType.GameMode)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			gameModeResults = DeSerializeClientAbilityResultsFromStream(ref stream);
		}
		ActorData triggeringMover = GameFlowData.Get().FindActorByActorIndex(value);
		return new ClientMovementResults(triggeringMover, triggeringPath, seqStartDataList, effectResults, barrierResults, powerupResults, gameModeResults);
	}

	public static List<ClientMovementResults> DeSerializeClientMovementResultsListFromStream(ref IBitStream stream)
	{
		List<ClientMovementResults> list = new List<ClientMovementResults>();
		sbyte value = 0;
		stream.Serialize(ref value);
		for (int i = 0; i < value; i++)
		{
			ClientMovementResults item = DeSerializeClientMovementResultsFromStream(ref stream);
			list.Add(item);
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public static List<ClientReactionResults> DeSerializeClientReactionResultsFromStream(ref IBitStream stream)
	{
		List<ClientReactionResults> list = new List<ClientReactionResults>();
		sbyte value = 0;
		stream.Serialize(ref value);
		for (int i = 0; i < value; i++)
		{
			List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
			ClientEffectResults effectResults = DeSerializeClientEffectResultsFromStream(ref stream);
			byte value2 = 0;
			stream.Serialize(ref value2);
			ClientReactionResults item = new ClientReactionResults(effectResults, seqStartDataList, value2);
			list.Add(item);
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public static List<int> DeSerializePowerupsToRemoveFromStream(ref IBitStream stream)
	{
		List<int> list = new List<int>();
		sbyte value = 0;
		stream.Serialize(ref value);
		for (int i = 0; i < value; i++)
		{
			int value2 = 0;
			stream.Serialize(ref value2);
			list.Add(value2);
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public static List<ClientPowerupStealData> DeSerializePowerupsToStealFromStream(ref IBitStream stream)
	{
		sbyte value = 0;
		stream.Serialize(ref value);
		List<ClientPowerupStealData> list = new List<ClientPowerupStealData>(value);
		for (int i = 0; i < value; i++)
		{
			int value2 = -1;
			stream.Serialize(ref value2);
			ClientPowerupResults powerupResults = DeSerializeClientPowerupResultsFromStream(ref stream);
			ClientPowerupStealData item = new ClientPowerupStealData(value2, powerupResults);
			list.Add(item);
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
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
		sbyte value = 0;
		stream.Serialize(ref value);
		for (int i = 0; i < value; i++)
		{
			ClientGameModeEvent item = DeSerializeClientGameModeEventFromStream(ref stream);
			list.Add(item);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public static ClientGameModeEvent DeSerializeClientGameModeEventFromStream(ref IBitStream stream)
	{
		sbyte value = 0;
		byte value2 = 0;
		sbyte value3 = 0;
		sbyte value4 = 0;
		sbyte value5 = -1;
		sbyte value6 = -1;
		int value7 = 0;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		stream.Serialize(ref value3);
		stream.Serialize(ref value4);
		stream.Serialize(ref value5);
		stream.Serialize(ref value6);
		stream.Serialize(ref value7);
		GameModeEventType eventType = (GameModeEventType)value;
		BoardSquare square;
		if (value5 == -1)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (value6 == -1)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				square = null;
				goto IL_0096;
			}
		}
		square = Board.Get().GetBoardSquare(value5, value6);
		goto IL_0096;
		IL_0096:
		ActorData primaryActor = (value3 != ActorData.s_invalidActorIndex) ? GameFlowData.Get().FindActorByActorIndex(value3) : null;
		ActorData secondaryActor;
		if (value4 == ActorData.s_invalidActorIndex)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			secondaryActor = null;
		}
		else
		{
			secondaryActor = GameFlowData.Get().FindActorByActorIndex(value4);
		}
		return new ClientGameModeEvent(eventType, value2, square, primaryActor, secondaryActor, value7);
	}

	public static List<int> DeSerializeClientOverconListFromStream(ref IBitStream stream)
	{
		List<int> list = new List<int>();
		sbyte value = 0;
		stream.Serialize(ref value);
		for (int i = 0; i < value; i++)
		{
			int value2 = -1;
			stream.Serialize(ref value2);
			list.Add(value2);
		}
		return list;
	}
}
