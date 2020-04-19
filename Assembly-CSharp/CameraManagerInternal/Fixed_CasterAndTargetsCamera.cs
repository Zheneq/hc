using System;
using UnityEngine;

namespace CameraManagerInternal
{
	public class Fixed_CasterAndTargetsCamera : MonoBehaviour
	{
		private GameObject m_animatorObject;

		public void SetAnimator(GameObject animatorObject)
		{
			this.m_animatorObject = animatorObject;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Fixed_CasterAndTargetsCamera.SetAnimator(GameObject)).MethodHandle;
				}
				base.transform.position = this.m_animatorObject.transform.position;
				base.transform.rotation = this.m_animatorObject.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
			}
		}
	}
}
