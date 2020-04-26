using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples
{
	public class TMP_TextSelector_A : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		private TextMeshPro _001D;

		private Camera _000E;

		private bool _0012;

		private int _0015 = -1;

		private int _0016 = -1;

		private int _0013 = -1;

		private void _0018()
		{
			_001D = base.gameObject.GetComponent<TextMeshPro>();
			_000E = Camera.main;
			_001D.ForceMeshUpdate();
		}

		private void _0009()
		{
			_0012 = false;
			if (TMP_TextUtilities.IsIntersectingRectTransform(_001D.rectTransform, Input.mousePosition, Camera.main))
			{
				_0012 = true;
			}
			if (!_0012)
			{
				return;
			}
			int num = TMP_TextUtilities.FindIntersectingCharacter(_001D, Input.mousePosition, Camera.main, true);
			if (num != -1)
			{
				if (num != _0016)
				{
					if (!Input.GetKey(KeyCode.LeftShift))
					{
						if (!Input.GetKey(KeyCode.RightShift))
						{
							goto IL_01b9;
						}
					}
					_0016 = num;
					int materialReferenceIndex = _001D.textInfo.characterInfo[num].materialReferenceIndex;
					int vertexIndex = _001D.textInfo.characterInfo[num].vertexIndex;
					Color32 color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), byte.MaxValue);
					Color32[] colors = _001D.textInfo.meshInfo[materialReferenceIndex].colors32;
					colors[vertexIndex] = color;
					colors[vertexIndex + 1] = color;
					colors[vertexIndex + 2] = color;
					colors[vertexIndex + 3] = color;
					_001D.textInfo.meshInfo[materialReferenceIndex].mesh.colors32 = colors;
				}
			}
			goto IL_01b9;
			IL_01b9:
			int num2 = TMP_TextUtilities.FindIntersectingLink(_001D, Input.mousePosition, _000E);
			if (num2 != -1 || _0015 == -1)
			{
				if (num2 == _0015)
				{
					goto IL_01fe;
				}
			}
			_0015 = -1;
			goto IL_01fe;
			IL_01fe:
			if (num2 != -1)
			{
				if (num2 != _0015)
				{
					_0015 = num2;
					TMP_LinkInfo tMP_LinkInfo = _001D.textInfo.linkInfo[num2];
					Debug.Log("Link ID: \"" + tMP_LinkInfo.GetLinkID() + "\"   Link Text: \"" + tMP_LinkInfo.GetLinkText() + "\"");
					Vector3 worldPoint = Vector3.zero;
					RectTransformUtility.ScreenPointToWorldPointInRectangle(_001D.rectTransform, Input.mousePosition, _000E, out worldPoint);
					string linkID = tMP_LinkInfo.GetLinkID();
					if (linkID != null)
					{
						if (!(linkID == "id_01"))
						{
							if (linkID == "id_02")
							{
							}
						}
					}
				}
			}
			int num3 = TMP_TextUtilities.FindIntersectingWord(_001D, Input.mousePosition, Camera.main);
			if (num3 == -1 || num3 == _0013)
			{
				return;
			}
			_0013 = num3;
			TMP_WordInfo tMP_WordInfo = _001D.textInfo.wordInfo[num3];
			Vector3 position = _001D.transform.TransformPoint(_001D.textInfo.characterInfo[tMP_WordInfo.firstCharacterIndex].bottomLeft);
			position = Camera.main.WorldToScreenPoint(position);
			Color32[] colors2 = _001D.textInfo.meshInfo[0].colors32;
			Color32 color2 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), byte.MaxValue);
			for (int i = 0; i < tMP_WordInfo.characterCount; i++)
			{
				int vertexIndex2 = _001D.textInfo.characterInfo[tMP_WordInfo.firstCharacterIndex + i].vertexIndex;
				colors2[vertexIndex2] = color2;
				colors2[vertexIndex2 + 1] = color2;
				colors2[vertexIndex2 + 2] = color2;
				colors2[vertexIndex2 + 3] = color2;
			}
			while (true)
			{
				_001D.mesh.colors32 = colors2;
				return;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Debug.Log("OnPointerEnter()");
			_0012 = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Debug.Log("OnPointerExit()");
			_0012 = false;
		}
	}
}
