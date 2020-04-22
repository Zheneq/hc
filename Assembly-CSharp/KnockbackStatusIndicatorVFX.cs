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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(m_attachParentObject != null))
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
				if (!(m_actor != null))
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
					if (!(m_actor.GetActorStatus() != null))
					{
						return;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						if (!(m_actor.GetActorMovement() != null))
						{
							return;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							bool flag = m_actor != null && !m_actor.IsModelAnimatorDisabled();
							bool flag2 = m_actor.GetActorStatus().HasStatus(m_status, false);
							int num;
							if (actorVisible)
							{
								while (true)
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
									while (true)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									if (flag && m_actor.KnockbackMoveStarted)
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
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
									switch (7)
									{
									case 0:
										continue;
									}
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
