using UnityEngine.Networking;

public class SyncListVisionProviderInfo : SyncListStruct<VisionProviderInfo>
{
	protected override void SerializeItem(NetworkWriter writer, VisionProviderInfo item)
	{
		writer.WritePackedUInt32((uint)item.m_actorIndex);
		writer.WritePackedUInt32((uint)item.m_satelliteIndex);
		writer.WritePackedUInt32((uint)item.m_boardX);
		writer.WritePackedUInt32((uint)item.m_boardY);
		writer.Write(item.m_radius);
		writer.Write(item.m_radiusAsStraightLineDist);
		writer.Write((int)item.m_flag);
		writer.Write((int)item.m_brushRevealType);
		writer.Write(item.m_ignoreLos);
		writer.Write(item.m_canFunctionInGlobalBlind);
	}

	protected override VisionProviderInfo DeserializeItem(NetworkReader reader)
	{
		VisionProviderInfo result = default(VisionProviderInfo);
		result.m_actorIndex = (int)reader.ReadPackedUInt32();
		result.m_satelliteIndex = (int)reader.ReadPackedUInt32();
		result.m_boardX = (int)reader.ReadPackedUInt32();
		result.m_boardY = (int)reader.ReadPackedUInt32();
		result.m_radius = reader.ReadSingle();
		result.m_radiusAsStraightLineDist = reader.ReadBoolean();
		result.m_flag = (BoardSquare.VisibilityFlags)reader.ReadInt32();
		result.m_brushRevealType = (VisionProviderInfo.BrushRevealType)reader.ReadInt32();
		result.m_ignoreLos = reader.ReadBoolean();
		result.m_canFunctionInGlobalBlind = reader.ReadBoolean();
		return result;
	}

	public void SerializeItemPublic(NetworkWriter writer, VisionProviderInfo item)
	{
		SerializeItem(writer, item);
	}

	public VisionProviderInfo DeserializeItemPublic(NetworkReader reader)
	{
		return DeserializeItem(reader);
	}
}
