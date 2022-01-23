using UnityEngine;
using UnityEngine.Networking;

public class KnockbackState : MoveState
{
	private bool m_endedExitAnim;

	private bool m_startedExitAnim;

	private float m_initialYVelocity;

	private float m_startTime;

	private float m_travelTime;

	private static int m_animHashKnockbackEnd;

	private static int m_tagHashKnockdown;

	private static int m_tagHashKnockdownImpact;

	private static float s_minTravelTime = 0.6f;

	private static float s_gravity = -20f;

	public KnockbackState(ActorMovement owner, BoardSquarePathInfo aesheticPath)
		: base(owner, aesheticPath)
	{
		base.stateName = "Knockback";
		if (m_animHashKnockbackEnd == 0)
		{
			m_animHashKnockbackEnd = Animator.StringToHash("Base Layer.Reaction.Knockback.KnockbackEnd");
		}
		if (m_tagHashKnockdown == 0)
		{
			m_tagHashKnockdown = Animator.StringToHash("Knockdown");
		}
		if (m_tagHashKnockdownImpact == 0)
		{
			m_tagHashKnockdownImpact = Animator.StringToHash("KnockdownImpact");
		}
		float magnitude = (m_pathSquareInfo.square.GetOccupantRefPos() - m_owner.m_actor.transform.position).magnitude;
		m_travelTime = magnitude / m_ownerActorData.m_knockbackSpeed;
		if (m_travelTime == 0f)
		{
			m_travelTime = s_minTravelTime;
		}
		m_initialYVelocity = -0.5f * s_gravity * m_travelTime;
		m_startTime = Time.time;
		if (!NetworkClient.active && m_owner.m_actor.GetModelAnimator() == null)
		{
			m_startedExitAnim = true;
			m_endedExitAnim = true;
		}
	}

	protected override void OnAnimChange(int prevAnimHash, int prevAnimTag, int nextAnimHash, int nextAnimTag)
	{
		if (nextAnimHash != m_animHashKnockbackEnd)
		{
			if (nextAnimTag != m_animHashKnockbackEnd)
			{
				if (nextAnimTag != m_tagHashKnockdown)
				{
					goto IL_0048;
				}
			}
		}
		m_startedExitAnim = true;
		goto IL_0048;
		IL_0048:
		if (prevAnimHash != m_animHashKnockbackEnd)
		{
			if (prevAnimTag != m_tagHashKnockdownImpact)
			{
				return;
			}
		}
		m_endedExitAnim = true;
	}

	protected override void UpdateState()
	{
		BoardSquare square = m_pathSquareInfo.square;
		Vector3 position = m_owner.m_actor.transform.position;
		Vector3 worldPosition = square.GetOccupantRefPos();
		Vector3 a = worldPosition - position;
		a.y = 0f;
		bool flag = false;
		if (a.magnitude < 0.01f)
		{
			a = m_owner.transform.forward;
			flag = true;
		}
		float num;
		if (flag)
		{
			num = 0f;
		}
		else
		{
			num = a.sqrMagnitude;
		}
		float num2 = num;
		float num3 = m_ownerActorData.m_knockbackSpeed * Time.deltaTime;
		float num4 = num3 * num3;
		float num5 = Time.time - m_startTime;
		float num6 = Mathf.Max(0f, m_initialYVelocity * num5 + 0.5f * s_gravity * num5 * num5);
		if (m_startedExitAnim)
		{
			m_updatePath = true;
		}
		if (!(num2 > num4))
		{
			if (!(num5 < m_travelTime))
			{
				if (m_startedExitAnim)
				{
					m_updatePath = true;
				}
				if (m_owner.m_actor.IsInRagdoll())
				{
					m_updatePath = true;
					m_done = true;
				}
				else
				{
					m_owner.m_actor.SetTransformPositionToVector(worldPosition);
				}
				if (!m_endedExitAnim)
				{
					if (!IsOnLastSegment())
					{
						return;
					}
				}
				m_updatePath = true;
				m_done = true;
				return;
			}
		}
		a.Normalize();
		float d = 0f;
		if (!flag)
		{
			d = Mathf.Min(num3, num2);
		}
		Vector3 groundPosition = m_owner.GetGroundPosition(position + a * d);
		groundPosition.y = (float)Board.Get().m_baselineHeight + num6;
		if (m_owner.m_actor.IsInRagdoll())
		{
			m_updatePath = true;
			m_done = true;
		}
		else
		{
			m_owner.m_actor.SetTransformPositionToVector(groundPosition);
		}
		Vector3 dir = -a;
		dir.y = 0f;
		if (!(dir.magnitude > 0.01f) || flag)
		{
			return;
		}
		while (true)
		{
			if (!m_owner.m_actor.IsInRagdoll())
			{
				while (true)
				{
					m_owner.m_actor.TurnToDirection(dir);
					return;
				}
			}
			return;
		}
	}
}
