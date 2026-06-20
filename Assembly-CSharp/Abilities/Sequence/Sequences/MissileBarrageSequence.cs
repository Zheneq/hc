using UnityEngine;

public class MissileBarrageSequence : SplineProjectileSequence
{
	public float m_initialForwardOffset = 8f;

	public float m_initialHeightOffset = 5f;

	public float m_angleRange = 180f;

	public override void FinishSetup()
	{
		base.FinishSetup();
		m_doHitsAsProjectileTravels = false;
	}

	internal override Vector3[] GetSplinePath(int curIndex, int maxIndex)
	{
		Quaternion quaternion = default(Quaternion);
		quaternion = m_fxJoint.m_jointObject.transform.rotation;
		Vector3 position = m_fxJoint.m_jointObject.transform.position;
		Vector3 vector = quaternion * Vector3.forward;
		Vector3 vector2 = vector;
		vector2.Normalize();
		vector2 *= m_initialForwardOffset;
		vector2.y = m_initialHeightOffset;
		float angle = m_angleRange * ((float)curIndex - 0.5f * (float)maxIndex) / (float)maxIndex;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
		vector2 = rotation * vector2;
		Vector3 vector3 = position + vector2;
		Vector3 targetHitPosition = GetTargetHitPosition(0, m_hitPosJoint);
		return new Vector3[5]
		{
			position - vector2,
			position,
			vector3,
			targetHitPosition,
			targetHitPosition + (targetHitPosition - vector3)
		};
	}
}
