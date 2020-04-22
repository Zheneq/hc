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
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (matchObjectiveEventArgs.objective == GameEventManager.MatchObjectiveEventArgs.ObjectiveType.CoinCollected)
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
				if (!(matchObjectiveEventArgs.activatingActor != component2))
				{
					goto IL_013c;
				}
			}
			return false;
		}
		if (m_matchObjective == MatchObjectiveType.SelfPickedUpFlag)
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
			if (matchObjectiveEventArgs.objective == GameEventManager.MatchObjectiveEventArgs.ObjectiveType.FlagPickedUp_Client)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(matchObjectiveEventArgs.activatingActor != component2))
				{
					goto IL_013c;
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
			}
			return false;
		}
		if (m_matchObjective == MatchObjectiveType.TeamCapturedControlPoint)
		{
			if (matchObjectiveEventArgs.objective == GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ControlPointCaptured)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (matchObjectiveEventArgs.team == component2.GetTeam())
				{
					goto IL_013c;
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
			}
			return false;
		}
		if (m_matchObjective == MatchObjectiveType.SelfPickedUpCase)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (matchObjectiveEventArgs.objective == GameEventManager.MatchObjectiveEventArgs.ObjectiveType.CasePickedUp_Client)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
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
