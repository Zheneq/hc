using UnityEngine;

public class GameOverWorldObjects : UIScene
{
	public Canvas m_GameOverWorldCanvas;

	public RectTransform m_worldVictory;

	public RectTransform m_worldDefeat;

	public RectTransform m_worldTie;

	public Animator m_worldResultAnimController;

	private static GameOverWorldObjects s_instance;

	public static GameOverWorldObjects Get()
	{
		return s_instance;
	}

	public override void Awake()
	{
		s_instance = this;
		base.Awake();
		SetVisible(false);
	}

	public override SceneType GetSceneType()
	{
		return SceneType.GameOverResultEnvironment;
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_GameOverWorldCanvas, visible);
		if (!visible)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_worldResultAnimController.Play("ResultAnimation");
			UIGameOverScreen.Get().m_worldGGBtnHitBox.SetClickable(true);
			m_GameOverWorldCanvas.gameObject.transform.position = UIManager.Get().GetEnvirontmentCamera().gameObject.transform.position + new Vector3(0f, 0f, 12f);
			return;
		}
	}

	public void Setup(GameResult gameResult, Team friendlyTeam, float GGPack_XPMult)
	{
		GameType gameType = GameManager.Get().GameConfig.GameType;
		if (gameType == GameType.Tutorial)
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
			AudioManager.PostEvent("ui/endgame/victory");
			UIManager.SetGameObjectActive(m_worldDefeat, false);
			UIManager.SetGameObjectActive(m_worldTie, false);
			UIManager.SetGameObjectActive(m_worldVictory, false);
			UITutorialPanel.Get().ShowTutorialPassedStamp();
		}
		else
		{
			if (gameResult != 0)
			{
				if (gameResult != GameResult.TieGame)
				{
					if (gameResult == GameResult.TeamAWon)
					{
						if (friendlyTeam == Team.TeamA)
						{
							goto IL_00d9;
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
					if (gameResult == GameResult.TeamBWon)
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
						if (friendlyTeam == Team.TeamB)
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
							goto IL_00d9;
						}
					}
					AudioManager.PostEvent("ui/endgame/defeat");
					AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Defeat);
					UIManager.SetGameObjectActive(m_worldVictory, false);
					UIManager.SetGameObjectActive(m_worldTie, false);
					UIManager.SetGameObjectActive(m_worldDefeat, true);
					goto IL_015f;
				}
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
			UIManager.SetGameObjectActive(m_worldVictory, false);
			UIManager.SetGameObjectActive(m_worldTie, true);
			UIManager.SetGameObjectActive(m_worldDefeat, false);
		}
		goto IL_015f;
		IL_015f:
		UIGameOverScreen.Get().SetWorldGGPackText(string.Format(StringUtil.TR("EndGameGGBonuses", "GameOver"), Mathf.RoundToInt((GGPack_XPMult - 1f) * 100f)));
		return;
		IL_00d9:
		AudioManager.PostEvent("ui/endgame/victory");
		AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Victory);
		UIManager.SetGameObjectActive(m_worldTie, false);
		UIManager.SetGameObjectActive(m_worldDefeat, false);
		UIManager.SetGameObjectActive(m_worldVictory, true);
		goto IL_015f;
	}

	public void NotificationArrived()
	{
		if (UIGameOverScreen.Get().m_worldGGBtnHitBox.IsClickable())
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
			UIGameOverScreen.Get().m_worldGGBtnHitBox.SetClickable(false);
		}
		UIManager.SetGameObjectActive(m_GameOverWorldCanvas, false);
	}
}
