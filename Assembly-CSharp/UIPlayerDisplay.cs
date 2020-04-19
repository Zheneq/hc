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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerDisplay.Update()).MethodHandle;
			}
			if (this.m_animsToPlayQueue.Count > 0)
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
				IL_4D:
				for (int j = 0; j < this.m_enemyPlayerIcons.Length; j++)
				{
					if (this.m_enemyPlayerIcons[j].ActorDataRef == theActor)
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
						this.m_enemyPlayerIcons[j].UpdateCatalysts(cardAbilities);
						return;
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					return;
				}
			}
			else
			{
				i++;
			}
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerDisplay.UpdateCatalysts(ActorData, List<Ability>)).MethodHandle;
			goto IL_4D;
		}
		goto IL_4D;
	}

	private bool IsAnimationPlaying()
	{
		if (this.m_animationController.GetCurrentAnimatorClipInfo(0) != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerDisplay.IsAnimationPlaying()).MethodHandle;
			}
			if (this.m_animationController.GetCurrentAnimatorClipInfo(0).Length > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerDisplay.SetDisplaysVisible(bool)).MethodHandle;
			}
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				bool doActive = visible;
				if (SinglePlayerManager.Get())
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
					if (SinglePlayerManager.Get().GetTeamPlayerIconForceOff(i))
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!this.m_enemyPlayerIcons[j].IsActiveDisplay())
			{
				goto IL_187;
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

	public void DisplayPanelShowAnimDone()
	{
	}

	public void DisplayPanelHideAnimDone()
	{
		if (this.m_PanelVisibility)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerDisplay.DisplayPanelHideAnimDone()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerDisplay.NotifyDecisionTimerShow()).MethodHandle;
			}
			this.m_PanelVisibility = true;
			this.SetDisplaysVisible(true);
		}
		else if (this.m_animsToPlayQueue != null)
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
			if (!this.m_animsToPlayQueue.Contains(this.showDisplayAnimName))
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
		}
	}

	public void NotifyLockedIn(bool isLocked)
	{
		for (int i = 0; i < this.m_teamPlayerIcons.Length; i++)
		{
			this.m_teamPlayerIcons[i].NotifyLockedIn(isLocked);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerDisplay.NotifyLockedIn(bool)).MethodHandle;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerDisplay.ProcessTeamsForSpectator()).MethodHandle;
			}
			if (ClientGameManager.Get().PlayerInfo != null && ClientGameManager.Get().PlayerInfo.TeamId == Team.Spectator)
			{
				flag = true;
				goto IL_8D;
			}
		}
		if (GameManager.Get() != null)
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
			if (GameManager.Get().PlayerInfo != null)
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(GameFlowData.Get().LocalPlayerData == null))
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
				if (flag2)
				{
					Team team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					if (team != Team.TeamA)
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
						if (team != Team.Invalid)
						{
							goto IL_104;
						}
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					team = Team.TeamA;
					IL_104:
					List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(team);
					GameFlowData gameFlowData = GameFlowData.Get();
					Team team2;
					if (team == Team.TeamA)
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
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								goto IL_208;
							}
							if (!GameplayUtils.IsPlayerControlled(actor))
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
							}
							else
							{
								this.m_teamPlayerIcons[num].Setup(actor);
								bool doActive = true;
								if (SinglePlayerManager.Get())
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
									if (SinglePlayerManager.Get().GetTeamPlayerIconForceOff(num))
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
										doActive = false;
									}
								}
								UIManager.SetGameObjectActive(this.m_teamPlayerIcons[num], doActive, null);
								num++;
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
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								return;
							}
							if (GameplayUtils.IsPlayerControlled(actor2))
							{
								this.m_enemyPlayerIcons[num].Setup(actor2);
								bool doActive2 = true;
								if (SinglePlayerManager.Get())
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
									if (SinglePlayerManager.Get().GetEnemyPlayerIconForceOff(num))
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
										doActive2 = false;
									}
								}
								UIManager.SetGameObjectActive(this.m_enemyPlayerIcons[num], doActive2, null);
								num++;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerDisplay.ProcessTeams()).MethodHandle;
			}
			List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.\u000E());
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
					else
					{
						this.m_teamPlayerIcons[num].Setup(actor);
						num++;
					}
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			IL_DB:
			playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.\u0012());
			num = 0;
			using (List<ActorData>.Enumerator enumerator2 = playerAndBotTeamMembers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actor2 = enumerator2.Current;
					if (num >= this.m_enemyPlayerIcons.Length)
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
						goto IL_163;
					}
					if (GameplayUtils.IsPlayerControlled(actor2))
					{
						this.m_enemyPlayerIcons[num].Setup(actor2);
						num++;
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
			IL_163:;
		}
		else
		{
			this.ProcessTeamsForSpectator();
		}
		this.SetDisplaysVisible(true);
	}
}
