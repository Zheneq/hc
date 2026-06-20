// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// TODO SERIALIZATION verify that server code corresponds to client code
public static class AbilityResultsUtils
{
	// server-only
#if SERVER
	public static void SerializeActorHitResultsDictionaryToStream(Dictionary<ActorData, ActorHitResults> actorToHitResults, NetworkWriter writer)
	{
		int position = writer.Position;
		if (actorToHitResults != null)
		{
			sbyte hitResultNum = (sbyte)actorToHitResults.Count;
			writer.Write(hitResultNum);
			foreach (KeyValuePair<ActorData, ActorHitResults> keyValuePair in actorToHitResults)
			{
				writer.Write((sbyte)keyValuePair.Key.ActorIndex);
				keyValuePair.Value.ActorHitResults_SerializeToStream(writer);
			}
		}
		else
		{
			sbyte hitResultNum = 0;
			writer.Write(hitResultNum);
		}
		int num = writer.Position - position;
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning("\t\t\t Serializing Actor Hit Results List: \n\t\t\t numBytes: " + num);
		}
	}

	// rogues compatibility example
	//public static Dictionary<ActorData, ClientActorHitResults> DeSerializeActorHitResultsDictionaryFromStream(NetworkReader reader)
	//{
	//	IBitStream stream = new NetworkReaderAdapter(reader);
	//	return DeSerializeActorHitResultsDictionaryFromStream(ref stream);
	//}
#endif

	// reactor
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

	// server-only
#if SERVER
	public static void SerializePositionHitResultsDictionaryToStream(Dictionary<Vector3, PositionHitResults> positionToHitResults, NetworkWriter writer)
	{
		int position = writer.Position;
		if (positionToHitResults != null)
		{
			sbyte b = (sbyte)positionToHitResults.Count;
			writer.Write(b);
			foreach (KeyValuePair<Vector3, PositionHitResults> keyValuePair in positionToHitResults)
			{
				Vector3 key = keyValuePair.Key;
				writer.Write(key);
				keyValuePair.Value.PositionHitResults_SerializeToStream(writer);
			}
		}
		else
		{
			sbyte b2 = 0;
			writer.Write(b2);
		}
		int num = writer.Position - position;
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning("\t\t\t Serializing Position Hits: \n\t\t\t numBytes: " + num);
		}
	}
#endif

	public static Dictionary<Vector3, ClientPositionHitResults> DeSerializePositionHitResultsDictionaryFromStream(ref IBitStream stream)
	{
		sbyte hitResultNum = 0;
		stream.Serialize(ref hitResultNum);
		Dictionary<Vector3, ClientPositionHitResults> dictionary = new Dictionary<Vector3, ClientPositionHitResults>((int)hitResultNum);
		for (int i = 0; i < hitResultNum; i++)
		{
			Vector3 pos = Vector3.zero;
			stream.Serialize(ref pos);
			ClientPositionHitResults hitResults = new ClientPositionHitResults(ref stream);
			dictionary.Add(pos, hitResults);
		}
		return dictionary;
	}

	// server-only
