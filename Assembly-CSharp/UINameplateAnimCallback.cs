using System;
using UnityEngine;

public class UINameplateAnimCallback : MonoBehaviour
{
	public UINameplateItem m_nameplateReference;

	public void Callback(UINameplateAnimCallback.AnimationCallback type)
	{
		if (this.m_nameplateReference != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplateAnimCallback.Callback(UINameplateAnimCallback.AnimationCallback)).MethodHandle;
			}
			this.m_nameplateReference.HandleAnimCallback(base.GetComponent<Animator>(), type);
		}
	}

	public enum AnimationCallback
	{
		BUFFGAINED,
		COMBAT_TEXT_COLOR
	}
}
