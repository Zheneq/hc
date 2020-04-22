using UnityEngine;

public class MoveState
{
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

	public string stateName
	{
		get;
		set;
	}

	public MoveState(ActorMovement owner, BoardSquarePathInfo aesheticPath)
	{
		m_owner = owner;
		m_ownerActorData = owner.GetComponent<ActorData>();
		m_done = false;
		m_updatePath = false;
		m_animator = m_ownerActorData.GetModelAnimator();
		m_pathSquareInfo = aesheticPath;
		m_connectionType = m_pathSquareInfo.connectionType;
		if (!(m_animator != null) || m_animator.layerCount <= 0)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AnimatorStateInfo currentAnimatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
			m_curAnimHash = currentAnimatorStateInfo.fullPathHash;
			m_curAnimTag = currentAnimatorStateInfo.tagHash;
			return;
		}
	}

	internal BoardSquarePathInfo GetAestheticPath()
	{
		return m_pathSquareInfo;
	}

	public bool IsOnLastSegment()
	{
		return m_pathSquareInfo.next == null;
	}

	public virtual void Setup()
	{
	}

	public void Update()
	{
		m_timeInAnim += Time.deltaTime;
		UpdateAnimInfo();
		UpdateState();
		UpdateTimeout();
	}

	private void UpdateTimeout()
	{
		if (!(m_timeInAnim > s_timeout))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_done = true;
			m_updatePath = true;
			m_forceAnimReset = true;
			m_owner.m_actor.SetTransformPositionToSquare(m_pathSquareInfo.square);
			return;
		}
	}

	private void UpdateAnimInfo()
	{
		int curAnimHash = m_curAnimHash;
		int curAnimTag = m_curAnimTag;
		if (m_animator == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_animator.layerCount < 1)
			{
				return;
			}
			AnimatorStateInfo currentAnimatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
			if (m_animator.IsInTransition(0))
			{
				AnimatorStateInfo nextAnimatorStateInfo = m_animator.GetNextAnimatorStateInfo(0);
				m_curAnimHash = nextAnimatorStateInfo.fullPathHash;
				m_curAnimTag = nextAnimatorStateInfo.tagHash;
			}
			else
			{
				m_curAnimHash = currentAnimatorStateInfo.fullPathHash;
				m_curAnimTag = currentAnimatorStateInfo.tagHash;
			}
			if (m_curAnimHash == curAnimHash)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				m_timeInAnim = 0f;
				OnAnimChange(curAnimHash, curAnimTag, m_curAnimHash, m_curAnimTag);
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
}