#if SERVER
	public static void SerializeEffectsToStartToStream(List<global::Effect> effects, NetworkWriter writer)
	{
		sbyte effectStartNum = effects != null ? (sbyte)effects.Count : (sbyte)0;
		writer.Write(effectStartNum);
		Log.Debug($"SerializeEffectsToStartToStream:: effectStartNum={effectStartNum}");
		for (int i = 0; i < effectStartNum; i++)
		{
			int position = writer.Position;
			global::Effect effect = effects[i];
			uint effectGUID = (uint)effect.m_guid;
			// rogues
			//string text = "";
			//EffectSystem.Effect effect2;
			//if ((effect2 = (effect as EffectSystem.Effect)) != null)
			//{
			//	text = effect2.EffectTemplate.ShortGUID();
			//}
			writer.WritePackedUInt32(effectGUID);
			Log.Debug($"SerializeEffectsToStartToStream:: effectGUID={effectGUID}");
			//writer.Write(text);
			List<ServerClientUtils.SequenceStartData> effectStartSeqDataList = effect.GetEffectStartSeqDataList();
			sbyte seqStartNum = (sbyte)effectStartSeqDataList.Count;
			writer.Write(seqStartNum);
			Log.Debug($"SerializeEffectsToStartToStream:: seqStartNum={seqStartNum}");
			for (int j = 0; j < seqStartNum; j++)
			{
				effectStartSeqDataList[j].SequenceStartData_SerializeToStream(writer);
				Log.Debug($"SerializeEffectsToStartToStream:: SequenceStartData_SerializeToStream");
			}
			writer.Write((sbyte)effect.CasterActorIndex);
			Log.Debug($"SerializeEffectsToStartToStream:: CasterActorIndex={effect.CasterActorIndex}");
			sbyte targetActorIndex = (sbyte)(effect.Target != null ? effect.Target.ActorIndex : ActorData.s_invalidActorIndex);
			writer.Write(targetActorIndex);
			Log.Debug($"SerializeEffectsToStartToStream:: targetActorIndex={targetActorIndex}");
			if (targetActorIndex != ActorData.s_invalidActorIndex)
			{
				List<StatusType> statuses = effect.GetStatuses();
				sbyte statusNum;
				if (statuses != null)
				{
					bool isMovementDebuffImmune = effect.Target.GetActorStatus().IsMovementDebuffImmune(true);
					for (int j = statuses.Count - 1; j >= 0; j--)
					{
						if (statuses[j] < StatusType.Revealed || (isMovementDebuffImmune && ActorStatus.IsDispellableMovementDebuff(statuses[j])))
						{
							statuses.RemoveAt(j);
						}
					}
					statusNum = (sbyte)statuses.Count;
				}
				else
				{
					statusNum = 0;
				}
				writer.Write(statusNum);
				Log.Debug($"SerializeEffectsToStartToStream:: statusNum={statusNum}");
				for (int j = 0; j < statusNum; j++)
				{
					writer.Write((byte)statuses[j]);
					Log.Debug($"SerializeEffectsToStartToStream:: status={statuses[j]} ({(byte)statuses[j]})");
				}
			}
			if (targetActorIndex != ActorData.s_invalidActorIndex)
			{
				List<StatusType> statusesOnTurnStart = effect.GetStatusesOnTurnStart();
				sbyte statusOnTurnStartNum;
				if (statusesOnTurnStart != null)
				{
					for (int j = statusesOnTurnStart.Count - 1; j >= 0; j--)
					{
						if (statusesOnTurnStart[j] < StatusType.Revealed)
						{
							statusesOnTurnStart.RemoveAt(j);
						}
					}
					statusOnTurnStartNum = (sbyte)statusesOnTurnStart.Count;
				}
				else
				{
					statusOnTurnStartNum = 0;
				}
				writer.Write(statusOnTurnStartNum);
				Log.Debug($"SerializeEffectsToStartToStream:: statusOnTurnStartNum={statusOnTurnStartNum}");
				for (int j = 0; j < statusOnTurnStartNum; j++)
				{
					writer.Write((byte)statusesOnTurnStart[j]);
					Log.Debug($"SerializeEffectsToStartToStream:: statusOnTurnStart={statusesOnTurnStart[j]} ({(byte)statusesOnTurnStart[j]})");
				}
			}
			short expectedHoT = 0;
			if (effect.Target != null)
			{
				expectedHoT = (short)effect.GetExpectedHealOverTimeTotal();
			}
			bool isBuff = effect.IsBuff();
			bool isDebuff = effect.IsDebuff();
			bool hasMovementDebuff = effect.HasDispellableMovementDebuff();
			bool hasAbsorb = effect.CanAbsorb();
			bool hasExpectedHoT = expectedHoT > 0;
			byte bitField = ServerClientUtils.CreateBitfieldFromBools(isBuff, isDebuff, hasMovementDebuff, hasAbsorb, hasExpectedHoT, false, false, false);
			writer.Write(bitField);
			Log.Debug($"SerializeEffectsToStartToStream:: bitField={bitField}");
			if (hasAbsorb)
			{
				writer.Write((short)effect.Absorbtion.m_absorbAmount);
				Log.Debug($"SerializeEffectsToStartToStream:: m_absorbAmount={effect.Absorbtion.m_absorbAmount}");
			}
			if (hasExpectedHoT)
			{
				writer.Write(expectedHoT);
				Log.Debug($"SerializeEffectsToStartToStream:: expectedHoT={expectedHoT}");
			}
			// rogues
			//if (effect != null && effect.Parent != null && effect.Parent.Ability != null)
			//{
			//	writer.Write((int)effect.Parent.Ability.CachedActionType);
			//}
			//else
			//{
			//	writer.Write(-1);
			//}
			int num3 = writer.Position - position;
			if (ClientAbilityResults.DebugSerializeSizeOn)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"\t\t\t\t Serializing Starting Effect ",
					effect.GetDebugIdentifier(),
					"\n\t\t\t\t numBytes: ",
					num3
				}));
			}
		}
	}
