using System.Collections.Generic;

public interface IQueueRequirementApplicant
{
	int TotalMatches
	{
		get;
	}

	int CharacterMatches
	{
		get;
	}

	int VsHumanMatches
	{
		get;
	}

	ClientAccessLevel AccessLevel
	{
		get;
	}

	int SeasonLevel
	{
		get;
	}

	CharacterType CharacterType
	{
		get;
	}

	LocalizationArg_Handle LocalizedHandle
	{
		get;
	}

	float GameLeavingPoints
	{
		get;
	}

	IEnumerable<CharacterType> AvailableCharacters
	{
		get;
	}

	bool IsCharacterTypeAvailable(CharacterType ct);

	int GetReactorLevel(List<SeasonTemplate> seasons);
}
