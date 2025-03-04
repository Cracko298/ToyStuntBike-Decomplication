using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppNamespace;

public class BloomManager
{
	public enum IntermediateBuffer
	{
		PreBloom,
		BlurredHorizontally,
		BlurredBothWays,
		FinalResult
	}

	private const int sampleCount = 15;

	private const float FLY_EFFECT_TIME = 2f;

	private Effect bloomExtractEffect;

	private Effect bloomCombineEffect;

	private Effect gaussianBlurEffect;

	private ResolveTexture2D resolveTarget;

	private RenderTarget2D renderTarget1;

	private RenderTarget2D renderTarget2;

	private bool m_FlyCaughtEffect;

	private float m_FlyCaughEffectTime;

	private float[] sampleWeights = new float[15];

	private Vector2[] sampleOffsets = (Vector2[])(object)new Vector2[15];

	public BloomSettings settings = BloomSettings.PresetSettings[0];

	private IntermediateBuffer showBuffer = IntermediateBuffer.FinalResult;

	public BloomSettings Settings
	{
		get
		{
			return settings;
		}
		set
		{
			settings = value;
		}
	}

	public IntermediateBuffer ShowBuffer
	{
		get
		{
			return showBuffer;
		}
		set
		{
			showBuffer = value;
		}
	}

