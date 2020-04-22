using UnityEngine;

namespace TMPro.Examples
{
	public class TMP_ExampleScript_01 : MonoBehaviour
	{
		public enum objectType
		{
			_001D,
			_000E
		}

		public objectType _001D;

		public bool _000E;

		private TMP_Text _0012;

		private const string _0015 = "The count is <#0080ff>{0}</color>";

		private int _0016;

		private void _0013()
		{
			if (_001D == objectType._001D)
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
				_0012 = (GetComponent<TextMeshPro>() ?? base.gameObject.AddComponent<TextMeshPro>());
			}
			else
			{
				TextMeshProUGUI textMeshProUGUI = GetComponent<TextMeshProUGUI>();
				if ((object)textMeshProUGUI == null)
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
					textMeshProUGUI = base.gameObject.AddComponent<TextMeshProUGUI>();
				}
				_0012 = textMeshProUGUI;
			}
			_0012.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");
			_0012.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");
			_0012.fontSize = 120f;
			_0012.text = "A <#0080ff>simple</color> line of text.";
			Vector2 preferredValues = _0012.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
			_0012.rectTransform.sizeDelta = new Vector2(preferredValues.x, preferredValues.y);
		}

		private void _0018()
		{
			if (_000E)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				_0012.SetText("The count is <#0080ff>{0}</color>", _0016 % 1000);
				_0016++;
				return;
			}
		}
	}
}
