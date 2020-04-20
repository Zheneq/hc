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
		this.m_cameraAnimationObj = actor.GetActorModelData().gameObject.FindInChildren("camera0", 0);
		if (faceCam != null)
		{
			faceCam.fieldOfView = this.m_fieldOfView;
			if (!faceCam.gameObject.activeInHierarchy)
			{
				faceCam.gameObject.SetActive(true);
			}
		}
		ActorModelData actorModelData = actor.GetActorModelData();
		Animator animator = (!(actorModelData == null)) ? actorModelData.GetModelAnimator() : null;
		if (animator != null)
		{
			animator.SetInteger("Attack", this.m_animationIndex);
		}
	}

	internal bool Update(Camera faceCam)
	{
		ActorModelData faceActorModelData = this.Actor.GetFaceActorModelData();
		if (faceActorModelData != null && !faceActorModelData.IsPlayingIdleAnim(false))
		{
			Animator animator;
			if (faceActorModelData == null)
			{
				animator = null;
			}
			else
			{
				animator = faceActorModelData.GetModelAnimator();
			}
			Animator animator2 = animator;
			if (animator2 != null)
			{
				animator2.SetInteger("Attack", 0);
			}
			faceCam.transform.position = this.m_cameraAnimationObj.transform.position;
			faceCam.transform.rotation = this.m_cameraAnimationObj.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
		}
		this.m_time += Time.deltaTime;
		bool result;
		if (this.m_time >= this.m_duration)
		{
			result = (this.m_duration <= 0f);
		}
		else
		{
			result = true;
		}
		return result;
	}
}
