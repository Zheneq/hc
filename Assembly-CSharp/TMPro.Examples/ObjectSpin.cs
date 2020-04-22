using UnityEngine;

namespace TMPro.Examples
{
	public class ObjectSpin : MonoBehaviour
	{
		public enum MotionType
		{
			_001D,
			_000E,
			_0012
		}

		public float _001D = 5f;

		public int _000E = 15;

		private Transform _0012;

		private float _0015;

		private Vector3 _0016;

		private Vector3 _0013;

		private Vector3 _0018;

		private Color32 _0009;

		private int _0019;

		public MotionType _0011;

		private void _001A()
		{
			_0012 = base.transform;
			_0013 = _0012.rotation.eulerAngles;
			_0018 = _0012.position;
			Light component = GetComponent<Light>();
			Color c;
			if (component != null)
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
				c = component.color;
			}
			else
			{
				c = Color.black;
			}
			_0009 = c;
		}

		private void _0004()
		{
			if (_0011 == MotionType._001D)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						_0012.Rotate(0f, _001D * Time.deltaTime, 0f);
						return;
					}
				}
			}
			if (_0011 == MotionType._000E)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						_0015 += _001D * Time.deltaTime;
						_0012.rotation = Quaternion.Euler(_0013.x, Mathf.Sin(_0015) * (float)_000E + _0013.y, _0013.z);
						return;
					}
				}
			}
			_0015 += _001D * Time.deltaTime;
			float x = 15f * Mathf.Cos(_0015 * 0.95f);
			float z = 10f;
			float y = 0f;
			_0012.position = _0018 + new Vector3(x, y, z);
			_0016 = _0012.position;
			_0019++;
		}
	}
}
