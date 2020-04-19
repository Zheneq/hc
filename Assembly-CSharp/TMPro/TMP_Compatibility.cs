using System;

namespace TMPro
{
	public static class TMP_Compatibility
	{
		public static TextAlignmentOptions ConvertTextAlignmentEnumValues(TextAlignmentOptions oldValue)
		{
			switch (oldValue)
			{
			case (TextAlignmentOptions)0:
				return TextAlignmentOptions.TopLeft;
			case (TextAlignmentOptions)1:
				return TextAlignmentOptions.Top;
			case (TextAlignmentOptions)2:
				return TextAlignmentOptions.TopRight;
			case (TextAlignmentOptions)3:
				return TextAlignmentOptions.TopJustified;
			case (TextAlignmentOptions)4:
				return TextAlignmentOptions.Left;
			case (TextAlignmentOptions)5:
				return TextAlignmentOptions.Center;
			case (TextAlignmentOptions)6:
				return TextAlignmentOptions.Right;
			case (TextAlignmentOptions)7:
				return TextAlignmentOptions.Justified;
			case (TextAlignmentOptions)8:
				return TextAlignmentOptions.BottomLeft;
			case (TextAlignmentOptions)9:
				return TextAlignmentOptions.Bottom;
			case (TextAlignmentOptions)0xA:
				return TextAlignmentOptions.BottomRight;
			case (TextAlignmentOptions)0xB:
				return TextAlignmentOptions.BottomJustified;
			case (TextAlignmentOptions)0xC:
				return TextAlignmentOptions.BaselineLeft;
			case (TextAlignmentOptions)0xD:
				return TextAlignmentOptions.Baseline;
			case (TextAlignmentOptions)0xE:
				return TextAlignmentOptions.BaselineRight;
			case (TextAlignmentOptions)0xF:
				return TextAlignmentOptions.BaselineJustified;
			case (TextAlignmentOptions)0x10:
				return TextAlignmentOptions.MidlineLeft;
			case (TextAlignmentOptions)0x11:
				return TextAlignmentOptions.Midline;
			case (TextAlignmentOptions)0x12:
				return TextAlignmentOptions.MidlineRight;
			case (TextAlignmentOptions)0x13:
				return TextAlignmentOptions.MidlineJustified;
			case (TextAlignmentOptions)0x14:
				return TextAlignmentOptions.CaplineLeft;
			case (TextAlignmentOptions)0x15:
				return TextAlignmentOptions.Capline;
			case (TextAlignmentOptions)0x16:
				return TextAlignmentOptions.CaplineRight;
			case (TextAlignmentOptions)0x17:
				return TextAlignmentOptions.CaplineJustified;
			default:
				return TextAlignmentOptions.TopLeft;
			}
		}

		public enum AnchorPositions
		{
			TopLeft,
			Top,
			TopRight,
			Left,
			Center,
			Right,
			BottomLeft,
			Bottom,
			BottomRight,
			BaseLine,
			None
		}
	}
}
