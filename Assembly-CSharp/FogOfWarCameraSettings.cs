using System;
using UnityEngine;

public class FogOfWarCameraSettings : MonoBehaviour
{
	public Color fogColor = new Color(0.4706f, 0.4706f, 0.4706f, 0f);

	public float minHeightOffset = -90f;

	public Camera m_postCamera;

	public Camera m_camera;

	private static FogOfWarCameraSettings s_instance;

	internal static FogOfWarCameraSettings Get()
	{
		return FogOfWarCameraSettings.s_instance;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		FogOfWarCameraSettings.s_instance = this;
	}

	private void OnDestroy()
	{
		FogOfWarCameraSettings.s_instance = null;
	}
}
