using System;
using UnityEngine.Networking;

public class SyncListSparkTetherAgeInfo : SyncListStruct<SparkBeamTrackerComponent.ActorIndexToTetherAge>
{
	protected override void SerializeItem(NetworkWriter writer, SparkBeamTrackerComponent.ActorIndexToTetherAge item)
	{
		writer.WritePackedUInt32((uint)item.m_actorIndex);
		writer.WritePackedUInt32((uint)item.m_tetherAge);
	}

	protected override SparkBeamTrackerComponent.ActorIndexToTetherAge DeserializeItem(NetworkReader reader)
	{
		return new SparkBeamTrackerComponent.ActorIndexToTetherAge
		{
			m_actorIndex = (int)reader.ReadPackedUInt32(),
			m_tetherAge = (int)reader.ReadPackedUInt32()
		};
	}

	public void SerializeItemPublic(NetworkWriter writer, SparkBeamTrackerComponent.ActorIndexToTetherAge item)
	{
		SerializeItem(writer, item);
	}

	public SparkBeamTrackerComponent.ActorIndexToTetherAge DeserializeItemPublic(NetworkReader reader)
	{
		return DeserializeItem(reader);
	}
}
