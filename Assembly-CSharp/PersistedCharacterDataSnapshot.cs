using System;

[Serializable]
public class PersistedCharacterDataSnapshot : ICloneable
{
	public string SnapshotId { get; set; }

	public PersistedCharacterData CharacterData { get; set; }

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
