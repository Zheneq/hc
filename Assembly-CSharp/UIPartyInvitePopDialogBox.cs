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
			m_blockButton.spriteController.callback = BlockButtonClicked;
			return;
		}
	}

	public void BlockButtonClicked(BaseEventData data)
	{
		if (m_blockCallback != null)
		{
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
