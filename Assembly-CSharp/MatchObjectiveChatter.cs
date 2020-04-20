using System;
using UnityEngine;

[Serializable]
public class MatchObjectiveChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public MatchObjectiveChatter.MatchObjectiveType m_matchObjective;

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.MatchObjectiveEvent;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MatchObjectiveChatter.ShouldPlayChatter(GameEventManager.EventType, GameEventManager.GameEventArgs, ChatterComponent)).MethodHandle;
			}
			return false;
		}
		GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs = args as GameEventManager.MatchObjectiveEventArgs;
		if (matchObjectiveEventArgs == null)
		{
			Log.Error("Missing args for Match Objective game event.", new object[0]);
			return false;
		}
		ActorData component2 = component.gameObject.GetComponent<ActorData>();
		if (this.m_matchObjective == MatchObjectiveChatter.MatchObjectiveType.SelfPickedUpCoin)
		{
			for (;;)
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
				for (;;)
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
					return true;
				}
			}
			return false;
		}
		if (this.m_matchObjective == MatchObjectiveChatter.MatchObjectiveType.SelfPickedUpFlag)
		{
			for (;;)
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
				for (;;)
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
					return true;
				}
				for (;;)
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
		if (this.m_matchObjective == MatchObjectiveChatter.MatchObjectiveType.TeamCapturedControlPoint)
		{
			if (matchObjectiveEventArgs.objective == GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ControlPointCaptured)
			{
				for (;;)
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
					return true;
				}
				for (;;)
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
		if (this.m_matchObjective == MatchObjectiveChatter.MatchObjectiveType.SelfPickedUpCase)
		{
			for (;;)
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
				for (;;)
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
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public enum MatchObjectiveType
	{
		SelfPickedUpCoin,
		SelfPickedUpFlag,
		TeamCapturedControlPoint,
		SelfPickedUpCase
	}
}
