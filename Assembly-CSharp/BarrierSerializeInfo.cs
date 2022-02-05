// ROGUES
// SERVER
using UnityEngine;
using UnityEngine.Networking;

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

	// custom
	// TODO SERIALIZATION
	// NetworkWriter in rogues
#if SERVER2
	public static void SerializeBarrierInfo(NetworkWriter writer, BarrierSerializeInfo info)
	{
		IBitStream stream = new NetworkWriterAdapter(writer);
		SerializeBarrierInfo(stream, ref info);
	}
#endif

	// reactor
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

	// rogues
	//public static void SerializeBarrierInfo(NetworkWriter writer, BarrierSerializeInfo info)
	//{
	//	writer.Write(info.m_guid);
	//	writer.Write(info.m_center);
	//	writer.Write(info.m_widthInWorld);
	//	writer.Write(info.m_facingHorizontalAngle);
	//	writer.Write(info.m_bidirectional);
	//	writer.Write(info.m_blocksVision);
	//	writer.Write(info.m_blocksAbilities);
	//	writer.Write(info.m_blocksMovement);
	//	writer.Write(info.m_blocksMovementOnCrossover);
	//	writer.Write(info.m_blocksPositionTargeting);
	//	writer.Write(info.m_considerAsCover);
	//	writer.Write(info.m_team);
	//	writer.Write(info.m_ownerIndex);
	//	writer.Write(info.m_makeClientGeo);
	//}

	// TODO SERIALIZATION
#if SERVER
	public static void DeserializeBarrierInfo(NetworkReader reader, ref BarrierSerializeInfo info)
	{
		info.m_guid = reader.ReadInt32();
		info.m_center = reader.ReadVector3();
		info.m_widthInWorld = reader.ReadSingle();
		info.m_facingHorizontalAngle = reader.ReadSingle();
		info.m_bidirectional = reader.ReadBoolean();
		info.m_blocksVision = reader.ReadSByte();
		info.m_blocksAbilities = reader.ReadSByte();
		info.m_blocksMovement = reader.ReadSByte();
		info.m_blocksMovementOnCrossover = reader.ReadSByte();
		info.m_blocksPositionTargeting = reader.ReadSByte();
		info.m_considerAsCover = reader.ReadBoolean();
		info.m_team = reader.ReadSByte();
		info.m_ownerIndex = reader.ReadInt32();
		info.m_makeClientGeo = reader.ReadBoolean();
	}
#endif
}
