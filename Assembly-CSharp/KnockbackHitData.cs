using UnityEngine;

public class KnockbackHitData
{
	public ActorData m_target;

	public ActorData m_sourceActor;

	public KnockbackType m_type;

	public Vector3 m_aimDir;

	public Vector3 m_sourcePos;

	public float m_distance;

	public KnockbackHitData(ActorData target, ActorData sourceActor, KnockbackType type, Vector3 aimDir, Vector3 sourcePos, float distance)
	{
		m_target = target;
		m_sourceActor = sourceActor;
		m_type = type;
		m_aimDir = aimDir;
		m_sourcePos = sourcePos;
		m_distance = distance;
	}
}
