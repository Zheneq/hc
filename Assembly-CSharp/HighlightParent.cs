using UnityEngine;

public class HighlightParent : MonoBehaviour
{
	private float m_oscillatingAlpha;

	private bool m_pulsing = true;

	private void Start()
	{
	}

	private void Update()
	{
		if (!m_pulsing)
		{
			return;
		}
		while (true)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num = Mathf.Sin(realtimeSinceStartup * 3f);
			m_oscillatingAlpha = 0.5f + Mathf.Clamp(num * 0.2f, -0.2f, 0.2f);
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				Color color = renderer.material.GetColor("_TintColor");
				Color value = new Color(color.r, color.g, color.b, m_oscillatingAlpha);
				renderer.material.SetColor("_TintColor", value);
			}
			while (true)
			{
				switch (3)
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
