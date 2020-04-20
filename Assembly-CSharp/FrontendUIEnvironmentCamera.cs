using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class FrontendUIEnvironmentCamera : MonoBehaviour
{
	public Color m_ambientColor = new Color(0.1111111f, 0.1111111f, 0.1111111f);

	public Cubemap m_customReflectionCubemap;

	private GameObject m_audioListener;

	private void Start()
	{
		if (AudioListenerController.Get() != null)
		{
			this.m_audioListener = AudioListenerController.Get().gameObject;
		}
		this.SetRenderSettings();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	private void Update()
	{
		if (this.m_audioListener != null)
		{
			this.m_audioListener.transform.position = base.transform.position;
		}
		this.UpdateDebugControls();
	}

	private void SetRenderSettings()
	{
		RenderSettings.ambientIntensity = 1f;
		RenderSettings.ambientLight = this.m_ambientColor;
		RenderSettings.ambientMode = AmbientMode.Flat;
		RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
		RenderSettings.customReflection = this.m_customReflectionCubemap;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.SetRenderSettings();
	}

	private void UpdateDebugControls()
	{
		if (DebugParameters.Get() != null)
		{
			if (DebugParameters.Get().GetParameterAsBool("DebugCamera"))
			{
				float d = 5f;
				float d2 = 0.1f;
				float num = 5f;
				float num2 = 5f;
				if (Input.GetMouseButton(2))
				{
					float x = base.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * num;
					float y = base.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * num2;
					base.transform.localEulerAngles = new Vector3(x, y, 0f);
				}
				Vector3 vector = Vector3.zero;
				if (!UIUtils.InputFieldHasFocus())
				{
					if (AccountPreferences.DoesApplicationHaveFocus())
					{
						if (Input.GetKey(KeyCode.W))
						{
							vector += base.transform.forward * d2;
						}
						if (Input.GetKey(KeyCode.A))
						{
							vector -= base.transform.right * d2;
						}
						if (Input.GetKey(KeyCode.S))
						{
							vector -= base.transform.forward * d2;
						}
						if (Input.GetKey(KeyCode.D))
						{
							vector += base.transform.right * d2;
						}
						if (Input.GetKey(KeyCode.R))
						{
							vector += base.transform.up * d2;
						}
						if (Input.GetKey(KeyCode.F))
						{
							vector -= base.transform.up * d2;
						}
						if (Input.GetKey(KeyCode.LeftShift))
						{
							vector *= d;
						}
					}
				}
				base.transform.position += vector;
			}
		}
	}
}
