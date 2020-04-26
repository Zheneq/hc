using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro.Examples
{
	public class Benchmark01_UGUI : MonoBehaviour
	{
		public int _001D;

		public Canvas _000E;

		public TMP_FontAsset _0012;

		public Font _0015;

		private TextMeshProUGUI _0016;

		private Text _0013;

		private const string _0018 = "The <#0050FF>count is: </color>";

		private const string _0009 = "The <color=#0050FF>count is: </color>";

		private Material _0019;

		private Material _0011;

		private IEnumerator _001A()
		{
			if (_001D == 0)
			{
				_0016 = base.gameObject.AddComponent<TextMeshProUGUI>();
				if (_0012 != null)
				{
					_0016.font = _0012;
				}
				_0016.fontSize = 48f;
				_0016.alignment = TextAlignmentOptions.Center;
				_0016.extraPadding = true;
				_0019 = _0016.font.material;
				_0011 = (Resources.Load("Fonts & Materials/LiberationSans SDF - BEVEL", typeof(Material)) as Material);
			}
			else if (_001D == 1)
			{
				_0013 = base.gameObject.AddComponent<Text>();
				if (_0015 != null)
				{
					_0013.font = _0015;
				}
				_0013.fontSize = 48;
				_0013.alignment = TextAnchor.MiddleCenter;
			}
			int num = 0;
			if (num <= 1000000)
			{
				if (_001D == 0)
				{
					_0016.text = "The <#0050FF>count is: </color>" + num % 1000;
					if (num % 1000 == 999)
					{
						TextMeshProUGUI textMeshProUGUI = _0016;
						Material fontSharedMaterial;
						if (_0016.fontSharedMaterial == _0019)
						{
							Material material = _0011;
							_0016.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						else
						{
							Material material = _0019;
							_0016.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						textMeshProUGUI.fontSharedMaterial = fontSharedMaterial;
					}
				}
				else if (_001D == 1)
				{
					_0013.text = "The <color=#0050FF>count is: </color>" + num % 1000;
				}
				yield return null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			while (true)
			{
				yield return null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
