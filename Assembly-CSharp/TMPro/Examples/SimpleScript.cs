using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class SimpleScript : MonoBehaviour
	{
		private TextMeshPro \u001D;

		private const string \u000E = "The <#0050FF>count is: </color>{0:2}";

		private float \u0012;

		private void \u0015()
		{
			this.\u001D = base.gameObject.AddComponent<TextMeshPro>();
			this.\u001D.autoSizeTextContainer = true;
			this.\u001D.fontSize = 48f;
			this.\u001D.alignment = TextAlignmentOptions.Center;
			this.\u001D.enableWordWrapping = false;
		}

		private void \u0016()
		{
			this.\u001D.SetText("The <#0050FF>count is: </color>{0:2}", this.\u0012 % 1000f);
			this.\u0012 += 1f * Time.deltaTime;
		}
	}
}
