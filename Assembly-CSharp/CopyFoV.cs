using System;
using UnityEngine;

public class CopyFoV : MonoBehaviour
{
	public Camera m_fovSource;

	private Camera m_thisCamera;

	private void Start()
	{
		this.m_thisCamera = base.GetComponent<Camera>();
	}

	private void Update()
	{
		this.m_thisCamera.fieldOfView = this.m_fovSource.fieldOfView;
	}

	private void LateUpdate()
	{
		this.m_thisCamera.fieldOfView = this.m_fovSource.fieldOfView;
	}
}
