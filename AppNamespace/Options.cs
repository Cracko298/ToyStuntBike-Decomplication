using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AppNamespace;

public class Options
{
	public const int DIFFICULTY_EASY = 1;

	public const int DIFFICULTY_NORMAL = 2;

	public const int DIFFICULTY_HARD = 3;

	public const int DIFFICULTY_CRAZY = 4;

	public const float SUBMENU_Y_OFFSET = 105f;

	private App m_App;

	public App.TEXTID m_SubMenuState;

	private Vector2 m_SubMenuPosition = Vector2.Zero;

	private Vector2 m_PrevSubMenuPosition = Vector2.Zero;

	private float m_MenuMoveTime;

	public float m_ValueRepeatTime;

	public int m_MusicVol;

	public int m_SFXVol;

	public bool m_bVibration = true;

	public int m_Difficulty = 2;

	private int m_MusicVolCopy;

	private int m_SFXVolCopy;

	private bool m_bVibrationCopy;

	public static Vector2 OPTIONS_TOP = new Vector2(390f, 200f);

	public static Vector2 OPTIONS_OFFSET = new Vector2(0f, 60f);

	public static Vector2 OPTIONS_TOP_OFFSET = new Vector2(-80f, OPTIONS_TOP.Y - 110f);

	private float[] m_aOptionsSilkWidth = new float[4] { 470f, 470f, 380f, 370f };

