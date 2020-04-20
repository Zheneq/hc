using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class ObjectSpin : MonoBehaviour
	{
		public float symbol_001D = 5f;

		public int symbol_000E = 0xF;

		private Transform symbol_0012;

		private float symbol_0015;

		private Vector3 symbol_0016;

		private Vector3 symbol_0013;

		private Vector3 symbol_0018;

		private Color32 symbol_0009;

		private int symbol_0019;

		public ObjectSpin.MotionType symbol_0011;

		private void symbol_001A()
		{
			this.symbol_0012 = base.transform;
			this.symbol_0013 = this.symbol_0012.rotation.eulerAngles;
			this.symbol_0018 = this.symbol_0012.position;
			Light component = base.GetComponent<Light>();
			Color c;
			if (component != null)
			{
				c = component.color;
			}
			else
			{
				c = Color.black;
			}
			this.symbol_0009 = c;
		}

		private void symbol_0004()
		{
			if (this.symbol_0011 == ObjectSpin.MotionType.symbol_001D)
			{
				this.symbol_0012.Rotate(0f, this.symbol_001D * Time.deltaTime, 0f);
			}
			else if (this.symbol_0011 == ObjectSpin.MotionType.symbol_000E)
			{
				this.symbol_0015 += this.symbol_001D * Time.deltaTime;
				this.symbol_0012.rotation = Quaternion.Euler(this.symbol_0013.x, Mathf.Sin(this.symbol_0015) * (float)this.symbol_000E + this.symbol_0013.y, this.symbol_0013.z);
			}
			else
			{
				this.symbol_0015 += this.symbol_001D * Time.deltaTime;
				float x = 15f * Mathf.Cos(this.symbol_0015 * 0.95f);
				float z = 10f;
				float y = 0f;
				this.symbol_0012.position = this.symbol_0018 + new Vector3(x, y, z);
				this.symbol_0016 = this.symbol_0012.position;
				this.symbol_0019++;
			}
		}

		public enum MotionType
		{
			symbol_001D,
			symbol_000E,
			symbol_0012
		}
	}
}
