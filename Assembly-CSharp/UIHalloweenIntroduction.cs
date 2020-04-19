using System;
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
		return UIHalloweenIntroduction.s_instance;
	}

	private void Awake()
	{
		UIHalloweenIntroduction.s_instance = this;
		this.m_WhatsInTheHauntLootMatrixNextBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NextBtnClicked);
		this.m_HowToGetStuffOKBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OKBtnClicked);
		this.m_HowToGetStuffBackBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BackBtnClicked);
		if (!PlayerPrefs.HasKey("HalloweenEvent"))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIHalloweenIntroduction.Awake()).MethodHandle;
			}
		}
	}

	public void NextBtnClicked(BaseEventData data)
	{
		this.m_WhatsInTheHauntAC.enabled = false;
		UIManager.SetGameObjectActive(this.m_WhatsInTheHauntLootMatrix, false, null);
		UIManager.SetGameObjectActive(this.m_HowToGetStuff, true, null);
	}

	public void OKBtnClicked(BaseEventData data)
	{
		this.SetVisible(false);
	}

	public void BackBtnClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_WhatsInTheHauntLootMatrix, true, null);
		UIManager.SetGameObjectActive(this.m_HowToGetStuff, false, null);
	}

	public void SetVisible(bool visible)
	{
		if (visible)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIHalloweenIntroduction.SetVisible(bool)).MethodHandle;
			}
			this.m_WhatsInTheHauntAC.enabled = true;
			UIManager.SetGameObjectActive(this.m_WhatsInTheHauntLootMatrix, true, null);
			UIManager.SetGameObjectActive(this.m_HowToGetStuff, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_WhatsInTheHauntLootMatrix, false, null);
			UIManager.SetGameObjectActive(this.m_HowToGetStuff, false, null);
		}
	}
}
