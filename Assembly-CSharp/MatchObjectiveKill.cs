// ROGUES
// SERVER
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
			return actor != null
			       && actor.GetCharacterResourceLink() != null
			       && m_killedCharacterTypes.Contains(actor.GetCharacterResourceLink().m_characterType);
		}
	}

	public enum KillObjectiveType
	{
		Actor,
		NPC,
		Player,
		Minion,  // removed in rogues
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
		case KillObjectiveType.Minion:  // removed in rogues
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
		foreach (CharacterKillPointAdjustOverride adjustOverride in m_characterTypeOverrides)
		{
			if (adjustOverride.IsOverrideRelevantForActor(actor))
			{
				pointsForDyingTeam = adjustOverride.m_pointAdjustOverrideForDyingTeam;
				pointsForKillingTeam = adjustOverride.m_pointAdjustOverrideForKillingTeam;
				return;
			}
		}
	}

	public override void Server_OnActorDeath(ActorData actor)
	{
		Log.Info(Log.Category.Temp, "MatchObjectiveKill.OnActorDeath");
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (objectivePoints != null && IsActorRelevant(actor))
		{
			GetPointAdjusts(actor, out int pointsForDyingTeam, out int pointsForKillingTeam);
			Team team = actor.GetTeam();
			objectivePoints.AdjustPoints(pointsForDyingTeam, team);
			objectivePoints.AdjustPoints(pointsForKillingTeam, team == Team.TeamA ? Team.TeamB : Team.TeamA);
		}
	}

	// rogues
	// public override void Server_OnActorRevived(ActorData actor)
	// {
	// 	Log.Info(Log.Category.Temp, "MatchObjectiveKill.OnActorDeath");
	// 	ObjectivePoints objectivePoints = ObjectivePoints.Get();
	// 	if (objectivePoints != null && IsActorRelevant(actor))
	// 	{
	// 		GetPointAdjusts(actor, out var num, out var num2);
	// 		Team team = actor.GetTeam();
	// 		objectivePoints.AdjustPoints(-num, team);
	// 		objectivePoints.AdjustPoints(-num2, team == Team.TeamA ? Team.TeamB : Team.TeamA);
	// 	}
	// }

	public override void Client_OnActorDeath(ActorData actor)
	{
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (objectivePoints != null && IsActorRelevant(actor))
		{
			GetPointAdjusts(actor, out int pointsForDyingTeam, out int pointsForKillingTeam);
			Team team = actor.GetTeam();
			objectivePoints.AdjustUnresolvedPoints(pointsForDyingTeam, team);
			objectivePoints.AdjustUnresolvedPoints(pointsForKillingTeam, team == Team.TeamA ? Team.TeamB : Team.TeamA);
		}
	}
}
