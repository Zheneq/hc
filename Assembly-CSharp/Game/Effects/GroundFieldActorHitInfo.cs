// ROGUES
// SERVER
#if SERVER
// added in rogues
public class GroundFieldActorHitInfo
{
	public ActorData m_hitActor;
	public int m_hitCount;
	public int m_damage;
	public int m_healing;
	public int m_energyGain;
	public StandardEffectInfo m_effectInfo;

	public GroundFieldActorHitInfo(ActorData hitActor)
	{
		m_hitActor = hitActor;
	}
}
#endif
