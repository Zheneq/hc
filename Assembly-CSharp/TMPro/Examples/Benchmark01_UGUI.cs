using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro.Examples
{
	public class Benchmark01_UGUI : MonoBehaviour
	{
		public int symbol_001D;

		public Canvas symbol_000E;

		public TMP_FontAsset symbol_0012;

		public Font symbol_0015;

		private TextMeshProUGUI symbol_0016;

		private Text symbol_0013;

		private const string symbol_0018 = "The <#0050FF>count is: </color>";

		private const string symbol_0009 = "The <color=#0050FF>count is: </color>";

		private Material symbol_0019;

		private Material symbol_0011;

		private IEnumerator symbol_001A()
		{
			if (this.symbol_001D == 0)
			{
				this.symbol_0016 = base.gameObject.AddComponent<TextMeshProUGUI>();
				if (this.symbol_0012 != null)
				{
					this.symbol_0016.font = this.symbol_0012;
				}
				this.symbol_0016.fontSize = 48f;
				this.symbol_0016.alignment = TextAlignmentOptions.Center;
				this.symbol_0016.extraPadding = true;
				this.symbol_0019 = this.symbol_0016.font.material;
				this.symbol_0011 = (Resources.Load("Fonts & Materials/LiberationSans SDF - BEVEL", typeof(Material)) as Material);
			}
			else if (this.symbol_001D == 1)
			{
				this.symbol_0013 = base.gameObject.AddComponent<Text>();
				if (this.symbol_0015 != null)
				{
					this.symbol_0013.font = this.symbol_0015;
				}
				this.symbol_0013.fontSize = 0x30;
				this.symbol_0013.alignment = TextAnchor.MiddleCenter;
			}
			for (int i = 0; i <= 0xF4240; i++)
			{
				if (this.symbol_001D == 0)
				{
					this.symbol_0016.text = "The <#0050FF>count is: </color>" + i % 0x3E8;
					if (i % 0x3E8 == 0x3E7)
					{
						TMP_Text u = this.symbol_0016;
						Material fontSharedMaterial;
						if (this.symbol_0016.fontSharedMaterial == this.symbol_0019)
						{
							Material material = this.symbol_0011;
							this.symbol_0016.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						else
						{
							Material material = this.symbol_0019;
							this.symbol_0016.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						u.fontSharedMaterial = fontSharedMaterial;
					}
				}
				else if (this.symbol_001D == 1)
				{
					this.symbol_0013.text = "The <color=#0050FF>count is: </color>" + (i % 0x3E8).ToString();
				}
				yield return null;
			}
			yield return null;
			yield break;
		}
	}
}
