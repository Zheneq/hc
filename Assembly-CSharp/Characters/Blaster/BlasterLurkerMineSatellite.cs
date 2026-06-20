// ROGUES
// SERVER
using UnityEngine;

public class BlasterLurkerMineSatellite : TempSatellite
{
	private Renderer[] m_renderers;
	private bool m_visible = true;
	private bool m_attackTriggered;

	protected override void Initialize()
	{
		base.Initialize();
		m_renderers = GetComponentsInChildren<Renderer>();
		EnableStealth();
	}

	public override void OnTempSatelliteDestroy()
	{
		if (m_renderers == null)
		{
			return;
		}
		foreach (Renderer renderer in m_renderers)
		{
			if (renderer != null)
			{
				foreach (Material material in renderer.materials)
				{
					Destroy(material);  // was bugged in both reactor and rogues (wrong index)
				}
			}
		}
	}

	public override void TriggerAttack(GameObject attackTarget)
	{
		base.TriggerAttack(attackTarget);
		m_attackTriggered = true;
		DisableStealth();
	}

	private void EnableStealth()
	{
		SetMaterialKeyword("STEALTH_ON", true);
		SetMaterialRenderQueue(3000);
		ActorData owner = GetOwner();
		if (owner == null)
		{
			return;
		}
		float team = GameFlowData.Get() != null
		              && GameFlowData.Get().activeOwnedActorData != null
		              && owner.GetEnemyTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam()
			? 1f
			: 0f;
		SetMaterialFloat(ActorModelData.s_materialPropertyIDTeam, team);
	}

	private void DisableStealth()
	{
		SetMaterialKeyword("STEALTH_ON", false);
		SetMaterialRenderQueue(2000);
	}

	private void SetMaterialFloat(int propertyID, float value)
	{
		foreach (Renderer renderer in m_renderers)
		{
			foreach (Material material in renderer.materials)
			{
				material.SetFloat(propertyID, value);
			}
		}
	}

	private void SetMaterialRenderQueue(int renderQueue)
	{
		foreach (Renderer renderer in m_renderers)
		{
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				Material material = materials[j];
				material.renderQueue = renderQueue < -1
					? renderer.sharedMaterials[j].renderQueue
					: renderQueue;
			}
		}
	}

	private void SetMaterialKeyword(string keyword, bool enable)
	{
		foreach (Renderer renderer in m_renderers)
		{
			foreach (Material material in renderer.materials)
			{
				if (material != null)
				{
					if (enable)
					{
						material.EnableKeyword(keyword);
					}
					else
					{
						material.DisableKeyword(keyword);
					}
				}
			}
		}
	}

	private void Update()
	{
		if (m_renderers == null)
		{
			return;
		}
		ActorData actorData = GameFlowData.Get() != null ? GameFlowData.Get().activeOwnedActorData : null;
		ActorData owner = GetOwner();
		bool visible = m_attackTriggered
		            || actorData != null
			            && owner != null
			            && actorData.GetTeam() == owner.GetTeam();
		if (!visible && m_modelAnimator != null)
		{
			AnimatorStateInfo currentAnimatorStateInfo = m_modelAnimator.GetCurrentAnimatorStateInfo(0);
			visible = currentAnimatorStateInfo.IsTag("Despawn") || currentAnimatorStateInfo.IsTag("Attack");
		}

		if (visible != m_visible)
		{
			foreach (Renderer renderer in m_renderers)
			{
				if (renderer != null)
				{
					renderer.enabled = visible;
				}
			}
			m_visible = visible;
		}
	}
}
