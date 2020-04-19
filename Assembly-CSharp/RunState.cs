using System;
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

	public RunState(ActorMovement owner, BoardSquarePathInfo aesheticPath) : base(owner, aesheticPath)
	{
		base.stateName = "Run";
		if (RunState.m_animHashRunRun == 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RunState..ctor(ActorMovement, BoardSquarePathInfo)).MethodHandle;
			}
			RunState.m_animHashRunRun = Animator.StringToHash("Base Layer.Run.Run");
			RunState.m_animHashMovementRun = Animator.StringToHash("Base Layer.Movement.Run");
			RunState.m_animHashRunStopping = Animator.StringToHash("Base Layer.Run.Stopping");
			RunState.m_animHashMovementRunEnd = Animator.StringToHash("Base Layer.Movement.Run_End");
			RunState.m_animTagHashIdle = Animator.StringToHash("Idle");
		}
		if (this.m_animator != null)
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
			if (this.m_animator.layerCount > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.IsRunStartHash(this.m_animator.GetCurrentAnimatorStateInfo(0).fullPathHash))
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
					this.m_startedRunAnim = true;
				}
				return;
			}
		}
		if (!NetworkClient.active)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_startedRunAnim = true;
			this.m_startedExitAnim = true;
		}
	}

	private bool IsRunStartHash(int hash)
	{
		bool result;
		if (hash != RunState.m_animHashRunRun)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RunState.IsRunStartHash(int)).MethodHandle;
			}
			result = (hash == RunState.m_animHashMovementRun);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool IsRunStopHash(int hash)
	{
		bool result;
		if (hash != RunState.m_animHashRunStopping)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RunState.IsRunStopHash(int)).MethodHandle;
			}
			result = (hash == RunState.m_animHashMovementRunEnd);
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected override void OnAnimChange(int prevAnimHash, int prevAnimTag, int nextAnimHash, int nextAnimTag)
	{
		if (this.IsRunStartHash(nextAnimHash))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RunState.OnAnimChange(int, int, int, int)).MethodHandle;
			}
			this.m_startedRunAnim = true;
		}
		if (this.IsRunStopHash(nextAnimHash))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_startedExitAnim = true;
		}
		if (nextAnimTag == RunState.m_animTagHashIdle)
		{
			this.m_startedExitAnim = true;
		}
	}

	protected override void UpdateState()
	{
		BoardSquare square = this.m_pathSquareInfo.square;
		Vector3 position = this.m_owner.m_actor.transform.position;
		Vector3 vector = square.\u001D();
		Vector3 vector2 = vector - position;
		vector2.y = 0f;
		float sqrMagnitude = vector2.sqrMagnitude;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RunState.UpdateState()).MethodHandle;
			}
			if (num > 0.04f)
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
				if (num < 0.08f)
				{
					for (;;)
					{
						switch (7)
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
		float num2;
		if (this.m_pathSquareInfo.connectionType == BoardSquarePathInfo.ConnectionType.Vault)
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
			num2 = this.m_ownerActorData.m_vaultSpeed * num;
		}
		else
		{
			num2 = this.m_ownerActorData.m_runSpeed * num;
		}
		float num3 = num2;
		float num4 = num3 * num3;
		if (sqrMagnitude > num4)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			vector2.Normalize();
			Vector3 transformPositionToVector = position + vector2 * num3;
			transformPositionToVector.y = (float)Board.\u000E().BaselineHeight;
			this.m_owner.m_actor.SetTransformPositionToVector(transformPositionToVector);
			Vector3 dir = vector2;
			dir.y = 0f;
			if (dir.magnitude > 0.01f)
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
				this.m_owner.m_actor.TurnToDirection(dir);
			}
		}
		else
		{
			this.m_updatePath = true;
			this.m_owner.m_actor.SetTransformPositionToVector(this.m_owner.GetGroundPosition(vector));
			this.m_done = true;
		}
	}
}
