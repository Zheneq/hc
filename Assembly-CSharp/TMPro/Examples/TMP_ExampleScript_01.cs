using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class TMP_ExampleScript_01 : MonoBehaviour
	{
		public TMP_ExampleScript_01.objectType symbol_001D;

		public bool symbol_000E;

		private TMP_Text symbol_0012;

		private const string symbol_0015 = "The count is <#0080ff>{0}</color>";

		private int symbol_0016;

		private void symbol_0013()
		{
			if (this.symbol_001D == TMP_ExampleScript_01.objectType.symbol_001D)
			{
				TextMeshPro u;
				if ((u = base.GetComponent<TextMeshPro>()) == null)
				{
					u = base.gameObject.AddComponent<TextMeshPro>();
				}
				this.symbol_0012 = u;
			}
			else
			{
				TextMeshProUGUI u2;
				if ((u2 = base.GetComponent<TextMeshProUGUI>()) == null)
				{
					u2 = base.gameObject.AddComponent<TextMeshProUGUI>();
				}
				this.symbol_0012 = u2;
			}
			this.symbol_0012.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");
			this.symbol_0012.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");
			this.symbol_0012.fontSize = 120f;
			this.symbol_0012.text = "A <#0080ff>simple</color> line of text.";
			Vector2 preferredValues = this.symbol_0012.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
			this.symbol_0012.rectTransform.sizeDelta = new Vector2(preferredValues.x, preferredValues.y);
		}

		private void symbol_0018()
		{
			if (!this.symbol_000E)
			{
				this.symbol_0012.SetText("The count is <#0080ff>{0}</color>", (float)(this.symbol_0016 % 0x3E8));
				this.symbol_0016++;
			}
		}

		public enum objectType
		{
			symbol_001D,
			symbol_000E
		}
	}
}
