using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark01 : MonoBehaviour
	{
		public int \u001D;

		public TMP_FontAsset \u000E;

		public Font \u0012;

		private TextMeshPro \u0015;

		private TextContainer \u0016;

		private TextMesh \u0013;

		private const string \u0018 = "The <#0050FF>count is: </color>{0}";

		private const string \u0009 = "The <color=#0050FF>count is: </color>";

		private Material \u0019;

		private Material \u0011;

		private IEnumerator \u001A()
		{
			if (this.\u001D == 0)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Benchmark01.<Start>c__Iterator0.MoveNext()).MethodHandle;
				}
				this.\u0015 = base.gameObject.AddComponent<TextMeshPro>();
				this.\u0015.autoSizeTextContainer = true;
				if (this.\u000E != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					this.\u0015.font = this.\u000E;
				}
				this.\u0015.fontSize = 48f;
				this.\u0015.alignment = TextAlignmentOptions.Center;
				this.\u0015.extraPadding = true;
				this.\u0015.enableWordWrapping = false;
				this.\u0019 = this.\u0015.font.material;
				this.\u0011 = (Resources.Load("Fonts & Materials/LiberationSans SDF - Drop Shadow", typeof(Material)) as Material);
			}
			else if (this.\u001D == 1)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				this.\u0013 = base.gameObject.AddComponent<TextMesh>();
				if (this.\u0012 != null)
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
					this.\u0013.font = this.\u0012;
					this.\u0013.GetComponent<Renderer>().sharedMaterial = this.\u0013.font.material;
				}
				else
				{
					this.\u0013.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
					this.\u0013.GetComponent<Renderer>().sharedMaterial = this.\u0013.font.material;
				}
				this.\u0013.fontSize = 0x30;
				this.\u0013.anchor = TextAnchor.MiddleCenter;
			}
			for (int i = 0; i <= 0xF4240; i++)
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
					this.\u0015.SetText("The <#0050FF>count is: </color>{0}", (float)(i % 0x3E8));
					if (i % 0x3E8 == 0x3E7)
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
						TMP_Text u = this.\u0015;
						Material fontSharedMaterial;
						if (this.\u0015.fontSharedMaterial == this.\u0019)
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
							Material material = this.\u0011;
							this.\u0015.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						else
						{
							Material material = this.\u0019;
							this.\u0015.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						u.fontSharedMaterial = fontSharedMaterial;
					}
				}
				else if (this.\u001D == 1)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					this.\u0013.text = "The <color=#0050FF>count is: </color>" + (i % 0x3E8).ToString();
				}
				yield return null;
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			yield return null;
			for (;;)
			{
				switch (4)
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
