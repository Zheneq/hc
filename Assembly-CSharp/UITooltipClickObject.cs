using UnityEngine.EventSystems;

public class UITooltipClickObject : UITooltipObject
{
	public bool m_leftClickEnabled;

	public bool m_rightClickEnabled;

	public bool m_middleClickEnabled;

	private void Awake()
	{
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerClick, OnMouseClick);
	}

	private void OnMouseClick(BaseEventData data)
	{
		if (!IsSetup())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		PointerEventData pointerEventData = data as PointerEventData;
		bool flag = false;
		if (m_leftClickEnabled && pointerEventData.button == PointerEventData.InputButton.Left)
		{
			flag = true;
		}
		else if (m_rightClickEnabled && pointerEventData.button == PointerEventData.InputButton.Right)
		{
			flag = true;
		}
		else if (m_middleClickEnabled)
		{
			if (pointerEventData.button == PointerEventData.InputButton.Middle)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		UITooltipManager.Get().ShowMenu(this);
	}
}
