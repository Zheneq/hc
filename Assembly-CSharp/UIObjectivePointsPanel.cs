using System;
using TMPro;
using UnityEngine;

public class UIObjectivePointsPanel : UIGameModePanel
{
	public TextMeshProUGUI FriendlyScore;

	public TextMeshProUGUI EnemyScore;

	public TextMeshProUGUI VictoryConditionLabel;

	public TextMeshProUGUI ResultLabel;

	public TextMeshProUGUI PhaseNameLabel;

	public _TopPhaseIndicator[] PhaseIndicators;

	public Animator FriendlyScoreAC;

	public Animator EnemyScoreAC;

	private float delayTimeToChangeScore = 0.5f;

	private int myTeamScoreToDisplay = -1;

	private int enemyTeamScoreToDisplay = -1;

	private float m_TimeMyScoreToChange = -1f;

	private float m_TimeEnemyScoreTChange = -1f;

	private void SetScoreText(TextMeshProUGUI textLabel, int score, bool friendly)
	{
		if (textLabel.text != score.ToString())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIObjectivePointsPanel.SetScoreText(TextMeshProUGUI, int, bool)).MethodHandle;
			}
			textLabel.text = score.ToString();
			if (friendly)
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
				this.FriendlyScoreAC.Play("ObjectiveScoreDefaultIN", 0, 0f);
			}
			else
			{
				this.EnemyScoreAC.Play("ObjectiveScoreDefaultIN", 0, 0f);
			}
		}
	}

	public void SetInMatchValues(string myTeamLabel, Color myTeamColor, int myTeamScore, string myEnemyTeamLabel, Color myEnemyTeamColor, int myEnemyScore, string victoryConditionString, string phaseName)
	{
		if (this.myTeamScoreToDisplay != myTeamScore)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIObjectivePointsPanel.SetInMatchValues(string, Color, int, string, Color, int, string, string)).MethodHandle;
			}
			this.SetScoreText(this.FriendlyScore, this.myTeamScoreToDisplay, true);
			this.myTeamScoreToDisplay = myTeamScore;
			this.m_TimeMyScoreToChange = Time.time + this.delayTimeToChangeScore;
		}
		if (this.enemyTeamScoreToDisplay != myEnemyScore)
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
			this.SetScoreText(this.EnemyScore, this.enemyTeamScoreToDisplay, false);
			this.enemyTeamScoreToDisplay = myEnemyScore;
			this.m_TimeEnemyScoreTChange = Time.time + this.delayTimeToChangeScore;
		}
		this.VictoryConditionLabel.text = victoryConditionString;
		this.PhaseNameLabel.text = phaseName;
		this.ResultLabel.text = string.Empty;
	}

	public void SetTutorialInMatchValue(string phaseName)
	{
		this.FriendlyScore.text = string.Empty;
		this.EnemyScore.text = string.Empty;
		this.VictoryConditionLabel.text = string.Empty;
		this.ResultLabel.text = string.Empty;
		this.PhaseNameLabel.text = phaseName;
	}

	public void SetPhaseIndicatorActive(bool active, int index)
	{
		if (0 <= index && index < this.PhaseIndicators.Length)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIObjectivePointsPanel.SetPhaseIndicatorActive(bool, int)).MethodHandle;
			}
			if (!this.PhaseIndicators[index].m_phaseImage.gameObject.activeSelf && active)
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
				if (this.PhaseIndicators[index].gameObject.activeInHierarchy)
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
					this.PhaseIndicators[index].m_phaseAnimationController.Play("PhaseIndicatorAnimation");
				}
			}
			UIManager.SetGameObjectActive(this.PhaseIndicators[index].m_phaseImage, active, null);
		}
	}

	public void SetEndMatchValues(string winnerString, Color winningTeamColor, string exitingString, int timeToExit)
	{
		this.FriendlyScore.text = string.Empty;
		this.EnemyScore.text = string.Empty;
		this.VictoryConditionLabel.text = exitingString + timeToExit;
		this.ResultLabel.text = winnerString;
		this.ResultLabel.color = winningTeamColor;
	}

	public void Update()
	{
		if (ObjectivePoints.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIObjectivePointsPanel.Update()).MethodHandle;
			}
			ObjectivePoints.Get().SetUpGameUI(this);
		}
		if (Tutorial.Get() != null)
		{
			Tutorial.Get().SetUpGameUI(this);
		}
		if (Time.time >= this.m_TimeMyScoreToChange)
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
			this.SetScoreText(this.FriendlyScore, this.myTeamScoreToDisplay, true);
		}
		if (Time.time >= this.m_TimeEnemyScoreTChange)
		{
			this.SetScoreText(this.EnemyScore, this.enemyTeamScoreToDisplay, false);
		}
	}

	private void OnEnable()
	{
		this.Update();
	}
}
