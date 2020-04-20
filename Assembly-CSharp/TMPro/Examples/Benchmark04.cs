using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark04 : MonoBehaviour
	{
		public int symbol_001D;

		public int symbol_000E = 0xC;

		public int symbol_0012 = 0x40;

		public int symbol_0015 = 4;

		private Transform symbol_0016;

		private void symbol_0013()
		{
			this.symbol_0016 = base.transform;
			float num = 0f;
			float num2 = (float)(Screen.height / 2);
			Camera.main.orthographicSize = num2;
			float num3 = num2;
			float num4 = (float)Screen.width / (float)Screen.height;
			for (int i = this.symbol_000E; i <= this.symbol_0012; i += this.symbol_0015)
			{
				if (this.symbol_001D == 0)
				{
					GameObject gameObject = new GameObject("Text - " + i + " Pts");
					if (num > num3 * 2f)
					{
						return;
					}
					gameObject.transform.position = this.symbol_0016.position + new Vector3(num4 * -num3 * 0.975f, num3 * 0.975f - num, 0f);
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.rectTransform.pivot = new Vector2(0f, 0.5f);
					textMeshPro.enableWordWrapping = false;
					textMeshPro.extraPadding = true;
					textMeshPro.isOrthographic = true;
					textMeshPro.fontSize = (float)i;
					textMeshPro.text = i + " pts - Lorem ipsum dolor sit...";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
					num += (float)i;
				}
			}
		}
	}
}
