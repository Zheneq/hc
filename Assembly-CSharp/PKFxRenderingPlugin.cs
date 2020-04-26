using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKFxRenderingPlugin : PKFxCamera
{
	private PKFxDistortionEffect m_PostFx;

	[Tooltip("Shows settings likely to impact performance.")]
	[HideInInspector]
	public bool m_ShowAdvancedSettings;

	[HideInInspector]
	[Tooltip("Loads a user-defined mesh to be used for particles world collisions.")]
	public bool m_UseSceneMesh;

	[HideInInspector]
	[Tooltip("Name of the scene mesh relative to the PackFx directory.")]
	public string m_SceneMeshPkmmPath = "Meshes/UnityScene.pkmm";

	public List<PkFxCustomShader> m_BoundShaders;

	private IEnumerator Start()
	{
		base.BaseInitialize();
		yield return base.WaitForPack(true);
		if (Application.isEditor && QualitySettings.desiredColorSpace != ColorSpace.Linear)
		{
			Debug.LogWarning("[PKFX] Current rendering not in linear space. Colors may not be accurate.\nTo properly set the color space, go to \"Player Settings\">\"Other Settings\">\"Color Space\"");
		}
		bool isDepthCopyEnabled;
		if (!this.m_EnableSoftParticles)
		{
			isDepthCopyEnabled = this.m_EnableDistortion;
		}
		else
		{
			isDepthCopyEnabled = true;
		}
		this.m_IsDepthCopyEnabled = isDepthCopyEnabled;
		if (this.m_EnableDistortion)
		{
			if (!SystemInfo.supportsImageEffects)
			{
				Debug.LogWarning("[PKFX] Image effects not supported, distortions disabled.");
				this.m_EnableDistortion = false;
			}
		}
		if (this.m_UseSceneMesh && this.m_SceneMeshPkmmPath.Length > 0)
		{
			if (PKFxManager.LoadPkmmAsSceneMesh(this.m_SceneMeshPkmmPath))
			{
				Debug.Log("[PKFX] Scene Mesh loaded");
			}
			else
			{
				Debug.LogError("[PKFX] Failed to load mesh " + this.m_SceneMeshPkmmPath + " as scene mesh");
			}
		}
		if (!this.m_EnableDistortion)
		{
			if (!this.m_EnableSoftParticles)
			{
				goto IL_197;
			}
		}
		base.SetupDepthGrab();
		IL_197:
		if (this.m_IsDepthCopyEnabled)
		{
			if (this.m_EnableDistortion)
			{
				if (this.m_Camera.actualRenderingPath == RenderingPath.Forward)
				{
					base.SetupDistortionPass();
					if (this.m_EnableDistortion)
					{
						if (this.m_Camera.actualRenderingPath == RenderingPath.Forward)
						{
							this.m_PostFx = base.gameObject.AddComponent<PKFxDistortionEffect>();
							this.m_PostFx.m_MaterialDistortion = this.m_DistortionMat;
							this.m_PostFx.m_MaterialBlur = ((!this.m_EnableBlur) ? null : this.m_DistBlurMat);
							this.m_PostFx.m_BlurFactor = this.m_BlurFactor;
							this.m_PostFx._DistortionRT = this.m_DistortionRT;
							this.m_PostFx.hideFlags = HideFlags.HideAndDontSave;
						}
					}
				}
			}
		}
		if (this.m_BoundShaders != null)
		{
			for (int i = 0; i < this.m_BoundShaders.Count; i++)
			{
				if (this.m_BoundShaders[i] != null && !string.IsNullOrEmpty(this.m_BoundShaders[i].m_ShaderName))
				{
					if (!string.IsNullOrEmpty(this.m_BoundShaders[i].m_ShaderGroup))
					{
						this.m_BoundShaders[i].m_LoadedShaderId = PKFxManager.LoadShader(this.m_BoundShaders[i].GetDesc());
						this.m_BoundShaders[i].UpdateShaderConstants(true);
					}
					else
					{
						Debug.LogWarning("[PKFX] " + this.m_BoundShaders[i].m_ShaderName + " has no ShaderGroup, it will not be loaded");
					}
				}
			}
		}
		yield break;
	}

	private void Update()
	{
		this.m_CurrentFrameID += 1U;
	}

	private void LateUpdate()
	{
		if (this.m_BoundShaders != null)
		{
			for (int i = 0; i < this.m_BoundShaders.Count; i++)
			{
				if (this.m_BoundShaders[i] != null)
				{
					if (!string.IsNullOrEmpty(this.m_BoundShaders[i].m_ShaderName))
					{
						if (!string.IsNullOrEmpty(this.m_BoundShaders[i].m_ShaderGroup))
						{
							this.m_BoundShaders[i].UpdateShaderConstants(false);
						}
					}
				}
			}
		}
	}
}
