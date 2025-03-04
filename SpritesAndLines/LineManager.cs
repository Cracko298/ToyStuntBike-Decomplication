using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpritesAndLines;

internal class LineManager
{
	private GraphicsDevice device;

	private Effect effect;

	private EffectParameter wvpMatrixParameter;

	private EffectParameter timeParameter;

	private EffectParameter lengthParameter;

	private EffectParameter rotationParameter;

	private EffectParameter radiusParameter;

	private EffectParameter lineColorParameter;

	private VertexBuffer vb;

	private IndexBuffer ib;

	private VertexDeclaration vdecl;

	private int numVertices;

	private int numIndices;

	private int numPrimitives;

	private int bytesPerVertex;

	public int numLinesDrawn;

	public void Init(GraphicsDevice device, ContentManager content)
	{
		this.device = device;
		effect = content.Load<Effect>("Line");
		wvpMatrixParameter = effect.Parameters["worldViewProj"];
		timeParameter = effect.Parameters["time"];
		lengthParameter = effect.Parameters["length"];
		rotationParameter = effect.Parameters["rotation"];
		radiusParameter = effect.Parameters["radius"];
		lineColorParameter = effect.Parameters["lineColor"];
		CreateLineMesh();
	}

	private void CreateLineMesh()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		numVertices = 12;
		numPrimitives = 4;
		numIndices = 3 * numPrimitives;
		short[] array = new short[numIndices];
		bytesPerVertex = VertexPositionNormalTexture.SizeInBytes;
		VertexPositionNormalTexture[] array2 = (VertexPositionNormalTexture[])(object)new VertexPositionNormalTexture[numVertices];
		ref VertexPositionNormalTexture reference = ref array2[0];
		reference = new VertexPositionNormalTexture(new Vector3(0f, -1f, 0f), new Vector3(1f, 4.712389f, 0f), new Vector2(0f, 0f));
		ref VertexPositionNormalTexture reference2 = ref array2[1];
		reference2 = new VertexPositionNormalTexture(new Vector3(0f, -1f, 0f), new Vector3(1f, 4.712389f, 0f), new Vector2(0f, 1f));
		ref VertexPositionNormalTexture reference3 = ref array2[2];
		reference3 = new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f), new Vector3(0f, 4.712389f, 0f), new Vector2(0f, 1f));
		ref VertexPositionNormalTexture reference4 = ref array2[3];
		reference4 = new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f), new Vector3(0f, 4.712389f, 0f), new Vector2(0f, 0f));
		ref VertexPositionNormalTexture reference5 = ref array2[4];
		reference5 = new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f), new Vector3(0f, (float)Math.PI / 2f, 0f), new Vector2(0f, 1f));
		ref VertexPositionNormalTexture reference6 = ref array2[5];
		reference6 = new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f), new Vector3(0f, (float)Math.PI / 2f, 0f), new Vector2(0f, 0f));
		ref VertexPositionNormalTexture reference7 = ref array2[6];
		reference7 = new VertexPositionNormalTexture(new Vector3(0f, 1f, 0f), new Vector3(1f, (float)Math.PI / 2f, 0f), new Vector2(0f, 1f));
		ref VertexPositionNormalTexture reference8 = ref array2[7];
		reference8 = new VertexPositionNormalTexture(new Vector3(0f, 1f, 0f), new Vector3(1f, (float)Math.PI / 2f, 0f), new Vector2(0f, 0f));
		array[0] = 0;
		array[1] = 1;
		array[2] = 2;
		array[3] = 2;
		array[4] = 3;
		array[5] = 0;
		array[6] = 4;
		array[7] = 6;
		array[8] = 5;
		array[9] = 6;
		array[10] = 7;
		array[11] = 5;
		vb = new VertexBuffer(device, numVertices * bytesPerVertex, (BufferUsage)0);
		vb.SetData<VertexPositionNormalTexture>(array2);
		vdecl = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
		ib = new IndexBuffer(device, numIndices * 2, (BufferUsage)0, (IndexElementSize)0);
		ib.SetData<short>(array);
	}

	public void FindNearbyLines(List<Line> lineList, List<Line> nearbyLineList, float globalLineRadius, Vector2 referencePos, float referenceRadius)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		nearbyLineList.Clear();
		foreach (Line line in lineList)
		{
			float num = ((globalLineRadius != 0f) ? (referenceRadius + globalLineRadius) : (referenceRadius + line.radius));
			if (line.DistanceSquaredPointToVirtualLine(referencePos) < num * num)
			{
				nearbyLineList.Add(line);
			}
		}
	}

	public float MinDistanceSquaredDeviation(List<Line> lineList, Vector2 currentPos, float discRadius)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		float num = float.MaxValue;
		foreach (Line line in lineList)
		{
			float num2 = (line.radius + discRadius) * (line.radius + discRadius);
			float num3 = line.DistanceSquaredPointToVirtualLine(currentPos);
			float num4 = num3 - num2;
			if (num4 < num)
			{
				num = num4;
			}
		}
		return num;
	}

	public void CollideAndSlide(List<Line> lineList, Vector2 currentPos, Vector2 proposedPos, float discRadius, out Vector2 finalPos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = currentPos;
		Vector2 val2 = proposedPos;
		bool flag = false;
		Vector2 p = default(Vector2);
		Vector2 p2 = default(Vector2);
		Vector2 p3 = default(Vector2);
		Vector2 val3 = default(Vector2);
		Vector2 currentPos2 = default(Vector2);
		Vector2 val6 = default(Vector2);
		while (Vector2.DistanceSquared(val, val2) > 1.0000001E-06f)
		{
			float num = 1f;
			Line line = null;
			foreach (Line line3 in lineList)
			{
				float num2 = line3.radius + discRadius;
				float num3 = num2 * num2;
				line3.DistanceSquaredPointToVirtualLine(val);
				line3.FindFirstIntersection(val, val2, num2, out var tMin);
				if (tMin >= 0f && tMin <= 1f)
				{
					((Vector2)(ref p))._002Ector(MathHelper.Lerp(val.X, val2.X, tMin), MathHelper.Lerp(val.Y, val2.Y, tMin));
					if (line3.DistanceSquaredPointToVirtualLine(p) - num3 < 0f)
					{
						float num4 = 0f;
						float num5 = tMin;
						for (int i = 0; i < 10; i++)
						{
							float num6 = (num4 + num5) / 2f;
							((Vector2)(ref p2))._002Ector(MathHelper.Lerp(val.X, val2.X, num6), MathHelper.Lerp(val.Y, val2.Y, num6));
							float num7 = line3.DistanceSquaredPointToVirtualLine(p2);
							if (num7 - num3 < 0f)
							{
								num5 = num6;
							}
							else
							{
								num4 = num6;
							}
						}
						tMin = num4;
					}
					if (tMin < num)
					{
						num = tMin;
						line = line3;
					}
					continue;
				}
				float num8 = line3.DistanceSquaredPointToVirtualLine(val2);
				if (!(num8 - num3 < 0f))
				{
					continue;
				}
				float num9 = 0f;
				float num10 = 1f;
				for (int j = 0; j < 10; j++)
				{
					float num11 = (num9 + num10) / 2f;
					((Vector2)(ref p3))._002Ector(MathHelper.Lerp(val.X, val2.X, num11), MathHelper.Lerp(val.Y, val2.Y, num11));
					float num12 = line3.DistanceSquaredPointToVirtualLine(p3);
					if (num12 - num3 < 0f)
					{
						num10 = num11;
					}
					else
					{
						num9 = num11;
					}
				}
				if (num9 < num)
				{
					num = num9;
					line = line3;
				}
			}
			if (line == null)
			{
				val = val2;
				continue;
			}
			((Vector2)(ref val3))._002Ector(MathHelper.Lerp(val.X, val2.X, num), MathHelper.Lerp(val.Y, val2.Y, num));
			float num13 = (line.radius + discRadius) * (line.radius + discRadius);
			float num14 = MinDistanceSquaredDeviation(lineList, val3, discRadius);
			if (num14 < 0f)
			{
				float num15 = 0f;
				float num16 = num;
				for (int k = 0; k < 10; k++)
				{
					float num17 = (num15 + num16) / 2f;
					((Vector2)(ref currentPos2))._002Ector(MathHelper.Lerp(val.X, val2.X, num17), MathHelper.Lerp(val.Y, val2.Y, num17));
					if (MinDistanceSquaredDeviation(lineList, currentPos2, discRadius) < 0f)
					{
						num16 = num17;
					}
					else
					{
						num15 = num17;
					}
				}
				num = num15;
				((Vector2)(ref val3))._002Ector(MathHelper.Lerp(val.X, val2.X, num), MathHelper.Lerp(val.Y, val2.Y, num));
			}
			bool flag2 = true;
			Vector2 val4;
			if (flag)
			{
				val4 = val3;
				flag2 = false;
			}
			else
			{
				flag = true;
			}
			if (flag2)
			{
				line.DistanceSquaredPointToVirtualLine(val3, out var closestP);
				Line line2 = new Line(val3, closestP);
				Vector2 val5 = val3 - closestP;
				((Vector2)(ref val5)).Normalize();
				float theta = line2.theta;
				theta += (float)Math.PI / 2f;
				((Vector2)(ref val6))._002Ector(val3.X + (float)Math.Cos(theta), val3.Y + (float)Math.Sin(theta));
				Vector2 val7 = val2 - val3;
				Vector2 val8 = val6 - val3;
				float num18 = Vector2.Dot(val7, val8);
				val4 = val3 + num18 * val8;
				float num19 = line.DistanceSquaredPointToVirtualLine(val4);
				if (num19 - num13 < 0f)
				{
					float num20 = 0f;
					float num21 = 0f - (num19 - num13) + 0.0001f;
					for (int l = 0; l < 10; l++)
					{
						float num22 = (num20 + num21) / 2f;
						Vector2 p4 = val4 + num22 * val5;
						float num23 = line.DistanceSquaredPointToVirtualLine(p4);
						if (num23 - num13 >= 0f)
						{
							num21 = num22;
						}
						else
						{
							num20 = num22;
						}
					}
					val4 += num21 * val5;
				}
			}
			else
			{
				val4 = val3;
			}
			val = val3;
			val2 = val4;
		}
		finalPos = val;
	}

	public void Draw(List<Line> lineList, float globalRadius, Color globalColor, Matrix viewMatrix, Matrix projMatrix, float time, string techniqueName)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (techniqueName == null)
		{
			effect.CurrentTechnique = effect.Techniques[0];
		}
		else
		{
			effect.CurrentTechnique = effect.Techniques[techniqueName];
		}
		effect.Begin();
		EffectPass val = effect.CurrentTechnique.Passes[0];
		device.VertexDeclaration = vdecl;
		device.Vertices[0].SetSource(vb, 0, bytesPerVertex);
		device.Indices = ib;
		val.Begin();
		timeParameter.SetValue(time);
		if (globalColor == Color.TransparentBlack)
		{
			flag = true;
		}
		else
		{
			Vector4 value = ((Color)(ref globalColor)).ToVector4();
			lineColorParameter.SetValue(value);
		}
		if (globalRadius != 0f)
		{
			radiusParameter.SetValue(globalRadius);
		}
		foreach (Line line in lineList)
		{
			Matrix value2 = line.WorldMatrix() * viewMatrix * projMatrix;
			wvpMatrixParameter.SetValue(value2);
			lengthParameter.SetValue(line.rho);
			rotationParameter.SetValue(line.theta);
			if (globalRadius == 0f)
			{
				radiusParameter.SetValue(line.radius);
			}
			if (flag)
			{
				Vector4 value = ((Color)(ref line.color)).ToVector4();
				lineColorParameter.SetValue(value);
			}
			effect.CommitChanges();
			device.DrawIndexedPrimitives((PrimitiveType)4, 0, 0, numVertices, 0, numPrimitives);
			numLinesDrawn++;
		}
		val.End();
		effect.End();
	}
}
