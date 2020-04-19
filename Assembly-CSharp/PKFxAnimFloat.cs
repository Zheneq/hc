using System;
using UnityEngine;

[RequireComponent(typeof(PKFxFX))]
public class PKFxAnimFloat : MonoBehaviour
{
	public string propertyName;

	public float value;

	private PKFxFX fx;

	private void Start()
	{
		this.fx = base.GetComponent<PKFxFX>();
		if (this.fx == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PKFxAnimFloat.Start()).MethodHandle;
			}
			base.enabled = false;
		}
	}

	private void LateUpdate()
	{
		this.fx.SetAttribute(new PKFxManager.Attribute(this.propertyName, this.value));
	}
}
