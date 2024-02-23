using System.Runtime.InteropServices;
using UnityEngine;

namespace NVIDIA
{
	public class Ansel : MonoBehaviour
	{
		public struct ConfigData
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] _001D;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] _000E;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] _0012;

			public float _0015;

			public float _0016;

			public uint _0013;

			public uint _0018;

			public float _0009;

			[MarshalAs(UnmanagedType.I1)]
			public bool _0019;

			[MarshalAs(UnmanagedType.I1)]
			public bool _0011;

			[MarshalAs(UnmanagedType.I1)]
			public bool _001A;

			[MarshalAs(UnmanagedType.I1)]
			public bool _0004;

			[MarshalAs(UnmanagedType.I1)]
			public bool _000B;
		}

		public struct CameraData
		{
			public float _001D;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public float[] _000E;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public float[] _0012;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] _0015;
		}

		public struct SessionData
		{
			[MarshalAs(UnmanagedType.I1)]
			public bool _001D;

			[MarshalAs(UnmanagedType.I1)]
			public bool _000E;

			[MarshalAs(UnmanagedType.I1)]
			public bool _0012;

			[MarshalAs(UnmanagedType.I1)]
			public bool _0015;

			[MarshalAs(UnmanagedType.I1)]
			public bool _0016;

			[MarshalAs(UnmanagedType.I1)]
			public bool _0013;

			[MarshalAs(UnmanagedType.I1)]
			public bool _0018;

			[MarshalAs(UnmanagedType.I1)]
			public bool _0009;
		}

		[SerializeField]
		public float _001D = 5f;

		[SerializeField]
		public float _000E = 45f;

		[SerializeField]
		public uint _0012;

		[SerializeField]
		public uint _0015;

		[SerializeField]
		public float _0016 = 1f;

		private static bool _0013;

		private static bool _0018;

		private static bool _0009;

		private bool _0019;

		private Vector3 _0011;

		private Quaternion _001A;

		private float _0004;

		private CameraData _000B;

		private Matrix4x4 _0003;

		private Camera _000F;

		public static bool GetFlag0013
		{
			get { return _0013; }
		}

		public static bool GetFlag0018
		{
			get { return _0018; }
		}

		public static bool IsAvailable
		{
			get { return anselIsAvailable(); }
		}

		public void Init()
		{
			if (!IsAvailable)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						MonoBehaviour.print("Ansel not available on this platform");
						return;
					}
				}
			}
			_000F = GetComponent<Camera>();
			ConfigData configData = default(ConfigData);
			configData._0012 = new float[3]
			{
				1f,
				0f,
				0f
			};
			configData._000E = new float[3]
			{
				0f,
				1f,
				0f
			};
			configData._001D = new float[3]
			{
				0f,
				0f,
				1f
			};
			configData._0015 = _001D;
			configData._0016 = _000E;
			configData._0013 = _0012;
			configData._0018 = _0015;
			configData._0009 = _0016;
			configData._0019 = true;
			configData._001A = true;
			configData._0011 = true;
			configData._0004 = true;
			configData._000B = false;
			anselInit(ref configData);
			_000B = default(CameraData);
			SessionData sessionData = default(SessionData);
			sessionData._001D = true;
			sessionData._0015 = true;
			sessionData._0016 = true;
			sessionData._0013 = true;
			sessionData._0018 = true;
			sessionData._0009 = true;
			sessionData._0012 = true;
			sessionData._000E = true;
			anselConfigureSession(ref sessionData);
			MonoBehaviour.print("Ansel is initialized and ready to use");
		}

		public void ConfigureSession(SessionData sessionData)
		{
			if (!IsAvailable)
			{
				Debug.LogError("Ansel is not available");
			}
			else if (anselIsSessionOn())
			{
				Debug.LogError("Ansel session cannot be configured while session is active");
			}
			else
			{
				anselConfigureSession(ref sessionData);
			}
		}

		public void _000D()
		{
			if (!IsAvailable)
			{
				Debug.LogError("Ansel is not available");
			}
			else if (anselIsSessionOn())
			{
				if (!_0013)
				{
					_0013 = true;
					_0008();
					MonoBehaviour.print("Started Ansel session");
				}
				_0018 = anselIsCaptureOn();
				Transform transform = _000F.transform;
				_000B._001D = _000F.fieldOfView;
				_000B._000E = new float[2];
				ref CameraData reference = ref _000B;
				float[] array = new float[3];
				Vector3 position = transform.position;
				array[0] = position.x;
				Vector3 position2 = transform.position;
				array[1] = position2.y;
				Vector3 position3 = transform.position;
				array[2] = position3.z;
				reference._0012 = array;
				ref CameraData reference2 = ref _000B;
				float[] array2 = new float[4];
				Quaternion rotation = transform.rotation;
				array2[0] = rotation.x;
				Quaternion rotation2 = transform.rotation;
				array2[1] = rotation2.y;
				Quaternion rotation3 = transform.rotation;
				array2[2] = rotation3.z;
				Quaternion rotation4 = transform.rotation;
				array2[3] = rotation4.w;
				reference2._0015 = array2;
				anselUpdateCamera(ref _000B);
				_000F.ResetProjectionMatrix();
				_000F.transform.position = new Vector3(_000B._0012[0], _000B._0012[1], _000B._0012[2]);
				_000F.transform.rotation = new Quaternion(_000B._0015[0], _000B._0015[1], _000B._0015[2], _000B._0015[3]);
				_000F.fieldOfView = _000B._001D;
				if (_000B._000E[0] == 0f)
				{
					if (_000B._000E[1] == 0f)
					{
						return;
					}
				}
				_0003 = _000F.projectionMatrix;
				float num = -1f + _000B._000E[0];
				float num2 = num + 2f;
				float num3 = -1f + _000B._000E[1];
				float num4 = num3 + 2f;
				_0003[0, 2] = (num + num2) / (num2 - num);
				_0003[1, 2] = (num4 + num3) / (num4 - num3);
				_000F.projectionMatrix = _0003;
			}
			else
			{
				if (!_0013)
				{
					return;
				}
				while (true)
				{
					_0013 = false;
					_0018 = false;
					_0002();
					MonoBehaviour.print("Stopped Ansel session");
					return;
				}
			}
		}

		private void _0008()
		{
			Transform transform = _000F.transform;
			_0011 = transform.position;
			_001A = transform.rotation;
			_0004 = _000F.fieldOfView;
			_0019 = Cursor.visible;
			Time.timeScale = 0f;
			Input.ResetInputAxes();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void _0002()
		{
			Time.timeScale = 1f;
			_000F.ResetProjectionMatrix();
			_000F.transform.position = _0011;
			_000F.transform.rotation = _001A;
			_000F.fieldOfView = _0004;
			Cursor.visible = _0019;
			Cursor.lockState = CursorLockMode.None;
		}

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void anselInit(ref ConfigData configData);

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void anselUpdateCamera(ref CameraData cameraData);

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void anselConfigureSession(ref SessionData sessionData);

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool anselIsSessionOn();

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool anselIsCaptureOn();

		[DllImport("AnselPlugin32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool anselIsAvailable();
	}
}
