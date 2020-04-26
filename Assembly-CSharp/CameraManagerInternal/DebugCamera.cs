using UnityEngine;

namespace CameraManagerInternal
{
	public class DebugCamera : MonoBehaviour
	{
		private float m_fastSpeedMultiplier = 5f;

		private float m_translationSpeed = 0.1f;

		private float m_sensitivityX = 5f;

		private float m_sensitivityY = 5f;

		private float m_debugFOV = 45f;

		private void Start()
		{
			m_debugFOV = Camera.main.fieldOfView;
		}

		internal bool AllowCameraShake()
		{
			return false;
		}

		internal float GetDebugFOV()
		{
			return m_debugFOV;
		}

		internal void SetDebugFOV(float fov)
		{
			m_debugFOV = fov;
		}

		private void Update()
		{
			bool flag = GameManager.IsEditorAndNotGame();
			if (Camera.main.fieldOfView != m_debugFOV)
			{
				Camera.main.fieldOfView = m_debugFOV;
			}
			Vector3 position = base.transform.position;
			if (!Input.GetMouseButton(2))
			{
				if (!flag)
				{
					goto IL_0105;
				}
				if (!Input.GetMouseButton(0))
				{
					if (!Input.GetMouseButton(1))
					{
						goto IL_0105;
					}
				}
			}
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			float x = localEulerAngles.x - Input.GetAxis("Mouse Y") * m_sensitivityX;
			Vector3 localEulerAngles2 = base.transform.localEulerAngles;
			float y = localEulerAngles2.y + Input.GetAxis("Mouse X") * m_sensitivityY;
			base.transform.localEulerAngles = new Vector3(x, y, 0f);
			goto IL_0105;
			IL_0142:
			Vector3 zero;
			if (Input.GetKey(KeyCode.W))
			{
				zero += base.transform.forward * m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.A))
			{
				zero -= base.transform.right * m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.S))
			{
				zero -= base.transform.forward * m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.D))
			{
				zero += base.transform.right * m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.R))
			{
				zero += base.transform.up * m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.F))
			{
				zero -= base.transform.up * m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				zero *= m_fastSpeedMultiplier;
			}
			goto IL_02bd;
			IL_0105:
			zero = Vector3.zero;
			if (!UIUtils.InputFieldHasFocus())
			{
				if (AccountPreferences.DoesApplicationHaveFocus())
				{
					goto IL_0142;
				}
			}
			if (flag)
			{
				goto IL_0142;
			}
			goto IL_02bd;
			IL_02bd:
			base.transform.position += zero;
			if (!((position - base.transform.position).sqrMagnitude > float.Epsilon))
			{
				return;
			}
			while (true)
			{
				if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null && HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SortNameplates();
				}
				return;
			}
		}
	}
}
