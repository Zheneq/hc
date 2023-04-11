public class Passive_Valkyrie : Passive
{
	private Valkyrie_SyncComponent m_syncComp;
	private ValkyrieGuard m_guardAbility;
	private ValkyrieDashAoE m_dashAbility;
	private ValkyriePullToLaserCenter m_ultAbility;
	private bool m_tookDamageThisTurn;
	private bool m_guardIsUp;
	private int m_lastUltCastTurn = -1;
	private int m_lastGuardCastTurn = -1;

	public int DamageThroughGuardCoverThisTurn { get; private set; }
}
