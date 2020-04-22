using UnityEngine;

[RequireComponent(typeof(_MaskChildFreezer))]
public class _NonRectangularSlider : MonoBehaviour
{
	[Range(0f, 1f)]
	public float m_val;

	private float m_lastValSet = -1f;

	private _MaskChildFreezer m_theMaskChildFreezer;

	private void Start()
	{
		m_theMaskChildFreezer = GetComponent<_MaskChildFreezer>();
	}

	private void SetBarPercentVisual(_MaskChildFreezer mask, float percent)
	{
		m_lastValSet = percent;
		float width = (mask.transform as RectTransform).rect.width;
		float x = (percent - 1f) * width;
		(mask.transform as RectTransform).anchoredPosition = m_theMaskChildFreezer.m_originalMaskTransform.anchoredPosition + new Vector2(x, 0f);
	}

	private void Update()
	{
		if (m_val == m_lastValSet)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SetBarPercentVisual(m_theMaskChildFreezer, m_val);
			return;
		}
	}
}
