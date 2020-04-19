using System;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class _CanvasLayerSorter : MonoBehaviour
{
	public CanvasLayerName m_layerName;

	private Canvas m_theCanvas;

	public Canvas GetCanvas()
	{
		return this.m_theCanvas;
	}

	private void Awake()
	{
		this.m_theCanvas = base.GetComponent<Canvas>();
		CanvasLayerManager.Get().AddCanvas(this);
	}

	private void OnEnable()
	{
		this.DoCanvasRefresh();
	}

	public void DoCanvasRefresh()
	{
		if (this.m_theCanvas != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_CanvasLayerSorter.DoCanvasRefresh()).MethodHandle;
			}
			this.m_theCanvas.overrideSorting = false;
			this.m_theCanvas.overrideSorting = true;
		}
	}
}
