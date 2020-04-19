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
		base.SetExpanded(this.m_expanded, true);
	}

	public void Setup(ActiveAlertMission alert)
	{
		UIManager.SetGameObjectActive(this.m_abandonElement, false, null);
		bool flag = alert != null;
		UIManager.SetGameObjectActive(this.m_remainingTime, flag, null);
		UIManager.SetGameObjectActive(this.m_progressText, flag, null);
		UIManager.SetGameObjectActive(this.m_progessFilled, flag, null);
		for (int i = 0; i < this.m_downArrows.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_downArrows[i], flag, null);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIAlertQuestEntry.Setup(ActiveAlertMission)).MethodHandle;
		}
		UIManager.SetGameObjectActive(this.m_contractedRewardsContainer, flag, null);
		UIManager.SetGameObjectActive(this.m_QuestDescription, flag, null);
		UIManager.SetGameObjectActive(this.m_expandedGroup, flag, null);
		UIManager.SetGameObjectActive(this.m_progressFillBg, flag, null);
		UIManager.SetGameObjectActive(this.m_questIcon, flag, null);
		this.m_btnHitBox.SetDisabled(!flag);
		UIManager.SetGameObjectActive(this.m_questCompleted, false, null);
		this.m_alert = alert;
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
			UIBaseQuestDisplayInfo uibaseQuestDisplayInfo = new UIBaseQuestDisplayInfo();
			uibaseQuestDisplayInfo.Setup(alert.QuestId);
			this.m_questIcon.sprite = Resources.Load<Sprite>(uibaseQuestDisplayInfo.QuestTemplateRef.ChallengeIconFileName);
			if (this.m_questIcon.sprite == null)
			{
				this.m_questIcon.sprite = Resources.Load<Sprite>(uibaseQuestDisplayInfo.QuestTemplateRef.IconFilename);
			}
			base.Setup(uibaseQuestDisplayInfo);
			this.CheckAndSetCompleted();
		}
	}

	protected override void SetProgress(int currentProgress, int maxProgress, QuestComponent questComponent, int questID)
	{
		if (this.CheckAndSetCompleted())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAlertQuestEntry.SetProgress(int, int, QuestComponent, int)).MethodHandle;
			}
			currentProgress = maxProgress;
		}
		base.SetProgress(currentProgress, maxProgress, questComponent, questID);
	}

	private bool CheckAndSetCompleted()
	{
		bool flag = false;
		if (this.m_alert != null)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			DateTime utcLastCompleted = clientGameManager.GetPlayerAccountData().QuestComponent.GetOrCreateQuestMetaData(this.m_alert.QuestId).UtcLastCompleted;
			if (utcLastCompleted > DateTime.MinValue)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIAlertQuestEntry.CheckAndSetCompleted()).MethodHandle;
				}
				DateTime t = utcLastCompleted - (clientGameManager.ServerUtcTime - clientGameManager.ServerPacificTime);
				bool flag2;
				if (t >= this.m_alert.StartTimePST)
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
					flag2 = (t <= this.m_alert.StartTimePST.AddHours((double)this.m_alert.DurationHours));
				}
				else
				{
					flag2 = false;
				}
				flag = flag2;
			}
		}
		UIManager.SetGameObjectActive(this.m_questCompleted, flag, null);
		return flag;
	}

	public override float GetExpandedHeight()
	{
		Canvas.ForceUpdateCanvases();
		float num = 15f;
		num += (this.m_headerElement.transform as RectTransform).rect.height;
		num += (this.m_detailsElement.transform as RectTransform).rect.height + 12f;
		return num + (this.m_rewardsElement.transform as RectTransform).rect.height;
	}

	protected override void PlayExpandAnimation()
	{
		UIManager.SetGameObjectActive(this.m_expandedGroup, true, null);
		this.m_animationController.Play("SeasonChallengeEntryExpandedIN");
	}

	protected override void PlayContractAnimation()
	{
		this.m_animationController.Play("SeasonChallengeEntryContractedIN");
	}
}
