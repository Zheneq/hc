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
		if (!m_checkLoad)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_loadingEntry.gameObject.activeSelf)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				RectTransform rectTransform = m_historyList.transform as RectTransform;
				Vector2 sizeDelta = rectTransform.sizeDelta;
				float num = sizeDelta.y - m_viewportHeight;
				Vector2 anchoredPosition = rectTransform.anchoredPosition;
				float y = anchoredPosition.y;
				if (!(y > num))
				{
					if (m_chunkNumber != 0)
					{
						m_checkLoad = false;
						return;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				LoadNextChunk();
				return;
			}
		}
	}

	public void SetMatchHistory(List<PersistedCharacterMatchData> matches)
	{
		if (m_matchData != null)
		{
			return;
		}
		UIPlayerProgressHistoryEntry[] componentsInChildren = m_historyList.GetComponentsInChildren<UIPlayerProgressHistoryEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_matchData = new List<PersistedCharacterMatchData>();
			m_scrollArea = GetComponentInChildren<ScrollRect>();
			selectedEntry = null;
			IEnumerator<PersistedCharacterMatchData> enumerator = matches.OrderByDescending((PersistedCharacterMatchData x) => x.MatchComponent.MatchTime).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					PersistedCharacterMatchData current = enumerator.Current;
					m_matchData.Add(current);
				}
			}
			finally
			{
				if (enumerator != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							enumerator.Dispose();
							goto end_IL_00b6;
						}
					}
				}
				end_IL_00b6:;
			}
			UIManager.SetGameObjectActive(m_loadingEntry, true);
			return;
		}
	}

	private void LoadNextChunk()
	{
		for (int i = 0; i < 2; i++)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_chunkNumber * 2 + i < m_matchData.Count)
			{
				PersistedCharacterMatchData entry = m_matchData[m_chunkNumber * 2 + i];
				UIPlayerProgressHistoryEntry uIPlayerProgressHistoryEntry = UnityEngine.Object.Instantiate(m_historyEntryPrefab);
				uIPlayerProgressHistoryEntry.Setup(entry, this);
				uIPlayerProgressHistoryEntry.transform.SetParent(m_historyList.transform);
				uIPlayerProgressHistoryEntry.transform.localScale = Vector3.one;
				uIPlayerProgressHistoryEntry.transform.localPosition = Vector3.zero;
				uIPlayerProgressHistoryEntry.m_hitbox.RegisterScrollListener(OnScroll);
				continue;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		m_chunkNumber++;
		if (m_chunkNumber * 2 >= m_matchData.Count)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(m_loadingEntry, false);
			m_checkLoad = false;
		}
		else
		{
			m_loadingEntry.transform.SetAsLastSibling();
		}
		RectTransform rectTransform = m_scrollArea.transform as RectTransform;
		RectTransform rectTransform2 = m_loadingEntry.transform as RectTransform;
		Vector2 sizeDelta = rectTransform.sizeDelta;
		float y = sizeDelta.y;
		Vector2 sizeDelta2 = rectTransform2.sizeDelta;
		m_viewportHeight = y + sizeDelta2.y;
	}

	public void OnScroll(BaseEventData eventData)
	{
		m_scrollArea.SendMessage("OnScroll", eventData);
		m_checkLoad = true;
	}

	public void MatchClicked(UIPlayerProgressHistoryEntry entry)
	{
		if ((bool)selectedEntry)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			selectedEntry.SetSelected(false);
		}
		selectedEntry = entry;
	}

	public void SelectMatch(PersistedCharacterMatchData matchData)
	{
		UIPlayerProgressHistoryEntry[] componentsInChildren = m_historyList.GetComponentsInChildren<UIPlayerProgressHistoryEntry>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (matchData.GameServerProcessCode == componentsInChildren[i].GameServerProcessCode)
			{
				componentsInChildren[i].SetSelected(true);
				MatchClicked(componentsInChildren[i]);
				return;
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public string GetReplayFilename(string gameServerProcessCode)
	{
		try
		{
			string filename;
			if (m_replayPaths == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				List<string> list = new List<string>();
				list.AddRange(Directory.GetFiles(HydrogenConfig.Get().ReplaysPath));
				string[] directories = Directory.GetDirectories(HydrogenConfig.Get().ReplaysPath);
				foreach (string path in directories)
				{
					list.AddRange(Directory.GetFiles(path));
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int startIndex = HydrogenConfig.Get().ReplaysPath.Length + 1;
				m_replayPaths = new Dictionary<string, string>();
				foreach (string item in list)
				{
					filename = item.Substring(startIndex);
					filename = ClientGameManager.RemoveTimeFromReplayFilename(filename);
					m_replayPaths.Add(filename, item);
				}
			}
			filename = ClientGameManager.FormReplayFilename(string.Empty, gameServerProcessCode, HydrogenConfig.Get().Ticket.Handle);
			if (m_replayPaths.TryGetValue(filename, out string value))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return value;
					}
				}
			}
		}
		catch (Exception)
		{
		}
		return null;
	}
}
