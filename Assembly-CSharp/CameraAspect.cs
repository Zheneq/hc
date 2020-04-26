using UnityEngine;

public class CameraAspect : MonoBehaviour
{
	[Tooltip("Width and Height are percentage of the screen that the camera box will take up. Both are percentage of current screen height.")]
	public float m_width = 0.17f;

	[Tooltip("Width and Height are percentage of the screen that the camera box will take up. Both are percentage of current screen height.")]
	public float m_height = 0.17f;

	private float Aspect
	{
		get
		{
			float result;
			if (m_height == 0f)
			{
				result = 1f;
			}
			else
			{
				result = m_width / m_height;
			}
			return result;
		}
	}

	private void Start()
	{
		SetAspect();
	}

	private void Update()
	{
		SetAspect();
	}

	private void SetAspect()
	{
		float num = (float)Screen.width * m_width;
		float width = num / (float)Screen.width;
		GetComponent<Camera>().rect = new Rect(GetComponent<Camera>().rect.x, GetComponent<Camera>().rect.y, width, m_height);
	}
}
