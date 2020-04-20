using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark02 : MonoBehaviour
	{
		public int symbol_001D;

		public int symbol_000E = 0xC;

		private TextMeshProFloatingText symbol_0012;

		private void symbol_0015()
		{
			for (int i = 0; i < this.symbol_000E; i++)
			{
				if (this.symbol_001D == 0)
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.25f, UnityEngine.Random.Range(-95f, 95f));
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.autoSizeTextContainer = true;
					textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0f);
					textMeshPro.alignment = TextAlignmentOptions.Bottom;
					textMeshPro.fontSize = 96f;
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMeshPro.text = "!";
					this.symbol_0012 = gameObject.AddComponent<TextMeshProFloatingText>();
					this.symbol_0012.symbol_0011 = 0;
				}
				else if (this.symbol_001D == 1)
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.25f, UnityEngine.Random.Range(-95f, 95f));
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
					textMesh.GetComponent<Renderer>().sharedMaterial = textMesh.font.material;
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 0x60;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "!";
					this.symbol_0012 = gameObject2.AddComponent<TextMeshProFloatingText>();
					this.symbol_0012.symbol_0011 = 1;
				}
				else if (this.symbol_001D == 2)
				{
					GameObject gameObject3 = new GameObject();
					Canvas canvas = gameObject3.AddComponent<Canvas>();
					canvas.worldCamera = Camera.main;
					gameObject3.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					gameObject3.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 5f, UnityEngine.Random.Range(-95f, 95f));
					TextMeshProUGUI textMeshProUGUI = new GameObject().AddComponent<TextMeshProUGUI>();
					textMeshProUGUI.rectTransform.SetParent(gameObject3.transform, false);
					textMeshProUGUI.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMeshProUGUI.alignment = TextAlignmentOptions.Bottom;
					textMeshProUGUI.fontSize = 96f;
					textMeshProUGUI.text = "!";
					this.symbol_0012 = gameObject3.AddComponent<TextMeshProFloatingText>();
					this.symbol_0012.symbol_0011 = 0;
				}
			}
		}
	}
}
