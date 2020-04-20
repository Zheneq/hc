using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

	private static int[] m_barPoints = new int[]
	{
		0,
		5,
		0xA,
		0x1E,
		0x3C,
		0x5A,
		0x3E8
	};

	private string[] m_tooltipTestDesc;

	private string[] m_tooltipTestLongDesc;

	private string[] m_tooltipTestDesc2;

	private string[] m_tooltipIconPath;

	private string[] m_tooltipIconPath2;

	private static UIRAFProgramScreen s_instance;

	public bool IsVisible { get; private set; }

	public static UIRAFProgramScreen Get()
	{
		return UIRAFProgramScreen.s_instance;
	}

	private void Start()
	{
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			this.HandleAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		ClientGameManager.Get().OnAccountDataUpdated += this.HandleAccountDataUpdated;
		for (int i = 0; i < this.m_rewardListBtns.Length; i++)
		{
			int index = i;
			this.m_rewardListBtns[i].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.RAFReward, (UITooltipBase tooltip) => this.SetupTooltip1(tooltip, index), null);
		}
		for (int j = 0; j < this.m_rewardListBtns2.Length; j++)
		{
			int index = j;
			this.m_rewardListBtns2[j].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.RAFReward, (UITooltipBase tooltip) => this.SetupTooltip2(tooltip, index), null);
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.ReferAFriend;
	}

	public override void Awake()
	{
		UIRAFProgramScreen.s_instance = this;
		this.m_copyFriendKeyBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CopyFriendKeyBtnClicked);
		this.m_sendInviteBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SendInviteBtnClicked);
		this.m_tooltipTestDesc = new string[UIRAFProgramScreen.m_barPoints.Length];
		this.m_tooltipTestLongDesc = new string[UIRAFProgramScreen.m_barPoints.Length];
		this.m_tooltipTestDesc2 = new string[UIRAFProgramScreen.m_barPoints.Length];
		this.m_tooltipIconPath = new string[UIRAFProgramScreen.m_barPoints.Length];
		this.m_tooltipIconPath2 = new string[UIRAFProgramScreen.m_barPoints.Length];
		this.SetVisible(false);
		this.m_inputfield.onValueChanged.AddListener(new UnityAction<string>(this.OnTypeInput));
		this.BuildRewardUI();
		base.Awake();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= this.HandleAccountDataUpdated;
		}
		UIRAFProgramScreen.s_instance = null;
	}

	public void SetVisible(bool visible)
	{
		this.IsVisible = visible;
		if (visible)
		{
			if (this.m_friendKey.IsNullOrEmpty())
			{
				this.CheckPersistedKey();
				if (this.m_friendKey.IsNullOrEmpty())
				{
					if (!this.m_requestedKey)
					{
						ClientGameManager.Get().SendCheckRAFStatusRequest(true, new Action<CheckRAFStatusResponse>(this.HandleCheckRAFStatusResponse));
						this.m_requestedKey = true;
					}
				}
			}
			if (UIPlayerProgressPanel.Get() != null)
			{
				UIPlayerProgressPanel.Get().SetVisible(false, true);
			}
			if (UIGGBoostPurchaseScreen.Get() != null)
			{
				UIGGBoostPurchaseScreen.Get().SetVisible(false);
			}
		}
		UIManager.SetGameObjectActive(this.m_container, this.IsVisible, null);
		if (FriendListPanel.Get() != null)
		{
			FriendListPanel.Get().m_recruitButton.SetSelected(this.IsVisible, false, string.Empty, string.Empty);
		}
	}

	public void CopyFriendKeyBtnClicked(BaseEventData data)
	{
		if (!this.m_friendKey.IsNullOrEmpty())
		{
			GUIUtility.systemCopyBuffer = this.m_friendKey;
		}
	}

	public void SendInviteBtnClicked(BaseEventData data)
	{
		string text = this.m_inputfield.text;
		bool flag = false;
		if (text != StringUtil.TR("EmailAddress", "SceneGlobal"))
		{
			List<string> list = new List<string>();
			foreach (string text2 in text.Split(new char[]
			{
				' ',
				',',
				';'
			}))
			{
				if (!text2.IsNullOrEmpty() && !list.Contains(text2))
				{
					if (list.Count < 5)
					{
						list.Add(text2);
					}
					else
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				TextConsole.Get().Write(StringUtil.TR("TooManyEmailsEntered", "ReferAFriend"), ConsoleMessageType.SystemMessage);
			}
			if (!list.IsNullOrEmpty<string>())
			{
				ClientGameManager.Get().SendRAFReferralEmailsRequest(list, new Action<SendRAFReferralEmailsResponse>(this.HandleSendRAFReferralEmailsResponse));
			}
			this.m_inputfield.text = string.Empty;
		}
	}

	public void OnTypeInput(string textString)
	{
		if (this.m_inputfield.text.Length > 0xFF)
		{
			this.m_inputfield.text = this.m_inputfield.text.Substring(0, 0xFF);
		}
	}

	private void CheckPersistedKey()
	{
		if (this.m_friendKey.IsNullOrEmpty())
		{
			if (ClientGameManager.Get() != null)
			{
				string rafreferralCode = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.RAFReferralCode;
				if (!rafreferralCode.IsNullOrEmpty())
				{
					this.m_friendKey = rafreferralCode;
				}
			}
		}
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		this.CheckPersistedKey();
		this.SetRAFPoints(accountData.BankComponent.GetCurrentAmount(CurrencyType.RAFPoints));
		this.m_playerRAFkey.text = this.m_friendKey;
		this.m_pointTotal.text = string.Format(StringUtil.TR("TotalRAFPoints", "ReferAFriend"), this.m_RAFPoints.ToString());
	}

	public void HandleCheckRAFStatusResponse(CheckRAFStatusResponse response)
	{
		if (!response.ReferralCode.IsNullOrEmpty())
		{
			this.m_friendKey = response.ReferralCode;
			if (this.IsVisible)
			{
				this.m_playerRAFkey.text = this.m_friendKey;
			}
		}
	}

	public void HandleSendRAFReferralEmailsResponse(SendRAFReferralEmailsResponse response)
	{
		TextConsole.Get().Write(StringUtil.TR("ReferralInvitationsSent", "ReferAFriend"), ConsoleMessageType.SystemMessage);
	}

	private void SetRAFPoints(int points)
	{
		this.m_RAFPoints = points;
		for (int i = 0; i < this.m_barSections.Length; i++)
		{
			bool flag;
			if (this.m_RAFPoints > 0)
			{
				flag = (this.m_RAFPoints >= UIRAFProgramScreen.m_barPoints[i]);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				if (this.m_RAFPoints < UIRAFProgramScreen.m_barPoints[i + 1])
				{
					float y = (float)(this.m_RAFPoints - UIRAFProgramScreen.m_barPoints[i]) / (float)(UIRAFProgramScreen.m_barPoints[i + 1] - UIRAFProgramScreen.m_barPoints[i]);
					this.m_barSections[i].localScale = new Vector3(1f, y, 1f);
				}
				else
				{
					this.m_barSections[i].localScale = Vector3.one;
				}
			}
			UIManager.SetGameObjectActive(this.m_barSections[i], flag2, null);
		}
		for (int j = 0; j < this.m_onRewardNumbers.Length; j++)
		{
			bool flag3;
			if (this.m_RAFPoints > 0)
			{
				flag3 = (this.m_RAFPoints >= UIRAFProgramScreen.m_barPoints[j + 1]);
			}
			else
			{
				flag3 = false;
			}
			bool flag4 = flag3;
			UIManager.SetGameObjectActive(this.m_onRewardNumbers[j], flag4, null);
			UIManager.SetGameObjectActive(this.m_offRewardNumbers[j], !flag4, null);
		}
	}

	private int FindRewardIndexFromPoints(int pointsRequired)
	{
		for (int i = 0; i < UIRAFProgramScreen.m_barPoints.Length; i++)
		{
			if (UIRAFProgramScreen.m_barPoints[i] == pointsRequired)
			{
				return i - 1;
			}
		}
		return -1;
	}

	private void BuildRewardUI()
	{
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			return;
		}
		for (int i = 0; i < this.m_rewardListBtns.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_rewardListBtns[i], false, null);
			UIManager.SetGameObjectActive(this.m_rewardListBtns2[i], false, null);
		}
		GameBalanceVars.RAFReward[] rafrewards = gameBalanceVars.RAFRewards;
		for (int j = 0; j < rafrewards.Length; j++)
		{
			GameBalanceVars.RAFReward rafreward = rafrewards[j];
			if (rafreward.isEnabled)
			{
				if (rafreward.isRepeating)
				{
				}
				if (rafreward.questId > QuestWideData.Get().m_quests.Count)
				{
					Log.Warning("Refer a friend reward quest id {0} that is missing data in QuestWideData.", new object[]
					{
						rafreward.questId
					});
				}
				else
				{
					if (j < this.m_rewardIconImages.Length)
					{
						if (j >= this.m_rewardIconImages2.Length)
						{
						}
						else
						{
							QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(rafreward.questId);
							int num = this.FindRewardIndexFromPoints(rafreward.pointsRequired);
							if (num == -1)
							{
								Log.Warning("Refer a friend reward quest id {0} point value doesn't match up to a UI icon", new object[]
								{
									rafreward.questId
								});
								goto IL_33A;
							}
							string text = questTemplate.IconFilename;
							string text2 = string.Empty;
							this.m_tooltipTestDesc[num] = StringUtil.TR_QuestDescription(rafreward.questId);
							this.m_tooltipTestLongDesc[num] = StringUtil.TR_QuestLongDescription(rafreward.questId);
							this.m_tooltipTestDesc2[num] = string.Empty;
							QuestRewards rewards = questTemplate.Rewards;
							bool flag = false;
							using (List<QuestItemReward>.Enumerator enumerator = rewards.ItemRewards.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									QuestItemReward questItemReward = enumerator.Current;
									InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(questItemReward.ItemTemplateId);
									if (!itemTemplate.IconPath.IsNullOrEmpty())
									{
										if (flag)
										{
											text2 = itemTemplate.IconPath;
											this.m_tooltipTestDesc2[num] = StringUtil.TR_InventoryItemName(questItemReward.ItemTemplateId);
											goto IL_2A3;
										}
										text = itemTemplate.IconPath;
										this.m_tooltipTestDesc[num] = StringUtil.TR_InventoryItemName(questItemReward.ItemTemplateId);
										flag = true;
									}
								}
							}
							IL_2A3:
							this.m_tooltipIconPath[num] = text;
							this.m_tooltipIconPath2[num] = text2;
							this.m_rewardIconImages[num].sprite = (Sprite)Resources.Load(text, typeof(Sprite));
							UIManager.SetGameObjectActive(this.m_rewardListBtns[num], true, null);
							if (!text2.IsNullOrEmpty())
							{
								this.m_rewardIconImages2[num].sprite = (Sprite)Resources.Load(text2, typeof(Sprite));
								UIManager.SetGameObjectActive(this.m_rewardListBtns2[num], true, null);
								goto IL_33A;
							}
							goto IL_33A;
						}
					}
					Log.Warning("Refer a friend reward quest id {0} cannot display reward icon", new object[]
					{
						rafreward.questId
					});
				}
			}
			IL_33A:;
		}
	}

	private bool SetupTooltip1(UITooltipBase tooltip, int index)
	{
		if (this.m_tooltipTestDesc[index].IsNullOrEmpty())
		{
			if (this.m_tooltipTestLongDesc[index].IsNullOrEmpty())
			{
				return false;
			}
		}
		(tooltip as UIRecruitAFriendRewardTooltip).Setup(this.m_tooltipTestDesc[index], this.m_tooltipTestLongDesc[index], this.m_tooltipIconPath[index]);
		return true;
	}

	private bool SetupTooltip2(UITooltipBase tooltip, int index)
	{
		if (this.m_tooltipTestDesc2[index].IsNullOrEmpty())
		{
			if (this.m_tooltipTestLongDesc[index].IsNullOrEmpty())
			{
				return false;
			}
		}
		(tooltip as UIRecruitAFriendRewardTooltip).Setup(this.m_tooltipTestDesc2[index], this.m_tooltipTestLongDesc[index], this.m_tooltipIconPath2[index]);
		return true;
	}
}
