using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro.Examples
{
	public class Benchmark01_UGUI : MonoBehaviour
	{
		public int \u001D;

		public Canvas \u000E;

		public TMP_FontAsset \u0012;

		public Font \u0015;

		private TextMeshProUGUI \u0016;

		private Text \u0013;

		private const string \u0018 = "The <#0050FF>count is: </color>";

		private const string \u0009 = "The <color=#0050FF>count is: </color>";

		private Material \u0019;

		private Material \u0011;

		private IEnumerator \u001A()
		{
			if (this.\u001D == 0)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Benchmark01_UGUI.<Start>c__Iterator0.MoveNext()).MethodHandle;
				}
				this.\u0016 = base.gameObject.AddComponent<TextMeshProUGUI>();
				if (this.\u0012 != null)
				{
					this.\u0016.font = this.\u0012;
				}
				this.\u0016.fontSize = 48f;
				this.\u0016.alignment = TextAlignmentOptions.Center;
				this.\u0016.extraPadding = true;
				this.\u0019 = this.\u0016.font.material;
				this.\u0011 = (Resources.Load("Fonts & Materials/LiberationSans SDF - BEVEL", typeof(Material)) as Material);
			}
			else if (this.\u001D == 1)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				this.\u0013 = base.gameObject.AddComponent<Text>();
				if (this.\u0015 != null)
				{
					this.\u0013.font = this.\u0015;
				}
				this.\u0013.fontSize = 0x30;
				this.\u0013.alignment = TextAnchor.MiddleCenter;
			}
			for (int i = 0; i <= 0xF4240; i++)
			{
				if (this.\u001D == 0)
				{
					this.\u0016.text = "The <#0050FF>count is: </color>" + i % 0x3E8;
					if (i % 0x3E8 == 0x3E7)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						TMP_Text u = this.\u0016;
						Material fontSharedMaterial;
						if (this.\u0016.fontSharedMaterial == this.\u0019)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							Material material = this.\u0011;
							this.\u0016.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						else
						{
							Material material = this.\u0019;
							this.\u0016.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						u.fontSharedMaterial = fontSharedMaterial;
					}
				}
				else if (this.\u001D == 1)
				{
					this.\u0013.text = "The <color=#0050FF>count is: </color>" + (i % 0x3E8).ToString();
				}
				yield return null;
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			yield return null;
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			yield break;
		}
	}
}
