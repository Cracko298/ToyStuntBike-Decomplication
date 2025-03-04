using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppNamespace;

public class OceanII
{
	public struct VertexMultitextured
	{
		public Vector3 Position;

		public Vector3 Normal;

		public Vector2 TextureCoordinate;

		public Vector3 Tangent;

		public Vector3 BiNormal;

		public static int SizeInBytes = 56;

		public static VertexElement[] VertexElements = (VertexElement[])(object)new VertexElement[5]
		{
			new VertexElement((short)0, (short)0, (VertexElementFormat)2, (VertexElementMethod)0, (VertexElementUsage)0, (byte)0),
			new VertexElement((short)0, (short)12, (VertexElementFormat)2, (VertexElementMethod)0, (VertexElementUsage)3, (byte)0),
			new VertexElement((short)0, (short)24, (VertexElementFormat)1, (VertexElementMethod)0, (VertexElementUsage)5, (byte)0),
			new VertexElement((short)0, (short)32, (VertexElementFormat)2, (VertexElementMethod)0, (VertexElementUsage)6, (byte)0),
			new VertexElement((short)0, (short)44, (VertexElementFormat)2, (VertexElementMethod)0, (VertexElementUsage)7, (byte)0)
		};
	}

	private VertexBuffer vb;

	private IndexBuffer ib;

	private VertexMultitextured[] myVertices;

	private int myHeight = 64;

	private int myWidth = 64;

	private Vector3 m_Position;

	private Vector3 myScale;

	private Quaternion myRotation;

	private Effect effect;

	private Vector3 basePosition = new Vector3(0f, -0.1f, 0f);

	private Color myDeepWater = Color.Black;

	private Color myShallowWater = Color.SkyBlue;

	private Color myReflection = Color.White;

	private float myHDRMult = 3f;

	private float myReflectionAmt = 1f;

	private float myWaterColorAmount = 1f;

	private float myWaveFrequency = 0.1f;

	private float myWaveAmplitude;

	private float myBumpHeight = 0.1f;

	private VertexDeclaration vertexDeclaration;

	private bool sparkle;

	private string EnvAsset;

