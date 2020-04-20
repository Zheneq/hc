using System;
using CameraManagerInternal;
using UnityEngine;

public class TauntBackgroundCamera : MonoBehaviour
{
	private AnimatedCamera m_animatedCameraComp;

	private Fixed_CasterAndTargetsCamera m_fixedCasterAndTargetCam;

	private Camera m_camera;

	private void Awake()
	{
		this.m_animatedCameraComp = base.GetComponent<AnimatedCamera>();
		this.m_fixedCasterAndTargetCam = base.GetComponent<Fixed_CasterAndTargetsCamera>();
		this.m_camera = base.GetComponentInChildren<Camera>();
	}

	public void SetAnimatedCameraTargetObj(GameObject obj)
	{
		if (this.m_animatedCameraComp != null)
		{
			this.m_animatedCameraComp.SetAnimator(obj);
		}
	}

	public void OnCamShotStart(global::CameraType camType)
	{
		if (camType == global::CameraType.Animated)
		{
			if (this.m_animatedCameraComp != null)
			{
				this.m_animatedCameraComp.enabled = true;
			}
		}
		if (camType == global::CameraType.Fixed_CasterAndTargets)
		{
			if (this.m_fixedCasterAndTargetCam != null)
			{
				this.m_fixedCasterAndTargetCam.enabled = true;
			}
		}
	}

	public void OnCamShotStop()
	{
		if (this.m_animatedCameraComp != null)
		{
			this.m_animatedCameraComp.SetAnimator(null);
			this.m_animatedCameraComp.enabled = false;
		}
		if (this.m_fixedCasterAndTargetCam != null)
		{
			this.m_fixedCasterAndTargetCam.SetAnimator(null);
			this.m_fixedCasterAndTargetCam.enabled = false;
		}
	}

	public void SetFixedCasterAndTargetObj(GameObject obj)
	{
		if (this.m_fixedCasterAndTargetCam != null)
		{
			this.m_fixedCasterAndTargetCam.SetAnimator(obj);
		}
	}

	private void LateUpdate()
	{
		if (this.m_camera != null)
		{
			if (Camera.main != null && CameraManager.Get() != null)
			{
				if (CameraManager.Get().ShotSequence != null)
				{
					this.m_camera.fieldOfView = Camera.main.fieldOfView;
				}
			}
		}
	}
}
