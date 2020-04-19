using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextMeshProFloatingText : MonoBehaviour
	{
		public Font \u001D;

		private GameObject \u000E;

		private TextMeshPro \u0012;

		private TextMesh \u0015;

		private Transform \u0016;

		private Transform \u0013;

		private Transform \u0018;

		private Vector3 \u0009 = Vector3.zero;

		private Quaternion \u0019 = Quaternion.identity;

		public int \u0011;

		private void \u001A()
		{
			this.\u0016 = base.transform;
			this.\u000E = new GameObject(base.name + " floating text");
			this.\u0018 = Camera.main.transform;
		}

		private void \u0004()
		{
			if (this.\u0011 == 0)
			{
				this.\u0012 = this.\u000E.AddComponent<TextMeshPro>();
				this.\u0012.rectTransform.sizeDelta = new Vector2(3f, 3f);
				this.\u0013 = this.\u000E.transform;
				this.\u0013.position = this.\u0016.position + new Vector3(0f, 15f, 0f);
				this.\u0012.alignment = TextAlignmentOptions.Center;
				this.\u0012.color = new Color32((byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), byte.MaxValue);
				this.\u0012.fontSize = 24f;
				this.\u0012.text = string.Empty;
				base.StartCoroutine(this.\u001A());
			}
			else if (this.\u0011 == 1)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProFloatingText.\u0004()).MethodHandle;
				}
				this.\u0013 = this.\u000E.transform;
				this.\u0013.position = this.\u0016.position + new Vector3(0f, 15f, 0f);
				this.\u0015 = this.\u000E.AddComponent<TextMesh>();
				this.\u0015.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
				this.\u0015.GetComponent<Renderer>().sharedMaterial = this.\u0015.font.material;
				this.\u0015.color = new Color32((byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), byte.MaxValue);
				this.\u0015.anchor = TextAnchor.LowerCenter;
				this.\u0015.fontSize = 0x18;
				base.StartCoroutine(this.\u0004());
			}
			else if (this.\u0011 == 2)
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
			}
		}

		public IEnumerator \u001A()
		{
			float num = 2f;
			float num2 = UnityEngine.Random.Range(5f, 20f);
			float num3 = num2;
			Vector3 position = this.\u0013.position;
			Color32 color = this.\u0012.color;
			float num4 = 255f;
			float num5 = 3f / num2 * num;
			IL_2E4:
			while (num3 > 0f)
			{
				num3 -= Time.deltaTime / num * num2;
				if (num3 <= 3f)
				{
					num4 = Mathf.Clamp(num4 - Time.deltaTime / num5 * 255f, 0f, 255f);
				}
				this.\u0012.SetText("{0}", (float)((int)num3));
				this.\u0012.color = new Color32(color.r, color.g, color.b, (byte)num4);
				this.\u0013.position += new Vector3(0f, num2 * Time.deltaTime, 0f);
				if (!this.\u0009.Compare(this.\u0018.position, 0x3E8))
				{
					goto IL_21B;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProFloatingText.<DisplayTextMeshProFloatingText>c__Iterator0.MoveNext()).MethodHandle;
				}
				if (!this.\u0019.Compare(this.\u0018.rotation, 0x3E8))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_21B;
					}
				}
				IL_2BB:
				yield return new WaitForEndOfFrame();
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_2E4;
				}
				IL_21B:
				this.\u0009 = this.\u0018.position;
				this.\u0019 = this.\u0018.rotation;
				this.\u0013.rotation = this.\u0019;
				Vector3 vector = this.\u0016.position - this.\u0009;
				this.\u0016.forward = new Vector3(vector.x, 0f, vector.z);
				goto IL_2BB;
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.\u0013.position = position;
			base.StartCoroutine(this.\u001A());
			yield break;
		}

		public IEnumerator \u0004()
		{
			float num = 2f;
			float num2 = UnityEngine.Random.Range(5f, 20f);
			float num3 = num2;
			Vector3 position = this.\u0013.position;
			Color32 color = this.\u0015.color;
			float num4 = 255f;
			int num5 = 0;
			float num6 = 3f / num2 * num;
			while (num3 > 0f)
			{
				num3 -= Time.deltaTime / num * num2;
				if (num3 <= 3f)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProFloatingText.<DisplayTextMeshFloatingText>c__Iterator1.MoveNext()).MethodHandle;
					}
					num4 = Mathf.Clamp(num4 - Time.deltaTime / num6 * 255f, 0f, 255f);
				}
				num5 = (int)num3;
				this.\u0015.text = num5.ToString();
				this.\u0015.color = new Color32(color.r, color.g, color.b, (byte)num4);
				this.\u0013.position += new Vector3(0f, num2 * Time.deltaTime, 0f);
				if (!this.\u0009.Compare(this.\u0018.position, 0x3E8) || !this.\u0019.Compare(this.\u0018.rotation, 0x3E8))
				{
					this.\u0009 = this.\u0018.position;
					this.\u0019 = this.\u0018.rotation;
					this.\u0013.rotation = this.\u0019;
					Vector3 vector = this.\u0016.position - this.\u0009;
					this.\u0016.forward = new Vector3(vector.x, 0f, vector.z);
				}
				yield return new WaitForEndOfFrame();
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
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.\u0013.position = position;
			base.StartCoroutine(this.\u0004());
			yield break;
		}
	}
}
