using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonFactionEntryLevel : MonoBehaviour
{
	public UISeasonFactionEntryLevel.LevelState m_completed;

	public UISeasonFactionEntryLevel.LevelState m_notCompleted;

	public void Setup(Faction faction, int tierIndex, long remainingScore, bool showText)
	{
		UISeasonFactionEntryLevel.LevelState levelState;
		UISeasonFactionEntryLevel.LevelState levelState2;
		float fillAmount;
		if (faction.Tiers[tierIndex].ContributionToComplete > remainingScore)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonFactionEntryLevel.Setup(Faction, int, long, bool)).MethodHandle;
			}
			levelState = this.m_completed;
			levelState2 = this.m_notCompleted;
			fillAmount = (float)remainingScore / (float)faction.Tiers[tierIndex].ContributionToComplete;
		}
		else
		{
			levelState = this.m_notCompleted;
			levelState2 = this.m_completed;
			fillAmount = 1f;
			float[] rbga = FactionWideData.Get().GetRBGA(faction);
			levelState2.m_progressBar.color = new Color(rbga[0], rbga[1], rbga[2], rbga[3]);
		}
		UIManager.SetGameObjectActive(levelState.m_levelText, false, null);
		UIManager.SetGameObjectActive(levelState.m_progressBar, false, null);
		levelState2.m_levelText.text = (tierIndex + 1).ToString();
		UIManager.SetGameObjectActive(levelState2.m_levelText, showText, null);
		levelState2.m_progressBar.fillAmount = fillAmount;
		UIManager.SetGameObjectActive(levelState2.m_progressBar, true, null);
	}

	[Serializable]
	public class LevelState
	{
		public TextMeshProUGUI m_levelText;

		public RectTransform m_rewardContainer;

		public Image m_rewardIcon;

		public Image m_progressBar;
	}
}
