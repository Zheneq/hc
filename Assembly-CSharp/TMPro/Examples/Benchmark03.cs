using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark03 : MonoBehaviour
	{
		public int symbol_001D;

		public int symbol_000E = 0xC;

		public Font symbol_0012;

		private void symbol_0015()
		{
		}

		private void symbol_0016()
		{
			for (int i = 0; i < this.symbol_000E; i++)
			{
				if (this.symbol_001D == 0)
				{
					TextMeshPro textMeshPro = new GameObject
					{
						transform = 
						{
							position = new Vector3(0f, 0f, 0f)
						}
					}.AddComponent<TextMeshPro>();
					textMeshPro.alignment = TextAlignmentOptions.Center;
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "@";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				}
				else
				{
					TextMesh textMesh = new GameObject
					{
						transform = 
						{
							position = new Vector3(0f, 0f, 0f)
						}
					}.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = this.symbol_0012.material;
					textMesh.font = this.symbol_0012;
					textMesh.anchor = TextAnchor.MiddleCenter;
					textMesh.fontSize = 0x60;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "@";
				}
			}
		}
	}
}
