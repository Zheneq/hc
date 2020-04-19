using System;
using System.Collections.Generic;

public interface IQueueRequirementSystemInfo
{
	DateTime GetCurrentUTCTime();

	EnvironmentType GetEnvironmentType();

	IEnumerable<GameType> GetGameTypes();

	GameLeavingPenalty GetGameLeavingPenaltyForGameType(GameType gameType);

	IEnumerable<QueueRequirement> GetQueueRequirements(GameType gameType);

	IFreelancerSetQueryInterface FreelancerSetQueryInterface { get; }

	List<SeasonTemplate> Seasons { get; }

	bool IsCharacterAllowed(CharacterType characterType, GameType gameType, GameSubType gameSubType);
}
