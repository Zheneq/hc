using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextMeshSpawner : MonoBehaviour
	{
		public int \u001D;

		public int \u000E = 0xC;

		public Font \u0012;

		private TextMeshProFloatingText \u0015;

		private void \u0016()
		{
		}

		private void \u0013()
		{
			for (int i = 0; i < this.\u000E; i++)
			{
				if (this.\u001D == 0)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshSpawner.\u0013()).MethodHandle;
					}
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "!";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					this.\u0015 = gameObject.AddComponent<TextMeshProFloatingText>();
					this.\u0015.\u0011 = 0;
				}
				else
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = this.\u0012.material;
					textMesh.font = this.\u0012;
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 0x60;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "!";
					this.\u0015 = gameObject2.AddComponent<TextMeshProFloatingText>();
					this.\u0015.\u0011 = 1;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}
}
