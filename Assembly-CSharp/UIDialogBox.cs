using System;
using UnityEngine;

public abstract class UIDialogBox : MonoBehaviour
{
	protected DialogBoxType m_BoxType;

	public virtual void Awake()
	{
		_ButtonSwapSprite[] componentsInChildren = base.gameObject.GetComponentsInChildren<_ButtonSwapSprite>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].m_ignoreDialogboxes = true;
			}
		}
	}

	public DialogBoxType GetBoxType()
	{
		return this.m_BoxType;
	}

	public abstract void ClearCallback();

	protected abstract void CloseCallback();

	public void DoCloseCallback()
	{
		this.CloseCallback();
	}

	public virtual void Close()
	{
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	public delegate void DialogButtonCallback(UIDialogBox boxReference);
}