#endif

	public static List<ClientEffectStartData> DeSerializeEffectsToStartFromStream(ref IBitStream stream)
	{
		sbyte effectStartNum = 0;
		stream.Serialize(ref effectStartNum);
		List<ClientEffectStartData> effectStartList = new List<ClientEffectStartData>(effectStartNum);
		for (int i = 0; i < effectStartNum; i++)
		{
			uint effectGUID = 0u;
			stream.Serialize(ref effectGUID);
			// rogues
			//string effectTemplateID = reader.ReadString();
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
			// rogues
			//AbilityData.ActionType abilityActionType = (AbilityData.ActionType)reader.ReadInt32();
			ClientEffectStartData effectStart = new ClientEffectStartData(
                (int)effectGUID,
                //effectTemplateID,  // rogues
                seqStartList,
                effectTarget,
                caster,
                statuses,
                statusesOnTurnStart,
                absorb,
                expectedHoT,
                isBuff,
                isDebuff,
                hasMovementDebuff
				//, abilityActionType  // rogues
				);
			effectStartList.Add(effectStart);
		}
		return effectStartList;
	}

	// server-only
#if SERVER
	public static void SerializeBarriersToStartToStream(List<Barrier> barriers, NetworkWriter writer)
	{
		sbyte barrierStartNum = (sbyte)(barriers != null ? barriers.Count : 0);
		writer.Write(barrierStartNum);
		for (int i = 0; i < barrierStartNum; i++)
		{
			int position = writer.Position;
			Barrier barrier = barriers[i];
			BarrierSerializeInfo info = Barrier.BarrierToSerializeInfo(barrier);
			BarrierSerializeInfo.SerializeBarrierInfo(writer, info);
			List<ServerClientUtils.SequenceStartData> sequenceStartDataList = barrier.GetSequenceStartDataList();
			sbyte b2 = (sbyte)sequenceStartDataList.Count;
			writer.Write(b2);
			for (int j = 0; j < (int)b2; j++)
			{
				sequenceStartDataList[j].SequenceStartData_SerializeToStream(writer);
			}
			int num = writer.Position - position;
			if (ClientAbilityResults.DebugSerializeSizeOn)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"\t\t\t\t Serializing Starting Barrier guid",
					barrier.m_guid,
					": \n\t\t\t\t numBytes: ",
					num
				}));
			}
		}
	}
