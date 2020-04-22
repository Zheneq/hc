using System.Collections;
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				_0015 = base.gameObject.AddComponent<TextMeshPro>();
				_0015.autoSizeTextContainer = true;
				if (_000E != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				_0013 = base.gameObject.AddComponent<TextMesh>();
				if (_0012 != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
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
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					_0015.SetText("The <#0050FF>count is: </color>{0}", num % 1000);
					if (num % 1000 == 999)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						TextMeshPro textMeshPro = _0015;
						Material fontSharedMaterial;
						if (_0015.fontSharedMaterial == _0019)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
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
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					_0013.text = "The <color=#0050FF>count is: </color>" + num % 1000;
				}
				yield return null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				yield return null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
