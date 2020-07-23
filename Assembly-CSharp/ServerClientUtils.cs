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
			if (!(square != null))
			{
				return;
			}
			while (true)
			{
				m_useTargetSquare = true;
				m_targetSquareX = square.x;
				m_targetSquareY = square.y;
				if (!m_useTargetPos)
				{
					while (true)
					{
						m_targetPos = square.ToVector3();
						return;
					}
				}
				return;
			}
		}

		private void InitRotation(Quaternion targetRotation)
		{
			m_useTargetRotation = true;
			m_targetRotation = targetRotation;
		}

		public void InitTargetActors(ActorData[] targetActorArray)
		{
			if (targetActorArray == null)
			{
				return;
			}
			while (true)
			{
				if (targetActorArray.Length <= 0)
				{
					return;
				}
				while (true)
				{
					m_numTargetActors = (byte)targetActorArray.Length;
					m_targetActorIndices = new int[m_numTargetActors];
					for (int i = 0; i < m_numTargetActors; i++)
					{
						m_targetActorIndices[i] = targetActorArray[i].ActorIndex;
					}
					return;
				}
			}
		}

		public List<int> GetTargetActorIndices()
		{
			if (m_targetActorIndices != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return new List<int>(m_targetActorIndices);
					}
				}
			}
			return new List<int>();
		}

		private void InitCasterActor(ActorData caster)
		{
			if (caster != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_casterActorIndex = caster.ActorIndex;
						return;
					}
				}
			}
			Log.Error("SequenceStartData trying to init its caster actor, but that actor is null.");
		}

		internal void InitSequenceSourceData(SequenceSource source)
		{
			if (!(source != null))
			{
				return;
			}
			while (true)
			{
				m_sourceRootID = source.RootID;
				m_sourceRemoveAtEndOfTurn = source.RemoveAtEndOfTurn;
				m_waitForClientEnable = source.WaitForClientEnable;
				return;
			}
		}

		public void InitExtraParams(Sequence.IExtraSequenceParams[] extraParams)
		{
			if (extraParams == null)
			{
				return;
			}
			while (true)
			{
				if (extraParams.Length > 0)
				{
					while (true)
					{
						m_extraParams = extraParams;
						m_numExtraParams = (byte)extraParams.Length;
						return;
					}
				}
				return;
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
			string text = string.Empty;
			if (m_targetActorIndices != null && m_targetActorIndices.Length > 0)
			{
				for (int i = 0; i < m_targetActorIndices.Length; i++)
				{
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_targetActorIndices[i]);
					if (actorData != null)
					{
						text = text + " | " + actorData.GetDebugName();
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
			byte value = CreateBitfieldFromBools(m_useTargetPos, m_useTargetSquare, m_useTargetRotation, m_sourceRemoveAtEndOfTurn, m_waitForClientEnable, false, false, false);
			stream.Serialize(ref m_prefabID);
			stream.Serialize(ref value);
			if (m_useTargetPos)
			{
				stream.Serialize(ref m_targetPos);
			}
			if (m_useTargetSquare)
			{
				byte value2 = (byte)m_targetSquareX;
				byte value3 = (byte)m_targetSquareY;
				stream.Serialize(ref value2);
				stream.Serialize(ref value3);
			}
			if (m_useTargetRotation)
			{
				Vector3 vec = m_targetRotation * new Vector3(1f, 0f, 0f);
				float value4 = VectorUtils.HorizontalAngle_Deg(vec);
				stream.Serialize(ref value4);
			}
			stream.Serialize(ref m_numTargetActors);
			for (byte b = 0; b < m_numTargetActors; b = (byte)(b + 1))
			{
				sbyte value5 = (sbyte)m_targetActorIndices[b];
				stream.Serialize(ref value5);
			}
			while (true)
			{
				sbyte value6 = (sbyte)m_casterActorIndex;
				stream.Serialize(ref value6);
				stream.Serialize(ref m_sourceRootID);
				stream.Serialize(ref m_numExtraParams);
				for (int i = 0; i < m_numExtraParams; i++)
				{
					Sequence.IExtraSequenceParams extraSequenceParams = m_extraParams[i];
					SequenceLookup.SequenceExtraParamEnum enumOfExtraParam = SequenceLookup.GetEnumOfExtraParam(extraSequenceParams);
					short value7 = (short)enumOfExtraParam;
					stream.Serialize(ref value7);
					extraSequenceParams.XSP_SerializeToStream(stream);
				}
				uint num = stream.Position - position;
				if (ClientAbilityResults._000E)
				{
					Debug.LogWarning("\t\t\t\t\t Serializing Sequence Start Data, using targetPos? " + m_useTargetPos.ToString() + " prefab id " + m_prefabID + ": \n\t\t\t\t\t numBytes: " + num);
				}
				return;
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
			GetBoolsFromBitfield(
				bitField,
				out useTargetPos,
				out useTargetSquare,
				out useTargetRotation,
				out sourceRemoveAtEndOfTurn,
				out waitForClientEnable);
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
				SequenceLookup.SequenceExtraParamEnum paramEnum = (SequenceLookup.SequenceExtraParamEnum)extraParam;
				Sequence.IExtraSequenceParams extraSequenceParams = SequenceLookup.Get().CreateExtraParamOfEnum(paramEnum);
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

		public static void Serialize(ref IBitStream stream, ref SequenceStartData sequenceStartData)
		{
			short prefabId = sequenceStartData.m_prefabID;
			bool useTargetPos = sequenceStartData.m_useTargetPos;
			Vector3 targetPos = sequenceStartData.m_targetPos;
			bool useTargetSquare = sequenceStartData.m_useTargetSquare;
			byte targetSquareX = (byte)sequenceStartData.m_targetSquareX;
			byte targetSquareY = (byte)sequenceStartData.m_targetSquareY;
			bool useTargetRotation = sequenceStartData.m_useTargetRotation;
			Quaternion targetRotation = sequenceStartData.m_targetRotation;
			byte numTargetActors = sequenceStartData.m_numTargetActors;
			sbyte casterActorIndex = (sbyte)sequenceStartData.m_casterActorIndex;
			uint sourceRootId = sequenceStartData.m_sourceRootID;
			bool sourceRemoveAtEndOfTurn = sequenceStartData.m_sourceRemoveAtEndOfTurn;
			bool waitForClientEnable = sequenceStartData.m_waitForClientEnable;
			byte numExtraParams = sequenceStartData.m_numExtraParams;

			List<int> targetActorIndices = new List<int>();
			List<Sequence.IExtraSequenceParams> extraParams = new List<Sequence.IExtraSequenceParams>();

			stream.Serialize(ref prefabId);

			byte bitField = CreateBitfieldFromBools(
				useTargetPos,
				useTargetSquare,
				useTargetRotation,
				sourceRemoveAtEndOfTurn,
				waitForClientEnable,
				false, false, false);
			stream.Serialize(ref bitField);
			GetBoolsFromBitfield(
				bitField,
				out useTargetPos,
				out useTargetSquare,
				out useTargetRotation,
				out sourceRemoveAtEndOfTurn,
				out waitForClientEnable);
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
				float angleDegrees = VectorUtils.HorizontalAngle_Deg(targetRotation * new Vector3(1f, 0f, 0f)); // TODO recheck
				stream.Serialize(ref angleDegrees);
				targetRotation = Quaternion.FromToRotation(new Vector3(1f, 0f, 0f), VectorUtils.AngleDegreesToVector(angleDegrees));
			}
			stream.Serialize(ref numTargetActors);
			for (int i = 0; i < numTargetActors; i++)
			{
				sbyte targetActorIndex = (sbyte)sequenceStartData.m_targetActorIndices[i];
				stream.Serialize(ref targetActorIndex);
				targetActorIndices.Add(targetActorIndex);
			}
			stream.Serialize(ref casterActorIndex);
			stream.Serialize(ref sourceRootId);
			stream.Serialize(ref numExtraParams);
			for (int j = 0; j < numExtraParams; j++)
			{
				Sequence.IExtraSequenceParams extraSequenceParams = sequenceStartData.m_extraParams[j];
				short extraParamType = (short)SequenceLookup.GetEnumOfExtraParam(extraSequenceParams);
				stream.Serialize(ref extraParamType);

				if (stream.isWriting)
				{
					extraSequenceParams.XSP_SerializeToStream(stream);
				}
				else
				{
					SequenceLookup.SequenceExtraParamEnum paramEnum = (SequenceLookup.SequenceExtraParamEnum)extraParamType;
					extraSequenceParams = SequenceLookup.Get().CreateExtraParamOfEnum(paramEnum);
					extraSequenceParams.XSP_DeserializeFromStream(stream);
				}
				extraParams.Add(extraSequenceParams);
			}

			if (stream.isReading)
			{
				sequenceStartData.m_prefabID = prefabId;
				sequenceStartData.m_useTargetPos = useTargetPos;
				sequenceStartData.m_targetPos = targetPos;
				sequenceStartData.m_useTargetSquare = useTargetSquare;
				sequenceStartData.m_targetSquareX = useTargetSquare ? targetSquareX : -1;
				sequenceStartData.m_targetSquareY = useTargetSquare ? targetSquareY : -1;
				sequenceStartData.m_useTargetRotation = useTargetRotation;
				sequenceStartData.m_targetRotation = targetRotation;
				sequenceStartData.m_numTargetActors = numTargetActors;
				sequenceStartData.m_targetActorIndices = targetActorIndices.ToArray();
				sequenceStartData.m_casterActorIndex = casterActorIndex;
				sequenceStartData.m_sourceRootID = sourceRootId;
				sequenceStartData.m_sourceRemoveAtEndOfTurn = sourceRemoveAtEndOfTurn;
				sequenceStartData.m_waitForClientEnable = waitForClientEnable;
				sequenceStartData.m_numExtraParams = numExtraParams;
				sequenceStartData.m_extraParams = extraParams.ToArray();
			}
		}

		internal Sequence[] CreateSequencesFromData(SequenceSource.ActorDelegate onHitActor, SequenceSource.Vector3Delegate onHitPos)
		{
			GameObject prefabOfSequenceId = SequenceLookup.Get().GetPrefabOfSequenceId(m_prefabID);
			BoardSquare targetSquare;
			if (m_useTargetSquare)
			{
				targetSquare = Board.Get().GetSquare(m_targetSquareX, m_targetSquareY);
			}
			else
			{
				targetSquare = null;
			}
			ActorData[] array = new ActorData[m_numTargetActors];
			for (int i = 0; i < m_numTargetActors; i++)
			{
				ActorData actorData = array[i] = GameFlowData.Get().FindActorByActorIndex(m_targetActorIndices[i]);
			}
			ActorData caster = GameFlowData.Get().FindActorByActorIndex(m_casterActorIndex);
			SequenceSource sequenceSource = new SequenceSource(onHitActor, onHitPos, m_sourceRootID, m_sourceRemoveAtEndOfTurn);
			sequenceSource.SetWaitForClientEnable(m_waitForClientEnable);
			if (m_useTargetRotation)
			{
				if (m_useTargetSquare)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, targetSquare, m_targetPos, m_targetRotation, array, caster, sequenceSource, m_extraParams);
						}
					}
				}
				return SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, m_targetPos, m_targetRotation, array, caster, sequenceSource, m_extraParams);
			}
			if (m_useTargetSquare)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, targetSquare, array, caster, sequenceSource, m_extraParams);
					}
				}
			}
			return SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, m_targetPos, array, caster, sequenceSource, m_extraParams);
		}

		internal bool HasSequencePrefab()
		{
			GameObject x = SequenceLookup.Get().GetPrefabOfSequenceId(m_prefabID);
			if (x == null)
			{
				if (SequenceLookup.Get() != null)
				{
					x = SequenceLookup.Get().GetSimpleHitSequencePrefab();
				}
			}
			return x != null;
		}

		internal bool Contains(SequenceSource sequenceSource)
		{
			int result;
			if (sequenceSource != null)
			{
				result = ((m_sourceRootID == sequenceSource.RootID) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
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
			sbyte value = (sbyte)m_associationType;
			stream.Serialize(ref m_prefabId);
			stream.Serialize(ref value);
			stream.Serialize(ref m_association);
			bool value2 = m_targetPos != Vector3.zero;
			stream.Serialize(ref value2);
			if (!value2)
			{
				return;
			}
			while (true)
			{
				stream.Serialize(ref m_targetPos);
				return;
			}
		}

		public static SequenceEndData SequenceEndData_DeserializeFromStream(ref IBitStream stream)
		{
			short value = -1;
			uint value2 = 0u;
			sbyte value3 = -1;
			bool value4 = false;
			Vector3 value5 = Vector3.zero;
			stream.Serialize(ref value);
			stream.Serialize(ref value3);
			stream.Serialize(ref value2);
			stream.Serialize(ref value4);
			if (value4)
			{
				stream.Serialize(ref value5);
			}
			return new SequenceEndData(value, (AssociationType)value3, (int)value2, value5);
		}

		public void EndClientSequences()
		{
			if (m_associationType == AssociationType.EffectGuid)
			{
				ClientEffectBarrierManager.Get().EndSequenceOfEffect(m_prefabId, (int)m_association, m_targetPos);
				return;
			}
			if (m_associationType == AssociationType.BarrierGuid)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						ClientEffectBarrierManager.Get().EndSequenceOfBarrier(m_prefabId, (int)m_association, m_targetPos);
						return;
					}
				}
			}
			if (m_associationType == AssociationType.SequenceSourceId)
			{
				SequenceManager.Get().MarkSequenceToEndBySourceId(m_prefabId, (int)m_association, m_targetPos);
			}
		}
	}

	public const bool c_sendClientCastActions = false;

	public static ActionBufferPhase GetCurrentActionPhase()
	{
		ActionBufferPhase result = ActionBufferPhase.Done;
		if (NetworkServer.active)
		{
		}
		else if (ClientActionBuffer.Get() != null)
		{
			result = ClientActionBuffer.Get().CurrentActionPhase;
		}
		else if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameStatus == GameStatus.Started)
			{
				Log.Error("Trying to examine current action phase, but ClientActionBuffer does not exist.");
			}
		}
		return result;
	}

	public static AbilityPriority GetCurrentAbilityPhase()
	{
		AbilityPriority result = AbilityPriority.INVALID;
		if (!NetworkServer.active)
		{
			if (ClientActionBuffer.Get() != null)
			{
				result = ClientActionBuffer.Get().AbilityPhase;
			}
			else
			{
				Log.Error("Trying to examine current ability phase, but ClientActionBuffer does not exist.");
			}
		}
		return result;
	}

	public static byte CreateBitfieldFromBoolsList(List<bool> bools)
	{
		byte b = 0;
		int num = Mathf.Min(bools.Count, 8);
		for (int i = 0; i < num; i++)
		{
			if (bools[i])
			{
				b = (byte)(b | (byte)(1 << i));
			}
		}
		while (true)
		{
			return b;
		}
	}

	public static short CreateBitfieldFromBoolsList_16bit(List<bool> bools)
	{
		short num = 0;
		int num2 = Mathf.Min(bools.Count, 16);
		for (int i = 0; i < num2; i++)
		{
			if (bools[i])
			{
				num = (short)(num | (short)(1 << i));
			}
		}
		return num;
	}

	public static int CreateBitfieldFromBoolsList_32bit(List<bool> bools)
	{
		int num = 0;
		int num2 = Mathf.Min(bools.Count, 32);
		for (int i = 0; i < num2; i++)
		{
			if (bools[i])
			{
				num |= 1 << i;
			}
		}
		while (true)
		{
			return num;
		}
	}

	public static byte CreateBitfieldFromBools(bool b0, bool b1, bool b2, bool b3, bool b4, bool b5, bool b6, bool b7)
	{
		byte b8 = 0;
		if (b0)
		{
			b8 = (byte)(b8 | 1);
		}
		if (b1)
		{
			b8 = (byte)(b8 | 2);
		}
		if (b2)
		{
			b8 = (byte)(b8 | 4);
		}
		if (b3)
		{
			b8 = (byte)(b8 | 8);
		}
		if (b4)
		{
			b8 = (byte)(b8 | 0x10);
		}
		if (b5)
		{
			b8 = (byte)(b8 | 0x20);
		}
		if (b6)
		{
			b8 = (byte)(b8 | 0x40);
		}
		if (b7)
		{
			b8 = (byte)(b8 | 0x80);
		}
		return b8;
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3, out bool out4, out bool out5, out bool out6, out bool out7)
	{
		out0 = ((bitField & 1) != 0);
		out1 = ((bitField & 2) != 0);
		out2 = ((bitField & 4) != 0);
		out3 = ((bitField & 8) != 0);
		out4 = ((bitField & 0x10) != 0);
		out5 = ((bitField & 0x20) != 0);
		out6 = ((bitField & 0x40) != 0);
		out7 = ((bitField & 0x80) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3, out bool out4, out bool out5, out bool out6)
	{
		out0 = ((bitField & 1) != 0);
		out1 = ((bitField & 2) != 0);
		out2 = ((bitField & 4) != 0);
		out3 = ((bitField & 8) != 0);
		out4 = ((bitField & 0x10) != 0);
		out5 = ((bitField & 0x20) != 0);
		out6 = ((bitField & 0x40) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3, out bool out4, out bool out5)
	{
		out0 = ((bitField & 1) != 0);
		out1 = ((bitField & 2) != 0);
		out2 = ((bitField & 4) != 0);
		out3 = ((bitField & 8) != 0);
		out4 = ((bitField & 0x10) != 0);
		out5 = ((bitField & 0x20) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3, out bool out4)
	{
		out0 = ((bitField & 1) != 0);
		out1 = ((bitField & 2) != 0);
		out2 = ((bitField & 4) != 0);
		out3 = ((bitField & 8) != 0);
		out4 = ((bitField & 0x10) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2, out bool out3)
	{
		out0 = ((bitField & 1) != 0);
		out1 = ((bitField & 2) != 0);
		out2 = ((bitField & 4) != 0);
		out3 = ((bitField & 8) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1, out bool out2)
	{
		out0 = ((bitField & 1) != 0);
		out1 = ((bitField & 2) != 0);
		out2 = ((bitField & 4) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0, out bool out1)
	{
		out0 = ((bitField & 1) != 0);
		out1 = ((bitField & 2) != 0);
	}

	public static void GetBoolsFromBitfield(byte bitField, out bool out0)
	{
		out0 = ((bitField & 1) != 0);
	}
}
