using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonFactionEntryLevel : MonoBehaviour
{
	[Serializable]
	public class LevelState
	{
		public TextMeshProUGUI m_levelText;

		public RectTransform m_rewardContainer;

		public Image m_rewardIcon;

		public Image m_progressBar;
	}

	public LevelState m_completed;

	public LevelState m_notCompleted;

	public void Setup(Faction faction, int tierIndex, long remainingScore, bool showText)
	{
		float num = 0f;
		LevelState levelState;
		LevelState levelState2;
		if (faction.Tiers[tierIndex].ContributionToComplete > remainingScore)
		{
			levelState = m_completed;
			levelState2 = m_notCompleted;
			num = (float)remainingScore / (float)faction.Tiers[tierIndex].ContributionToComplete;
		}
		else
		{
			levelState = m_notCompleted;
			levelState2 = m_completed;
			num = 1f;
			float[] rBGA = FactionWideData.Get().GetRBGA(faction);
			levelState2.m_progressBar.color = new Color(rBGA[0], rBGA[1], rBGA[2], rBGA[3]);
		}
		UIManager.SetGameObjectActive(levelState.m_levelText, false);
		UIManager.SetGameObjectActive(levelState.m_progressBar, false);
		levelState2.m_levelText.text = (tierIndex + 1).ToString();
		UIManager.SetGameObjectActive(levelState2.m_levelText, showText);
		levelState2.m_progressBar.fillAmount = num;
		UIManager.SetGameObjectActive(levelState2.m_progressBar, true);
	}
}
