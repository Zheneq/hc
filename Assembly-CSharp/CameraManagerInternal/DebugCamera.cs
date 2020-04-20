using System;
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
			this.m_debugFOV = Camera.main.fieldOfView;
		}

		internal bool AllowCameraShake()
		{
			return false;
		}

		internal float GetDebugFOV()
		{
			return this.m_debugFOV;
		}

		internal void SetDebugFOV(float fov)
		{
			this.m_debugFOV = fov;
		}

		private void Update()
		{
			bool flag = GameManager.IsEditorAndNotGame();
			if (Camera.main.fieldOfView != this.m_debugFOV)
			{
				Camera.main.fieldOfView = this.m_debugFOV;
			}
			Vector3 position = base.transform.position;
			if (!Input.GetMouseButton(2))
			{
				if (!flag)
				{
					goto IL_105;
				}
				if (!Input.GetMouseButton(0))
				{
					if (!Input.GetMouseButton(1))
					{
						goto IL_105;
					}
				}
			}
			float x = base.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * this.m_sensitivityX;
			float y = base.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * this.m_sensitivityY;
			base.transform.localEulerAngles = new Vector3(x, y, 0f);
			IL_105:
			Vector3 vector = Vector3.zero;
			if (!UIUtils.InputFieldHasFocus())
			{
				if (AccountPreferences.DoesApplicationHaveFocus())
				{
					goto IL_142;
				}
			}
			if (!flag)
			{
				goto IL_2BD;
			}
			IL_142:
			if (Input.GetKey(KeyCode.W))
			{
				vector += base.transform.forward * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.A))
			{
				vector -= base.transform.right * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.S))
			{
				vector -= base.transform.forward * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.D))
			{
				vector += base.transform.right * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.R))
			{
				vector += base.transform.up * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.F))
			{
				vector -= base.transform.up * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector *= this.m_fastSpeedMultiplier;
			}
			IL_2BD:
			base.transform.position += vector;
			if ((position - base.transform.position).sqrMagnitude > 1.401298E-45f)
			{
				if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null && HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SortNameplates();
				}
			}
		}
	}
}
