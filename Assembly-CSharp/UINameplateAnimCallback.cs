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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_nameplateReference.HandleAnimCallback(GetComponent<Animator>(), type);
			return;
		}
	}
}
