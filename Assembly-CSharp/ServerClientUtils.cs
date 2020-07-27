using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ServerClientUtils
{
	public class SequenceStartData
	{
		private short m_prefabID;
		private GameObject m_serverOnlyPrefabReference;
		private bool m_useTargetPos;
		private Vector3 m_targetPos;
		private bool m_useTargetSquare;
		private int m_targetSquareX;
		private int m_targetSquareY;
		private bool m_useTargetRotation;
		private Quaternion m_targetRotation;
		private byte m_numTargetActors;
		private int[] m_targetActorIndices;
		private int m_casterActorIndex;
		private byte m_numExtraParams;
		private Sequence.IExtraSequenceParams[] m_extraParams;
		private uint m_sourceRootID;
		private bool m_sourceRemoveAtEndOfTurn;
		private bool m_waitForClientEnable;

		public string Json()
		{
			string targetActors = "";
			for (int i = 0; i < m_numTargetActors; ++i)
			{
				targetActors += (targetActors.Length == 0 ? "" : ", ") + $"\"{GameFlowData.Get().FindActorByActorIndex(m_targetActorIndices[i])?.DisplayName ?? "none"}\"";
			}
			string extraParams = "";
			for (int i = 0; i < m_numExtraParams; ++i)
			{
				extraParams += (extraParams.Length == 0 ? "" : ", ") + m_extraParams[i].Json();
			}
			return $"{{" +
				$"\"prefabId\": {m_prefabID}, " +
				$"\"useTargetPos\": \"{m_useTargetPos}\", " +
				$"\"targetPos\": [{m_targetPos.x}, {m_targetPos.y}, {m_targetPos.z}], " +
				$"\"useTargetSquare\": \"{m_useTargetSquare}\", " +
				$"\"targetSquare\": [{m_targetSquareX}, {m_targetSquareY}], " +
				$"\"useTargetRotation\": \"{m_useTargetRotation}\", " +
				$"\"targetRotation\": \"{m_targetRotation}\", " +
				$"\"targetActors\": [{targetActors}], " +
				$"\"caster\": \"{GameFlowData.Get().FindActorByActorIndex(m_casterActorIndex)?.DisplayName ?? "none"}\", " +
				$"\"extraParams\": {extraParams}, " +
				$"\"sourceRootID\": {m_sourceRootID}, " +
				$"\"sourceRemoveAtEndOfTurn\": {m_sourceRemoveAtEndOfTurn}, " +
				$"\"waitForClientEnable\": {m_waitForClientEnable}" +
				$"}}";
		}

		public SequenceStartData(GameObject prefab, BoardSquare targetSquare, ActorData[] targetActorArray, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams = null)
		{
			InitToDefaults();
			InitPrefab(prefab);
			InitSquare(targetSquare);
			InitTargetActors(targetActorArray);
			InitCasterActor(caster);
			InitSequenceSourceData(source);
			InitExtraParams(extraParams);
		}

		public SequenceStartData(GameObject prefab, BoardSquare targetSquare, Quaternion targetRotation, ActorData[] targetActorArray, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams = null)
		{
			InitToDefaults();
			InitPrefab(prefab);
			InitSquare(targetSquare);
			InitRotation(targetRotation);
			InitTargetActors(targetActorArray);
			InitCasterActor(caster);
			InitSequenceSourceData(source);
			InitExtraParams(extraParams);
		}

		public SequenceStartData(GameObject prefab, Vector3 targetPos, ActorData[] targetActorArray, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams = null)
		{
			InitToDefaults();
			InitPrefab(prefab);
			InitPos(targetPos);
			InitTargetActors(targetActorArray);
			InitCasterActor(caster);
			InitSequenceSourceData(source);
			InitExtraParams(extraParams);
		}

		public SequenceStartData(GameObject prefab, Vector3 targetPos, Quaternion targetRotation, ActorData[] targetActorArray, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams = null)
		{
			InitToDefaults();
			InitPrefab(prefab);
			InitPos(targetPos);
			InitRotation(targetRotation);
			InitTargetActors(targetActorArray);
			InitCasterActor(caster);
			InitSequenceSourceData(source);
			InitExtraParams(extraParams);
		}

		public SequenceStartData()
		{
			InitToDefaults();
		}

		public void SetRemoveAtEndOfTurn(bool val)
		{
			m_sourceRemoveAtEndOfTurn = val;
		}

		public void SetTargetPos(Vector3 pos)
		{
			InitPos(pos);
		}

		public Vector3 GetTargetPos()
		{
			return m_targetPos;
		}

		public int GetCasterActorIndex()
		{
			return m_casterActorIndex;
		}

		public Sequence.IExtraSequenceParams[] GetExtraParams()
		{
			return m_extraParams;
		}

		private void InitToDefaults()
		{
			m_prefabID = -1;
			m_serverOnlyPrefabReference = null;
			m_useTargetPos = false;
			m_targetPos = Vector3.zero;
			m_useTargetSquare = false;
			m_targetSquareX = 0;
			m_targetSquareY = 0;
			m_useTargetRotation = false;
			m_targetRotation = Quaternion.identity;
			m_numTargetActors = 0;
			m_targetActorIndices = null;
			m_casterActorIndex = ActorData.s_invalidActorIndex;
			m_numExtraParams = 0;
			m_extraParams = null;
			m_sourceRootID = 0u;
			m_sourceRemoveAtEndOfTurn = true;
			m_waitForClientEnable = false;
		}

		private void InitPrefab(GameObject prefab)
		{
			m_prefabID = SequenceLookup.Get().GetSequenceIdOfPrefab(prefab);
			m_serverOnlyPrefabReference = prefab;
		}

		private void InitPos(Vector3 targetPos)
		{
			m_useTargetPos = true;
			m_targetPos = targetPos;
		}

		private void InitSquare(BoardSquare square)
		{
			if (square != null)
			{
				m_useTargetSquare = true;
				m_targetSquareX = square.x;
				m_targetSquareY = square.y;
				if (!m_useTargetPos)
				{
					m_targetPos = square.ToVector3();
				}
			}
		}

		private void InitRotation(Quaternion targetRotation)
		{
			m_useTargetRotation = true;
			m_targetRotation = targetRotation;
		}

		public void InitTargetActors(ActorData[] targetActorArray)
		{
			if (targetActorArray != null && targetActorArray.Length > 0)
			{
				m_numTargetActors = (byte)targetActorArray.Length;
				m_targetActorIndices = new int[m_numTargetActors];
				for (int i = 0; i < m_numTargetActors; i++)
				{
					m_targetActorIndices[i] = targetActorArray[i].ActorIndex;
				}
			}
		}

		public List<int> GetTargetActorIndices()
		{
			if (m_targetActorIndices != null)
			{
				return new List<int>(m_targetActorIndices);
			}
			return new List<int>();
		}

		private void InitCasterActor(ActorData caster)
		{
			if (caster == null)
			{
				Log.Error("SequenceStartData trying to init its caster actor, but that actor is null.");
				return;
			}

			m_casterActorIndex = caster.ActorIndex;
		}

		internal void InitSequenceSourceData(SequenceSource source)
		{
			if (source != null)
			{
				m_sourceRootID = source.RootID;
				m_sourceRemoveAtEndOfTurn = source.RemoveAtEndOfTurn;
				m_waitForClientEnable = source.WaitForClientEnable;
			}
		}

		public void InitExtraParams(Sequence.IExtraSequenceParams[] extraParams)
		{
			if (extraParams != null && extraParams.Length > 0)
			{
				m_extraParams = extraParams;
				m_numExtraParams = (byte)extraParams.Length;
			}
		}

		public short GetSequencePrefabId()
		{
			return m_prefabID;
		}

		public GameObject GetServerOnlyPrefabReference()
		{
			return m_serverOnlyPrefabReference;
		}

		public string GetTargetActorsString()
		{
			string text = "";
			if (m_targetActorIndices != null && m_targetActorIndices.Length > 0)
			{
				for (int i = 0; i < m_targetActorIndices.Length; i++)
				{
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_targetActorIndices[i]);
					if (actorData != null)
					{
						text = text + " | " + actorData.DebugNameString();
					}
					else
					{
						text = text + " | (Unknown Actor) " + m_targetActorIndices[i];
					}
				}
			}
			else
			{
				text = "(Empty)";
			}
			return text;
		}

		public void SequenceStartData_SerializeToStream(ref IBitStream stream)
		{
			uint position = stream.Position;
			byte bitfield = CreateBitfieldFromBools(m_useTargetPos, m_useTargetSquare, m_useTargetRotation, m_sourceRemoveAtEndOfTurn, m_waitForClientEnable, false, false, false);
			stream.Serialize(ref m_prefabID);
			stream.Serialize(ref bitfield);
			if (m_useTargetPos)
			{
				stream.Serialize(ref m_targetPos);
			}
			if (m_useTargetSquare)
			{
				byte targetSquareX = (byte)m_targetSquareX;
				byte targetSquareY = (byte)m_targetSquareY;
				stream.Serialize(ref targetSquareX);
				stream.Serialize(ref targetSquareY);
			}
			if (m_useTargetRotation)
			{
				Vector3 dir = m_targetRotation * new Vector3(1f, 0f, 0f);
				float angle = VectorUtils.HorizontalAngle_Deg(dir);
				stream.Serialize(ref angle);
			}
			stream.Serialize(ref m_numTargetActors);
			for (byte b = 0; b < m_numTargetActors; b = (byte)(b + 1))
			{
				sbyte actorIndex = (sbyte)m_targetActorIndices[b];
				stream.Serialize(ref actorIndex);
			}
			sbyte casterActorIndex = (sbyte)m_casterActorIndex;
			stream.Serialize(ref casterActorIndex);
			stream.Serialize(ref m_sourceRootID);
			stream.Serialize(ref m_numExtraParams);
			for (int i = 0; i < m_numExtraParams; i++)
			{
				short extraParam = (short)SequenceLookup.GetEnumOfExtraParam(m_extraParams[i]);
				stream.Serialize(ref extraParam);
				m_extraParams[i].XSP_SerializeToStream(stream);
			}
			uint numBytes = stream.Position - position;
			if (ClientAbilityResults.DebugSerializeSizeOn)
			{
				Debug.LogWarning("\t\t\t\t\t Serializing Sequence Start Data, using targetPos? " + m_useTargetPos.ToString() + " prefab id " + m_prefabID + ": \n\t\t\t\t\t numBytes: " + numBytes);
			}
		}

		public static SequenceStartData SequenceStartData_DeserializeFromStream(ref IBitStream stream)
		{
			short prefabId = -1;
			byte bitField = 0;
			bool useTargetPos = false;
			Vector3 targetPos = Vector3.zero;
			bool useTargetSquare = false;
			byte targetSquareX = 0;
			byte targetSquareY = 0;
			bool useTargetRotation = false;
			Quaternion targetRotation = Quaternion.identity;
			byte numTargetActors = 0;
			List<int> targetActorIndices = new List<int>();
			sbyte casterActorIndex = 0;
			uint sourceRootId = 0u;
			bool sourceRemoveAtEndOfTurn = true;
			bool waitForClientEnable = false;
			byte numExtraParams = 0;
			List<Sequence.IExtraSequenceParams> extraParams = new List<Sequence.IExtraSequenceParams>();
			stream.Serialize(ref prefabId);
			stream.Serialize(ref bitField);
			GetBoolsFromBitfield(bitField, out useTargetPos, out useTargetSquare, out useTargetRotation, out sourceRemoveAtEndOfTurn, out waitForClientEnable);
			if (useTargetPos)
			{
				stream.Serialize(ref targetPos);
			}
			if (useTargetSquare)
			{
				stream.Serialize(ref targetSquareX);
				stream.Serialize(ref targetSquareY);
			}
			if (useTargetRotation)
			{
				float angleDegrees = 0f;
				stream.Serialize(ref angleDegrees);
				Vector3 toDirection = VectorUtils.AngleDegreesToVector(angleDegrees);
				targetRotation = Quaternion.FromToRotation(new Vector3(1f, 0f, 0f), toDirection);
			}
			stream.Serialize(ref numTargetActors);
			for (int i = 0; i < numTargetActors; i++)
			{
				sbyte targetActorIndex = (sbyte)ActorData.s_invalidActorIndex;
				stream.Serialize(ref targetActorIndex);
				targetActorIndices.Add(targetActorIndex);
			}
			stream.Serialize(ref casterActorIndex);
			stream.Serialize(ref sourceRootId);
			stream.Serialize(ref numExtraParams);
			for (int j = 0; j < numExtraParams; j++)
			{
				short extraParam = 0;
				stream.Serialize(ref extraParam);
				Sequence.IExtraSequenceParams extraSequenceParams = SequenceLookup.Get().CreateExtraParamOfEnum((SequenceLookup.SequenceExtraParamEnum)extraParam);
				extraSequenceParams.XSP_DeserializeFromStream(stream);
				extraParams.Add(extraSequenceParams);
			}
			SequenceStartData sequenceStartData = new SequenceStartData
			{
				m_prefabID = prefabId,
				m_useTargetPos = useTargetPos,
				m_targetPos = targetPos,
				m_useTargetSquare = useTargetSquare,
				m_targetSquareX = useTargetSquare ? targetSquareX : -1,
				m_targetSquareY = useTargetSquare ? targetSquareY : -1,
				m_useTargetRotation = useTargetRotation,
				m_targetRotation = targetRotation,
				m_numTargetActors = numTargetActors,
				m_targetActorIndices = targetActorIndices.ToArray(),
				m_casterActorIndex = casterActorIndex,
				m_sourceRootID = sourceRootId,
				m_sourceRemoveAtEndOfTurn = sourceRemoveAtEndOfTurn,
				m_waitForClientEnable = waitForClientEnable,
				m_numExtraParams = numExtraParams,
				m_extraParams = extraParams.ToArray()
			};
			return sequenceStartData;
		}

		internal Sequence[] CreateSequencesFromData(SequenceSource.ActorDelegate onHitActor, SequenceSource.Vector3Delegate onHitPos)
		{
			GameObject prefabOfSequenceId = SequenceLookup.Get().GetPrefabOfSequenceId(m_prefabID);
			BoardSquare targetSquare = m_useTargetSquare
				? Board.Get().GetSquareFromIndex(m_targetSquareX, m_targetSquareY)
				: null;
			ActorData[] targetActors = new ActorData[m_numTargetActors];
			for (int i = 0; i < m_numTargetActors; i++)
			{
				targetActors[i] = GameFlowData.Get().FindActorByActorIndex(m_targetActorIndices[i]);
			}
			ActorData caster = GameFlowData.Get().FindActorByActorIndex(m_casterActorIndex);
			SequenceSource sequenceSource = new SequenceSource(onHitActor, onHitPos, m_sourceRootID, m_sourceRemoveAtEndOfTurn);
			sequenceSource.SetWaitForClientEnable(m_waitForClientEnable);
			if (m_useTargetRotation)
			{
				if (m_useTargetSquare)
				{
					return SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, targetSquare, m_targetPos, m_targetRotation, targetActors, caster, sequenceSource, m_extraParams);
				}
				else
				{
					return SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, m_targetPos, m_targetRotation, targetActors, caster, sequenceSource, m_extraParams);
				}
			}
			else if (m_useTargetSquare)
			{
				return SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, targetSquare, targetActors, caster, sequenceSource, m_extraParams);
			}
			else
			{
				return SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, m_targetPos, targetActors, caster, sequenceSource, m_extraParams);
			}
		}

		internal bool HasSequencePrefab()
		{
			GameObject prefab = SequenceLookup.Get().GetPrefabOfSequenceId(m_prefabID);
			if (prefab == null && SequenceLookup.Get() != null)
			{
				prefab = SequenceLookup.Get().GetSimpleHitSequencePrefab();
			}
			return prefab != null;
		}

		internal bool Contains(SequenceSource sequenceSource)
		{
			return sequenceSource != null && m_sourceRootID == sequenceSource.RootID;
		}

		internal bool ContainsSequenceSourceID(uint id)
		{
			return m_sourceRootID == id;
		}
	}

	public class SequenceEndData
	{
		public enum AssociationType
		{
			EffectGuid,
			BarrierGuid,
			SequenceSourceId
		}

		private short m_prefabId;
		private uint m_association;
		private AssociationType m_associationType;
		private Vector3 m_targetPos;

		public SequenceEndData(int prefabIdToEnd, AssociationType associationType, int guid, Vector3 targetPos)
		{
			m_prefabId = (short)prefabIdToEnd;
			m_associationType = associationType;
			m_association = (uint)Mathf.Max(0, guid);
			m_targetPos = targetPos;
		}

		public SequenceEndData(GameObject prefabToEnd, AssociationType associationType, int guid, Vector3 targetPos)
		{
			m_prefabId = SequenceLookup.Get().GetSequenceIdOfPrefab(prefabToEnd);
			m_associationType = associationType;
			m_association = (uint)Mathf.Max(0, guid);
			m_targetPos = targetPos;
		}

		public void SequenceEndData_SerializeToStream(ref IBitStream stream)
		{
			sbyte associationType = (sbyte)m_associationType;
			stream.Serialize(ref m_prefabId);
			stream.Serialize(ref associationType);
			stream.Serialize(ref m_association);
			bool hasTargetPos = m_targetPos != Vector3.zero;
			stream.Serialize(ref hasTargetPos);
			if (hasTargetPos)
			{
				stream.Serialize(ref m_targetPos);
			}
		}

		public static SequenceEndData SequenceEndData_DeserializeFromStream(ref IBitStream stream)
		{
			short prefabId = -1;
			uint guid = 0u;
			sbyte associationType = -1;
			bool hasTargetPos = false;
			Vector3 targetPos = Vector3.zero;
			stream.Serialize(ref prefabId);
			stream.Serialize(ref associationType);
			stream.Serialize(ref guid);
			stream.Serialize(ref hasTargetPos);
			if (hasTargetPos)
			{
				stream.Serialize(ref targetPos);
			}
			return new SequenceEndData(prefabId, (AssociationType)associationType, (int)guid, targetPos);
		}

		public void EndClientSequences()
		{
			switch (m_associationType)
			{
				case AssociationType.EffectGuid:
					ClientEffectBarrierManager.Get().EndSequenceOfEffect(m_prefabId, (int)m_association, m_targetPos);
					break;
				case AssociationType.BarrierGuid:
					ClientEffectBarrierManager.Get().EndSequenceOfBarrier(m_prefabId, (int)m_association, m_targetPos);
					break;
				case AssociationType.SequenceSourceId:
					SequenceManager.Get().MarkSequenceToEndBySourceId(m_prefabId, (int)m_association, m_targetPos);
					break;
			}
		}
	}

	public const bool c_sendClientCastActions = false;

	public static ActionBufferPhase GetCurrentActionPhase()
	{
		if (!NetworkServer.active)
		{
			if (ClientActionBuffer.Get() != null)
			{
				return ClientActionBuffer.Get().CurrentActionPhase;
			}
			if (GameManager.Get() != null && GameManager.Get().GameStatus == GameStatus.Started)
			{
				Log.Error("Trying to examine current action phase, but ClientActionBuffer does not exist.");
			}
		}
		return ActionBufferPhase.Done;
	}

	public static AbilityPriority GetCurrentAbilityPhase()
	{
		if (!NetworkServer.active)
		{
			if (ClientActionBuffer.Get() != null)
			{
				return ClientActionBuffer.Get().AbilityPhase;
			}
			else
			{
				Log.Error("Trying to examine current ability phase, but ClientActionBuffer does not exist.");
			}
		}
		return AbilityPriority.INVALID;
	}

	public static byte CreateBitfieldFromBoolsList(List<bool> bools)
	{
		byte bitfield = 0;
		int num = Mathf.Min(bools.Count, 8);
		for (int i = 0; i < num; i++)
		{
			if (bools[i])
			{
				bitfield |= (byte)(1 << i);
			}
		}
		return bitfield;
	}

	public static short CreateBitfieldFromBoolsList_16bit(List<bool> bools)
	{
		short bitfield = 0;
		int num = Mathf.Min(bools.Count, 16);
		for (int i = 0; i < num; i++)
		{
			if (bools[i])
			{
				bitfield |= (short)(1 << i);
			}
		}
		return bitfield;
	}

	public static int CreateBitfieldFromBoolsList_32bit(List<bool> bools)
	{
		int bitfield = 0;
		int num = Mathf.Min(bools.Count, 32);
		for (int i = 0; i < num; i++)
		{
			if (bools[i])
			{
				bitfield |= 1 << i;
			}
		}
		return bitfield;
	}

	public static byte CreateBitfieldFromBools(bool b0, bool b1, bool b2, bool b3, bool b4, bool b5, bool b6, bool b7)
	{
		byte bitfield = 0;
		if (b0)
		{
			bitfield |= 0x1;
		}
		if (b1)
		{
			bitfield |= 0x2;
		}
		if (b2)
		{
			bitfield |= 0x4;
		}
		if (b3)
		{
			bitfield |= 0x8;
		}
		if (b4)
		{
			bitfield |= 0x10;
		}
		if (b5)
		{
			bitfield |= 0x20;
		}
		if (b6)
		{
			bitfield |= 0x40;
		}
		if (b7)
		{
			bitfield |= 0x80;
		}
		return bitfield;
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3, out bool out4, out bool out5, out bool out6, out bool out7)
	{
		out0 = ((bitField & 0x1) != 0);
		out1 = ((bitField & 0x2) != 0);
		out2 = ((bitField & 0x4) != 0);
		out3 = ((bitField & 0x8) != 0);
		out4 = ((bitField & 0x10) != 0);
		out5 = ((bitField & 0x20) != 0);
		out6 = ((bitField & 0x40) != 0);
		out7 = ((bitField & 0x80) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3, out bool out4, out bool out5, out bool out6)
	{
		out0 = ((bitField & 0x1) != 0);
		out1 = ((bitField & 0x2) != 0);
		out2 = ((bitField & 0x4) != 0);
		out3 = ((bitField & 0x8) != 0);
		out4 = ((bitField & 0x10) != 0);
		out5 = ((bitField & 0x20) != 0);
		out6 = ((bitField & 0x40) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3, out bool out4, out bool out5)
	{
		out0 = ((bitField & 0x1) != 0);
		out1 = ((bitField & 0x2) != 0);
		out2 = ((bitField & 0x4) != 0);
		out3 = ((bitField & 0x8) != 0);
		out4 = ((bitField & 0x10) != 0);
		out5 = ((bitField & 0x20) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3, out bool out4)
	{
		out0 = ((bitField & 0x1) != 0);
		out1 = ((bitField & 0x2) != 0);
		out2 = ((bitField & 0x4) != 0);
		out3 = ((bitField & 0x8) != 0);
		out4 = ((bitField & 0x10) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3)
	{
		out0 = ((bitField & 0x1) != 0);
		out1 = ((bitField & 0x2) != 0);
		out2 = ((bitField & 0x4) != 0);
		out3 = ((bitField & 0x8) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2)
	{
		out0 = ((bitField & 0x1) != 0);
		out1 = ((bitField & 0x2) != 0);
		out2 = ((bitField & 0x4) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1)
	{
		out0 = ((bitField & 0x1) != 0);
		out1 = ((bitField & 0x2) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0)
	{
		out0 = ((bitField & 0x1) != 0);
	}
}
