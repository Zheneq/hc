using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark02 : MonoBehaviour
	{
		public int _001D;

		public int _000E = 12;

		private TextMeshProFloatingText _0012;

		private void _0015()
		{
			for (int i = 0; i < _000E; i++)
			{
				if (_001D == 0)
				{
					while (true)
					{
						switch (5)
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
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.autoSizeTextContainer = true;
					textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0f);
					textMeshPro.alignment = TextAlignmentOptions.Bottom;
					textMeshPro.fontSize = 96f;
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMeshPro.text = "!";
					_0012 = gameObject.AddComponent<TextMeshProFloatingText>();
					_0012._0011 = 0;
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
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
					textMesh.GetComponent<Renderer>().sharedMaterial = textMesh.font.material;
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "!";
					_0012 = gameObject2.AddComponent<TextMeshProFloatingText>();
					_0012._0011 = 1;
				}
				else if (_001D == 2)
				{
					GameObject gameObject3 = new GameObject();
					Canvas canvas = gameObject3.AddComponent<Canvas>();
					canvas.worldCamera = Camera.main;
					gameObject3.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					gameObject3.transform.position = new Vector3(Random.Range(-95f, 95f), 5f, Random.Range(-95f, 95f));
					TextMeshProUGUI textMeshProUGUI = new GameObject().AddComponent<TextMeshProUGUI>();
					textMeshProUGUI.rectTransform.SetParent(gameObject3.transform, false);
					textMeshProUGUI.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMeshProUGUI.alignment = TextAlignmentOptions.Bottom;
					textMeshProUGUI.fontSize = 96f;
					textMeshProUGUI.text = "!";
					_0012 = gameObject3.AddComponent<TextMeshProFloatingText>();
					_0012._0011 = 0;
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}
}
