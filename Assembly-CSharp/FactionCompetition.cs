using System;
using System.Collections.Generic;

[Serializable]
public class FactionCompetition
{
	public string InternalName;

	public bool Enabled;

	public bool ShouldShowcase;

	public string StartTime;

	public string EndTime;

	public List<Faction> Factions;

	public FactionRewards RewardsOnLogin;

	public AccountComponent.UIStateIdentifier UIToDisplayOnLogin = AccountComponent.UIStateIdentifier.NONE;
}
