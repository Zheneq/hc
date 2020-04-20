using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark01 : MonoBehaviour
	{
		public int symbol_001D;

		public TMP_FontAsset symbol_000E;

		public Font symbol_0012;

		private TextMeshPro symbol_0015;

		private TextContainer symbol_0016;

		private TextMesh symbol_0013;

		private const string symbol_0018 = "The <#0050FF>count is: </color>{0}";

		private const string symbol_0009 = "The <color=#0050FF>count is: </color>";

		private Material symbol_0019;

		private Material symbol_0011;

		private IEnumerator symbol_001A()
		{
			if (this.symbol_001D == 0)
			{
				this.symbol_0015 = base.gameObject.AddComponent<TextMeshPro>();
				this.symbol_0015.autoSizeTextContainer = true;
				if (this.symbol_000E != null)
				{
					this.symbol_0015.font = this.symbol_000E;
				}
				this.symbol_0015.fontSize = 48f;
				this.symbol_0015.alignment = TextAlignmentOptions.Center;
				this.symbol_0015.extraPadding = true;
				this.symbol_0015.enableWordWrapping = false;
				this.symbol_0019 = this.symbol_0015.font.material;
				this.symbol_0011 = (Resources.Load("Fonts & Materials/LiberationSans SDF - Drop Shadow", typeof(Material)) as Material);
			}
			else if (this.symbol_001D == 1)
			{
				this.symbol_0013 = base.gameObject.AddComponent<TextMesh>();
				if (this.symbol_0012 != null)
				{
					this.symbol_0013.font = this.symbol_0012;
					this.symbol_0013.GetComponent<Renderer>().sharedMaterial = this.symbol_0013.font.material;
				}
				else
				{
					this.symbol_0013.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
					this.symbol_0013.GetComponent<Renderer>().sharedMaterial = this.symbol_0013.font.material;
				}
				this.symbol_0013.fontSize = 0x30;
				this.symbol_0013.anchor = TextAnchor.MiddleCenter;
			}
			for (int i = 0; i <= 0xF4240; i++)
			{
				if (this.symbol_001D == 0)
				{
					this.symbol_0015.SetText("The <#0050FF>count is: </color>{0}", (float)(i % 0x3E8));
					if (i % 0x3E8 == 0x3E7)
					{
						TMP_Text u = this.symbol_0015;
						Material fontSharedMaterial;
						if (this.symbol_0015.fontSharedMaterial == this.symbol_0019)
						{
							Material material = this.symbol_0011;
							this.symbol_0015.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						else
						{
							Material material = this.symbol_0019;
							this.symbol_0015.fontSharedMaterial = material;
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
