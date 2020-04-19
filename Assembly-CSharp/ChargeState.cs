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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ChargeState..ctor(ActorMovement, BoardSquarePathInfo)).MethodHandle;
			}
			this.m_moveSpeed = aesheticPath.segmentMovementSpeed;
		}
		else if (aesheticPath.segmentMovementDuration > 0f)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			BoardSquarePathInfo prev = aesheticPath.prev;
			Vector3 a;
			if (prev == null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
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
		if (this.m_ownerActorData.\u000E() != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ChargeState.UpdateState()).MethodHandle;
			}
			if (this.m_ownerActorData.\u000E().IsPlayingChargeEnd())
			{
				BoardSquare square = this.m_pathSquareInfo.square;
				Vector3 vector = square.\u001D();
				vector.y = (float)Board.\u000E().BaselineHeight;
				this.m_owner.m_actor.SetTransformPositionToVector(vector);
				if (this.m_pathSquareInfo.next != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_pathSquareInfo.chargeEndType == BoardSquarePathInfo.ChargeEndType.Pivot)
					{
						Vector3 dir = this.m_pathSquareInfo.next.square.ToVector3() - vector;
						dir.y = 0f;
						if (dir.magnitude > 0f)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_updatePath = true;
			this.m_playedEnd = false;
			this.m_done = true;
		}
		else
		{
			BoardSquare square2 = this.m_pathSquareInfo.square;
			Vector3 position = this.m_owner.m_actor.transform.position;
			Vector3 vector2 = square2.\u001D();
			vector2.y = (float)Board.\u000E().BaselineHeight;
			Vector3 vector3 = vector2 - position;
			vector3.y = 0f;
			float sqrMagnitude = vector3.sqrMagnitude;
			float num = Time.deltaTime;
			if (Application.isEditor)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (num > 0.04f)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (num < 0.08f)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						num = 0.04f;
					}
				}
			}
			float num2 = this.m_moveSpeed * num;
			float num3 = num2 * num2;
			if (sqrMagnitude > num3)
			{
				vector3.Normalize();
				this.m_owner.m_actor.SetTransformPositionToVector(position + vector3 * num2);
				Vector3 dir2 = vector3;
				if (this.m_pathSquareInfo.m_reverse)
				{
					dir2 = -vector3;
				}
				float num4 = Board.\u000E().squareSize * Board.\u000E().squareSize;
				if (this.m_owner.m_actor.\u0013() != Vector3.zero)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_playedEnd || sqrMagnitude < num4)
					{
						dir2 = this.m_owner.m_actor.\u0013();
					}
				}
				dir2.y = 0f;
				if (dir2.magnitude > 0.01f)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_owner.m_actor.TurnToDirection(dir2);
				}
			}
			else
			{
				if (this.m_pathSquareInfo.chargeEndType != BoardSquarePathInfo.ChargeEndType.None)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_pathSquareInfo.next != null)
					{
						this.m_owner.m_actor.SetTransformPositionToVector(vector2);
						if (NetworkClient.active)
						{
							return;
						}
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_owner.m_actor.\u000E() == null)
						{
							this.m_updatePath = true;
							this.m_done = true;
							return;
						}
						return;
					}
				}
				this.m_updatePath = true;
				this.m_owner.m_actor.SetTransformPositionToVector(vector2);
				if (this.m_owner.m_actor.\u0013() != Vector3.zero)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					Vector3 dir3 = this.m_owner.m_actor.\u0013();
					dir3.y = 0f;
					if (dir3.magnitude > 0.01f)
					{
						this.m_owner.m_actor.TurnToDirection(dir3);
					}
				}
				this.m_done = true;
			}
		}
	}
}
