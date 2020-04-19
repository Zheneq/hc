using System;
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
		if (this.m_Button != null)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITrustWarEndDialog.Start()).MethodHandle;
			}
			this.m_Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ButtonClicked);
		}
	}

	public void ButtonClicked(BaseEventData data)
	{
		this.Close();
	}

	public void Setup()
	{
	}
}