#endif

	public static List<ClientBarrierStartData> DeSerializeBarriersToStartFromStream(ref IBitStream stream)
	{
		sbyte barrierStartNum = 0;
		stream.Serialize(ref barrierStartNum);
		List<ClientBarrierStartData> list = new List<ClientBarrierStartData>(barrierStartNum);

		for (int num = 0; num < barrierStartNum; num++)
		{
			BarrierSerializeInfo info = new BarrierSerializeInfo();
			// reactor
			BarrierSerializeInfo.SerializeBarrierInfo(stream, ref info);
			// rogues
			//BarrierSerializeInfo.DeserializeBarrierInfo(reader, ref info);
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

	// server-only
#if SERVER
	public static void SerializeEffectsForRemovalToStream(List<ServerAbilityUtils.EffectRemovalData> effectsForRemoval, NetworkWriter writer)
	{
		sbyte b;
		if (effectsForRemoval != null)
		{
			b = (sbyte)effectsForRemoval.Count;
		}
		else
		{
			b = 0;
		}
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			int guid = effectsForRemoval[i].m_effectToRemove.m_guid;
			writer.Write(guid);
		}
	}
#endif

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

	//server-only
#if SERVER
	public static void SerializeBarriersForRemovalToStream(List<ServerAbilityUtils.BarrierRemovalData> barriersForRemoval, NetworkWriter writer)
	{
		sbyte b;
		if (barriersForRemoval != null)
		{
			b = (sbyte)barriersForRemoval.Count;
		}
		else
		{
			b = 0;
		}
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			int guid = barriersForRemoval[i].m_barrierToRemove.m_guid;
			writer.Write(guid);
		}
	}
#endif

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

	// rogues
	//public static void SerializeDynamicGeoDamageToStream(Dictionary<BoardSquare, int> dynamicGeoDamage, NetworkWriter writer)
	//{
	//	short num = 0;
	//	if (dynamicGeoDamage != null)
	//	{
	//		num = (short)dynamicGeoDamage.Count;
	//	}
	//	writer.Write(num);
	//	if (dynamicGeoDamage != null)
	//	{
	//		foreach (KeyValuePair<BoardSquare, int> keyValuePair in dynamicGeoDamage)
	//		{
	//			writer.Write((sbyte)keyValuePair.Key.x);
	//			writer.Write((sbyte)keyValuePair.Key.y);
	//			writer.Write(keyValuePair.Value);
	//		}
	//	}
	//}

	// rogues
	//public static Dictionary<BoardSquare, int> DeSerializeDynamicGeoDamageFromStream(NetworkReader reader)
	//{
	//	short num = reader.ReadInt16();
	//	Dictionary<BoardSquare, int> dictionary = new Dictionary<BoardSquare, int>((int)num);
	//	for (int i = 0; i < (int)num; i++)
	//	{
	//		sbyte x = reader.ReadSByte();
	//		sbyte y = reader.ReadSByte();
	//		BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex((int)x, (int)y);
	//		int value = reader.ReadInt32();
	//		dictionary.Add(squareFromIndex, value);
	//	}
	//	return dictionary;
	//}

	//server-only
#if SERVER
	public static void SerializeSequenceStartDataListToStream(List<ServerClientUtils.SequenceStartData> sequenceDataList, NetworkWriter writer)
	{
		int position = writer.Position;
		sbyte b;
		if (sequenceDataList == null)
		{
			b = 0;
		}
		else
		{
			b = (sbyte)sequenceDataList.Count;
		}
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			sequenceDataList[i].SequenceStartData_SerializeToStream(writer);
		}
		int num = writer.Position - position;
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning("\t\t\t Serializing SequenceStartDataList:\n\t\t\t numBytes: " + num);
		}
	}
#endif

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

	//server-only
#if SERVER
	public static void SerializeSequenceEndDataListToStream(List<ServerClientUtils.SequenceEndData> sequenceEndDataList, NetworkWriter writer)
	{
		int position = writer.Position;
		sbyte b;
		if (sequenceEndDataList == null)
		{
			b = 0;
		}
		else
		{
			b = (sbyte)sequenceEndDataList.Count;
		}
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			sequenceEndDataList[i].SequenceEndData_SerializeToStream(writer);
		}
		int num = writer.Position - position;
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning("\t\t\t Serializing SequenceEndDataList:\n\t\t\t numBytes: " + num);
		}
	}
