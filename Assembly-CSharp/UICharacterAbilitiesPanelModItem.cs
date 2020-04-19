using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICharacterAbilitiesPanelModItem : MonoBehaviour
{
	public TextMeshProUGUI[] m_textLabels;

	public _SelectableBtn m_btn;

	public _SelectableBtn m_saveBtn;

	private UICharacterAbiltiesPanelModLoadout m_panel;

	public CharacterLoadout LoadoutRef { get; private set; }

	private void Start()
	{
		if (this.m_saveBtn != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbilitiesPanelModItem.Start()).MethodHandle;
			}
			this.m_btn.spriteController.AddSubButton(this.m_saveBtn.spriteController);
			this.m_saveBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SaveLoadoutClicked);
		}
		for (int i = 0; i < this.m_textLabels.Length; i++)
		{
			this.m_textLabels[i].raycastTarget = false;
		}
		this.m_btn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LoadoutClicked);
		this.m_panel = base.GetComponentInParent<UICharacterAbiltiesPanelModLoadout>();
	}

	public void LoadoutClicked(BaseEventData data)
	{
		if (this.m_panel != null)
		{
			this.m_panel.NotifyModLoadoutClicked(this);
		}
	}

	public void SaveLoadoutClicked(BaseEventData data)
	{
		if (this.m_panel != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbilitiesPanelModItem.SaveLoadoutClicked(BaseEventData)).MethodHandle;
			}
			this.m_panel.NotifySaveModLoadoutClicked(this);
		}
	}

	public void Setup(CharacterLoadout loadout)
	{
		this.LoadoutRef = loadout;
		this.SetModLabels(StringUtil.TR_GetLoadoutName(this.LoadoutRef.LoadoutName));
	}

	private void SetModLabels(string text)
	{
		for (int i = 0; i < this.m_textLabels.Length; i++)
		{
			this.m_textLabels[i].text = text;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbilitiesPanelModItem.SetModLabels(string)).MethodHandle;
		}
	}
}
