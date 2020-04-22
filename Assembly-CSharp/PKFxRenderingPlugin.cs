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
		yield return WaitForPack(true);
		/*Error: Unable to find new state assignment for yield return*/;
	}

	private void Update()
	{
		m_CurrentFrameID++;
	}

	private void LateUpdate()
	{
		if (m_BoundShaders == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_BoundShaders.Count; i++)
			{
				if (!(m_BoundShaders[i] != null))
				{
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!string.IsNullOrEmpty(m_BoundShaders[i].m_ShaderName))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!string.IsNullOrEmpty(m_BoundShaders[i].m_ShaderGroup))
					{
						m_BoundShaders[i].UpdateShaderConstants(false);
					}
				}
			}
			return;
		}
	}
}