	public Options(App app)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		m_App = app;
		m_SubMenuState = App.TEXTID.MUSIC_VOL;
		m_MusicVol = 50;
		m_SFXVol = 90;
	}

	public void Start()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		m_SubMenuState = App.TEXTID.MUSIC_VOL;
		m_SubMenuPosition = OPTIONS_TOP + (float)(m_SubMenuState - 20) * OPTIONS_OFFSET;
		m_PrevSubMenuPosition = Vector2.Zero;
		m_MusicVolCopy = m_MusicVol;
		m_SFXVolCopy = m_SFXVol;
		m_bVibrationCopy = m_bVibration;
		Program.m_App.m_bScroll = true;
		Program.m_App.m_ScrollSpeed = 0.04f;
		Program.m_CameraManager3D.m_State = CameraManager3D.State.INGAME;
		Program.m_CameraManager3D.m_CameraPositionTarget = new Vector3(0f, 10f, 19.8f);
		Program.m_CameraManager3D.m_CameraLookAtTarget = new Vector3(0f, 10f, -19.8f);
		Program.m_App.m_Fog = new Vector3(0.6627451f, 62f / 85f, 47f / 51f);
	}

	public void Stop()
	{
	}

	public void Update()
	{
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Invalid comparison between Unknown and I4
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Invalid comparison between Unknown and I4
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Invalid comparison between Unknown and I4
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Invalid comparison between Unknown and I4
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
			{
				OnSelectPressed();
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192) && !m_App.Fading())
			{
				OnBackPressed();
				return;
			}
			GamePadThumbSticks thumbSticks = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
			float num = ((GamePadThumbSticks)(ref thumbSticks)).Left.Y;
			if (Math.Abs(num) < 0.25f)
			{
				num = 0f;
			}
			GamePadDPad dPad = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad)).Up == 1)
			{
				num = 1f;
			}
			GamePadDPad dPad2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad2)).Down == 1)
			{
				num = -1f;
			}
			GamePadThumbSticks thumbSticks2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
			float num2 = ((GamePadThumbSticks)(ref thumbSticks2)).Left.X;
			if (Math.Abs(num2) < 0.25f)
			{
				num2 = 0f;
			}
			GamePadDPad dPad3 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad3)).Right == 1)
			{
				num2 = 1f;
			}
			GamePadDPad dPad4 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad4)).Left == 1)
			{
				num2 = -1f;
			}
			if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (num < -0.9f && m_SubMenuState < App.TEXTID.CREDITS)
				{
					m_SubMenuState++;
					m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
					m_SubMenuPosition = OPTIONS_TOP + (float)(m_SubMenuState - 20) * OPTIONS_OFFSET;
					Program.m_SoundManager.Play(1);
				}
				if (num > 0.9f && m_SubMenuState > App.TEXTID.MUSIC_VOL)
				{
					m_SubMenuState--;
					m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
					m_SubMenuPosition = OPTIONS_TOP + (float)(m_SubMenuState - 20) * OPTIONS_OFFSET;
					Program.m_SoundManager.Play(0);
				}
			}
			if (m_ValueRepeatTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (num2 < -0.9f)
				{
					if (m_SubMenuState == App.TEXTID.MUSIC_VOL)
					{
						if (m_MusicVol > 0)
						{
							m_MusicVol--;
						}
						m_ValueRepeatTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 40f;
						MediaPlayer.Volume = (float)m_MusicVol / 100f;
					}
					if (m_SubMenuState == App.TEXTID.SFX_VOL)
					{
						if (m_SFXVol > 0)
						{
							m_SFXVol--;
							Program.m_SoundManager.Play(1);
						}
						m_ValueRepeatTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 40f;
						SoundEffect.MasterVolume = (float)m_SFXVol / 100f;
					}
					if (m_SubMenuState == App.TEXTID.VIBRATION)
					{
						m_bVibration = !m_bVibration;
						m_ValueRepeatTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 500f;
						Program.m_SoundManager.Play(1);
					}
				}
				if (num2 > 0.9f)
				{
					if (m_SubMenuState == App.TEXTID.MUSIC_VOL)
					{
						if (m_MusicVol < 100)
						{
							m_MusicVol++;
						}
						m_ValueRepeatTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 40f;
						MediaPlayer.Volume = (float)m_MusicVol / 100f;
					}
					if (m_SubMenuState == App.TEXTID.SFX_VOL)
					{
						if (m_SFXVol < 100)
						{
							m_SFXVol++;
							Program.m_SoundManager.Play(1);
						}
						m_ValueRepeatTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 40f;
						SoundEffect.MasterVolume = (float)m_SFXVol / 100f;
					}
					if (m_SubMenuState == App.TEXTID.VIBRATION)
					{
						m_bVibration = !m_bVibration;
						m_ValueRepeatTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 500f;
						Program.m_SoundManager.Play(1);
					}
				}
			}
			if (m_SubMenuPosition != m_PrevSubMenuPosition)
			{
				CreateEmitters();
			}
			m_PrevSubMenuPosition = m_SubMenuPosition;
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	private void OnSelectPressed()
	{
		if (!m_App.Fading())
		{
			if (m_SubMenuState == App.TEXTID.HELP)
			{
				m_App.m_NextState = App.STATE.HELP;
				m_App.StartFade(up: false);
			}
			else if (m_SubMenuState == App.TEXTID.CREDITS)
			{
				m_App.m_NextState = App.STATE.CREDITS;
				m_App.StartFade(up: false);
			}
			else
			{
				m_App.m_NextState = App.STATE.MAINMENU;
				m_App.StartFade(up: false);
			}
			MediaPlayer.Volume = (float)m_MusicVol / 100f;
			SoundEffect.MasterVolume = (float)m_SFXVol / 100f;
			Program.m_SoundManager.Play(2);
			if (Program.m_App.m_bSaveExists)
			{
				Program.m_LoadSaveManager.SaveGame();
			}
		}
	}

	private void OnBackPressed()
	{
		m_App.m_NextState = App.STATE.MAINMENU;
		m_App.StartFade(up: false);
		m_MusicVol = m_MusicVolCopy;
		m_SFXVol = m_SFXVolCopy;
		m_bVibration = m_bVibrationCopy;
		MediaPlayer.Volume = (float)m_MusicVol / 100f;
		SoundEffect.MasterVolume = (float)m_SFXVol / 100f;
		Program.m_SoundManager.Play(3);
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		Vector2 oPTIONS_TOP = OPTIONS_TOP;
		m_App.m_SpriteBatch.Begin();
		m_App.m_MenuText.mColor = m_App.m_FrontEnd.TITLE_COL;
		m_App.m_MenuText.Position = oPTIONS_TOP - OPTIONS_TOP_OFFSET;
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.OPTIONS_TITLE);
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.MUSIC_VOL);
		m_App.m_MenuText.Position = oPTIONS_TOP;
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.MUSIC_VOL);
		m_App.m_MenuText.mText = string.Format(m_App.m_MenuText.mText + "{0}", m_MusicVol);
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		oPTIONS_TOP += OPTIONS_OFFSET;
		GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.SFX_VOL);
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.SFX_VOL);
		m_App.m_MenuText.mText = string.Format(m_App.m_MenuText.mText + "{0}", m_SFXVol);
		m_App.m_MenuText.Position = oPTIONS_TOP;
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		oPTIONS_TOP += OPTIONS_OFFSET;
		GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.VIBRATION);
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.VIBRATION);
		if (m_bVibration)
		{
			m_App.m_MenuText.mText = string.Format(m_App.m_MenuText.mText + m_App.GetText(App.TEXTID.ON));
		}
		else
		{
			m_App.m_MenuText.mText = string.Format(m_App.m_MenuText.mText + m_App.GetText(App.TEXTID.OFF));
		}
		m_App.m_MenuText.Position = oPTIONS_TOP;
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		oPTIONS_TOP += OPTIONS_OFFSET;
		GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.HELP);
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.HELP);
		m_App.m_MenuText.Position = oPTIONS_TOP;
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		oPTIONS_TOP += OPTIONS_OFFSET;
		GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.CREDITS);
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.CREDITS);
		m_App.m_MenuText.Position = oPTIONS_TOP;
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		oPTIONS_TOP.X = 120f;
		oPTIONS_TOP.Y = 545f;
		if (m_SubMenuState >= App.TEXTID.MUSIC_VOL && m_SubMenuState <= App.TEXTID.VIBRATION)
		{
			m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SAVE);
		}
		else
		{
			m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SELECT);
		}
		m_App.m_GameText.Position = oPTIONS_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		oPTIONS_TOP.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.CANCEL);
		m_App.m_GameText.Position = oPTIONS_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		Program.m_App.DrawMenuPlane(m_SubMenuPosition);
		m_App.m_SpriteBatch.End();
		m_App.RenderLines();
	}

	public void CreateEmitters()
	{
	}

	public void GetMenuColour(out Color c, App.TEXTID highlightedId)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (highlightedId == m_SubMenuState)
		{
			c = m_App.m_FrontEnd.HIGHLIGHT_COL;
		}
		else
		{
			c = m_App.m_FrontEnd.LOWLIGHT_COL;
		}
	}
}
