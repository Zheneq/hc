using UnityEngine.Networking;

public class SyncListTempCoverInfo : SyncListStruct<TempCoverInfo>
{
	public override void SerializeItem(NetworkWriter writer, TempCoverInfo item)
	{
		writer.Write((int)item.m_coverDir);
		writer.Write(item.m_ignoreMinDist);
	}

	public override TempCoverInfo DeserializeItem(NetworkReader reader)
	{
		TempCoverInfo result = default(TempCoverInfo);
		result.m_coverDir = (ActorCover.CoverDirections)reader.ReadInt32();
		result.m_ignoreMinDist = reader.ReadBoolean();
		return result;
	}
}
