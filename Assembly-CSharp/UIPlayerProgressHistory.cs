using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressHistory : UIPlayerProgressSubPanel
{
	public UIPlayerProgressHistoryEntry m_historyEntryPrefab;

	public VerticalLayoutGroup m_historyList;

	public RectTransform m_loadingEntry;

	private ScrollRect m_scrollArea;

	private UIPlayerProgressHistoryEntry selectedEntry;

	private List<PersistedCharacterMatchData> m_matchData;

	private const int kMatchesPerChunk = 2;

	private int m_chunkNumber;

	private float m_viewportHeight;

	private bool m_checkLoad = true;

	private Dictionary<string, string> m_replayPaths;

	private void Update()
	{
		if (this.m_checkLoad)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressHistory.Update()).MethodHandle;
			}
			if (this.m_loadingEntry.gameObject.activeSelf)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				RectTransform rectTransform = this.m_historyList.transform as RectTransform;
				float num = rectTransform.sizeDelta.y - this.m_viewportHeight;
				float y = rectTransform.anchoredPosition.y;
				if (y <= num)
				{
					if (this.m_chunkNumber != 0)
					{
						this.m_checkLoad = false;
						return;
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.LoadNextChunk();
			}
		}
	}

	public void SetMatchHistory(List<PersistedCharacterMatchData> matches)
	{
		if (this.m_matchData != null)
		{
			return;
		}
		UIPlayerProgressHistoryEntry[] componentsInChildren = this.m_historyList.GetComponentsInChildren<UIPlayerProgressHistoryEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressHistory.SetMatchHistory(List<PersistedCharacterMatchData>)).MethodHandle;
		}
		this.m_matchData = new List<PersistedCharacterMatchData>();
		this.m_scrollArea = base.GetComponentInChildren<ScrollRect>();
		this.selectedEntry = null;
		IEnumerator<PersistedCharacterMatchData> enumerator = (from x in matches
		orderby x.MatchComponent.MatchTime descending
		select x).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PersistedCharacterMatchData item = enumerator.Current;
				this.m_matchData.Add(item);
			}
		}
		finally
		{
			if (enumerator != null)
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
				enumerator.Dispose();
			}
		}
		UIManager.SetGameObjectActive(this.m_loadingEntry, true, null);
	}

	private void LoadNextChunk()
	{
		int i = 0;
		while (i < 2)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressHistory.LoadNextChunk()).MethodHandle;
			}
			if (this.m_chunkNumber * 2 + i >= this.m_matchData.Count)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					goto IL_C7;
				}
			}
			else
			{
				PersistedCharacterMatchData entry = this.m_matchData[this.m_chunkNumber * 2 + i];
				UIPlayerProgressHistoryEntry uiplayerProgressHistoryEntry = UnityEngine.Object.Instantiate<UIPlayerProgressHistoryEntry>(this.m_historyEntryPrefab);
				uiplayerProgressHistoryEntry.Setup(entry, this);
				uiplayerProgressHistoryEntry.transform.SetParent(this.m_historyList.transform);
				uiplayerProgressHistoryEntry.transform.localScale = Vector3.one;
				uiplayerProgressHistoryEntry.transform.localPosition = Vector3.zero;
				uiplayerProgressHistoryEntry.m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
				i++;
			}
		}
		IL_C7:
		this.m_chunkNumber++;
		if (this.m_chunkNumber * 2 >= this.m_matchData.Count)
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
			UIManager.SetGameObjectActive(this.m_loadingEntry, false, null);
			this.m_checkLoad = false;
		}
		else
		{
			this.m_loadingEntry.transform.SetAsLastSibling();
		}
		RectTransform rectTransform = this.m_scrollArea.transform as RectTransform;
		RectTransform rectTransform2 = this.m_loadingEntry.transform as RectTransform;
		this.m_viewportHeight = rectTransform.sizeDelta.y + rectTransform2.sizeDelta.y;
	}

	public void OnScroll(BaseEventData eventData)
	{
		this.m_scrollArea.SendMessage("OnScroll", eventData);
		this.m_checkLoad = true;
	}

	public void MatchClicked(UIPlayerProgressHistoryEntry entry)
	{
		if (this.selectedEntry)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressHistory.MatchClicked(UIPlayerProgressHistoryEntry)).MethodHandle;
			}
			this.selectedEntry.SetSelected(false);
		}
		this.selectedEntry = entry;
	}

	public void SelectMatch(PersistedCharacterMatchData matchData)
	{
		UIPlayerProgressHistoryEntry[] componentsInChildren = this.m_historyList.GetComponentsInChildren<UIPlayerProgressHistoryEntry>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (matchData.GameServerProcessCode == componentsInChildren[i].GameServerProcessCode)
			{
				componentsInChildren[i].SetSelected(true);
				this.MatchClicked(componentsInChildren[i]);
				return;
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressHistory.SelectMatch(PersistedCharacterMatchData)).MethodHandle;
			return;
		}
	}

	public string GetReplayFilename(string gameServerProcessCode)
	{
		try
		{
			string text2;
			if (this.m_replayPaths == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressHistory.GetReplayFilename(string)).MethodHandle;
				}
				List<string> list = new List<string>();
				list.AddRange(Directory.GetFiles(HydrogenConfig.Get().ReplaysPath));
				foreach (string path in Directory.GetDirectories(HydrogenConfig.Get().ReplaysPath))
				{
					list.AddRange(Directory.GetFiles(path));
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int startIndex = HydrogenConfig.Get().ReplaysPath.Length + 1;
				this.m_replayPaths = new Dictionary<string, string>();
				foreach (string text in list)
				{
					text2 = text.Substring(startIndex);
					text2 = ClientGameManager.RemoveTimeFromReplayFilename(text2);
					this.m_replayPaths.Add(text2, text);
				}
			}
			text2 = ClientGameManager.FormReplayFilename(string.Empty, gameServerProcessCode, HydrogenConfig.Get().Ticket.Handle);
			string result;
			if (this.m_replayPaths.TryGetValue(text2, out result))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				return result;
			}
		}
		catch (Exception)
		{
		}
		return null;
	}
}
