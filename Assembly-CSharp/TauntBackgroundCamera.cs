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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TauntBackgroundCamera.OnCamShotStart(global::CameraType)).MethodHandle;
			}
			if (this.m_animatedCameraComp != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_animatedCameraComp.enabled = true;
			}
		}
		if (camType == global::CameraType.Fixed_CasterAndTargets)
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TauntBackgroundCamera.OnCamShotStop()).MethodHandle;
			}
			this.m_animatedCameraComp.SetAnimator(null);
			this.m_animatedCameraComp.enabled = false;
		}
		if (this.m_fixedCasterAndTargetCam != null)
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
			this.m_fixedCasterAndTargetCam.SetAnimator(null);
			this.m_fixedCasterAndTargetCam.enabled = false;
		}
	}

	public void SetFixedCasterAndTargetObj(GameObject obj)
	{
		if (this.m_fixedCasterAndTargetCam != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TauntBackgroundCamera.SetFixedCasterAndTargetObj(GameObject)).MethodHandle;
			}
			this.m_fixedCasterAndTargetCam.SetAnimator(obj);
		}
	}

	private void LateUpdate()
	{
		if (this.m_camera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TauntBackgroundCamera.LateUpdate()).MethodHandle;
			}
			if (Camera.main != null && CameraManager.Get() != null)
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
				if (CameraManager.Get().ShotSequence != null)
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
					this.m_camera.fieldOfView = Camera.main.fieldOfView;
				}
			}
		}
	}
}
