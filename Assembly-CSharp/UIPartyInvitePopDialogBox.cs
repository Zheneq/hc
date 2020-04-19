using System;
using UnityEngine.EventSystems;

public class UIPartyInvitePopDialogBox : UITwoButtonDialog
{
	public _SelectableBtn m_blockButton;

	private UIDialogBox.DialogButtonCallback m_blockCallback;

	public override void Start()
	{
		base.Start();
		if (this.m_blockButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPartyInvitePopDialogBox.Start()).MethodHandle;
			}
			this.m_blockButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BlockButtonClicked);
		}
	}

	public void BlockButtonClicked(BaseEventData data)
	{
		if (this.m_blockCallback != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPartyInvitePopDialogBox.BlockButtonClicked(BaseEventData)).MethodHandle;
			}
			this.m_blockCallback(this);
		}
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	public void Setup(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, UIDialogBox.DialogButtonCallback blockCallback, UIDialogBox.DialogButtonCallback firstBtnCallback = null, UIDialogBox.DialogButtonCallback secondBtnCallback = null)
	{
		base.Setup(Title, Description, LeftButtonLabel, RightButtonLabel, firstBtnCallback, secondBtnCallback, false, true);
		this.m_blockCallback = blockCallback;
		if (blockCallback == null)
		{
			UIManager.SetGameObjectActive(this.m_blockButton, false, null);
		}
	}
}
