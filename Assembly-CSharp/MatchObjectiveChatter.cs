using System;
using UnityEngine;

[Serializable]
public class MatchObjectiveChatter : ScriptableObject, IChatterData
{
	public enum MatchObjectiveType
	{
		SelfPickedUpCoin,
		SelfPickedUpFlag,
		TeamCapturedControlPoint,
		SelfPickedUpCase
	}

	public ChatterData m_baseData = new ChatterData();

	public MatchObjectiveType m_matchObjective;

	public ChatterData GetCommonData()
	{
		return m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.MatchObjectiveEvent;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			while (true)
			{
				return false;
			}
		}
		GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs = args as GameEventManager.MatchObjectiveEventArgs;
		if (matchObjectiveEventArgs == null)
		{
			Log.Error("Missing args for Match Objective game event.");
			return false;
		}
		ActorData component2 = component.gameObject.GetComponent<ActorData>();
		if (m_matchObjective == MatchObjectiveType.SelfPickedUpCoin)
		{
			if (matchObjectiveEventArgs.objective == GameEventManager.MatchObjectiveEventArgs.ObjectiveType.CoinCollected)
			{
				if (!(matchObjectiveEventArgs.activatingActor != component2))
				{
					goto IL_013c;
				}
			}
			return false;
		}
		if (m_matchObjective == MatchObjectiveType.SelfPickedUpFlag)
		{
			if (matchObjectiveEventArgs.objective == GameEventManager.MatchObjectiveEventArgs.ObjectiveType.FlagPickedUp_Client)
			{
				if (!(matchObjectiveEventArgs.activatingActor != component2))
				{
					goto IL_013c;
				}
			}
			return false;
		}
		if (m_matchObjective == MatchObjectiveType.TeamCapturedControlPoint)
		{
			if (matchObjectiveEventArgs.objective == GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ControlPointCaptured)
			{
				if (matchObjectiveEventArgs.team == component2.GetTeam())
				{
					goto IL_013c;
				}
			}
			return false;
		}
		if (m_matchObjective == MatchObjectiveType.SelfPickedUpCase)
		{
			if (matchObjectiveEventArgs.objective == GameEventManager.MatchObjectiveEventArgs.ObjectiveType.CasePickedUp_Client)
			{
				if (!(matchObjectiveEventArgs.activatingActor != component2))
				{
					goto IL_013c;
				}
			}
			return false;
		}
		goto IL_013c;
		IL_013c:
		return true;
	}
}
