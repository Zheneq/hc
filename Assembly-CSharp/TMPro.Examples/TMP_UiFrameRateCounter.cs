using System.Text;
using UnityEngine;

namespace TMPro.Examples
{
	public class TMP_UiFrameRateCounter : MonoBehaviour
	{
		public enum FpsCounterAnchorPositions
		{
			_001D,
			_000E,
			_0012,
			_0015
		}

		public float _001D = 5f;

		private float _000E;

		private int _0012;

		public FpsCounterAnchorPositions _0015 = FpsCounterAnchorPositions._0012;

		private string _0016;

		private const string _0013 = "{0:2}</color> FPS \n{1:2} <#8080ff>MS";

		private TextMeshProUGUI _0018;

		private RectTransform _0009;

		private FpsCounterAnchorPositions _0019;

		private void _0011()
		{
			if (!base.enabled)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			Application.targetFrameRate = 120;
			GameObject gameObject = new GameObject("Frame Counter");
			_0009 = gameObject.AddComponent<RectTransform>();
			_0009.SetParent(base.transform, false);
			_0018 = gameObject.AddComponent<TextMeshProUGUI>();
			_0018.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			_0018.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			_0018.enableWordWrapping = false;
			_0018.fontSize = 36f;
			_0018.isOverlay = true;
			_0011(_0015);
			_0019 = _0015;
		}

		private void _001A()
		{
			_000E = Time.realtimeSinceStartup;
			_0012 = 0;
		}

		private void _0004()
		{
			if (_0015 != _0019)
			{
				_0011(_0015);
			}
			_0019 = _0015;
			_0012++;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (!(realtimeSinceStartup > _000E + _001D))
			{
				return;
			}
			while (true)
			{
				float num = (float)_0012 / (realtimeSinceStartup - _000E);
				float arg = 1000f / Mathf.Max(num, 1E-05f);
				if (num < 30f)
				{
					_0016 = "<color=yellow>";
				}
				else if (num < 10f)
				{
					_0016 = "<color=red>";
				}
				else
				{
					_0016 = "<color=green>";
				}
				_0018.SetText(new StringBuilder().Append(_0016).Append("{0:2}</color> FPS \n{1:2} <#8080ff>MS").ToString(), num, arg);
				_0012 = 0;
				_000E = realtimeSinceStartup;
				return;
			}
		}

		private void _0011(FpsCounterAnchorPositions _001D)
		{
			switch (_001D)
			{
			case FpsCounterAnchorPositions._001D:
				_0018.alignment = TextAlignmentOptions.TopLeft;
				_0009.pivot = new Vector2(0f, 1f);
				_0009.anchorMin = new Vector2(0.01f, 0.99f);
				_0009.anchorMax = new Vector2(0.01f, 0.99f);
				_0009.anchoredPosition = new Vector2(0f, 1f);
				break;
			case FpsCounterAnchorPositions._000E:
				_0018.alignment = TextAlignmentOptions.BottomLeft;
				_0009.pivot = new Vector2(0f, 0f);
				_0009.anchorMin = new Vector2(0.01f, 0.01f);
				_0009.anchorMax = new Vector2(0.01f, 0.01f);
				_0009.anchoredPosition = new Vector2(0f, 0f);
				break;
			case FpsCounterAnchorPositions._0012:
				_0018.alignment = TextAlignmentOptions.TopRight;
				_0009.pivot = new Vector2(1f, 1f);
				_0009.anchorMin = new Vector2(0.99f, 0.99f);
				_0009.anchorMax = new Vector2(0.99f, 0.99f);
				_0009.anchoredPosition = new Vector2(1f, 1f);
				break;
			case FpsCounterAnchorPositions._0015:
				_0018.alignment = TextAlignmentOptions.BottomRight;
				_0009.pivot = new Vector2(1f, 0f);
				_0009.anchorMin = new Vector2(0.99f, 0.01f);
				_0009.anchorMax = new Vector2(0.99f, 0.01f);
				_0009.anchoredPosition = new Vector2(1f, 0f);
				break;
			}
		}
	}
}
