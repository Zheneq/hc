using UnityEngine;
using UnityEngine.Networking;

internal class NetworkReaderAdapter : IBitStream
{
	private NetworkReader m_stream;

	public bool isReading
	{
		get { return true; }
	}

	public bool isWriting
	{
		get { return false; }
	}

	public uint Position
	{
		get { return m_stream.Position; }
	}

	public NetworkWriter Writer
	{
		get { return null; }
	}

	internal NetworkReaderAdapter(NetworkReader stream)
	{
		m_stream = stream;
	}

	public void Serialize(ref bool value)
	{
		value = m_stream.ReadBoolean();
	}

	public void Serialize(ref char value)
	{
		value = m_stream.ReadChar();
	}

	public void Serialize(ref byte value)
	{
		value = m_stream.ReadByte();
	}

	public void Serialize(ref sbyte value)
	{
		value = m_stream.ReadSByte();
	}

	public void Serialize(ref float value)
	{
		value = m_stream.ReadSingle();
	}

	public void Serialize(ref int value)
	{
		value = m_stream.ReadInt32();
	}

	public void Serialize(ref Quaternion value)
	{
		value = m_stream.ReadQuaternion();
	}

	public void Serialize(ref short value)
	{
		value = m_stream.ReadInt16();
	}

	public void Serialize(ref Vector3 value)
	{
		value = m_stream.ReadVector3();
	}

	public void Serialize(ref uint value)
	{
		value = m_stream.ReadPackedUInt32();
	}

	public void Serialize(ref long value)
	{
		value = m_stream.ReadInt64();
	}

	public void Serialize(ref ulong value)
	{
		value = m_stream.ReadUInt64();
	}

	public void Serialize(ref string value)
	{
		value = m_stream.ReadString();
	}

	public void Write(byte[] buffer, int offset, int count)
	{
	}

	public byte[] ReadBytes(int count)
	{
		return m_stream.ReadBytes(count);
	}
}
