using System;
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
		this.m_characterIcon.sprite = actorData.GetScreenIndicatorIcon();
		if (this.m_grayCharacterIcon != null)
		{
			this.m_grayCharacterIcon.sprite = actorData.GetScreenIndicatorBWIcon();
		}
	}

	protected override bool ShouldGrayOutIndicator()
	{
		int num = 1;
		return this.m_attachedToActor.GetLastVisibleTurnToClient() < GameFlowData.Get().CurrentTurn - num;
	}

	protected override bool CalculateVisibility()
	{
		bool result;
		if (this.m_attachedToActor == null)
		{
			result = false;
		}
		else
		{
			if (!this.m_attachedToActor.IsDead())
			{
				if (this.m_attachedToActor.IsModelAnimatorDisabled())
				{
				}
				else
				{
					if (this.m_attachedToActor.IgnoreForAbilityHits)
					{
						result = false;
						goto IL_AD;
					}
					if (this.m_attachedToActor.ClientLastKnownPosSquare == null)
					{
						result = false;
						goto IL_AD;
					}
					result = !this.m_attachedToActor.IsVisibleToClient();
					goto IL_AD;
				}
			}
			result = false;
		}
		IL_AD:
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				if (this.m_lastActiveOwnedActor != GameFlowData.Get().activeOwnedActorData)
				{
					this.m_lastActiveOwnedActor = GameFlowData.Get().activeOwnedActorData;
					base.MarkGrayoutForUpdate();
				}
			}
		}
		return result;
	}

	protected override Vector2 CalculateScreenPos()
	{
		Vector3 clientLastKnownPos = this.m_attachedToActor.GetClientLastKnownPos();
		return base.ScreenPosFromWorldPos(clientLastKnownPos);
	}

	protected override bool IsVisibleWhenOnScreen()
	{
		return true;
	}
}
