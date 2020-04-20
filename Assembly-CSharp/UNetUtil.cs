using System;
using System.Collections.Generic;
using System.Text;

public static class UNetUtil
{
	public static short ReadInt16(byte[] buf, int offset)
	{
		ushort num = 0;
		num |= (ushort)buf[offset];
		num |= (ushort)(buf[offset + 1] << 8);
		return (short)num;
	}

	public static ushort ReadUInt16(byte[] buf, int offset)
	{
		ushort num = 0;
		num |= (ushort)buf[offset];
		return num | (ushort)(buf[offset + 1] << 8);
	}

	public static uint ReadUInt32(byte[] buf, int offset)
	{
		uint num = 0U;
		num |= (uint)buf[offset];
		num |= (uint)((uint)buf[offset + 1] << 8);
		num |= (uint)((uint)buf[offset + 2] << 0x10);
		return num | (uint)((uint)buf[offset + 3] << 0x18);
	}

	public static uint GetSeqNum(byte[] bytes)
	{
		return UNetUtil.ReadUInt32(bytes, 0);
	}

	public static ushort GetSize(byte[] bytes)
	{
		return UNetUtil.ReadUInt16(bytes, 4);
	}

	public static short GetType(byte[] bytes)
	{
		return UNetUtil.ReadInt16(bytes, 6);
	}

	public static List<UNetMessageHeader> ExtractMessageHeaders(byte[] bytes, int numBytes)
	{
		List<UNetMessageHeader> list = new List<UNetMessageHeader>();
		int i = numBytes;
		int num = 0;
		int num2 = 0;
		while (i > 0)
		{
			UNetMessageHeader item = new UNetMessageHeader
			{
				msgSeqNum = UNetUtil.ReadUInt32(bytes, num2),
				msgSize = UNetUtil.ReadUInt16(bytes, num2 + 4),
				msgType = UNetUtil.ReadInt16(bytes, num2 + 6)
			};
			list.Add(item);
			int num3 = (int)(8 + item.msgSize);
			num++;
			num2 += num3;
			i -= num3;
		}
		return list;
	}

	public static string DumpMessageHeaders(byte[] bytes, int numBytes)
	{
		int i = numBytes;
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		while (i > 0)
		{
			uint num2 = UNetUtil.ReadUInt32(bytes, num);
			ushort num3 = UNetUtil.ReadUInt16(bytes, num + 4);
			short num4 = UNetUtil.ReadInt16(bytes, num + 6);
			int num5 = (int)(8 + num3);
			stringBuilder.AppendFormat("message #{0} msgSize {1}, msgType {2}", num2, num3, num4);
			stringBuilder.AppendLine();
			num += num5;
			i -= num5;
		}
		return stringBuilder.ToString();
	}
}
