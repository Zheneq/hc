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
				return string.Concat(new string[]
				{
					"x: ",
					this.x.ToString("f2"),
					" y: ",
					this.y.ToString("f2"),
					" h: ",
					this.h.ToString("f2"),
					" w: ",
					this.w.ToString("f2")
				});
			}
		}

		[Serializable]
		public struct SpriteSize
		{
			public float w;

			public float h;

			public override string ToString()
			{
				return "w: " + this.w.ToString("f2") + " h: " + this.h.ToString("f2");
			}
		}

		[Serializable]
		public struct SpriteData
		{
			public string filename;

			public TexturePacker.SpriteFrame frame;

			public bool rotated;

			public bool trimmed;

			public TexturePacker.SpriteFrame spriteSourceSize;

			public TexturePacker.SpriteSize sourceSize;

			public Vector2 pivot;
		}

		[Serializable]
		public class SpriteDataObject
		{
			public List<TexturePacker.SpriteData> frames;
		}
	}
}
