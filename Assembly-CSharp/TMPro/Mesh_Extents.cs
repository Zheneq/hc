using System;
using System.Text;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public struct Mesh_Extents
	{
		public Vector2 min;

		public Vector2 max;

		public Mesh_Extents(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		public override string ToString()
		{
			return new StringBuilder().Append("Min (").Append(min.x.ToString("f2")).Append(", ").Append(min.y.ToString("f2")).Append(")   Max (").Append(max.x.ToString("f2")).Append(", ").Append(max.y.ToString("f2")).Append(")").ToString();
		}
	}
}
