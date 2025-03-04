using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class Map
{
	public const float MENU_REPEAT_TIME = 250f;

	private App m_App;

	public int m_LastLevelReached;

	private int m_NumFrames;

	private int m_LevelSelected;

	private float m_MenuMoveTime;

	private int m_ShowSave;

	private float m_MapUnlockTime;

	private bool m_bFlash;

	private float m_FlashTime;

	private int m_FlashLevel = -1;

	private int m_FlashLevel2 = -1;

	private int m_FlashLevel3 = -1;

	public static Vector2 TEXT_TOP = new Vector2(450f, 50f);

	private int[] MapCoords = new int[22]
	{
		386, 225, 525, 221, 679, 281, 841, 273, 911, 447,
		689, 421, 549, 435, 417, 429, 291, 445, 583, 591,
		890, 620
	};

	public static float[] CupTargets = new float[20]
	{
		20000f, 36000f, 27500f, 65000f, 32500f, 55000f, 37600f, 95000f, 33500f, 43000f,
		39800f, 43000f, 37300f, 46000f, 40300f, 57000f, 38600f, 86000f, 50000f, 30000f
	};

	private float[] m_aButtonX = new float[20]
	{
		386f, 931f, 915f, 787f, 599f, 469f, 435f, 491f, 869f, 857f,
		731f, 605f, 505f, 485f, 523f, 747f, 753f, 673f, 559f, 625f
	};

	private float[] m_aButtonY = new float[20]
	{
		237f, 155f, 415f, 575f, 605f, 503f, 383f, 191f, 205f, 409f,
		505f, 533f, 471f, 385f, 243f, 303f, 403f, 449f, 391f, 393f
	};

	private static Vector2 logoOffset = new Vector2(-8f, -8f);

	private static Vector2 shark1Offset = new Vector2(40f, 0f);

	private static Vector2 shark2Offset = new Vector2(50f, 20f);

	private static Vector2 shark3Offset = new Vector2(50f, -20f);

	private static Vector2 playerOffset = new Vector2(-25f, -70f);

	private static Vector2 weevilOffset = new Vector2(70f, -30f);

	private static Vector2 medalOffset = new Vector2(-30f, 0f);

	public Map(App app)
	{
		m_App = app;
	}

	public void Start()
	{
		Program.m_App.m_LevelReached = Program.m_ItemManager.TotalCupsCollected() + 1;
		if (Program.m_App.m_LevelReached > 10)
		{
			Program.m_App.m_LevelReached = 10;
		}
		if (Guide.IsTrialMode && Program.m_App.m_LevelReached > 3)
		{
			Program.m_App.m_LevelReached = 3;
		}
		int num = Program.m_App.m_Level;
		if (num > Program.m_App.m_LevelReached)
		{
			num = Program.m_App.m_LevelReached;
		}
		if (num > 10)
		{
			num = 10;
		}
		if (Program.m_App.m_Level <= 10)
		{
			m_LevelSelected = num - 1;
		}
		else
		{
			m_LevelSelected = 0;
		}
		m_NumFrames = 0;
		m_ShowSave = 120;
		Program.m_LoadSaveManager.SaveGame();
		Program.m_PlayerManager.m_Player[0].m_State = Player.State.WAITING_AT_START;
		m_FlashLevel = -1;
		m_FlashLevel2 = -1;
		m_FlashLevel3 = -1;
	}

	public void Stop()
	{
		Program.m_PlayerManager.m_Player[0].m_State = Player.State.WAITING_AT_START;
	}

	public void Update()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			UpdateMapScreen();
		}
	}

	private void UpdateMapScreen()
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Invalid comparison between Unknown and I4
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Invalid comparison between Unknown and I4
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Invalid comparison between Unknown and I4
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Invalid comparison between Unknown and I4
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		m_NumFrames++;
		if (m_NumFrames == 50)
		{
			for (int num = Program.m_App.m_LevelReached; num > m_LastLevelReached; num--)
			{
				if (Guide.IsTrialMode)
				{
					if (num < 4)
					{
						DoUnlockEffect(num - 1);
					}
				}
				else
				{
					DoUnlockEffect(num - 1);
				}
			}
			m_LastLevelReached = Program.m_App.m_LevelReached;
		}
		for (int i = 0; i < 3; i++)
		{
			Program.m_ParticleManager.Update();
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
		{
			m_LastLevelReached = Program.m_App.m_LevelReached;
			OnSelectPressed();
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192))
		{
			OnBackPressed();
		}
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
		float num2 = ((GamePadThumbSticks)(ref thumbSticks)).Left.Y;
		if (Math.Abs(num2) < 0.25f)
		{
			num2 = 0f;
		}
		GamePadDPad dPad = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad)).Up == 1)
		{
			num2 = 1f;
		}
		GamePadDPad dPad2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad2)).Down == 1)
		{
			num2 = -1f;
		}
		GamePadThumbSticks thumbSticks2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
		float num3 = ((GamePadThumbSticks)(ref thumbSticks2)).Left.X;
		if (Math.Abs(num3) < 0.25f)
		{
			num3 = 0f;
		}
		GamePadDPad dPad3 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad3)).Left == 1)
		{
			num3 = -1f;
		}
		GamePadDPad dPad4 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad4)).Right == 1)
		{
			num3 = 1f;
		}
		if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			Vector2 zero = Vector2.Zero;
			zero.X = num3;
			zero.Y = 0f - num2;
			((Vector2)(ref zero)).Normalize();
			int levelSelected = m_LevelSelected;
			m_LevelSelected = FindNearest(zero);
			m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
			if (levelSelected != m_LevelSelected)
			{
				Program.m_SoundManager.Play(4);
			}
		}
		Program.m_PlayerManager.GetPrimaryPlayer().CheckJukeboxControls();
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	private int FindNearest(Vector2 vDir)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.Zero;
		zero.X = MapCoords[m_LevelSelected * 2];
		zero.Y = MapCoords[m_LevelSelected * 2 + 1];
		Vector2 zero2 = Vector2.Zero;
		Vector2 val = Vector2.Zero;
		float num = 0f;
		float num2 = 999999f;
		int num3 = -1;
		int num4 = Program.m_App.m_LevelReached;
		if (num4 > 10)
		{
			num4 = 10;
		}
		if (Program.m_ItemManager.TotalCupsCollected() == 30)
		{
			num4 = 11;
		}
		for (int i = 0; i < num4; i++)
		{
			if (m_LevelSelected == i)
			{
				continue;
			}
			zero2.X = MapCoords[i * 2];
			zero2.Y = MapCoords[i * 2 + 1];
			val = zero2 - zero;
			num = ((Vector2)(ref val)).LengthSquared();
			((Vector2)(ref val)).Normalize();
			float num5 = Vector2.Dot(val, vDir);
			if (num5 > 0.707f)
			{
				float num6 = 1f;
				num6 = ((!(num5 > 0.97f)) ? 1f : 0.8f);
				if (num * num6 < num2)
				{
					num2 = num * num6;
					num3 = i;
				}
			}
		}
		if (num3 == -1)
		{
			return m_LevelSelected;
		}
		return num3;
	}

	private void OnSelectPressed()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		if (!m_App.Fading())
		{
			m_App.m_Level = m_LevelSelected + 1;
			m_App.m_NextState = App.STATE.CONTINUEGAME;
			m_App.StartFade(up: false);
			Program.m_PlayerManager.GetPrimaryPlayer().ResetBike(bForce: true, restartRace: true, Vector2.Zero);
			Program.m_PlayerManager.GetPrimaryPlayer().m_ReadySteadyTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 4500f;
			Program.m_SoundManager.Play(2);
		}
	}

	private void OnBackPressed()
	{
		if (!m_App.Fading())
		{
			m_App.m_NextState = App.STATE.MAINMENU;
			m_App.StartFade(up: false);
			Program.m_SoundManager.Play(3);
		}
	}

	public void DoUnlockEffect(int buttonId)
	{
		m_MapUnlockTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalSeconds + 3f;
		if (m_FlashLevel == -1)
		{
			m_FlashLevel = buttonId;
		}
		else if (m_FlashLevel2 == -1)
		{
			m_FlashLevel2 = buttonId;
		}
		else if (m_FlashLevel3 == -1)
		{
			m_FlashLevel3 = buttonId;
		}
		Program.m_SoundManager.Play(29);
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09da: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MapBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		m_App.m_SpriteBatch.Begin();
		int num = Program.m_App.m_LevelReached;
		Vector2 val = default(Vector2);
		Color val2 = default(Color);
		for (int i = 0; i < 10; i++)
		{
			string arg = (i + 1).ToString();
			((Vector2)(ref val))._002Ector((float)MapCoords[i * 2], (float)MapCoords[i * 2 + 1]);
			if (i >= num)
			{
				((Color)(ref val2))._002Ector((byte)64, (byte)64, (byte)64, (byte)32);
			}
			else
			{
				((Color)(ref val2))._002Ector((byte)64, (byte)64, (byte)64, byte.MaxValue);
			}
			Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, $"{arg}.", val, val2);
			if (Program.m_ItemManager.GetTimeCupOnLevel(i) == 1)
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(25f, -15f), Color.White);
			}
			else
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(25f, -15f), new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)32));
			}
			if (Program.m_ItemManager.GetScoreCupOnLevel(i) == 1)
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(45f, -15f), Color.White);
			}
			else
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(45f, -15f), new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)32));
			}
			if (Program.m_ItemManager.GetFlagsCupOnLevel(i) == 1)
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(65f, -15f), Color.White);
			}
			else
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(65f, -15f), new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)32));
			}
			if (num > 10)
			{
				num = 10;
			}
			_ = num - 1;
		}
		if (m_MapUnlockTime > (float)Program.m_App.m_GameTime.TotalGameTime.TotalSeconds)
		{
			Vector2 zero = Vector2.Zero;
			if (m_FlashLevel != -1)
			{
				((Vector2)(ref zero))._002Ector((float)MapCoords[m_FlashLevel * 2], (float)MapCoords[m_FlashLevel * 2 + 1]);
				if (m_bFlash)
				{
					Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, $"{(m_FlashLevel + 1).ToString()}.", zero, Color.LightGoldenrodYellow);
				}
			}
			if (m_FlashLevel2 != -1)
			{
				((Vector2)(ref zero))._002Ector((float)MapCoords[m_FlashLevel2 * 2], (float)MapCoords[m_FlashLevel2 * 2 + 1]);
				if (m_bFlash)
				{
					Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, $"{(m_FlashLevel2 + 1).ToString()}.", zero, Color.LightGoldenrodYellow);
				}
			}
			if (m_FlashLevel3 != -1)
			{
				((Vector2)(ref zero))._002Ector((float)MapCoords[m_FlashLevel3 * 2], (float)MapCoords[m_FlashLevel3 * 2 + 1]);
				if (m_bFlash)
				{
					Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, $"{(m_FlashLevel3 + 1).ToString()}.", zero, Color.LightGoldenrodYellow);
				}
			}
			if (m_FlashTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				m_bFlash = !m_bFlash;
				m_FlashTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 100f;
			}
		}
		if (Program.m_ItemManager.TotalCupsCollected() == 30)
		{
			m_App.m_SpriteBatch.Draw(m_App.m_GoldFlagTexture, new Vector2(841f, 517f), Color.White);
			Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, "Champion!", new Vector2(820f, 618f), new Color((byte)64, (byte)64, (byte)64, byte.MaxValue));
		}
		Vector2 val3 = default(Vector2);
		((Vector2)(ref val3))._002Ector((float)MapCoords[m_LevelSelected * 2], (float)MapCoords[m_LevelSelected * 2 + 1]);
		Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_MenuPlaneTexture, val3 + playerOffset, Color.White);
		Vector2 val4 = TEXT_TOP;
		m_App.m_MenuText.mColor = m_App.m_FrontEnd.TITLE_COL;
		m_App.m_MenuText.Position = val4;
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.SELECT_LEVEL);
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		Vector2 val5 = default(Vector2);
		((Vector2)(ref val5))._002Ector(120f, 50f);
		new Vector2(0f, 120f);
		val4 = val5;
		if (m_ShowSave > 0 && !Guide.IsTrialMode)
		{
			m_ShowSave--;
			val4.X = 500f;
			val4.Y = 600f;
			m_App.m_GameText.Position = val4;
			if (m_App.m_SaveErrorNoSpace)
			{
				m_App.m_GameText.Position.X = 480f;
				m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SAVE_ERROR_NO_SPACE);
			}
			else if (m_App.m_SaveError)
			{
				m_App.m_GameText.Position.X = 480f;
				m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SAVE_ERROR);
			}
			else
			{
				m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SAVING);
			}
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		}
		val4.X = 120f;
		val4.Y = 545f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SELECT);
		m_App.m_GameText.Position = val4;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		val4.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.BACK);
		m_App.m_GameText.Position = val4;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		if (m_LevelSelected < 10)
		{
			Color val6 = default(Color);
			((Color)(ref val6))._002Ector((byte)200, byte.MaxValue, (byte)50);
			((Vector2)(ref val4))._002Ector(960f, 60f);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Track", val4, val6);
			val4.Y += 35f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Targets", val4, val6);
			val4.Y += 50f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Time", val4, val6);
			val4.Y += 35f;
			int num2 = (int)(CupTargets[m_LevelSelected * 2] / 60000f);
			int num3 = (int)(CupTargets[m_LevelSelected * 2] % 60000f / 1000f);
			int num4 = (int)(CupTargets[m_LevelSelected * 2] % 1000f);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{num2:d2}:{num3:d2}:{num4:d3}", val4, val6);
			val4.Y += 50f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Score", val4, val6);
			val4.Y += 35f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{CupTargets[m_LevelSelected * 2 + 1]}", val4, val6);
			val4.Y += 50f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Flags", val4, val6);
			val4.Y += 35f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "3", val4, val6);
		}
		((Vector2)(ref val4))._002Ector(940f, 540f);
		m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTextureMed, val4 + new Vector2(130f, -30f), Color.White);
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, $"{Program.m_ItemManager.TotalCupsCollected()}/30", val4, new Color(byte.MaxValue, byte.MaxValue, (byte)100, byte.MaxValue));
		if (m_LevelSelected < 10)
		{
			Color val7 = default(Color);
			((Color)(ref val7))._002Ector(byte.MaxValue, (byte)200, (byte)0, byte.MaxValue);
			Color val8 = default(Color);
			((Color)(ref val8))._002Ector(byte.MaxValue, (byte)230, (byte)0, byte.MaxValue);
			Color val9 = default(Color);
			((Color)(ref val9))._002Ector(byte.MaxValue, (byte)150, (byte)0, byte.MaxValue);
			((Vector2)(ref val4))._002Ector(120f, 60f);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Track", val4, val7);
			val4.Y += 35f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Status", val4, val7);
			val4.Y += 50f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Time", val4, val7);
			val4.Y += 35f;
			if (Program.m_ItemManager.GetTimeCupOnLevel(m_LevelSelected) == 1)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Complete", val4, val8);
			}
			else
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "No", val4, val9);
			}
			val4.Y += 50f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Score", val4, val7);
			val4.Y += 35f;
			if (Program.m_ItemManager.GetScoreCupOnLevel(m_LevelSelected) == 1)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Complete", val4, val8);
			}
			else
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "No", val4, val9);
			}
			val4.Y += 50f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Flags", val4, val7);
			val4.Y += 35f;
			if (Program.m_ItemManager.GetFlagsCupOnLevel(m_LevelSelected) == 1)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Complete", val4, val8);
			}
			else
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{Program.m_ItemManager.GetNumFlagsCollectedOnLevel(m_LevelSelected)}/3", val4, val9);
			}
		}
		m_App.RenderLines();
		m_App.m_SpriteBatch.End();
	}
}
