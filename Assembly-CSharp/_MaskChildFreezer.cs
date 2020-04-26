using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Mask))]
public class _MaskChildFreezer : MonoBehaviour
{
	private Vector3[] m_frozenPosition;

	private Vector3[] m_frozenScale;

	private Vector2 m_previousAnchorPosition;

	private Vector3 m_previousScalePosition;

	private RectTransform maskRectTransform;

	public RectTransform m_originalMaskTransform;

	public RectTransform[] m_frozenChildren;

	private void Awake()
	{
		m_frozenPosition = new Vector3[m_frozenChildren.Length];
		m_frozenScale = new Vector3[m_frozenChildren.Length];
		maskRectTransform = (base.gameObject.transform as RectTransform);
		m_originalMaskTransform.eulerAngles = maskRectTransform.eulerAngles;
		m_originalMaskTransform.position = maskRectTransform.position;
		m_originalMaskTransform.localScale = maskRectTransform.localScale;
		m_originalMaskTransform.anchoredPosition3D = maskRectTransform.anchoredPosition3D;
		m_originalMaskTransform.anchorMax = maskRectTransform.anchorMax;
		m_originalMaskTransform.anchorMin = maskRectTransform.anchorMin;
		m_originalMaskTransform.pivot = maskRectTransform.pivot;
		m_originalMaskTransform.sizeDelta = maskRectTransform.sizeDelta;
		for (int i = 0; i < m_frozenChildren.Length; i++)
		{
			m_frozenPosition[i] = m_frozenChildren[i].GetComponent<RectTransform>().anchoredPosition;
			m_frozenScale[i] = m_frozenChildren[i].GetComponent<RectTransform>().localScale;
		}
		while (true)
		{
			return;
		}
	}

	private void LateUpdate()
	{
		if (m_previousAnchorPosition != maskRectTransform.anchoredPosition)
		{
			m_previousAnchorPosition = maskRectTransform.anchoredPosition;
			for (int i = 0; i < m_frozenPosition.Length; i++)
			{
				m_frozenChildren[i].SetParent(m_originalMaskTransform);
				m_frozenChildren[i].anchoredPosition = m_frozenPosition[i];
				m_frozenChildren[i].SetParent(maskRectTransform);
			}
		}
		if (!(m_previousScalePosition != maskRectTransform.localScale))
		{
			return;
		}
		while (true)
		{
			m_previousScalePosition = maskRectTransform.localScale;
			for (int j = 0; j < m_frozenScale.Length; j++)
			{
				m_frozenChildren[j].SetParent(m_originalMaskTransform);
				m_frozenChildren[j].localScale = m_frozenScale[j];
				m_frozenChildren[j].anchoredPosition = m_frozenPosition[j];
				m_frozenChildren[j].SetParent(maskRectTransform);
			}
			return;
		}
	}
}
