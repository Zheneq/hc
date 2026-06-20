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
		if (!m_checkLoad || !m_loadingEntry.gameObject.activeSelf)
		{
			return;
		}
		RectTransform rectTransform = m_historyList.transform as RectTransform;
		Vector2 sizeDelta = rectTransform.sizeDelta;
		float num = sizeDelta.y - m_viewportHeight;
		Vector2 anchoredPosition = rectTransform.anchoredPosition;
		float y = anchoredPosition.y;
		if (y > num || m_chunkNumber == 0)
		{
			LoadNextChunk();
		}
		else
		{
			m_checkLoad = false;
		}
	}

	public void SetMatchHistory(List<PersistedCharacterMatchData> matches)
	{
		if (m_matchData != null)
		{
			return;
		}
		foreach (UIPlayerProgressHistoryEntry entry in m_historyList.GetComponentsInChildren<UIPlayerProgressHistoryEntry>(true))
		{
			Destroy(entry.gameObject);
		}
		m_matchData = new List<PersistedCharacterMatchData>();
		m_scrollArea = GetComponentInChildren<ScrollRect>();
		selectedEntry = null;
		foreach (PersistedCharacterMatchData current in matches.OrderByDescending(x => x.MatchComponent.MatchTime))
		{
			m_matchData.Add(current);
		}
		UIManager.SetGameObjectActive(m_loadingEntry, true);
	}

	private void LoadNextChunk()
	{
		for (int i = 0; i < 2; i++)
		{
			if (m_chunkNumber * 2 + i >= m_matchData.Count)
			{
				break;
			}
			PersistedCharacterMatchData entry = m_matchData[m_chunkNumber * 2 + i];
			UIPlayerProgressHistoryEntry uIPlayerProgressHistoryEntry = Instantiate(m_historyEntryPrefab);
			uIPlayerProgressHistoryEntry.Setup(entry, this);
			uIPlayerProgressHistoryEntry.transform.SetParent(m_historyList.transform);
			uIPlayerProgressHistoryEntry.transform.localScale = Vector3.one;
			uIPlayerProgressHistoryEntry.transform.localPosition = Vector3.zero;
			uIPlayerProgressHistoryEntry.m_hitbox.RegisterScrollListener(OnScroll);
		}
		m_chunkNumber++;
		if (m_chunkNumber * 2 >= m_matchData.Count)
		{
			UIManager.SetGameObjectActive(m_loadingEntry, false);
			m_checkLoad = false;
		}
		else
		{
			m_loadingEntry.transform.SetAsLastSibling();
		}
		RectTransform rectTransform = m_scrollArea.transform as RectTransform;
		RectTransform rectTransform2 = m_loadingEntry.transform as RectTransform;
		m_viewportHeight = rectTransform.sizeDelta.y + rectTransform2.sizeDelta.y;
	}

	public void OnScroll(BaseEventData eventData)
	{
		m_scrollArea.SendMessage("OnScroll", eventData);
		m_checkLoad = true;
	}

	public void MatchClicked(UIPlayerProgressHistoryEntry entry)
	{
		if (selectedEntry != null)
		{
			selectedEntry.SetSelected(false);
		}
		selectedEntry = entry;
	}

	public void SelectMatch(PersistedCharacterMatchData matchData)
	{
		foreach (UIPlayerProgressHistoryEntry entry in m_historyList.GetComponentsInChildren<UIPlayerProgressHistoryEntry>())
		{
			if (matchData.GameServerProcessCode == entry.GameServerProcessCode)
			{
				entry.SetSelected(true);
				MatchClicked(entry);
				return;
			}
		}
	}

	public string GetReplayFilename(string gameServerProcessCode)
	{
		try
		{
			string filename;
			if (m_replayPaths == null)
			{
				List<string> list = new List<string>();
				list.AddRange(Directory.GetFiles(HydrogenConfig.Get().ReplaysPath));
				string[] directories = Directory.GetDirectories(HydrogenConfig.Get().ReplaysPath);
				foreach (string path in directories)
				{
					list.AddRange(Directory.GetFiles(path));
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
				return value;
			}
		}
		catch (Exception)
		{
		}
		return null;
	}
}
