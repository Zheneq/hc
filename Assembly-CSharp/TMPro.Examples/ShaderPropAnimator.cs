using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class ShaderPropAnimator : MonoBehaviour
	{
		private Renderer renderer;

		private Material material;

		public AnimationCurve curve;

		public float alpha;

		private void _0016()
		{
			renderer = GetComponent<Renderer>();
			material = renderer.material;
		}

		private void Start()
		{
			StartCoroutine(Animate());
		}

		private IEnumerator Animate()
		{
			alpha = Random.Range(0f, 1f);
			float value = curve.Evaluate(alpha);
			material.SetFloat(ShaderUtilities.ID_GlowPower, value);
			alpha += Time.deltaTime * Random.Range(0.2f, 0.3f);
			yield return new WaitForEndOfFrame();
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
