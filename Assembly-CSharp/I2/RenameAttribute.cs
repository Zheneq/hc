using System;
using UnityEngine;

namespace I2
{
	public class RenameAttribute : PropertyAttribute
	{
		public readonly string Name;

		public readonly string Tooltip;

		public readonly int HorizSpace;

		public RenameAttribute(int symbol_001D, string symbol_000E, string symbol_0012 = null)
		{
			this.Name = symbol_000E;
			this.Tooltip = symbol_0012;
			this.HorizSpace = symbol_001D;
		}

		public RenameAttribute(string symbol_001D, string symbol_000E = null) : this(0, symbol_001D, symbol_000E)
		{
		}
	}
}
