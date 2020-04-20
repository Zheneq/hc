using System;
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

	public KnockbackState(ActorMovement owner, BoardSquarePathInfo aesheticPath) : base(owner, aesheticPath)
	{
		base.stateName = "Knockback";
		if (KnockbackState.m_animHashKnockbackEnd == 0)
		{
			KnockbackState.m_animHashKnockbackEnd = Animator.StringToHash("Base Layer.Reaction.Knockback.KnockbackEnd");
		}
		if (KnockbackState.m_tagHashKnockdown == 0)
		{
			KnockbackState.m_tagHashKnockdown = Animator.StringToHash("Knockdown");
		}
		if (KnockbackState.m_tagHashKnockdownImpact == 0)
		{
			KnockbackState.m_tagHashKnockdownImpact = Animator.StringToHash("KnockdownImpact");
		}
		float magnitude = (this.m_pathSquareInfo.square.GetWorldPosition() - this.m_owner.m_actor.transform.position).magnitude;
		this.m_travelTime = magnitude / this.m_ownerActorData.m_knockbackSpeed;
		if (this.m_travelTime == 0f)
		{
			this.m_travelTime = KnockbackState.s_minTravelTime;
		}
		this.m_initialYVelocity = -0.5f * KnockbackState.s_gravity * this.m_travelTime;
		this.m_startTime = Time.time;
		if (!NetworkClient.active && this.m_owner.m_actor.GetModelAnimator() == null)
		{
			this.m_startedExitAnim = true;
			this.m_endedExitAnim = true;
		}
	}

	protected override void OnAnimChange(int prevAnimHash, int prevAnimTag, int nextAnimHash, int nextAnimTag)
	{
		if (nextAnimHash != KnockbackState.m_animHashKnockbackEnd)
		{
			if (nextAnimTag != KnockbackState.m_animHashKnockbackEnd)
			{
				if (nextAnimTag != KnockbackState.m_tagHashKnockdown)
				{
					goto IL_48;
				}
			}
		}
		this.m_startedExitAnim = true;
		IL_48:
		if (prevAnimHash != KnockbackState.m_animHashKnockbackEnd)
		{
			if (prevAnimTag != KnockbackState.m_tagHashKnockdownImpact)
			{
				return;
			}
		}
		this.m_endedExitAnim = true;
	}

	protected override void UpdateState()
	{
		BoardSquare square = this.m_pathSquareInfo.square;
		Vector3 position = this.m_owner.m_actor.transform.position;
		Vector3 worldPosition = square.GetWorldPosition();
		Vector3 a = worldPosition - position;
		a.y = 0f;
		bool flag = false;
		if (a.magnitude < 0.01f)
		{
			a = this.m_owner.transform.forward;
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
		float num3 = this.m_ownerActorData.m_knockbackSpeed * Time.deltaTime;
		float num4 = num3 * num3;
		float num5 = Time.time - this.m_startTime;
		float num6 = Mathf.Max(0f, this.m_initialYVelocity * num5 + 0.5f * KnockbackState.s_gravity * num5 * num5);
		if (this.m_startedExitAnim)
		{
			this.m_updatePath = true;
		}
		if (num2 <= num4)
		{
			if (num5 >= this.m_travelTime)
			{
				if (this.m_startedExitAnim)
				{
					this.m_updatePath = true;
				}
				if (this.m_owner.m_actor.IsModelAnimatorDisabled())
				{
					this.m_updatePath = true;
					this.m_done = true;
				}
				else
				{
					this.m_owner.m_actor.SetTransformPositionToVector(worldPosition);
				}
				if (!this.m_endedExitAnim)
				{
					if (!base.IsOnLastSegment())
					{
						return;
					}
				}
				this.m_updatePath = true;
				this.m_done = true;
				return;
			}
		}
		a.Normalize();
		float d = 0f;
		if (!flag)
		{
			d = Mathf.Min(num3, num2);
		}
		Vector3 groundPosition = this.m_owner.GetGroundPosition(position + a * d);
		groundPosition.y = (float)Board.Get().m_baselineHeight + num6;
		if (this.m_owner.m_actor.IsModelAnimatorDisabled())
		{
			this.m_updatePath = true;
			this.m_done = true;
		}
		else
		{
			this.m_owner.m_actor.SetTransformPositionToVector(groundPosition);
		}
		Vector3 dir = -a;
		dir.y = 0f;
		if (dir.magnitude > 0.01f && !flag)
		{
			if (!this.m_owner.m_actor.IsModelAnimatorDisabled())
			{
				this.m_owner.m_actor.TurnToDirection(dir);
			}
		}
	}
}
