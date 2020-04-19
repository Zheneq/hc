using System;
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
		return GameOverWorldObjects.s_instance;
	}

	public override void Awake()
	{
		GameOverWorldObjects.s_instance = this;
		base.Awake();
		this.SetVisible(false);
	}

	public override SceneType GetSceneType()
	{
		return SceneType.GameOverResultEnvironment;
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_GameOverWorldCanvas, visible, null);
		if (visible)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverWorldObjects.SetVisible(bool)).MethodHandle;
			}
			this.m_worldResultAnimController.Play("ResultAnimation");
			UIGameOverScreen.Get().m_worldGGBtnHitBox.SetClickable(true);
			this.m_GameOverWorldCanvas.gameObject.transform.position = UIManager.Get().GetEnvirontmentCamera().gameObject.transform.position + new Vector3(0f, 0f, 12f);
		}
	}

	public void Setup(GameResult gameResult, Team friendlyTeam, float GGPack_XPMult)
	{
		GameType gameType = GameManager.Get().GameConfig.GameType;
		if (gameType == GameType.Tutorial)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverWorldObjects.Setup(GameResult, Team, float)).MethodHandle;
			}
			AudioManager.PostEvent("ui/endgame/victory", null);
			UIManager.SetGameObjectActive(this.m_worldDefeat, false, null);
			UIManager.SetGameObjectActive(this.m_worldTie, false, null);
			UIManager.SetGameObjectActive(this.m_worldVictory, false, null);
			UITutorialPanel.Get().ShowTutorialPassedStamp();
		}
		else
		{
			if (gameResult != GameResult.NoResult)
			{
				if (gameResult != GameResult.TieGame)
				{
					if (gameResult == GameResult.TeamAWon)
					{
						if (friendlyTeam == Team.TeamA)
						{
							goto IL_D9;
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
					if (gameResult == GameResult.TeamBWon)
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
						if (friendlyTeam == Team.TeamB)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								goto IL_D9;
							}
						}
					}
					AudioManager.PostEvent("ui/endgame/defeat", null);
					AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Defeat);
					UIManager.SetGameObjectActive(this.m_worldVictory, false, null);
					UIManager.SetGameObjectActive(this.m_worldTie, false, null);
					UIManager.SetGameObjectActive(this.m_worldDefeat, true, null);
					goto IL_15F;
					IL_D9:
					AudioManager.PostEvent("ui/endgame/victory", null);
					AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Victory);
					UIManager.SetGameObjectActive(this.m_worldTie, false, null);
					UIManager.SetGameObjectActive(this.m_worldDefeat, false, null);
					UIManager.SetGameObjectActive(this.m_worldVictory, true, null);
					goto IL_15F;
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
			UIManager.SetGameObjectActive(this.m_worldVictory, false, null);
			UIManager.SetGameObjectActive(this.m_worldTie, true, null);
			UIManager.SetGameObjectActive(this.m_worldDefeat, false, null);
		}
		IL_15F:
		UIGameOverScreen.Get().SetWorldGGPackText(string.Format(StringUtil.TR("EndGameGGBonuses", "GameOver"), Mathf.RoundToInt((GGPack_XPMult - 1f) * 100f)));
	}

	public void NotificationArrived()
	{
		if (UIGameOverScreen.Get().m_worldGGBtnHitBox.IsClickable())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverWorldObjects.NotificationArrived()).MethodHandle;
			}
			UIGameOverScreen.Get().m_worldGGBtnHitBox.SetClickable(false);
		}
		UIManager.SetGameObjectActive(this.m_GameOverWorldCanvas, false, null);
	}
}
