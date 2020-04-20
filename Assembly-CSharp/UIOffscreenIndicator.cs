using System;
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
		if (this.m_attachedToActor != null)
		{
			bool flag;
			if (this.m_attachedToActor.GetActorMovement() != null)
			{
				flag = this.m_attachedToActor.GetActorMovement().InChargeState();
			}
			else
			{
				flag = false;
			}
			bool result;
			if (!flag)
			{
				result = this.m_attachedToActor.ShouldShowNameplate();
			}
			else
			{
				result = false;
			}
			return result;
		}
		if (this.m_attachedToControlPoint != null)
		{
			return this.m_attachedToControlPoint.CurrentControlPointState != ControlPoint.State.Disabled;
		}
		if (this.m_attachedToFlag != null)
		{
			return this.m_attachedToFlag.ShouldShowIndicator();
		}
		if (this.m_attachedToBoardRegion != null)
		{
			return this.m_attachedToBoardRegion.ShouldShowIndicator();
		}
		if (this.m_attachedToPing != null)
		{
			return true;
		}
		return true;
	}

	protected override Vector2 CalculateScreenPos()
	{
		Vector3 worldPos = Vector3.zero;
		if (this.m_attachedToActor != null)
		{
			worldPos = this.m_attachedToActor.GetNameplatePosition(0f);
			float num = (float)Board.Get().BaselineHeight;
			worldPos.y = Mathf.Clamp(worldPos.y, num, num + 2f);
		}
		else if (this.m_attachedToControlPoint != null)
		{
			worldPos = this.m_attachedToControlPoint.m_region.GetCenter();
		}
		else if (this.m_attachedToFlag != null)
		{
			worldPos = this.m_attachedToFlag.transform.position;
		}
		else if (this.m_attachedToBoardRegion != null)
		{
			worldPos = this.m_attachedToBoardRegion.GetCenter();
		}
		else if (this.m_attachedToPing != null)
		{
			worldPos = this.m_attachedToPing.transform.position;
		}
		else
		{
			Log.Error("Offscreen Indicator is not attached to anything.", new object[0]);
		}
		return base.ScreenPosFromWorldPos(worldPos);
	}

	protected override bool IsVisibleWhenOnScreen()
	{
		return false;
	}
}
