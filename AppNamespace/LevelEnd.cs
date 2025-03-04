using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class LevelEnd
{
	public static Vector2 LEVELEND_TOP = new Vector2(200f, 340f);

	public static Vector2 LEVELEND_OFFSET = new Vector2(0f, 40f);

	public static Vector2 LEVELEND_TOP_OFFSET = new Vector2(-200f, LEVELEND_TOP.Y - 30f);

	public static Vector2 LEVEL_END_TARGETS = new Vector2(130f, 100f);

	private App m_App;

	private static int NUM_HISCORES = 6;

	private TopScoreEntry[] page = new TopScoreEntry[NUM_HISCORES];

	private int pageIndex;

	private int m_NumPages;

	private bool bTimeAlready;

	private bool bTimeWon;

	private bool bScoreAlready;

	private bool bScoreWon;

	private bool bFlagsAlready;

	private bool bFlagsWon;

	private float m_TargetTime;

	private float m_TargetScore;

	private int m_FlagsWereCollected;

	private float m_MenuMoveTime;

	public LevelEnd(App app)
	{
		m_App = app;
	}

	public void Start()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		bTimeAlready = false;
		bTimeWon = false;
		bScoreAlready = false;
		bScoreWon = false;
		bFlagsAlready = false;
		bFlagsWon = false;
		m_TargetTime = 0f;
		m_TargetScore = 0f;
		m_FlagsWereCollected = 0;
		if (Program.m_App.m_Level != 11 && Program.m_App.mScores != null)
		{
			if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
			{
				pageIndex = Program.m_App.mScores.fillPageThatContainsGamertagFromFullList(Program.m_App.m_Level - 1, page, ((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag);
			}
			else
			{
				Program.m_App.mScores.fillPageFromFullList(Program.m_App.m_Level - 1, pageIndex, page);
			}
			m_NumPages = (Program.m_App.mScores.getFullListSize(Program.m_App.m_Level - 1) - 1) / NUM_HISCORES;
			m_TargetTime = Map.CupTargets[(Program.m_App.m_Level - 1) * 2];
			m_TargetScore = Map.CupTargets[(Program.m_App.m_Level - 1) * 2 + 1];
			m_FlagsWereCollected = Program.m_ItemManager.GetNumFlagsCollectedOnLevel(Program.m_App.m_Level - 1);
			int level = Program.m_App.m_Level - 1;
			if (Program.m_ItemManager.GetTimeCupOnLevel(level) == 1)
			{
				bTimeAlready = true;
			}
			else
			{
				bTimeAlready = false;
				if (Program.m_App.m_TrackTime < m_TargetTime)
				{
					bTimeWon = true;
					Program.m_ItemManager.GiveTimeCupOnLevel(level);
				}
			}
			if (Program.m_ItemManager.GetScoreCupOnLevel(level) == 1)
			{
				bScoreAlready = true;
			}
			else
			{
				bScoreAlready = false;
				if ((float)Program.m_PlayerManager.GetPrimaryPlayer().m_Score > m_TargetScore)
				{
					bScoreWon = true;
					Program.m_ItemManager.GiveScoreCupOnLevel(level);
				}
			}
			if (Program.m_ItemManager.GetFlagsCupOnLevel(level) == 1)
			{
				bFlagsAlready = true;
			}
			else
			{
				bFlagsAlready = false;
				if (Program.m_ItemManager.GetNumFlagsCollectedOnLevel(level) == 3)
				{
					bFlagsWon = true;
					Program.m_ItemManager.GiveFlagsCupOnLevel(level);
				}
			}
			m_App.IncrementLevel();
		}
		Program.m_LoadSaveManager.SaveGame();
	}

	public void Stop()
	{
	}

	public void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() == null)
		{
			return;
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
		{
			m_App.m_NextState = App.STATE.MAP;
			m_App.StartFade(up: false);
			Program.m_SoundManager.Play(2);
		}
		Program.m_PlayerManager.GetPrimaryPlayer().CheckJukeboxControls();
		if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			if (Program.m_PlayerManager.GetPrimaryPlayer().LAY() < -0.9f && pageIndex < m_NumPages)
			{
				pageIndex++;
				Program.m_App.mScores.fillPageFromFullList(Program.m_App.m_Level - 1, pageIndex, page);
				m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
				Program.m_SoundManager.Play(4);
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().LAY() > 0.9f && pageIndex > 0)
			{
				pageIndex--;
				Program.m_App.mScores.fillPageFromFullList(Program.m_App.m_Level - 1, pageIndex, page);
				m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
				Program.m_SoundManager.Play(4);
			}
		}
		Program.m_App.CheckInGamePurchase();
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_098e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		Vector2 lEVELEND_TOP = LEVELEND_TOP;
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, $"TRACK {Program.m_App.m_Level - 1} SCORES", lEVELEND_TOP - LEVELEND_TOP_OFFSET, m_App.m_FrontEnd.TITLE_COL);
		lEVELEND_TOP.X -= 80f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "GLOBAL RANK", lEVELEND_TOP, Color.White);
		lEVELEND_TOP.X += 360f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "NAME", lEVELEND_TOP, Color.White);
		lEVELEND_TOP.X += 450f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "BEST TIME", lEVELEND_TOP, Color.White);
		lEVELEND_TOP.Y += 35f;
		if (Guide.IsTrialMode)
		{
			lEVELEND_TOP.X = LEVELEND_TOP.X + 160f;
			lEVELEND_TOP.Y += 20f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Not Available in Trial Mode.", lEVELEND_TOP, Color.White);
			lEVELEND_TOP.X += 30f;
			lEVELEND_TOP.Y += 60f;
			Program.m_App.m_GameText.mText = Program.m_App.GetText(App.TEXTID.PRESS_X_TO_BUY);
			Program.m_App.m_GameText.Position = lEVELEND_TOP;
			Program.m_App.m_GameText.Draw(Program.m_App.m_SpriteBatch, 0.75f);
		}
		TopScoreEntry[] array = page;
		foreach (TopScoreEntry topScoreEntry in array)
		{
			if (topScoreEntry != null)
			{
				float num = topScoreEntry.Score;
				int num2 = (int)(num / 60000f);
				int num3 = (int)(num % 60000f / 1000f);
				int num4 = (int)(num % 1000f);
				lEVELEND_TOP.X = LEVELEND_TOP.X + 30f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{topScoreEntry.RankAtLastPageFill}. ", lEVELEND_TOP, Color.LightGoldenrodYellow);
				lEVELEND_TOP.X += 250f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{topScoreEntry.Gamertag}", lEVELEND_TOP, Color.LightGoldenrodYellow);
				lEVELEND_TOP.X += 500f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{num2:d2}:{num3:d2}:{num4:d3}", lEVELEND_TOP, Color.LightGoldenrodYellow);
				lEVELEND_TOP.Y += 35f;
			}
		}
		if (Program.m_App.m_Level != 11)
		{
			lEVELEND_TOP.X = LEVEL_END_TARGETS.X + 30f;
			lEVELEND_TOP.Y = LEVEL_END_TARGETS.Y;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "RESULT", lEVELEND_TOP, Color.White);
			lEVELEND_TOP.X += 370f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "TARGET", lEVELEND_TOP, Color.White);
			lEVELEND_TOP.X += 370f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "CUP STATUS", lEVELEND_TOP, Color.White);
			lEVELEND_TOP.X = LEVEL_END_TARGETS.X;
			lEVELEND_TOP.Y += 55f;
			int num2 = (int)(Program.m_App.m_TrackTime / 60000f);
			int num3 = (int)(Program.m_App.m_TrackTime % 60000f / 1000f);
			int num4 = (int)(Program.m_App.m_TrackTime % 1000f);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Time:", lEVELEND_TOP, Color.Yellow);
			lEVELEND_TOP.X += 150f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{num2:d2}:{num3:d2}.{num4:d3}", lEVELEND_TOP, Color.LightGoldenrodYellow);
			lEVELEND_TOP.X += 250f;
			num2 = (int)(m_TargetTime / 60000f);
			num3 = (int)(m_TargetTime % 60000f / 1000f);
			num4 = (int)(m_TargetTime % 1000f);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{num2:d2}:{num3:d2}.{num4:d3}", lEVELEND_TOP, Color.YellowGreen);
			lEVELEND_TOP.X += 330f;
			if (bTimeAlready)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "ALREADY HAVE", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			else if (bTimeWon)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "COMPLETE!", lEVELEND_TOP, Color.Gold);
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, lEVELEND_TOP + new Vector2(-30f, -20f), Color.LightGoldenrodYellow);
			}
			else
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "FAILED", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			lEVELEND_TOP.X = LEVEL_END_TARGETS.X;
			lEVELEND_TOP.Y += 55f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Score:", lEVELEND_TOP, Color.Yellow);
			lEVELEND_TOP.X += 150f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{Program.m_PlayerManager.GetPrimaryPlayer().m_Score}", lEVELEND_TOP, Color.LightGoldenrodYellow);
			lEVELEND_TOP.X += 250f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{m_TargetScore}", lEVELEND_TOP, Color.YellowGreen);
			lEVELEND_TOP.X += 330f;
			if (bScoreAlready)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "ALREADY HAVE", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			else if (bScoreWon)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "COMPLETE!", lEVELEND_TOP, Color.Gold);
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, lEVELEND_TOP + new Vector2(-30f, -20f), Color.LightGoldenrodYellow);
			}
			else
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "FAILED", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			lEVELEND_TOP.X = LEVEL_END_TARGETS.X;
			lEVELEND_TOP.Y += 55f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Flags:", lEVELEND_TOP, Color.Yellow);
			lEVELEND_TOP.X += 150f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{m_FlagsWereCollected}", lEVELEND_TOP, Color.LightGoldenrodYellow);
			lEVELEND_TOP.X += 250f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "3", lEVELEND_TOP, Color.YellowGreen);
			lEVELEND_TOP.X += 330f;
			if (bFlagsAlready)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "ALREADY HAVE", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			else if (bFlagsWon)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "COMPLETE!", lEVELEND_TOP, Color.Gold);
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, lEVELEND_TOP + new Vector2(-30f, -20f), Color.LightGoldenrodYellow);
			}
			else
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "FAILED", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
		}
		lEVELEND_TOP.X = 120f;
		lEVELEND_TOP.Y = 545f;
		lEVELEND_TOP.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.ATOCONTINUE);
		m_App.m_GameText.Position = lEVELEND_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		m_App.RenderLines();
		m_App.m_SpriteBatch.End();
	}
}
