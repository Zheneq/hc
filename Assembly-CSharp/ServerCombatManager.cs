using System;
using UnityEngine;

public class ServerCombatManager : MonoBehaviour
{
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

	public void ExecuteObjectivePointGain(ActorData caster, ActorData target, int finalChange)
	{
		if (target == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					throw new ApplicationException("Objective point change requires a target");
				}
			}
		}
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if ((bool)objectivePoints)
		{
			Team team = target.GetTeam();
			objectivePoints.AdjustPoints(finalChange, team);
		}
	}
}
