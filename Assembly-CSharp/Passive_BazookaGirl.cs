// ROGUES
// SERVER
using System.Collections.Generic;

public class Passive_BazookaGirl : Passive
{
#if SERVER
	private BazookaGirlRocketJump m_rocketJumpAbility;

	protected override void OnStartup()
	{
		m_rocketJumpAbility = Owner.GetAbilityData().GetAbilityOfType(typeof(BazookaGirlRocketJump)) as BazookaGirlRocketJump;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		HandleRocketJumpCooldownResetOnTurnStart();
	}

	private void HandleRocketJumpCooldownResetOnTurnStart()
	{
		List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(Owner.GetEnemyTeam());
		if (m_rocketJumpAbility != null
		    && m_rocketJumpAbility.ResetCooldownOnKill()
		    && playerAndBotTeamMembers != null)
		{
			bool flag = false;
			ActorBehavior.TurnBehavior turnBehavior = Owner.GetActorBehavior()
				? Owner.GetActorBehavior().GetBehaviorOfTurn(GameFlowData.Get().CurrentTurn - 1)
				: null;
			if (turnBehavior != null)
			{
				foreach (ActorData actorData in playerAndBotTeamMembers)
				{
					if (GameplayUtils.IsPlayerControlled(actorData)
					    && actorData.IsDead()
					    && turnBehavior.DamagedSpecificActor(actorData))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				AbilityData.ActionType actionTypeOfAbility = Owner.GetAbilityData().GetActionTypeOfAbility(m_rocketJumpAbility);
				Owner.GetAbilityData().OverrideCooldown(actionTypeOfAbility, 0);
			}
		}
	}
#endif
}