#endif

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

	//server-only
#if SERVER
	public static void SerializeServerAbilityResultsToStream(AbilityResults results, bool includeRunSequences, NetworkWriter writer)
	{
		sbyte b = (sbyte)results.Caster.ActorIndex;
		sbyte b2 = (sbyte)results.Caster.GetAbilityData().GetActionTypeOfAbility(results.Ability);
		List<ServerClientUtils.SequenceStartData> sequenceDataList;
		if (includeRunSequences)
		{
			sequenceDataList = results.AbilityRunSequenceDataList;
		}
		else
		{
			sequenceDataList = null;
		}
		writer.Write(b);
		writer.Write(b2);
		AbilityResultsUtils.SerializeSequenceStartDataListToStream(sequenceDataList, writer);
		AbilityResultsUtils.SerializeActorHitResultsDictionaryToStream(results.m_actorToHitResults, writer);
		AbilityResultsUtils.SerializePositionHitResultsDictionaryToStream(results.m_positionToHitResults, writer);
	}
#endif

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

	//server-only
#if SERVER
	public static void SerializeServerEffectResultsToStream(EffectResults results, bool includeHitSequences, NetworkWriter writer)
	{
		int position = writer.Position;
		uint effectGUID = (uint)Mathf.Max(0, results.Effect.m_guid);
		sbyte casterActorIndex = (sbyte)results.Effect.CasterActorIndex;

		// custom reactor
		// TODO check this
		AbilityData.ActionType abilityActionType = AbilityData.ActionType.INVALID_ACTION;
		Ability ability = results.Effect.Parent.Ability;
		if (ability != null && ability.GetInstanceID() != 0)  // powerups reference broken abilities
		{
			AbilityData abilityData = ability.GetComponent<AbilityData>();
			if (abilityData)
			{
				abilityActionType = abilityData.GetActionTypeOfAbility(ability);
			}
		}
		// rogues
		//bool isStandalone = results.IsStandalone;

		List<ServerClientUtils.SequenceStartData> list = null;
		if (includeHitSequences)
		{
			list = new List<ServerClientUtils.SequenceStartData>(results.m_sequenceStartData);
			foreach (ServerClientUtils.SequenceStartData sequenceStartData in results.Effect.GetEffectHitSeqDataList())
			{
				if (sequenceStartData != null)
				{
					sequenceStartData.SetRemoveAtEndOfTurn(true);
					list.Add(sequenceStartData);
				}
			}
		}
		else
        {
			list = null;
		}
		writer.WritePackedUInt32(effectGUID);
		writer.Write(casterActorIndex);
		// custom reactor
		writer.Write((sbyte)abilityActionType);
		// rogues
		//writer.Write(isStandalone);
		//if (isStandalone)
		//{
		//	AbilityResultsUtils.SerializeEffectsToStartToStream(new List<global::Effect>
		//	{
		//		results.Effect
		//	}, writer);
		//}
		AbilityResultsUtils.SerializeSequenceStartDataListToStream(list, writer);
		AbilityResultsUtils.SerializeActorHitResultsDictionaryToStream(results.m_actorToHitResults, writer);
		AbilityResultsUtils.SerializePositionHitResultsDictionaryToStream(results.m_positionToHitResults, writer);
		int num2 = writer.Position - position;
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning("\t\t\t Serializing Effect Results: \n\t\t\t numBytes: " + num2);
		}
	}
