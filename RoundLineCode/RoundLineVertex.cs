using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoundLineCode;

internal struct RoundLineVertex
{
	public Vector3 pos;

	public Vector2 rhoTheta;

	public Vector2 scaleTrans;

	public float index;

	public static int SizeInBytes = 32;

	public static VertexElement[] VertexElements = (VertexElement[])(object)new VertexElement[4]
	{
		new VertexElement((short)0, (short)0, (VertexElementFormat)2, (VertexElementMethod)0, (VertexElementUsage)0, (byte)0),
		new VertexElement((short)0, (short)12, (VertexElementFormat)1, (VertexElementMethod)0, (VertexElementUsage)3, (byte)0),
		new VertexElement((short)0, (short)20, (VertexElementFormat)1, (VertexElementMethod)0, (VertexElementUsage)5, (byte)0),
		new VertexElement((short)0, (short)28, (VertexElementFormat)0, (VertexElementMethod)0, (VertexElementUsage)5, (byte)1)
	};

	public RoundLineVertex(Vector3 pos, Vector2 norm, Vector2 tex, float index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		this.pos = pos;
		rhoTheta = norm;
		scaleTrans = tex;
		this.index = index;
	}
}
