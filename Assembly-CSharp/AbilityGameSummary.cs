using System;

[Serializable]
public class AbilityGameSummary
{
	public int ActionType;

	public string AbilityClassName;

	public string AbilityName;

	public string ModName;

	public int CastCount;

	public int TauntCount;

	public int TotalTargetsHit;

	public int TotalDamage;

	public int TotalHealing;

	public int TotalAbsorb;

	public int TotalPotentialAbsorb;

	public int TotalEnergyGainOnSelf;

	public int TotalEnergyGainToOthers;

	public int TotalEnergyLossToOthers;
}
