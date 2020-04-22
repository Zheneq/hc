using UnityEngine;

public class CopyFoV : MonoBehaviour
{
	public Camera m_fovSource;

	private Camera m_thisCamera;

	private void Start()
	{
		m_thisCamera = GetComponent<Camera>();
	}

	private void Update()
	{
		m_thisCamera.fieldOfView = m_fovSource.fieldOfView;
	}

	private void LateUpdate()
	{
		m_thisCamera.fieldOfView = m_fovSource.fieldOfView;
	}
}
