// ROGUES
// SERVER

using System.Collections.Generic;

// empty in reactor
public class Passive_Thief : Passive
{
	#if SERVER
	// added in rogues
	private List<ActorData> m_alliesVisibleAtStartOfCombat = new List<ActorData>();

	// added in rogues
	public List<ActorData> GetAlliesVisibleAtStartOfCombat()
	{
		return m_alliesVisibleAtStartOfCombat;
	}

	// added in rogues
	public void OnHidAlly(ActorData allyActor)
	{
		m_alliesVisibleAtStartOfCombat.Remove(allyActor);
	}

	// added in rogues
	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		if (phase != AbilityPriority.Combat_Damage)
		{
			return;
		}
		m_alliesVisibleAtStartOfCombat.Clear();
		foreach (ActorData actorData in GameFlowData.Get().GetAllTeamMembers(Owner.GetTeam()))
		{
			if (GameplayUtils.IsPlayerControlled(actorData) && actorData.IsActorVisibleToAnyEnemy())
			{
				m_alliesVisibleAtStartOfCombat.Add(actorData);
			}
		}
	}
#endif
}
