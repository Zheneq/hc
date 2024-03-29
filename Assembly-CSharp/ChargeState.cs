using UnityEngine;
using UnityEngine.Networking;

public class ChargeState : MoveState
{
	private bool m_playedEnd;

	private float m_moveSpeed;

	public ChargeState(ActorMovement owner, BoardSquarePathInfo aesheticPath)
		: base(owner, aesheticPath)
	{
		base.stateName = "Charge";
		if (aesheticPath.segmentMovementSpeed > 0f)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_moveSpeed = aesheticPath.segmentMovementSpeed;
					return;
				}
			}
		}
		if (!(aesheticPath.segmentMovementDuration > 0f))
		{
			return;
		}
		while (true)
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
				m_moveSpeed = num;
			}
			return;
		}
	}

	public bool DoneMoving()
	{
		return m_done;
	}

	protected override void UpdateState()
	{
		if (m_ownerActorData.GetActorModelData() != null)
		{
			if (m_ownerActorData.GetActorModelData().IsPlayingChargeEnd())
			{
				BoardSquare square = m_pathSquareInfo.square;
				Vector3 worldPosition = square.GetOccupantRefPos();
				worldPosition.y = Board.Get().BaselineHeight;
				m_owner.m_actor.SetTransformPositionToVector(worldPosition);
				if (m_pathSquareInfo.next != null)
				{
					if (m_pathSquareInfo.chargeEndType == BoardSquarePathInfo.ChargeEndType.Pivot)
					{
						Vector3 dir = m_pathSquareInfo.next.square.ToVector3() - worldPosition;
						dir.y = 0f;
						if (dir.magnitude > 0f)
						{
							dir.Normalize();
							m_owner.m_actor.TurnToDirection(dir);
						}
					}
				}
				m_playedEnd = true;
				return;
			}
		}
		if (m_playedEnd)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_updatePath = true;
					m_playedEnd = false;
					m_done = true;
					return;
				}
			}
		}
		BoardSquare square2 = m_pathSquareInfo.square;
		Vector3 position = m_owner.m_actor.transform.position;
		Vector3 worldPosition2 = square2.GetOccupantRefPos();
		worldPosition2.y = Board.Get().BaselineHeight;
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
		float num2 = m_moveSpeed * num;
		float num3 = num2 * num2;
		if (sqrMagnitude > num3)
		{
			vector.Normalize();
			m_owner.m_actor.SetTransformPositionToVector(position + vector * num2);
			Vector3 dir2 = vector;
			if (m_pathSquareInfo.m_reverse)
			{
				dir2 = -vector;
			}
			float num4 = Board.Get().squareSize * Board.Get().squareSize;
			if (m_owner.m_actor.GetFacingDirectionAfterMovement() != Vector3.zero)
			{
				if (m_playedEnd || sqrMagnitude < num4)
				{
					dir2 = m_owner.m_actor.GetFacingDirectionAfterMovement();
				}
			}
			dir2.y = 0f;
			if (!(dir2.magnitude > 0.01f))
			{
				return;
			}
			while (true)
			{
				m_owner.m_actor.TurnToDirection(dir2);
				return;
			}
		}
		if (m_pathSquareInfo.chargeEndType != BoardSquarePathInfo.ChargeEndType.None)
		{
			if (m_pathSquareInfo.next != null)
			{
				m_owner.m_actor.SetTransformPositionToVector(worldPosition2);
				if (NetworkClient.active)
				{
					return;
				}
				while (true)
				{
					if (m_owner.m_actor.GetActorModelData() == null)
					{
						m_updatePath = true;
						m_done = true;
					}
					return;
				}
			}
		}
		m_updatePath = true;
		m_owner.m_actor.SetTransformPositionToVector(worldPosition2);
		Vector3 dir3 = vector;
		if (m_owner.m_actor.GetFacingDirectionAfterMovement() != Vector3.zero)
		{
			dir3 = m_owner.m_actor.GetFacingDirectionAfterMovement();
			dir3.y = 0f;
			if (dir3.magnitude > 0.01f)
			{
				m_owner.m_actor.TurnToDirection(dir3);
			}
		}
		m_done = true;
	}
}
