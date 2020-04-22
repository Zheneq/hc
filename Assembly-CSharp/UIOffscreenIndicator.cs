using UnityEngine;

public class UIOffscreenIndicator : UIBaseIndicator
{
	protected override bool ShouldRotate()
	{
		return true;
	}

	protected override bool ShouldHideOptionalArrowWhenOffscreen()
	{
		return false;
	}

	protected override bool ShouldGrayOutIndicator()
	{
		return false;
	}

	protected override bool CalculateVisibility()
	{
		if (m_attachedToActor != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					int num;
					if (m_attachedToActor.GetActorMovement() != null)
					{
						num = (m_attachedToActor.GetActorMovement().InChargeState() ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					int result;
					if (num == 0)
					{
						result = (m_attachedToActor.ShouldShowNameplate() ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		if (m_attachedToControlPoint != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_attachedToControlPoint.CurrentControlPointState != ControlPoint.State.Disabled;
				}
			}
		}
		if (m_attachedToFlag != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_attachedToFlag.ShouldShowIndicator();
				}
			}
		}
		if (m_attachedToBoardRegion != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_attachedToBoardRegion.ShouldShowIndicator();
				}
			}
		}
		if (m_attachedToPing != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		return true;
	}

	protected override Vector2 CalculateScreenPos()
	{
		Vector3 worldPos = Vector3.zero;
		if (m_attachedToActor != null)
		{
			worldPos = m_attachedToActor.GetNameplatePosition(0f);
			float num = Board.Get().BaselineHeight;
			worldPos.y = Mathf.Clamp(worldPos.y, num, num + 2f);
		}
		else if (m_attachedToControlPoint != null)
		{
			worldPos = m_attachedToControlPoint.m_region.GetCenter();
		}
		else if (m_attachedToFlag != null)
		{
			worldPos = m_attachedToFlag.transform.position;
		}
		else if (m_attachedToBoardRegion != null)
		{
			worldPos = m_attachedToBoardRegion.GetCenter();
		}
		else if (m_attachedToPing != null)
		{
			worldPos = m_attachedToPing.transform.position;
		}
		else
		{
			Log.Error("Offscreen Indicator is not attached to anything.");
		}
		return ScreenPosFromWorldPos(worldPos);
	}

	protected override bool IsVisibleWhenOnScreen()
	{
		return false;
	}
}
