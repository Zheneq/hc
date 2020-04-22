using UnityEngine;

public class UIStoreBasePanel : MonoBehaviour
{
	public Animator m_animatorController;

	public UIStorePanel.StorePanelScreen ScreenType
	{
		get;
		set;
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					UIManager.SetGameObjectActive(base.gameObject, true);
					return;
				}
			}
		}
		if (m_animatorController.gameObject.activeInHierarchy)
		{
			m_animatorController.Play("StorePanelDefaultOUT");
			OnHidden();
		}
	}

	protected virtual void OnHidden()
	{
	}
}
