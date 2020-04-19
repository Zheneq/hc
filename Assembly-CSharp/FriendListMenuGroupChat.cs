using System;
using UnityEngine;
using UnityEngine.UI;

public class FriendListMenuGroupChat : MonoBehaviour
{
	public GridLayoutGroup m_gridLayout;

	public FriendListMenuGroupChatEntry m_groupChatEntryPrefab;

	private int numEntries;

	public void NotifyOnEntryClick(FriendListMenuGroupChatEntry entry)
	{
		UIManager.SetGameObjectActive(base.gameObject, false, null);
	}

	public void NotifyOnEntryEnter(FriendListMenuGroupChatEntry entry)
	{
		FriendListMenuGroupChatEntry[] componentsInChildren = this.m_gridLayout.GetComponentsInChildren<FriendListMenuGroupChatEntry>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] == entry)
			{
				componentsInChildren[i].m_nameLabel.color = Color.white;
			}
			else
			{
				componentsInChildren[i].m_nameLabel.color = Color.gray;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListMenuGroupChat.NotifyOnEntryEnter(FriendListMenuGroupChatEntry)).MethodHandle;
		}
		this.ResizeWindowSize();
	}

	public void ResizeWindowSize()
	{
		(base.gameObject.transform as RectTransform).sizeDelta = new Vector2((base.gameObject.transform as RectTransform).sizeDelta.x, (float)this.numEntries * this.m_gridLayout.cellSize.y + 40f);
	}

	public void Setup()
	{
		FriendListMenuGroupChatEntry[] componentsInChildren = this.m_gridLayout.GetComponentsInChildren<FriendListMenuGroupChatEntry>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListMenuGroupChat.Setup()).MethodHandle;
		}
		this.numEntries = Mathf.FloorToInt(UnityEngine.Random.value * 10f);
		for (int j = 0; j < this.numEntries; j++)
		{
			FriendListMenuGroupChatEntry friendListMenuGroupChatEntry = UnityEngine.Object.Instantiate<FriendListMenuGroupChatEntry>(this.m_groupChatEntryPrefab);
			friendListMenuGroupChatEntry.transform.SetParent(this.m_gridLayout.transform);
			friendListMenuGroupChatEntry.Setup(string.Format(StringUtil.TR("GroupChatRoom", "FriendList"), Mathf.FloorToInt(UnityEngine.Random.value * 1000f)));
			friendListMenuGroupChatEntry.transform.localScale = Vector3.one;
			friendListMenuGroupChatEntry.transform.localPosition = Vector3.zero;
			friendListMenuGroupChatEntry.transform.localEulerAngles = Vector3.zero;
			friendListMenuGroupChatEntry.m_nameLabel.color = Color.gray;
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		this.ResizeWindowSize();
	}
}
