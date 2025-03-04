using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class FrontEndScores
{
	public const float SUBMENU_Y_OFFSET = 105f;

	private App m_App;

	public App.TEXTID m_SubMenuState;

	private Vector2 m_SubMenuPosition = Vector2.Zero;

	private Vector2 m_PrevSubMenuPosition = Vector2.Zero;

	public float m_ValueRepeatTime;

	private int m_Track;

	private int m_NumPages;

	private float m_MenuMoveTime;

	private TopScoreEntry[] page = new TopScoreEntry[10];

	private int pageIndex;

	public static Vector2 FRONTENDSCORES_TOP = new Vector2(300f, 120f);

	public static Vector2 FRONTENDSCORES_OFFSET = new Vector2(0f, 60f);

	public static Vector2 FRONTENDSCORES_TOP_OFFSET = new Vector2(-100f, FRONTENDSCORES_TOP.Y - 40f);

	public FrontEndScores(App app)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		m_App = app;
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		m_SubMenuState = App.TEXTID.MUSIC_VOL;
		m_SubMenuPosition = FRONTENDSCORES_TOP + (float)(m_SubMenuState - 20) * FRONTENDSCORES_OFFSET;
		m_PrevSubMenuPosition = Vector2.Zero;
		pageIndex = 0;
		if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
		{
			pageIndex = Program.m_App.mScores.fillPageThatContainsGamertagFromFullList(m_Track, page, ((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag);
		}
		else
		{
			Program.m_App.mScores.fillPageFromFullList(m_Track, pageIndex, page);
		}
		m_NumPages = (Program.m_App.mScores.getFullListSize(m_Track) - 1) / 10;
		Program.m_App.m_MenuIdleTime = 0f;
	}

	public void Stop()
	{
	}

	public void Update()
	{
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096) && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
			{
				pageIndex = Program.m_App.mScores.fillPageThatContainsGamertagFromFullList(m_Track, page, ((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag);
				Program.m_SoundManager.Play(2);
				Program.m_App.m_MenuIdleTime = 0f;
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)16384))
			{
				pageIndex = 0;
				Program.m_App.mScores.fillPageFromFullList(m_Track, pageIndex, page);
				Program.m_SoundManager.Play(2);
				Program.m_App.m_MenuIdleTime = 0f;
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192))
			{
				Program.m_App.m_MenuIdleTime = 0f;
				OnBackPressed();
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)256) && m_Track > 0)
			{
				m_Track--;
				pageIndex = 0;
				if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
				{
					pageIndex = Program.m_App.mScores.fillPageThatContainsGamertagFromFullList(m_Track, page, ((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag);
				}
				else
				{
					Program.m_App.mScores.fillPageFromFullList(m_Track, pageIndex, page);
				}
				m_NumPages = (Program.m_App.mScores.getFullListSize(m_Track) - 1) / 10;
				Program.m_SoundManager.Play(2);
				Program.m_App.m_MenuIdleTime = 0f;
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)512) && m_Track < 9)
			{
				m_Track++;
				pageIndex = 0;
				if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
				{
					pageIndex = Program.m_App.mScores.fillPageThatContainsGamertagFromFullList(m_Track, page, ((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag);
				}
				else
				{
					Program.m_App.mScores.fillPageFromFullList(m_Track, pageIndex, page);
				}
				m_NumPages = (Program.m_App.mScores.getFullListSize(m_Track) - 1) / 10;
				Program.m_SoundManager.Play(2);
				Program.m_App.m_MenuIdleTime = 0f;
			}
			if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (Program.m_PlayerManager.GetPrimaryPlayer().LAY() < -0.9f && pageIndex < m_NumPages)
				{
					pageIndex++;
					Program.m_App.mScores.fillPageFromFullList(m_Track, pageIndex, page);
					m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
					Program.m_SoundManager.Play(4);
					Program.m_App.m_MenuIdleTime = 0f;
				}
				if (Program.m_PlayerManager.GetPrimaryPlayer().LAY() > 0.9f && pageIndex > 0)
				{
					pageIndex--;
					Program.m_App.mScores.fillPageFromFullList(m_Track, pageIndex, page);
					m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
					Program.m_SoundManager.Play(4);
					Program.m_App.m_MenuIdleTime = 0f;
				}
			}
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	private void OnBackPressed()
	{
		m_App.m_NextState = App.STATE.MAINMENU;
		m_App.StartFade(up: false);
		Program.m_SoundManager.Play(3);
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		Vector2 fRONTENDSCORES_TOP = FRONTENDSCORES_TOP;
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, m_App.GetText(App.TEXTID.FRONTENDSCORES_TITLE), fRONTENDSCORES_TOP - FRONTENDSCORES_TOP_OFFSET, m_App.m_FrontEnd.TITLE_COL);
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, m_App.GetText(App.TEXTID.TRACK), new Vector2(130f, 150f), m_App.m_FrontEnd.TITLE_COL);
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, $"{m_Track + 1}", new Vector2(130f, 200f), m_App.m_FrontEnd.TITLE_COL);
		int num = (Program.m_App.mScores.getFullListSize(0) - 1) / 10;
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, m_App.GetText(App.TEXTID.PAGE), new Vector2(130f, 300f), m_App.m_FrontEnd.TITLE_COL);
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, $"{pageIndex + 1}/{num + 1}", new Vector2(130f, 350f), m_App.m_FrontEnd.TITLE_COL);
		fRONTENDSCORES_TOP.X += 30f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Rank", fRONTENDSCORES_TOP, Color.YellowGreen);
		fRONTENDSCORES_TOP.X += 150f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Name", fRONTENDSCORES_TOP, Color.YellowGreen);
		fRONTENDSCORES_TOP.X += 500f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Time", fRONTENDSCORES_TOP, Color.YellowGreen);
		fRONTENDSCORES_TOP.Y += 35f;
		TopScoreEntry[] array = page;
		foreach (TopScoreEntry topScoreEntry in array)
		{
			if (topScoreEntry != null)
			{
				float num2 = topScoreEntry.Score;
				int num3 = (int)(num2 / 60000f);
				int num4 = (int)(num2 % 60000f / 1000f);
				int num5 = (int)(num2 % 1000f);
				fRONTENDSCORES_TOP.X = FRONTENDSCORES_TOP.X + 30f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{topScoreEntry.RankAtLastPageFill}. ", fRONTENDSCORES_TOP, Color.White);
				fRONTENDSCORES_TOP.X += 150f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{topScoreEntry.Gamertag}", fRONTENDSCORES_TOP, Color.White);
				fRONTENDSCORES_TOP.X += 500f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{num3:d2}:{num4:d2}.{num5:d3}", fRONTENDSCORES_TOP, Color.White);
				fRONTENDSCORES_TOP.Y += 35f;
			}
		}
		fRONTENDSCORES_TOP.X = 120f;
		fRONTENDSCORES_TOP.Y = 485f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.MYSCORE);
		m_App.m_GameText.Position = fRONTENDSCORES_TOP;
		if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
		{
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		}
		fRONTENDSCORES_TOP.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.TOPSCORE);
		m_App.m_GameText.Position = fRONTENDSCORES_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		fRONTENDSCORES_TOP.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.BACK);
		m_App.m_GameText.Position = fRONTENDSCORES_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		fRONTENDSCORES_TOP.X += 370f;
		fRONTENDSCORES_TOP.Y = 590f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.LSTICK);
		m_App.m_GameText.Position = fRONTENDSCORES_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.4f);
		fRONTENDSCORES_TOP.X += -40f;
		fRONTENDSCORES_TOP.Y = 650f;
		m_App.m_GameText.mText = "[LSHOULDER]";
		m_App.m_GameText.Position = fRONTENDSCORES_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.3f);
		fRONTENDSCORES_TOP.X += 70f;
		m_App.m_GameText.mText = "[RSHOULDER]";
		m_App.m_GameText.Position = fRONTENDSCORES_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.3f);
		fRONTENDSCORES_TOP.X += 80f;
		fRONTENDSCORES_TOP.Y = 565f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Page Up/Down with Left Stick", fRONTENDSCORES_TOP, Color.White);
		fRONTENDSCORES_TOP.Y += 50f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Select Track with Shoulders", fRONTENDSCORES_TOP, Color.White);
		m_App.m_SpriteBatch.End();
		m_App.RenderLines();
	}
}
