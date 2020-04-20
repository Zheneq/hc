using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextMeshProFloatingText : MonoBehaviour
	{
		public Font symbol_001D;

		private GameObject symbol_000E;

		private TextMeshPro symbol_0012;

		private TextMesh symbol_0015;

		private Transform symbol_0016;

		private Transform symbol_0013;

		private Transform symbol_0018;

		private Vector3 symbol_0009 = Vector3.zero;

		private Quaternion symbol_0019 = Quaternion.identity;

		public int symbol_0011;

		private void symbol_001A()
		{
			this.symbol_0016 = base.transform;
			this.symbol_000E = new GameObject(base.name + " floating text");
			this.symbol_0018 = Camera.main.transform;
		}

		private void symbol_0004()
		{
			if (this.symbol_0011 == 0)
			{
				this.symbol_0012 = this.symbol_000E.AddComponent<TextMeshPro>();
				this.symbol_0012.rectTransform.sizeDelta = new Vector2(3f, 3f);
				this.symbol_0013 = this.symbol_000E.transform;
				this.symbol_0013.position = this.symbol_0016.position + new Vector3(0f, 15f, 0f);
				this.symbol_0012.alignment = TextAlignmentOptions.Center;
				this.symbol_0012.color = new Color32((byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), byte.MaxValue);
				this.symbol_0012.fontSize = 24f;
				this.symbol_0012.text = string.Empty;
				base.StartCoroutine(this.coroutine001A());
			}
			else if (this.symbol_0011 == 1)
			{
				this.symbol_0013 = this.symbol_000E.transform;
				this.symbol_0013.position = this.symbol_0016.position + new Vector3(0f, 15f, 0f);
				this.symbol_0015 = this.symbol_000E.AddComponent<TextMesh>();
				this.symbol_0015.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
				this.symbol_0015.GetComponent<Renderer>().sharedMaterial = this.symbol_0015.font.material;
				this.symbol_0015.color = new Color32((byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), byte.MaxValue);
				this.symbol_0015.anchor = TextAnchor.LowerCenter;
				this.symbol_0015.fontSize = 0x18;
				base.StartCoroutine(this.Coroutine0004());
			}
			else if (this.symbol_0011 == 2)
			{
			}
		}

		public IEnumerator coroutine001A()
		{
			float num = 2f;
			float num2 = UnityEngine.Random.Range(5f, 20f);
			float num3 = num2;
			Vector3 position = this.symbol_0013.position;
			Color32 color = this.symbol_0012.color;
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
				this.symbol_0012.SetText("{0}", (float)((int)num3));
				this.symbol_0012.color = new Color32(color.r, color.g, color.b, (byte)num4);
				this.symbol_0013.position += new Vector3(0f, num2 * Time.deltaTime, 0f);
				if (!this.symbol_0009.Compare(this.symbol_0018.position, 0x3E8))
				{
					goto IL_21B;
				}
				if (!this.symbol_0019.Compare(this.symbol_0018.rotation, 0x3E8))
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
				this.symbol_0009 = this.symbol_0018.position;
				this.symbol_0019 = this.symbol_0018.rotation;
				this.symbol_0013.rotation = this.symbol_0019;
				Vector3 vector = this.symbol_0016.position - this.symbol_0009;
				this.symbol_0016.forward = new Vector3(vector.x, 0f, vector.z);
				goto IL_2BB;
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
			this.symbol_0013.position = position;
			base.StartCoroutine(this.coroutine001A());
			yield break;
		}

		public IEnumerator Coroutine0004()
		{
			float num = 2f;
			float num2 = UnityEngine.Random.Range(5f, 20f);
			float num3 = num2;
			Vector3 position = this.symbol_0013.position;
			Color32 color = this.symbol_0015.color;
			float num4 = 255f;
			int num5 = 0;
			float num6 = 3f / num2 * num;
			while (num3 > 0f)
			{
				num3 -= Time.deltaTime / num * num2;
				if (num3 <= 3f)
				{
					num4 = Mathf.Clamp(num4 - Time.deltaTime / num6 * 255f, 0f, 255f);
				}
				num5 = (int)num3;
				this.symbol_0015.text = num5.ToString();
				this.symbol_0015.color = new Color32(color.r, color.g, color.b, (byte)num4);
				this.symbol_0013.position += new Vector3(0f, num2 * Time.deltaTime, 0f);
				if (!this.symbol_0009.Compare(this.symbol_0018.position, 0x3E8) || !this.symbol_0019.Compare(this.symbol_0018.rotation, 0x3E8))
				{
					this.symbol_0009 = this.symbol_0018.position;
					this.symbol_0019 = this.symbol_0018.rotation;
					this.symbol_0013.rotation = this.symbol_0019;
					Vector3 vector = this.symbol_0016.position - this.symbol_0009;
					this.symbol_0016.forward = new Vector3(vector.x, 0f, vector.z);
				}
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
			this.symbol_0013.position = position;
			base.StartCoroutine(this.Coroutine0004());
			yield break;
		}
	}
}
