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
			switch (7)
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
				switch (2)
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
					switch (6)
					{
					case 0:
						continue;
					}
					if (!(m_actor.GetActorStatus() != null))
					{
						return;
					}
					bool flag = actorVisible && m_actor.GetActorStatus().HasStatus(m_status, false);
					if (m_vfxInstance.activeSelf != flag)
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
						m_vfxInstance.SetActive(flag);
					}
					if (!flag)
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
						if (m_alignToRootOrientation)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
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
