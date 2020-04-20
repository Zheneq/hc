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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UILastKnownPosIndicator.SetupCharacterIcons(ActorData)).MethodHandle;
			}
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UILastKnownPosIndicator.CalculateVisibility()).MethodHandle;
				}
				if (this.m_attachedToActor.IsModelAnimatorDisabled())
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
				}
				else
				{
					if (this.m_attachedToActor.IgnoreForAbilityHits)
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
						result = false;
						goto IL_AD;
					}
					if (this.m_attachedToActor.ClientLastKnownPosSquare == null)
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
			for (;;)
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_lastActiveOwnedActor != GameFlowData.Get().activeOwnedActorData)
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
