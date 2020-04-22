using LobbyGameClientMessages;
using Steamworks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestCompletePanel : UIScene
{
	public QuestItem[] m_questItems;

	public TextMeshProUGUI m_contractCompletedLabel;

	private static QuestCompletePanel s_instance;

	private const float c_completionDisplaySeconds = 10f;

	private List<QuestCompleteData> m_recentlyCompletedQuests;

	private List<QuestCompleteNotification> m_savedNotificationsForGameOver = new List<QuestCompleteNotification>();

	private bool m_initialized;

	public static QuestCompletePanel Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.QuestComplete;
	}

	public override void Awake()
	{
		s_instance = this;
		m_recentlyCompletedQuests = new List<QuestCompleteData>();
		m_initialized = false;
		ClientGameManager.Get().OnQuestCompleteNotification += HandleQuestCompleteNotification;
		ClientGameManager.Get().OnQuestProgressChanged += HandleQuestProgressChanged;
		base.Awake();
	}

	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			return;
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
			if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (!(GameManager.Get() != null))
				{
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					if (GameManager.Get().GameplayOverrides.EnableSteamAchievements)
					{
						using (Dictionary<int, QuestMetaData>.Enumerator enumerator = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.QuestMetaDatas.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<int, QuestMetaData> current = enumerator.Current;
								if (current.Value.CompletedCount > 0)
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
									QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(current.Key);
									if (questTemplate.AchievmentType != 0)
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
										SteamUserStats.SetAchievement("AR_QUEST_ID_" + current.Key);
									}
								}
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
						foreach (QuestProgress value in ClientGameManager.Get().GetPlayerAccountData().QuestComponent.Progress.Values)
						{
							QuestItem.GetQuestProgress(value.Id, out int currentProgress, out int _);
							if (currentProgress <= 0)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							else
							{
								QuestTemplate questTemplate2 = QuestWideData.Get().GetQuestTemplate(value.Id);
								if (questTemplate2.AchievmentType == AchievementType.None)
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
								}
								else
								{
									SteamUserStats.SetStat("AR_QUEST_ID_" + value.Id, currentProgress);
								}
							}
						}
					}
					return;
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().OnQuestCompleteNotification -= HandleQuestCompleteNotification;
			ClientGameManager.Get().OnQuestProgressChanged -= HandleQuestProgressChanged;
		}
		s_instance = null;
	}

	public void RemoveQuestCompleteNotification(int questId)
	{
		using (List<QuestCompleteData>.Enumerator enumerator = m_recentlyCompletedQuests.GetEnumerator())
		{
			while (true)
			{
				if (!enumerator.MoveNext())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							goto end_IL_000e;
						}
					}
				}
				QuestCompleteData current = enumerator.Current;
				if (current.questId == questId)
				{
					m_recentlyCompletedQuests.Remove(current);
					break;
				}
			}
			end_IL_000e:;
		}
		Setup(true);
	}

	public int TotalQuestsToDisplayForGameOver()
	{
		return m_savedNotificationsForGameOver.Count;
	}

	public void DisplayGameOverQuestComplete(int index)
	{
		for (int i = 0; i < m_savedNotificationsForGameOver.Count; i++)
		{
			if (i <= index)
			{
				while (true)
				{
					switch (3)
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
				if (m_savedNotificationsForGameOver[i].questId > 0)
				{
					DisplayNewQuestComplete(m_savedNotificationsForGameOver[i]);
					m_savedNotificationsForGameOver[i].questId = -1;
				}
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void DisplayNewQuestComplete(QuestCompleteNotification message)
	{
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(message.questId);
		if (questTemplate != null)
		{
			while (true)
			{
				switch (3)
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
			if (questTemplate.HideCompletion)
			{
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		QuestCompleteData questCompleteData = new QuestCompleteData();
		questCompleteData.questId = message.questId;
		questCompleteData.rejectedCount = message.rejectedCount;
		questCompleteData.fadeTime = Time.time + 10f;
		m_recentlyCompletedQuests.Add(questCompleteData);
		if (questTemplate.AchievmentType != 0)
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
			if (SteamManager.Initialized && GameManager.Get() != null)
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
				if (GameManager.Get().GameplayOverrides.EnableSteamAchievements)
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
					SteamUserStats.SetAchievement("AR_QUEST_ID_" + questTemplate.Index);
				}
			}
		}
		Setup();
		if (!(UIPlayerNavPanel.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			UIPlayerNavPanel.Get().NotifyQuestCompleted(m_recentlyCompletedQuests[0]);
			return;
		}
	}

	private void HandleQuestCompleteNotification(QuestCompleteNotification message)
	{
		if (UIGameOverScreen.Get() != null)
		{
			while (true)
			{
				switch (3)
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
			if (UIGameOverScreen.Get().IsVisible)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						m_savedNotificationsForGameOver.Add(message);
						return;
					}
				}
			}
		}
		DisplayNewQuestComplete(message);
	}

	private void HandleQuestProgressChanged(QuestProgress[] questProgresses)
	{
		if (!SteamManager.Initialized || GameManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!GameManager.Get().GameplayOverrides.EnableSteamAchievements)
			{
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			for (int i = 0; i < questProgresses.Length; i++)
			{
				QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questProgresses[i].Id);
				if (questTemplate.AchievmentType != 0)
				{
					QuestItem.GetQuestProgress(questTemplate.Index, out int currentProgress, out int _);
					SteamUserStats.SetStat("AR_QUEST_ID_" + questTemplate.Index, currentProgress);
				}
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void AddSpecialQuestNotification(int questId)
	{
		if (UIGameOverScreen.Get() != null)
		{
			while (true)
			{
				switch (5)
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
			if (UIGameOverScreen.Get().IsVisible)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_savedNotificationsForGameOver.Add(new QuestCompleteNotification
						{
							questId = questId,
							rejectedCount = 0
						});
						return;
					}
				}
			}
		}
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questId);
		if (questTemplate != null && questTemplate.HideCompletion)
		{
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		QuestCompleteData questCompleteData = new QuestCompleteData();
		questCompleteData.questId = questId;
		questCompleteData.rejectedCount = 0;
		questCompleteData.fadeTime = Time.time + 10f;
		m_recentlyCompletedQuests.Add(questCompleteData);
		Setup();
	}

	private void Setup(bool removedQuestCompleteNotification = false)
	{
		for (int i = 0; i < m_questItems.Length; i++)
		{
			QuestItem questItem = m_questItems[i];
			if (i < m_recentlyCompletedQuests.Count)
			{
				while (true)
				{
					switch (3)
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
				QuestCompleteData questCompleteData = m_recentlyCompletedQuests[i];
				UIManager.SetGameObjectActive(questItem, true);
				questItem.SetState(QuestItemState.Finished);
				if (removedQuestCompleteNotification)
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
					UIAnimationEventManager.Get().PlayAnimation(questItem.m_animatorController, "contractItemDefaultIDLE", null, string.Empty);
				}
				questItem.SetQuestId(questCompleteData.questId, questCompleteData.rejectedCount, true, removedQuestCompleteNotification);
				if (questItem.m_expandArrow != null)
				{
					UIManager.SetGameObjectActive(questItem.m_expandArrow, false);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(questItem, false);
			}
		}
		if (m_recentlyCompletedQuests.Count > 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (m_contractCompletedLabel != null)
					{
						UIManager.SetGameObjectActive(m_contractCompletedLabel, true);
					}
					return;
				}
			}
		}
		if (!(m_contractCompletedLabel != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			UIManager.SetGameObjectActive(m_contractCompletedLabel, false);
			return;
		}
	}

	private void Update()
	{
		bool flag = false;
		if (!m_initialized)
		{
			m_initialized = true;
			flag = true;
		}
		bool flag2 = false;
		if (UIGameOverScreen.Get() != null)
		{
			while (true)
			{
				switch (5)
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
			flag2 = UIGameOverScreen.Get().IsVisible;
		}
		else
		{
			if (SteamManager.Initialized)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameManager.Get() != null)
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
					if (GameManager.Get().GameplayOverrides.EnableSteamAchievements)
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
						for (int i = 0; i < m_savedNotificationsForGameOver.Count; i++)
						{
							int questId = m_savedNotificationsForGameOver[i].questId;
							if (questId < 0)
							{
								continue;
							}
							QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questId);
							if (questTemplate.AchievmentType != 0)
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
								SteamUserStats.SetAchievement("AR_QUEST_ID_" + questId);
							}
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
					}
				}
			}
			m_savedNotificationsForGameOver.Clear();
		}
		if (m_recentlyCompletedQuests.Count > 0)
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
			if (!flag2)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				while (m_recentlyCompletedQuests.Count > 0)
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
					if (m_recentlyCompletedQuests[0].fadeTime < Time.time)
					{
						m_recentlyCompletedQuests.RemoveAt(0);
						flag = true;
						continue;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
				}
			}
		}
		if (!flag)
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
			Setup();
			return;
		}
	}
}
