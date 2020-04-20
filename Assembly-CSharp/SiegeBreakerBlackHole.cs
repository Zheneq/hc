using System;

public class SiegeBreakerBlackHole : Ability
{
	public int m_knockbackDistance = 4;

	public float m_radius = 3f;

	public bool m_penetrateLineOfSight = true;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Black Hole";
		}
	}
}
