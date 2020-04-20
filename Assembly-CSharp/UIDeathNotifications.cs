using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDeathNotifications : MonoBehaviour
{
	public Animator m_mainAnimator;

	public Animator m_enemyTakenDown;

	public Animator m_enemyTeamAce;

	public Animator m_allyTakenDown;

	public Animator m_allyTeamAce;

	public TextMeshProUGUI m_enemyTakeDownMainLabel;

	public TextMeshProUGUI m_enemyTakeDownDescriptionLabel;

	public TextMeshProUGUI m_allyTakeDownMainLabel;

	public TextMeshProUGUI m_allyTakeDownDescriptionLabel;

	public RectTransform[] m_allyCharacterList;

	public RectTransform[] m_enemyCharacterList;

	public Image[] m_allyCharacterSprites;

	public Image[] m_enemyCharacterSprites;

	private static UIDeathNotifications s_instance;

	private List<UIDeathNotifications.DeathDisplayInfo> m_currentAllyDeathDisplays = new List<UIDeathNotifications.DeathDisplayInfo>();

	private List<UIDeathNotifications.DeathDisplayInfo> m_currentEnemyDeathDisplays = new List<UIDeathNotifications.DeathDisplayInfo>();

	private int LastTurnDisplayed = -1;

	private bool m_allyAceOccurred;

	private bool m_enemyAceOccurred;

	public static UIDeathNotifications Get()
	{
		return UIDeathNotifications.s_instance;
	}

	public void Awake()
	{
		UIDeathNotifications.s_instance = this;
	}

	private int GetNumAlliesTakedowns()
	{
		return this.m_currentAllyDeathDisplays.Count;
	}

	private int GetNumEnemyTakedowns()
	{
		return this.m_currentEnemyDeathDisplays.Count;
	}

	public void NotifyDeathOccurred(ActorData actorData, bool isAlly)
	{
		if (GameFlowData.Get().CurrentTurn != this.LastTurnDisplayed)
		{
			this.LastTurnDisplayed = GameFlowData.Get().CurrentTurn;
			this.m_currentAllyDeathDisplays.Clear();
			this.m_currentEnemyDeathDisplays.Clear();
			this.m_allyAceOccurred = false;
			this.m_enemyAceOccurred = false;
		}
		if (isAlly)
		{
			using (List<UIDeathNotifications.DeathDisplayInfo>.Enumerator enumerator = this.m_currentAllyDeathDisplays.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIDeathNotifications.DeathDisplayInfo deathDisplayInfo = enumerator.Current;
					if (deathDisplayInfo.m_actorData == actorData)
					{
						return;
					}
				}
			}
		}
		else
		{
			using (List<UIDeathNotifications.DeathDisplayInfo>.Enumerator enumerator2 = this.m_currentEnemyDeathDisplays.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UIDeathNotifications.DeathDisplayInfo deathDisplayInfo2 = enumerator2.Current;
					if (deathDisplayInfo2.m_actorData == actorData)
					{
						return;
					}
				}
			}
		}
		int num;
		int num2;
		if (GameManager.Get().GameInfo.GameConfig.GameType == GameType.Tutorial)
		{
			num = 4;
			num2 = 4;
		}
		else if (GameFlowData.Get().LocalPlayerData.GetTeamViewing() == Team.TeamA)
		{
			num = GameManager.Get().GameInfo.GameConfig.TeamAPlayers;
			num2 = GameManager.Get().GameInfo.GameConfig.TeamBPlayers;
		}
		else
		{
			num = GameManager.Get().GameInfo.GameConfig.TeamBPlayers;
			num2 = GameManager.Get().GameInfo.GameConfig.TeamAPlayers;
		}
		int numAlliesTakedowns = this.GetNumAlliesTakedowns();
		int numEnemyTakedowns = this.GetNumEnemyTakedowns();
		if (numAlliesTakedowns == 0)
		{
			if (numEnemyTakedowns == 0)
			{
				this.m_mainAnimator.Play("TakedownMainSingleIN");
				goto IL_268;
			}
		}
		if (numAlliesTakedowns == 0 && isAlly)
		{
			if (numEnemyTakedowns > 0)
			{
				this.m_mainAnimator.Play("TakedownMainDoubleIN");
				goto IL_268;
			}
		}
		if (numEnemyTakedowns == 0 && !isAlly && numAlliesTakedowns > 0)
		{
			this.m_mainAnimator.Play("TakedownMainDoubleIN");
		}
		IL_268:
		UIDeathNotifications.DeathDisplayInfo item;
		item.IsAlly = isAlly;
		item.m_actorData = actorData;
		if (isAlly)
		{
			this.m_currentAllyDeathDisplays.Add(item);
			numAlliesTakedowns = this.GetNumAlliesTakedowns();
			for (int i = 0; i < this.m_allyCharacterList.Length; i++)
			{
				if (i < this.m_currentAllyDeathDisplays.Count)
				{
					UIManager.SetGameObjectActive(this.m_allyCharacterList[i], true, null);
					this.m_allyCharacterSprites[i].sprite = GameWideData.Get().GetCharacterResourceLink(this.m_currentAllyDeathDisplays[i].m_actorData.m_characterType).GetCharacterSelectIcon();
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_allyCharacterList[i], false, null);
				}
			}
			UIManager.SetGameObjectActive(this.m_allyTakenDown, true, null);
			this.m_allyTakenDown.Play("TakedownDefaultIN", 0, 0f);
		}
		else
		{
			this.m_currentEnemyDeathDisplays.Add(item);
			numEnemyTakedowns = this.GetNumEnemyTakedowns();
			for (int j = 0; j < this.m_enemyCharacterList.Length; j++)
			{
				if (j < this.m_currentEnemyDeathDisplays.Count)
				{
					UIManager.SetGameObjectActive(this.m_enemyCharacterList[j], true, null);
					this.m_enemyCharacterSprites[j].sprite = GameWideData.Get().GetCharacterResourceLink(this.m_currentEnemyDeathDisplays[j].m_actorData.m_characterType).GetCharacterSelectIcon();
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_enemyCharacterList[j], false, null);
				}
			}
			UIManager.SetGameObjectActive(this.m_enemyTakenDown, true, null);
			this.m_enemyTakenDown.Play("TakedownDefaultIN", 0, 0f);
		}
		if (AudioManager.s_deathAudio)
		{
			if (isAlly)
			{
				AudioManager.PostEvent("ui/ingame/takedown/ally", null);
			}
			else
			{
				AudioManager.PostEvent("ui/ingame/takedown/enemy", null);
			}
		}
		int numKills = numAlliesTakedowns;
		CharacterType singleKillType;
		if (this.m_currentAllyDeathDisplays.Count > 0)
		{
			singleKillType = this.m_currentAllyDeathDisplays[0].m_actorData.m_characterType;
		}
		else
		{
			singleKillType = CharacterType.None;
		}
		string descriptionForNumKills = this.GetDescriptionForNumKills(numKills, singleKillType);
		int numKills2 = numEnemyTakedowns;
		CharacterType singleKillType2;
		if (this.m_currentEnemyDeathDisplays.Count > 0)
		{
			singleKillType2 = this.m_currentEnemyDeathDisplays[0].m_actorData.m_characterType;
		}
		else
		{
			singleKillType2 = CharacterType.None;
		}
		string descriptionForNumKills2 = this.GetDescriptionForNumKills(numKills2, singleKillType2);
		this.m_allyTakeDownMainLabel.text = descriptionForNumKills;
		this.m_enemyTakeDownMainLabel.text = descriptionForNumKills2;
		this.m_allyAceOccurred = (num == numAlliesTakedowns);
		this.m_enemyAceOccurred = (num2 == numEnemyTakedowns);
	}

	private string GetDescriptionForNumKills(int numKills, CharacterType singleKillType)
	{
		string result = StringUtil.TR("TAKEDOWN", "Global");
		if (numKills == 1)
		{
			if (singleKillType != CharacterType.None)
			{
				return singleKillType.GetDisplayName();
			}
		}
		if (numKills == 2)
		{
			result = StringUtil.TR("DOUBLEKILL", "Global");
		}
		else if (numKills == 3)
		{
			result = StringUtil.TR("TRIPLEKILL", "Global");
		}
		else if (numKills == 4)
		{
			result = StringUtil.TR("QUADRAKILL", "Global");
		}
		else if (numKills == 5)
		{
			result = StringUtil.TR("PENTAKILL", "Global");
		}
		return result;
	}

	private void Update()
	{
		if (this.m_allyAceOccurred)
		{
			bool flag = false;
			if (!this.m_allyTeamAce.gameObject.activeSelf)
			{
				if (this.m_allyTakenDown.gameObject.activeInHierarchy && this.m_allyTakenDown.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TakedownDefaultIDLE")
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.m_allyAceOccurred = false;
				this.m_allyTakenDown.Play("TakedownDefaultOUT", 0, 0f);
				UIManager.SetGameObjectActive(this.m_allyTeamAce, true, null);
			}
		}
		if (this.m_enemyAceOccurred)
		{
			bool flag2 = false;
			if (!this.m_enemyTeamAce.gameObject.activeSelf)
			{
				if (this.m_enemyTakenDown.gameObject.activeInHierarchy)
				{
					if (this.m_enemyTakenDown.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TakedownDefaultIDLE")
					{
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				this.m_enemyAceOccurred = false;
				this.m_enemyTakenDown.Play("TakedownDefaultOUT", 0, 0f);
				UIManager.SetGameObjectActive(this.m_enemyTeamAce, true, null);
			}
		}
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
			{
				if (this.GetNumEnemyTakedowns() <= 0)
				{
					if (this.GetNumAlliesTakedowns() <= 0)
					{
						return;
					}
				}
				if (!this.m_enemyTeamAce.gameObject.activeSelf)
				{
					if (!this.m_allyTeamAce.gameObject.activeSelf)
					{
						UIManager.SetGameObjectActive(this.m_allyTakenDown, false, null);
						UIManager.SetGameObjectActive(this.m_enemyTakenDown, false, null);
						return;
					}
				}
				if (this.m_enemyTeamAce.gameObject.activeSelf)
				{
					if (this.m_enemyTeamAce.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TakedownDefaultIDLE")
					{
						this.m_enemyTeamAce.Play("TakedownDefaultOUT");
					}
				}
				if (this.m_allyTeamAce.gameObject.activeSelf)
				{
					if (this.m_allyTeamAce.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TakedownDefaultIDLE")
					{
						this.m_allyTeamAce.Play("TakedownDefaultOUT");
					}
				}
			}
		}
	}

	public struct DeathDisplayInfo
	{
		public ActorData m_actorData;

		public bool IsAlly;
	}
}
