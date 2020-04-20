using System;
using UnityEngine;

public class StatusIndicatorVFX : AttachedActorVFXInfo
{
	private StatusType m_status;

	public StatusIndicatorVFX(GameObject vfxInstance, ActorData actor, StatusType status, JointPopupProperty vfxJoint, bool alignToRootOrientation, string vfxParentObjectName) : base(vfxInstance, actor, vfxJoint, alignToRootOrientation, vfxParentObjectName, AttachedActorVFXInfo.FriendOrFoeVisibility.Both)
	{
		this.m_status = status;
	}

	public override void UpdateVisibility(bool actorVisible, bool sameTeamAsClientActor)
	{
		if (this.m_vfxInstance != null)
		{
			if (this.m_attachParentObject != null)
			{
				if (this.m_actor != null)
				{
					if (this.m_actor.GetActorStatus() != null)
					{
						bool flag = actorVisible && this.m_actor.GetActorStatus().HasStatus(this.m_status, false);
						if (this.m_vfxInstance.activeSelf != flag)
						{
							this.m_vfxInstance.SetActive(flag);
						}
						if (flag)
						{
							if (this.m_alignToRootOrientation)
							{
								this.m_vfxInstance.transform.rotation = this.m_actor.gameObject.transform.rotation;
							}
						}
					}
				}
			}
		}
	}
}
