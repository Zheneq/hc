using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SerializeHelper
{
	protected delegate void BitstreamSerializeDelegate<T>(IBitStream stream, ref T val);

	private IBitStream m_writeStream;

	private NetworkWriter m_appendWriter;

	private byte[] m_lastData;

	private uint m_lastDataLength;

	internal bool ShouldReturnImmediately(ref IBitStream stream)
	{
		if (stream.isWriting)
		{
			m_writeStream = stream;
			m_appendWriter = new NetworkWriter();
			stream = new NetworkWriterAdapter(m_appendWriter);
		}
		else if (stream.isReading)
		{
			if (NetworkServer.active)
			{
				if (m_lastDataLength != 0)
				{
					stream.ReadBytes((int)m_lastDataLength);
				}
				return true;
			}
		}
		return false;
	}

	internal bool End(bool initialState, uint syncVarDirtyBits)
	{
		bool flag = false;
		if (m_writeStream != null)
		{
			bool flag2 = false;
			byte[] array = m_appendWriter.ToArray();
			uint num = (uint)m_appendWriter.Position;
			if (m_lastData != null && m_lastDataLength == num)
			{
				flag2 = true;
				for (int i = 0; i < m_lastDataLength; i++)
				{
					if (m_lastData[i] != array[i])
					{
						flag2 = false;
						break;
					}
				}
			}
			uint value = (!flag2) ? syncVarDirtyBits : 0u;
			if (!initialState)
			{
				m_writeStream.Serialize(ref value);
			}
			if (!initialState)
			{
				if (value == 0)
				{
					goto IL_00dd;
				}
			}
			m_writeStream.Write(array, (int)m_writeStream.Position, (int)num);
			m_lastData = array;
			m_lastDataLength = num;
			flag = (num != 0);
			goto IL_00dd;
		}
		goto IL_00eb;
		IL_00eb:
		int result;
		if (!flag)
		{
			result = (initialState ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
		IL_00dd:
		m_writeStream = null;
		m_appendWriter = null;
		goto IL_00eb;
	}

	public static void SerializeActorDataArray(IBitStream stream, ref ActorData[] actorsToSerialize)
	{
		int value = 0;
		int[] array = null;
		if (stream.isWriting)
		{
			if (actorsToSerialize != null)
			{
				value = actorsToSerialize.Length;
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
			stream.Serialize(ref value);
			for (int j = 0; j < value; j++)
			{
				int value2 = array[j];
				stream.Serialize(ref value2);
			}
		}
		if (!stream.isReading)
		{
			return;
		}
		stream.Serialize(ref value);
		array = new int[value];
		for (int k = 0; k < array.Length; k++)
		{
			int value3 = ActorData.s_invalidActorIndex;
			stream.Serialize(ref value3);
			array[k] = value3;
		}
		while (true)
		{
			ActorData[] array2 = new ActorData[array.Length];
			for (int l = 0; l < array2.Length; l++)
			{
				if (array[l] != ActorData.s_invalidActorIndex)
				{
					array2[l] = ((!(GameFlowData.Get() != null)) ? null : GameFlowData.Get().FindActorByActorIndex(array[l]));
				}
			}
			while (true)
			{
				actorsToSerialize = array2;
				return;
			}
		}
	}

	public static void SerializeActorToIntDictionary(IBitStream stream, ref Dictionary<ActorData, int> actorToInt)
	{
		int num;
		if (actorToInt == null)
		{
			num = 0;
		}
		else
		{
			num = actorToInt.Count;
		}
		sbyte value = checked((sbyte)num);
		stream.Serialize(ref value);
		if (value > 0)
		{
			if (actorToInt == null)
			{
				actorToInt = new Dictionary<ActorData, int>();
			}
		}
		if (stream.isWriting)
		{
			if (value > 0)
			{
				using (Dictionary<ActorData, int>.Enumerator enumerator = actorToInt.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, int> current = enumerator.Current;
						int num2;
						if (current.Key == null)
						{
							num2 = ActorData.s_invalidActorIndex;
						}
						else
						{
							num2 = current.Key.ActorIndex;
						}
						short value2 = (short)num2;
						if (value2 != ActorData.s_invalidActorIndex)
						{
							short value3 = (short)current.Value;
							stream.Serialize(ref value2);
							stream.Serialize(ref value3);
						}
					}
				}
			}
		}
		if (!stream.isReading)
		{
			return;
		}
		if (actorToInt != null)
		{
			actorToInt.Clear();
		}
		for (int i = 0; i < value; i++)
		{
			short value4 = (short)ActorData.s_invalidActorIndex;
			short value5 = 0;
			stream.Serialize(ref value4);
			stream.Serialize(ref value5);
			ActorData actorData = null;
			if (GameFlowData.Get() != null)
			{
				actorData = GameFlowData.Get().FindActorByActorIndex(value4);
			}
			if (actorData != null)
			{
				actorToInt[actorData] = value5;
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public static void SerializeArray(IBitStream stream, ref int[] toSerialize)
	{
		SerializeArray_Base(stream, ref toSerialize, 0, delegate(IBitStream s, ref int value)
		{
			s.Serialize(ref value);
		});
	}

	public static void SerializeArray(IBitStream stream, ref Vector3[] toSerialize)
	{
		Vector3 zero = Vector3.zero;
		
		SerializeArray_Base(stream, ref toSerialize, zero, delegate(IBitStream s, ref Vector3 value)
			{
				s.Serialize(ref value);
			});
	}

	private static void SerializeArray_Base<T>(IBitStream stream, ref T[] toSerializeArray, T defaultValue, BitstreamSerializeDelegate<T> serializeDelegate)
	{
		int value = 0;
		T[] array = null;
		if (stream.isWriting)
		{
			if (toSerializeArray != null)
			{
				value = toSerializeArray.Length;
			}
			stream.Serialize(ref value);
			for (int i = 0; i < value; i++)
			{
				T val = toSerializeArray[i];
				serializeDelegate(stream, ref val);
			}
		}
		if (!stream.isReading)
		{
			return;
		}
		while (true)
		{
			stream.Serialize(ref value);
			array = new T[value];
			for (int j = 0; j < array.Length; j++)
			{
				T val2 = defaultValue;
				serializeDelegate(stream, ref val2);
				array[j] = val2;
			}
			while (true)
			{
				toSerializeArray = array;
				return;
			}
		}
	}
}
