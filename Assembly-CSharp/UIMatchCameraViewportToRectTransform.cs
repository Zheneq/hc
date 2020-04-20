using System;
using UnityEngine;

public class UIMatchCameraViewportToRectTransform : MonoBehaviour
{
	public Transform m_3dModelContainer;

	public float m_3dModelZDistanceInvariant;

	public RectTransform m_3DModelDisplayBox;

	private Camera myCamera;

	private Camera canvasCamera;

	private float viewportWidth;

	private float viewportHeight;

	private void Start()
	{
		if (!(this.m_3dModelContainer == null))
		{
			if (!(this.m_3DModelDisplayBox == null))
			{
				this.myCamera = base.GetComponent<Camera>();
				this.canvasCamera = this.m_3DModelDisplayBox.GetComponentInParent<Canvas>().worldCamera;
				return;
			}
		}
		throw new MissingReferenceException("3d model container and display boxes MUST be set");
	}

	private void Update()
	{
		Vector3[] array = new Vector3[4];
		this.m_3DModelDisplayBox.GetWorldCorners(array);
		Vector3 vector = this.canvasCamera.WorldToViewportPoint(array[1]);
		Vector3 vector2 = this.canvasCamera.WorldToViewportPoint(array[3]);
		float num = vector2.x - vector.x;
		float num2 = vector.y - vector2.y;
		if (num == this.viewportWidth)
		{
			if (num2 == this.viewportHeight)
			{
				return;
			}
		}
		this.viewportWidth = num;
		this.viewportHeight = num2;
		this.myCamera.rect = new Rect(vector.x, vector2.y, num, num2);
		float z = this.m_3dModelZDistanceInvariant / Mathf.Min(num, num2);
		this.m_3dModelContainer.localPosition = new Vector3(this.m_3dModelContainer.localPosition.x, this.m_3dModelContainer.localPosition.y, z);
	}
}
