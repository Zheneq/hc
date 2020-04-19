using System;
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
		this.m_redEnemyBtn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_blueMoveBtn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_yellowDefendBtn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_greenAssistBtn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_hoverPingType = ActorController.PingType.Default;
	}

	public ActorController.PingType GetPingType()
	{
		return this.m_hoverPingType;
	}

	private void SetSelectedButton(ActorController.PingType hoverPingType, bool clear = false)
	{
		_SelectableBtn selectableBtn = null;
		switch (hoverPingType)
		{
		case ActorController.PingType.Assist:
			selectableBtn = this.m_greenAssistBtn;
			break;
		case ActorController.PingType.Defend:
			selectableBtn = this.m_yellowDefendBtn;
			break;
		case ActorController.PingType.Enemy:
			selectableBtn = this.m_redEnemyBtn;
			break;
		case ActorController.PingType.Move:
			selectableBtn = this.m_blueMoveBtn;
			break;
		}
		if (selectableBtn != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BigPingPanelControlpad.SetSelectedButton(ActorController.PingType, bool)).MethodHandle;
			}
			if (!selectableBtn.IsDisabled)
			{
				selectableBtn.SetSelected(!clear, false, string.Empty, string.Empty);
			}
		}
	}

	public void SelectAbilityButtonFromAngle(float angle, float lineSize)
	{
		if (lineSize == 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BigPingPanelControlpad.SelectAbilityButtonFromAngle(float, float)).MethodHandle;
			}
			if (this.m_hoverPingType != ActorController.PingType.Default)
			{
				this.SetSelectedButton(this.m_hoverPingType, true);
				this.m_hoverPingType = ActorController.PingType.Default;
				return;
			}
		}
		if (lineSize > 0f)
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
			if (pingType != this.m_hoverPingType)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_hoverPingType != ActorController.PingType.Default)
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
					this.SetSelectedButton(this.m_hoverPingType, true);
				}
				this.m_hoverPingType = pingType;
				this.SetSelectedButton(this.m_hoverPingType, false);
			}
		}
	}

	public ActorController.PingType GetPingTypeHover()
	{
		return this.m_hoverPingType;
	}
}
