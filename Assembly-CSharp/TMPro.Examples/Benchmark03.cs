using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark03 : MonoBehaviour
	{
		public int _001D;

		public int _000E = 12;

		public Font _0012;

		private void _0015()
		{
		}

		private void _0016()
		{
			for (int i = 0; i < _000E; i++)
			{
				if (_001D == 0)
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(0f, 0f, 0f);
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.alignment = TextAlignmentOptions.Center;
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "@";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				}
				else
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(0f, 0f, 0f);
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = _0012.material;
					textMesh.font = _0012;
					textMesh.anchor = TextAnchor.MiddleCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "@";
				}
			}
			while (true)
			{
				switch (3)
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
