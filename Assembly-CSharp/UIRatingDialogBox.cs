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

	public DialogButtonCallback submitCallback;

	public DialogButtonCallback cancelCallback;

	private int selectedButton;

	public override void ClearCallback()
	{
		submitCallback = null;
		cancelCallback = null;
	}

	protected override void CloseCallback()
	{
		if (cancelCallback != null)
		{
			cancelCallback(this);
		}
	}

	public int GetRating()
	{
		return selectedButton;
	}

	public void RatingButtonClicked(BaseEventData data)
	{
		for (int i = 0; i < m_ratingButtons.Length; i++)
		{
			if (m_ratingButtons[i].m_hitBox.gameObject == data.selectedObject)
			{
				m_ratingButtons[i].SetSelected(true);
				selectedButton = i;
			}
			else
			{
				m_ratingButtons[i].SetSelected(false);
			}
		}
		while (true)
		{
			m_submitButton.interactable = true;
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
			return;
		}
	}

	public void CancelClicked(BaseEventData data)
	{
		CloseCallback();
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
		if (!m_submitButton.interactable)
		{
			return;
		}
		while (true)
		{
			if (submitCallback != null)
			{
				submitCallback(this);
			}
			UIDialogPopupManager.Get().CloseDialog(this);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
			return;
		}
	}

	public void Start()
	{
		UIEventTriggerUtils.AddListener(m_cancelButton.gameObject, EventTriggerType.PointerClick, CancelClicked);
		UIEventTriggerUtils.AddListener(m_submitButton.gameObject, EventTriggerType.PointerClick, SubmitClicked);
		UIEventTriggerUtils.AddListener(m_reportBug.gameObject, EventTriggerType.PointerClick, ReportBugClicked);
		for (int i = 0; i < m_ratingButtons.Length; i++)
		{
			m_ratingButtons[i].m_hitBox.callback = RatingButtonClicked;
		}
		while (true)
		{
			selectedButton = -1;
			return;
		}
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

	public void Setup(string Title, string Description, DialogButtonCallback callback = null, DialogButtonCallback cancel = null)
	{
		m_Title.text = Title;
		m_Info.text = Description;
		submitCallback = callback;
		cancelCallback = cancel;
		for (int i = 0; i < m_ratingButtons.Length; i++)
		{
			m_ratingButtons[i].m_numberLabel.text = (i + 1).ToString();
			m_ratingButtons[i].m_ratingDescription.text = GetDescriptionText(i);
		}
		while (true)
		{
			m_submitButton.interactable = false;
			return;
		}
	}
}
