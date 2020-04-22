using UnityEngine;

public class UILastKnownPosIndicator : UIBaseIndicator
{
	private ActorData m_lastActiveOwnedActor;

	protected override bool ShouldRotate()
	{
		return false;
	}

	protected override bool ShouldHideOptionalArrowWhenOffscreen()
	{
		return true;
	}

	protected override void SetupCharacterIcons(ActorData actorData)
	{
		m_characterIcon.sprite = actorData.GetScreenIndicatorIcon();
		if (!(m_grayCharacterIcon != null))
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
			m_grayCharacterIcon.sprite = actorData.GetScreenIndicatorBWIcon();
			return;
		}
	}

	protected override bool ShouldGrayOutIndicator()
	{
		int num = 1;
		return m_attachedToActor.GetLastVisibleTurnToClient() < GameFlowData.Get().CurrentTurn - num;
	}

	protected override bool CalculateVisibility()
	{
		bool result;
		if (m_attachedToActor == null)
		{
			result = false;
		}
		else
		{
			if (!m_attachedToActor.IsDead())
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!m_attachedToActor.IsModelAnimatorDisabled())
				{
					if (m_attachedToActor.IgnoreForAbilityHits)
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
						result = false;
					}
					else if (!(m_attachedToActor.ClientLastKnownPosSquare == null))
					{
						result = ((!m_attachedToActor.IsVisibleToClient()) ? true : false);
					}
					else
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
						result = false;
					}
					goto IL_00ad;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			result = false;
		}
		goto IL_00ad;
		IL_00ad:
		if (GameFlowData.Get() != null)
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
			if (GameFlowData.Get().activeOwnedActorData != null)
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
				if (m_lastActiveOwnedActor != GameFlowData.Get().activeOwnedActorData)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					m_lastActiveOwnedActor = GameFlowData.Get().activeOwnedActorData;
					MarkGrayoutForUpdate();
				}
			}
		}
		return result;
	}

	protected override Vector2 CalculateScreenPos()
	{
		Vector3 clientLastKnownPos = m_attachedToActor.GetClientLastKnownPos();
		return ScreenPosFromWorldPos(clientLastKnownPos);
	}

	protected override bool IsVisibleWhenOnScreen()
	{
		return true;
	}
}
