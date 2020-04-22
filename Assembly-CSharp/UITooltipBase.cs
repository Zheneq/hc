using UnityEngine;

public abstract class UITooltipBase : MonoBehaviour
{
	public Vector2[] m_anchorPoints;

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(base.gameObject, visible);
	}
}
