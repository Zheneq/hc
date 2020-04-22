using TMPro;
using UnityEngine;

public class UINotchedFillBar : MonoBehaviour
{
	public RectTransform[] m_notches;

	public TextMeshProUGUI m_textCount;

	public void Setup(int filled)
	{
		for (int i = 0; i < m_notches.Length; i++)
		{
			UIManager.SetGameObjectActive(m_notches[i], i < filled);
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
			m_textCount.text = filled.ToString();
			return;
		}
	}
}
