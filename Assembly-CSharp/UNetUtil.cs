using System.Collections.Generic;
using System.Text;

public static class UNetUtil
{
	public static short ReadInt16(byte[] buf, int offset)
	{
		ushort num = 0;
		num = (ushort)(num | buf[offset]);
		num = (ushort)(num | (ushort)(buf[offset + 1] << 8));
		return (short)num;
	}

	public static ushort ReadUInt16(byte[] buf, int offset)
	{
		ushort num = 0;
		num = (ushort)(num | buf[offset]);
		return (ushort)(num | (ushort)(buf[offset + 1] << 8));
	}

	public static uint ReadUInt32(byte[] buf, int offset)
	{
		uint num = 0u;
		num |= buf[offset];
		num = (uint)((int)num | (buf[offset + 1] << 8));
		num = (uint)((int)num | (buf[offset + 2] << 16));
		return (uint)((int)num | (buf[offset + 3] << 24));
	}

	public static uint GetSeqNum(byte[] bytes)
	{
		return ReadUInt32(bytes, 0);
	}

	public static ushort GetSize(byte[] bytes)
	{
		return ReadUInt16(bytes, 4);
	}

	public static short GetType(byte[] bytes)
	{
		return ReadInt16(bytes, 6);
	}

	public static List<UNetMessageHeader> ExtractMessageHeaders(byte[] bytes, int numBytes)
	{
		List<UNetMessageHeader> list = new List<UNetMessageHeader>();
		int num = numBytes;
		int num2 = 0;
		int num3 = 0;
		while (num > 0)
		{
			UNetMessageHeader item = default(UNetMessageHeader);
			item.msgSeqNum = ReadUInt32(bytes, num3);
			item.msgSize = ReadUInt16(bytes, num3 + 4);
			item.msgType = ReadInt16(bytes, num3 + 6);
			list.Add(item);
			int num4 = 8 + item.msgSize;
			num2++;
			num3 += num4;
			num -= num4;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public static string DumpMessageHeaders(byte[] bytes, int numBytes)
	{
		int num = numBytes;
		int num2 = 0;
		StringBuilder stringBuilder = new StringBuilder();
		while (num > 0)
		{
			uint num3 = ReadUInt32(bytes, num2);
			ushort num4 = ReadUInt16(bytes, num2 + 4);
			short num5 = ReadInt16(bytes, num2 + 6);
			int num6 = 8 + num4;
			stringBuilder.AppendFormat("message #{0} msgSize {1}, msgType {2}", num3, num4, num5);
			stringBuilder.AppendLine();
			num2 += num6;
			num -= num6;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return stringBuilder.ToString();
		}
	}
}
