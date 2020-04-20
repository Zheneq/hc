using System;
using UnityEngine;

public abstract class UITooltipObject : MonoBehaviour
{
	public Vector2 m_gravityWell = new Vector2(0.5f, 0.5f);

	private TooltipPopulateCall m_populateCall;

	private TooltipDisableCall m_disableCall;

	private TooltipType m_tooltipType;

	public void Setup(TooltipType tooltipType, TooltipPopulateCall call, TooltipDisableCall disableCall = null)
	{
		this.m_tooltipType = tooltipType;
		this.m_populateCall = call;
		this.m_disableCall = disableCall;
	}

	public void CallDisableTooltip()
	{
		if (this.m_disableCall != null)
		{
			this.m_disableCall();
		}
	}

	public bool PopulateTooltip(UITooltipBase tooltip)
	{
		return this.m_populateCall(tooltip);
	}

	public void Refresh()
	{
		if (UITooltipManager.Get() != null)
		{
			UITooltipManager.Get().UpdateTooltip(this);
		}
	}

	public TooltipType GetTooltipType()
	{
		return this.m_tooltipType;
	}

	protected bool IsSetup()
	{
		return this.m_populateCall != null;
	}
}
