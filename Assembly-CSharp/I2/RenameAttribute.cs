using UnityEngine;

namespace I2
{
	public class RenameAttribute : PropertyAttribute
	{
		public readonly string Name;

		public readonly string Tooltip;

		public readonly int HorizSpace;

		public RenameAttribute(int _001D, string _000E, string _0012 = null)
		{
			Name = _000E;
			Tooltip = _0012;
			HorizSpace = _001D;
		}

		public RenameAttribute(string _001D, string _000E = null)
			: this(0, _001D, _000E)
		{
		}
	}
}
