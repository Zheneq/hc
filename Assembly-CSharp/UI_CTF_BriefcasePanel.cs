using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CTF_BriefcasePanel : MonoBehaviour
{
	public RectTransform m_container;

	public RectTransform m_neutralContainer;

	public Animator m_redContainer;

	public Animator m_blueContainer;

	public Image[] m_freelancerImages;

	public _SelectableBtn[] m_freelancerBtn;

	public ImageFilledSloped[] m_briefcaseFillAmount;

	public TextMeshProUGUI[] m_briefcaseLimitText;

	public _SelectableBtn m_briefcaseHitbox;

	private static UI_CTF_BriefcasePanel s_instance;

	public bool m_initialized;

	public static UI_CTF_BriefcasePanel Get()
	{
		return UI_CTF_BriefcasePanel.s_instance;
	}

	private void Awake()
	{
		UI_CTF_BriefcasePanel.s_instance = this;
		UIManager.SetGameObjectActive(this.m_container, false, null);
		this.m_briefcaseHitbox.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BriefcaseBoxClicked);
	}

	public void BriefcaseBoxClicked(BaseEventData data)
	{
		this.CenterCameraToFlagCarrier();
	}

	public bool UpdateDamageForFlagHolder(float currentDamage, float thresholdDamage)
	{
		ActorData mainFlagCarrier_Client = CaptureTheFlag.GetMainFlagCarrier_Client();
		if (mainFlagCarrier_Client != null)
		{
			float num = thresholdDamage - currentDamage;
			if (num < 0f)
			{
				num = 0f;
			}
			string text = string.Format("{0}/{1}", num, thresholdDamage);
			float num2 = num / thresholdDamage;
			for (int i = 0; i < this.m_briefcaseLimitText.Length; i++)
			{
				this.m_briefcaseLimitText[i].text = text;
			}
			for (int j = 0; j < this.m_briefcaseFillAmount.Length; j++)
			{
				this.m_briefcaseFillAmount[j].fillAmount = num2;
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateBriefcaseThreshold(mainFlagCarrier_Client, num2);
			return true;
		}
		return false;
	}

	public void UpdateFlagHolder(ActorData oldHolder, ActorData newHolder)
	{
		this.CheckFlagCarrierStatus(newHolder);
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.NotifyFlagStatusChange(newHolder, newHolder != null);
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.NotifyFlagStatusChange(oldHolder, false);
		}
	}

	private void SetFlagCarrierSprite(ActorData data)
	{
		for (int i = 0; i < this.m_freelancerImages.Length; i++)
		{
			this.m_freelancerImages[i].sprite = data.GetAliveHUDIcon();
		}
	}

	private bool TeamsMatchForCase(Team selfTeam, Team targetTeam)
	{
		bool result;
		if (selfTeam != targetTeam)
		{
			result = (selfTeam == Team.Invalid && targetTeam == Team.TeamA);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private void CheckFlagCarrierStatus(ActorData flagCarrier)
	{
		bool flag;
		bool flag2;
		if (flagCarrier != null)
		{
			this.SetFlagCarrierSprite(flagCarrier);
			if (!(GameFlowData.Get() == null))
			{
				if (GameFlowData.Get().LocalPlayerData == null)
				{
				}
				else
				{
					if (this.TeamsMatchForCase(GameFlowData.Get().LocalPlayerData.GetTeamViewing(), flagCarrier.GetTeam()))
					{
						flag = true;
						flag2 = false;
						goto IL_CF;
					}
					if (this.TeamsMatchForCase(GameFlowData.Get().LocalPlayerData.GetTeamViewing(), flagCarrier.GetOpposingTeam()))
					{
						flag = false;
						flag2 = true;
						goto IL_CF;
					}
					flag = false;
					flag2 = false;
					goto IL_CF;
				}
			}
			flag = false;
			flag2 = false;
			IL_CF:;
		}
		else
		{
			flag = false;
			flag2 = false;
		}
		if (flag)
		{
			UIManager.SetGameObjectActive(this.m_neutralContainer, false, null);
			UIManager.SetGameObjectActive(this.m_redContainer, false, null);
			UIManager.SetGameObjectActive(this.m_blueContainer, true, null);
			this.m_blueContainer.Play("BriefcaseUIDefaultIN");
		}
		else if (flag2)
		{
			UIManager.SetGameObjectActive(this.m_neutralContainer, false, null);
			UIManager.SetGameObjectActive(this.m_redContainer, true, null);
			UIManager.SetGameObjectActive(this.m_blueContainer, false, null);
			this.m_redContainer.Play("BriefcaseUIDefaultIN");
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_neutralContainer, true, null);
			this.m_redContainer.Play("BriefcaseUIDefaultOUT");
			this.m_blueContainer.Play("BriefcaseUIDefaultOUT");
		}
	}

	public void Setup(CaptureTheFlag ctfInfo)
	{
		if (this.m_initialized)
		{
			return;
		}
		UIManager.SetGameObjectActive(this.m_container, true, null);
		ActorData mainFlagCarrier_Client = CaptureTheFlag.GetMainFlagCarrier_Client();
		this.CheckFlagCarrierStatus(mainFlagCarrier_Client);
		this.m_initialized = true;
	}

	public void CenterCameraToFlagCarrier()
	{
		ActorData mainFlagCarrier_Client = CaptureTheFlag.GetMainFlagCarrier_Client();
		if (CameraManager.Get() != null)
		{
			if (mainFlagCarrier_Client != null)
			{
				CameraManager.Get().SetTargetObject(mainFlagCarrier_Client.gameObject, CameraManager.CameraTargetReason.UserFocusingOnActor);
			}
		}
	}
}
