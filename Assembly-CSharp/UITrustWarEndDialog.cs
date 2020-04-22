using UnityEngine.EventSystems;

public class UITrustWarEndDialog : UIDialogBox
{
	public _SelectableBtn m_Button;

	public override void ClearCallback()
	{
	}

	protected override void CloseCallback()
	{
	}

	public void Start()
	{
		if (!(m_Button != null))
		{
			return;
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
			m_Button.spriteController.callback = ButtonClicked;
			return;
		}
	}

	public void ButtonClicked(BaseEventData data)
	{
		Close();
	}

	public void Setup()
	{
	}
}
