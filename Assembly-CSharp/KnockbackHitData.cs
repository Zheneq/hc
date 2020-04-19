using System;
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
		this.m_target = target;
		this.m_sourceActor = sourceActor;
		this.m_type = type;
		this.m_aimDir = aimDir;
		this.m_sourcePos = sourcePos;
		this.m_distance = distance;
	}
}
