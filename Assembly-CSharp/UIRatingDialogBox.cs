using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRatingDialogBox : UIDialogBox
{
	public Text m_Title;

	public Text m_Info;

	public Button m_cancelButton;

	public Button m_submitButton;

	public Button m_reportBug;

	public UIRatingButton[] m_ratingButtons;

	public UIDialogBox.DialogButtonCallback submitCallback;

	public UIDialogBox.DialogButtonCallback cancelCallback;

	private int selectedButton;

	public override void ClearCallback()
	{
		this.submitCallback = null;
		this.cancelCallback = null;
	}

	protected override void CloseCallback()
	{
		if (this.cancelCallback != null)
		{
			this.cancelCallback(this);
		}
	}

	public int GetRating()
	{
		return this.selectedButton;
	}

	public void RatingButtonClicked(BaseEventData data)
	{
		for (int i = 0; i < this.m_ratingButtons.Length; i++)
		{
			if (this.m_ratingButtons[i].m_hitBox.gameObject == data.selectedObject)
			{
				this.m_ratingButtons[i].SetSelected(true);
				this.selectedButton = i;
			}
			else
			{
				this.m_ratingButtons[i].SetSelected(false);
			}
		}
		this.m_submitButton.interactable = true;
		UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
	}

	public void CancelClicked(BaseEventData data)
	{
		this.CloseCallback();
		UIDialogPopupManager.Get().CloseDialog(this);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsCancel);
	}

	public void ReportBugClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		UISystemEscapeMenu.Get().OnReportBugClick(data);
	}

	public void SubmitClicked(BaseEventData data)
	{
		if (this.m_submitButton.interactable)
		{
			if (this.submitCallback != null)
			{
				this.submitCallback(this);
			}
			UIDialogPopupManager.Get().CloseDialog(this);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		}
	}

	public void Start()
	{
		UIEventTriggerUtils.AddListener(this.m_cancelButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.CancelClicked));
		UIEventTriggerUtils.AddListener(this.m_submitButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.SubmitClicked));
		UIEventTriggerUtils.AddListener(this.m_reportBug.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.ReportBugClicked));
		for (int i = 0; i < this.m_ratingButtons.Length; i++)
		{
			this.m_ratingButtons[i].m_hitBox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.RatingButtonClicked);
		}
		this.selectedButton = -1;
	}

	public string GetDescriptionText(int index)
	{
		string result = string.Empty;
		switch (index)
		{
		case 0:
			result = StringUtil.TR("DislikedALot", "Rating");
			break;
		case 1:
			result = StringUtil.TR("DislikedALittle", "Rating");
			break;
		case 2:
			result = StringUtil.TR("EnjoyedALittle", "Rating");
			break;
		case 3:
			result = StringUtil.TR("EnjoyedALot", "Rating");
			break;
		}
		return result;
	}

	public void Setup(string Title, string Description, UIDialogBox.DialogButtonCallback callback = null, UIDialogBox.DialogButtonCallback cancel = null)
	{
		this.m_Title.text = Title;
		this.m_Info.text = Description;
		this.submitCallback = callback;
		this.cancelCallback = cancel;
		for (int i = 0; i < this.m_ratingButtons.Length; i++)
		{
			this.m_ratingButtons[i].m_numberLabel.text = (i + 1).ToString();
			this.m_ratingButtons[i].m_ratingDescription.text = this.GetDescriptionText(i);
		}
		this.m_submitButton.interactable = false;
	}
}
