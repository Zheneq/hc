using UnityEngine;
using UnityEngine.EventSystems;

public class UITooltipHoverObject : UITooltipObject
{
	public Vector2[] m_anchorPoints;

	private void Awake()
	{
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerEnter, OnMouseHover);
	}

	private void OnMouseHover(BaseEventData data)
	{
		if (!IsSetup())
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
			UITooltipManager.Get().ShowDisplayTooltip(this);
			return;
		}
	}
}
