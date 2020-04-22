using UnityEngine;

namespace TMPro.Examples
{
	public class TMP_FrameRateCounter : MonoBehaviour
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

		private TextMeshPro _0018;

		private Transform _0009;

		private Camera _0019;

		private FpsCounterAnchorPositions _0011;

		private void _001A()
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			_0019 = Camera.main;
			Application.targetFrameRate = -1;
			GameObject gameObject = new GameObject("Frame Counter");
			_0018 = gameObject.AddComponent<TextMeshPro>();
			_0018.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			_0018.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			_0009 = gameObject.transform;
			_0009.SetParent(_0019.transform);
			_0009.localRotation = Quaternion.identity;
			_0018.enableWordWrapping = false;
			_0018.fontSize = 24f;
			_0018.isOverlay = true;
			_001A(_0015);
			_0011 = _0015;
		}

		private void _0004()
		{
			_000E = Time.realtimeSinceStartup;
			_0012 = 0;
		}

		private void _000B()
		{
			if (_0015 != _0011)
			{
				_001A(_0015);
			}
			_0011 = _0015;
			_0012++;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (!(realtimeSinceStartup > _000E + _001D))
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
				float num = (float)_0012 / (realtimeSinceStartup - _000E);
				float arg = 1000f / Mathf.Max(num, 1E-05f);
				if (num < 30f)
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
				_0018.SetText(_0016 + "{0:2}</color> FPS \n{1:2} <#8080ff>MS", num, arg);
				_0012 = 0;
				_000E = realtimeSinceStartup;
				return;
			}
		}

		private void _001A(FpsCounterAnchorPositions _001D)
		{
			_0018.margin = new Vector4(1f, 1f, 1f, 1f);
			switch (_001D)
			{
			case FpsCounterAnchorPositions._001D:
				_0018.alignment = TextAlignmentOptions.TopLeft;
				_0018.rectTransform.pivot = new Vector2(0f, 1f);
				_0009.position = _0019.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				break;
			case FpsCounterAnchorPositions._000E:
				_0018.alignment = TextAlignmentOptions.BottomLeft;
				_0018.rectTransform.pivot = new Vector2(0f, 0f);
				_0009.position = _0019.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				break;
			case FpsCounterAnchorPositions._0012:
				_0018.alignment = TextAlignmentOptions.TopRight;
				_0018.rectTransform.pivot = new Vector2(1f, 1f);
				_0009.position = _0019.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				break;
			case FpsCounterAnchorPositions._0015:
				_0018.alignment = TextAlignmentOptions.BottomRight;
				_0018.rectTransform.pivot = new Vector2(1f, 0f);
				_0009.position = _0019.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				break;
			}
		}
	}
}
