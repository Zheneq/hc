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
	private static UIEventTriggerUtils.EventDelegate _003C_003Ef__mg_0024cache0;

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static UIFrontEndUIResources Get()
	{
		return s_instance;
	}

	public static void CheckOnClickForURL(TextMeshProUGUI textObject)
	{
		if (!(textObject != null))
		{
			return;
		}
		while (true)
		{
			if (!(textObject.GetComponentInParent<TMP_InputField>() == null))
			{
				return;
			}
			while (true)
			{
				if (textObject.GetComponentInParent<InputField>() == null)
				{
					while (true)
					{
						UIEventTriggerUtils.AddListener(textObject.gameObject, EventTriggerType.PointerClick, OnTextClicked);
						return;
					}
				}
				return;
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
			TMP_LinkInfo tMP_LinkInfo = component.textInfo.linkInfo[num];
			Application.OpenURL(tMP_LinkInfo.GetLinkID().Substring(4));
		}
		if (!component.textInfo.linkInfo[num].GetLinkID().StartsWith("discord:"))
		{
			return;
		}
		while (true)
		{
			string a = component.textInfo.linkInfo[num].GetLinkID().Substring(8);
			if (a == "join")
			{
				while (true)
				{
					DebugCommands.Get().RunDebugCommand("/discord", "join");
					return;
				}
			}
			return;
		}
	}
}