#endif

	public static ClientEffectResults DeSerializeClientEffectResultsFromStream(ref IBitStream stream)
	{
		uint effectGUID = 0u;
		sbyte casterActorIndex = (sbyte)ActorData.s_invalidActorIndex;
		stream.Serialize(ref effectGUID);
		stream.Serialize(ref casterActorIndex);

		// reactor
		sbyte sourceAbilityActionType = -1;
		stream.Serialize(ref sourceAbilityActionType);
		// rogues
		//bool isStandalone = reader.ReadBoolean();

		// rogues
		//ClientEffectResults clientEffectResults = null;
		//if (isStandalone)
		//{
		//	ClientEffectStartData clientEffectStartData = AbilityResultsUtils.DeSerializeEffectsToStartFromStream(reader).FirstOrDefault<ClientEffectStartData>();
		//	if (clientEffectStartData != null)
		//	{
		//		clientEffectResults = new ClientEffectResults(clientEffectStartData);
		//	}
		//	else
		//	{
		//		Debug.LogError("Server sent an empty effect start list");
		//		clientEffectResults = null;
		//	}
		//}

		List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = DeSerializePositionHitResultsDictionaryFromStream(ref stream);

		ActorData effectCaster = GameFlowData.Get().FindActorByActorIndex(casterActorIndex);
		
		// reactor
		return new ClientEffectResults(
			(int)effectGUID,
			effectCaster,
			(AbilityData.ActionType)sourceAbilityActionType,
			seqStartDataList,
			actorToHitResults,
			posToHitResults);
		// rogues
		//if (clientEffectResults == null)
		//{
		//	clientEffectResults = new ClientEffectResults((int)effectGUID, effectCaster, seqStartDataList, actorToHitResults, posToHitResults);
		//}
		//else
		//{
		//	clientEffectResults.m_actorToHitResults = actorToHitResults;
		//	clientEffectResults.m_posToHitResults = posToHitResults;
		//	clientEffectResults.m_seqStartDataList = seqStartDataList;
		//}
		//return clientEffectResults;
	}

	//server-only
#if SERVER
	public static void SerializeServerBarrierResultsToStream(BarrierResults results, NetworkWriter writer)
	{
		int guid = results.Barrier.m_guid;
		sbyte b = (sbyte)((results.Barrier.Caster != null) ? results.Barrier.Caster.ActorIndex : -1);
		writer.Write(guid);
		writer.Write(b);
		AbilityResultsUtils.SerializeActorHitResultsDictionaryToStream(results.m_actorToHitResults, writer);
		AbilityResultsUtils.SerializePositionHitResultsDictionaryToStream(results.m_positionToHitResults, writer);
	}
#endif

	public static ClientBarrierResults DeSerializeClientBarrierResultsFromStream(ref IBitStream stream)
	{
		int barrierGUID = -1;
		sbyte casterIndex = (sbyte)ActorData.s_invalidActorIndex;
		stream.Serialize(ref barrierGUID);
		stream.Serialize(ref casterIndex);
		Dictionary<ActorData, ClientActorHitResults> actorToHitResults = DeSerializeActorHitResultsDictionaryFromStream(ref stream);
		Dictionary<Vector3, ClientPositionHitResults> posToHitResults = DeSerializePositionHitResultsDictionaryFromStream(ref stream);
	#if VANILLA
		ActorData barrierCaster = GameFlowData.Get().FindActorByActorIndex(casterIndex);
	#else
		ActorData barrierCaster = casterIndex >= 0 ? GameFlowData.Get().FindActorByActorIndex(casterIndex) : null; // check added in rogues
	#endif
		return new ClientBarrierResults(barrierGUID, barrierCaster, actorToHitResults, posToHitResults);
	}

