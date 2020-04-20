using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFrontEndUIResources : MonoBehaviour
{
	public GameObject m_RollOverPrefab;

	private static UIFrontEndUIResources s_instance;

	[CompilerGenerated]
	private static UIEventTriggerUtils.EventDelegate f__mg_cache0;

	private void Awake()
	{
		UIFrontEndUIResources.s_instance = this;
	}

	private void OnDestroy()
	{
		UIFrontEndUIResources.s_instance = null;
	}

	public static UIFrontEndUIResources Get()
	{
		return UIFrontEndUIResources.s_instance;
	}

	public static void CheckOnClickForURL(TextMeshProUGUI textObject)
	{
		if (textObject != null)
		{
			if (textObject.GetComponentInParent<TMP_InputField>() == null)
			{
				if (textObject.GetComponentInParent<InputField>() == null)
				{
					GameObject gameObject = textObject.gameObject;
					EventTriggerType triggerType = EventTriggerType.PointerClick;
					
					UIEventTriggerUtils.AddListener(gameObject, triggerType, new UIEventTriggerUtils.EventDelegate(UIFrontEndUIResources.OnTextClicked));
				}
			}
		}
	}

	public static void OnTextClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		TextMeshProUGUI component = pointerEventData.pointerPress.GetComponent<TextMeshProUGUI>();
		int num = TMP_TextUtilities.FindIntersectingLink(component, Input.mousePosition, component.GetComponentInParent<Canvas>().worldCamera);
		if (num < 0)
		{
			return;
		}
		if (component.textInfo.linkInfo[num].GetLinkID().StartsWith("url:"))
		{
			TMP_LinkInfo tmp_LinkInfo = component.textInfo.linkInfo[num];
			Application.OpenURL(tmp_LinkInfo.GetLinkID().Substring(4));
		}
		if (component.textInfo.linkInfo[num].GetLinkID().StartsWith("discord:"))
		{
			string a = component.textInfo.linkInfo[num].GetLinkID().Substring(8);
			if (a == "join")
			{
				DebugCommands.Get().RunDebugCommand("/discord", "join");
			}
		}
	}
}
