using System;
using System.Collections.Generic;

public class MatchObjectiveKill : MatchObjective
{
	public MatchObjectiveKill.KillObjectiveType killType = MatchObjectiveKill.KillObjectiveType.Player;

	public string m_tag = string.Empty;

	public int pointAdjustForKillingTeam = 1;

	public int pointAdjustForDyingTeam;

	public List<MatchObjectiveKill.CharacterKillPointAdjustOverride> m_characterTypeOverrides;

	public bool IsActorRelevant(ActorData actor)
	{
		bool result;
		switch (this.killType)
		{
		case MatchObjectiveKill.KillObjectiveType.Actor:
			result = true;
			break;
		case MatchObjectiveKill.KillObjectiveType.NPC:
			result = !GameplayUtils.IsPlayerControlled(actor);
			break;
		case MatchObjectiveKill.KillObjectiveType.Player:
			result = GameplayUtils.IsPlayerControlled(actor);
			break;
		case MatchObjectiveKill.KillObjectiveType.Minion:
			result = GameplayUtils.IsMinion(actor);
			break;
		case MatchObjectiveKill.KillObjectiveType.ActorWithTag:
			result = actor.HasTag(this.m_tag);
			break;
		case MatchObjectiveKill.KillObjectiveType.ActorWithoutTag:
			result = !actor.HasTag(this.m_tag);
			break;
		default:
			result = false;
			break;
		}
		return result;
	}

	private unsafe void GetPointAdjusts(ActorData actor, out int pointsForDyingTeam, out int pointsForKillingTeam)
	{
		pointsForDyingTeam = this.pointAdjustForDyingTeam;
		pointsForKillingTeam = this.pointAdjustForKillingTeam;
		if (this.m_characterTypeOverrides != null)
		{
			for (int i = 0; i < this.m_characterTypeOverrides.Count; i++)
			{
				if (this.m_characterTypeOverrides[i].IsOverrideRelevantForActor(actor))
				{
					pointsForDyingTeam = this.m_characterTypeOverrides[i].m_pointAdjustOverrideForDyingTeam;
					pointsForKillingTeam = this.m_characterTypeOverrides[i].m_pointAdjustOverrideForKillingTeam;
					return;
				}
			}
		}
	}

	public override void Server_OnActorDeath(ActorData actor)
	{
		Log.Info(Log.Category.Temp, "MatchObjectiveKill.OnActorDeath", new object[0]);
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (objectivePoints != null)
		{
			if (this.IsActorRelevant(actor))
			{
				int adjustAmount;
				int adjustAmount2;
				this.GetPointAdjusts(actor, out adjustAmount, out adjustAmount2);
				Team team = actor.GetTeam();
				objectivePoints.AdjustPoints(adjustAmount, team);
				objectivePoints.AdjustPoints(adjustAmount2, (team != Team.TeamA) ? Team.TeamA : Team.TeamB);
			}
		}
	}

	public override void Client_OnActorDeath(ActorData actor)
	{
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (objectivePoints != null && this.IsActorRelevant(actor))
		{
			int adjustAmount;
			int num;
			this.GetPointAdjusts(actor, out adjustAmount, out num);
			Team team = actor.GetTeam();
			objectivePoints.AdjustUnresolvedPoints(adjustAmount, team);
			ObjectivePoints objectivePoints2 = objectivePoints;
			int adjustAmount2 = num;
			Team teamToAdjust;
			if (team == Team.TeamA)
			{
				teamToAdjust = Team.TeamB;
			}
			else
			{
				teamToAdjust = Team.TeamA;
			}
			objectivePoints2.AdjustUnresolvedPoints(adjustAmount2, teamToAdjust);
		}
	}

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
					return this.m_killedCharacterTypes.Contains(actor.GetCharacterResourceLink().m_characterType);
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
}
