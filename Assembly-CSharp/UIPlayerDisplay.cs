using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerDisplay : MonoBehaviour
{
	public Animator m_animationController;

	public Image m_background;

	public Image m_centerPiece;

	public Image m_tutorialBar;

	public TextMeshProUGUI m_tutorialText;

	public GameObject m_tutorialCameraControlsPanel;

	public GameObject m_tutorialCombatPhasePanel;

	public GameObject m_tutorialDashPhasePanel;

	public GameObject m_tutorialPrepPhasePanel;

	public UIPlayerStatus[] m_teamPlayerIcons;

	public UIPlayerStatus[] m_enemyPlayerIcons;

	private bool m_PanelVisibility;

	private List<string> m_animsToPlayQueue;

	private string showDisplayAnimName = "TopDisplayPanelShow";

	private void Awake()
	{
		UIManager.SetGameObjectActive(this.m_tutorialBar, false, null);
		UIManager.SetGameObjectActive(this.m_tutorialText, false, null);
		UIManager.SetGameObjectActive(this.m_tutorialCameraControlsPanel, false, null);
		UIManager.SetGameObjectActive(this.m_tutorialCombatPhasePanel, false, null);
		UIManager.SetGameObjectActive(this.m_tutorialDashPhasePanel, false, null);
		UIManager.SetGameObjectActive(this.m_tutorialPrepPhasePanel, false, null);
	}

	private void Start()
	{
		this.m_animsToPlayQueue = new List<string>();
	}

	private void Update()
	{
		if (!this.IsAnimationPlaying())
		{
			if (this.m_animsToPlayQueue.Count > 0)
			{
				this.SetDisplaysVisible(true);
				this.m_animsToPlayQueue.RemoveAt(0);
			}
		}
		this.ProcessTeamsForSpectator();
	}

	public void UpdateCatalysts(ActorData theActor, List<Ability> cardAbilities)
	{
		int i = 0;
		while (i < this.m_teamPlayerIcons.Length)
		{
			if (this.m_teamPlayerIcons[i].ActorDataRef == theActor)
			{
				this.m_teamPlayerIcons[i].UpdateCatalysts(cardAbilities);
				break;
				
			}
			else
			{
				i++;
			}
		}
		for (int j = 0; j < this.m_enemyPlayerIcons.Length; j++)
		{
			if (this.m_enemyPlayerIcons[j].ActorDataRef == theActor)
			{
				this.m_enemyPlayerIcons[j].UpdateCatalysts(cardAbilities);
				return;
			}
		}
	}

	private bool IsAnimationPlaying()
	{
		if (this.m_animationController.GetCurrentAnimatorClipInfo(0) != null)
		{
			if (this.m_animationController.GetCurrentAnimatorClipInfo(0).Length > 0)
			{
				return this.m_animationController.GetCurrentAnimatorClipInfo(0)[0].clip.name != "EmptyAnimation";
			}
		}
		return false;
	}

	private void SetDisplaysVisible(bool visible)
	{
		int num;
		if (this.m_teamPlayerIcons[0].IsActiveDisplay())
		{
			num = GameFlowData.Get().GetPlayerAndBotTeamMembers(this.m_teamPlayerIcons[0].GetTeam()).Count;
		}
		else
		{
			num = this.m_teamPlayerIcons.Length;
		}
		for (int i = 0; i < this.m_teamPlayerIcons.Length; i++)
		{
			if (i < num && this.m_teamPlayerIcons[i].IsActiveDisplay())
			{
				bool doActive = visible;
				if (SinglePlayerManager.Get())
				{
					if (SinglePlayerManager.Get().GetTeamPlayerIconForceOff(i))
					{
						doActive = false;
					}
				}
				UIManager.SetGameObjectActive(this.m_teamPlayerIcons[i], doActive, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_teamPlayerIcons[i], false, null);
			}
		}
		if (this.m_enemyPlayerIcons[0].IsActiveDisplay())
		{
			num = GameFlowData.Get().GetPlayerAndBotTeamMembers(this.m_enemyPlayerIcons[0].GetTeam()).Count;
		}
		else
		{
			num = this.m_enemyPlayerIcons.Length;
		}
		int j = 0;
		while (j < this.m_enemyPlayerIcons.Length)
		{
			if (j >= num)
			{
				goto IL_187;
			}
			if (!this.m_enemyPlayerIcons[j].IsActiveDisplay())
			{
				goto IL_187;
			}
			bool doActive2 = visible;
			if (SinglePlayerManager.Get() && SinglePlayerManager.Get().GetEnemyPlayerIconForceOff(j))
			{
				doActive2 = false;
			}
			UIManager.SetGameObjectActive(this.m_enemyPlayerIcons[j], doActive2, null);
			IL_196:
			j++;
			continue;
			IL_187:
			UIManager.SetGameObjectActive(this.m_enemyPlayerIcons[j], false, null);
			goto IL_196;
		}
	}

	public void DisplayPanelShowAnimDone()
	{
	}

	public void DisplayPanelHideAnimDone()
	{
		if (this.m_PanelVisibility)
		{
			this.SetDisplaysVisible(true);
		}
		else
		{
			this.SetDisplaysVisible(false);
		}
	}

	public void NotifyDecisionTimerShow()
	{
		if (!this.IsAnimationPlaying())
		{
			this.m_PanelVisibility = true;
			this.SetDisplaysVisible(true);
		}
		else if (this.m_animsToPlayQueue != null)
		{
			if (!this.m_animsToPlayQueue.Contains(this.showDisplayAnimName))
			{
			}
		}
	}

	public void NotifyLockedIn(bool isLocked)
	{
		for (int i = 0; i < this.m_teamPlayerIcons.Length; i++)
		{
			this.m_teamPlayerIcons[i].NotifyLockedIn(isLocked);
		}
		for (int j = 0; j < this.m_enemyPlayerIcons.Length; j++)
		{
			this.m_enemyPlayerIcons[j].NotifyLockedIn(isLocked);
		}
	}

	private void ProcessTeamsForSpectator()
	{
		bool flag;
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().PlayerInfo != null && ClientGameManager.Get().PlayerInfo.TeamId == Team.Spectator)
			{
				flag = true;
				goto IL_8D;
			}
		}
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().PlayerInfo != null)
			{
				flag = (GameManager.Get().PlayerInfo.TeamId == Team.Spectator);
				goto IL_8A;
			}
		}
		flag = false;
		IL_8A:
		IL_8D:
		bool flag2 = flag;
		if (!(GameFlowData.Get() == null))
		{
			if (!(GameFlowData.Get().LocalPlayerData == null))
			{
				if (flag2)
				{
					Team team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					if (team != Team.TeamA)
					{
						if (team != Team.Invalid)
						{
							goto IL_104;
						}
					}
					team = Team.TeamA;
					IL_104:
					List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(team);
					GameFlowData gameFlowData = GameFlowData.Get();
					Team team2;
					if (team == Team.TeamA)
					{
						team2 = Team.TeamB;
					}
					else
					{
						team2 = Team.TeamA;
					}
					List<ActorData> playerAndBotTeamMembers2 = gameFlowData.GetPlayerAndBotTeamMembers(team2);
					int num = 0;
					using (List<ActorData>.Enumerator enumerator = playerAndBotTeamMembers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData actor = enumerator.Current;
							if (num >= this.m_teamPlayerIcons.Length)
							{
								goto IL_208;
							}
							if (!GameplayUtils.IsPlayerControlled(actor))
							{
							}
							else
							{
								this.m_teamPlayerIcons[num].Setup(actor);
								bool doActive = true;
								if (SinglePlayerManager.Get())
								{
									if (SinglePlayerManager.Get().GetTeamPlayerIconForceOff(num))
									{
										doActive = false;
									}
								}
								UIManager.SetGameObjectActive(this.m_teamPlayerIcons[num], doActive, null);
								num++;
							}
						}
					}
					IL_208:
					num = 0;
					using (List<ActorData>.Enumerator enumerator2 = playerAndBotTeamMembers2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ActorData actor2 = enumerator2.Current;
							if (num >= this.m_enemyPlayerIcons.Length)
							{
								return;
							}
							if (GameplayUtils.IsPlayerControlled(actor2))
							{
								this.m_enemyPlayerIcons[num].Setup(actor2);
								bool doActive2 = true;
								if (SinglePlayerManager.Get())
								{
									if (SinglePlayerManager.Get().GetEnemyPlayerIconForceOff(num))
									{
										doActive2 = false;
									}
								}
								UIManager.SetGameObjectActive(this.m_enemyPlayerIcons[num], doActive2, null);
								num++;
							}
						}
					}
					return;
				}
			}
		}
	}

	public void ProcessTeams()
	{
		if (GameFlowData.Get() == null || GameManager.Get().GameConfig.GameType == GameType.Tutorial)
		{
			return;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.GetTeam());
			int num = 0;
			using (List<ActorData>.Enumerator enumerator = playerAndBotTeamMembers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					if (num >= this.m_teamPlayerIcons.Length)
					{
						goto IL_DB;
					}
					if (!GameplayUtils.IsPlayerControlled(actor))
					{
					}
					else
					{
						this.m_teamPlayerIcons[num].Setup(actor);
						num++;
					}
				}
			}
			IL_DB:
			playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.GetOpposingTeam());
			num = 0;
			using (List<ActorData>.Enumerator enumerator2 = playerAndBotTeamMembers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actor2 = enumerator2.Current;
					if (num >= this.m_enemyPlayerIcons.Length)
					{
						goto IL_163;
					}
					if (GameplayUtils.IsPlayerControlled(actor2))
					{
						this.m_enemyPlayerIcons[num].Setup(actor2);
						num++;
					}
				}
			}
			IL_163:;
		}
		else
		{
			this.ProcessTeamsForSpectator();
		}
		this.SetDisplaysVisible(true);
	}
}
