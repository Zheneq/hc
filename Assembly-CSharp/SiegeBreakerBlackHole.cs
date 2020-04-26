public class SiegeBreakerBlackHole : Ability
{
	public int m_knockbackDistance = 4;

	public float m_radius = 3f;

	public bool m_penetrateLineOfSight = true;

	private void Start()
	{
		if (!(m_abilityName == "Base Ability"))
		{
			return;
		}
		while (true)
		{
			m_abilityName = "Black Hole";
			return;
		}
	}
}
