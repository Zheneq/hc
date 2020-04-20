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
				base.transform.position = this.m_animatorObject.transform.position;
				base.transform.rotation = this.m_animatorObject.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
			}
		}
	}
}
