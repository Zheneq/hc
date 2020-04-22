using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class SkewTextExample : MonoBehaviour
	{
		private TMP_Text _001D;

		public AnimationCurve _000E = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.25f, 2f), new Keyframe(0.5f, 0f), new Keyframe(0.75f, 2f), new Keyframe(1f, 0f));

		public float _0012 = 1f;

		public float _0015 = 1f;

		private void _0016()
		{
			_001D = base.gameObject.GetComponent<TMP_Text>();
		}

		private void _0013()
		{
			StartCoroutine(coroutine0016_0016());
		}

		private AnimationCurve _0016(AnimationCurve _001D)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			animationCurve.keys = _001D.keys;
			return animationCurve;
		}

		private IEnumerator coroutine0016_0016()
		{
			_000E.preWrapMode = WrapMode.Once;
			_000E.postWrapMode = WrapMode.Once;
			_001D.havePropertiesChanged = true;
			_0012 *= 10f;
			float num = _0012;
			float num2 = _0015;
			AnimationCurve animationCurve = _0016(_000E);
			TMP_TextInfo textInfo;
			int characterCount;
			while (true)
			{
				if (!_001D.havePropertiesChanged && num == _0012)
				{
					if (animationCurve.keys[1].value == _000E.keys[1].value)
					{
						if (num2 == _0015)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									yield return null;
									/*Error: Unable to find new state assignment for yield return*/;
								}
							}
						}
					}
				}
				num = _0012;
				animationCurve = _0016(_000E);
				num2 = _0015;
				_001D.ForceMeshUpdate();
				textInfo = _001D.textInfo;
				characterCount = textInfo.characterCount;
				if (characterCount != 0)
				{
					break;
				}
			}
			Vector3 min = _001D.bounds.min;
			float x = min.x;
			Vector3 max = _001D.bounds.max;
			float x2 = max.x;
			for (int i = 0; i < characterCount; i++)
			{
				if (!textInfo.characterInfo[i].isVisible)
				{
					continue;
				}
				int vertexIndex = textInfo.characterInfo[i].vertexIndex;
				int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
				Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
				Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
				vertices[vertexIndex] += -vector;
				vertices[vertexIndex + 1] += -vector;
				vertices[vertexIndex + 2] += -vector;
				vertices[vertexIndex + 3] += -vector;
				float num3 = _0015 * 0.01f;
				Vector3 vector2 = new Vector3(num3 * (textInfo.characterInfo[i].topRight.y - textInfo.characterInfo[i].baseLine), 0f, 0f);
				Vector3 a = new Vector3(num3 * (textInfo.characterInfo[i].baseLine - textInfo.characterInfo[i].bottomRight.y), 0f, 0f);
				vertices[vertexIndex] += -a;
				vertices[vertexIndex + 1] += vector2;
				vertices[vertexIndex + 2] += vector2;
				vertices[vertexIndex + 3] += -a;
				float num4 = (vector.x - x) / (x2 - x);
				float num5 = num4 + 0.0001f;
				float y = _000E.Evaluate(num4) * _0012;
				float y2 = _000E.Evaluate(num5) * _0012;
				Vector3 lhs = new Vector3(1f, 0f, 0f);
				Vector3 rhs = new Vector3(num5 * (x2 - x) + x, y2) - new Vector3(vector.x, y);
				float num6 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
				Vector3 vector3 = Vector3.Cross(lhs, rhs);
				float num7;
				if (vector3.z > 0f)
				{
					num7 = num6;
				}
				else
				{
					num7 = 360f - num6;
				}
				float z = num7;
				Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, y, 0f), Quaternion.Euler(0f, 0f, z), Vector3.one);
				vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
				vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
				vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
				vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
				vertices[vertexIndex] += vector;
				vertices[vertexIndex + 1] += vector;
				vertices[vertexIndex + 2] += vector;
				vertices[vertexIndex + 3] += vector;
			}
			while (true)
			{
				_001D.UpdateVertexData();
				yield return null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
