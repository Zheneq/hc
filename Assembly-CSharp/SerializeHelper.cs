using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SerializeHelper
{
	private IBitStream m_writeStream;

	private NetworkWriter m_appendWriter;

	private byte[] m_lastData;

	private uint m_lastDataLength;

	internal unsafe bool ShouldReturnImmediately(ref IBitStream stream)
	{
		if (stream.isWriting)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SerializeHelper.ShouldReturnImmediately(IBitStream*)).MethodHandle;
			}
			this.m_writeStream = stream;
			this.m_appendWriter = new NetworkWriter();
			stream = new NetworkWriterAdapter(this.m_appendWriter);
		}
		else if (stream.isReading)
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
			if (NetworkServer.active)
			{
				if (this.m_lastDataLength > 0U)
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
					stream.ReadBytes((int)this.m_lastDataLength);
				}
				return true;
			}
		}
		return false;
	}

	internal bool End(bool initialState, uint syncVarDirtyBits)
	{
		bool flag = false;
		if (this.m_writeStream != null)
		{
			bool flag2 = false;
			byte[] array = this.m_appendWriter.ToArray();
			uint num = (uint)this.m_appendWriter.Position;
			if (this.m_lastData != null && this.m_lastDataLength == num)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SerializeHelper.End(bool, uint)).MethodHandle;
				}
				flag2 = true;
				int num2 = 0;
				while ((long)num2 < (long)((ulong)this.m_lastDataLength))
				{
					if (this.m_lastData[num2] != array[num2])
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
						flag2 = false;
						break;
					}
					num2++;
				}
			}
			uint num3 = (!flag2) ? syncVarDirtyBits : 0U;
			if (!initialState)
			{
				this.m_writeStream.Serialize(ref num3);
			}
			if (!initialState)
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
				if (num3 == 0U)
				{
					goto IL_DD;
				}
			}
			this.m_writeStream.Write(array, (int)this.m_writeStream.Position, (int)num);
			this.m_lastData = array;
			this.m_lastDataLength = num;
			flag = (num > 0U);
			IL_DD:
			this.m_writeStream = null;
			this.m_appendWriter = null;
		}
		bool result;
		if (!flag)
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
			result = initialState;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public unsafe static void SerializeActorDataArray(IBitStream stream, ref ActorData[] actorsToSerialize)
	{
		int num = 0;
		int[] array = null;
		if (stream.isWriting)
		{
			if (actorsToSerialize != null)
			{
				num = actorsToSerialize.Length;
				array = new int[actorsToSerialize.Length];
				for (int i = 0; i < array.Length; i++)
				{
					if (actorsToSerialize[i] != null)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(SerializeHelper.SerializeActorDataArray(IBitStream, ActorData[]*)).MethodHandle;
						}
						array[i] = actorsToSerialize[i].ActorIndex;
					}
					else
					{
						array[i] = ActorData.s_invalidActorIndex;
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
			stream.Serialize(ref num);
			for (int j = 0; j < num; j++)
			{
				int num2 = array[j];
				stream.Serialize(ref num2);
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
		}
		if (stream.isReading)
		{
			stream.Serialize(ref num);
			array = new int[num];
			for (int k = 0; k < array.Length; k++)
			{
				int s_invalidActorIndex = ActorData.s_invalidActorIndex;
				stream.Serialize(ref s_invalidActorIndex);
				array[k] = s_invalidActorIndex;
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
			ActorData[] array2 = new ActorData[array.Length];
			for (int l = 0; l < array2.Length; l++)
			{
				if (array[l] != ActorData.s_invalidActorIndex)
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
					array2[l] = ((!(GameFlowData.Get() != null)) ? null : GameFlowData.Get().FindActorByActorIndex(array[l]));
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			actorsToSerialize = array2;
		}
	}

	public unsafe static void SerializeActorToIntDictionary(IBitStream stream, ref Dictionary<ActorData, int> actorToInt)
	{
		sbyte b;
		if (actorToInt == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SerializeHelper.SerializeActorToIntDictionary(IBitStream, Dictionary<ActorData, int>*)).MethodHandle;
			}
			b = 0;
		}
		else
		{
			b = (sbyte)actorToInt.Count;
		}
		sbyte b2 = b;
		stream.Serialize(ref b2);
		if ((int)b2 > 0)
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
			if (actorToInt == null)
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
				actorToInt = new Dictionary<ActorData, int>();
			}
		}
		if (stream.isWriting)
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
			if ((int)b2 > 0)
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
				using (Dictionary<ActorData, int>.Enumerator enumerator = actorToInt.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
						short num;
						if (keyValuePair.Key == null)
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
							num = (short)ActorData.s_invalidActorIndex;
						}
						else
						{
							num = (short)keyValuePair.Key.ActorIndex;
						}
						short num2 = num;
						if ((int)num2 != ActorData.s_invalidActorIndex)
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
							short num3 = (short)keyValuePair.Value;
							stream.Serialize(ref num2);
							stream.Serialize(ref num3);
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
				}
			}
		}
		if (stream.isReading)
		{
			if (actorToInt != null)
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
				actorToInt.Clear();
			}
			for (int i = 0; i < (int)b2; i++)
			{
				short actorIndex = (short)ActorData.s_invalidActorIndex;
				short value = 0;
				stream.Serialize(ref actorIndex);
				stream.Serialize(ref value);
				ActorData actorData = null;
				if (GameFlowData.Get() != null)
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
					actorData = GameFlowData.Get().FindActorByActorIndex((int)actorIndex);
				}
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
					actorToInt[actorData] = (int)value;
				}
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
		}
	}

	public static void SerializeArray(IBitStream stream, ref int[] toSerialize)
	{
		SerializeHelper.SerializeArray_Base<int>(stream, ref toSerialize, 0, delegate(IBitStream s, ref int value)
		{
			s.Serialize(ref value);
		});
	}

	public unsafe static void SerializeArray(IBitStream stream, ref Vector3[] toSerialize)
	{
		Vector3 zero = Vector3.zero;
		if (SerializeHelper.<>f__am$cache1 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SerializeHelper.SerializeArray(IBitStream, Vector3[]*)).MethodHandle;
			}
			SerializeHelper.<>f__am$cache1 = delegate(IBitStream s, ref Vector3 value)
			{
				s.Serialize(ref value);
			};
		}
		SerializeHelper.SerializeArray_Base<Vector3>(stream, ref toSerialize, zero, SerializeHelper.<>f__am$cache1);
	}

	private unsafe static void SerializeArray_Base<T>(IBitStream stream, ref T[] toSerializeArray, T defaultValue, SerializeHelper.BitstreamSerializeDelegate<T> serializeDelegate)
	{
		int num = 0;
		if (stream.isWriting)
		{
			if (toSerializeArray != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SerializeHelper.SerializeArray_Base(IBitStream, T[]*, T, SerializeHelper.BitstreamSerializeDelegate<T>)).MethodHandle;
				}
				num = toSerializeArray.Length;
			}
			stream.Serialize(ref num);
			for (int i = 0; i < num; i++)
			{
				T t = toSerializeArray[i];
				serializeDelegate(stream, ref t);
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
		}
		if (stream.isReading)
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
			stream.Serialize(ref num);
			T[] array = new T[num];
			for (int j = 0; j < array.Length; j++)
			{
				T t2 = defaultValue;
				serializeDelegate(stream, ref t2);
				array[j] = t2;
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
			toSerializeArray = array;
		}
	}

	protected delegate void BitstreamSerializeDelegate<T>(IBitStream stream, ref T val);
}
