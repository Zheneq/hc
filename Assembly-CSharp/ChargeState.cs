using System;
using UnityEngine;
using UnityEngine.Networking;

public class ChargeState : MoveState
{
	private bool m_playedEnd;

	private float m_moveSpeed;

	public ChargeState(ActorMovement owner, BoardSquarePathInfo aesheticPath) : base(owner, aesheticPath)
	{
		base.stateName = "Charge";
		if (aesheticPath.segmentMovementSpeed > 0f)
		{
			this.m_moveSpeed = aesheticPath.segmentMovementSpeed;
		}
		else if (aesheticPath.segmentMovementDuration > 0f)
		{
			BoardSquarePathInfo prev = aesheticPath.prev;
			Vector3 a;
			if (prev == null)
			{
				a = owner.m_actor.transform.position;
			}
			else
			{
				a = prev.square.ToVector3();
			}
			Vector3 vector = a - aesheticPath.square.ToVector3();
			vector.y = 0f;
			float num = vector.magnitude / aesheticPath.segmentMovementDuration;
			if (num >= 1f)
			{
				this.m_moveSpeed = num;
			}
		}
	}

	public bool DoneMoving()
	{
		return this.m_done;
	}

	protected override void UpdateState()
	{
		if (this.m_ownerActorData.GetActorModelData() != null)
		{
			if (this.m_ownerActorData.GetActorModelData().IsPlayingChargeEnd())
			{
				BoardSquare square = this.m_pathSquareInfo.square;
				Vector3 worldPosition = square.GetWorldPosition();
				worldPosition.y = (float)Board.Get().BaselineHeight;
				this.m_owner.m_actor.SetTransformPositionToVector(worldPosition);
				if (this.m_pathSquareInfo.next != null)
				{
					if (this.m_pathSquareInfo.chargeEndType == BoardSquarePathInfo.ChargeEndType.Pivot)
					{
						Vector3 dir = this.m_pathSquareInfo.next.square.ToVector3() - worldPosition;
						dir.y = 0f;
						if (dir.magnitude > 0f)
						{
							dir.Normalize();
							this.m_owner.m_actor.TurnToDirection(dir);
						}
					}
				}
				this.m_playedEnd = true;
				return;
			}
		}
		if (this.m_playedEnd)
		{
			this.m_updatePath = true;
			this.m_playedEnd = false;
			this.m_done = true;
		}
		else
		{
			BoardSquare square2 = this.m_pathSquareInfo.square;
			Vector3 position = this.m_owner.m_actor.transform.position;
			Vector3 worldPosition2 = square2.GetWorldPosition();
			worldPosition2.y = (float)Board.Get().BaselineHeight;
			Vector3 vector = worldPosition2 - position;
			vector.y = 0f;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = Time.deltaTime;
			if (Application.isEditor)
			{
				if (num > 0.04f)
				{
					if (num < 0.08f)
					{
						num = 0.04f;
					}
				}
			}
			float num2 = this.m_moveSpeed * num;
			float num3 = num2 * num2;
			if (sqrMagnitude > num3)
			{
				vector.Normalize();
				this.m_owner.m_actor.SetTransformPositionToVector(position + vector * num2);
				Vector3 dir2 = vector;
				if (this.m_pathSquareInfo.m_reverse)
				{
					dir2 = -vector;
				}
				float num4 = Board.Get().squareSize * Board.Get().squareSize;
				if (this.m_owner.m_actor.GetFacingDirAfterMovement() != Vector3.zero)
				{
					if (this.m_playedEnd || sqrMagnitude < num4)
					{
						dir2 = this.m_owner.m_actor.GetFacingDirAfterMovement();
					}
				}
				dir2.y = 0f;
				if (dir2.magnitude > 0.01f)
				{
					this.m_owner.m_actor.TurnToDirection(dir2);
				}
			}
			else
			{
				if (this.m_pathSquareInfo.chargeEndType != BoardSquarePathInfo.ChargeEndType.None)
				{
					if (this.m_pathSquareInfo.next != null)
					{
						this.m_owner.m_actor.SetTransformPositionToVector(worldPosition2);
						if (NetworkClient.active)
						{
							return;
						}
						if (this.m_owner.m_actor.GetActorModelData() == null)
						{
							this.m_updatePath = true;
							this.m_done = true;
							return;
						}
						return;
					}
				}
				this.m_updatePath = true;
				this.m_owner.m_actor.SetTransformPositionToVector(worldPosition2);
				if (this.m_owner.m_actor.GetFacingDirAfterMovement() != Vector3.zero)
				{
					Vector3 facingDirAfterMovement = this.m_owner.m_actor.GetFacingDirAfterMovement();
					facingDirAfterMovement.y = 0f;
					if (facingDirAfterMovement.magnitude > 0.01f)
					{
						this.m_owner.m_actor.TurnToDirection(facingDirAfterMovement);
					}
				}
				this.m_done = true;
			}
		}
	}
}
