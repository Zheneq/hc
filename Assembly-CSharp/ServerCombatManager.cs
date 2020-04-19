using System;
using UnityEngine;

public class ServerCombatManager : MonoBehaviour
{
	public void ExecuteObjectivePointGain(ActorData caster, ActorData target, int finalChange)
	{
		if (target == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ServerCombatManager.ExecuteObjectivePointGain(ActorData, ActorData, int)).MethodHandle;
			}
			throw new ApplicationException("Objective point change requires a target");
		}
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (objectivePoints)
		{
			Team teamToAdjust = target.\u000E();
			objectivePoints.AdjustPoints(finalChange, teamToAdjust);
		}
	}

	public enum DamageType
	{
		Ability,
		Effect,
		Thorns,
		Barrier
	}

	public enum HealingType
	{
		Ability,
		Effect,
		Card,
		Powerup,
		Lifesteal,
		Barrier
	}

	public enum TechPointChangeType
	{
		Ability,
		Effect,
		Barrier,
		AbilityInteraction,
		Card,
		Powerup,
		Regen
	}
}
