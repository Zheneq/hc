using UnityEngine;
using UnityEngine.EventSystems;

public class UIHalloweenIntroduction : MonoBehaviour
{
	public RectTransform m_WhatsInTheHauntLootMatrix;

	public Animator m_WhatsInTheHauntAC;

	public RectTransform m_HowToGetStuff;

	public _SelectableBtn m_WhatsInTheHauntLootMatrixNextBtn;

	public _SelectableBtn m_HowToGetStuffOKBtn;

	public _SelectableBtn m_HowToGetStuffBackBtn;

	private static UIHalloweenIntroduction s_instance;

	public static UIHalloweenIntroduction Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_WhatsInTheHauntLootMatrixNextBtn.spriteController.callback = NextBtnClicked;
		m_HowToGetStuffOKBtn.spriteController.callback = OKBtnClicked;
		m_HowToGetStuffBackBtn.spriteController.callback = BackBtnClicked;
		if (PlayerPrefs.HasKey("HalloweenEvent"))
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
			return;
		}
	}

	public void NextBtnClicked(BaseEventData data)
	{
		m_WhatsInTheHauntAC.enabled = false;
		UIManager.SetGameObjectActive(m_WhatsInTheHauntLootMatrix, false);
		UIManager.SetGameObjectActive(m_HowToGetStuff, true);
	}

	public void OKBtnClicked(BaseEventData data)
	{
		SetVisible(false);
	}

	public void BackBtnClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_WhatsInTheHauntLootMatrix, true);
		UIManager.SetGameObjectActive(m_HowToGetStuff, false);
	}

	public void SetVisible(bool visible)
	{
		if (visible)
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
					m_WhatsInTheHauntAC.enabled = true;
					UIManager.SetGameObjectActive(m_WhatsInTheHauntLootMatrix, true);
					UIManager.SetGameObjectActive(m_HowToGetStuff, false);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_WhatsInTheHauntLootMatrix, false);
		UIManager.SetGameObjectActive(m_HowToGetStuff, false);
	}
}
