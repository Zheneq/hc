using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationBar : UIScene
{
	public class NavigationBarSceneStateParameters : SceneStateParameters
	{
	}

	public TextMeshProUGUI m_searchQueueText;

	public _SelectableBtn m_cancelBtn;

	public Button m_cancelHitbox;

	public Animator m_cancelBtnAnimator;

	public _SelectableBtn m_gameSettingsBtn;

	public TextMeshProUGUI[] m_timeInQueueLabel;

	private static NavigationBar s_instance;

	private string m_queueStatusDisplayString = string.Empty;

	private static NavigationBarSceneStateParameters m_currentState = new NavigationBarSceneStateParameters();

	public static NavigationBar Get()
	{
		return s_instance;
	}

	public override SceneStateParameters GetCurrentState()
	{
		return m_currentState;
	}

	public static NavigationBarSceneStateParameters GetCurrentSpecificState()
	{
		return m_currentState;
	}

	private void OnShowGameSettingsClicked(BaseEventData data)
	{
		UICharacterSelectScreen.Get().OnShowGameSettingsClicked(data);
	}

	private void CancelButtonClickCallback(BaseEventData data)
	{
		UICharacterSelectScreenController.Get().CancelButtonClickCallback(data);
	}

	public override void Awake()
	{
		s_instance = this;
		m_gameSettingsBtn.spriteController.callback = OnShowGameSettingsClicked;
		UIManager.SetGameObjectActive(m_cancelBtn, false);
		m_cancelBtn.spriteController.callback = CancelButtonClickCallback;
		m_searchQueueText.raycastTarget = false;
		UIManager.SetGameObjectActive(m_gameSettingsBtn, false);
		m_cancelHitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.SearchQueue, ShowTooltip);
		base.Awake();
	}

	public void UpdateTimeInQueueLabel(string newText)
	{
		if (m_timeInQueueLabel == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_timeInQueueLabel.Length; i++)
			{
				m_timeInQueueLabel[i].text = newText;
			}
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private bool ShowTooltip(UITooltipBase tooltip)
	{
		LobbyMatchmakingQueueInfo queueInfo = GameManager.Get().QueueInfo;
		if (queueInfo != null)
		{
			UISearchQueueTooltip uISearchQueueTooltip = (UISearchQueueTooltip)tooltip;
			uISearchQueueTooltip.Setup();
			return true;
		}
		return false;
	}

	public void UpdateSearchQueueTooltipLabels()
	{
		if (!m_cancelBtn.gameObject.activeSelf)
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
			m_cancelHitbox.GetComponent<UITooltipHoverObject>().Refresh();
			return;
		}
	}

	public void SearchQueueTextExit()
	{
		UITooltipManager.Get().HideDisplayTooltip(TooltipType.SearchQueue);
	}

	public void NotifyStatusQueueAnimDone()
	{
		m_searchQueueText.text = m_queueStatusDisplayString;
	}

	public void UpdateStatusMessage()
	{
		bool isWaitingForGroup = SceneStateParameters.IsWaitingForGroup;
		bool isInCustomGame = SceneStateParameters.IsInCustomGame;
		string newText = string.Empty;
		if (!isWaitingForGroup)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!isInCustomGame)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				newText = string.Format(arg0: (int)SceneStateParameters.TimeInQueue.TotalSeconds, format: StringUtil.TR("SecondsTimerShort", "Global"));
			}
		}
		UpdateTimeInQueueLabel(newText);
		if (!(m_searchQueueText != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			m_queueStatusDisplayString = ClientGameManager.Get().GenerateQueueLabel();
			if (!m_searchQueueText.text.IsNullOrEmpty())
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
				if (!m_queueStatusDisplayString.IsNullOrEmpty())
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							bool flag = m_queueStatusDisplayString != m_searchQueueText.text;
							string value = StringUtil.TR("Searching", "Frontend");
							if (m_queueStatusDisplayString.Contains(value) && m_searchQueueText.text.Contains(value))
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								flag = false;
							}
							if (flag)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										try
										{
											AnimatorStateInfo currentAnimatorStateInfo = m_cancelBtnAnimator.GetCurrentAnimatorStateInfo(0);
											AnimatorClipInfo animatorClipInfo = m_cancelBtnAnimator.GetCurrentAnimatorClipInfo(0)[0];
											if (animatorClipInfo.clip.name != "CancelBtnStatusChange")
											{
												goto IL_01c1;
											}
											if (animatorClipInfo.clip.name == "CancelBtnStatusChange" && currentAnimatorStateInfo.normalizedTime >= currentAnimatorStateInfo.length)
											{
												while (true)
												{
													switch (2)
													{
													case 0:
														continue;
													}
													break;
												}
												goto IL_01c1;
											}
											goto end_IL_0146;
											IL_01c1:
											m_cancelBtnAnimator.Play("CancelBtnStatusChange", 0, 0f);
											end_IL_0146:;
										}
										catch
										{
											m_searchQueueText.text = m_queueStatusDisplayString;
										}
										return;
									}
								}
							}
							m_searchQueueText.text = m_queueStatusDisplayString;
							return;
						}
						}
					}
				}
			}
			if (!m_searchQueueText.text.IsNullOrEmpty())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!m_queueStatusDisplayString.IsNullOrEmpty())
				{
					return;
				}
			}
			m_searchQueueText.text = m_queueStatusDisplayString;
			return;
		}
	}

	public void Update()
	{
		if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
		{
			while (true)
			{
				switch (4)
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
			if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(AppState_LandingPage.Get() == AppState.GetCurrent()))
				{
					return;
				}
			}
		}
		if (UIGameSettingsPanel.Get().m_lastVisible)
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
			UpdateStatusMessage();
			UpdateSearchQueueTooltipLabels();
			return;
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.FrontEndNavPanel;
	}
}
