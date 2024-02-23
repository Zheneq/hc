using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TMPro.SpriteAssetUtilities
{
	public class TexturePacker
	{
		[Serializable]
		public struct SpriteFrame
		{
			public float x;

			public float y;

			public float w;

			public float h;

			public override string ToString()
			{
				return new StringBuilder().Append("x: ").Append(x.ToString("f2")).Append(" y: ").Append(y.ToString("f2")).Append(" h: ").Append(h.ToString("f2")).Append(" w: ").Append(w.ToString("f2")).ToString();
			}
		}

		[Serializable]
		public struct SpriteSize
		{
			public float w;

			public float h;

			public override string ToString()
			{
				return new StringBuilder().Append("w: ").Append(w.ToString("f2")).Append(" h: ").Append(h.ToString("f2")).ToString();
			}
		}

		[Serializable]
		public struct SpriteData
		{
			public string filename;

			public SpriteFrame frame;

			public bool rotated;

			public bool trimmed;

			public SpriteFrame spriteSourceSize;

			public SpriteSize sourceSize;

			public Vector2 pivot;
		}

		[Serializable]
		public class SpriteDataObject
		{
			public List<SpriteData> frames;
		}
	}
}
