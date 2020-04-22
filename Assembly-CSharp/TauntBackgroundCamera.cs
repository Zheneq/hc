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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_animatedCameraComp != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_animatedCameraComp.enabled = true;
			}
		}
		if (camType != CameraType.Fixed_CasterAndTargets)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_animatedCameraComp.SetAnimator(null);
			m_animatedCameraComp.enabled = false;
		}
		if (!(m_fixedCasterAndTargetCam != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(Camera.main != null) || !(CameraManager.Get() != null))
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
				if (CameraManager.Get().ShotSequence != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						m_camera.fieldOfView = Camera.main.fieldOfView;
						return;
					}
				}
				return;
			}
		}
	}
}
