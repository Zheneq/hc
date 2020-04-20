using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITeamSizeButton : MonoBehaviour
{
	public Button m_btnHitBox;

	public Image m_background;

	public Image m_hover;

	public TextMeshProUGUI m_textLabel;

	public UITeamSizeButton.NotifyClickDelegate m_callback;

	private bool m_isChecked;

	private int m_team;

	private int m_index;

	public bool Clickable = true;

	public bool IsChecked()
	{
		return this.m_isChecked;
	}

	private void Start()
	{
		UIEventTriggerUtils.AddListener(this.m_btnHitBox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.BtnClicked));
		UIEventTriggerUtils.AddListener(this.m_btnHitBox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.BtnEnter));
		UIEventTriggerUtils.AddListener(this.m_btnHitBox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.BtnExit));
		this.SetChecked(this.IsChecked());
	}

	public int GetTeam()
	{
		return this.m_team;
	}

	public void SetTeam(int teamNum)
	{
		this.m_team = teamNum;
	}

	public int GetIndex()
	{
		return this.m_index;
	}

	public void SetIndex(int indexNum)
	{
		this.m_index = indexNum;
	}

	public void BtnEnter(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_hover, true, null);
		this.m_textLabel.color = Color.white;
	}

	public void BtnExit(BaseEventData data)
	{
		if (!this.IsChecked())
		{
			UIManager.SetGameObjectActive(this.m_hover, false, null);
			this.m_textLabel.color = Color.white;
		}
	}

	public void BtnClicked(BaseEventData data)
	{
		if (this.m_callback != null)
		{
			if (this.Clickable)
			{
				this.m_callback(this);
			}
		}
	}

	public void SetChecked(bool isChecked)
	{
		this.m_isChecked = isChecked;
		UIManager.SetGameObjectActive(this.m_background, this.m_isChecked, null);
		UIManager.SetGameObjectActive(this.m_hover, this.m_isChecked, null);
		if (this.m_isChecked)
		{
			this.m_textLabel.color = Color.white;
		}
		else
		{
			this.m_textLabel.color = Color.white;
		}
	}

	public delegate void NotifyClickDelegate(UITeamSizeButton m_ref);
}
