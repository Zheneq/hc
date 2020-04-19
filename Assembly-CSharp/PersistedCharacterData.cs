using System;

[Serializable]
public class PersistedCharacterData : ICloneable
{
	public PersistedCharacterData(CharacterType characterType)
	{
		this.CharacterType = characterType;
		this.SchemaVersion = new SchemaVersion<CharacterSchemaChange>();
		this.CharacterComponent = new CharacterComponent();
		this.ExperienceComponent = new ExperienceComponent();
	}

	public SchemaVersion<CharacterSchemaChange> SchemaVersion { get; set; }

	public DateTime CreateDate { get; set; }

	public DateTime UpdateDate { get; set; }

	public CharacterType CharacterType { get; set; }

	public CharacterComponent CharacterComponent { get; set; }

	public ExperienceComponent ExperienceComponent { get; set; }

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
