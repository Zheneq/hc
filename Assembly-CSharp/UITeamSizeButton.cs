using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITeamSizeButton : MonoBehaviour
{
	public delegate void NotifyClickDelegate(UITeamSizeButton m_ref);

	public Button m_btnHitBox;

	public Image m_background;

	public Image m_hover;

	public TextMeshProUGUI m_textLabel;

	public NotifyClickDelegate m_callback;

	private bool m_isChecked;

	private int m_team;

	private int m_index;

	public bool Clickable = true;

	public bool IsChecked()
	{
		return m_isChecked;
	}

	private void Start()
	{
		UIEventTriggerUtils.AddListener(m_btnHitBox.gameObject, EventTriggerType.PointerClick, BtnClicked);
		UIEventTriggerUtils.AddListener(m_btnHitBox.gameObject, EventTriggerType.PointerEnter, BtnEnter);
		UIEventTriggerUtils.AddListener(m_btnHitBox.gameObject, EventTriggerType.PointerExit, BtnExit);
		SetChecked(IsChecked());
	}

	public int GetTeam()
	{
		return m_team;
	}

	public void SetTeam(int teamNum)
	{
		m_team = teamNum;
	}

	public int GetIndex()
	{
		return m_index;
	}

	public void SetIndex(int indexNum)
	{
		m_index = indexNum;
	}

	public void BtnEnter(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_hover, true);
		m_textLabel.color = Color.white;
	}

	public void BtnExit(BaseEventData data)
	{
		if (IsChecked())
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_hover, false);
			m_textLabel.color = Color.white;
			return;
		}
	}

	public void BtnClicked(BaseEventData data)
	{
		if (m_callback == null)
		{
			return;
		}
		while (true)
		{
			if (Clickable)
			{
				m_callback(this);
			}
			return;
		}
	}

	public void SetChecked(bool isChecked)
	{
		m_isChecked = isChecked;
		UIManager.SetGameObjectActive(m_background, m_isChecked);
		UIManager.SetGameObjectActive(m_hover, m_isChecked);
		if (m_isChecked)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_textLabel.color = Color.white;
					return;
				}
			}
		}
		m_textLabel.color = Color.white;
	}
}
