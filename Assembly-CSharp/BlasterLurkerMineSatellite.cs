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
		for (int i = 0; i < m_renderers.Length; i++)
		{
			Renderer renderer = m_renderers[i];
			if (renderer != null)
			{
				Material[] materials = renderer.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					Object.Destroy(materials[i]);
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
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
		if (!(owner != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float num;
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (owner.GetOpposingTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					num = 1f;
					goto IL_00a5;
				}
			}
			num = 0f;
			goto IL_00a5;
			IL_00a5:
			float value = num;
			SetMaterialFloat(ActorModelData.s_materialPropertyIDTeam, value);
			return;
		}
	}

	private void DisableStealth()
	{
		SetMaterialKeyword("STEALTH_ON", false);
		SetMaterialRenderQueue(2000);
	}

	private void SetMaterialFloat(int propertyID, float value)
	{
		for (int i = 0; i < m_renderers.Length; i++)
		{
			Renderer renderer = m_renderers[i];
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				material.SetFloat(propertyID, value);
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	private void SetMaterialRenderQueue(int renderQueue)
	{
		for (int i = 0; i < m_renderers.Length; i++)
		{
			Renderer renderer = m_renderers[i];
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				Material material = materials[j];
				int renderQueue2;
				if (renderQueue < -1)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					renderQueue2 = renderer.sharedMaterials[j].renderQueue;
				}
				else
				{
					renderQueue2 = renderQueue;
				}
				material.renderQueue = renderQueue2;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					goto end_IL_0059;
				}
				continue;
				end_IL_0059:
				break;
			}
		}
	}

	private void SetMaterialKeyword(string keyword, bool enable)
	{
		int num = 0;
		while (num < m_renderers.Length)
		{
			Renderer renderer = m_renderers[num];
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num++;
				goto IL_0061;
			}
			IL_0061:;
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void Update()
	{
		if (m_renderers == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			object obj;
			if (GameFlowData.Get() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				obj = GameFlowData.Get().activeOwnedActorData;
			}
			else
			{
				obj = null;
			}
			ActorData actorData = (ActorData)obj;
			ActorData owner = GetOwner();
			int num;
			if (!m_attackTriggered)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (actorData != null && owner != null)
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
					num = ((actorData.GetTeam() == owner.GetTeam()) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
			}
			else
			{
				num = 1;
			}
			bool flag = (byte)num != 0;
			if (!flag)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_modelAnimator != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					AnimatorStateInfo currentAnimatorStateInfo = m_modelAnimator.GetCurrentAnimatorStateInfo(0);
					if (!currentAnimatorStateInfo.IsTag("Despawn"))
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
						if (!currentAnimatorStateInfo.IsTag("Attack"))
						{
							goto IL_00fb;
						}
					}
					flag = true;
				}
			}
			goto IL_00fb;
			IL_00fb:
			if (flag == m_visible)
			{
				return;
			}
			for (int i = 0; i < m_renderers.Length; i++)
			{
				if (m_renderers[i] != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					m_renderers[i].enabled = flag;
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				m_visible = flag;
				return;
			}
		}
	}
}
