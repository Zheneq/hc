using System;
using TMPro;
using UnityEngine.EventSystems;

public class UIReportBugDialogBox : UIDialogBox
{
	public TextMeshProUGUI m_Title;

	public TextMeshProUGUI m_Info;

	public _SelectableBtn m_firstButton;

	public _SelectableBtn m_secondButton;

	public TextMeshProUGUI[] m_firstButtonLabel;

	public TextMeshProUGUI[] m_secondButtonLabel;

	public TMP_InputField m_descriptionBoxInputField;

	private UIDialogBox.DialogButtonCallback firstButtonCallback;

	private UIDialogBox.DialogButtonCallback secondButtonCallback;

	public override void ClearCallback()
	{
		this.firstButtonCallback = null;
		this.secondButtonCallback = null;
	}

	protected override void CloseCallback()
	{
	}

	public void FirstButtonClicked(BaseEventData data)
	{
		if (this.firstButtonCallback != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIReportBugDialogBox.FirstButtonClicked(BaseEventData)).MethodHandle;
			}
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

	public void Start()
	{
		if (this.m_secondButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIReportBugDialogBox.Start()).MethodHandle;
			}
			this.m_secondButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SecondButtonClicked);
		}
		if (this.m_firstButton != null)
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
			this.m_firstButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.FirstButtonClicked);
		}
		this.m_descriptionBoxInputField.Select();
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
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIReportBugDialogBox.SetSecondButtonLabels(string)).MethodHandle;
		}
	}

	public void Setup(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback sendCallback = null, UIDialogBox.DialogButtonCallback cancelCallback = null)
	{
		this.m_Title.text = Title;
		this.m_Info.text = Description;
		this.firstButtonCallback = sendCallback;
		this.secondButtonCallback = cancelCallback;
		this.SetFirstButtonLabels(LeftButtonLabel);
		this.SetSecondButtonLabels(RightButtonLabel);
	}
}
