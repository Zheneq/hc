using System;
using UnityEngine;

public class UINameplateAnimCallback : MonoBehaviour
{
	public UINameplateItem m_nameplateReference;

	public void Callback(UINameplateAnimCallback.AnimationCallback type)
	{
		if (this.m_nameplateReference != null)
		{
			this.m_nameplateReference.HandleAnimCallback(base.GetComponent<Animator>(), type);
		}
	}

	public enum AnimationCallback
	{
		BUFFGAINED,
		COMBAT_TEXT_COLOR
	}
}
