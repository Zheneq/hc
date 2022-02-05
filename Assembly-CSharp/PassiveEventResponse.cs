// ROGUES
// SERVER
using System;

[Serializable]
public class PassiveEventResponse
{
	public int m_healthBonus;
	public int m_techPointsBonus;
	// TODO LOW apply credit bonus
	// removed in rogues
	public int m_personalCreditsBonus;
	public int m_mechanicPointAdjust;
	public PassiveActionType[] m_actions;
	public StandardEffectInfo m_effect;

	// added in rogues
#if SERVER
	public void ApplyResponse(ActorData responseTarget, Passive source)
	{
		ActorData owner = source.Owner;
		if (this.m_healthBonus != 0)
		{
			ServerCombatManager.Get().Heal(source, owner, responseTarget, this.m_healthBonus, ServerCombatManager.HealingType.Ability);
		}
		if (this.m_techPointsBonus != 0)
		{
			ServerCombatManager.Get().TechPointGain(source, owner, responseTarget, this.m_techPointsBonus, ServerCombatManager.TechPointChangeType.Ability);
		}
		if (this.m_mechanicPointAdjust != 0)
		{
			owner.MechanicPoints += this.m_mechanicPointAdjust;
		}
		foreach (PassiveActionType passiveActionType in this.m_actions)
		{
			AbilityData abilityData = responseTarget.GetAbilityData();
			switch (passiveActionType)
			{
			case PassiveActionType.AdvanceCharacterCooldowns:
				abilityData.ProgressCharacterAbilityCooldowns();
				break;
			case PassiveActionType.ClearCharacterCooldowns:
				abilityData.ClearCharacterAbilityCooldowns();
				break;
			case PassiveActionType.ClearMechanicPoints:
				owner.MechanicPoints = 0;
				break;
			}
		}
		if (this.m_effect.m_applyEffect)
		{
			StandardActorEffect effect = new StandardActorEffect(source.AsEffectSource(), responseTarget.GetCurrentBoardSquare(), responseTarget, owner, this.m_effect.m_effectData);
			ServerEffectManager.Get().ApplyEffect(effect, 1);
		}
	}
#endif
}
