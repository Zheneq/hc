using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class SimpleScript : MonoBehaviour
	{
		private TextMeshPro symbol_001D;

		private const string symbol_000E = "The <#0050FF>count is: </color>{0:2}";

		private float symbol_0012;

		private void symbol_0015()
		{
			this.symbol_001D = base.gameObject.AddComponent<TextMeshPro>();
			this.symbol_001D.autoSizeTextContainer = true;
			this.symbol_001D.fontSize = 48f;
			this.symbol_001D.alignment = TextAlignmentOptions.Center;
			this.symbol_001D.enableWordWrapping = false;
		}

		private void symbol_0016()
		{
			this.symbol_001D.SetText("The <#0050FF>count is: </color>{0:2}", this.symbol_0012 % 1000f);
			this.symbol_0012 += 1f * Time.deltaTime;
		}
	}
}
