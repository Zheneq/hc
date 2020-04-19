using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class CameraController : MonoBehaviour
	{
		private Transform \u001D;

		private Transform \u000E;

		public Transform \u0012;

		public float \u0015 = 30f;

		public float \u0016 = 100f;

		public float \u0013 = 2f;

		public float \u0018 = 30f;

		public float \u0009 = 85f;

		public float \u0019;

		public float \u0011;

		public CameraController.CameraModes \u001A;

		public bool \u0004 = true;

		public bool \u000B;

		private bool \u0003;

		public float \u000F = 25f;

		public float \u0017 = 5f;

		public float \u000D = 2f;

		private Vector3 \u0008 = Vector3.zero;

		private Vector3 \u0002;

		private float \u000A;

		private float \u0006;

		private Vector3 \u0020;

		private float \u000C;

		private const string \u0014 = "Slider - Smoothing Value";

		private const string \u0005 = "Slider - Camera Zoom";

		private void \u001B()
		{
			if (QualitySettings.vSyncCount > 0)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraController.\u001B()).MethodHandle;
				}
				Application.targetFrameRate = 0x3C;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
			if (Application.platform != RuntimePlatform.IPhonePlayer)
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
				if (Application.platform != RuntimePlatform.Android)
				{
					goto IL_4D;
				}
			}
			Input.simulateMouseWithTouches = false;
			IL_4D:
			this.\u001D = base.transform;
			this.\u0003 = this.\u0004;
		}

		private void \u001E()
		{
			if (this.\u0012 == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraController.\u001E()).MethodHandle;
				}
				this.\u000E = new GameObject("Camera Target").transform;
				this.\u0012 = this.\u000E;
			}
		}

		private void \u0001()
		{
			this.\u001F();
			if (this.\u0012 != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraController.\u0001()).MethodHandle;
				}
				if (this.\u001A == CameraController.CameraModes.\u000E)
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
					this.\u0002 = this.\u0012.position + Quaternion.Euler(this.\u0018, this.\u0011, 0f) * new Vector3(0f, 0f, -this.\u0015);
				}
				else if (this.\u001A == CameraController.CameraModes.\u001D)
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
					this.\u0002 = this.\u0012.position + this.\u0012.TransformDirection(Quaternion.Euler(this.\u0018, this.\u0011, 0f) * new Vector3(0f, 0f, -this.\u0015));
				}
				if (this.\u0004)
				{
					this.\u001D.position = Vector3.SmoothDamp(this.\u001D.position, this.\u0002, ref this.\u0008, this.\u000F * Time.fixedDeltaTime);
				}
				else
				{
					this.\u001D.position = this.\u0002;
				}
				if (this.\u000B)
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
					this.\u001D.rotation = Quaternion.Lerp(this.\u001D.rotation, Quaternion.LookRotation(this.\u0012.position - this.\u001D.position), this.\u0017 * Time.deltaTime);
				}
				else
				{
					this.\u001D.LookAt(this.\u0012);
				}
			}
		}

		private void \u001F()
		{
			this.\u0020 = Vector3.zero;
			this.\u000C = Input.GetAxis("Mouse ScrollWheel");
			float num = (float)Input.touchCount;
			if (!Input.GetKey(KeyCode.LeftShift))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraController.\u001F()).MethodHandle;
				}
				if (!Input.GetKey(KeyCode.RightShift))
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
					if (num <= 0f)
					{
						goto IL_541;
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
			}
			this.\u000C *= 10f;
			if (Input.GetKeyDown(KeyCode.I))
			{
				this.\u001A = CameraController.CameraModes.\u000E;
			}
			if (Input.GetKeyDown(KeyCode.F))
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
				this.\u001A = CameraController.CameraModes.\u001D;
			}
			if (Input.GetKeyDown(KeyCode.S))
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
				this.\u0004 = !this.\u0004;
			}
			if (Input.GetMouseButton(1))
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
				this.\u0006 = Input.GetAxis("Mouse Y");
				this.\u000A = Input.GetAxis("Mouse X");
				if (this.\u0006 <= 0.01f)
				{
					if (this.\u0006 >= -0.01f)
					{
						goto IL_16C;
					}
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.\u0018 -= this.\u0006 * this.\u000D;
				this.\u0018 = Mathf.Clamp(this.\u0018, this.\u0019, this.\u0009);
				IL_16C:
				if (this.\u000A <= 0.01f)
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
					if (this.\u000A >= -0.01f)
					{
						goto IL_1FC;
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
				this.\u0011 += this.\u000A * this.\u000D;
				if (this.\u0011 > 360f)
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
					this.\u0011 -= 360f;
				}
				if (this.\u0011 < 0f)
				{
					this.\u0011 += 360f;
				}
			}
			IL_1FC:
			if (num == 1f && Input.GetTouch(0).phase == TouchPhase.Moved)
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
				Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
				if (deltaPosition.y <= 0.01f)
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
					if (deltaPosition.y >= -0.01f)
					{
						goto IL_2A3;
					}
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.\u0018 -= deltaPosition.y * 0.1f;
				this.\u0018 = Mathf.Clamp(this.\u0018, this.\u0019, this.\u0009);
				IL_2A3:
				if (deltaPosition.x <= 0.01f)
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
					if (deltaPosition.x >= -0.01f)
					{
						goto IL_33F;
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
				this.\u0011 += deltaPosition.x * 0.1f;
				if (this.\u0011 > 360f)
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
					this.\u0011 -= 360f;
				}
				if (this.\u0011 < 0f)
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
					this.\u0011 += 360f;
				}
			}
			IL_33F:
			if (Input.GetMouseButton(0))
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
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, 300f, 0x5C00))
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
					if (raycastHit.transform == this.\u0012)
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
						this.\u0011 = 0f;
					}
					else
					{
						this.\u0012 = raycastHit.transform;
						this.\u0011 = 0f;
						this.\u0004 = this.\u0003;
					}
				}
			}
			if (Input.GetMouseButton(2))
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
				if (this.\u000E == null)
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
					this.\u000E = new GameObject("Camera Target").transform;
					this.\u000E.position = this.\u0012.position;
					this.\u000E.rotation = this.\u0012.rotation;
					this.\u0012 = this.\u000E;
					this.\u0003 = this.\u0004;
					this.\u0004 = false;
				}
				else if (this.\u000E != this.\u0012)
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
					this.\u000E.position = this.\u0012.position;
					this.\u000E.rotation = this.\u0012.rotation;
					this.\u0012 = this.\u000E;
					this.\u0003 = this.\u0004;
					this.\u0004 = false;
				}
				this.\u0006 = Input.GetAxis("Mouse Y");
				this.\u000A = Input.GetAxis("Mouse X");
				this.\u0020 = this.\u001D.TransformDirection(this.\u000A, this.\u0006, 0f);
				this.\u000E.Translate(-this.\u0020, Space.World);
			}
			IL_541:
			if (num == 2f)
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
				Touch touch = Input.GetTouch(0);
				Touch touch2 = Input.GetTouch(1);
				Vector2 a = touch.position - touch.deltaPosition;
				Vector2 b = touch2.position - touch2.deltaPosition;
				float magnitude = (a - b).magnitude;
				float magnitude2 = (touch.position - touch2.position).magnitude;
				float num2 = magnitude - magnitude2;
				if (num2 <= 0.01f)
				{
					if (num2 >= -0.01f)
					{
						goto IL_62D;
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
				}
				this.\u0015 += num2 * 0.25f;
				this.\u0015 = Mathf.Clamp(this.\u0015, this.\u0013, this.\u0016);
			}
			IL_62D:
			if (this.\u000C >= -0.01f)
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
				if (this.\u000C <= 0.01f)
				{
					return;
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
			this.\u0015 -= this.\u000C * 5f;
			this.\u0015 = Mathf.Clamp(this.\u0015, this.\u0013, this.\u0016);
		}

		public enum CameraModes
		{
			\u001D,
			\u000E,
			\u0012
		}
	}
}
