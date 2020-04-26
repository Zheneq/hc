using System;

[Serializable]
public class PersistedCharacterData : ICloneable
{
	public SchemaVersion<CharacterSchemaChange> SchemaVersion
	{
		get;
		set;
	}

	public DateTime CreateDate
	{
		get;
		set;
	}

	public DateTime UpdateDate
	{
		get;
		set;
	}

	public CharacterType CharacterType
	{
		get;
		set;
	}

	public CharacterComponent CharacterComponent
	{
		get;
		set;
	}

	public ExperienceComponent ExperienceComponent
	{
		get;
		set;
	}

	public PersistedCharacterData(CharacterType characterType)
	{
		CharacterType = characterType;
		SchemaVersion = new SchemaVersion<CharacterSchemaChange>();
		CharacterComponent = new CharacterComponent();
		ExperienceComponent = new ExperienceComponent();
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
