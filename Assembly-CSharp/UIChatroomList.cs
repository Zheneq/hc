using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIChatroomList : MonoBehaviour
{
	public GridLayoutGroup m_gridContent;

	public UIAutocompleteEntry m_entryPrefab;

	public CanvasGroup m_canvasGroup;

	private const int kMaxViewportSize = 10;

	private UITextConsole m_parent;

	private ScrollRect m_scrollRect;

	private RectTransform m_rectTransform;

	private bool m_isInitialized;

	private List<GameObject> m_clickListenerExceptions;

	private void Initialize()
	{
		if (m_isInitialized)
		{
			return;
		}
		m_isInitialized = true;
		m_rectTransform = (base.transform as RectTransform);
		m_scrollRect = GetComponent<ScrollRect>();
		m_clickListenerExceptions = new List<GameObject>();
		m_clickListenerExceptions.Add(m_parent.m_chatroomHitbox.gameObject);
		m_clickListenerExceptions.Add(base.gameObject);
		UIAutocompleteEntry[] componentsInChildren = m_gridContent.GetComponentsInChildren<UIAutocompleteEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].m_hitbox.callback = OnClick;
			componentsInChildren[i].m_hitbox.RegisterScrollListener(OnScroll);
			m_clickListenerExceptions.Add(componentsInChildren[i].m_hitbox.gameObject);
		}
		while (true)
		{
			return;
		}
	}

	private void OnDestroy()
	{
		if (!(UIClickListener.Get() != null))
		{
			return;
		}
		while (true)
		{
			UIClickListener.Get().Disable();
			return;
		}
	}

	public bool IsVisible()
	{
		return base.gameObject.activeSelf;
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(base.gameObject, visible);
		if (!(UIClickListener.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!visible)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						UIClickListener.Get().Disable();
						return;
					}
				}
			}
			UIClickListener.Get().Enable(m_clickListenerExceptions, delegate
			{
				SetVisible(false);
			});
			return;
		}
	}

	public void Setup(UITextConsole parent)
	{
		m_parent = parent;
		Initialize();
		List<string> availableChatRooms = parent.GetAvailableChatRooms();
		UIAutocompleteEntry[] componentsInChildren = m_gridContent.GetComponentsInChildren<UIAutocompleteEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UIManager.SetGameObjectActive(componentsInChildren[i], false);
		}
		while (true)
		{
			for (int j = 0; j < availableChatRooms.Count; j++)
			{
				UIAutocompleteEntry uIAutocompleteEntry;
				if (j < componentsInChildren.Length)
				{
					uIAutocompleteEntry = componentsInChildren[j];
				}
				else
				{
					uIAutocompleteEntry = Object.Instantiate(m_entryPrefab);
					uIAutocompleteEntry.transform.SetParent(m_gridContent.transform);
					uIAutocompleteEntry.transform.localPosition = Vector3.zero;
					uIAutocompleteEntry.transform.localScale = Vector3.one;
					uIAutocompleteEntry.m_hitbox.callback = OnClick;
					uIAutocompleteEntry.m_hitbox.RegisterScrollListener(OnScroll);
					m_clickListenerExceptions.Add(uIAutocompleteEntry.m_hitbox.gameObject);
				}
				UIManager.SetGameObjectActive(uIAutocompleteEntry, true);
				uIAutocompleteEntry.m_hitbox.ResetMouseState();
				uIAutocompleteEntry.Setup(availableChatRooms[j].Substring(1).ToInitialCharUpper(), availableChatRooms[j]);
			}
			Vector3 v = m_rectTransform.sizeDelta;
			float num = m_gridContent.padding.vertical;
			Vector2 cellSize = m_gridContent.cellSize;
			v.y = num + cellSize.y * (float)Mathf.Min(availableChatRooms.Count, 10);
			m_rectTransform.sizeDelta = v;
			return;
		}
	}

	private void OnClick(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (pointerEventData.button != 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		UIAutocompleteEntry componentInParent = data.selectedObject.GetComponentInParent<UIAutocompleteEntry>();
		m_parent.m_textInput.text = new StringBuilder().Append(componentInParent.GetTextValue()).Append(" ").Append(m_parent.m_textInput.text).ToString();
		m_parent.m_textInput.Select();
		m_parent.m_textInput.ActivateInputField();
		m_parent.DehighlightTextAndPositionCarat();
		m_parent.Show();
		SetVisible(false);
	}

	private void OnScroll(BaseEventData data)
	{
		m_scrollRect.OnScroll((PointerEventData)data);
	}
}
