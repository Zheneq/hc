using UnityEngine;
using UnityEngine.Networking;

public class RunState : MoveState
{
	private bool m_startedExitAnim;

	private bool m_startedRunAnim;

	private static int m_animHashRunRun;

	private static int m_animHashMovementRun;

	private static int m_animHashRunStopping;

	private static int m_animHashMovementRunEnd;

	private static int m_animTagHashIdle;

	public RunState(ActorMovement owner, BoardSquarePathInfo aesheticPath)
		: base(owner, aesheticPath)
	{
		base.stateName = "Run";
		if (m_animHashRunRun == 0)
		{
			m_animHashRunRun = Animator.StringToHash("Base Layer.Run.Run");
			m_animHashMovementRun = Animator.StringToHash("Base Layer.Movement.Run");
			m_animHashRunStopping = Animator.StringToHash("Base Layer.Run.Stopping");
			m_animHashMovementRunEnd = Animator.StringToHash("Base Layer.Movement.Run_End");
			m_animTagHashIdle = Animator.StringToHash("Idle");
		}
		if (m_animator != null)
		{
			if (m_animator.layerCount > 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (IsRunStartHash(m_animator.GetCurrentAnimatorStateInfo(0).fullPathHash))
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									m_startedRunAnim = true;
									return;
								}
							}
						}
						return;
					}
				}
			}
		}
		if (NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			m_startedRunAnim = true;
			m_startedExitAnim = true;
			return;
		}
	}

	private bool IsRunStartHash(int hash)
	{
		int result;
		if (hash != m_animHashRunRun)
		{
			result = ((hash == m_animHashMovementRun) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool IsRunStopHash(int hash)
	{
		int result;
		if (hash != m_animHashRunStopping)
		{
			result = ((hash == m_animHashMovementRunEnd) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	protected override void OnAnimChange(int prevAnimHash, int prevAnimTag, int nextAnimHash, int nextAnimTag)
	{
		if (IsRunStartHash(nextAnimHash))
		{
			m_startedRunAnim = true;
		}
		if (IsRunStopHash(nextAnimHash))
		{
			m_startedExitAnim = true;
		}
		if (nextAnimTag == m_animTagHashIdle)
		{
			m_startedExitAnim = true;
		}
	}

	protected override void UpdateState()
	{
		BoardSquare square = m_pathSquareInfo.square;
		Vector3 position = m_owner.m_actor.transform.position;
		Vector3 worldPosition = square.GetOccupantRefPos();
		Vector3 vector = worldPosition - position;
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
		float num2;
		if (m_pathSquareInfo.connectionType == BoardSquarePathInfo.ConnectionType.Vault)
		{
			num2 = m_ownerActorData.m_vaultSpeed * num;
		}
		else
		{
			num2 = m_ownerActorData.m_runSpeed * num;
		}
		float num3 = num2;
		float num4 = num3 * num3;
		if (sqrMagnitude > num4)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					vector.Normalize();
					Vector3 transformPositionToVector = position + vector * num3;
					transformPositionToVector.y = Board.Get().BaselineHeight;
					m_owner.m_actor.SetTransformPositionToVector(transformPositionToVector);
					Vector3 dir = vector;
					dir.y = 0f;
					if (dir.magnitude > 0.01f)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								m_owner.m_actor.TurnToDirection(dir);
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		m_updatePath = true;
		m_owner.m_actor.SetTransformPositionToVector(m_owner.GetGroundPosition(worldPosition));
		m_done = true;
	}
}
