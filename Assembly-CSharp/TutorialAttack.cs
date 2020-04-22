public class TutorialAttack : Ability
{
	public float m_width = 1f;

	public float m_distance = 15f;

	public bool m_penetrateLineOfSight;

	public int m_damageAmount = 10;

	public int m_knockbackDistance = 3;

	public bool m_knockback = true;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Laser(this, m_width, m_distance, m_penetrateLineOfSight);
	}
}
