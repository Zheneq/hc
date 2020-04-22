using UnityEngine;

namespace TMPro.Examples
{
	public class SimpleScript : MonoBehaviour
	{
		private TextMeshPro _001D;

		private const string _000E = "The <#0050FF>count is: </color>{0:2}";

		private float _0012;

		private void _0015()
		{
			_001D = base.gameObject.AddComponent<TextMeshPro>();
			_001D.autoSizeTextContainer = true;
			_001D.fontSize = 48f;
			_001D.alignment = TextAlignmentOptions.Center;
			_001D.enableWordWrapping = false;
		}

		private void _0016()
		{
			_001D.SetText("The <#0050FF>count is: </color>{0:2}", _0012 % 1000f);
			_0012 += 1f * Time.deltaTime;
		}
	}
}
