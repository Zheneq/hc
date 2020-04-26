using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class _CanvasLayerSorter : MonoBehaviour
{
	public CanvasLayerName m_layerName;

	private Canvas m_theCanvas;

	public Canvas GetCanvas()
	{
		return m_theCanvas;
	}

	private void Awake()
	{
		m_theCanvas = GetComponent<Canvas>();
		CanvasLayerManager.Get().AddCanvas(this);
	}

	private void OnEnable()
	{
		DoCanvasRefresh();
	}

	public void DoCanvasRefresh()
	{
		if (!(m_theCanvas != null))
		{
			return;
		}
		while (true)
		{
			m_theCanvas.overrideSorting = false;
			m_theCanvas.overrideSorting = true;
			return;
		}
	}
}
