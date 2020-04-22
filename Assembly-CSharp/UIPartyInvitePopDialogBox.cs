using UnityEngine.EventSystems;

public class UIPartyInvitePopDialogBox : UITwoButtonDialog
{
	public _SelectableBtn m_blockButton;

	private DialogButtonCallback m_blockCallback;

	public override void Start()
	{
		base.Start();
		if (!(m_blockButton != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_blockButton.spriteController.callback = BlockButtonClicked;
			return;
		}
	}

	public void BlockButtonClicked(BaseEventData data)
	{
		if (m_blockCallback != null)
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
			m_blockCallback(this);
		}
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	public void Setup(string Title, string Description, string LeftButtonLabel, string RightButtonLabel, DialogButtonCallback blockCallback, DialogButtonCallback firstBtnCallback = null, DialogButtonCallback secondBtnCallback = null)
	{
		Setup(Title, Description, LeftButtonLabel, RightButtonLabel, firstBtnCallback, secondBtnCallback, false, true);
		m_blockCallback = blockCallback;
		if (blockCallback == null)
		{
			UIManager.SetGameObjectActive(m_blockButton, false);
		}
	}
}
