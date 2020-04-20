using System;
using UnityEngine;

public class KnockbackStatusIndicatorVFX : AttachedActorVFXInfo
{
	private StatusType m_status;

	public KnockbackStatusIndicatorVFX(GameObject vfxInstance, ActorData actor, JointPopupProperty vfxJoint, string vfxParentObjectName) : base(vfxInstance, actor, vfxJoint, false, vfxParentObjectName, AttachedActorVFXInfo.FriendOrFoeVisibility.Both)
	{
		this.m_status = StatusType.KnockedBack;
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
						if (this.m_actor.GetActorMovement() != null)
						{
							bool flag = this.m_actor != null && !this.m_actor.IsModelAnimatorDisabled();
							bool flag2 = this.m_actor.GetActorStatus().HasStatus(this.m_status, false);
							bool flag3;
							if (actorVisible)
							{
								if (flag2)
								{
									if (flag && this.m_actor.KnockbackMoveStarted)
									{
										flag3 = !this.m_actor.GetActorMovement().AmMoving();
										goto IL_12C;
									}
								}
							}
							flag3 = false;
							IL_12C:
							bool flag4 = flag3;
							if (this.m_vfxInstance.activeSelf != flag4)
							{
								this.m_vfxInstance.SetActive(flag4);
							}
						}
					}
				}
			}
		}
	}
}
