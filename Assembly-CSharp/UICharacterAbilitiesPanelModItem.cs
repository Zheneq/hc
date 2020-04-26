using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICharacterAbilitiesPanelModItem : MonoBehaviour
{
	public TextMeshProUGUI[] m_textLabels;

	public _SelectableBtn m_btn;

	public _SelectableBtn m_saveBtn;

	private UICharacterAbiltiesPanelModLoadout m_panel;

	public CharacterLoadout LoadoutRef
	{
		get;
		private set;
	}

	private void Start()
	{
		if (m_saveBtn != null)
		{
			m_btn.spriteController.AddSubButton(m_saveBtn.spriteController);
			m_saveBtn.spriteController.callback = SaveLoadoutClicked;
		}
		for (int i = 0; i < m_textLabels.Length; i++)
		{
			m_textLabels[i].raycastTarget = false;
		}
		m_btn.spriteController.callback = LoadoutClicked;
		m_panel = GetComponentInParent<UICharacterAbiltiesPanelModLoadout>();
	}

	public void LoadoutClicked(BaseEventData data)
	{
		if (m_panel != null)
		{
			m_panel.NotifyModLoadoutClicked(this);
		}
	}

	public void SaveLoadoutClicked(BaseEventData data)
	{
		if (!(m_panel != null))
		{
			return;
		}
		while (true)
		{
			m_panel.NotifySaveModLoadoutClicked(this);
			return;
		}
	}

	public void Setup(CharacterLoadout loadout)
	{
		LoadoutRef = loadout;
		SetModLabels(StringUtil.TR_GetLoadoutName(LoadoutRef.LoadoutName));
	}

	private void SetModLabels(string text)
	{
		for (int i = 0; i < m_textLabels.Length; i++)
		{
			m_textLabels[i].text = text;
		}
		while (true)
		{
			return;
		}
	}
}
