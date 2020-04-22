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

	public GameType GameType
	{
		get
		{
			int result;
			if (GameConfig == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				result = -1;
			}
			else
			{
				result = (int)GameConfig.GameType;
			}
			return (GameType)result;
		}
	}

	public string WhatQueueIsWaitingForToMakeNextGame
	{
		get
		{
			switch (QueueStatus)
			{
			case QueueStatus.Idle:
			case QueueStatus.Success:
			case QueueStatus.WaitingForHumans:
			case QueueStatus.QueueDoesntHaveEnoughHumans:
			case QueueStatus.SubQueueConflict:
			case QueueStatus.MultipleRegions:
				return StringUtil.TR("AFewPlayers", "Global");
			case QueueStatus.BlockedByExpertCollisions:
			case QueueStatus.BlockedByNoobCollisions:
				return StringUtil.TR("VarietyOfFreelancers", "Global");
			case QueueStatus.TooManyWillFills:
				return StringUtil.TR("TooManyWillFillFreelancers", "Global");
			case QueueStatus.NotEnoughWillFills:
				return StringUtil.TR("NotEnoughWillFillFreelancers", "Global");
			case QueueStatus.WaitingForRoleAssassin:
			case QueueStatus.WaitingOnImbalancedRoleAssassin:
				return LocalizationPayload.Create("QueueNeedsMoreOfRoleX", "Global", LocalizationArg_LocalizationPayload.Create(CharacterRole.Assassin.GetLocalizedPayload())).ToString();
			case QueueStatus.WaitingForRoleTank:
			case QueueStatus.WaitingOnImbalancedRoleTank:
				return LocalizationPayload.Create("QueueNeedsMoreOfRoleX", "Global", LocalizationArg_LocalizationPayload.Create(CharacterRole.Tank.GetLocalizedPayload())).ToString();
			case QueueStatus.WaitingForRoleSupport:
			case QueueStatus.WaitingOnImbalancedRoleSupport:
				return LocalizationPayload.Create("QueueNeedsMoreOfRoleX", "Global", LocalizationArg_LocalizationPayload.Create(CharacterRole.Support.GetLocalizedPayload())).ToString();
			case QueueStatus.WaitingOnImbalancedRoleGeneric:
				return StringUtil.TR("VarietyOfRoles", "Global");
			case QueueStatus.WontMixNoobAndExpert:
			case QueueStatus.EloRangeTooExtreme:
			case QueueStatus.TeamEloTooExtreme:
			case QueueStatus.NeedDifferentCoopDifficulties:
			case QueueStatus.TierTooHighForPlacementPlayers:
			case QueueStatus.TierBreadthTooExtreme:
				return StringUtil.TR("VarietyOfPlayersSkills", "Global");
			case QueueStatus.WaitingOnImbalancedGroups:
			case QueueStatus.WaitingToBreakGroups:
			case QueueStatus.WaitingOnPerfectGroupComposition:
				return StringUtil.TR("VarietyOfGroupSizes", "Global");
			case QueueStatus.AllServersBusy:
				return StringUtil.TR("ServerResources", "Global");
			case QueueStatus.NeedDifferentOpponents:
				return StringUtil.TR("DifferentOpponents", "Global");
			default:
				return StringUtil.TR("BetterMatchmakingCode", "Global");
			}
		}
	}

	public override string ToString()
	{
		return (GameConfig != null) ? GameConfig.ToString() : "unknown";
	}

	public LobbyMatchmakingQueueInfo Clone()
	{
		return (LobbyMatchmakingQueueInfo)MemberwiseClone();
	}

	public bool IsSame(GameType gameType)
	{
		int result;
		if (GameConfig == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = 0;
		}
		else
		{
			result = ((GameConfig.GameType == gameType) ? 1 : 0);
		}
		return (byte)result != 0;
	}
}
