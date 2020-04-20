using System;
using TMPro;
using UnityEngine.EventSystems;

public class UITwoButtonDialog : UIDialogBox
{
	public TextMeshProUGUI m_twoBtnTitle;

	public TextMeshProUGUI m_twoBtnInfo;

	public _SelectableBtn m_firstButton;

	public _SelectableBtn m_secondButton;

	public TextMeshProUGUI[] m_firstButtonLabel;

	public TextMeshProUGUI[] m_secondButtonLabel;

	private UIDialogBox.DialogButtonCallback firstButtonCallback;

	private UIDialogBox.DialogButtonCallback secondButtonCallback;

	private bool m_callLeftOnClose;

	private bool m_callRightOnClose;

	public override void ClearCallback()
	{
		this.firstButtonCallback = null;
		this.secondButtonCallback = null;
	}

	protected override void CloseCallback()
	{
		if (this.m_callLeftOnClose)
		{
			if (this.firstButtonCallback != null)
			{
				this.firstButtonCallback(this);
			}
		}
		if (this.m_callRightOnClose)
		{
			if (this.secondButtonCallback != null)
			{
				this.secondButtonCallback(this);
			}
		}
	}

	public void FirstButtonClicked(BaseEventData data)
	{
		if (this.firstButtonCallback != null)
		{
			this.firstButtonCallback(this);
		}
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	public void SecondButtonClicked(BaseEventData data)
	{
		if (this.secondButtonCallback != null)
		{
			this.secondButtonCallback(this);
		}
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	public virtual void Start()
	{
		if (this.m_secondButton != null)
		{
			this.m_secondButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SecondButtonClicked);
		}
		if (this.m_firstButton != null)
		{
			this.m_firstButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.FirstButtonClicked);
		}
	}

	private void SetFirstButtonLabels(string text)
	{
		for (int i = 0; i < this.m_firstButtonLabel.Length; i++)
		{
			this.m_firstButtonLabel[i].text = text;
		}
	}

	private void SetSecondButtonLabels(string text)
	{
		for (int i = 0; i < this.m_secondButtonLabel.Length; i++)
		{
			this.m_secondButtonLabel[i].text = text;
		}
	}

	public void Setup(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback firstBtnCallback = null, UIDialogBox.DialogButtonCallback secondBtnCallback = null, bool CallLeftOnClose = false, bool CallRightOnClose = false)
	{
		this.m_twoBtnTitle.text = Title;
		this.m_twoBtnInfo.text = Description;
		this.firstButtonCallback = firstBtnCallback;
		this.secondButtonCallback = secondBtnCallback;
		this.m_callLeftOnClose = CallLeftOnClose;
		this.m_callRightOnClose = CallRightOnClose;
		this.SetFirstButtonLabels(LeftButtonLabel);
		this.SetSecondButtonLabels(RightButtonLabel);
	}
}
