using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark04 : MonoBehaviour
	{
		public int \u001D;

		public int \u000E = 0xC;

		public int \u0012 = 0x40;

		public int \u0015 = 4;

		private Transform \u0016;

		private void \u0013()
		{
			this.\u0016 = base.transform;
			float num = 0f;
			float num2 = (float)(Screen.height / 2);
			Camera.main.orthographicSize = num2;
			float num3 = num2;
			float num4 = (float)Screen.width / (float)Screen.height;
			for (int i = this.\u000E; i <= this.\u0012; i += this.\u0015)
			{
				if (this.\u001D == 0)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(Benchmark04.\u0013()).MethodHandle;
					}
					GameObject gameObject = new GameObject("Text - " + i + " Pts");
					if (num > num3 * 2f)
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
						return;
					}
					gameObject.transform.position = this.\u0016.position + new Vector3(num4 * -num3 * 0.975f, num3 * 0.975f - num, 0f);
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.rectTransform.pivot = new Vector2(0f, 0.5f);
					textMeshPro.enableWordWrapping = false;
					textMeshPro.extraPadding = true;
					textMeshPro.isOrthographic = true;
					textMeshPro.fontSize = (float)i;
					textMeshPro.text = i + " pts - Lorem ipsum dolor sit...";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
					num += (float)i;
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}
}
