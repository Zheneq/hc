using CameraManagerInternal;
using UnityEngine;

public class TauntBackgroundCamera : MonoBehaviour
{
	private AnimatedCamera m_animatedCameraComp;

	private Fixed_CasterAndTargetsCamera m_fixedCasterAndTargetCam;

	private Camera m_camera;

	private void Awake()
	{
		m_animatedCameraComp = GetComponent<AnimatedCamera>();
		m_fixedCasterAndTargetCam = GetComponent<Fixed_CasterAndTargetsCamera>();
		m_camera = GetComponentInChildren<Camera>();
	}

	public void SetAnimatedCameraTargetObj(GameObject obj)
	{
		if (m_animatedCameraComp != null)
		{
			m_animatedCameraComp.SetAnimator(obj);
		}
	}

	public void OnCamShotStart(CameraType camType)
	{
		if (camType == CameraType.Animated)
		{
			if (m_animatedCameraComp != null)
			{
				m_animatedCameraComp.enabled = true;
			}
		}
		if (camType != CameraType.Fixed_CasterAndTargets)
		{
			return;
		}
		while (true)
		{
			if (m_fixedCasterAndTargetCam != null)
			{
				m_fixedCasterAndTargetCam.enabled = true;
			}
			return;
		}
	}

	public void OnCamShotStop()
	{
		if (m_animatedCameraComp != null)
		{
			m_animatedCameraComp.SetAnimator(null);
			m_animatedCameraComp.enabled = false;
		}
		if (!(m_fixedCasterAndTargetCam != null))
		{
			return;
		}
		while (true)
		{
			m_fixedCasterAndTargetCam.SetAnimator(null);
			m_fixedCasterAndTargetCam.enabled = false;
			return;
		}
	}

	public void SetFixedCasterAndTargetObj(GameObject obj)
	{
		if (!(m_fixedCasterAndTargetCam != null))
		{
			return;
		}
		while (true)
		{
			m_fixedCasterAndTargetCam.SetAnimator(obj);
			return;
		}
	}

	private void LateUpdate()
	{
		if (!(m_camera != null))
		{
			return;
		}
		while (true)
		{
			if (!(Camera.main != null) || !(CameraManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (CameraManager.Get().ShotSequence != null)
				{
					while (true)
					{
						m_camera.fieldOfView = Camera.main.fieldOfView;
						return;
					}
				}
				return;
			}
		}
	}
}
