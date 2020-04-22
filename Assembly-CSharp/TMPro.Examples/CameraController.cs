using UnityEngine;

namespace TMPro.Examples
{
	public class CameraController : MonoBehaviour
	{
		public enum CameraModes
		{
			_001D,
			_000E,
			_0012
		}

		private Transform _001D;

		private Transform _000E;

		public Transform _0012;

		public float _0015 = 30f;

		public float _0016 = 100f;

		public float _0013 = 2f;

		public float _0018 = 30f;

		public float _0009 = 85f;

		public float _0019;

		public float _0011;

		public CameraModes _001A;

		public bool _0004 = true;

		public bool _000B;

		private bool _0003;

		public float _000F = 25f;

		public float _0017 = 5f;

		public float _000D = 2f;

		private Vector3 _0008 = Vector3.zero;

		private Vector3 _0002;

		private float _000A;

		private float _0006;

		private Vector3 _0020;

		private float _000C;

		private const string _0014 = "Slider - Smoothing Value";

		private const string _0005 = "Slider - Camera Zoom";

		private void _001B()
		{
			if (QualitySettings.vSyncCount > 0)
			{
				while (true)
				{
					switch (7)
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
				Application.targetFrameRate = 60;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
			if (Application.platform != RuntimePlatform.IPhonePlayer)
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
				if (Application.platform != RuntimePlatform.Android)
				{
					goto IL_004d;
				}
			}
			Input.simulateMouseWithTouches = false;
			goto IL_004d;
			IL_004d:
			_001D = base.transform;
			_0003 = _0004;
		}

		private void _001E()
		{
			if (!(_0012 == null))
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				_000E = new GameObject("Camera Target").transform;
				_0012 = _000E;
				return;
			}
		}

		private void _0001()
		{
			_001F();
			if (!(_0012 != null))
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (_001A == CameraModes._000E)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					_0002 = _0012.position + Quaternion.Euler(_0018, _0011, 0f) * new Vector3(0f, 0f, 0f - _0015);
				}
				else if (_001A == CameraModes._001D)
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
					_0002 = _0012.position + _0012.TransformDirection(Quaternion.Euler(_0018, _0011, 0f) * new Vector3(0f, 0f, 0f - _0015));
				}
				if (_0004)
				{
					_001D.position = Vector3.SmoothDamp(_001D.position, _0002, ref _0008, _000F * Time.fixedDeltaTime);
				}
				else
				{
					_001D.position = _0002;
				}
				if (_000B)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							_001D.rotation = Quaternion.Lerp(_001D.rotation, Quaternion.LookRotation(_0012.position - _001D.position), _0017 * Time.deltaTime);
							return;
						}
					}
				}
				_001D.LookAt(_0012);
				return;
			}
		}

		private void _001F()
		{
			_0020 = Vector3.zero;
			_000C = Input.GetAxis("Mouse ScrollWheel");
			float num = Input.touchCount;
			if (!Input.GetKey(KeyCode.LeftShift))
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!Input.GetKey(KeyCode.RightShift))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(num > 0f))
					{
						goto IL_0541;
					}
					while (true)
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
			_000C *= 10f;
			if (Input.GetKeyDown(KeyCode.I))
			{
				_001A = CameraModes._000E;
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				_001A = CameraModes._001D;
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				_0004 = !_0004;
			}
			if (Input.GetMouseButton(1))
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
				_0006 = Input.GetAxis("Mouse Y");
				_000A = Input.GetAxis("Mouse X");
				if (!(_0006 > 0.01f))
				{
					if (!(_0006 < -0.01f))
					{
						goto IL_016c;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				_0018 -= _0006 * _000D;
				_0018 = Mathf.Clamp(_0018, _0019, _0009);
				goto IL_016c;
			}
			goto IL_01fc;
			IL_01fc:
			Vector2 deltaPosition;
			if (num == 1f && Input.GetTouch(0).phase == TouchPhase.Moved)
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
				deltaPosition = Input.GetTouch(0).deltaPosition;
				if (!(deltaPosition.y > 0.01f))
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
					if (!(deltaPosition.y < -0.01f))
					{
						goto IL_02a3;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				_0018 -= deltaPosition.y * 0.1f;
				_0018 = Mathf.Clamp(_0018, _0019, _0009);
				goto IL_02a3;
			}
			goto IL_033f;
			IL_0541:
			if (num == 2f)
			{
				while (true)
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
				if (!(num2 > 0.01f))
				{
					if (!(num2 < -0.01f))
					{
						goto IL_062d;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				_0015 += num2 * 0.25f;
				_0015 = Mathf.Clamp(_0015, _0013, _0016);
			}
			goto IL_062d;
			IL_062d:
			if (!(_000C < -0.01f))
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
				if (!(_000C > 0.01f))
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
					break;
				}
			}
			_0015 -= _000C * 5f;
			_0015 = Mathf.Clamp(_0015, _0013, _0016);
			return;
			IL_016c:
			if (!(_000A > 0.01f))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(_000A < -0.01f))
				{
					goto IL_01fc;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			_0011 += _000A * _000D;
			if (_0011 > 360f)
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
				_0011 -= 360f;
			}
			if (_0011 < 0f)
			{
				_0011 += 360f;
			}
			goto IL_01fc;
			IL_02a3:
			if (!(deltaPosition.x > 0.01f))
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
				if (!(deltaPosition.x < -0.01f))
				{
					goto IL_033f;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			_0011 += deltaPosition.x * 0.1f;
			if (_0011 > 360f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				_0011 -= 360f;
			}
			if (_0011 < 0f)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				_0011 += 360f;
			}
			goto IL_033f;
			IL_033f:
			if (Input.GetMouseButton(0))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out RaycastHit hitInfo, 300f, 23552))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (hitInfo.transform == _0012)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						_0011 = 0f;
					}
					else
					{
						_0012 = hitInfo.transform;
						_0011 = 0f;
						_0004 = _0003;
					}
				}
			}
			if (Input.GetMouseButton(2))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (_000E == null)
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
					_000E = new GameObject("Camera Target").transform;
					_000E.position = _0012.position;
					_000E.rotation = _0012.rotation;
					_0012 = _000E;
					_0003 = _0004;
					_0004 = false;
				}
				else if (_000E != _0012)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					_000E.position = _0012.position;
					_000E.rotation = _0012.rotation;
					_0012 = _000E;
					_0003 = _0004;
					_0004 = false;
				}
				_0006 = Input.GetAxis("Mouse Y");
				_000A = Input.GetAxis("Mouse X");
				_0020 = _001D.TransformDirection(_000A, _0006, 0f);
				_000E.Translate(-_0020, Space.World);
			}
			goto IL_0541;
		}
	}
}
