using System;
using TMPro;
using UnityEngine.EventSystems;

public class UIOneButtonDialog : UIDialogBox
{
	public TextMeshProUGUI m_Title;

	public TextMeshProUGUI m_Desc;

	public _SelectableBtn m_Button;

	public TextMeshProUGUI[] m_ButtonLabel;

	private UIDialogBox.DialogButtonCallback m_btnCallback;

	public UIDialogBox.DialogButtonCallback GetCallbackReference()
	{
		return this.m_btnCallback;
	}

	public override void ClearCallback()
	{
		this.m_btnCallback = null;
	}

	protected override void CloseCallback()
	{
		if (this.m_btnCallback != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIOneButtonDialog.CloseCallback()).MethodHandle;
			}
			this.m_btnCallback(this);
		}
	}

	public void Start()
	{
		if (this.m_Button != null)
		{
			this.m_Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ButtonClicked);
			this.m_Button.spriteController.RegisterControlPadInput(ControlpadInputValue.Button_A);
		}
	}

	public void ButtonClicked(BaseEventData data)
	{
		this.CloseCallback();
		this.Close();
	}

	private void SetButtonLabel(string text)
	{
		for (int i = 0; i < this.m_ButtonLabel.Length; i++)
		{
			this.m_ButtonLabel[i].text = text;
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIOneButtonDialog.SetButtonLabel(string)).MethodHandle;
		}
	}

	public void Setup(string Title, string Description, string ButtonLabel, UIDialogBox.DialogButtonCallback callback = null)
	{
		this.m_Title.text = Title;
		this.m_Desc.text = Description;
		this.SetButtonLabel(ButtonLabel);
		this.m_btnCallback = callback;
	}
}
