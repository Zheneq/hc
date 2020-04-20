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
			this.m_writeStream = stream;
			this.m_appendWriter = new NetworkWriter();
			stream = new NetworkWriterAdapter(this.m_appendWriter);
		}
		else if (stream.isReading)
		{
			if (NetworkServer.active)
			{
				if (this.m_lastDataLength > 0U)
				{
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
				flag2 = true;
				int num2 = 0;
				while ((long)num2 < (long)((ulong)this.m_lastDataLength))
				{
					if (this.m_lastData[num2] != array[num2])
					{
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
						array[i] = actorsToSerialize[i].ActorIndex;
					}
					else
					{
						array[i] = ActorData.s_invalidActorIndex;
					}
				}
			}
			stream.Serialize(ref num);
			for (int j = 0; j < num; j++)
			{
				int num2 = array[j];
				stream.Serialize(ref num2);
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
			ActorData[] array2 = new ActorData[array.Length];
			for (int l = 0; l < array2.Length; l++)
			{
				if (array[l] != ActorData.s_invalidActorIndex)
				{
					array2[l] = ((!(GameFlowData.Get() != null)) ? null : GameFlowData.Get().FindActorByActorIndex(array[l]));
				}
			}
			actorsToSerialize = array2;
		}
	}

	public unsafe static void SerializeActorToIntDictionary(IBitStream stream, ref Dictionary<ActorData, int> actorToInt)
	{
		sbyte b;
		if (actorToInt == null)
		{
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
			if (actorToInt == null)
			{
				actorToInt = new Dictionary<ActorData, int>();
			}
		}
		if (stream.isWriting)
		{
			if ((int)b2 > 0)
			{
				using (Dictionary<ActorData, int>.Enumerator enumerator = actorToInt.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
						short num;
						if (keyValuePair.Key == null)
						{
							num = (short)ActorData.s_invalidActorIndex;
						}
						else
						{
							num = (short)keyValuePair.Key.ActorIndex;
						}
						short num2 = num;
						if ((int)num2 != ActorData.s_invalidActorIndex)
						{
							short num3 = (short)keyValuePair.Value;
							stream.Serialize(ref num2);
							stream.Serialize(ref num3);
						}
					}
				}
			}
		}
		if (stream.isReading)
		{
			if (actorToInt != null)
			{
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
					actorData = GameFlowData.Get().FindActorByActorIndex((int)actorIndex);
				}
				if (actorData != null)
				{
					actorToInt[actorData] = (int)value;
				}
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
		
		SerializeHelper.SerializeArray_Base<Vector3>(stream, ref toSerialize, zero, delegate(IBitStream s, ref Vector3 value)
			{
				s.Serialize(ref value);
			});
	}

	private unsafe static void SerializeArray_Base<T>(IBitStream stream, ref T[] toSerializeArray, T defaultValue, SerializeHelper.BitstreamSerializeDelegate<T> serializeDelegate)
	{
		int num = 0;
		if (stream.isWriting)
		{
			if (toSerializeArray != null)
			{
				num = toSerializeArray.Length;
			}
			stream.Serialize(ref num);
			for (int i = 0; i < num; i++)
			{
				T t = toSerializeArray[i];
				serializeDelegate(stream, ref t);
			}
		}
		if (stream.isReading)
		{
			stream.Serialize(ref num);
			T[] array = new T[num];
			for (int j = 0; j < array.Length; j++)
			{
				T t2 = defaultValue;
				serializeDelegate(stream, ref t2);
				array[j] = t2;
			}
			toSerializeArray = array;
		}
	}

	protected delegate void BitstreamSerializeDelegate<T>(IBitStream stream, ref T val);
}
