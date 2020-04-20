using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NVIDIA
{
	public class Ansel : MonoBehaviour
	{
		[SerializeField]
		public float \u001D = 5f;

		[SerializeField]
		public float \u000E = 45f;

		[SerializeField]
		public uint \u0012;

		[SerializeField]
		public uint \u0015;

		[SerializeField]
		public float \u0016 = 1f;

		private static bool \u0013;

		private static bool \u0018;

		private static bool \u0009;

		private bool \u0019;

		private Vector3 \u0011;

		private Quaternion \u001A;

		private float \u0004;

		private Ansel.CameraData \u000B;

		private Matrix4x4 \u0003;

		private Camera \u000F;

		public static bool GetFlag0013
		{
			get
			{
				return Ansel.\u0013;
			}
		}

		public static bool GetFlag0018
		{
			get
			{
				return Ansel.\u0018;
			}
		}

		public static bool IsAvailable
		{
			get
			{
				return Ansel.anselIsAvailable();
			}
		}

		public void Init()
		{
			if (!Ansel.IsAvailable)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Ansel.Init()).MethodHandle;
				}
				MonoBehaviour.print("Ansel not available on this platform");
				return;
			}
			this.\u000F = base.GetComponent<Camera>();
			Ansel.ConfigData configData = default(Ansel.ConfigData);
			float[] array = new float[3];
			array[0] = 1f;
			configData.\u0012 = array;
			float[] array2 = new float[3];
			array2[1] = 1f;
			configData.\u000E = array2;
			configData.\u001D = new float[]
			{
				0f,
				0f,
				1f
			};
			configData.\u0015 = this.\u001D;
			configData.\u0016 = this.\u000E;
			configData.\u0013 = this.\u0012;
			configData.\u0018 = this.\u0015;
			configData.\u0009 = this.\u0016;
			configData.\u0019 = true;
			configData.\u001A = true;
			configData.\u0011 = true;
			configData.\u0004 = true;
			configData.\u000B = false;
			Ansel.anselInit(ref configData);
			this.\u000B = default(Ansel.CameraData);
			Ansel.SessionData sessionData = default(Ansel.SessionData);
			sessionData.\u001D = true;
			sessionData.\u0015 = true;
			sessionData.\u0016 = true;
			sessionData.\u0013 = true;
			sessionData.\u0018 = true;
			sessionData.\u0009 = true;
			sessionData.\u0012 = true;
			sessionData.\u000E = true;
			Ansel.anselConfigureSession(ref sessionData);
			MonoBehaviour.print("Ansel is initialized and ready to use");
		}

		public void ConfigureSession(Ansel.SessionData sessionData)
		{
			if (!Ansel.IsAvailable)
			{
				Debug.LogError("Ansel is not available");
				return;
			}
			if (Ansel.anselIsSessionOn())
			{
				Debug.LogError("Ansel session cannot be configured while session is active");
				return;
			}
			Ansel.anselConfigureSession(ref sessionData);
		}

		public void \u000D()
		{
			if (!Ansel.IsAvailable)
			{
				Debug.LogError("Ansel is not available");
				return;
			}
			if (Ansel.anselIsSessionOn())
			{
				if (!Ansel.\u0013)
				{
					Ansel.\u0013 = true;
					this.\u0008();
					MonoBehaviour.print("Started Ansel session");
				}
				Ansel.\u0018 = Ansel.anselIsCaptureOn();
				Transform transform = this.\u000F.transform;
				this.\u000B.\u001D = this.\u000F.fieldOfView;
				this.\u000B.\u000E = new float[2];
				this.\u000B.\u0012 = new float[]
				{
					transform.position.x,
					transform.position.y,
					transform.position.z
				};
				this.\u000B.\u0015 = new float[]
				{
					transform.rotation.x,
					transform.rotation.y,
					transform.rotation.z,
					transform.rotation.w
				};
				Ansel.anselUpdateCamera(ref this.\u000B);
				this.\u000F.ResetProjectionMatrix();
				this.\u000F.transform.position = new Vector3(this.\u000B.\u0012[0], this.\u000B.\u0012[1], this.\u000B.\u0012[2]);
				this.\u000F.transform.rotation = new Quaternion(this.\u000B.\u0015[0], this.\u000B.\u0015[1], this.\u000B.\u0015[2], this.\u000B.\u0015[3]);
				this.\u000F.fieldOfView = this.\u000B.\u001D;
				if (this.\u000B.\u000E[0] == 0f)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(Ansel.\u000D()).MethodHandle;
					}
					if (this.\u000B.\u000E[1] == 0f)
					{
						goto IL_2B2;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.\u0003 = this.\u000F.projectionMatrix;
				float num = -1f + this.\u000B.\u000E[0];
				float num2 = num + 2f;
				float num3 = -1f + this.\u000B.\u000E[1];
				float num4 = num3 + 2f;
				this.\u0003[0, 2] = (num + num2) / (num2 - num);
				this.\u0003[1, 2] = (num4 + num3) / (num4 - num3);
				this.\u000F.projectionMatrix = this.\u0003;
				IL_2B2:;
			}
			else if (Ansel.\u0013)
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
				Ansel.\u0013 = false;
				Ansel.\u0018 = false;
				this.\u0002();
				MonoBehaviour.print("Stopped Ansel session");
			}
		}

		private void \u0008()
		{
			Transform transform = this.\u000F.transform;
			this.\u0011 = transform.position;
			this.\u001A = transform.rotation;
			this.\u0004 = this.\u000F.fieldOfView;
			this.\u0019 = Cursor.visible;
			Time.timeScale = 0f;
			Input.ResetInputAxes();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void \u0002()
		{
			Time.timeScale = 1f;
			this.\u000F.ResetProjectionMatrix();
			this.\u000F.transform.position = this.\u0011;
			this.\u000F.transform.rotation = this.\u001A;
			this.\u000F.fieldOfView = this.\u0004;
			Cursor.visible = this.\u0019;
			Cursor.lockState = CursorLockMode.None;
		}

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void anselInit(ref Ansel.ConfigData configData);

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void anselUpdateCamera(ref Ansel.CameraData cameraData);

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void anselConfigureSession(ref Ansel.SessionData sessionData);

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool anselIsSessionOn();

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool anselIsCaptureOn();

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool anselIsAvailable();

		public struct ConfigData
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] \u001D;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] \u000E;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] \u0012;

			public float \u0015;

			public float \u0016;

			public uint \u0013;

			public uint \u0018;

			public float \u0009;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u0019;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u0011;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u001A;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u0004;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u000B;
		}

		public struct CameraData
		{
			public float \u001D;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public float[] \u000E;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] \u0012;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] \u0015;
		}

		public struct SessionData
		{
			[MarshalAs(UnmanagedType.I1)]
			public bool \u001D;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u000E;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u0012;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u0015;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u0016;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u0013;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u0018;

			[MarshalAs(UnmanagedType.I1)]
			public bool \u0009;
		}
	}
}
