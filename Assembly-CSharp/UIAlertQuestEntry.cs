using System;
using UnityEngine;
using UnityEngine.UI;

public class UIAlertQuestEntry : UISeasonsBaseContract
{
	public Image m_questIcon;

	public RectTransform m_progressFillBg;

	public RectTransform m_questCompleted;

	private ActiveAlertMission m_alert;

	private void OnEnable()
	{
		SetExpanded(m_expanded, true);
	}

	public void Setup(ActiveAlertMission alert)
	{
		UIManager.SetGameObjectActive(m_abandonElement, false);
		bool flag = alert != null;
		UIManager.SetGameObjectActive(m_remainingTime, flag);
		UIManager.SetGameObjectActive(m_progressText, flag);
		UIManager.SetGameObjectActive(m_progessFilled, flag);
		for (int i = 0; i < m_downArrows.Length; i++)
		{
			UIManager.SetGameObjectActive(m_downArrows[i], flag);
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(m_contractedRewardsContainer, flag);
			UIManager.SetGameObjectActive(m_QuestDescription, flag);
			UIManager.SetGameObjectActive(m_expandedGroup, flag);
			UIManager.SetGameObjectActive(m_progressFillBg, flag);
			UIManager.SetGameObjectActive(m_questIcon, flag);
			m_btnHitBox.SetDisabled(!flag);
			UIManager.SetGameObjectActive(m_questCompleted, false);
			m_alert = alert;
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
				UIBaseQuestDisplayInfo uIBaseQuestDisplayInfo = new UIBaseQuestDisplayInfo();
				uIBaseQuestDisplayInfo.Setup(alert.QuestId);
				m_questIcon.sprite = Resources.Load<Sprite>(uIBaseQuestDisplayInfo.QuestTemplateRef.ChallengeIconFileName);
				if (m_questIcon.sprite == null)
				{
					m_questIcon.sprite = Resources.Load<Sprite>(uIBaseQuestDisplayInfo.QuestTemplateRef.IconFilename);
				}
				Setup(uIBaseQuestDisplayInfo);
				CheckAndSetCompleted();
				return;
			}
		}
	}

	protected override void SetProgress(int currentProgress, int maxProgress, QuestComponent questComponent, int questID)
	{
		if (CheckAndSetCompleted())
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
			currentProgress = maxProgress;
		}
		base.SetProgress(currentProgress, maxProgress, questComponent, questID);
	}

	private bool CheckAndSetCompleted()
	{
		bool flag = false;
		if (m_alert != null)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			DateTime utcLastCompleted = clientGameManager.GetPlayerAccountData().QuestComponent.GetOrCreateQuestMetaData(m_alert.QuestId).UtcLastCompleted;
			if (utcLastCompleted > DateTime.MinValue)
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
				DateTime t = utcLastCompleted - (clientGameManager.ServerUtcTime - clientGameManager.ServerPacificTime);
				int num;
				if (t >= m_alert.StartTimePST)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					num = ((t <= m_alert.StartTimePST.AddHours(m_alert.DurationHours)) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				flag = ((byte)num != 0);
			}
		}
		UIManager.SetGameObjectActive(m_questCompleted, flag);
		return flag;
	}

	public override float GetExpandedHeight()
	{
		Canvas.ForceUpdateCanvases();
		float num = 15f;
		num += (m_headerElement.transform as RectTransform).rect.height;
		num += (m_detailsElement.transform as RectTransform).rect.height + 12f;
		return num + (m_rewardsElement.transform as RectTransform).rect.height;
	}

	protected override void PlayExpandAnimation()
	{
		UIManager.SetGameObjectActive(m_expandedGroup, true);
		m_animationController.Play("SeasonChallengeEntryExpandedIN");
	}

	protected override void PlayContractAnimation()
	{
		m_animationController.Play("SeasonChallengeEntryContractedIN");
	}
}
