public class Laser : Ability
{
	public bool m_penetrateLineOfSight;

	public int m_damageAmount = 15;

	public float m_width = 1f;

	public int m_distance = 15;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Laser";
		}
		base.Targeter = new AbilityUtil_Targeter_Laser(this, m_width, m_distance, m_penetrateLineOfSight);
	}
}
