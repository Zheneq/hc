using System;
using UnityEngine;

namespace CameraManagerInternal
{
	public class AnimatedCamera : MonoBehaviour
	{
		private GameObject m_animatorObject;

		public void SetAnimator(GameObject animatorObject)
		{
			this.m_animatorObject = animatorObject;
		}

		private void LateUpdate()
		{
			if (this.m_animatorObject != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AnimatedCamera.LateUpdate()).MethodHandle;
				}
				base.transform.position = this.m_animatorObject.transform.position;
				base.transform.rotation = this.m_animatorObject.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
			}
		}
	}
}
