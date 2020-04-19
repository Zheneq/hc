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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCamera.Update()).MethodHandle;
				}
				Camera.main.fieldOfView = this.m_debugFOV;
			}
			Vector3 position = base.transform.position;
			if (!Input.GetMouseButton(2))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!flag)
				{
					goto IL_105;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!Input.GetMouseButton(0))
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
					if (!Input.GetMouseButton(1))
					{
						goto IL_105;
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (AccountPreferences.DoesApplicationHaveFocus())
				{
					goto IL_142;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (!flag)
			{
				goto IL_2BD;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			IL_142:
			if (Input.GetKey(KeyCode.W))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				vector += base.transform.forward * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.A))
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
				vector -= base.transform.right * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.S))
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
				vector -= base.transform.forward * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.D))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				vector += base.transform.right * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.R))
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
				vector += base.transform.up * this.m_translationSpeed;
			}
			if (Input.GetKey(KeyCode.F))
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null && HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SortNameplates();
				}
			}
		}
	}
}
