using System;
using UnityEngine;

[Serializable]
public class CameraFaceShot
{
	public string m_name = "Camera Face Shot Name";

	public int m_index;

	public int m_animationIndex = 0x3E8;

	public float m_fieldOfView = 45f;

	public float m_duration = 2f;

	private GameObject m_cameraAnimationObj;

	private float m_time;

	internal ActorData Actor { get; private set; }

	internal void Begin(ActorData actor, Camera faceCam)
	{
		this.m_time = 0f;
		this.Actor = actor;
		this.m_cameraAnimationObj = actor.\u000E().gameObject.FindInChildren("camera0", 0);
		if (faceCam != null)
		{
			faceCam.fieldOfView = this.m_fieldOfView;
			if (!faceCam.gameObject.activeInHierarchy)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraFaceShot.Begin(ActorData, Camera)).MethodHandle;
				}
				faceCam.gameObject.SetActive(true);
			}
		}
		ActorModelData actorModelData = actor.\u000E();
		Animator animator = (!(actorModelData == null)) ? actorModelData.GetModelAnimator() : null;
		if (animator != null)
		{
			animator.SetInteger("Attack", this.m_animationIndex);
		}
	}

	internal bool Update(Camera faceCam)
	{
		ActorModelData actorModelData = this.Actor.\u0012();
		if (actorModelData != null && !actorModelData.IsPlayingIdleAnim(false))
		{
			Animator animator;
			if (actorModelData == null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraFaceShot.Update(Camera)).MethodHandle;
				}
				animator = null;
			}
			else
			{
				animator = actorModelData.GetModelAnimator();
			}
			Animator animator2 = animator;
			if (animator2 != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				animator2.SetInteger("Attack", 0);
			}
			faceCam.transform.position = this.m_cameraAnimationObj.transform.position;
			faceCam.transform.rotation = this.m_cameraAnimationObj.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
		}
		this.m_time += Time.deltaTime;
		bool result;
		if (this.m_time >= this.m_duration)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			result = (this.m_duration <= 0f);
		}
		else
		{
			result = true;
		}
		return result;
	}
}
