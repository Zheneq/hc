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
				RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackStatusIndicatorVFX.UpdateVisibility(bool, bool)).MethodHandle;
			}
			if (this.m_attachParentObject != null)
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
				if (this.m_actor != null)
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
					if (this.m_actor.\u000E() != null)
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
						if (this.m_actor.\u000E() != null)
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
							bool flag = this.m_actor != null && !this.m_actor.\u0012();
							bool flag2 = this.m_actor.\u000E().HasStatus(this.m_status, false);
							bool flag3;
							if (actorVisible)
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
								if (flag2)
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
									if (flag && this.m_actor.KnockbackMoveStarted)
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
										flag3 = !this.m_actor.\u000E().AmMoving();
										goto IL_12C;
									}
								}
							}
							flag3 = false;
							IL_12C:
							bool flag4 = flag3;
							if (this.m_vfxInstance.activeSelf != flag4)
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
								this.m_vfxInstance.SetActive(flag4);
							}
						}
					}
				}
			}
		}
	}
}
