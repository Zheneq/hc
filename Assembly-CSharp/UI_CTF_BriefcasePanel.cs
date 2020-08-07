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
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_container, false);
		m_briefcaseHitbox.spriteController.callback = BriefcaseBoxClicked;
	}

	public void BriefcaseBoxClicked(BaseEventData data)
	{
		CenterCameraToFlagCarrier();
	}

	public bool UpdateDamageForFlagHolder(float currentDamage, float thresholdDamage)
	{
		ActorData mainFlagCarrier_Client = CaptureTheFlag.GetMainFlagCarrier_Client();
		if (mainFlagCarrier_Client != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					float num = thresholdDamage - currentDamage;
					if (num < 0f)
					{
						num = 0f;
					}
					string text = $"{num}/{thresholdDamage}";
					float num2 = num / thresholdDamage;
					for (int i = 0; i < m_briefcaseLimitText.Length; i++)
					{
						m_briefcaseLimitText[i].text = text;
					}
					for (int j = 0; j < m_briefcaseFillAmount.Length; j++)
					{
						m_briefcaseFillAmount[j].fillAmount = num2;
					}
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateBriefcaseThreshold(mainFlagCarrier_Client, num2);
					return true;
				}
				}
			}
		}
		return false;
	}

	public void UpdateFlagHolder(ActorData oldHolder, ActorData newHolder)
	{
		CheckFlagCarrierStatus(newHolder);
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.NotifyFlagStatusChange(newHolder, newHolder != null);
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.NotifyFlagStatusChange(oldHolder, false);
			return;
		}
	}

	private void SetFlagCarrierSprite(ActorData data)
	{
		for (int i = 0; i < m_freelancerImages.Length; i++)
		{
			m_freelancerImages[i].sprite = data.GetAliveHUDIcon();
		}
		while (true)
		{
			return;
		}
	}

	private bool TeamsMatchForCase(Team selfTeam, Team targetTeam)
	{
		int result;
		if (selfTeam != targetTeam)
		{
			result = ((selfTeam == Team.Invalid && targetTeam == Team.TeamA) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private void CheckFlagCarrierStatus(ActorData flagCarrier)
	{
		bool flag;
		bool flag2;
		if (flagCarrier != null)
		{
			SetFlagCarrierSprite(flagCarrier);
			if (!(GameFlowData.Get() == null))
			{
				if (!(GameFlowData.Get().LocalPlayerData == null))
				{
					if (TeamsMatchForCase(GameFlowData.Get().LocalPlayerData.GetTeamViewing(), flagCarrier.GetTeam()))
					{
						flag = true;
						flag2 = false;
					}
					else if (TeamsMatchForCase(GameFlowData.Get().LocalPlayerData.GetTeamViewing(), flagCarrier.GetEnemyTeam()))
					{
						flag = false;
						flag2 = true;
					}
					else
					{
						flag = false;
						flag2 = false;
					}
					goto IL_00d5;
				}
			}
			flag = false;
			flag2 = false;
		}
		else
		{
			flag = false;
			flag2 = false;
		}
		goto IL_00d5;
		IL_00d5:
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_neutralContainer, false);
					UIManager.SetGameObjectActive(m_redContainer, false);
					UIManager.SetGameObjectActive(m_blueContainer, true);
					m_blueContainer.Play("BriefcaseUIDefaultIN");
					return;
				}
			}
		}
		if (flag2)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_neutralContainer, false);
					UIManager.SetGameObjectActive(m_redContainer, true);
					UIManager.SetGameObjectActive(m_blueContainer, false);
					m_redContainer.Play("BriefcaseUIDefaultIN");
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_neutralContainer, true);
		m_redContainer.Play("BriefcaseUIDefaultOUT");
		m_blueContainer.Play("BriefcaseUIDefaultOUT");
	}

	public void Setup(CaptureTheFlag ctfInfo)
	{
		if (m_initialized)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_container, true);
		ActorData mainFlagCarrier_Client = CaptureTheFlag.GetMainFlagCarrier_Client();
		CheckFlagCarrierStatus(mainFlagCarrier_Client);
		m_initialized = true;
	}

	public void CenterCameraToFlagCarrier()
	{
		ActorData mainFlagCarrier_Client = CaptureTheFlag.GetMainFlagCarrier_Client();
		if (!(CameraManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (mainFlagCarrier_Client != null)
			{
				while (true)
				{
					CameraManager.Get().SetTargetObject(mainFlagCarrier_Client.gameObject, CameraManager.CameraTargetReason.UserFocusingOnActor);
					return;
				}
			}
			return;
		}
	}
}
