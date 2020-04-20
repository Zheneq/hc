using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class CameraController : MonoBehaviour
	{
		private Transform symbol_001D;

		private Transform symbol_000E;

		public Transform symbol_0012;

		public float symbol_0015 = 30f;

		public float symbol_0016 = 100f;

		public float symbol_0013 = 2f;

		public float symbol_0018 = 30f;

		public float symbol_0009 = 85f;

		public float symbol_0019;

		public float symbol_0011;

		public CameraController.CameraModes symbol_001A;

		public bool symbol_0004 = true;

		public bool symbol_000B;

		private bool symbol_0003;

		public float symbol_000F = 25f;

		public float symbol_0017 = 5f;

		public float symbol_000D = 2f;

		private Vector3 symbol_0008 = Vector3.zero;

		private Vector3 symbol_0002;

		private float symbol_000A;

		private float symbol_0006;

		private Vector3 symbol_0020;

		private float symbol_000C;

		private const string symbol_0014 = "Slider - Smoothing Value";

		private const string symbol_0005 = "Slider - Camera Zoom";

		private void symbol_001B()
		{
			if (QualitySettings.vSyncCount > 0)
			{
				Application.targetFrameRate = 0x3C;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform != RuntimePlatform.Android)
				{
					goto IL_4D;
				}
			}
			Input.simulateMouseWithTouches = false;
			IL_4D:
			this.symbol_001D = base.transform;
			this.symbol_0003 = this.symbol_0004;
		}

		private void symbol_001E()
		{
			if (this.symbol_0012 == null)
			{
				this.symbol_000E = new GameObject("Camera Target").transform;
				this.symbol_0012 = this.symbol_000E;
			}
		}

		private void symbol_0001()
		{
			this.symbol_001F();
			if (this.symbol_0012 != null)
			{
				if (this.symbol_001A == CameraController.CameraModes.symbol_000E)
				{
					this.symbol_0002 = this.symbol_0012.position + Quaternion.Euler(this.symbol_0018, this.symbol_0011, 0f) * new Vector3(0f, 0f, -this.symbol_0015);
				}
				else if (this.symbol_001A == CameraController.CameraModes.symbol_001D)
				{
					this.symbol_0002 = this.symbol_0012.position + this.symbol_0012.TransformDirection(Quaternion.Euler(this.symbol_0018, this.symbol_0011, 0f) * new Vector3(0f, 0f, -this.symbol_0015));
				}
				if (this.symbol_0004)
				{
					this.symbol_001D.position = Vector3.SmoothDamp(this.symbol_001D.position, this.symbol_0002, ref this.symbol_0008, this.symbol_000F * Time.fixedDeltaTime);
				}
				else
				{
					this.symbol_001D.position = this.symbol_0002;
				}
				if (this.symbol_000B)
				{
					this.symbol_001D.rotation = Quaternion.Lerp(this.symbol_001D.rotation, Quaternion.LookRotation(this.symbol_0012.position - this.symbol_001D.position), this.symbol_0017 * Time.deltaTime);
				}
				else
				{
					this.symbol_001D.LookAt(this.symbol_0012);
				}
			}
		}

		private void symbol_001F()
		{
			this.symbol_0020 = Vector3.zero;
			this.symbol_000C = Input.GetAxis("Mouse ScrollWheel");
			float num = (float)Input.touchCount;
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				if (!Input.GetKey(KeyCode.RightShift))
				{
					if (num <= 0f)
					{
						goto IL_541;
					}
				}
			}
			this.symbol_000C *= 10f;
			if (Input.GetKeyDown(KeyCode.I))
			{
				this.symbol_001A = CameraController.CameraModes.symbol_000E;
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				this.symbol_001A = CameraController.CameraModes.symbol_001D;
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				this.symbol_0004 = !this.symbol_0004;
			}
			if (Input.GetMouseButton(1))
			{
				this.symbol_0006 = Input.GetAxis("Mouse Y");
				this.symbol_000A = Input.GetAxis("Mouse X");
				if (this.symbol_0006 <= 0.01f)
				{
					if (this.symbol_0006 >= -0.01f)
					{
						goto IL_16C;
					}
				}
				this.symbol_0018 -= this.symbol_0006 * this.symbol_000D;
				this.symbol_0018 = Mathf.Clamp(this.symbol_0018, this.symbol_0019, this.symbol_0009);
				IL_16C:
				if (this.symbol_000A <= 0.01f)
				{
					if (this.symbol_000A >= -0.01f)
					{
						goto IL_1FC;
					}
				}
				this.symbol_0011 += this.symbol_000A * this.symbol_000D;
				if (this.symbol_0011 > 360f)
				{
					this.symbol_0011 -= 360f;
				}
				if (this.symbol_0011 < 0f)
				{
					this.symbol_0011 += 360f;
				}
			}
			IL_1FC:
			if (num == 1f && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
				if (deltaPosition.y <= 0.01f)
				{
					if (deltaPosition.y >= -0.01f)
					{
						goto IL_2A3;
					}
				}
				this.symbol_0018 -= deltaPosition.y * 0.1f;
				this.symbol_0018 = Mathf.Clamp(this.symbol_0018, this.symbol_0019, this.symbol_0009);
				IL_2A3:
				if (deltaPosition.x <= 0.01f)
				{
					if (deltaPosition.x >= -0.01f)
					{
						goto IL_33F;
					}
				}
				this.symbol_0011 += deltaPosition.x * 0.1f;
				if (this.symbol_0011 > 360f)
				{
					this.symbol_0011 -= 360f;
				}
				if (this.symbol_0011 < 0f)
				{
					this.symbol_0011 += 360f;
				}
			}
			IL_33F:
			if (Input.GetMouseButton(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, 300f, 0x5C00))
				{
					if (raycastHit.transform == this.symbol_0012)
					{
						this.symbol_0011 = 0f;
					}
					else
					{
						this.symbol_0012 = raycastHit.transform;
						this.symbol_0011 = 0f;
						this.symbol_0004 = this.symbol_0003;
					}
				}
			}
			if (Input.GetMouseButton(2))
			{
				if (this.symbol_000E == null)
				{
					this.symbol_000E = new GameObject("Camera Target").transform;
					this.symbol_000E.position = this.symbol_0012.position;
					this.symbol_000E.rotation = this.symbol_0012.rotation;
					this.symbol_0012 = this.symbol_000E;
					this.symbol_0003 = this.symbol_0004;
					this.symbol_0004 = false;
				}
				else if (this.symbol_000E != this.symbol_0012)
				{
					this.symbol_000E.position = this.symbol_0012.position;
					this.symbol_000E.rotation = this.symbol_0012.rotation;
					this.symbol_0012 = this.symbol_000E;
					this.symbol_0003 = this.symbol_0004;
					this.symbol_0004 = false;
				}
				this.symbol_0006 = Input.GetAxis("Mouse Y");
				this.symbol_000A = Input.GetAxis("Mouse X");
				this.symbol_0020 = this.symbol_001D.TransformDirection(this.symbol_000A, this.symbol_0006, 0f);
				this.symbol_000E.Translate(-this.symbol_0020, Space.World);
			}
			IL_541:
			if (num == 2f)
			{
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
				}
				this.symbol_0015 += num2 * 0.25f;
				this.symbol_0015 = Mathf.Clamp(this.symbol_0015, this.symbol_0013, this.symbol_0016);
			}
			IL_62D:
			if (this.symbol_000C >= -0.01f)
			{
				if (this.symbol_000C <= 0.01f)
				{
					return;
				}
			}
			this.symbol_0015 -= this.symbol_000C * 5f;
			this.symbol_0015 = Mathf.Clamp(this.symbol_0015, this.symbol_0013, this.symbol_0016);
		}

		public enum CameraModes
		{
			symbol_001D,
			symbol_000E,
			symbol_0012
		}
	}
}
