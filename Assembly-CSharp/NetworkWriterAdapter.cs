using System;
using UnityEngine;
using UnityEngine.Networking;

internal class NetworkWriterAdapter : IBitStream
{
	private NetworkWriter m_stream;

	internal NetworkWriterAdapter(NetworkWriter stream)
	{
		this.m_stream = stream;
	}

	public bool isReading
	{
		get
		{
			return false;
		}
	}

	public bool isWriting
	{
		get
		{
			return true;
		}
	}

	public uint Position
	{
		get
		{
			uint result;
			if (this.m_stream.Position < 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NetworkWriterAdapter.get_Position()).MethodHandle;
				}
				result = 0U;
			}
			else
			{
				result = (uint)this.m_stream.Position;
			}
			return result;
		}
	}

	public NetworkWriter Writer
	{
		get
		{
			return this.m_stream;
		}
	}

	public void Serialize(ref bool value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref char value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref byte value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref sbyte value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref float value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref int value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref Quaternion value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref short value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref Vector3 value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref uint value)
	{
		this.m_stream.WritePackedUInt32(value);
	}

	public void Serialize(ref long value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref ulong value)
	{
		this.m_stream.Write(value);
	}

	public void Serialize(ref string value)
	{
		this.m_stream.Write(value);
	}

	public void Write(byte[] buffer, int offset, int count)
	{
		this.m_stream.Write(buffer, offset, count);
	}

	public byte[] ReadBytes(int count)
	{
		return null;
	}
}
