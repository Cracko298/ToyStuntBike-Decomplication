using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpritesAndLines;

public class Line
{
	public Vector2 p1;

	public Vector2 p2;

	public float radius = 0.1f;

	public Vector2 rhoTheta;

	public Color color = Color.White;

	public float rho => rhoTheta.X;

	public float theta => rhoTheta.Y;

	public Line(Vector2 p1, Vector2 p2)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		this.p1 = p1;
		this.p2 = p2;
		Recalc();
	}

	public Line(Vector2 p1, Vector2 p2, float radius)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		this.p1 = p1;
		this.p2 = p2;
		this.radius = radius;
		Recalc();
	}

	public Line(Vector2 p1, Vector2 p2, float radius, Color color)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		this.p1 = p1;
		this.p2 = p2;
		this.radius = radius;
		this.color = color;
		Recalc();
	}

	public void Move(Vector2 p1, Vector2 p2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		this.p1 = p1;
		this.p2 = p2;
		Recalc();
	}

	public void Recalc()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = p2 - p1;
		float num = ((Vector2)(ref val)).Length();
		float num2 = (float)Math.Atan2(val.Y, val.X);
		rhoTheta = new Vector2(num, num2);
	}

	public float DistanceSquaredPointToVirtualLine(Vector2 p, out Vector2 closestP)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = p2 - p1;
		Vector2 val2 = p - p1;
		float num = Vector2.Dot(val2, val);
		if (num <= 0f)
		{
			closestP = p1;
			return Vector2.DistanceSquared(p, p1);
		}
		float num2 = Vector2.Dot(val, val);
		if (num2 <= num)
		{
			closestP = p2;
			return Vector2.DistanceSquared(p, p2);
		}
		float num3 = num / num2;
		return Vector2.DistanceSquared(p, closestP = p1 + num3 * val);
	}

	public float DistanceSquaredPointToVirtualLine(Vector2 p)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Vector2 closestP;
		return DistanceSquaredPointToVirtualLine(p, out closestP);
	}

	private static void FindRadialT(Vector2 p1, Vector2 p2, Vector2 p3, float dist, out float t1, out float t2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		p1 -= p3;
		p2 -= p3;
		float x = p1.X;
		float num = p2.X - p1.X;
		float y = p1.Y;
		float num2 = p2.Y - p1.Y;
		float num3 = num * num + num2 * num2;
		float num4 = 2f * x * num + 2f * y * num2;
		float num5 = x * x + y * y - dist * dist;
		float num6 = num4 * num4 - 4f * num3 * num5;
		if (num6 < 0f)
		{
			t1 = float.MaxValue;
			t2 = float.MaxValue;
			return;
		}
		float num7 = (float)Math.Sqrt(num6);
		t1 = (0f - num4 + num7) / (2f * num3);
		t2 = (0f - num4 - num7) / (2f * num3);
		if (t2 < t1)
		{
			float num8 = t1;
			t1 = t2;
			t2 = num8;
		}
		if (t1 <= 0f && t2 >= 0f)
		{
			float num9 = Math.Abs(t1);
			float num10 = Math.Abs(t2);
			if ((double)t2 < 0.1)
			{
				t1 = float.MaxValue;
				t2 = float.MaxValue;
			}
			else if (num9 < num10)
			{
				t1 = 0f;
			}
			else
			{
				t2 = t1;
			}
		}
		else if ((double)Math.Abs(t1 - t2) < 0.01)
		{
			t1 = float.MaxValue;
			t2 = float.MaxValue;
		}
	}

	private static Vector2 Rotate(Vector2 vec, float theta)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Vector4 val = default(Vector4);
		((Vector4)(ref val))._002Ector(vec.X, vec.Y, 0f, 1f);
		Matrix val2 = Matrix.CreateRotationZ(theta);
		Vector4 val3 = Vector4.Transform(val, val2);
		return new Vector2(val3.X, val3.Y);
	}

	private void FindLinearT(Vector2 p1, Vector2 p2, float dist, out float t1, out float t2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		p1 -= this.p1;
		p2 -= this.p1;
		p1 = Rotate(p1, 0f - theta);
		p2 = Rotate(p2, 0f - theta);
		p1.X *= 1f / rho;
		p2.X *= 1f / rho;
		float y = p1.Y;
		float num = p2.Y - p1.Y;
		t1 = (dist - y) / num;
		t2 = (0f - dist - y) / num;
		if ((double)Math.Abs(t1) < 0.0001)
		{
			if ((double)num > -1E-05)
			{
				t1 = float.MaxValue;
			}
			else if (t1 < 0f)
			{
				t1 = 0f;
			}
		}
		if ((double)Math.Abs(t2) < 0.0001)
		{
			if (num < 0f)
			{
				t2 = float.MaxValue;
			}
			else if (t2 < 0f)
			{
				t2 = 0f;
			}
		}
		if (t2 < t1)
		{
			float num2 = t2;
			t2 = t1;
			t1 = num2;
		}
		float x = p1.X;
		float num3 = p2.X - p1.X;
		float num4 = x + num3 * t1;
		if (num4 < 0f || num4 > 1f)
		{
			t1 = float.MaxValue;
		}
		float num5 = x + num3 * t2;
		if (num5 < 0f || num5 > 1f)
		{
			t2 = float.MaxValue;
		}
	}

	public void FindFirstIntersection(Vector2 p1, Vector2 p2, float dist, out float tMin)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		tMin = float.MaxValue;
		FindRadialT(p1, p2, this.p1, dist, out var t, out var t2);
		if (t < tMin && t >= 0f && t < 1f)
		{
			tMin = t;
		}
		FindRadialT(p1, p2, this.p2, dist, out t, out t2);
		if (t < tMin && t >= 0f && t < 1f)
		{
			tMin = t;
		}
		FindLinearT(p1, p2, dist, out t, out t2);
		if (t < tMin && t >= 0f && t < 1f)
		{
			tMin = t;
		}
	}

	public Matrix WorldMatrix()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		Matrix val = Matrix.CreateRotationZ(theta);
		Matrix val2 = Matrix.CreateTranslation(p1.X, p1.Y, 0f);
		return val * val2;
	}
}
