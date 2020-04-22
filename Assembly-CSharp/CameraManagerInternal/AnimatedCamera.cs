using UnityEngine;

namespace CameraManagerInternal
{
	public class AnimatedCamera : MonoBehaviour
	{
		private GameObject m_animatorObject;

		public void SetAnimator(GameObject animatorObject)
		{
			m_animatorObject = animatorObject;
		}

		private void LateUpdate()
		{
			if (!(m_animatorObject != null))
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				base.transform.position = m_animatorObject.transform.position;
				base.transform.rotation = m_animatorObject.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
				return;
			}
		}
	}
}
