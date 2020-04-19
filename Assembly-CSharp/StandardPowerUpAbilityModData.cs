using System;

public class StandardPowerUpAbilityModData
{
	public AbilityModPropertyInt m_healMod;

	public AbilityModPropertyInt m_techPointMod;

	public int m_extraHealIfDirectHit;

	public int m_extraTechPointIfDirectHit;

	public StandardPowerUpAbilityModData()
	{
		this.m_healMod = new AbilityModPropertyInt();
		this.m_techPointMod = new AbilityModPropertyInt();
		this.m_extraHealIfDirectHit = 0;
		this.m_extraTechPointIfDirectHit = 0;
	}
}
