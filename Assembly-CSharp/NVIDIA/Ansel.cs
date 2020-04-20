using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NVIDIA
{
	public class Ansel : MonoBehaviour
	{
		[SerializeField]
		public float symbol_001D = 5f;

		[SerializeField]
		public float symbol_000E = 45f;

		[SerializeField]
		public uint symbol_0012;

		[SerializeField]
		public uint symbol_0015;

		[SerializeField]
		public float symbol_0016 = 1f;

		private static bool symbol_0013;

		private static bool symbol_0018;

		private static bool symbol_0009;

		private bool symbol_0019;

		private Vector3 symbol_0011;

		private Quaternion symbol_001A;

		private float symbol_0004;

		private Ansel.CameraData symbol_000B;

		private Matrix4x4 symbol_0003;

		private Camera symbol_000F;

		public static bool GetFlag0013
		{
			get
			{
				return Ansel.symbol_0013;
			}
		}

		public static bool GetFlag0018
		{
			get
			{
				return Ansel.symbol_0018;
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
				MonoBehaviour.print("Ansel not available on this platform");
				return;
			}
			this.symbol_000F = base.GetComponent<Camera>();
			Ansel.ConfigData configData = default(Ansel.ConfigData);
			float[] array = new float[3];
			array[0] = 1f;
			configData.symbol_0012 = array;
			float[] array2 = new float[3];
			array2[1] = 1f;
			configData.symbol_000E = array2;
			configData.symbol_001D = new float[]
			{
				0f,
				0f,
				1f
			};
			configData.symbol_0015 = this.symbol_001D;
			configData.symbol_0016 = this.symbol_000E;
			configData.symbol_0013 = this.symbol_0012;
			configData.symbol_0018 = this.symbol_0015;
			configData.symbol_0009 = this.symbol_0016;
			configData.symbol_0019 = true;
			configData.symbol_001A = true;
			configData.symbol_0011 = true;
			configData.symbol_0004 = true;
			configData.symbol_000B = false;
			Ansel.anselInit(ref configData);
			this.symbol_000B = default(Ansel.CameraData);
			Ansel.SessionData sessionData = default(Ansel.SessionData);
			sessionData.symbol_001D = true;
			sessionData.symbol_0015 = true;
			sessionData.symbol_0016 = true;
			sessionData.symbol_0013 = true;
			sessionData.symbol_0018 = true;
			sessionData.symbol_0009 = true;
			sessionData.symbol_0012 = true;
			sessionData.symbol_000E = true;
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

		public void symbol_000D()
		{
			if (!Ansel.IsAvailable)
			{
				Debug.LogError("Ansel is not available");
				return;
			}
			if (Ansel.anselIsSessionOn())
			{
				if (!Ansel.symbol_0013)
				{
					Ansel.symbol_0013 = true;
					this.symbol_0008();
					MonoBehaviour.print("Started Ansel session");
				}
				Ansel.symbol_0018 = Ansel.anselIsCaptureOn();
				Transform transform = this.symbol_000F.transform;
				this.symbol_000B.symbol_001D = this.symbol_000F.fieldOfView;
				this.symbol_000B.symbol_000E = new float[2];
				this.symbol_000B.symbol_0012 = new float[]
				{
					transform.position.x,
					transform.position.y,
					transform.position.z
				};
				this.symbol_000B.symbol_0015 = new float[]
				{
					transform.rotation.x,
					transform.rotation.y,
					transform.rotation.z,
					transform.rotation.w
				};
				Ansel.anselUpdateCamera(ref this.symbol_000B);
				this.symbol_000F.ResetProjectionMatrix();
				this.symbol_000F.transform.position = new Vector3(this.symbol_000B.symbol_0012[0], this.symbol_000B.symbol_0012[1], this.symbol_000B.symbol_0012[2]);
				this.symbol_000F.transform.rotation = new Quaternion(this.symbol_000B.symbol_0015[0], this.symbol_000B.symbol_0015[1], this.symbol_000B.symbol_0015[2], this.symbol_000B.symbol_0015[3]);
				this.symbol_000F.fieldOfView = this.symbol_000B.symbol_001D;
				if (this.symbol_000B.symbol_000E[0] == 0f)
				{
					if (this.symbol_000B.symbol_000E[1] == 0f)
					{
						goto IL_2B2;
					}
				}
				this.symbol_0003 = this.symbol_000F.projectionMatrix;
				float num = -1f + this.symbol_000B.symbol_000E[0];
				float num2 = num + 2f;
				float num3 = -1f + this.symbol_000B.symbol_000E[1];
				float num4 = num3 + 2f;
				this.symbol_0003[0, 2] = (num + num2) / (num2 - num);
				this.symbol_0003[1, 2] = (num4 + num3) / (num4 - num3);
				this.symbol_000F.projectionMatrix = this.symbol_0003;
				IL_2B2:;
			}
			else if (Ansel.symbol_0013)
			{
				Ansel.symbol_0013 = false;
				Ansel.symbol_0018 = false;
				this.symbol_0002();
				MonoBehaviour.print("Stopped Ansel session");
			}
		}

		private void symbol_0008()
		{
			Transform transform = this.symbol_000F.transform;
			this.symbol_0011 = transform.position;
			this.symbol_001A = transform.rotation;
			this.symbol_0004 = this.symbol_000F.fieldOfView;
			this.symbol_0019 = Cursor.visible;
			Time.timeScale = 0f;
			Input.ResetInputAxes();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void symbol_0002()
		{
			Time.timeScale = 1f;
			this.symbol_000F.ResetProjectionMatrix();
			this.symbol_000F.transform.position = this.symbol_0011;
			this.symbol_000F.transform.rotation = this.symbol_001A;
			this.symbol_000F.fieldOfView = this.symbol_0004;
			Cursor.visible = this.symbol_0019;
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
			public float[] symbol_001D;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] symbol_000E;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] symbol_0012;

			public float symbol_0015;

			public float symbol_0016;

			public uint symbol_0013;

			public uint symbol_0018;

			public float symbol_0009;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_0019;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_0011;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_001A;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_0004;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_000B;
		}

		public struct CameraData
		{
			public float symbol_001D;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public float[] symbol_000E;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] symbol_0012;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] symbol_0015;
		}

		public struct SessionData
		{
			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_001D;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_000E;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_0012;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_0015;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_0016;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_0013;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_0018;

			[MarshalAs(UnmanagedType.I1)]
			public bool symbol_0009;
		}
	}
}
