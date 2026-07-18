using UnityEngine;

public class KnockbackStatusIndicatorVFX : AttachedActorVFXInfo
{
	private StatusType m_status;

	public KnockbackStatusIndicatorVFX(GameObject vfxInstance, ActorData actor, JointPopupProperty vfxJoint, string vfxParentObjectName)
		: base(vfxInstance, actor, vfxJoint, false, vfxParentObjectName, FriendOrFoeVisibility.Both)
	{
		m_status = StatusType.KnockedBack;
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
					while (true)
					{
						if (!(m_actor.GetActorMovement() != null))
						{
							return;
						}
						while (true)
						{
							bool flag = m_actor != null && !m_actor.IsInRagdoll();
							bool flag2 = m_actor.GetActorStatus().HasStatus(m_status, false);
							int num;
							if (actorVisible)
							{
								if (flag2)
								{
									if (flag && m_actor.KnockbackMoveStarted)
									{
										num = ((!m_actor.GetActorMovement().AmMoving()) ? 1 : 0);
										goto IL_012c;
									}
								}
							}
							num = 0;
							goto IL_012c;
							IL_012c:
							bool flag3 = (byte)num != 0;
							if (m_vfxInstance.activeSelf != flag3)
							{
								while (true)
								{
									m_vfxInstance.SetActive(flag3);
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
}
