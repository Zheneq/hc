using System;
using UnityEngine.Networking;

public class SyncListVisionProviderInfo : SyncListStruct<VisionProviderInfo>
{
	public override void SerializeItem(NetworkWriter writer, VisionProviderInfo item)
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

	public override VisionProviderInfo DeserializeItem(NetworkReader reader)
	{
		return new VisionProviderInfo
		{
			m_actorIndex = (int)reader.ReadPackedUInt32(),
			m_satelliteIndex = (int)reader.ReadPackedUInt32(),
			m_boardX = (int)reader.ReadPackedUInt32(),
			m_boardY = (int)reader.ReadPackedUInt32(),
			m_radius = reader.ReadSingle(),
			m_radiusAsStraightLineDist = reader.ReadBoolean(),
			m_flag = (BoardSquare.VisibilityFlags)reader.ReadInt32(),
			m_brushRevealType = (VisionProviderInfo.BrushRevealType)reader.ReadInt32(),
			m_ignoreLos = reader.ReadBoolean(),
			m_canFunctionInGlobalBlind = reader.ReadBoolean()
		};
	}
}
