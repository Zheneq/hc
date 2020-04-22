using System;

public class UNetMessage
{
	public const int MSG_HEADER_SIZE = 9;

	public byte[] Bytes;

	public int NumBytes;

	public byte[] Serialize()
	{
		if (Bytes != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (NumBytes + 1 >= 9)
			{
				byte[] array = new byte[NumBytes + 1];
				array[0] = 0;
				Buffer.BlockCopy(Bytes, 0, array, 1, NumBytes);
				return array;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		Log.Error("BinaryMessage.Serialize invalid message numBytes={0}", NumBytes);
		return null;
	}

	public void Deserialize(byte[] rawData)
	{
		if (rawData != null)
		{
			if (rawData.Length >= 9)
			{
				NumBytes = rawData.Length - 1;
				Bytes = new byte[NumBytes];
				Buffer.BlockCopy(rawData, 1, Bytes, 0, NumBytes);
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		Log.Error("BinaryMessage.Deserialize invalid message bytes {0}", (rawData == null) ? (-1) : rawData.Length);
	}
}
