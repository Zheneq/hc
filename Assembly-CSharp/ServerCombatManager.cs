using System;
using UnityEngine;

public class ServerCombatManager : MonoBehaviour
{
	public void ExecuteObjectivePointGain(ActorData caster, ActorData target, int finalChange)
	{
		if (target == null)
		{
			throw new ApplicationException("Objective point change requires a target");
		}
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (objectivePoints)
		{
			Team team = target.GetTeam();
			objectivePoints.AdjustPoints(finalChange, team);
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
