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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(StatusIndicatorVFX.UpdateVisibility(bool, bool)).MethodHandle;
			}
			if (this.m_attachParentObject != null)
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
				if (this.m_actor != null)
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
					if (this.m_actor.GetActorStatus() != null)
					{
						bool flag = actorVisible && this.m_actor.GetActorStatus().HasStatus(this.m_status, false);
						if (this.m_vfxInstance.activeSelf != flag)
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
							this.m_vfxInstance.SetActive(flag);
						}
						if (flag)
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
							if (this.m_alignToRootOrientation)
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
								this.m_vfxInstance.transform.rotation = this.m_actor.gameObject.transform.rotation;
							}
						}
					}
				}
			}
		}
	}
}
