using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendListMenuGroupChatEntry : MonoBehaviour
{
	public Button m_hitbox;

	public TextMeshProUGUI m_nameLabel;

	private FriendListMenuGroupChat m_parent;

	public void Start()
	{
		m_parent = GetComponentInParent<FriendListMenuGroupChat>();
		UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerClick, OnGroupChatEntryClick);
		UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerEnter, OnGroupChatEntryEnter);
	}

	public void OnGroupChatEntryClick(BaseEventData data)
	{
		m_parent.NotifyOnEntryClick(this);
	}

	public void OnGroupChatEntryEnter(BaseEventData data)
	{
		m_parent.NotifyOnEntryEnter(this);
	}

	public void Setup(string roomName)
	{
		m_nameLabel.text = roomName;
	}
}
