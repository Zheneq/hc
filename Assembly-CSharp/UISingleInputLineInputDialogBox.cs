using TMPro;
using UnityEngine.EventSystems;

public class UISingleInputLineInputDialogBox : UIDialogBox
{
	public TextMeshProUGUI m_Title;

	public TextMeshProUGUI m_Info;

	public _SelectableBtn m_firstButton;

	public _SelectableBtn m_secondButton;

	public TextMeshProUGUI[] m_firstButtonLabel;

	public TextMeshProUGUI[] m_secondButtonLabel;

	public TMP_InputField m_descriptionBoxInputField;

	private DialogButtonCallback firstButtonCallback;

	private DialogButtonCallback secondButtonCallback;

	public override void ClearCallback()
	{
		firstButtonCallback = null;
		secondButtonCallback = null;
	}

	protected override void CloseCallback()
	{
	}

	public void FirstButtonClicked(BaseEventData data)
	{
		if (firstButtonCallback != null)
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
			firstButtonCallback(this);
		}
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	public void SecondButtonClicked(BaseEventData data)
	{
		if (secondButtonCallback != null)
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
			secondButtonCallback(this);
		}
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	public void Start()
	{
		if (m_secondButton != null)
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
			m_secondButton.spriteController.callback = SecondButtonClicked;
		}
		if (m_firstButton != null)
		{
			m_firstButton.spriteController.callback = FirstButtonClicked;
		}
		m_descriptionBoxInputField.Select();
	}

	private void SetFirstButtonLabels(string text)
	{
		for (int i = 0; i < m_firstButtonLabel.Length; i++)
		{
			m_firstButtonLabel[i].text = text;
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
			return;
		}
	}

	private void SetSecondButtonLabels(string text)
	{
		for (int i = 0; i < m_secondButtonLabel.Length; i++)
		{
			m_secondButtonLabel[i].text = text;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void Setup(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, DialogButtonCallback sendCallback = null, DialogButtonCallback cancelCallback = null)
	{
		m_Title.text = Title;
		m_Info.text = Description;
		firstButtonCallback = sendCallback;
		secondButtonCallback = cancelCallback;
		SetFirstButtonLabels(LeftButtonLabel);
		SetSecondButtonLabels(RightButtonLabel);
	}
}
