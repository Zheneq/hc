using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ServerClientUtils
{
	public const bool c_sendClientCastActions = false;

	public static ActionBufferPhase GetCurrentActionPhase()
	{
		ActionBufferPhase result = ActionBufferPhase.Done;
		if (NetworkServer.active)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.GetCurrentActionPhase()).MethodHandle;
			}
		}
		else if (ClientActionBuffer.Get() != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			result = ClientActionBuffer.Get().CurrentActionPhase;
		}
		else if (GameManager.Get() != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameManager.Get().GameStatus == GameStatus.Started)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Error("Trying to examine current action phase, but ClientActionBuffer does not exist.", new object[0]);
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
				Log.Error("Trying to examine current ability phase, but ClientActionBuffer does not exist.", new object[0]);
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.CreateBitfieldFromBoolsList(List<bool>)).MethodHandle;
				}
				b |= (byte)(1 << i);
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		return b;
	}

	public static short CreateBitfieldFromBoolsList_16bit(List<bool> bools)
	{
		short num = 0;
		int num2 = Mathf.Min(bools.Count, 0x10);
		for (int i = 0; i < num2; i++)
		{
			if (bools[i])
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.CreateBitfieldFromBoolsList_16bit(List<bool>)).MethodHandle;
				}
				num |= (short)(1 << i);
			}
		}
		return num;
	}

	public static int CreateBitfieldFromBoolsList_32bit(List<bool> bools)
	{
		int num = 0;
		int num2 = Mathf.Min(bools.Count, 0x20);
		for (int i = 0; i < num2; i++)
		{
			if (bools[i])
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.CreateBitfieldFromBoolsList_32bit(List<bool>)).MethodHandle;
				}
				num |= 1 << i;
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		return num;
	}

	public static byte CreateBitfieldFromBools(bool b0, bool b1, bool b2, bool b3, bool b4, bool b5, bool b6, bool b7)
	{
		byte b8 = 0;
		if (b0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.CreateBitfieldFromBools(bool, bool, bool, bool, bool, bool, bool, bool)).MethodHandle;
			}
			b8 |= 1;
		}
		if (b1)
		{
			b8 |= 2;
		}
		if (b2)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			b8 |= 4;
		}
		if (b3)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			b8 |= 8;
		}
		if (b4)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			b8 |= 0x10;
		}
		if (b5)
		{
			b8 |= 0x20;
		}
		if (b6)
		{
			b8 |= 0x40;
		}
		if (b7)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			b8 |= 0x80;
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
			this.InitToDefaults();
			this.InitPrefab(prefab);
			this.InitSquare(targetSquare);
			this.InitTargetActors(targetActorArray);
			this.InitCasterActor(caster);
			this.InitSequenceSourceData(source);
			this.InitExtraParams(extraParams);
		}

		public SequenceStartData(GameObject prefab, BoardSquare targetSquare, Quaternion targetRotation, ActorData[] targetActorArray, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams = null)
		{
			this.InitToDefaults();
			this.InitPrefab(prefab);
			this.InitSquare(targetSquare);
			this.InitRotation(targetRotation);
			this.InitTargetActors(targetActorArray);
			this.InitCasterActor(caster);
			this.InitSequenceSourceData(source);
			this.InitExtraParams(extraParams);
		}

		public SequenceStartData(GameObject prefab, Vector3 targetPos, ActorData[] targetActorArray, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams = null)
		{
			this.InitToDefaults();
			this.InitPrefab(prefab);
			this.InitPos(targetPos);
			this.InitTargetActors(targetActorArray);
			this.InitCasterActor(caster);
			this.InitSequenceSourceData(source);
			this.InitExtraParams(extraParams);
		}

		public SequenceStartData(GameObject prefab, Vector3 targetPos, Quaternion targetRotation, ActorData[] targetActorArray, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams = null)
		{
			this.InitToDefaults();
			this.InitPrefab(prefab);
			this.InitPos(targetPos);
			this.InitRotation(targetRotation);
			this.InitTargetActors(targetActorArray);
			this.InitCasterActor(caster);
			this.InitSequenceSourceData(source);
			this.InitExtraParams(extraParams);
		}

		public SequenceStartData()
		{
			this.InitToDefaults();
		}

		public void SetRemoveAtEndOfTurn(bool val)
		{
			this.m_sourceRemoveAtEndOfTurn = val;
		}

		public void SetTargetPos(Vector3 pos)
		{
			this.InitPos(pos);
		}

		public Vector3 GetTargetPos()
		{
			return this.m_targetPos;
		}

		public int GetCasterActorIndex()
		{
			return this.m_casterActorIndex;
		}

		public Sequence.IExtraSequenceParams[] GetExtraParams()
		{
			return this.m_extraParams;
		}

		private void InitToDefaults()
		{
			this.m_prefabID = -1;
			this.m_serverOnlyPrefabReference = null;
			this.m_useTargetPos = false;
			this.m_targetPos = Vector3.zero;
			this.m_useTargetSquare = false;
			this.m_targetSquareX = 0;
			this.m_targetSquareY = 0;
			this.m_useTargetRotation = false;
			this.m_targetRotation = Quaternion.identity;
			this.m_numTargetActors = 0;
			this.m_targetActorIndices = null;
			this.m_casterActorIndex = ActorData.s_invalidActorIndex;
			this.m_numExtraParams = 0;
			this.m_extraParams = null;
			this.m_sourceRootID = 0U;
			this.m_sourceRemoveAtEndOfTurn = true;
			this.m_waitForClientEnable = false;
		}

		private void InitPrefab(GameObject prefab)
		{
			this.m_prefabID = SequenceLookup.Get().GetSequenceIdOfPrefab(prefab);
			this.m_serverOnlyPrefabReference = prefab;
		}

		private void InitPos(Vector3 targetPos)
		{
			this.m_useTargetPos = true;
			this.m_targetPos = targetPos;
		}

		private void InitSquare(BoardSquare square)
		{
			if (square != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.InitSquare(BoardSquare)).MethodHandle;
				}
				this.m_useTargetSquare = true;
				this.m_targetSquareX = square.x;
				this.m_targetSquareY = square.y;
				if (!this.m_useTargetPos)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_targetPos = square.ToVector3();
				}
			}
		}

		private void InitRotation(Quaternion targetRotation)
		{
			this.m_useTargetRotation = true;
			this.m_targetRotation = targetRotation;
		}

		public void InitTargetActors(ActorData[] targetActorArray)
		{
			if (targetActorArray != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.InitTargetActors(ActorData[])).MethodHandle;
				}
				if (targetActorArray.Length > 0)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_numTargetActors = (byte)targetActorArray.Length;
					this.m_targetActorIndices = new int[(int)this.m_numTargetActors];
					for (int i = 0; i < (int)this.m_numTargetActors; i++)
					{
						this.m_targetActorIndices[i] = targetActorArray[i].ActorIndex;
					}
				}
			}
		}

		public List<int> GetTargetActorIndices()
		{
			if (this.m_targetActorIndices != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.GetTargetActorIndices()).MethodHandle;
				}
				return new List<int>(this.m_targetActorIndices);
			}
			return new List<int>();
		}

		private void InitCasterActor(ActorData caster)
		{
			if (caster != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.InitCasterActor(ActorData)).MethodHandle;
				}
				this.m_casterActorIndex = caster.ActorIndex;
			}
			else
			{
				Log.Error("SequenceStartData trying to init its caster actor, but that actor is null.", new object[0]);
			}
		}

		internal void InitSequenceSourceData(SequenceSource source)
		{
			if (source != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.InitSequenceSourceData(SequenceSource)).MethodHandle;
				}
				this.m_sourceRootID = source.RootID;
				this.m_sourceRemoveAtEndOfTurn = source.RemoveAtEndOfTurn;
				this.m_waitForClientEnable = source.WaitForClientEnable;
			}
		}

		public void InitExtraParams(Sequence.IExtraSequenceParams[] extraParams)
		{
			if (extraParams != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.InitExtraParams(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				if (extraParams.Length > 0)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_extraParams = extraParams;
					this.m_numExtraParams = (byte)extraParams.Length;
				}
			}
		}

		public short GetSequencePrefabId()
		{
			return this.m_prefabID;
		}

		public GameObject GetServerOnlyPrefabReference()
		{
			return this.m_serverOnlyPrefabReference;
		}

		public string GetTargetActorsString()
		{
			string text = string.Empty;
			if (this.m_targetActorIndices != null && this.m_targetActorIndices.Length > 0)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.GetTargetActorsString()).MethodHandle;
				}
				for (int i = 0; i < this.m_targetActorIndices.Length; i++)
				{
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(this.m_targetActorIndices[i]);
					if (actorData != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						text = text + " | " + actorData.GetDebugName();
					}
					else
					{
						text = text + " | (Unknown Actor) " + this.m_targetActorIndices[i];
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				text = "(Empty)";
			}
			return text;
		}

		public unsafe void SequenceStartData_SerializeToStream(ref IBitStream stream)
		{
			uint position = stream.Position;
			byte b = ServerClientUtils.CreateBitfieldFromBools(this.m_useTargetPos, this.m_useTargetSquare, this.m_useTargetRotation, this.m_sourceRemoveAtEndOfTurn, this.m_waitForClientEnable, false, false, false);
			stream.Serialize(ref this.m_prefabID);
			stream.Serialize(ref b);
			if (this.m_useTargetPos)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.SequenceStartData_SerializeToStream(IBitStream*)).MethodHandle;
				}
				stream.Serialize(ref this.m_targetPos);
			}
			if (this.m_useTargetSquare)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				byte b2 = (byte)this.m_targetSquareX;
				byte b3 = (byte)this.m_targetSquareY;
				stream.Serialize(ref b2);
				stream.Serialize(ref b3);
			}
			if (this.m_useTargetRotation)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 vec = this.m_targetRotation * new Vector3(1f, 0f, 0f);
				float num = VectorUtils.HorizontalAngle_Deg(vec);
				stream.Serialize(ref num);
			}
			stream.Serialize(ref this.m_numTargetActors);
			for (byte b4 = 0; b4 < this.m_numTargetActors; b4 += 1)
			{
				sbyte b5 = (sbyte)this.m_targetActorIndices[(int)b4];
				stream.Serialize(ref b5);
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			sbyte b6 = (sbyte)this.m_casterActorIndex;
			stream.Serialize(ref b6);
			stream.Serialize(ref this.m_sourceRootID);
			stream.Serialize(ref this.m_numExtraParams);
			for (int i = 0; i < (int)this.m_numExtraParams; i++)
			{
				Sequence.IExtraSequenceParams extraSequenceParams = this.m_extraParams[i];
				SequenceLookup.SequenceExtraParamEnum enumOfExtraParam = SequenceLookup.GetEnumOfExtraParam(extraSequenceParams);
				short num2 = (short)enumOfExtraParam;
				stream.Serialize(ref num2);
				extraSequenceParams.XSP_SerializeToStream(stream);
			}
			uint num3 = stream.Position - position;
			if (ClientAbilityResults.\u000E)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"\t\t\t\t\t Serializing Sequence Start Data, using targetPos? ",
					this.m_useTargetPos.ToString(),
					" prefab id ",
					this.m_prefabID,
					": \n\t\t\t\t\t numBytes: ",
					num3
				}));
			}
		}

		public unsafe static ServerClientUtils.SequenceStartData SequenceStartData_DeserializeFromStream(ref IBitStream stream)
		{
			short prefabID = -1;
			byte bitField = 0;
			bool flag = false;
			Vector3 zero = Vector3.zero;
			bool flag2 = false;
			byte b = 0;
			byte b2 = 0;
			bool flag3 = false;
			Quaternion targetRotation = Quaternion.identity;
			byte b3 = 0;
			List<int> list = new List<int>();
			sbyte b4 = 0;
			uint sourceRootID = 0U;
			bool sourceRemoveAtEndOfTurn = true;
			bool waitForClientEnable = false;
			byte b5 = 0;
			List<Sequence.IExtraSequenceParams> list2 = new List<Sequence.IExtraSequenceParams>();
			stream.Serialize(ref prefabID);
			stream.Serialize(ref bitField);
			bool flag4;
			bool flag5;
			bool flag6;
			ServerClientUtils.GetBoolsFromBitfield(bitField, out flag, out flag2, out flag3, out sourceRemoveAtEndOfTurn, out waitForClientEnable, out flag4, out flag5, out flag6);
			if (flag)
			{
				stream.Serialize(ref zero);
			}
			if (flag2)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.SequenceStartData_DeserializeFromStream(IBitStream*)).MethodHandle;
				}
				stream.Serialize(ref b);
				stream.Serialize(ref b2);
			}
			if (flag3)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				float angle = 0f;
				stream.Serialize(ref angle);
				Vector3 toDirection = VectorUtils.AngleDegreesToVector(angle);
				targetRotation = Quaternion.FromToRotation(new Vector3(1f, 0f, 0f), toDirection);
			}
			stream.Serialize(ref b3);
			for (int i = 0; i < (int)b3; i++)
			{
				sbyte b6 = (sbyte)ActorData.s_invalidActorIndex;
				stream.Serialize(ref b6);
				list.Add((int)b6);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			stream.Serialize(ref b4);
			stream.Serialize(ref sourceRootID);
			stream.Serialize(ref b5);
			for (int j = 0; j < (int)b5; j++)
			{
				short num = 0;
				stream.Serialize(ref num);
				SequenceLookup.SequenceExtraParamEnum paramEnum = (SequenceLookup.SequenceExtraParamEnum)num;
				Sequence.IExtraSequenceParams extraSequenceParams = SequenceLookup.Get().CreateExtraParamOfEnum(paramEnum);
				extraSequenceParams.XSP_DeserializeFromStream(stream);
				list2.Add(extraSequenceParams);
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return new ServerClientUtils.SequenceStartData
			{
				m_prefabID = prefabID,
				m_useTargetPos = flag,
				m_targetPos = zero,
				m_useTargetSquare = flag2,
				m_targetSquareX = ((!flag2) ? -1 : ((int)b)),
				m_targetSquareY = ((!flag2) ? -1 : ((int)b2)),
				m_useTargetRotation = flag3,
				m_targetRotation = targetRotation,
				m_numTargetActors = b3,
				m_targetActorIndices = list.ToArray(),
				m_casterActorIndex = (int)b4,
				m_sourceRootID = sourceRootID,
				m_sourceRemoveAtEndOfTurn = sourceRemoveAtEndOfTurn,
				m_waitForClientEnable = waitForClientEnable,
				m_numExtraParams = b5,
				m_extraParams = list2.ToArray()
			};
		}

		internal Sequence[] CreateSequencesFromData(SequenceSource.ActorDelegate onHitActor, SequenceSource.Vector3Delegate onHitPos)
		{
			GameObject prefabOfSequenceId = SequenceLookup.Get().GetPrefabOfSequenceId(this.m_prefabID);
			BoardSquare targetSquare;
			if (this.m_useTargetSquare)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.CreateSequencesFromData(SequenceSource.ActorDelegate, SequenceSource.Vector3Delegate)).MethodHandle;
				}
				targetSquare = Board.Get().GetBoardSquare(this.m_targetSquareX, this.m_targetSquareY);
			}
			else
			{
				targetSquare = null;
			}
			ActorData[] array = new ActorData[(int)this.m_numTargetActors];
			for (int i = 0; i < (int)this.m_numTargetActors; i++)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(this.m_targetActorIndices[i]);
				array[i] = actorData;
			}
			ActorData caster = GameFlowData.Get().FindActorByActorIndex(this.m_casterActorIndex);
			SequenceSource sequenceSource = new SequenceSource(onHitActor, onHitPos, this.m_sourceRootID, this.m_sourceRemoveAtEndOfTurn);
			sequenceSource.SetWaitForClientEnable(this.m_waitForClientEnable);
			Sequence[] result;
			if (this.m_useTargetRotation)
			{
				if (this.m_useTargetSquare)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					result = SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, targetSquare, this.m_targetPos, this.m_targetRotation, array, caster, sequenceSource, this.m_extraParams);
				}
				else
				{
					result = SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, this.m_targetPos, this.m_targetRotation, array, caster, sequenceSource, this.m_extraParams);
				}
			}
			else if (this.m_useTargetSquare)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				result = SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, targetSquare, array, caster, sequenceSource, this.m_extraParams);
			}
			else
			{
				result = SequenceManager.Get().CreateClientSequences(prefabOfSequenceId, this.m_targetPos, array, caster, sequenceSource, this.m_extraParams);
			}
			return result;
		}

		internal bool HasSequencePrefab()
		{
			GameObject x = SequenceLookup.Get().GetPrefabOfSequenceId(this.m_prefabID);
			if (x == null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.HasSequencePrefab()).MethodHandle;
				}
				if (SequenceLookup.Get() != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					x = SequenceLookup.Get().GetSimpleHitSequencePrefab();
				}
			}
			return x != null;
		}

		internal bool Contains(SequenceSource sequenceSource)
		{
			bool result;
			if (sequenceSource != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceStartData.Contains(SequenceSource)).MethodHandle;
				}
				result = (this.m_sourceRootID == sequenceSource.RootID);
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal bool ContainsSequenceSourceID(uint id)
		{
			return this.m_sourceRootID == id;
		}
	}

	public class SequenceEndData
	{
		private short m_prefabId;

		private uint m_association;

		private ServerClientUtils.SequenceEndData.AssociationType m_associationType;

		private Vector3 m_targetPos;

		public SequenceEndData(int prefabIdToEnd, ServerClientUtils.SequenceEndData.AssociationType associationType, int guid, Vector3 targetPos)
		{
			this.m_prefabId = (short)prefabIdToEnd;
			this.m_associationType = associationType;
			this.m_association = (uint)Mathf.Max(0, guid);
			this.m_targetPos = targetPos;
		}

		public SequenceEndData(GameObject prefabToEnd, ServerClientUtils.SequenceEndData.AssociationType associationType, int guid, Vector3 targetPos)
		{
			this.m_prefabId = SequenceLookup.Get().GetSequenceIdOfPrefab(prefabToEnd);
			this.m_associationType = associationType;
			this.m_association = (uint)Mathf.Max(0, guid);
			this.m_targetPos = targetPos;
		}

		public unsafe void SequenceEndData_SerializeToStream(ref IBitStream stream)
		{
			sbyte b = (sbyte)this.m_associationType;
			stream.Serialize(ref this.m_prefabId);
			stream.Serialize(ref b);
			stream.Serialize(ref this.m_association);
			bool flag = this.m_targetPos != Vector3.zero;
			stream.Serialize(ref flag);
			if (flag)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceEndData.SequenceEndData_SerializeToStream(IBitStream*)).MethodHandle;
				}
				stream.Serialize(ref this.m_targetPos);
			}
		}

		public unsafe static ServerClientUtils.SequenceEndData SequenceEndData_DeserializeFromStream(ref IBitStream stream)
		{
			short prefabIdToEnd = -1;
			uint guid = 0U;
			sbyte b = -1;
			bool flag = false;
			Vector3 zero = Vector3.zero;
			stream.Serialize(ref prefabIdToEnd);
			stream.Serialize(ref b);
			stream.Serialize(ref guid);
			stream.Serialize(ref flag);
			if (flag)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceEndData.SequenceEndData_DeserializeFromStream(IBitStream*)).MethodHandle;
				}
				stream.Serialize(ref zero);
			}
			return new ServerClientUtils.SequenceEndData((int)prefabIdToEnd, (ServerClientUtils.SequenceEndData.AssociationType)b, (int)guid, zero);
		}

		public void EndClientSequences()
		{
			if (this.m_associationType == ServerClientUtils.SequenceEndData.AssociationType.EffectGuid)
			{
				ClientEffectBarrierManager.Get().EndSequenceOfEffect((int)this.m_prefabId, (int)this.m_association, this.m_targetPos);
			}
			else if (this.m_associationType == ServerClientUtils.SequenceEndData.AssociationType.BarrierGuid)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ServerClientUtils.SequenceEndData.EndClientSequences()).MethodHandle;
				}
				ClientEffectBarrierManager.Get().EndSequenceOfBarrier((int)this.m_prefabId, (int)this.m_association, this.m_targetPos);
			}
			else if (this.m_associationType == ServerClientUtils.SequenceEndData.AssociationType.SequenceSourceId)
			{
				SequenceManager.Get().MarkSequenceToEndBySourceId((int)this.m_prefabId, (int)this.m_association, this.m_targetPos);
			}
		}

		public enum AssociationType
		{
			EffectGuid,
			BarrierGuid,
			SequenceSourceId
		}
	}
}
