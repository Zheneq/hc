using System;
using UnityEngine;

public class MoveState
{
	protected BoardSquarePathInfo m_pathSquareInfo;

	protected ActorMovement m_owner;

	protected Animator m_animator;

	protected ActorData m_ownerActorData;

	public bool m_done;

	public bool m_updatePath;

	public bool m_forceAnimReset;

	public BoardSquarePathInfo.ConnectionType m_connectionType;

	public float m_yVelocity;

	private int m_curAnimHash;

	private int m_curAnimTag;

	public float m_timeInAnim;

	private static float s_timeout = 4f;

	public MoveState(ActorMovement owner, BoardSquarePathInfo aesheticPath)
	{
		this.m_owner = owner;
		this.m_ownerActorData = owner.GetComponent<ActorData>();
		this.m_done = false;
		this.m_updatePath = false;
		this.m_animator = this.m_ownerActorData.GetModelAnimator();
		this.m_pathSquareInfo = aesheticPath;
		this.m_connectionType = this.m_pathSquareInfo.connectionType;
		if (this.m_animator != null && this.m_animator.layerCount > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MoveState..ctor(ActorMovement, BoardSquarePathInfo)).MethodHandle;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(0);
			this.m_curAnimHash = currentAnimatorStateInfo.fullPathHash;
			this.m_curAnimTag = currentAnimatorStateInfo.tagHash;
		}
	}

	public string stateName { get; set; }

	internal BoardSquarePathInfo GetAestheticPath()
	{
		return this.m_pathSquareInfo;
	}

	public bool IsOnLastSegment()
	{
		return this.m_pathSquareInfo.next == null;
	}

	public virtual void Setup()
	{
	}

	public void Update()
	{
		this.m_timeInAnim += Time.deltaTime;
		this.UpdateAnimInfo();
		this.UpdateState();
		this.UpdateTimeout();
	}

	private void UpdateTimeout()
	{
		if (this.m_timeInAnim > MoveState.s_timeout)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MoveState.UpdateTimeout()).MethodHandle;
			}
			this.m_done = true;
			this.m_updatePath = true;
			this.m_forceAnimReset = true;
			this.m_owner.m_actor.SetTransformPositionToSquare(this.m_pathSquareInfo.square);
		}
	}

	private void UpdateAnimInfo()
	{
		int curAnimHash = this.m_curAnimHash;
		int curAnimTag = this.m_curAnimTag;
		if (!(this.m_animator == null))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MoveState.UpdateAnimInfo()).MethodHandle;
			}
			if (this.m_animator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(0);
				if (this.m_animator.IsInTransition(0))
				{
					AnimatorStateInfo nextAnimatorStateInfo = this.m_animator.GetNextAnimatorStateInfo(0);
					this.m_curAnimHash = nextAnimatorStateInfo.fullPathHash;
					this.m_curAnimTag = nextAnimatorStateInfo.tagHash;
				}
				else
				{
					this.m_curAnimHash = currentAnimatorStateInfo.fullPathHash;
					this.m_curAnimTag = currentAnimatorStateInfo.tagHash;
				}
				if (this.m_curAnimHash != curAnimHash)
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
					this.m_timeInAnim = 0f;
					this.OnAnimChange(curAnimHash, curAnimTag, this.m_curAnimHash, this.m_curAnimTag);
				}
				return;
			}
		}
	}

	protected virtual void OnAnimChange(int prevAnimHash, int prevAnimTag, int curAnimHash, int nextAnimTag)
	{
	}

	protected virtual void UpdateState()
	{
	}

	public enum LinkType
	{
		None,
		Run,
		Unused1,
		Unused2,
		KnockBack,
		Charge,
		Vault
	}
}
