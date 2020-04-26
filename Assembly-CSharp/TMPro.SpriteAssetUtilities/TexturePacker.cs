using System;
using System.Collections.Generic;
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
				return "x: " + x.ToString("f2") + " y: " + y.ToString("f2") + " h: " + h.ToString("f2") + " w: " + w.ToString("f2");
			}
		}

		[Serializable]
		public struct SpriteSize
		{
			public float w;

			public float h;

			public override string ToString()
			{
				return "w: " + w.ToString("f2") + " h: " + h.ToString("f2");
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
