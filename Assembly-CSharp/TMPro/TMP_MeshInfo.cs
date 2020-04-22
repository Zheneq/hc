using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	public struct TMP_MeshInfo
	{
		private static readonly Color32 s_DefaultColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private static readonly Vector3 s_DefaultNormal = new Vector3(0f, 0f, -1f);

		private static readonly Vector4 s_DefaultTangent = new Vector4(-1f, 0f, 0f, 1f);

		public Mesh mesh;

		public int vertexCount;

		public Vector3[] vertices;

		public Vector3[] normals;

		public Vector4[] tangents;

		public Vector2[] uvs0;

		public Vector2[] uvs2;

		public Color32[] colors32;

		public int[] triangles;

		public TMP_MeshInfo(Mesh mesh, int size)
		{
			if (mesh == null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				mesh = new Mesh();
			}
			else
			{
				mesh.Clear();
			}
			this.mesh = mesh;
			size = Mathf.Min(size, 16383);
			int num = size * 4;
			int num2 = size * 6;
			vertexCount = 0;
			vertices = new Vector3[num];
			uvs0 = new Vector2[num];
			uvs2 = new Vector2[num];
			colors32 = new Color32[num];
			normals = new Vector3[num];
			tangents = new Vector4[num];
			triangles = new int[num2];
			int num3 = 0;
			int num4 = 0;
			while (num4 / 4 < size)
			{
				for (int i = 0; i < 4; i++)
				{
					vertices[num4 + i] = Vector3.zero;
					uvs0[num4 + i] = Vector2.zero;
					uvs2[num4 + i] = Vector2.zero;
					colors32[num4 + i] = s_DefaultColor;
					normals[num4 + i] = s_DefaultNormal;
					tangents[num4 + i] = s_DefaultTangent;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						goto end_IL_0165;
					}
					continue;
					end_IL_0165:
					break;
				}
				triangles[num3] = num4;
				triangles[num3 + 1] = num4 + 1;
				triangles[num3 + 2] = num4 + 2;
				triangles[num3 + 3] = num4 + 2;
				triangles[num3 + 4] = num4 + 3;
				triangles[num3 + 5] = num4;
				num4 += 4;
				num3 += 6;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				this.mesh.vertices = vertices;
				this.mesh.normals = normals;
				this.mesh.tangents = tangents;
				this.mesh.triangles = triangles;
				this.mesh.bounds = new Bounds(Vector3.zero, new Vector3(3840f, 2160f, 0f));
				return;
			}
		}

		public TMP_MeshInfo(Mesh mesh, int size, bool isVolumetric)
		{
			if (mesh == null)
			{
				mesh = new Mesh();
			}
			else
			{
				mesh.Clear();
			}
			this.mesh = mesh;
			int num = isVolumetric ? 8 : 4;
			int num2;
			if (!isVolumetric)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num2 = 6;
			}
			else
			{
				num2 = 36;
			}
			int num3 = num2;
			size = Mathf.Min(size, 65532 / num);
			int num4 = size * num;
			int num5 = size * num3;
			vertexCount = 0;
			vertices = new Vector3[num4];
			uvs0 = new Vector2[num4];
			uvs2 = new Vector2[num4];
			colors32 = new Color32[num4];
			normals = new Vector3[num4];
			tangents = new Vector4[num4];
			triangles = new int[num5];
			int num6 = 0;
			int num7 = 0;
			while (num6 / num < size)
			{
				for (int i = 0; i < num; i++)
				{
					vertices[num6 + i] = Vector3.zero;
					uvs0[num6 + i] = Vector2.zero;
					uvs2[num6 + i] = Vector2.zero;
					colors32[num6 + i] = s_DefaultColor;
					normals[num6 + i] = s_DefaultNormal;
					tangents[num6 + i] = s_DefaultTangent;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					triangles[num7] = num6;
					triangles[num7 + 1] = num6 + 1;
					triangles[num7 + 2] = num6 + 2;
					triangles[num7 + 3] = num6 + 2;
					triangles[num7 + 4] = num6 + 3;
					triangles[num7 + 5] = num6;
					if (isVolumetric)
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
						triangles[num7 + 6] = num6 + 4;
						triangles[num7 + 7] = num6 + 5;
						triangles[num7 + 8] = num6 + 1;
						triangles[num7 + 9] = num6 + 1;
						triangles[num7 + 10] = num6;
						triangles[num7 + 11] = num6 + 4;
						triangles[num7 + 12] = num6 + 3;
						triangles[num7 + 13] = num6 + 2;
						triangles[num7 + 14] = num6 + 6;
						triangles[num7 + 15] = num6 + 6;
						triangles[num7 + 16] = num6 + 7;
						triangles[num7 + 17] = num6 + 3;
						triangles[num7 + 18] = num6 + 1;
						triangles[num7 + 19] = num6 + 5;
						triangles[num7 + 20] = num6 + 6;
						triangles[num7 + 21] = num6 + 6;
						triangles[num7 + 22] = num6 + 2;
						triangles[num7 + 23] = num6 + 1;
						triangles[num7 + 24] = num6 + 4;
						triangles[num7 + 25] = num6;
						triangles[num7 + 26] = num6 + 3;
						triangles[num7 + 27] = num6 + 3;
						triangles[num7 + 28] = num6 + 7;
						triangles[num7 + 29] = num6 + 4;
						triangles[num7 + 30] = num6 + 7;
						triangles[num7 + 31] = num6 + 6;
						triangles[num7 + 32] = num6 + 5;
						triangles[num7 + 33] = num6 + 5;
						triangles[num7 + 34] = num6 + 4;
						triangles[num7 + 35] = num6 + 7;
					}
					num6 += num;
					num7 += num3;
					goto IL_03cf;
				}
				IL_03cf:;
			}
			this.mesh.vertices = vertices;
			this.mesh.normals = normals;
			this.mesh.tangents = tangents;
			this.mesh.triangles = triangles;
			this.mesh.bounds = new Bounds(Vector3.zero, new Vector3(3840f, 2160f, 64f));
		}

		public void ResizeMeshInfo(int size)
		{
			size = Mathf.Min(size, 16383);
			int newSize = size * 4;
			int newSize2 = size * 6;
			int num = vertices.Length / 4;
			Array.Resize(ref vertices, newSize);
			Array.Resize(ref normals, newSize);
			Array.Resize(ref tangents, newSize);
			Array.Resize(ref uvs0, newSize);
			Array.Resize(ref uvs2, newSize);
			Array.Resize(ref colors32, newSize);
			Array.Resize(ref triangles, newSize2);
			if (size <= num)
			{
				mesh.triangles = triangles;
				mesh.vertices = vertices;
				mesh.normals = normals;
				mesh.tangents = tangents;
				return;
			}
			for (int i = num; i < size; i++)
			{
				int num2 = i * 4;
				int num3 = i * 6;
				normals[num2] = s_DefaultNormal;
				normals[1 + num2] = s_DefaultNormal;
				normals[2 + num2] = s_DefaultNormal;
				normals[3 + num2] = s_DefaultNormal;
				tangents[num2] = s_DefaultTangent;
				tangents[1 + num2] = s_DefaultTangent;
				tangents[2 + num2] = s_DefaultTangent;
				tangents[3 + num2] = s_DefaultTangent;
				triangles[num3] = num2;
				triangles[1 + num3] = 1 + num2;
				triangles[2 + num3] = 2 + num2;
				triangles[3 + num3] = 2 + num2;
				triangles[4 + num3] = 3 + num2;
				triangles[5 + num3] = num2;
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
				mesh.vertices = vertices;
				mesh.normals = normals;
				mesh.tangents = tangents;
				mesh.triangles = triangles;
				return;
			}
		}

		public void ResizeMeshInfo(int size, bool isVolumetric)
		{
			int num;
			if (!isVolumetric)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = 4;
			}
			else
			{
				num = 8;
			}
			int num2 = num;
			int num3;
			if (!isVolumetric)
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
				num3 = 6;
			}
			else
			{
				num3 = 36;
			}
			int num4 = num3;
			size = Mathf.Min(size, 65532 / num2);
			int newSize = size * num2;
			int newSize2 = size * num4;
			int num5 = vertices.Length / num2;
			Array.Resize(ref vertices, newSize);
			Array.Resize(ref normals, newSize);
			Array.Resize(ref tangents, newSize);
			Array.Resize(ref uvs0, newSize);
			Array.Resize(ref uvs2, newSize);
			Array.Resize(ref colors32, newSize);
			Array.Resize(ref triangles, newSize2);
			if (size <= num5)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						mesh.triangles = triangles;
						mesh.vertices = vertices;
						mesh.normals = normals;
						mesh.tangents = tangents;
						return;
					}
				}
			}
			for (int i = num5; i < size; i++)
			{
				int num6 = i * num2;
				int num7 = i * num4;
				normals[num6] = s_DefaultNormal;
				normals[1 + num6] = s_DefaultNormal;
				normals[2 + num6] = s_DefaultNormal;
				normals[3 + num6] = s_DefaultNormal;
				tangents[num6] = s_DefaultTangent;
				tangents[1 + num6] = s_DefaultTangent;
				tangents[2 + num6] = s_DefaultTangent;
				tangents[3 + num6] = s_DefaultTangent;
				if (isVolumetric)
				{
					normals[4 + num6] = s_DefaultNormal;
					normals[5 + num6] = s_DefaultNormal;
					normals[6 + num6] = s_DefaultNormal;
					normals[7 + num6] = s_DefaultNormal;
					tangents[4 + num6] = s_DefaultTangent;
					tangents[5 + num6] = s_DefaultTangent;
					tangents[6 + num6] = s_DefaultTangent;
					tangents[7 + num6] = s_DefaultTangent;
				}
				triangles[num7] = num6;
				triangles[1 + num7] = 1 + num6;
				triangles[2 + num7] = 2 + num6;
				triangles[3 + num7] = 2 + num6;
				triangles[4 + num7] = 3 + num6;
				triangles[5 + num7] = num6;
				if (isVolumetric)
				{
					triangles[num7 + 6] = num6 + 4;
					triangles[num7 + 7] = num6 + 5;
					triangles[num7 + 8] = num6 + 1;
					triangles[num7 + 9] = num6 + 1;
					triangles[num7 + 10] = num6;
					triangles[num7 + 11] = num6 + 4;
					triangles[num7 + 12] = num6 + 3;
					triangles[num7 + 13] = num6 + 2;
					triangles[num7 + 14] = num6 + 6;
					triangles[num7 + 15] = num6 + 6;
					triangles[num7 + 16] = num6 + 7;
					triangles[num7 + 17] = num6 + 3;
					triangles[num7 + 18] = num6 + 1;
					triangles[num7 + 19] = num6 + 5;
					triangles[num7 + 20] = num6 + 6;
					triangles[num7 + 21] = num6 + 6;
					triangles[num7 + 22] = num6 + 2;
					triangles[num7 + 23] = num6 + 1;
					triangles[num7 + 24] = num6 + 4;
					triangles[num7 + 25] = num6;
					triangles[num7 + 26] = num6 + 3;
					triangles[num7 + 27] = num6 + 3;
					triangles[num7 + 28] = num6 + 7;
					triangles[num7 + 29] = num6 + 4;
					triangles[num7 + 30] = num6 + 7;
					triangles[num7 + 31] = num6 + 6;
					triangles[num7 + 32] = num6 + 5;
					triangles[num7 + 33] = num6 + 5;
					triangles[num7 + 34] = num6 + 4;
					triangles[num7 + 35] = num6 + 7;
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				mesh.vertices = vertices;
				mesh.normals = normals;
				mesh.tangents = tangents;
				mesh.triangles = triangles;
				return;
			}
		}

		public void Clear()
		{
			if (vertices == null)
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
			Array.Clear(vertices, 0, vertices.Length);
			vertexCount = 0;
			if (mesh != null)
			{
				mesh.vertices = vertices;
			}
		}

		public void Clear(bool uploadChanges)
		{
			if (vertices == null)
			{
				while (true)
				{
					switch (4)
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
			Array.Clear(vertices, 0, vertices.Length);
			vertexCount = 0;
			if (!uploadChanges)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (mesh != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						mesh.vertices = vertices;
						return;
					}
				}
				return;
			}
		}

		public void ClearUnusedVertices()
		{
			int num = vertices.Length - vertexCount;
			if (num <= 0)
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
				Array.Clear(vertices, vertexCount, num);
				return;
			}
		}

		public void ClearUnusedVertices(int startIndex)
		{
			int num = vertices.Length - startIndex;
			if (num > 0)
			{
				Array.Clear(vertices, startIndex, num);
			}
		}

		public void ClearUnusedVertices(int startIndex, bool updateMesh)
		{
			int num = vertices.Length - startIndex;
			if (num > 0)
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
				Array.Clear(vertices, startIndex, num);
			}
			if (!updateMesh || !(mesh != null))
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
				mesh.vertices = vertices;
				return;
			}
		}

		public void SortGeometry(VertexSortingOrder order)
		{
			switch (order)
			{
			case VertexSortingOrder.Normal:
				return;
			case VertexSortingOrder.Reverse:
			{
				int num = vertexCount / 4;
				for (int i = 0; i < num; i++)
				{
					int num2 = i * 4;
					int num3 = (num - i - 1) * 4;
					if (num2 < num3)
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
						SwapVertexData(num2, num3);
					}
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
				return;
			}
		}

		public void SortGeometry(IList<int> sortingOrder)
		{
			int count = sortingOrder.Count;
			if (count * 4 > vertices.Length)
			{
				return;
			}
			int num = 0;
			while (num < count)
			{
				int num2;
				for (num2 = sortingOrder[num]; num2 < num; num2 = sortingOrder[num2])
				{
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
					if (num2 != num)
					{
						SwapVertexData(num2 * 4, num * 4);
					}
					num++;
					goto IL_005a;
				}
				IL_005a:;
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}

		public void SwapVertexData(int src, int dst)
		{
			Vector3 vector = vertices[dst];
			vertices[dst] = vertices[src];
			vertices[src] = vector;
			vector = vertices[dst + 1];
			vertices[dst + 1] = vertices[src + 1];
			vertices[src + 1] = vector;
			vector = vertices[dst + 2];
			vertices[dst + 2] = vertices[src + 2];
			vertices[src + 2] = vector;
			vector = vertices[dst + 3];
			vertices[dst + 3] = vertices[src + 3];
			vertices[src + 3] = vector;
			Vector2 vector2 = uvs0[dst];
			uvs0[dst] = uvs0[src];
			uvs0[src] = vector2;
			vector2 = uvs0[dst + 1];
			uvs0[dst + 1] = uvs0[src + 1];
			uvs0[src + 1] = vector2;
			vector2 = uvs0[dst + 2];
			uvs0[dst + 2] = uvs0[src + 2];
			uvs0[src + 2] = vector2;
			vector2 = uvs0[dst + 3];
			uvs0[dst + 3] = uvs0[src + 3];
			uvs0[src + 3] = vector2;
			vector2 = uvs2[dst];
			uvs2[dst] = uvs2[src];
			uvs2[src] = vector2;
			vector2 = uvs2[dst + 1];
			uvs2[dst + 1] = uvs2[src + 1];
			uvs2[src + 1] = vector2;
			vector2 = uvs2[dst + 2];
			uvs2[dst + 2] = uvs2[src + 2];
			uvs2[src + 2] = vector2;
			vector2 = uvs2[dst + 3];
			uvs2[dst + 3] = uvs2[src + 3];
			uvs2[src + 3] = vector2;
			Color32 color = colors32[dst];
			colors32[dst] = colors32[src];
			colors32[src] = color;
			color = colors32[dst + 1];
			colors32[dst + 1] = colors32[src + 1];
			colors32[src + 1] = color;
			color = colors32[dst + 2];
			colors32[dst + 2] = colors32[src + 2];
			colors32[src + 2] = color;
			color = colors32[dst + 3];
			colors32[dst + 3] = colors32[src + 3];
			colors32[src + 3] = color;
		}
	}
}
