using LobbyGameClientMessages;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRAFProgramScreen : UIScene
{
	public RectTransform m_container;

	public _SelectableBtn m_copyFriendKeyBtn;

	public _SelectableBtn m_sendInviteBtn;

	public TextMeshProUGUI m_playerRAFkey;

	public InputField m_inputfield;

	public TextMeshProUGUI m_pointTotal;

	public RectTransform[] m_barSections;

	public Image[] m_rewardIconImages;

	public _SelectableBtn[] m_rewardListBtns;

	public Image[] m_rewardIconImages2;

	public _SelectableBtn[] m_rewardListBtns2;

	public RectTransform[] m_onRewardNumbers;

	public RectTransform[] m_offRewardNumbers;

	private string m_friendKey = string.Empty;

	private bool m_requestedKey;

	private int m_RAFPoints;

	private static int[] m_barPoints = new int[7]
	{
		0,
		5,
		10,
		30,
		60,
		90,
		1000
	};

	private string[] m_tooltipTestDesc;

	private string[] m_tooltipTestLongDesc;

	private string[] m_tooltipTestDesc2;

	private string[] m_tooltipIconPath;

	private string[] m_tooltipIconPath2;

	private static UIRAFProgramScreen s_instance;

	public bool IsVisible
	{
		get;
		private set;
	}

	public static UIRAFProgramScreen Get()
	{
		return s_instance;
	}

	private void Start()
	{
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			while (true)
			{
				switch (7)
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
			HandleAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		ClientGameManager.Get().OnAccountDataUpdated += HandleAccountDataUpdated;
		for (int i = 0; i < m_rewardListBtns.Length; i++)
		{
			int index = i;
			m_rewardListBtns[i].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.RAFReward, (UITooltipBase tooltip) => SetupTooltip1(tooltip, index));
		}
		for (int j = 0; j < m_rewardListBtns2.Length; j++)
		{
			int index2 = j;
			m_rewardListBtns2[j].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.RAFReward, (UITooltipBase tooltip) => SetupTooltip2(tooltip, index2));
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.ReferAFriend;
	}

	public override void Awake()
	{
		s_instance = this;
		m_copyFriendKeyBtn.spriteController.callback = CopyFriendKeyBtnClicked;
		m_sendInviteBtn.spriteController.callback = SendInviteBtnClicked;
		m_tooltipTestDesc = new string[m_barPoints.Length];
		m_tooltipTestLongDesc = new string[m_barPoints.Length];
		m_tooltipTestDesc2 = new string[m_barPoints.Length];
		m_tooltipIconPath = new string[m_barPoints.Length];
		m_tooltipIconPath2 = new string[m_barPoints.Length];
		SetVisible(false);
		m_inputfield.onValueChanged.AddListener(OnTypeInput);
		BuildRewardUI();
		base.Awake();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			while (true)
			{
				switch (3)
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
			ClientGameManager.Get().OnAccountDataUpdated -= HandleAccountDataUpdated;
		}
		s_instance = null;
	}

	public void SetVisible(bool visible)
	{
		IsVisible = visible;
		if (visible)
		{
			if (m_friendKey.IsNullOrEmpty())
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
				CheckPersistedKey();
				if (m_friendKey.IsNullOrEmpty())
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
					if (!m_requestedKey)
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
						ClientGameManager.Get().SendCheckRAFStatusRequest(true, HandleCheckRAFStatusResponse);
						m_requestedKey = true;
					}
				}
			}
			if (UIPlayerProgressPanel.Get() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				UIPlayerProgressPanel.Get().SetVisible(false);
			}
			if (UIGGBoostPurchaseScreen.Get() != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				UIGGBoostPurchaseScreen.Get().SetVisible(false);
			}
		}
		UIManager.SetGameObjectActive(m_container, IsVisible);
		if (!(FriendListPanel.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			FriendListPanel.Get().m_recruitButton.SetSelected(IsVisible, false, string.Empty, string.Empty);
			return;
		}
	}

	public void CopyFriendKeyBtnClicked(BaseEventData data)
	{
		if (m_friendKey.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GUIUtility.systemCopyBuffer = m_friendKey;
			return;
		}
	}

	public void SendInviteBtnClicked(BaseEventData data)
	{
		string text = m_inputfield.text;
		bool flag = false;
		if (!(text != StringUtil.TR("EmailAddress", "SceneGlobal")))
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
			List<string> list = new List<string>();
			string[] array = text.Split(' ', ',', ';');
			foreach (string text2 in array)
			{
				if (text2.IsNullOrEmpty() || list.Contains(text2))
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (list.Count < 5)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(text2);
				}
				else
				{
					flag = true;
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (flag)
				{
					TextConsole.Get().Write(StringUtil.TR("TooManyEmailsEntered", "ReferAFriend"));
				}
				if (!list.IsNullOrEmpty())
				{
					ClientGameManager.Get().SendRAFReferralEmailsRequest(list, HandleSendRAFReferralEmailsResponse);
				}
				m_inputfield.text = string.Empty;
				return;
			}
		}
	}

	public void OnTypeInput(string textString)
	{
		if (m_inputfield.text.Length <= 255)
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
			m_inputfield.text = m_inputfield.text.Substring(0, 255);
			return;
		}
	}

	private void CheckPersistedKey()
	{
		if (!m_friendKey.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ClientGameManager.Get() != null)
			{
				string rAFReferralCode = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.RAFReferralCode;
				if (!rAFReferralCode.IsNullOrEmpty())
				{
					m_friendKey = rAFReferralCode;
				}
			}
			return;
		}
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		CheckPersistedKey();
		SetRAFPoints(accountData.BankComponent.GetCurrentAmount(CurrencyType.RAFPoints));
		m_playerRAFkey.text = m_friendKey;
		m_pointTotal.text = string.Format(StringUtil.TR("TotalRAFPoints", "ReferAFriend"), m_RAFPoints.ToString());
	}

	public void HandleCheckRAFStatusResponse(CheckRAFStatusResponse response)
	{
		if (response.ReferralCode.IsNullOrEmpty())
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
			m_friendKey = response.ReferralCode;
			if (IsVisible)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					m_playerRAFkey.text = m_friendKey;
					return;
				}
			}
			return;
		}
	}

	public void HandleSendRAFReferralEmailsResponse(SendRAFReferralEmailsResponse response)
	{
		TextConsole.Get().Write(StringUtil.TR("ReferralInvitationsSent", "ReferAFriend"));
	}

	private void SetRAFPoints(int points)
	{
		m_RAFPoints = points;
		for (int i = 0; i < m_barSections.Length; i++)
		{
			int num;
			if (m_RAFPoints > 0)
			{
				while (true)
				{
					switch (4)
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
				num = ((m_RAFPoints >= m_barPoints[i]) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			if (flag)
			{
				if (m_RAFPoints < m_barPoints[i + 1])
				{
					float y = (float)(m_RAFPoints - m_barPoints[i]) / (float)(m_barPoints[i + 1] - m_barPoints[i]);
					m_barSections[i].localScale = new Vector3(1f, y, 1f);
				}
				else
				{
					m_barSections[i].localScale = Vector3.one;
				}
			}
			UIManager.SetGameObjectActive(m_barSections[i], flag);
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			for (int j = 0; j < m_onRewardNumbers.Length; j++)
			{
				int num2;
				if (m_RAFPoints > 0)
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
					num2 = ((m_RAFPoints >= m_barPoints[j + 1]) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				bool flag2 = (byte)num2 != 0;
				UIManager.SetGameObjectActive(m_onRewardNumbers[j], flag2);
				UIManager.SetGameObjectActive(m_offRewardNumbers[j], !flag2);
			}
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

	private int FindRewardIndexFromPoints(int pointsRequired)
	{
		for (int i = 0; i < m_barPoints.Length; i++)
		{
			if (m_barPoints[i] != pointsRequired)
			{
				continue;
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
				return i - 1;
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			return -1;
		}
	}

	private void BuildRewardUI()
	{
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		for (int i = 0; i < m_rewardListBtns.Length; i++)
		{
			UIManager.SetGameObjectActive(m_rewardListBtns[i], false);
			UIManager.SetGameObjectActive(m_rewardListBtns2[i], false);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			GameBalanceVars.RAFReward[] rAFRewards = gameBalanceVars.RAFRewards;
			for (int j = 0; j < rAFRewards.Length; j++)
			{
				GameBalanceVars.RAFReward rAFReward = rAFRewards[j];
				if (!rAFReward.isEnabled)
				{
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (rAFReward.isRepeating)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (rAFReward.questId > QuestWideData.Get().m_quests.Count)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					Log.Warning("Refer a friend reward quest id {0} that is missing data in QuestWideData.", rAFReward.questId);
					continue;
				}
				if (j < m_rewardIconImages.Length)
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
					if (j < m_rewardIconImages2.Length)
					{
						QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(rAFReward.questId);
						int num = FindRewardIndexFromPoints(rAFReward.pointsRequired);
						if (num == -1)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							Log.Warning("Refer a friend reward quest id {0} point value doesn't match up to a UI icon", rAFReward.questId);
							continue;
						}
						string text = questTemplate.IconFilename;
						string text2 = string.Empty;
						m_tooltipTestDesc[num] = StringUtil.TR_QuestDescription(rAFReward.questId);
						m_tooltipTestLongDesc[num] = StringUtil.TR_QuestLongDescription(rAFReward.questId);
						m_tooltipTestDesc2[num] = string.Empty;
						QuestRewards rewards = questTemplate.Rewards;
						bool flag = false;
						using (List<QuestItemReward>.Enumerator enumerator = rewards.ItemRewards.GetEnumerator())
						{
							while (true)
							{
								if (!enumerator.MoveNext())
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
									break;
								}
								QuestItemReward current = enumerator.Current;
								InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(current.ItemTemplateId);
								if (!itemTemplate.IconPath.IsNullOrEmpty())
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (flag)
									{
										text2 = itemTemplate.IconPath;
										m_tooltipTestDesc2[num] = StringUtil.TR_InventoryItemName(current.ItemTemplateId);
										break;
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
									text = itemTemplate.IconPath;
									m_tooltipTestDesc[num] = StringUtil.TR_InventoryItemName(current.ItemTemplateId);
									flag = true;
								}
							}
						}
						m_tooltipIconPath[num] = text;
						m_tooltipIconPath2[num] = text2;
						m_rewardIconImages[num].sprite = (Sprite)Resources.Load(text, typeof(Sprite));
						UIManager.SetGameObjectActive(m_rewardListBtns[num], true);
						if (!text2.IsNullOrEmpty())
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							m_rewardIconImages2[num].sprite = (Sprite)Resources.Load(text2, typeof(Sprite));
							UIManager.SetGameObjectActive(m_rewardListBtns2[num], true);
						}
						continue;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				Log.Warning("Refer a friend reward quest id {0} cannot display reward icon", rAFReward.questId);
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private bool SetupTooltip1(UITooltipBase tooltip, int index)
	{
		if (m_tooltipTestDesc[index].IsNullOrEmpty())
		{
			if (m_tooltipTestLongDesc[index].IsNullOrEmpty())
			{
				return false;
			}
			while (true)
			{
				switch (3)
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
		}
		(tooltip as UIRecruitAFriendRewardTooltip).Setup(m_tooltipTestDesc[index], m_tooltipTestLongDesc[index], m_tooltipIconPath[index]);
		return true;
	}

	private bool SetupTooltip2(UITooltipBase tooltip, int index)
	{
		if (m_tooltipTestDesc2[index].IsNullOrEmpty())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_tooltipTestLongDesc[index].IsNullOrEmpty())
			{
				return false;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		(tooltip as UIRecruitAFriendRewardTooltip).Setup(m_tooltipTestDesc2[index], m_tooltipTestLongDesc[index], m_tooltipIconPath2[index]);
		return true;
	}
}
