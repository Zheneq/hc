using UnityEngine;

public abstract class UITooltipObject : MonoBehaviour
{
	public Vector2 m_gravityWell = new Vector2(0.5f, 0.5f);

	private TooltipPopulateCall m_populateCall;

	private TooltipDisableCall m_disableCall;

	private TooltipType m_tooltipType;

	public void Setup(TooltipType tooltipType, TooltipPopulateCall call, TooltipDisableCall disableCall = null)
	{
		m_tooltipType = tooltipType;
		m_populateCall = call;
		m_disableCall = disableCall;
	}

	public void CallDisableTooltip()
	{
		if (m_disableCall == null)
		{
			return;
		}
		while (true)
		{
			m_disableCall();
			return;
		}
	}

	public bool PopulateTooltip(UITooltipBase tooltip)
	{
		return m_populateCall(tooltip);
	}

	public void Refresh()
	{
		if (!(UITooltipManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			UITooltipManager.Get().UpdateTooltip(this);
			return;
		}
	}

	public TooltipType GetTooltipType()
	{
		return m_tooltipType;
	}

	protected bool IsSetup()
	{
		return m_populateCall != null;
	}
}
