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
	private static UIEventTriggerUtils.EventDelegate <>f__mg$cache0;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEndUIResources.CheckOnClickForURL(TextMeshProUGUI)).MethodHandle;
			}
			if (textObject.GetComponentInParent<TMP_InputField>() == null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (textObject.GetComponentInParent<InputField>() == null)
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
					GameObject gameObject = textObject.gameObject;
					EventTriggerType triggerType = EventTriggerType.PointerClick;
					if (UIFrontEndUIResources.<>f__mg$cache0 == null)
					{
						UIFrontEndUIResources.<>f__mg$cache0 = new UIEventTriggerUtils.EventDelegate(UIFrontEndUIResources.OnTextClicked);
					}
					UIEventTriggerUtils.AddListener(gameObject, triggerType, UIFrontEndUIResources.<>f__mg$cache0);
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEndUIResources.OnTextClicked(BaseEventData)).MethodHandle;
			}
			string a = component.textInfo.linkInfo[num].GetLinkID().Substring(8);
			if (a == "join")
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
				DebugCommands.Get().RunDebugCommand("/discord", "join");
			}
		}
	}
}
