using System;
using UnityEngine;

public class MissileBarrageSequence : SplineProjectileSequence
{
	public float m_initialForwardOffset = 8f;

	public float m_initialHeightOffset = 5f;

	public float m_angleRange = 180f;

	public override void FinishSetup()
	{
		base.FinishSetup();
		this.m_doHitsAsProjectileTravels = false;
	}

	internal override Vector3[] GetSplinePath(int curIndex, int maxIndex)
	{
		Quaternion rotation = default(Quaternion);
		rotation = this.m_fxJoint.m_jointObject.transform.rotation;
		Vector3 position = this.m_fxJoint.m_jointObject.transform.position;
		Vector3 vector = rotation * Vector3.forward;
		Vector3 vector2 = vector;
		vector2.Normalize();
		vector2 *= this.m_initialForwardOffset;
		vector2.y = this.m_initialHeightOffset;
		float angle = this.m_angleRange * ((float)curIndex - 0.5f * (float)maxIndex) / (float)maxIndex;
		Quaternion rotation2 = Quaternion.AngleAxis(angle, Vector3.up);
		vector2 = rotation2 * vector2;
		Vector3 vector3 = position + vector2;
		Vector3 targetHitPosition = base.GetTargetHitPosition(0, this.m_hitPosJoint);
		return new Vector3[]
		{
			position - vector2,
			position,
			vector3,
			targetHitPosition,
			targetHitPosition + (targetHitPosition - vector3)
		};
	}
}
