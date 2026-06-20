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
		if (!(m_3dModelContainer == null))
		{
			if (!(m_3DModelDisplayBox == null))
			{
				myCamera = GetComponent<Camera>();
				canvasCamera = m_3DModelDisplayBox.GetComponentInParent<Canvas>().worldCamera;
				return;
			}
		}
		throw new MissingReferenceException("3d model container and display boxes MUST be set");
	}

	private void Update()
	{
		Vector3[] array = new Vector3[4];
		m_3DModelDisplayBox.GetWorldCorners(array);
		Vector3 vector = canvasCamera.WorldToViewportPoint(array[1]);
		Vector3 vector2 = canvasCamera.WorldToViewportPoint(array[3]);
		float num = vector2.x - vector.x;
		float num2 = vector.y - vector2.y;
		if (num == viewportWidth)
		{
			if (num2 == viewportHeight)
			{
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		viewportWidth = num;
		viewportHeight = num2;
		myCamera.rect = new Rect(vector.x, vector2.y, num, num2);
		float z = m_3dModelZDistanceInvariant / Mathf.Min(num, num2);
		Transform _3dModelContainer = m_3dModelContainer;
		Vector3 localPosition = m_3dModelContainer.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = m_3dModelContainer.localPosition;
		_3dModelContainer.localPosition = new Vector3(x, localPosition2.y, z);
	}
}
