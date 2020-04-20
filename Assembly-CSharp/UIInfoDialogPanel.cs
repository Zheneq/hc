using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoDialogPanel : MonoBehaviour
{
	public GameObject m_infoDialogPrefab;

	public GameObject m_infoLineSpritePrefab;

	public GameObject m_infoParentPrefab;

	private RectTransform myCanvasRect;

	private Dictionary<int, UIInfoDialogPanel.InfoDialogEntry> m_infoDialogEntries;

	private int m_id;

	private static UIInfoDialogPanel s_instance;

	public static UIInfoDialogPanel Get()
	{
		return UIInfoDialogPanel.s_instance;
	}

	private void Awake()
	{
		UIInfoDialogPanel.s_instance = this;
	}

	private int NextId()
	{
		this.m_id++;
		return this.m_id;
	}

	private void Start()
	{
		if (this.m_infoDialogEntries == null)
		{
			this.m_infoDialogEntries = new Dictionary<int, UIInfoDialogPanel.InfoDialogEntry>();
		}
		this.myCanvasRect = (base.GetComponentInParent<Canvas>().transform as RectTransform);
	}

	private void Update()
	{
		if (this.m_infoDialogEntries != null)
		{
			using (Dictionary<int, UIInfoDialogPanel.InfoDialogEntry>.ValueCollection.Enumerator enumerator = this.m_infoDialogEntries.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIInfoDialogPanel.InfoDialogEntry infoDialogEntry = enumerator.Current;
					if (infoDialogEntry.m_useLine)
					{
						if (infoDialogEntry.m_translate)
						{
							UIManager.SetGameObjectActive(infoDialogEntry.m_lineObject, true, null);
						}
					}
					if (!infoDialogEntry.m_uiPts)
					{
						Vector3 originPt = infoDialogEntry.m_originPt;
						Vector2 vector = Camera.main.WorldToViewportPoint(originPt);
						Vector2 anchoredPosition = new Vector2(vector.x * this.myCanvasRect.sizeDelta.x - this.myCanvasRect.sizeDelta.x * 0.5f, vector.y * this.myCanvasRect.sizeDelta.y - this.myCanvasRect.sizeDelta.y * 0.5f);
						(infoDialogEntry.m_infoDialogObject.transform as RectTransform).anchoredPosition = anchoredPosition;
					}
				}
			}
		}
	}

	private Vector3 GetDeltaVectorFromPivotType(UIInfoDialogPanel.Pivot pivot)
	{
		Vector3 result;
		switch (pivot)
		{
		case UIInfoDialogPanel.Pivot.TopLeft:
			result = new Vector3(-1f, 1f, 0f);
			goto IL_113;
		case UIInfoDialogPanel.Pivot.Top:
			result = new Vector3(0f, 1f, 0f);
			goto IL_113;
		case UIInfoDialogPanel.Pivot.TopRight:
			result = new Vector3(1f, 1f, 0f);
			goto IL_113;
		case UIInfoDialogPanel.Pivot.Left:
			result = new Vector3(-1f, 0f, 0f);
			goto IL_113;
		case UIInfoDialogPanel.Pivot.Right:
			result = new Vector3(1f, 0f, 0f);
			goto IL_113;
		case UIInfoDialogPanel.Pivot.BottomLeft:
			result = new Vector3(-1f, -1f, 0f);
			goto IL_113;
		case UIInfoDialogPanel.Pivot.Bottom:
			result = new Vector3(0f, -1f, 0f);
			goto IL_113;
		case UIInfoDialogPanel.Pivot.BottomRight:
			result = new Vector3(1f, -1f, 0f);
			goto IL_113;
		}
		result = new Vector3(0f, 1f, 0f);
		IL_113:
		result.Normalize();
		return result;
	}

	public int AddInfoDialog(Vector3 originPt, Vector3 destPt, UIInfoDialogPanel.Pivot pivot, float lineLength, string infoTxt, int fontSize, bool useLine, bool translate, bool uiPts)
	{
		if (originPt == Vector3.zero)
		{
			if (!uiPts)
			{
				return -1;
			}
		}
		if (this.m_infoDialogEntries == null)
		{
			this.m_infoDialogEntries = new Dictionary<int, UIInfoDialogPanel.InfoDialogEntry>();
			this.myCanvasRect = (base.GetComponentInParent<Canvas>().transform as RectTransform);
			UIManager.SetGameObjectActive(base.gameObject, true, null);
		}
		if (uiPts)
		{
		}
		if (!translate)
		{
		}
		int num = this.NextId();
		string name = string.Format("Info Dialog {0}", num);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_infoParentPrefab);
		gameObject.name = name;
		gameObject.transform.SetParent(base.transform);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_infoLineSpritePrefab);
		gameObject2.transform.SetParent(gameObject.transform);
		if (useLine)
		{
			UIManager.SetGameObjectActive(gameObject2, !translate, null);
		}
		else
		{
			UIManager.SetGameObjectActive(gameObject2, false, null);
		}
		GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.m_infoDialogPrefab);
		Text componentInChildren = gameObject3.GetComponentInChildren<Text>();
		componentInChildren.text = infoTxt;
		componentInChildren.fontSize = fontSize;
		(gameObject3.transform as RectTransform).sizeDelta = new Vector2((gameObject3.transform as RectTransform).rect.width, gameObject3.GetComponentInChildren<Text>().preferredHeight);
		gameObject3.transform.SetParent(gameObject.transform);
		Vector3 deltaVectorFromPivotType = this.GetDeltaVectorFromPivotType(pivot);
		Vector2 sizeDelta = (gameObject2.transform as RectTransform).sizeDelta;
		sizeDelta.x = lineLength;
		float num2 = Mathf.Atan2(deltaVectorFromPivotType.y, deltaVectorFromPivotType.x) * 57.29578f;
		(gameObject2.transform as RectTransform).sizeDelta = sizeDelta;
		(gameObject2.transform as RectTransform).localEulerAngles = new Vector3(0f, 0f, num2);
		gameObject2.transform.SetAsLastSibling();
		float width = (gameObject3.transform as RectTransform).rect.width;
		float height = (gameObject3.transform as RectTransform).rect.height;
		num2 *= 0.0174532924f;
		float num3;
		if (pivot != UIInfoDialogPanel.Pivot.Top)
		{
			if (pivot != UIInfoDialogPanel.Pivot.Bottom)
			{
				if (pivot != UIInfoDialogPanel.Pivot.Right)
				{
					if (pivot != UIInfoDialogPanel.Pivot.Left)
					{
						num3 = Mathf.Abs(Mathf.Min(width, height) * 0.5f / Mathf.Cos(num2));
						goto IL_2D9;
					}
				}
				num3 = width * 0.5f;
				goto IL_2D9;
			}
		}
		num3 = height * 0.5f;
		IL_2D9:
		(gameObject3.transform as RectTransform).anchoredPosition = (gameObject3.transform as RectTransform).anchoredPosition + new Vector2(deltaVectorFromPivotType.x, deltaVectorFromPivotType.y) * (lineLength + num3);
		this.m_infoDialogEntries[num] = new UIInfoDialogPanel.InfoDialogEntry(originPt, pivot, lineLength, infoTxt, useLine, translate, uiPts, gameObject2.gameObject, gameObject3.gameObject, gameObject.gameObject);
		return num;
	}

	public void RemoveInfoDialog(int index)
	{
		UnityEngine.Object.Destroy(this.m_infoDialogEntries[index].m_infoDialogObject);
		this.m_infoDialogEntries.Remove(index);
	}

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

		public UIInfoDialogPanel.Pivot m_pivot;

		public float m_lineLength;

		public string m_infoTxt;

		public GameObject m_infoDialogObject;

		public GameObject m_lineObject;

		public GameObject m_dialogObject;

		public InfoDialogEntry(Vector3 originPt, UIInfoDialogPanel.Pivot pivot, float lineLength, string infoTxt, bool useLine, bool translate, bool uiPts, GameObject lineObject, GameObject dialogObject, GameObject infoDialogObject)
		{
			this.m_originPt = originPt;
			this.m_infoTxt = infoTxt;
			this.m_uiPts = uiPts;
			this.m_useLine = useLine;
			this.m_translate = translate;
			this.m_pivot = pivot;
			this.m_lineLength = lineLength;
			this.m_lineObject = lineObject;
			this.m_dialogObject = dialogObject;
			this.m_infoDialogObject = infoDialogObject;
		}
	}
}
