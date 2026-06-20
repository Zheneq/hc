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
		if (!(textLabel.text != score.ToString()))
		{
			return;
		}
		while (true)
		{
			textLabel.text = score.ToString();
			if (friendly)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						FriendlyScoreAC.Play("ObjectiveScoreDefaultIN", 0, 0f);
						return;
					}
				}
			}
			EnemyScoreAC.Play("ObjectiveScoreDefaultIN", 0, 0f);
			return;
		}
	}

	public void SetInMatchValues(string myTeamLabel, Color myTeamColor, int myTeamScore, string myEnemyTeamLabel, Color myEnemyTeamColor, int myEnemyScore, string victoryConditionString, string phaseName)
	{
		if (myTeamScoreToDisplay != myTeamScore)
		{
			SetScoreText(FriendlyScore, myTeamScoreToDisplay, true);
			myTeamScoreToDisplay = myTeamScore;
			m_TimeMyScoreToChange = Time.time + delayTimeToChangeScore;
		}
		if (enemyTeamScoreToDisplay != myEnemyScore)
		{
			SetScoreText(EnemyScore, enemyTeamScoreToDisplay, false);
			enemyTeamScoreToDisplay = myEnemyScore;
			m_TimeEnemyScoreTChange = Time.time + delayTimeToChangeScore;
		}
		VictoryConditionLabel.text = victoryConditionString;
		PhaseNameLabel.text = phaseName;
		ResultLabel.text = string.Empty;
	}

	public void SetTutorialInMatchValue(string phaseName)
	{
		FriendlyScore.text = string.Empty;
		EnemyScore.text = string.Empty;
		VictoryConditionLabel.text = string.Empty;
		ResultLabel.text = string.Empty;
		PhaseNameLabel.text = phaseName;
	}

	public void SetPhaseIndicatorActive(bool active, int index)
	{
		if (0 > index || index >= PhaseIndicators.Length)
		{
			return;
		}
		while (true)
		{
			if (!PhaseIndicators[index].m_phaseImage.gameObject.activeSelf && active)
			{
				if (PhaseIndicators[index].gameObject.activeInHierarchy)
				{
					PhaseIndicators[index].m_phaseAnimationController.Play("PhaseIndicatorAnimation");
				}
			}
			UIManager.SetGameObjectActive(PhaseIndicators[index].m_phaseImage, active);
			return;
		}
	}

	public void SetEndMatchValues(string winnerString, Color winningTeamColor, string exitingString, int timeToExit)
	{
		FriendlyScore.text = string.Empty;
		EnemyScore.text = string.Empty;
		VictoryConditionLabel.text = exitingString + timeToExit;
		ResultLabel.text = winnerString;
		ResultLabel.color = winningTeamColor;
	}

	public void Update()
	{
		if (ObjectivePoints.Get() != null)
		{
			ObjectivePoints.Get().SetUpGameUI(this);
		}
		if (Tutorial.Get() != null)
		{
			Tutorial.Get().SetUpGameUI(this);
		}
		if (Time.time >= m_TimeMyScoreToChange)
		{
			SetScoreText(FriendlyScore, myTeamScoreToDisplay, true);
		}
		if (Time.time >= m_TimeEnemyScoreTChange)
		{
			SetScoreText(EnemyScore, enemyTeamScoreToDisplay, false);
		}
	}

	private void OnEnable()
	{
		Update();
	}
}
