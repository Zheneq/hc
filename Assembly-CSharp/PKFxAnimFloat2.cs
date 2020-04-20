using System;
using UnityEngine;

[RequireComponent(typeof(PKFxFX))]
public class PKFxAnimFloat2 : MonoBehaviour
{
	public string propertyName;

	public Vector2 value;

	private PKFxFX fx;

	private void Start()
	{
		this.fx = base.GetComponent<PKFxFX>();
		if (this.fx == null)
		{
			base.enabled = false;
		}
	}

	private void LateUpdate()
	{
		this.fx.SetAttribute(new PKFxManager.Attribute(this.propertyName, this.value));
	}
}
