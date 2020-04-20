using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationBar : UIScene
{
	public TextMeshProUGUI m_searchQueueText;

	public _SelectableBtn m_cancelBtn;

	public Button m_cancelHitbox;

	public Animator m_cancelBtnAnimator;

	public _SelectableBtn m_gameSettingsBtn;

	public TextMeshProUGUI[] m_timeInQueueLabel;

	private static NavigationBar s_instance;

	private string m_queueStatusDisplayString = string.Empty;

	private static NavigationBar.NavigationBarSceneStateParameters m_currentState = new NavigationBar.NavigationBarSceneStateParameters();

	public static NavigationBar Get()
	{
		return NavigationBar.s_instance;
	}

	public override SceneStateParameters GetCurrentState()
	{
		return NavigationBar.m_currentState;
	}

	public static NavigationBar.NavigationBarSceneStateParameters GetCurrentSpecificState()
	{
		return NavigationBar.m_currentState;
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
		NavigationBar.s_instance = this;
		this.m_gameSettingsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnShowGameSettingsClicked);
		UIManager.SetGameObjectActive(this.m_cancelBtn, false, null);
		this.m_cancelBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CancelButtonClickCallback);
		this.m_searchQueueText.raycastTarget = false;
		UIManager.SetGameObjectActive(this.m_gameSettingsBtn, false, null);
		this.m_cancelHitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.SearchQueue, new TooltipPopulateCall(this.ShowTooltip), null);
		base.Awake();
	}

	public void UpdateTimeInQueueLabel(string newText)
	{
		if (this.m_timeInQueueLabel != null)
		{
			for (int i = 0; i < this.m_timeInQueueLabel.Length; i++)
			{
				this.m_timeInQueueLabel[i].text = newText;
			}
		}
	}

	private bool ShowTooltip(UITooltipBase tooltip)
	{
		LobbyMatchmakingQueueInfo queueInfo = GameManager.Get().QueueInfo;
		if (queueInfo != null)
		{
			UISearchQueueTooltip uisearchQueueTooltip = (UISearchQueueTooltip)tooltip;
			uisearchQueueTooltip.Setup();
			return true;
		}
		return false;
	}

	public void UpdateSearchQueueTooltipLabels()
	{
		if (this.m_cancelBtn.gameObject.activeSelf)
		{
			this.m_cancelHitbox.GetComponent<UITooltipHoverObject>().Refresh();
		}
	}

	public void SearchQueueTextExit()
	{
		UITooltipManager.Get().HideDisplayTooltip(TooltipType.SearchQueue);
	}

	public void NotifyStatusQueueAnimDone()
	{
		this.m_searchQueueText.text = this.m_queueStatusDisplayString;
	}

	public void UpdateStatusMessage()
	{
		bool isWaitingForGroup = SceneStateParameters.IsWaitingForGroup;
		bool isInCustomGame = SceneStateParameters.IsInCustomGame;
		string newText = string.Empty;
		if (!isWaitingForGroup)
		{
			if (!isInCustomGame)
			{
				TimeSpan timeInQueue = SceneStateParameters.TimeInQueue;
				newText = string.Format(StringUtil.TR("SecondsTimerShort", "Global"), (int)timeInQueue.TotalSeconds);
			}
		}
		this.UpdateTimeInQueueLabel(newText);
		if (this.m_searchQueueText != null)
		{
			this.m_queueStatusDisplayString = ClientGameManager.Get().GenerateQueueLabel();
			if (!this.m_searchQueueText.text.IsNullOrEmpty())
			{
				if (!this.m_queueStatusDisplayString.IsNullOrEmpty())
				{
					bool flag = this.m_queueStatusDisplayString != this.m_searchQueueText.text;
					string value = StringUtil.TR("Searching", "Frontend");
					if (this.m_queueStatusDisplayString.Contains(value) && this.m_searchQueueText.text.Contains(value))
					{
						flag = false;
					}
					if (flag)
					{
						try
						{
							AnimatorStateInfo currentAnimatorStateInfo = this.m_cancelBtnAnimator.GetCurrentAnimatorStateInfo(0);
							AnimatorClipInfo animatorClipInfo = this.m_cancelBtnAnimator.GetCurrentAnimatorClipInfo(0)[0];
							if (!(animatorClipInfo.clip.name != "CancelBtnStatusChange"))
							{
								if (!(animatorClipInfo.clip.name == "CancelBtnStatusChange") || currentAnimatorStateInfo.normalizedTime < currentAnimatorStateInfo.length)
								{
									goto IL_1D7;
								}
							}
							this.m_cancelBtnAnimator.Play("CancelBtnStatusChange", 0, 0f);
							IL_1D7:;
						}
						catch
						{
							this.m_searchQueueText.text = this.m_queueStatusDisplayString;
						}
					}
					else
					{
						this.m_searchQueueText.text = this.m_queueStatusDisplayString;
					}
					return;
				}
			}
			if (!this.m_searchQueueText.text.IsNullOrEmpty())
			{
				if (!this.m_queueStatusDisplayString.IsNullOrEmpty())
				{
					return;
				}
			}
			this.m_searchQueueText.text = this.m_queueStatusDisplayString;
		}
	}

	public void Update()
	{
		if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
		{
			if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
			{
				if (!(AppState_LandingPage.Get() == AppState.GetCurrent()))
				{
					return;
				}
			}
		}
		if (!UIGameSettingsPanel.Get().m_lastVisible)
		{
			this.UpdateStatusMessage();
			this.UpdateSearchQueueTooltipLabels();
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.FrontEndNavPanel;
	}

	public class NavigationBarSceneStateParameters : SceneStateParameters
	{
	}
}
