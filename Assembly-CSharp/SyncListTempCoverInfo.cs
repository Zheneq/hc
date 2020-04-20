using System;
using UnityEngine.Networking;

public class SyncListTempCoverInfo : SyncListStruct<TempCoverInfo>
{
    protected override void SerializeItem(NetworkWriter writer, TempCoverInfo item)
	{
		writer.Write((int)item.m_coverDir);
		writer.Write(item.m_ignoreMinDist);
	}

    protected override TempCoverInfo DeserializeItem(NetworkReader reader)
	{
		return new TempCoverInfo
		{
			m_coverDir = (ActorCover.CoverDirections)reader.ReadInt32(),
			m_ignoreMinDist = reader.ReadBoolean()
		};
	}

	public void SerializeItemPublic(NetworkWriter writer, TempCoverInfo item)
	{
		SerializeItem(writer, item);
	}

	public TempCoverInfo DeserializeItemPublic(NetworkReader reader)
	{
		return DeserializeItem(reader);
	}
}
