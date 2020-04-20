using System;

[Serializable]
public class LobbyMatchmakingQueueInfo
{
	public bool ShowQueueSize;

	public int QueuedPlayers;

	public float PlayersPerMinute;

	public TimeSpan AverageWaitTime = TimeSpan.FromMinutes(5.0);

	public LobbyGameConfig GameConfig;

	public QueueStatus QueueStatus;

	public override string ToString()
	{
		return (this.GameConfig != null) ? this.GameConfig.ToString() : "unknown";
	}

	public GameType GameType
	{
		get
		{
			GameType result;
			if (this.GameConfig == null)
			{
				result = GameType.None;
			}
			else
			{
				result = this.GameConfig.GameType;
			}
			return result;
		}
	}

	public LobbyMatchmakingQueueInfo Clone()
	{
		return (LobbyMatchmakingQueueInfo)base.MemberwiseClone();
	}

	public bool IsSame(GameType gameType)
	{
		bool result;
		if (this.GameConfig == null)
		{
			result = false;
		}
		else
		{
			result = (this.GameConfig.GameType == gameType);
		}
		return result;
	}

	public string WhatQueueIsWaitingForToMakeNextGame
	{
		get
		{
			switch (this.QueueStatus)
			{
			case QueueStatus.Idle:
			case QueueStatus.Success:
			case QueueStatus.WaitingForHumans:
			case QueueStatus.QueueDoesntHaveEnoughHumans:
			case QueueStatus.SubQueueConflict:
			case QueueStatus.MultipleRegions:
				return StringUtil.TR("AFewPlayers", "Global");
			case QueueStatus.WontMixNoobAndExpert:
			case QueueStatus.EloRangeTooExtreme:
			case QueueStatus.TeamEloTooExtreme:
			case QueueStatus.NeedDifferentCoopDifficulties:
			case QueueStatus.TierTooHighForPlacementPlayers:
			case QueueStatus.TierBreadthTooExtreme:
				return StringUtil.TR("VarietyOfPlayersSkills", "Global");
			case QueueStatus.WaitingForRoleAssassin:
			case QueueStatus.WaitingOnImbalancedRoleAssassin:
				return LocalizationPayload.Create("QueueNeedsMoreOfRoleX", "Global", new LocalizationArg[]
				{
					LocalizationArg_LocalizationPayload.Create(CharacterRole.Assassin.GetLocalizedPayload())
				}).ToString();
			case QueueStatus.WaitingForRoleTank:
			case QueueStatus.WaitingOnImbalancedRoleTank:
				return LocalizationPayload.Create("QueueNeedsMoreOfRoleX", "Global", new LocalizationArg[]
				{
					LocalizationArg_LocalizationPayload.Create(CharacterRole.Tank.GetLocalizedPayload())
				}).ToString();
			case QueueStatus.WaitingForRoleSupport:
			case QueueStatus.WaitingOnImbalancedRoleSupport:
				return LocalizationPayload.Create("QueueNeedsMoreOfRoleX", "Global", new LocalizationArg[]
				{
					LocalizationArg_LocalizationPayload.Create(CharacterRole.Support.GetLocalizedPayload())
				}).ToString();
			case QueueStatus.WaitingOnImbalancedRoleGeneric:
				return StringUtil.TR("VarietyOfRoles", "Global");
			case QueueStatus.BlockedByExpertCollisions:
			case QueueStatus.BlockedByNoobCollisions:
				return StringUtil.TR("VarietyOfFreelancers", "Global");
			case QueueStatus.WaitingOnImbalancedGroups:
			case QueueStatus.WaitingToBreakGroups:
			case QueueStatus.WaitingOnPerfectGroupComposition:
				return StringUtil.TR("VarietyOfGroupSizes", "Global");
			case QueueStatus.AllServersBusy:
				return StringUtil.TR("ServerResources", "Global");
			case QueueStatus.NeedDifferentOpponents:
				return StringUtil.TR("DifferentOpponents", "Global");
			case QueueStatus.TooManyWillFills:
				return StringUtil.TR("TooManyWillFillFreelancers", "Global");
			case QueueStatus.NotEnoughWillFills:
				return StringUtil.TR("NotEnoughWillFillFreelancers", "Global");
			}
			return StringUtil.TR("BetterMatchmakingCode", "Global");
		}
	}
}
