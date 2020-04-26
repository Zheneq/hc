using UnityEngine;

public abstract class UIDialogBox : MonoBehaviour
{
	public delegate void DialogButtonCallback(UIDialogBox boxReference);

	protected DialogBoxType m_BoxType;

	public virtual void Awake()
	{
		_ButtonSwapSprite[] componentsInChildren = base.gameObject.GetComponentsInChildren<_ButtonSwapSprite>(true);
		if (componentsInChildren == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].m_ignoreDialogboxes = true;
			}
			return;
		}
	}

	public DialogBoxType GetBoxType()
	{
		return m_BoxType;
	}

	public abstract void ClearCallback();

	protected abstract void CloseCallback();

	public void DoCloseCallback()
	{
		CloseCallback();
	}

	public virtual void Close()
	{
		UIDialogPopupManager.Get().CloseDialog(this);
	}
}
