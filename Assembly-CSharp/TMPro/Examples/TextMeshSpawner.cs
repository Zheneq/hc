using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextMeshSpawner : MonoBehaviour
	{
		public int symbol_001D;

		public int symbol_000E = 0xC;

		public Font symbol_0012;

		private TextMeshProFloatingText symbol_0015;

		private void symbol_0016()
		{
		}

		private void symbol_0013()
		{
			for (int i = 0; i < this.symbol_000E; i++)
			{
				if (this.symbol_001D == 0)
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "!";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					this.symbol_0015 = gameObject.AddComponent<TextMeshProFloatingText>();
					this.symbol_0015.symbol_0011 = 0;
				}
				else
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = this.symbol_0012.material;
					textMesh.font = this.symbol_0012;
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 0x60;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "!";
					this.symbol_0015 = gameObject2.AddComponent<TextMeshProFloatingText>();
					this.symbol_0015.symbol_0011 = 1;
				}
			}
		}
	}
}
