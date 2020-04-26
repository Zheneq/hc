public class StandardPowerUpAbilityModData
{
	public AbilityModPropertyInt m_healMod;

	public AbilityModPropertyInt m_techPointMod;

	public int m_extraHealIfDirectHit;

	public int m_extraTechPointIfDirectHit;

	public StandardPowerUpAbilityModData()
	{
		m_healMod = new AbilityModPropertyInt();
		m_techPointMod = new AbilityModPropertyInt();
		m_extraHealIfDirectHit = 0;
		m_extraTechPointIfDirectHit = 0;
	}
}
