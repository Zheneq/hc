using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class WarpTextExample : MonoBehaviour
	{
		private TMP_Text _001D;

		public AnimationCurve _000E = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.25f, 2f), new Keyframe(0.5f, 0f), new Keyframe(0.75f, 2f), new Keyframe(1f, 0f));

		public float _0012 = 1f;

		public float _0015 = 1f;

		public float _0016 = 1f;

		private void _0013()
		{
			_001D = base.gameObject.GetComponent<TMP_Text>();
		}

		private void _0018()
		{
			StartCoroutine(coroutine0013());
		}

		private AnimationCurve _0013(AnimationCurve _001D)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			animationCurve.keys = _001D.keys;
			return animationCurve;
		}

		private IEnumerator coroutine0013()
		{
			_000E.preWrapMode = WrapMode.Once;
			_000E.postWrapMode = WrapMode.Once;
			_001D.havePropertiesChanged = true;
			_0016 *= 10f;
			float num = _0016;
			AnimationCurve animationCurve = _0013(_000E);
			while (true)
			{
				if (!_001D.havePropertiesChanged)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (num == _0016)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (animationCurve.keys[1].value == _000E.keys[1].value)
						{
							break;
						}
					}
				}
				num = _0016;
				animationCurve = _0013(_000E);
				_001D.ForceMeshUpdate();
				TMP_TextInfo textInfo = _001D.textInfo;
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
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
					continue;
				}
				Vector3 min = _001D.bounds.min;
				float x = min.x;
				Vector3 max = _001D.bounds.max;
				float x2 = max.x;
				for (int i = 0; i < characterCount; i++)
				{
					if (!textInfo.characterInfo[i].isVisible)
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
					float num2 = (vector.x - x) / (x2 - x);
					float num3 = num2 + 0.0001f;
					float y = _000E.Evaluate(num2) * _0016;
					float y2 = _000E.Evaluate(num3) * _0016;
					Vector3 lhs = new Vector3(1f, 0f, 0f);
					Vector3 rhs = new Vector3(num3 * (x2 - x) + x, y2) - new Vector3(vector.x, y);
					float num4 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
					Vector3 vector2 = Vector3.Cross(lhs, rhs);
					float num5;
					if (vector2.z > 0f)
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
						num5 = num4;
					}
					else
					{
						num5 = 360f - num4;
					}
					float z = num5;
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
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				_001D.UpdateVertexData();
				yield return new WaitForSeconds(0.025f);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				yield return null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
