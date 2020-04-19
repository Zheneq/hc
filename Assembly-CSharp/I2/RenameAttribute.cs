using System;
using UnityEngine;

namespace I2
{
	public class RenameAttribute : PropertyAttribute
	{
		public readonly string Name;

		public readonly string Tooltip;

		public readonly int HorizSpace;

		public RenameAttribute(int \u001D, string \u000E, string \u0012 = null)
		{
			this.Name = \u000E;
			this.Tooltip = \u0012;
			this.HorizSpace = \u001D;
		}

		public RenameAttribute(string \u001D, string \u000E = null) : this(0, \u001D, \u000E)
		{
		}
	}
}
