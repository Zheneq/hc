using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoDialogPanel : MonoBehaviour
{
	public enum Pivot
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}

	public class InfoDialogEntry
	{
		public bool m_useLine;

		public bool m_translate;

		public bool m_uiPts;

		public Vector3 m_originPt;

		public Pivot m_pivot;

		public float m_lineLength;

		public string m_infoTxt;

		public GameObject m_infoDialogObject;

		public GameObject m_lineObject;

		public GameObject m_dialogObject;

		public InfoDialogEntry(Vector3 originPt, Pivot pivot, float lineLength, string infoTxt, bool useLine, bool translate, bool uiPts, GameObject lineObject, GameObject dialogObject, GameObject infoDialogObject)
		{
			m_originPt = originPt;
			m_infoTxt = infoTxt;
			m_uiPts = uiPts;
			m_useLine = useLine;
			m_translate = translate;
			m_pivot = pivot;
			m_lineLength = lineLength;
			m_lineObject = lineObject;
			m_dialogObject = dialogObject;
			m_infoDialogObject = infoDialogObject;
		}
	}

	public GameObject m_infoDialogPrefab;

	public GameObject m_infoLineSpritePrefab;

	public GameObject m_infoParentPrefab;

	private RectTransform myCanvasRect;

	private Dictionary<int, InfoDialogEntry> m_infoDialogEntries;

	private int m_id;

	private static UIInfoDialogPanel s_instance;

	public static UIInfoDialogPanel Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private int NextId()
	{
		m_id++;
		return m_id;
	}

	private void Start()
	{
		if (m_infoDialogEntries == null)
		{
			m_infoDialogEntries = new Dictionary<int, InfoDialogEntry>();
		}
		myCanvasRect = (GetComponentInParent<Canvas>().transform as RectTransform);
	}

	private void Update()
	{
		if (m_infoDialogEntries != null)
		{
			using (Dictionary<int, InfoDialogEntry>.ValueCollection.Enumerator enumerator = m_infoDialogEntries.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InfoDialogEntry current = enumerator.Current;
					if (current.m_useLine)
					{
						if (current.m_translate)
						{
							UIManager.SetGameObjectActive(current.m_lineObject, true);
						}
					}
					if (!current.m_uiPts)
					{
						Vector3 originPt = current.m_originPt;
						Vector2 vector = Camera.main.WorldToViewportPoint(originPt);
						float x = vector.x;
						Vector2 sizeDelta = myCanvasRect.sizeDelta;
						float num = x * sizeDelta.x;
						Vector2 sizeDelta2 = myCanvasRect.sizeDelta;
						float x2 = num - sizeDelta2.x * 0.5f;
						float y = vector.y;
						Vector2 sizeDelta3 = myCanvasRect.sizeDelta;
						float num2 = y * sizeDelta3.y;
						Vector2 sizeDelta4 = myCanvasRect.sizeDelta;
						Vector2 anchoredPosition = new Vector2(x2, num2 - sizeDelta4.y * 0.5f);
						(current.m_infoDialogObject.transform as RectTransform).anchoredPosition = anchoredPosition;
					}
				}
				while (true)
				{
					switch (7)
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

	private Vector3 GetDeltaVectorFromPivotType(Pivot pivot)
	{
		Vector3 result;
		switch (pivot)
		{
		case Pivot.Bottom:
			result = new Vector3(0f, -1f, 0f);
			break;
		case Pivot.Top:
			result = new Vector3(0f, 1f, 0f);
			break;
		case Pivot.Left:
			result = new Vector3(-1f, 0f, 0f);
			break;
		case Pivot.Right:
			result = new Vector3(1f, 0f, 0f);
			break;
		case Pivot.TopLeft:
			result = new Vector3(-1f, 1f, 0f);
			break;
		case Pivot.TopRight:
			result = new Vector3(1f, 1f, 0f);
			break;
		case Pivot.BottomLeft:
			result = new Vector3(-1f, -1f, 0f);
			break;
		case Pivot.BottomRight:
			result = new Vector3(1f, -1f, 0f);
			break;
		default:
			result = new Vector3(0f, 1f, 0f);
			break;
		}
		result.Normalize();
		return result;
	}

	public int AddInfoDialog(Vector3 originPt, Vector3 destPt, Pivot pivot, float lineLength, string infoTxt, int fontSize, bool useLine, bool translate, bool uiPts)
	{
		if (originPt == Vector3.zero)
		{
			if (!uiPts)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return -1;
					}
				}
			}
		}
		if (m_infoDialogEntries == null)
		{
			m_infoDialogEntries = new Dictionary<int, InfoDialogEntry>();
			myCanvasRect = (GetComponentInParent<Canvas>().transform as RectTransform);
			UIManager.SetGameObjectActive(base.gameObject, true);
		}
		if (uiPts)
		{
		}
		if (!translate)
		{
		}
		int num = NextId();
		string name = new StringBuilder().Append("Info Dialog ").Append(num).ToString();
		GameObject gameObject = UnityEngine.Object.Instantiate(m_infoParentPrefab);
		gameObject.name = name;
		gameObject.transform.SetParent(base.transform);
		GameObject gameObject2 = UnityEngine.Object.Instantiate(m_infoLineSpritePrefab);
		gameObject2.transform.SetParent(gameObject.transform);
		if (useLine)
		{
			UIManager.SetGameObjectActive(gameObject2, !translate);
		}
		else
		{
			UIManager.SetGameObjectActive(gameObject2, false);
		}
		GameObject gameObject3 = UnityEngine.Object.Instantiate(m_infoDialogPrefab);
		Text componentInChildren = gameObject3.GetComponentInChildren<Text>();
		componentInChildren.text = infoTxt;
		componentInChildren.fontSize = fontSize;
		(gameObject3.transform as RectTransform).sizeDelta = new Vector2((gameObject3.transform as RectTransform).rect.width, gameObject3.GetComponentInChildren<Text>().preferredHeight);
		gameObject3.transform.SetParent(gameObject.transform);
		Vector3 deltaVectorFromPivotType = GetDeltaVectorFromPivotType(pivot);
		Vector2 sizeDelta = (gameObject2.transform as RectTransform).sizeDelta;
		sizeDelta.x = lineLength;
		float num2 = Mathf.Atan2(deltaVectorFromPivotType.y, deltaVectorFromPivotType.x) * 57.29578f;
		(gameObject2.transform as RectTransform).sizeDelta = sizeDelta;
		(gameObject2.transform as RectTransform).localEulerAngles = new Vector3(0f, 0f, num2);
		gameObject2.transform.SetAsLastSibling();
		float width = (gameObject3.transform as RectTransform).rect.width;
		float height = (gameObject3.transform as RectTransform).rect.height;
		num2 *= (float)Math.PI / 180f;
		float num3 = 0f;
		if (pivot != Pivot.Top)
		{
			if (pivot != Pivot.Bottom)
			{
				if (pivot != Pivot.Right)
				{
					if (pivot != Pivot.Left)
					{
						num3 = Mathf.Abs(Mathf.Min(width, height) * 0.5f / Mathf.Cos(num2));
						goto IL_02d9;
					}
				}
				num3 = width * 0.5f;
				goto IL_02d9;
			}
		}
		num3 = height * 0.5f;
		goto IL_02d9;
		IL_02d9:
		(gameObject3.transform as RectTransform).anchoredPosition = (gameObject3.transform as RectTransform).anchoredPosition + new Vector2(deltaVectorFromPivotType.x, deltaVectorFromPivotType.y) * (lineLength + num3);
		m_infoDialogEntries[num] = new InfoDialogEntry(originPt, pivot, lineLength, infoTxt, useLine, translate, uiPts, gameObject2.gameObject, gameObject3.gameObject, gameObject.gameObject);
		return num;
	}

	public void RemoveInfoDialog(int index)
	{
		UnityEngine.Object.Destroy(m_infoDialogEntries[index].m_infoDialogObject);
		m_infoDialogEntries.Remove(index);
	}
}
