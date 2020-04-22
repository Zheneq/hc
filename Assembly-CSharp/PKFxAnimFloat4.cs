using UnityEngine;

[RequireComponent(typeof(PKFxFX))]
public class PKFxAnimFloat4 : MonoBehaviour
{
	public string propertyName;

	public Vector4 value;

	private PKFxFX fx;

	private void Start()
	{
		fx = GetComponent<PKFxFX>();
		if (fx == null)
		{
			base.enabled = false;
		}
	}

	private void LateUpdate()
	{
		fx.SetAttribute(new PKFxManager.Attribute(propertyName, value));
	}
}
