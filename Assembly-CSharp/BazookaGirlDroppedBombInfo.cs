using System;

[Serializable]
public class BazookaGirlDroppedBombInfo
{
	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three;

	public int m_damageAmount = 5;

	public int m_subsequentDamageAmount = 1;

	public StandardEffectInfo m_enemyHitEffect;

	public bool m_penetrateLos;

	public BazookaGirlDroppedBombInfo GetShallowCopy()
	{
		return (BazookaGirlDroppedBombInfo)base.MemberwiseClone();
	}
}
