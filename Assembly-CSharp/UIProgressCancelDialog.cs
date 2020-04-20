using System;
using TMPro;
using UnityEngine.EventSystems;

public class UIProgressCancelDialog : UIDialogBox
{
	public TextMeshProUGUI m_progressCancelTitle;

	public TextMeshProUGUI m_progressCancelInfo;

	public _SelectableBtn m_progressCancelButton;

	public TextMeshProUGUI[] m_progressCancelLabel;

	public ImageFilledSloped m_progressBar;

	private UIDialogBox.DialogButtonCallback progressBarCallback;

	public override void ClearCallback()
	{
		this.progressBarCallback = null;
	}

	protected override void CloseCallback()
	{
		if (this.progressBarCallback != null)
		{
			this.progressBarCallback(this);
		}
	}

	public void Start()
	{
		if (this.m_progressCancelButton != null)
		{
			this.m_progressCancelButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ProgressCancelClicked);
		}
	}

	public float GetValue()
	{
		return this.m_progressBar.fillAmount;
	}

	public void SetValue(float val)
	{
		this.m_progressBar.fillAmount = val;
	}

	public void ProgressCancelClicked(BaseEventData data)
	{
		this.CloseCallback();
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	private void SetButtonLabels(string text)
	{
		for (int i = 0; i < this.m_progressCancelLabel.Length; i++)
		{
			this.m_progressCancelLabel[i].text = text;
		}
	}

	public void Setup(string Title, string Description, string CancelButtonLabelText, float initialVal, UIDialogBox.DialogButtonCallback callback = null)
	{
		this.m_progressCancelTitle.text = Title;
		this.m_progressCancelInfo.text = Description;
		this.progressBarCallback = callback;
		this.m_progressBar.fillAmount = initialVal;
		this.SetButtonLabels(CancelButtonLabelText);
	}
}
