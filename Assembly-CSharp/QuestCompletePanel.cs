using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using Steamworks;
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
		return QuestCompletePanel.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.QuestComplete;
	}

	public override void Awake()
	{
		QuestCompletePanel.s_instance = this;
		this.m_recentlyCompletedQuests = new List<QuestCompleteData>();
		this.m_initialized = false;
		ClientGameManager.Get().OnQuestCompleteNotification += this.HandleQuestCompleteNotification;
		ClientGameManager.Get().OnQuestProgressChanged += this.HandleQuestProgressChanged;
		base.Awake();
	}

	private void Start()
	{
		if (SteamManager.Initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.Start()).MethodHandle;
			}
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
				if (GameManager.Get() != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameManager.Get().GameplayOverrides.EnableSteamAchievements)
					{
						using (Dictionary<int, QuestMetaData>.Enumerator enumerator = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.QuestMetaDatas.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<int, QuestMetaData> keyValuePair = enumerator.Current;
								if (keyValuePair.Value.CompletedCount > 0)
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
									QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(keyValuePair.Key);
									if (questTemplate.AchievmentType != AchievementType.None)
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
										SteamUserStats.SetAchievement("AR_QUEST_ID_" + keyValuePair.Key);
									}
								}
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
						foreach (QuestProgress questProgress in ClientGameManager.Get().GetPlayerAccountData().QuestComponent.Progress.Values)
						{
							int num;
							int num2;
							QuestItem.GetQuestProgress(questProgress.Id, out num, out num2);
							if (num <= 0)
							{
								for (;;)
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
								QuestTemplate questTemplate2 = QuestWideData.Get().GetQuestTemplate(questProgress.Id);
								if (questTemplate2.AchievmentType == AchievementType.None)
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
								}
								else
								{
									SteamUserStats.SetStat("AR_QUEST_ID_" + questProgress.Id, num);
								}
							}
						}
					}
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnQuestCompleteNotification -= this.HandleQuestCompleteNotification;
			ClientGameManager.Get().OnQuestProgressChanged -= this.HandleQuestProgressChanged;
		}
		QuestCompletePanel.s_instance = null;
	}

	public void RemoveQuestCompleteNotification(int questId)
	{
		using (List<QuestCompleteData>.Enumerator enumerator = this.m_recentlyCompletedQuests.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestCompleteData questCompleteData = enumerator.Current;
				if (questCompleteData.questId == questId)
				{
					this.m_recentlyCompletedQuests.Remove(questCompleteData);
					goto IL_60;
				}
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.RemoveQuestCompleteNotification(int)).MethodHandle;
			}
		}
		IL_60:
		this.Setup(true);
	}

	public int TotalQuestsToDisplayForGameOver()
	{
		return this.m_savedNotificationsForGameOver.Count;
	}

	public void DisplayGameOverQuestComplete(int index)
	{
		for (int i = 0; i < this.m_savedNotificationsForGameOver.Count; i++)
		{
			if (i <= index)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.DisplayGameOverQuestComplete(int)).MethodHandle;
				}
				if (this.m_savedNotificationsForGameOver[i].questId > 0)
				{
					this.DisplayNewQuestComplete(this.m_savedNotificationsForGameOver[i]);
					this.m_savedNotificationsForGameOver[i].questId = -1;
				}
			}
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
	}

	private void DisplayNewQuestComplete(QuestCompleteNotification message)
	{
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(message.questId);
		if (questTemplate != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.DisplayNewQuestComplete(QuestCompleteNotification)).MethodHandle;
			}
			if (questTemplate.HideCompletion)
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
				return;
			}
		}
		QuestCompleteData questCompleteData = new QuestCompleteData();
		questCompleteData.questId = message.questId;
		questCompleteData.rejectedCount = message.rejectedCount;
		questCompleteData.fadeTime = Time.time + 10f;
		this.m_recentlyCompletedQuests.Add(questCompleteData);
		if (questTemplate.AchievmentType != AchievementType.None)
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
			if (SteamManager.Initialized && GameManager.Get() != null)
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
				if (GameManager.Get().GameplayOverrides.EnableSteamAchievements)
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
					SteamUserStats.SetAchievement("AR_QUEST_ID_" + questTemplate.Index);
				}
			}
		}
		this.Setup(false);
		if (UIPlayerNavPanel.Get() != null)
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
			UIPlayerNavPanel.Get().NotifyQuestCompleted(this.m_recentlyCompletedQuests[0]);
		}
	}

	private void HandleQuestCompleteNotification(QuestCompleteNotification message)
	{
		if (UIGameOverScreen.Get() != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.HandleQuestCompleteNotification(QuestCompleteNotification)).MethodHandle;
			}
			if (UIGameOverScreen.Get().IsVisible)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_savedNotificationsForGameOver.Add(message);
				return;
			}
		}
		this.DisplayNewQuestComplete(message);
	}

	private void HandleQuestProgressChanged(QuestProgress[] questProgresses)
	{
		if (SteamManager.Initialized && !(GameManager.Get() == null))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.HandleQuestProgressChanged(QuestProgress[])).MethodHandle;
			}
			if (GameManager.Get().GameplayOverrides.EnableSteamAchievements)
			{
				for (int i = 0; i < questProgresses.Length; i++)
				{
					QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questProgresses[i].Id);
					if (questTemplate.AchievmentType != AchievementType.None)
					{
						int nData;
						int num;
						QuestItem.GetQuestProgress(questTemplate.Index, out nData, out num);
						SteamUserStats.SetStat("AR_QUEST_ID_" + questTemplate.Index, nData);
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
	}

	public void AddSpecialQuestNotification(int questId)
	{
		if (UIGameOverScreen.Get() != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.AddSpecialQuestNotification(int)).MethodHandle;
			}
			if (UIGameOverScreen.Get().IsVisible)
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
				this.m_savedNotificationsForGameOver.Add(new QuestCompleteNotification
				{
					questId = questId,
					rejectedCount = 0
				});
				return;
			}
		}
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questId);
		if (questTemplate != null && questTemplate.HideCompletion)
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
			return;
		}
		QuestCompleteData questCompleteData = new QuestCompleteData();
		questCompleteData.questId = questId;
		questCompleteData.rejectedCount = 0;
		questCompleteData.fadeTime = Time.time + 10f;
		this.m_recentlyCompletedQuests.Add(questCompleteData);
		this.Setup(false);
	}

	private void Setup(bool removedQuestCompleteNotification = false)
	{
		for (int i = 0; i < this.m_questItems.Length; i++)
		{
			QuestItem questItem = this.m_questItems[i];
			if (i < this.m_recentlyCompletedQuests.Count)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.Setup(bool)).MethodHandle;
				}
				QuestCompleteData questCompleteData = this.m_recentlyCompletedQuests[i];
				UIManager.SetGameObjectActive(questItem, true, null);
				questItem.SetState(QuestItemState.Finished);
				if (removedQuestCompleteNotification)
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
					UIAnimationEventManager.Get().PlayAnimation(questItem.m_animatorController, "contractItemDefaultIDLE", null, string.Empty, 0, 0f, true, false, null, null);
				}
				questItem.SetQuestId(questCompleteData.questId, questCompleteData.rejectedCount, true, removedQuestCompleteNotification);
				if (questItem.m_expandArrow != null)
				{
					UIManager.SetGameObjectActive(questItem.m_expandArrow, false, null);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(questItem, false, null);
			}
		}
		if (this.m_recentlyCompletedQuests.Count > 0)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_contractCompletedLabel != null)
			{
				UIManager.SetGameObjectActive(this.m_contractCompletedLabel, true, null);
			}
		}
		else if (this.m_contractCompletedLabel != null)
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
			UIManager.SetGameObjectActive(this.m_contractCompletedLabel, false, null);
		}
	}

	private void Update()
	{
		bool flag = false;
		if (!this.m_initialized)
		{
			this.m_initialized = true;
			flag = true;
		}
		bool flag2 = false;
		if (UIGameOverScreen.Get() != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCompletePanel.Update()).MethodHandle;
			}
			flag2 = UIGameOverScreen.Get().IsVisible;
		}
		else
		{
			if (SteamManager.Initialized)
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
				if (GameManager.Get() != null)
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
					if (GameManager.Get().GameplayOverrides.EnableSteamAchievements)
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
						for (int i = 0; i < this.m_savedNotificationsForGameOver.Count; i++)
						{
							int questId = this.m_savedNotificationsForGameOver[i].questId;
							if (questId >= 0)
							{
								QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questId);
								if (questTemplate.AchievmentType != AchievementType.None)
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
									SteamUserStats.SetAchievement("AR_QUEST_ID_" + questId);
								}
							}
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
					}
				}
			}
			this.m_savedNotificationsForGameOver.Clear();
		}
		if (this.m_recentlyCompletedQuests.Count > 0)
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
			if (!flag2)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				while (this.m_recentlyCompletedQuests.Count > 0)
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
					if (this.m_recentlyCompletedQuests[0].fadeTime >= Time.time)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							goto IL_191;
						}
					}
					else
					{
						this.m_recentlyCompletedQuests.RemoveAt(0);
						flag = true;
					}
				}
			}
		}
		IL_191:
		if (flag)
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
			this.Setup(false);
		}
	}
}
