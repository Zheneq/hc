public class MinionAttack : Ability
{
	public int m_damageVsMinions = 50;

	public int m_damageVsPlayers = 50;

	public int m_damageVsOthers = 50;

	private void Start()
	{
		m_abilityName = "Minion Cannon";
	}
}
