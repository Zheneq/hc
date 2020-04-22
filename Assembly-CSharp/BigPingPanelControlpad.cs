using UnityEngine;

public class BigPingPanelControlpad : MonoBehaviour
{
	public _SelectableBtn m_redEnemyBtn;

	public _SelectableBtn m_blueMoveBtn;

	public _SelectableBtn m_yellowDefendBtn;

	public _SelectableBtn m_greenAssistBtn;

	public CanvasGroup m_closeButtonHover;

	public GameObject m_closeButton;

	public GameObject m_line;

	private ActorController.PingType m_hoverPingType;

	public void Init()
	{
		m_redEnemyBtn.SetSelected(false, false, string.Empty, string.Empty);
		m_blueMoveBtn.SetSelected(false, false, string.Empty, string.Empty);
		m_yellowDefendBtn.SetSelected(false, false, string.Empty, string.Empty);
		m_greenAssistBtn.SetSelected(false, false, string.Empty, string.Empty);
		m_hoverPingType = ActorController.PingType.Default;
	}

	public ActorController.PingType GetPingType()
	{
		return m_hoverPingType;
	}

	private void SetSelectedButton(ActorController.PingType hoverPingType, bool clear = false)
	{
		_SelectableBtn selectableBtn = null;
		switch (hoverPingType)
		{
		case ActorController.PingType.Enemy:
			selectableBtn = m_redEnemyBtn;
			break;
		case ActorController.PingType.Move:
			selectableBtn = m_blueMoveBtn;
			break;
		case ActorController.PingType.Defend:
			selectableBtn = m_yellowDefendBtn;
			break;
		case ActorController.PingType.Assist:
			selectableBtn = m_greenAssistBtn;
			break;
		}
		if (!(selectableBtn != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!selectableBtn.IsDisabled)
			{
				selectableBtn.SetSelected(!clear, false, string.Empty, string.Empty);
			}
			return;
		}
	}

	public void SelectAbilityButtonFromAngle(float angle, float lineSize)
	{
		if (lineSize == 0f)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_hoverPingType != 0)
			{
				SetSelectedButton(m_hoverPingType, true);
				m_hoverPingType = ActorController.PingType.Default;
				return;
			}
		}
		if (!(lineSize > 0f))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			ActorController.PingType pingType = ActorController.PingType.Default;
			angle += 45f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			switch ((int)(angle / 360f * 4f))
			{
			case 0:
				pingType = ActorController.PingType.Move;
				break;
			case 1:
				pingType = ActorController.PingType.Enemy;
				break;
			case 2:
				pingType = ActorController.PingType.Defend;
				break;
			case 3:
				pingType = ActorController.PingType.Assist;
				break;
			}
			if (pingType == m_hoverPingType)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (m_hoverPingType != 0)
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
					SetSelectedButton(m_hoverPingType, true);
				}
				m_hoverPingType = pingType;
				SetSelectedButton(m_hoverPingType);
				return;
			}
		}
	}

	public ActorController.PingType GetPingTypeHover()
	{
		return m_hoverPingType;
	}
}
