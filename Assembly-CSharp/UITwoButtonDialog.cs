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

	private DialogButtonCallback firstButtonCallback;

	private DialogButtonCallback secondButtonCallback;

	private bool m_callLeftOnClose;

	private bool m_callRightOnClose;

	public override void ClearCallback()
	{
		firstButtonCallback = null;
		secondButtonCallback = null;
	}

	protected override void CloseCallback()
	{
		if (m_callLeftOnClose)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (firstButtonCallback != null)
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
				firstButtonCallback(this);
			}
		}
		if (!m_callRightOnClose)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (secondButtonCallback != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					secondButtonCallback(this);
					return;
				}
			}
			return;
		}
	}

	public void FirstButtonClicked(BaseEventData data)
	{
		if (firstButtonCallback != null)
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

	public virtual void Start()
	{
		if (m_secondButton != null)
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
	}

	private void SetFirstButtonLabels(string text)
	{
		for (int i = 0; i < m_firstButtonLabel.Length; i++)
		{
			m_firstButtonLabel[i].text = text;
		}
		while (true)
		{
			switch (7)
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
			switch (7)
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

	public void Setup(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, DialogButtonCallback firstBtnCallback = null, DialogButtonCallback secondBtnCallback = null, bool CallLeftOnClose = false, bool CallRightOnClose = false)
	{
		m_twoBtnTitle.text = Title;
		m_twoBtnInfo.text = Description;
		firstButtonCallback = firstBtnCallback;
		secondButtonCallback = secondBtnCallback;
		m_callLeftOnClose = CallLeftOnClose;
		m_callRightOnClose = CallRightOnClose;
		SetFirstButtonLabels(LeftButtonLabel);
		SetSecondButtonLabels(RightButtonLabel);
	}
}
