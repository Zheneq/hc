using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark04 : MonoBehaviour
	{
		public int _001D;

		public int _000E = 12;

		public int _0012 = 64;

		public int _0015 = 4;

		private Transform _0016;

		private void _0013()
		{
			_0016 = base.transform;
			float num = 0f;
			float num2 = Screen.height / 2;
			Camera.main.orthographicSize = num2;
			float num3 = num2;
			float num4 = (float)Screen.width / (float)Screen.height;
			for (int i = _000E; i <= _0012; i += _0015)
			{
				if (_001D != 0)
				{
					continue;
				}
				GameObject gameObject = new GameObject("Text - " + i + " Pts");
				if (num > num3 * 2f)
				{
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
				gameObject.transform.position = _0016.position + new Vector3(num4 * (0f - num3) * 0.975f, num3 * 0.975f - num, 0f);
				TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
				textMeshPro.rectTransform.pivot = new Vector2(0f, 0.5f);
				textMeshPro.enableWordWrapping = false;
				textMeshPro.extraPadding = true;
				textMeshPro.isOrthographic = true;
				textMeshPro.fontSize = i;
				textMeshPro.text = i + " pts - Lorem ipsum dolor sit...";
				textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				num += (float)i;
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
