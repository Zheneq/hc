// ROGUES
// SERVER
using UnityEngine.Networking;

// generated network code
#if SERVER
public class SyncListBarrierSpawnData : SyncList<BarrierSerializeInfo>
{
	public void SerializeItem_Public(NetworkWriter writer, BarrierSerializeInfo value)
	{
		SerializeItem(writer, value);
	}

	protected override void SerializeItem(NetworkWriter writer, BarrierSerializeInfo value)
	{
		writer.WritePackedUInt32((uint)value.m_guid);
		writer.Write(value.m_center);
		writer.Write(value.m_widthInWorld);
		writer.Write(value.m_facingHorizontalAngle);
		writer.Write(value.m_bidirectional);
		writer.Write(value.m_blocksVision);
		writer.Write(value.m_blocksAbilities);
		writer.Write(value.m_blocksMovement);
		writer.Write(value.m_blocksMovementOnCrossover);
		writer.Write(value.m_blocksPositionTargeting);
		writer.Write(value.m_considerAsCover);
		writer.Write(value.m_team);
		writer.WritePackedUInt32((uint)value.m_ownerIndex);
		writer.Write(value.m_makeClientGeo);
	}

	public BarrierSerializeInfo DeserializeItem_Public(NetworkReader reader)
	{
		return DeserializeItem(reader);
	}

	protected override BarrierSerializeInfo DeserializeItem(NetworkReader reader)
	{
		return new BarrierSerializeInfo
		{
			m_guid = (int)reader.ReadPackedUInt32(),
			m_center = reader.ReadVector3(),
			m_widthInWorld = reader.ReadSingle(),
			m_facingHorizontalAngle = reader.ReadSingle(),
			m_bidirectional = reader.ReadBoolean(),
			m_blocksVision = reader.ReadSByte(),
			m_blocksAbilities = reader.ReadSByte(),
			m_blocksMovement = reader.ReadSByte(),
			m_blocksMovementOnCrossover = reader.ReadSByte(),
			m_blocksPositionTargeting = reader.ReadSByte(),
			m_considerAsCover = reader.ReadBoolean(),
			m_team = reader.ReadSByte(),
			m_ownerIndex = (int)reader.ReadPackedUInt32(),
			m_makeClientGeo = reader.ReadBoolean()
		};
	}
}
#endif
