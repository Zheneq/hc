using UnityEngine;
using UnityEngine.UI;

public class FriendListMenuGroupChat : MonoBehaviour
{
	public GridLayoutGroup m_gridLayout;

	public FriendListMenuGroupChatEntry m_groupChatEntryPrefab;

	private int numEntries;

	public void NotifyOnEntryClick(FriendListMenuGroupChatEntry entry)
	{
		UIManager.SetGameObjectActive(base.gameObject, false);
	}

	public void NotifyOnEntryEnter(FriendListMenuGroupChatEntry entry)
	{
		FriendListMenuGroupChatEntry[] componentsInChildren = m_gridLayout.GetComponentsInChildren<FriendListMenuGroupChatEntry>();
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
			ResizeWindowSize();
			return;
		}
	}

	public void ResizeWindowSize()
	{
		RectTransform obj = base.gameObject.transform as RectTransform;
		Vector2 sizeDelta = (base.gameObject.transform as RectTransform).sizeDelta;
		float x = sizeDelta.x;
		float num = numEntries;
		Vector2 cellSize = m_gridLayout.cellSize;
		obj.sizeDelta = new Vector2(x, num * cellSize.y + 40f);
	}

	public void Setup()
	{
		FriendListMenuGroupChatEntry[] componentsInChildren = m_gridLayout.GetComponentsInChildren<FriendListMenuGroupChatEntry>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i].gameObject);
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			numEntries = Mathf.FloorToInt(Random.value * 10f);
			for (int j = 0; j < numEntries; j++)
			{
				FriendListMenuGroupChatEntry friendListMenuGroupChatEntry = Object.Instantiate(m_groupChatEntryPrefab);
				friendListMenuGroupChatEntry.transform.SetParent(m_gridLayout.transform);
				friendListMenuGroupChatEntry.Setup(string.Format(StringUtil.TR("GroupChatRoom", "FriendList"), Mathf.FloorToInt(Random.value * 1000f)));
				friendListMenuGroupChatEntry.transform.localScale = Vector3.one;
				friendListMenuGroupChatEntry.transform.localPosition = Vector3.zero;
				friendListMenuGroupChatEntry.transform.localEulerAngles = Vector3.zero;
				friendListMenuGroupChatEntry.m_nameLabel.color = Color.gray;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				ResizeWindowSize();
				return;
			}
		}
	}
}
