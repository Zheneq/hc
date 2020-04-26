using System;
using System.Collections.Generic;

public interface IQueueRequirementSystemInfo
{
	IFreelancerSetQueryInterface FreelancerSetQueryInterface
	{
		get;
	}

	List<SeasonTemplate> Seasons
	{
		get;
	}

	DateTime GetCurrentUTCTime();

	EnvironmentType GetEnvironmentType();

	IEnumerable<GameType> GetGameTypes();

	GameLeavingPenalty GetGameLeavingPenaltyForGameType(GameType gameType);

	IEnumerable<QueueRequirement> GetQueueRequirements(GameType gameType);

	bool IsCharacterAllowed(CharacterType characterType, GameType gameType, GameSubType gameSubType);
}
