using System;
using UnityEngine;

public class BlasterLurkerMineSatellite : TempSatellite
{
	private Renderer[] m_renderers;

	private bool m_visible = true;

	private bool m_attackTriggered;

	protected override void Initialize()
	{
		base.Initialize();
		this.m_renderers = base.GetComponentsInChildren<Renderer>();
		this.EnableStealth();
	}

	public override void OnTempSatelliteDestroy()
	{
		if (this.m_renderers != null)
		{
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				Renderer renderer = this.m_renderers[i];
				if (renderer != null)
				{
					Material[] materials = renderer.materials;
					for (int j = 0; j < materials.Length; j++)
					{
						UnityEngine.Object.Destroy(materials[i]);
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterLurkerMineSatellite.OnTempSatelliteDestroy()).MethodHandle;
					}
				}
			}
		}
	}

	public override void TriggerAttack(GameObject attackTarget)
	{
		base.TriggerAttack(attackTarget);
		this.m_attackTriggered = true;
		this.DisableStealth();
	}

	private void EnableStealth()
	{
		this.SetMaterialKeyword("STEALTH_ON", true);
		this.SetMaterialRenderQueue(0xBB8);
		ActorData owner = base.GetOwner();
		if (owner != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterLurkerMineSatellite.EnableStealth()).MethodHandle;
			}
			float num;
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (owner.\u0012() == GameFlowData.Get().activeOwnedActorData.\u000E())
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					num = 1f;
					goto IL_A5;
				}
			}
			num = 0f;
			IL_A5:
			float value = num;
			this.SetMaterialFloat(ActorModelData.s_materialPropertyIDTeam, value);
		}
	}

	private void DisableStealth()
	{
		this.SetMaterialKeyword("STEALTH_ON", false);
		this.SetMaterialRenderQueue(0x7D0);
	}

	private void SetMaterialFloat(int propertyID, float value)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			foreach (Material material in renderer.materials)
			{
				material.SetFloat(propertyID, value);
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterLurkerMineSatellite.SetMaterialFloat(int, float)).MethodHandle;
		}
	}

	private void SetMaterialRenderQueue(int renderQueue)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				Material material = materials[j];
				Material material2 = material;
				int renderQueue2;
				if (renderQueue < -1)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterLurkerMineSatellite.SetMaterialRenderQueue(int)).MethodHandle;
					}
					renderQueue2 = renderer.sharedMaterials[j].renderQueue;
				}
				else
				{
					renderQueue2 = renderQueue;
				}
				material2.renderQueue = renderQueue2;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void SetMaterialKeyword(string keyword, bool enable)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterLurkerMineSatellite.SetMaterialKeyword(string, bool)).MethodHandle;
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void Update()
	{
		if (this.m_renderers != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterLurkerMineSatellite.Update()).MethodHandle;
			}
			ActorData actorData;
			if (GameFlowData.Get() != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				actorData = GameFlowData.Get().activeOwnedActorData;
			}
			else
			{
				actorData = null;
			}
			ActorData actorData2 = actorData;
			ActorData owner = base.GetOwner();
			bool flag;
			if (!this.m_attackTriggered)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (actorData2 != null && owner != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = (actorData2.\u000E() == owner.\u000E());
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_modelAnimator != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					AnimatorStateInfo currentAnimatorStateInfo = this.m_modelAnimator.GetCurrentAnimatorStateInfo(0);
					if (!currentAnimatorStateInfo.IsTag("Despawn"))
					{
						for (;;)
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
							goto IL_FB;
						}
					}
					flag2 = true;
				}
			}
			IL_FB:
			if (flag2 != this.m_visible)
			{
				for (int i = 0; i < this.m_renderers.Length; i++)
				{
					if (this.m_renderers[i] != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_renderers[i].enabled = flag2;
					}
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_visible = flag2;
			}
		}
	}
}