#if SERVER
	public static void SerializeServerMovementResultsToStream(MovementResults results, NetworkWriter writer)
	{
		int position = writer.Position;
		sbyte b = (sbyte)results.GetTriggeringActor().ActorIndex;
		List<ServerClientUtils.SequenceStartData> triggerSeqDataList = results.GetTriggerSeqDataList();
		if (results.m_triggeringPath_endingAtHit == null && results.m_triggeringPath != null)
		{
			results.m_triggeringPath_endingAtHit = MovementUtils.CreateClonePathEndingAt(results.m_triggeringPath);
		}
		BoardSquarePathInfo triggeringPath_endingAtHit = results.m_triggeringPath_endingAtHit;
		sbyte b2 = (sbyte)results.m_gameplayResponseType;
		writer.Write(b);
		MovementUtils.SerializeLightweightPath(triggeringPath_endingAtHit, writer);
		SerializeSequenceStartDataListToStream(triggerSeqDataList, writer);
		writer.Write(b2);
		if (results.m_gameplayResponseType == MovementResults_GameplayResponseType.Effect)
		{
			SerializeServerEffectResultsToStream(results.GetEffectResults(), false, writer);
		}
		else if (results.m_gameplayResponseType == MovementResults_GameplayResponseType.Barrier)
		{
			SerializeServerBarrierResultsToStream(results.GetBarrierResults(), writer);
		}
		else if (results.m_gameplayResponseType == MovementResults_GameplayResponseType.Powerup)
		{
			SerializeServerAbilityResultsToStream(results.GetPowerUpResults(), false, writer);
		}
		else if (results.m_gameplayResponseType == MovementResults_GameplayResponseType.GameMode)
		{
			SerializeServerAbilityResultsToStream(results.GetGameModeResults(), false, writer);
		}
		int num = writer.Position - position;
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning("\t\t Serialized Movement Results: \n\t\t numBytes: " + num);
		}
	}
#endif

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

	// server-only
#if SERVER
	public static void SerializeServerMovementResultsListToStream(List<MovementResults> movementResults, NetworkWriter writer)
	{
		sbyte b;
		if (movementResults == null)
		{
			b = 0;
		}
		else
		{
			b = (sbyte)movementResults.Count;
		}
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			SerializeServerMovementResultsToStream(movementResults[i], writer);
		}
	}
#endif

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

	// server-only
#if SERVER
	public static void SerializeServerReactionResultsToStream(List<AbilityResults_Reaction> reactionResultsList, NetworkWriter writer)
	{
		int position = writer.Position;
		sbyte b;
		if (reactionResultsList == null)
		{
			b = 0;
		}
		else
		{
			b = (sbyte)reactionResultsList.Count;
		}
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			AbilityResults_Reaction abilityResults_Reaction = reactionResultsList[i];
			List<ServerClientUtils.SequenceStartData> reactionSeqDataList = abilityResults_Reaction.GetReactionSeqDataList();
			EffectResults reactionResults = abilityResults_Reaction.GetReactionResults();
			AbilityResultsUtils.SerializeSequenceStartDataListToStream(reactionSeqDataList, writer);
			AbilityResultsUtils.SerializeServerEffectResultsToStream(reactionResults, false, writer);
			byte extraFlags = abilityResults_Reaction.GetExtraFlags();
			writer.Write(extraFlags);
		}
		int num = writer.Position - position;
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning("\t\t Serialized Reaction Results: \n\t\t numBytes: " + num);
		}
	}
#endif

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

	// server-only
#if SERVER
	public static void SerializePowerupsToRemoveToStream(List<ServerAbilityUtils.PowerupRemovalData> powerupsForRemoval, NetworkWriter writer)
	{
		sbyte b;
		if (powerupsForRemoval == null)
		{
			b = 0;
		}
		else
		{
			b = (sbyte)powerupsForRemoval.Count;
		}
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			int guid = powerupsForRemoval[i].m_powerupToRemove.Guid;
			writer.Write(guid);
		}
	}
#endif

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

	// server-only
#if SERVER
	public static void SerializePowerupsToStealToStream(List<ServerAbilityUtils.PowerUpStealData> powerupsForSteal, NetworkWriter writer)
	{
		sbyte b;
		if (powerupsForSteal == null)
		{
			b = 0;
		}
		else
		{
			b = (sbyte)powerupsForSteal.Count;
		}
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			int guid = powerupsForSteal[i].m_powerUp.Guid;
			AbilityResults_Powerup results = powerupsForSteal[i].m_results;
			writer.Write(guid);
			AbilityResultsUtils.SerializeServerPowerupResultsToStream(results, writer);
		}
	}
#endif

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

	// server-only