	public OceanII(Game game, string Environment)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		m_Position = new Vector3(0f, 0f, 0f);
		myScale = Vector3.One;
		myRotation = new Quaternion(0f, 0f, 0f, 1f);
		EnvAsset = Environment;
	}

	public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		float num = 5f;
		effect = Content.Load<Effect>("Ocean");
		effect.Parameters["cubeMap"].SetValue((Texture)(object)Content.Load<TextureCube>(EnvAsset));
		effect.Parameters["normalMap"].SetValue((Texture)(object)Content.Load<Texture2D>("Sprites/waves2"));
		m_Position = new Vector3(basePosition.X - (float)myWidth * num / 2f, basePosition.Y, basePosition.Z - (float)myHeight * num / 2f);
		myVertices = new VertexMultitextured[myWidth * myHeight];
		for (int i = 0; i < myWidth; i++)
		{
			for (int j = 0; j < myHeight; j++)
			{
				myVertices[i + j * myWidth].Position = new Vector3((float)j * num, 0f, (float)i * num);
				myVertices[i + j * myWidth].Normal = new Vector3(0f, -1f, 0f);
				myVertices[i + j * myWidth].TextureCoordinate.X = (float)i / 30f;
				myVertices[i + j * myWidth].TextureCoordinate.Y = (float)j / 30f;
			}
		}
		for (int k = 0; k < myWidth; k++)
		{
			for (int l = 0; l < myHeight; l++)
			{
				if (k != 0 && k < myWidth - 1)
				{
					myVertices[k + l * myWidth].Tangent = myVertices[k - 1 + l * myWidth].Position - myVertices[k + 1 + l * myWidth].Position;
				}
				else if (k == 0)
				{
					myVertices[k + l * myWidth].Tangent = myVertices[k + l * myWidth].Position - myVertices[k + 1 + l * myWidth].Position;
				}
				else
				{
					myVertices[k + l * myWidth].Tangent = myVertices[k - 1 + l * myWidth].Position - myVertices[k + l * myWidth].Position;
				}
				if (l != 0 && l < myHeight - 1)
				{
					myVertices[k + l * myWidth].BiNormal = myVertices[k + (l - 1) * myWidth].Position - myVertices[k + (l + 1) * myWidth].Position;
				}
				else if (l == 0)
				{
					myVertices[k + l * myWidth].BiNormal = myVertices[k + l * myWidth].Position - myVertices[k + (l + 1) * myWidth].Position;
				}
				else
				{
					myVertices[k + l * myWidth].BiNormal = myVertices[k + (l - 1) * myWidth].Position - myVertices[k + l * myWidth].Position;
				}
			}
		}
		vb = new VertexBuffer(GraphicsDevice, VertexMultitextured.SizeInBytes * myWidth * myHeight, (BufferUsage)8);
		vb.SetData<VertexMultitextured>(myVertices);
		short[] array = new short[(myWidth - 1) * (myHeight - 1) * 6];
		for (short num2 = 0; num2 < myWidth - 1; num2++)
		{
			for (short num3 = 0; num3 < myHeight - 1; num3++)
			{
				array[(num2 + num3 * (myWidth - 1)) * 6] = (short)(num2 + 1 + (num3 + 1) * myWidth);
				array[(num2 + num3 * (myWidth - 1)) * 6 + 1] = (short)(num2 + 1 + num3 * myWidth);
				array[(num2 + num3 * (myWidth - 1)) * 6 + 2] = (short)(num2 + num3 * myWidth);
				array[(num2 + num3 * (myWidth - 1)) * 6 + 3] = (short)(num2 + 1 + (num3 + 1) * myWidth);
				array[(num2 + num3 * (myWidth - 1)) * 6 + 4] = (short)(num2 + num3 * myWidth);
				array[(num2 + num3 * (myWidth - 1)) * 6 + 5] = (short)(num2 + (num3 + 1) * myWidth);
			}
		}
		ib = new IndexBuffer(GraphicsDevice, typeof(short), (myWidth - 1) * (myHeight - 1) * 6, (BufferUsage)8);
		ib.SetData<short>(array);
		vertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexMultitextured.VertexElements);
	}

	public void Draw(GameTime gameTime)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		Matrix val = Matrix.CreateScale(myScale) * Matrix.CreateTranslation(new Vector3(m_Position.X, m_Position.Y, m_Position.Z));
		Matrix value = val * Program.m_CameraManager3D.m_CameraViewMatrix * Program.m_CameraManager3D.m_CameraProjectionMatrix;
		Matrix value2 = val * Program.m_CameraManager3D.m_CameraViewMatrix;
		Matrix value3 = Matrix.Invert(Program.m_CameraManager3D.m_CameraViewMatrix);
		effect.Parameters["world"].SetValue(val);
		effect.Parameters["wvp"].SetValue(value);
		effect.Parameters["worldView"].SetValue(value2);
		effect.Parameters["viewI"].SetValue(value3);
		effect.Parameters["sparkle"].SetValue(sparkle);
		effect.Parameters["deepColor"].SetValue(((Color)(ref myDeepWater)).ToVector4());
		effect.Parameters["shallowColor"].SetValue(((Color)(ref myShallowWater)).ToVector4());
		effect.Parameters["reflectionColor"].SetValue(((Color)(ref myReflection)).ToVector4());
		effect.Parameters["bumpHeight"].SetValue(myBumpHeight);
		effect.Parameters["hdrMultiplier"].SetValue(myHDRMult);
		effect.Parameters["reflectionAmount"].SetValue(myReflectionAmt);
		effect.Parameters["waterAmount"].SetValue(myWaterColorAmount);
		effect.Parameters["waveAmp"].SetValue(myWaveAmplitude);
		effect.Parameters["waveFreq"].SetValue(myWaveFrequency);
		((Game)Program.m_App).GraphicsDevice.VertexDeclaration = vertexDeclaration;
		((Game)Program.m_App).GraphicsDevice.Vertices[0].SetSource(vb, 0, VertexMultitextured.SizeInBytes);
		((Game)Program.m_App).GraphicsDevice.Indices = ib;
		((Game)Program.m_App).GraphicsDevice.RenderState.CullMode = (CullMode)1;
		GraphicsDevice graphicsDevice = ((Game)Program.m_App).GraphicsDevice;
		RenderState renderState = ((Game)Program.m_App).GraphicsDevice.RenderState;
		renderState.AlphaBlendEnable = false;
		renderState.AlphaTestEnable = false;
		renderState.DepthBufferEnable = true;
		graphicsDevice.SamplerStates[0].AddressU = (TextureAddressMode)1;
		graphicsDevice.SamplerStates[0].AddressV = (TextureAddressMode)1;
		graphicsDevice.SamplerStates[0].MagFilter = (TextureFilter)1;
		graphicsDevice.SamplerStates[0].MinFilter = (TextureFilter)1;
		graphicsDevice.SamplerStates[0].MipFilter = (TextureFilter)1;
		graphicsDevice.SamplerStates[1].AddressU = (TextureAddressMode)1;
		graphicsDevice.SamplerStates[1].AddressV = (TextureAddressMode)1;
		effect.Begin();
		for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
		{
			effect.CurrentTechnique.Passes[i].Begin();
			((Game)Program.m_App).GraphicsDevice.DrawIndexedPrimitives((PrimitiveType)4, 0, 0, myWidth * myHeight, 0, (myWidth - 1) * (myHeight - 1) * 2);
			effect.CurrentTechnique.Passes[i].End();
		}
		effect.End();
		((Game)Program.m_App).GraphicsDevice.RenderState.CullMode = (CullMode)3;
	}

	public void Update(GameTime gameTime)
	{
		effect.Parameters["time"].SetValue((float)gameTime.TotalRealTime.TotalSeconds);
		ref Vector3 position = ref m_Position;
		position.X -= Program.m_App.m_ScrollSpeed;
		m_Position.Z = -120f;
		if (m_Position.X < -220f)
		{
			m_Position.X = -100f;
		}
	}
}
