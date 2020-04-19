using System;

public interface ELOProvider
{
	float ELO { get; }

	long GroupId { get; }

	Team Team { get; set; }

	CharacterType SelectedCharacter { get; }

	CharacterRole SelectedRole { get; }

	bool IsNPCBot { get; }

	bool IsCollisionNoob { get; }

	int LossStreak { get; }

	string LanguageCode { get; }

	Region? Region { get; }
}
