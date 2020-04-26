using UnityEngine;
using UnityEngine.Networking;

public interface IBitStream
{
	bool isReading
	{
		get;
	}

	bool isWriting
	{
		get;
	}

	uint Position
	{
		get;
	}

	NetworkWriter Writer
	{
		get;
	}

	void Serialize(ref bool value);

	void Serialize(ref char value);

	void Serialize(ref byte value);

	void Serialize(ref sbyte value);

	void Serialize(ref float value);

	void Serialize(ref int value);

	void Serialize(ref Quaternion value);

	void Serialize(ref short value);

	void Serialize(ref Vector3 value);

	void Serialize(ref uint value);

	void Serialize(ref long value);

	void Serialize(ref ulong value);

	void Serialize(ref string value);

	void Write(byte[] buffer, int offset, int count);

	byte[] ReadBytes(int count);
}
