using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class TMP_ExampleScript_01 : MonoBehaviour
	{
		public TMP_ExampleScript_01.objectType \u001D;

		public bool \u000E;

		private TMP_Text \u0012;

		private const string \u0015 = "The count is <#0080ff>{0}</color>";

		private int \u0016;

		private void \u0013()
		{
			if (this.\u001D == TMP_ExampleScript_01.objectType.\u001D)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_ExampleScript_01.\u0013()).MethodHandle;
				}
				TextMeshPro u;
				if ((u = base.GetComponent<TextMeshPro>()) == null)
				{
					u = base.gameObject.AddComponent<TextMeshPro>();
				}
				this.\u0012 = u;
			}
			else
			{
				TextMeshProUGUI u2;
				if ((u2 = base.GetComponent<TextMeshProUGUI>()) == null)
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
					u2 = base.gameObject.AddComponent<TextMeshProUGUI>();
				}
				this.\u0012 = u2;
			}
			this.\u0012.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");
			this.\u0012.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");
			this.\u0012.fontSize = 120f;
			this.\u0012.text = "A <#0080ff>simple</color> line of text.";
			Vector2 preferredValues = this.\u0012.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
			this.\u0012.rectTransform.sizeDelta = new Vector2(preferredValues.x, preferredValues.y);
		}

		private void \u0018()
		{
			if (!this.\u000E)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_ExampleScript_01.\u0018()).MethodHandle;
				}
				this.\u0012.SetText("The count is <#0080ff>{0}</color>", (float)(this.\u0016 % 0x3E8));
				this.\u0016++;
			}
		}

		public enum objectType
		{
			\u001D,
			\u000E
		}
	}
}
