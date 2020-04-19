using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class ObjectSpin : MonoBehaviour
	{
		public float \u001D = 5f;

		public int \u000E = 0xF;

		private Transform \u0012;

		private float \u0015;

		private Vector3 \u0016;

		private Vector3 \u0013;

		private Vector3 \u0018;

		private Color32 \u0009;

		private int \u0019;

		public ObjectSpin.MotionType \u0011;

		private void \u001A()
		{
			this.\u0012 = base.transform;
			this.\u0013 = this.\u0012.rotation.eulerAngles;
			this.\u0018 = this.\u0012.position;
			Light component = base.GetComponent<Light>();
			Color c;
			if (component != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectSpin.\u001A()).MethodHandle;
				}
				c = component.color;
			}
			else
			{
				c = Color.black;
			}
			this.\u0009 = c;
		}

		private void \u0004()
		{
			if (this.\u0011 == ObjectSpin.MotionType.\u001D)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ObjectSpin.\u0004()).MethodHandle;
				}
				this.\u0012.Rotate(0f, this.\u001D * Time.deltaTime, 0f);
			}
			else if (this.\u0011 == ObjectSpin.MotionType.\u000E)
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
				this.\u0015 += this.\u001D * Time.deltaTime;
				this.\u0012.rotation = Quaternion.Euler(this.\u0013.x, Mathf.Sin(this.\u0015) * (float)this.\u000E + this.\u0013.y, this.\u0013.z);
			}
			else
			{
				this.\u0015 += this.\u001D * Time.deltaTime;
				float x = 15f * Mathf.Cos(this.\u0015 * 0.95f);
				float z = 10f;
				float y = 0f;
				this.\u0012.position = this.\u0018 + new Vector3(x, y, z);
				this.\u0016 = this.\u0012.position;
				this.\u0019++;
			}
		}

		public enum MotionType
		{
			\u001D,
			\u000E,
			\u0012
		}
	}
}
