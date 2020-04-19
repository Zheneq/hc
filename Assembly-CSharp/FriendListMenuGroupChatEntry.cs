using System;
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
		this.m_parent = base.GetComponentInParent<FriendListMenuGroupChat>();
		UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnGroupChatEntryClick));
		UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnGroupChatEntryEnter));
	}

	public void OnGroupChatEntryClick(BaseEventData data)
	{
		this.m_parent.NotifyOnEntryClick(this);
	}

	public void OnGroupChatEntryEnter(BaseEventData data)
	{
		this.m_parent.NotifyOnEntryEnter(this);
	}

	public void Setup(string roomName)
	{
		this.m_nameLabel.text = roomName;
	}
}
