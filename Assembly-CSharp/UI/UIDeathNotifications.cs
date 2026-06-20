using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDeathNotifications : MonoBehaviour
{
	public struct DeathDisplayInfo
	{
		public ActorData m_actorData;

		public bool IsAlly;
	}

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

	private List<DeathDisplayInfo> m_currentAllyDeathDisplays = new List<DeathDisplayInfo>();

	private List<DeathDisplayInfo> m_currentEnemyDeathDisplays = new List<DeathDisplayInfo>();

	private int LastTurnDisplayed = -1;

	private bool m_allyAceOccurred;

	private bool m_enemyAceOccurred;

	public static UIDeathNotifications Get()
	{
		return s_instance;
	}

	public void Awake()
	{
		s_instance = this;
	}

	private int GetNumAlliesTakedowns()
	{
		return m_currentAllyDeathDisplays.Count;
	}

	private int GetNumEnemyTakedowns()
	{
		return m_currentEnemyDeathDisplays.Count;
	}

	public void NotifyDeathOccurred(ActorData actorData, bool isAlly)
	{
		if (GameFlowData.Get().CurrentTurn != LastTurnDisplayed)
		{
			LastTurnDisplayed = GameFlowData.Get().CurrentTurn;
			m_currentAllyDeathDisplays.Clear();
			m_currentEnemyDeathDisplays.Clear();
			m_allyAceOccurred = false;
			m_enemyAceOccurred = false;
		}
		if (isAlly)
		{
			using (List<DeathDisplayInfo>.Enumerator enumerator = m_currentAllyDeathDisplays.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DeathDisplayInfo current = enumerator.Current;
					if (current.m_actorData == actorData)
					{
						while (true)
						{
							switch (6)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
				}
			}
		}
		else
		{
			using (List<DeathDisplayInfo>.Enumerator enumerator2 = m_currentEnemyDeathDisplays.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					DeathDisplayInfo current2 = enumerator2.Current;
					if (current2.m_actorData == actorData)
					{
						while (true)
						{
							switch (3)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
				}
			}
		}
		int num = -1;
		int num2 = -1;
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
		int numAlliesTakedowns = GetNumAlliesTakedowns();
		int numEnemyTakedowns = GetNumEnemyTakedowns();
		if (numAlliesTakedowns == 0)
		{
			if (numEnemyTakedowns == 0)
			{
				m_mainAnimator.Play("TakedownMainSingleIN");
				goto IL_0268;
			}
		}
		if (numAlliesTakedowns == 0 && isAlly)
		{
			if (numEnemyTakedowns > 0)
			{
				m_mainAnimator.Play("TakedownMainDoubleIN");
				goto IL_0268;
			}
		}
		if (numEnemyTakedowns == 0 && !isAlly && numAlliesTakedowns > 0)
		{
			m_mainAnimator.Play("TakedownMainDoubleIN");
		}
		goto IL_0268;
		IL_0268:
		DeathDisplayInfo item = default(DeathDisplayInfo);
		item.IsAlly = isAlly;
		item.m_actorData = actorData;
		if (isAlly)
		{
			m_currentAllyDeathDisplays.Add(item);
			numAlliesTakedowns = GetNumAlliesTakedowns();
			for (int i = 0; i < m_allyCharacterList.Length; i++)
			{
				if (i < m_currentAllyDeathDisplays.Count)
				{
					UIManager.SetGameObjectActive(m_allyCharacterList[i], true);
					Image obj = m_allyCharacterSprites[i];
					GameWideData gameWideData = GameWideData.Get();
					DeathDisplayInfo deathDisplayInfo = m_currentAllyDeathDisplays[i];
					obj.sprite = gameWideData.GetCharacterResourceLink(deathDisplayInfo.m_actorData.m_characterType).GetCharacterSelectIcon();
				}
				else
				{
					UIManager.SetGameObjectActive(m_allyCharacterList[i], false);
				}
			}
			UIManager.SetGameObjectActive(m_allyTakenDown, true);
			m_allyTakenDown.Play("TakedownDefaultIN", 0, 0f);
		}
		else
		{
			m_currentEnemyDeathDisplays.Add(item);
			numEnemyTakedowns = GetNumEnemyTakedowns();
			for (int j = 0; j < m_enemyCharacterList.Length; j++)
			{
				if (j < m_currentEnemyDeathDisplays.Count)
				{
					UIManager.SetGameObjectActive(m_enemyCharacterList[j], true);
					Image obj2 = m_enemyCharacterSprites[j];
					GameWideData gameWideData2 = GameWideData.Get();
					DeathDisplayInfo deathDisplayInfo2 = m_currentEnemyDeathDisplays[j];
					obj2.sprite = gameWideData2.GetCharacterResourceLink(deathDisplayInfo2.m_actorData.m_characterType).GetCharacterSelectIcon();
				}
				else
				{
					UIManager.SetGameObjectActive(m_enemyCharacterList[j], false);
				}
			}
			UIManager.SetGameObjectActive(m_enemyTakenDown, true);
			m_enemyTakenDown.Play("TakedownDefaultIN", 0, 0f);
		}
		if (AudioManager.s_deathAudio)
		{
			if (isAlly)
			{
				AudioManager.PostEvent("ui/ingame/takedown/ally");
			}
			else
			{
				AudioManager.PostEvent("ui/ingame/takedown/enemy");
			}
		}
		int numKills = numAlliesTakedowns;
		int singleKillType;
		if (m_currentAllyDeathDisplays.Count > 0)
		{
			DeathDisplayInfo deathDisplayInfo3 = m_currentAllyDeathDisplays[0];
			singleKillType = (int)deathDisplayInfo3.m_actorData.m_characterType;
		}
		else
		{
			singleKillType = 0;
		}
		string descriptionForNumKills = GetDescriptionForNumKills(numKills, (CharacterType)singleKillType);
		int numKills2 = numEnemyTakedowns;
		int singleKillType2;
		if (m_currentEnemyDeathDisplays.Count > 0)
		{
			DeathDisplayInfo deathDisplayInfo4 = m_currentEnemyDeathDisplays[0];
			singleKillType2 = (int)deathDisplayInfo4.m_actorData.m_characterType;
		}
		else
		{
			singleKillType2 = 0;
		}
		string descriptionForNumKills2 = GetDescriptionForNumKills(numKills2, (CharacterType)singleKillType2);
		m_allyTakeDownMainLabel.text = descriptionForNumKills;
		m_enemyTakeDownMainLabel.text = descriptionForNumKills2;
		m_allyAceOccurred = (num == numAlliesTakedowns);
		m_enemyAceOccurred = (num2 == numEnemyTakedowns);
	}

	private string GetDescriptionForNumKills(int numKills, CharacterType singleKillType)
	{
		string result = StringUtil.TR("TAKEDOWN", "Global");
		if (numKills == 1)
		{
			if (singleKillType != 0)
			{
				result = singleKillType.GetDisplayName();
				goto IL_00b4;
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
		goto IL_00b4;
		IL_00b4:
		return result;
	}

	private void Update()
	{
		if (m_allyAceOccurred)
		{
			bool flag = false;
			if (!m_allyTeamAce.gameObject.activeSelf)
			{
				if (m_allyTakenDown.gameObject.activeInHierarchy && m_allyTakenDown.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TakedownDefaultIDLE")
				{
					flag = true;
				}
			}
			if (flag)
			{
				m_allyAceOccurred = false;
				m_allyTakenDown.Play("TakedownDefaultOUT", 0, 0f);
				UIManager.SetGameObjectActive(m_allyTeamAce, true);
			}
		}
		if (m_enemyAceOccurred)
		{
			bool flag2 = false;
			if (!m_enemyTeamAce.gameObject.activeSelf)
			{
				if (m_enemyTakenDown.gameObject.activeInHierarchy)
				{
					if (m_enemyTakenDown.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TakedownDefaultIDLE")
					{
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				m_enemyAceOccurred = false;
				m_enemyTakenDown.Play("TakedownDefaultOUT", 0, 0f);
				UIManager.SetGameObjectActive(m_enemyTeamAce, true);
			}
		}
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get().gameState != GameState.BothTeams_Decision)
			{
				return;
			}
			while (true)
			{
				if (GetNumEnemyTakedowns() <= 0)
				{
					if (GetNumAlliesTakedowns() <= 0)
					{
						return;
					}
				}
				if (!m_enemyTeamAce.gameObject.activeSelf)
				{
					if (!m_allyTeamAce.gameObject.activeSelf)
					{
						UIManager.SetGameObjectActive(m_allyTakenDown, false);
						UIManager.SetGameObjectActive(m_enemyTakenDown, false);
						return;
					}
				}
				if (m_enemyTeamAce.gameObject.activeSelf)
				{
					if (m_enemyTeamAce.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TakedownDefaultIDLE")
					{
						m_enemyTeamAce.Play("TakedownDefaultOUT");
					}
				}
				if (!m_allyTeamAce.gameObject.activeSelf)
				{
					return;
				}
				while (true)
				{
					if (m_allyTeamAce.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TakedownDefaultIDLE")
					{
						while (true)
						{
							m_allyTeamAce.Play("TakedownDefaultOUT");
							return;
						}
					}
					return;
				}
			}
		}
	}
}
