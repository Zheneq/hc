using UnityEngine;

public class UINameplateAnimCallback : MonoBehaviour
{
	public enum AnimationCallback
	{
		BUFFGAINED,
		COMBAT_TEXT_COLOR
	}

	public UINameplateItem m_nameplateReference;

	public void Callback(AnimationCallback type)
	{
		if (!(m_nameplateReference != null))
		{
			return;
		}
		while (true)
		{
			m_nameplateReference.HandleAnimCallback(GetComponent<Animator>(), type);
			return;
		}
	}
}
