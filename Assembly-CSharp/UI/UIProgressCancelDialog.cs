using TMPro;
using UnityEngine.EventSystems;

public class UIProgressCancelDialog : UIDialogBox
{
	public TextMeshProUGUI m_progressCancelTitle;

	public TextMeshProUGUI m_progressCancelInfo;

	public _SelectableBtn m_progressCancelButton;

	public TextMeshProUGUI[] m_progressCancelLabel;

	public ImageFilledSloped m_progressBar;

	private DialogButtonCallback progressBarCallback;

	public override void ClearCallback()
	{
		progressBarCallback = null;
	}

	protected override void CloseCallback()
	{
		if (progressBarCallback == null)
		{
			return;
		}
		while (true)
		{
			progressBarCallback(this);
			return;
		}
	}

	public void Start()
	{
		if (m_progressCancelButton != null)
		{
			m_progressCancelButton.spriteController.callback = ProgressCancelClicked;
		}
	}

	public float GetValue()
	{
		return m_progressBar.fillAmount;
	}

	public void SetValue(float val)
	{
		m_progressBar.fillAmount = val;
	}

	public void ProgressCancelClicked(BaseEventData data)
	{
		CloseCallback();
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	private void SetButtonLabels(string text)
	{
		for (int i = 0; i < m_progressCancelLabel.Length; i++)
		{
			m_progressCancelLabel[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	public void Setup(string Title, string Description, string CancelButtonLabelText, float initialVal, DialogButtonCallback callback = null)
	{
		m_progressCancelTitle.text = Title;
		m_progressCancelInfo.text = Description;
		progressBarCallback = callback;
		m_progressBar.fillAmount = initialVal;
		SetButtonLabels(CancelButtonLabelText);
	}
}
