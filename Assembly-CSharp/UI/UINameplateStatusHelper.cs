using UnityEngine;

public class UINameplateStatusHelper : MonoBehaviour
{
	public void AnimDone()
	{
		UINameplateStatus componentInParent = GetComponentInParent<UINameplateStatus>();
		if (componentInParent != null)
		{
			componentInParent.AnimDone();
		}
	}
}
