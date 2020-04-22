using System;
using System.Collections.Generic;

public class MatchObjectiveKill : MatchObjective
{
	[Serializable]
	public class CharacterKillPointAdjustOverride
	{
		public List<CharacterType> m_killedCharacterTypes;

		public int m_pointAdjustOverrideForKillingTeam;

		public int m_pointAdjustOverrideForDyingTeam;

		public bool IsOverrideRelevantForActor(ActorData actor)
		{
			if (!(actor == null))
			{
				if (!(actor.GetCharacterResourceLink() == null))
				{
					if (!m_killedCharacterTypes.Contains(actor.GetCharacterResourceLink().m_characterType))
					{
						return false;
					}
					return true;
				}
				while (true)
				{
					switch (5)
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
			}
			return false;
		}
	}

	public enum KillObjectiveType
	{
		Actor,
		NPC,
		Player,
		Minion,
		ActorWithTag,
		ActorWithoutTag
	}

	public KillObjectiveType killType = KillObjectiveType.Player;

	public string m_tag = string.Empty;

	public int pointAdjustForKillingTeam = 1;

	public int pointAdjustForDyingTeam;

	public List<CharacterKillPointAdjustOverride> m_characterTypeOverrides;

	public bool IsActorRelevant(ActorData actor)
	{
		switch (killType)
		{
		case KillObjectiveType.Actor:
			return true;
		case KillObjectiveType.NPC:
			return !GameplayUtils.IsPlayerControlled(actor);
		case KillObjectiveType.Player:
			return GameplayUtils.IsPlayerControlled(actor);
		case KillObjectiveType.Minion:
			return GameplayUtils.IsMinion(actor);
		case KillObjectiveType.ActorWithTag:
			return actor.HasTag(m_tag);
		case KillObjectiveType.ActorWithoutTag:
			return !actor.HasTag(m_tag);
		default:
			return false;
		}
	}

	private void GetPointAdjusts(ActorData actor, out int pointsForDyingTeam, out int pointsForKillingTeam)
	{
		pointsForDyingTeam = pointAdjustForDyingTeam;
		pointsForKillingTeam = pointAdjustForKillingTeam;
		if (m_characterTypeOverrides == null)
		{
			return;
		}
		for (int i = 0; i < m_characterTypeOverrides.Count; i++)
		{
			if (!m_characterTypeOverrides[i].IsOverrideRelevantForActor(actor))
			{
				continue;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				pointsForDyingTeam = m_characterTypeOverrides[i].m_pointAdjustOverrideForDyingTeam;
				pointsForKillingTeam = m_characterTypeOverrides[i].m_pointAdjustOverrideForKillingTeam;
				return;
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override void Server_OnActorDeath(ActorData actor)
	{
		Log.Info(Log.Category.Temp, "MatchObjectiveKill.OnActorDeath");
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (!(objectivePoints != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (IsActorRelevant(actor))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					GetPointAdjusts(actor, out int pointsForDyingTeam, out int pointsForKillingTeam);
					Team team = actor.GetTeam();
					objectivePoints.AdjustPoints(pointsForDyingTeam, team);
					objectivePoints.AdjustPoints(pointsForKillingTeam, (team == Team.TeamA) ? Team.TeamB : Team.TeamA);
					return;
				}
			}
			return;
		}
	}

	public override void Client_OnActorDeath(ActorData actor)
	{
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (!(objectivePoints != null) || !IsActorRelevant(actor))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GetPointAdjusts(actor, out int pointsForDyingTeam, out int pointsForKillingTeam);
			Team team = actor.GetTeam();
			objectivePoints.AdjustUnresolvedPoints(pointsForDyingTeam, team);
			int adjustAmount = pointsForKillingTeam;
			int teamToAdjust;
			if (team == Team.TeamA)
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
				teamToAdjust = 1;
			}
			else
			{
				teamToAdjust = 0;
			}
			objectivePoints.AdjustUnresolvedPoints(adjustAmount, (Team)teamToAdjust);
			return;
		}
	}
}
