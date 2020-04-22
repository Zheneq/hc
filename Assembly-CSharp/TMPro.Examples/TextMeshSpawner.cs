using UnityEngine;

namespace TMPro.Examples
{
	public class TextMeshSpawner : MonoBehaviour
	{
		public int _001D;

		public int _000E = 12;

		public Font _0012;

		private TextMeshProFloatingText _0015;

		private void _0016()
		{
		}

		private void _0013()
		{
			for (int i = 0; i < _000E; i++)
			{
				if (_001D == 0)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(Random.Range(-95f, 95f), 0.5f, Random.Range(-95f, 95f));
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "!";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					_0015 = gameObject.AddComponent<TextMeshProFloatingText>();
					_0015._0011 = 0;
				}
				else
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(Random.Range(-95f, 95f), 0.5f, Random.Range(-95f, 95f));
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = _0012.material;
					textMesh.font = _0012;
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "!";
					_0015 = gameObject2.AddComponent<TextMeshProFloatingText>();
					_0015._0011 = 1;
				}
			}
			while (true)
			{
				switch (5)
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
