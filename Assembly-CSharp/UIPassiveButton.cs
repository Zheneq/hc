using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPassiveButton : MonoBehaviour
{
	public Button theButton;

	private void Start()
	{
		if (!(theButton != null))
		{
			return;
		}
		while (true)
		{
			UIEventTriggerUtils.AddListener(theButton.gameObject, EventTriggerType.PointerEnter, OnPointerEnter);
			UIEventTriggerUtils.AddListener(theButton.gameObject, EventTriggerType.PointerExit, OnPointerExit);
			return;
		}
	}

	public void OnPointerEnter(BaseEventData data)
	{
		OnButtonHover(true);
	}

	public void OnPointerExit(BaseEventData data)
	{
		OnButtonHover(false);
	}

	private void OnButtonHover(bool hover)
	{
	}
}
