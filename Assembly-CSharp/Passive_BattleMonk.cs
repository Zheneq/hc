public class Passive_BattleMonk : Passive
{
	public StandardEffectInfo m_effectOnOwner_nearAllies;

	public StandardEffectInfo m_effectOnOwner_nearEnemies;

	public StandardEffectInfo m_effectOnAllies;

	public StandardEffectInfo m_effectOnEnemies;

	public AbilityAreaShape m_alliesShape;

	public AbilityAreaShape m_enemiesShape;

	public bool m_penetrateLosForAllies;

	public bool m_penetrateLosForEnemies;

	public BattleMonkSelfBuff m_buffAbility;

	public BattleMonkBoundingLeap m_chargeAbility;

	public int m_chargeLastCastTurn = -1;

	public int m_buffLastCastTurn = -1;

	public int m_damagedThisTurn;
}