#if SERVER
	public static void SerializeServerPowerupResultsToStream(AbilityResults_Powerup powerupResults, NetworkWriter writer)
	{
		List<ServerClientUtils.SequenceStartData> sequenceDataList = powerupResults.BuildAbilityRunSequenceDataList();
		AbilityResults abilityResults = powerupResults.AbilityResults;
		AbilityResultsUtils.SerializeSequenceStartDataListToStream(sequenceDataList, writer);
		AbilityResultsUtils.SerializeServerAbilityResultsToStream(abilityResults, true, writer);
	}
#endif

	public static ClientPowerupResults DeSerializeClientPowerupResultsFromStream(ref IBitStream stream)
	{
		List<ServerClientUtils.SequenceStartData> seqStartDataList = DeSerializeSequenceStartDataListFromStream(ref stream);
		ClientAbilityResults clientAbilityResults = DeSerializeClientAbilityResultsFromStream(ref stream);
		return new ClientPowerupResults(seqStartDataList, clientAbilityResults);
	}

	// server-only
#if SERVER
	public static void SerializeServerGameModeEventListToStream(List<GameModeEvent> gameModeEvents, NetworkWriter writer)
	{
		sbyte b;
		if (gameModeEvents == null)
		{
			b = 0;
		}
		else
		{
			b = (sbyte)gameModeEvents.Count;
		}
		writer.Write(b);
		for (int i = 0; i < (int)b; i++)
		{
			AbilityResultsUtils.SerializeServerGameModeEventToStream(gameModeEvents[i], writer);
		}
	}
#endif

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

	// server-only
#if SERVER
	public static void SerializeServerGameModeEventToStream(GameModeEvent gameModeEvent, NetworkWriter writer)
	{
		sbyte b = (sbyte)gameModeEvent.m_eventType;
		byte objectGuid = gameModeEvent.m_objectGuid;
		sbyte b2;
		if (gameModeEvent.m_primaryActor == null)
		{
			b2 = (sbyte)ActorData.s_invalidActorIndex;
		}
		else
		{
			b2 = (sbyte)gameModeEvent.m_primaryActor.ActorIndex;
		}
		sbyte b3;
		if (gameModeEvent.m_secondaryActor == null)
		{
			b3 = (sbyte)ActorData.s_invalidActorIndex;
		}
		else
		{
			b3 = (sbyte)gameModeEvent.m_secondaryActor.ActorIndex;
		}
		sbyte b4;
		sbyte b5;
		if (gameModeEvent.m_square == null)
		{
			b4 = -1;
			b5 = -1;
		}
		else
		{
			b4 = (sbyte)gameModeEvent.m_square.x;
			b5 = (sbyte)gameModeEvent.m_square.y;
		}
		int eventGuid = gameModeEvent.m_eventGuid;
		writer.Write(b);
		writer.Write(objectGuid);
		writer.Write(b2);
		writer.Write(b3);
		writer.Write(b4);
		writer.Write(b5);
		writer.Write(eventGuid);
	}
#endif

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
		BoardSquare square = (x == -1 && y == -1) ? null : Board.Get().GetSquareFromIndex(x, y);
        ActorData primaryActor = primaryActorIndex != ActorData.s_invalidActorIndex ? GameFlowData.Get().FindActorByActorIndex(primaryActorIndex) : null;
        ActorData secondaryActor = secondaryActorIndex != ActorData.s_invalidActorIndex ? GameFlowData.Get().FindActorByActorIndex(secondaryActorIndex) : null;
        return new ClientGameModeEvent(eventType, objectGUID, square, primaryActor, secondaryActor, eventGUID);
	}

	// server-only
#if SERVER
	public static void SerializeServerOverconListToStream(List<int> overconIds, NetworkWriter writer)
	{
		sbyte b;
		if (overconIds == null)
		{
			b = 0;
		}
		else
		{
			b = (sbyte)overconIds.Count;
		}
		writer.Write(b);
		for (int i = 0; i < overconIds.Count; i++)
		{
			int num = overconIds[i];
			writer.Write(num);
		}
	}
#endif

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
}
