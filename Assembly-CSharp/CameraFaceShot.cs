using System;
using UnityEngine;

[Serializable]
public class CameraFaceShot
{
	public string m_name = "Camera Face Shot Name";

	public int m_index;

	public int m_animationIndex = 1000;

	public float m_fieldOfView = 45f;

	public float m_duration = 2f;

	private GameObject m_cameraAnimationObj;

	private float m_time;

	internal ActorData Actor
	{
		get;
		private set;
	}

	internal void Begin(ActorData actor, Camera faceCam)
	{
		m_time = 0f;
		Actor = actor;
		m_cameraAnimationObj = actor.GetActorModelData().gameObject.FindInChildren("camera0");
		if (faceCam != null)
		{
			faceCam.fieldOfView = m_fieldOfView;
			if (!faceCam.gameObject.activeInHierarchy)
			{
				faceCam.gameObject.SetActive(true);
			}
		}
		ActorModelData actorModelData = actor.GetActorModelData();
		Animator animator = (!(actorModelData == null)) ? actorModelData.GetModelAnimator() : null;
		if (animator != null)
		{
			animator.SetInteger("Attack", m_animationIndex);
		}
	}

	internal bool Update(Camera faceCam)
	{
		ActorModelData faceActorModelData = Actor.GetFaceActorModelData();
		if (faceActorModelData != null && !faceActorModelData.IsPlayingIdleAnim())
		{
			object obj;
			if (faceActorModelData == null)
			{
				obj = null;
			}
			else
			{
				obj = faceActorModelData.GetModelAnimator();
			}
			Animator animator = (Animator)obj;
			if (animator != null)
			{
				animator.SetInteger("Attack", 0);
			}
			faceCam.transform.position = m_cameraAnimationObj.transform.position;
			faceCam.transform.rotation = m_cameraAnimationObj.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
		}
		m_time += Time.deltaTime;
		int result;
		if (!(m_time < m_duration))
		{
			result = ((m_duration <= 0f) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}
}
