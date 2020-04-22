using UnityEngine.EventSystems;
using UnityEngine.UI;

public class _ScrollRect : ScrollRect
{
	public bool m_ignoreOnDrag;

	public override void OnDrag(PointerEventData eventData)
	{
		if (m_ignoreOnDrag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		base.OnDrag(eventData);
	}
}
