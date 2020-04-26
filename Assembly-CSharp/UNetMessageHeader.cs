public struct UNetMessageHeader
{
	public const int SIZE = 8;

	public uint msgSeqNum;

	public ushort msgSize;

	public short msgType;
}
