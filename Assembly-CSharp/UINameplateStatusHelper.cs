using System;
using UnityEngine;

public class UINameplateStatusHelper : MonoBehaviour
{
	public void AnimDone()
	{
		UINameplateStatus componentInParent = base.GetComponentInParent<UINameplateStatus>();
		if (componentInParent != null)
		{
			componentInParent.AnimDone();
		}
	}
}
