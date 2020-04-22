using TMPro;
using UnityEngine.EventSystems;

public class UIOneButtonDialog : UIDialogBox
{
	public TextMeshProUGUI m_Title;

	public TextMeshProUGUI m_Desc;

	public _SelectableBtn m_Button;

	public TextMeshProUGUI[] m_ButtonLabel;

	private DialogButtonCallback m_btnCallback;

	public DialogButtonCallback GetCallbackReference()
	{
		return m_btnCallback;
	}

	public override void ClearCallback()
	{
		m_btnCallback = null;
	}

	protected override void CloseCallback()
	{
		if (m_btnCallback == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_btnCallback(this);
			return;
		}
	}

	public void Start()
	{
		if (m_Button != null)
		{
			m_Button.spriteController.callback = ButtonClicked;
			m_Button.spriteController.RegisterControlPadInput(ControlpadInputValue.Button_A);
		}
	}

	public void ButtonClicked(BaseEventData data)
	{
		CloseCallback();
		Close();
	}

	private void SetButtonLabel(string text)
	{
		for (int i = 0; i < m_ButtonLabel.Length; i++)
		{
			m_ButtonLabel[i].text = text;
		}
		while (true)
		{
			switch (1)
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

	public void Setup(string Title, string Description, string ButtonLabel, DialogButtonCallback callback = null)
	{
		m_Title.text = Title;
		m_Desc.text = Description;
		SetButtonLabel(ButtonLabel);
		m_btnCallback = callback;
	}
}
