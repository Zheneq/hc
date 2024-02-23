using System.Collections;
using System.Text;
using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark01 : MonoBehaviour
	{
		public int _001D;

		public TMP_FontAsset _000E;

		public Font _0012;

		private TextMeshPro _0015;

		private TextContainer _0016;

		private TextMesh _0013;

		private const string _0018 = "The <#0050FF>count is: </color>{0}";

		private const string _0009 = "The <color=#0050FF>count is: </color>";

		private Material _0019;

		private Material _0011;

		private IEnumerator _001A()
		{
			if (_001D == 0)
			{
				_0015 = base.gameObject.AddComponent<TextMeshPro>();
				_0015.autoSizeTextContainer = true;
				if (_000E != null)
				{
					_0015.font = _000E;
				}
				_0015.fontSize = 48f;
				_0015.alignment = TextAlignmentOptions.Center;
				_0015.extraPadding = true;
				_0015.enableWordWrapping = false;
				_0019 = _0015.font.material;
				_0011 = (Resources.Load("Fonts & Materials/LiberationSans SDF - Drop Shadow", typeof(Material)) as Material);
			}
			else if (_001D == 1)
			{
				_0013 = base.gameObject.AddComponent<TextMesh>();
				if (_0012 != null)
				{
					_0013.font = _0012;
					_0013.GetComponent<Renderer>().sharedMaterial = _0013.font.material;
				}
				else
				{
					_0013.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
					_0013.GetComponent<Renderer>().sharedMaterial = _0013.font.material;
				}
				_0013.fontSize = 48;
				_0013.anchor = TextAnchor.MiddleCenter;
			}
			int num = 0;
			if (num <= 1000000)
			{
				if (_001D == 0)
				{
					_0015.SetText("The <#0050FF>count is: </color>{0}", num % 1000);
					if (num % 1000 == 999)
					{
						TextMeshPro textMeshPro = _0015;
						Material fontSharedMaterial;
						if (_0015.fontSharedMaterial == _0019)
						{
							Material material = _0011;
							_0015.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						else
						{
							Material material = _0019;
							_0015.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						textMeshPro.fontSharedMaterial = fontSharedMaterial;
					}
				}
				else if (_001D == 1)
				{
					_0013.text = new StringBuilder().Append("The <color=#0050FF>count is: </color>").Append(num % 1000).ToString();
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
