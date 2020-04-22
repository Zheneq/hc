using UnityEngine;

public class StatusIndicatorVFX : AttachedActorVFXInfo
{
	private StatusType m_status;

	public StatusIndicatorVFX(GameObject vfxInstance, ActorData actor, StatusType status, JointPopupProperty vfxJoint, bool alignToRootOrientation, string vfxParentObjectName)
		: base(vfxInstance, actor, vfxJoint, alignToRootOrientation, vfxParentObjectName, FriendOrFoeVisibility.Both)
	{
		m_status = status;
	}

	public override void UpdateVisibility(bool actorVisible, bool sameTeamAsClientActor)
	{
		if (!(m_vfxInstance != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_attachParentObject != null))
			{
				return;
			}
			while (true)
			{
				if (!(m_actor != null))
				{
					return;
				}
				while (true)
				{
					if (!(m_actor.GetActorStatus() != null))
					{
						return;
					}
					bool flag = actorVisible && m_actor.GetActorStatus().HasStatus(m_status, false);
					if (m_vfxInstance.activeSelf != flag)
					{
						m_vfxInstance.SetActive(flag);
					}
					if (!flag)
					{
						return;
					}
					while (true)
					{
						if (m_alignToRootOrientation)
						{
							while (true)
							{
								m_vfxInstance.transform.rotation = m_actor.gameObject.transform.rotation;
								return;
							}
						}
						return;
					}
				}
			}
		}
	}
}