	public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		bloomExtractEffect = Content.Load<Effect>("BloomExtract");
		bloomCombineEffect = Content.Load<Effect>("BloomCombine");
		gaussianBlurEffect = Content.Load<Effect>("GaussianBlur");
		PresentationParameters presentationParameters = GraphicsDevice.PresentationParameters;
		int backBufferWidth = presentationParameters.BackBufferWidth;
		int backBufferHeight = presentationParameters.BackBufferHeight;
		SurfaceFormat backBufferFormat = presentationParameters.BackBufferFormat;
		resolveTarget = new ResolveTexture2D(GraphicsDevice, backBufferWidth, backBufferHeight, 1, backBufferFormat);
		backBufferWidth /= 2;
		backBufferHeight /= 2;
		renderTarget1 = new RenderTarget2D(GraphicsDevice, backBufferWidth, backBufferHeight, 1, backBufferFormat, GraphicsDevice.PresentationParameters.MultiSampleType, GraphicsDevice.PresentationParameters.MultiSampleQuality);
		renderTarget2 = new RenderTarget2D(GraphicsDevice, backBufferWidth, backBufferHeight, 1, backBufferFormat, GraphicsDevice.PresentationParameters.MultiSampleType, GraphicsDevice.PresentationParameters.MultiSampleQuality);
	}

	public void StartCaughtFlyEffect()
	{
		m_FlyCaughtEffect = true;
		m_FlyCaughEffectTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalSeconds + 2f;
	}

	public void UpdateEffects()
	{
		if (Program.m_App.m_Paused)
		{
			m_FlyCaughEffectTime += (float)Program.m_App.m_GameTime.ElapsedGameTime.TotalSeconds;
		}
		if (!m_FlyCaughtEffect)
		{
			return;
		}
		float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalSeconds;
		if (m_FlyCaughEffectTime < num)
		{
			m_FlyCaughtEffect = false;
			settings.BlurAmount = BloomSettings.PresetSettings[1].BlurAmount;
			settings.BloomThreshold = BloomSettings.PresetSettings[1].BloomThreshold;
			return;
		}
		float num2 = 2f - (m_FlyCaughEffectTime - num);
		float num3 = num2 / 2f;
		float num4 = (float)Math.Sin((double)num3 * Math.PI * 2.0) + 1f;
		if (num4 > 2f)
		{
			num4 = 2f;
		}
		if (num4 < 1f)
		{
			num4 = 1f;
		}
		settings.BlurAmount = BloomSettings.PresetSettings[1].BlurAmount * num4;
		float num5 = (float)Math.Cos((double)num3 * Math.PI * 2.0);
		if (num5 > 1f)
		{
			num5 = 1f;
		}
		if (num5 < -1f)
		{
			num5 = -1f;
		}
		settings.BloomThreshold = BloomSettings.PresetSettings[1].BloomThreshold * num5;
	}

	public void Bloom()
	{
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		UpdateEffects();
		((Game)Program.m_App).GraphicsDevice.ResolveBackBuffer(resolveTarget);
		bloomExtractEffect.Parameters["BloomThreshold"].SetValue(Settings.BloomThreshold);
		DrawFullscreenQuad((Texture2D)(object)resolveTarget, renderTarget1, bloomExtractEffect, IntermediateBuffer.PreBloom);
		SetBlurEffectParameters(1f / (float)((RenderTarget)renderTarget1).Width, 0f);
		DrawFullscreenQuad(renderTarget1.GetTexture(), renderTarget2, gaussianBlurEffect, IntermediateBuffer.BlurredHorizontally);
		SetBlurEffectParameters(0f, 1f / (float)((RenderTarget)renderTarget1).Height);
		DrawFullscreenQuad(renderTarget2.GetTexture(), renderTarget1, gaussianBlurEffect, IntermediateBuffer.BlurredBothWays);
		((Game)Program.m_App).GraphicsDevice.SetRenderTarget(0, (RenderTarget2D)null);
		EffectParameterCollection parameters = bloomCombineEffect.Parameters;
		parameters["BloomIntensity"].SetValue(Settings.BloomIntensity);
		parameters["BaseIntensity"].SetValue(Settings.BaseIntensity);
		parameters["BloomSaturation"].SetValue(Settings.BloomSaturation);
		parameters["BaseSaturation"].SetValue(Settings.BaseSaturation);
		((Game)Program.m_App).GraphicsDevice.Textures[1] = (Texture)(object)resolveTarget;
		Viewport viewport = ((Game)Program.m_App).GraphicsDevice.Viewport;
		DrawFullscreenQuad(renderTarget1.GetTexture(), ((Viewport)(ref viewport)).Width, ((Viewport)(ref viewport)).Height, bloomCombineEffect, IntermediateBuffer.FinalResult);
	}

	private void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect, IntermediateBuffer currentBuffer)
	{
		((Game)Program.m_App).GraphicsDevice.SetRenderTarget(0, renderTarget);
		DrawFullscreenQuad(texture, ((RenderTarget)renderTarget).Width, ((RenderTarget)renderTarget).Height, effect, currentBuffer);
		((Game)Program.m_App).GraphicsDevice.SetRenderTarget(0, (RenderTarget2D)null);
	}

	private void DrawFullscreenQuad(Texture2D texture, int width, int height, Effect effect, IntermediateBuffer currentBuffer)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Program.m_App.m_SpriteBatch.Begin((SpriteBlendMode)0, (SpriteSortMode)0, (SaveStateMode)0);
		if (showBuffer >= currentBuffer)
		{
			effect.Begin();
			effect.CurrentTechnique.Passes[0].Begin();
		}
		Program.m_App.m_SpriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
		Program.m_App.m_SpriteBatch.End();
		if (showBuffer >= currentBuffer)
		{
			effect.CurrentTechnique.Passes[0].End();
			effect.End();
		}
	}

	private void SetBlurEffectParameters(float dx, float dy)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		EffectParameter val = gaussianBlurEffect.Parameters["SampleWeights"];
		EffectParameter val2 = gaussianBlurEffect.Parameters["SampleOffsets"];
		sampleWeights[0] = ComputeGaussian(0f);
		ref Vector2 reference = ref sampleOffsets[0];
		reference = Vector2.Zero;
		float num = sampleWeights[0];
		for (int i = 0; i < 7; i++)
		{
			float num2 = ComputeGaussian(i + 1);
			sampleWeights[i * 2 + 1] = num2;
			sampleWeights[i * 2 + 2] = num2;
			num += num2 * 2f;
			float num3 = (float)(i * 2) + 1.5f;
			Vector2 val3 = new Vector2(dx, dy) * num3;
			sampleOffsets[i * 2 + 1] = val3;
			ref Vector2 reference2 = ref sampleOffsets[i * 2 + 2];
			reference2 = -val3;
		}
		for (int j = 0; j < sampleWeights.Length; j++)
		{
			sampleWeights[j] /= num;
		}
		val.SetValue(sampleWeights);
		val2.SetValue(sampleOffsets);
	}

	private float ComputeGaussian(float n)
	{
		float blurAmount = Settings.BlurAmount;
		return (float)(1.0 / Math.Sqrt(Math.PI * 2.0 * (double)blurAmount) * Math.Exp((0f - n * n) / (2f * blurAmount * blurAmount)));
	}
}
