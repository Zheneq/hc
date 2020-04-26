using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextMeshProFloatingText : MonoBehaviour
	{
		public Font _001D;

		private GameObject _000E;

		private TextMeshPro _0012;

		private TextMesh _0015;

		private Transform _0016;

		private Transform _0013;

		private Transform _0018;

		private Vector3 _0009 = Vector3.zero;

		private Quaternion _0019 = Quaternion.identity;

		public int _0011;

		private void _001A()
		{
			_0016 = base.transform;
			_000E = new GameObject(base.name + " floating text");
			_0018 = Camera.main.transform;
		}

		private void _0004()
		{
			if (_0011 == 0)
			{
				_0012 = _000E.AddComponent<TextMeshPro>();
				_0012.rectTransform.sizeDelta = new Vector2(3f, 3f);
				_0013 = _000E.transform;
				_0013.position = _0016.position + new Vector3(0f, 15f, 0f);
				_0012.alignment = TextAlignmentOptions.Center;
				_0012.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), byte.MaxValue);
				_0012.fontSize = 24f;
				_0012.text = string.Empty;
				StartCoroutine(coroutine001A());
				return;
			}
			if (_0011 == 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						_0013 = _000E.transform;
						_0013.position = _0016.position + new Vector3(0f, 15f, 0f);
						_0015 = _000E.AddComponent<TextMesh>();
						_0015.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
						_0015.GetComponent<Renderer>().sharedMaterial = _0015.font.material;
						_0015.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), byte.MaxValue);
						_0015.anchor = TextAnchor.LowerCenter;
						_0015.fontSize = 24;
						StartCoroutine(Coroutine0004());
						return;
					}
				}
			}
			if (_0011 != 2)
			{
				return;
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

		public IEnumerator coroutine001A()
		{
			float num = 2f;
			float num2 = Random.Range(5f, 20f);
			float num3 = num2;
			Vector3 position = _0013.position;
			Color32 color = _0012.color;
			float num4 = 255f;
			float num5 = 3f / num2 * num;
			if (num3 > 0f)
			{
				num3 -= Time.deltaTime / num * num2;
				if (num3 <= 3f)
				{
					num4 = Mathf.Clamp(num4 - Time.deltaTime / num5 * 255f, 0f, 255f);
				}
				_0012.SetText("{0}", (int)num3);
				_0012.color = new Color32(color.r, color.g, color.b, (byte)num4);
				_0013.position += new Vector3(0f, num2 * Time.deltaTime, 0f);
				if (_0009.Compare(_0018.position, 1000))
				{
					if (_0019.Compare(_0018.rotation, 1000))
					{
						goto IL_02bb;
					}
				}
				_0009 = _0018.position;
				_0019 = _0018.rotation;
				_0013.rotation = _0019;
				Vector3 vector = _0016.position - _0009;
				_0016.forward = new Vector3(vector.x, 0f, vector.z);
				goto IL_02bb;
			}
			yield return new WaitForSeconds(Random.Range(0.1f, 1f));
			/*Error: Unable to find new state assignment for yield return*/;
			IL_02bb:
			yield return new WaitForEndOfFrame();
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public IEnumerator Coroutine0004()
		{
			float num = 2f;
			float num2 = Random.Range(5f, 20f);
			float num3 = num2;
			Vector3 position = _0013.position;
			Color32 color = _0015.color;
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
				_0015.text = ((int)num3).ToString();
				_0015.color = new Color32(color.r, color.g, color.b, (byte)num4);
				_0013.position += new Vector3(0f, num2 * Time.deltaTime, 0f);
				if (!_0009.Compare(_0018.position, 1000) || !_0019.Compare(_0018.rotation, 1000))
				{
					_0009 = _0018.position;
					_0019 = _0018.rotation;
					_0013.rotation = _0019;
					Vector3 vector = _0016.position - _0009;
					_0016.forward = new Vector3(vector.x, 0f, vector.z);
				}
				yield return new WaitForEndOfFrame();
			}
			while (true)
			{
				yield return new WaitForSeconds(Random.Range(0.1f, 1f));
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
