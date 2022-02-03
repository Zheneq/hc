using UnityEngine;

public class BarrierSerializeInfo
{
	public int m_guid;
	public Vector3 m_center;
	public float m_widthInWorld;
	public float m_facingHorizontalAngle;
	public bool m_bidirectional;
	public sbyte m_blocksVision;
	public sbyte m_blocksAbilities;
	public sbyte m_blocksMovement;
	public sbyte m_blocksMovementOnCrossover;
	public sbyte m_blocksPositionTargeting;
	public bool m_considerAsCover;
	public sbyte m_team;
	public int m_ownerIndex;
	public bool m_makeClientGeo;
	public bool m_clientSequenceStartAttempted;

	public static void SerializeBarrierInfo(IBitStream stream, ref BarrierSerializeInfo info)
	{
		if (info == null)
		{
			info = new BarrierSerializeInfo();
			if (stream.isWriting)
			{
				Log.Error("Trying to serialize null barrier start info");
			}
		}
		stream.Serialize(ref info.m_guid);
		stream.Serialize(ref info.m_center);
		stream.Serialize(ref info.m_widthInWorld);
		stream.Serialize(ref info.m_facingHorizontalAngle);
		stream.Serialize(ref info.m_bidirectional);
		stream.Serialize(ref info.m_blocksVision);
		stream.Serialize(ref info.m_blocksAbilities);
		stream.Serialize(ref info.m_blocksMovement);
		stream.Serialize(ref info.m_blocksMovementOnCrossover);
		stream.Serialize(ref info.m_blocksPositionTargeting);
		stream.Serialize(ref info.m_considerAsCover);
		stream.Serialize(ref info.m_team);
		stream.Serialize(ref info.m_ownerIndex);
		stream.Serialize(ref info.m_makeClientGeo);
	}
}
