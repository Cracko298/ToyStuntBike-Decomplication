using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using AvatarWrapper;
using CustomAvatarAnimation;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using RoundLineCode;

namespace AppNamespace;

public class App : Game
{
	public enum STATE
	{
		NONE,
		LOADING,
		LOGO,
		TITLE,
		MAINMENU,
		MAP,
		INGAME,
		OPTIONS,
		HELP,
		CREDITS,
		FRONTENDSCORES,
		NEWGAME,
		NEWGAME_ARE_YOU_SURE,
		CONTINUEGAME,
		EXITING,
		EXIT_BUY,
		EXITING_TRIAL,
		MISSION_COMPLETE,
		LEVEL_END
	}

	private enum PAUSE_MENU
	{
		PAUSED,
		EXIT_ARE_YOU_SURE,
		OPTIONS_INGAME,
		DROP_OUT_ARE_YOU_SURE
	}

	public enum LANG
	{
		ENGLISH
	}

	public enum TEXTID
	{
		START,
		CONTINUE,
		NEW,
		OPTIONS,
		TRACKTIMES,
		BUY,
		EXIT_GAME,
		SELECT,
		BACK,
		TUTORIAL_PRESS_A_TO_JUMP,
		TUTORIAL_HOLD_A_TO_CLIMB,
		CHECKPOINT,
		TUTORIAL_CATCH_FLIES,
		TUTORIAL_THREE_FLIES,
		TUTORIAL_PRESS_X_TO_HOOK_ON_SILK,
		TUTORIAL_PRESS_X_TO_RELEASE,
		TUTORIAL_PRESS_B_TO_MAKE_A_WEB,
		MAINMENU_TITLE,
		TUTORIAL_HOLD_A_TO_JUMP_HIGHER,
		OPTIONS_TITLE,
		MUSIC_VOL,
		SFX_VOL,
		VIBRATION,
		HELP,
		CREDITS,
		PAUSED_TITLE,
		RESUME,
		OPTIONS_INGAME,
		EXIT_TO_MENU,
		ARE_YOU_SURE,
		OK,
		CANCEL,
		SAVE,
		INGAME_MUSIC_VOL,
		INGAME_SFX_VOL,
		INGAME_VIBRATION,
		TUTORIAL_MAKE_A_SILK_STRAND,
		TUTORIAL_THEN_PRESS_A_FOR_A_BOOST_JUMP,
		ON,
		OFF,
		EASY,
		NORMAL,
		HARD,
		CRAZY,
		NEWGAME_ARE_YOU_SURE,
		SELECT_LEVEL,
		LEVEL,
		SAVING,
		FLIES,
		TOTAL,
		SAVE_ERROR_NO_SPACE,
		SAVE_ERROR,
		BOSS_HINT,
		BOSS,
		BOSS_WIN,
		EXIT_BUY,
		EXIT_QUIT,
		EXIT_TWENTY_LEVELS,
		EXIT_FOUR_BOSS_LEVELS,
		EXIT_SIXTY_FLIES,
		EXIT_JUMP,
		EXIT_THROUGH,
		GAME_WIN,
		ATOCONTINUE,
		BANG,
		ZAP,
		THUNK,
		SPLAT,
		TING,
		KATING,
		BOOM,
		KABOOM,
		CRASH,
		SMASH,
		WHOOSH,
		CHARGE,
		SCRAPE,
		BOMBSAWAY,
		HAMMERTIME,
		FIVEK,
		TENK,
		TWENTYK,
		CLANG,
		MAGNETIC,
		CLOUDY,
		BATTY,
		PINBALL,
		FIRST,
		SECOND,
		THIRD,
		FOURTH,
		HOOF,
		TRRING,
		BRRING,
		PINBALL20000,
		PINBALL50000,
		PINBALL75000,
		POW,
		BZZT,
		WHOOSH_SMALL,
		LIVES,
		PRESS_START_TO_JOIN1,
		PRESS_START_TO_JOIN2,
		PRESS_START_TO_JOIN3,
		PRESS_START_TO_JOIN4,
		MEDALS,
		EXTRALIFE,
		LANDING_BONUS,
		PRESS_X_TO_BUY,
		HELP_TITLE,
		CREDITS_TITLE,
		CREDITS1,
		CREDITS2,
		CREDITS3,
		CREDITS4,
		CREDITS5,
		CREDITS6,
		CREDITS7,
		CREDITS8,
		CREDITS9,
		CREDITS10,
		CREDITS11,
		CREDITS12,
		CREDITS13,
		CREDITS14,
		CREDITS15,
		CREDITSEND,
		HELP1,
		HELP2,
		HELP3,
		HELP4,
		HELP5,
		HELP6,
		HELP7,
		HELP8,
		HELP9,
		HELP10,
		HELP11,
		HELP12,
		HELP13,
		HELP14,
		HELP15,
		HELPEND,
		FRONTENDSCORES_TITLE,
		TRACK,
		PAGE,
		RETRY,
		LSTICK,
		MYSCORE,
		TOPSCORE,
		TIP1,
		TIP2,
		TIP3,
		TIP4,
		TIP5,
		TIP6,
		TIP7,
		TIP8,
		TIP9,
		TIP10,
		TIP11,
		TIP12,
		TIP13,
		TIP14,
		GET_READY,
		GO,
		FINISHED,
		AIR,
		WHEELIE,
		ENDO,
		CONGRATS,
		RETURNTOCHECKPOINT
	}

	public const int MENUPLANE_FRAMES = 16;

	private const int MAX_LINES = 4096;

	private const int TargetFrameRate = 60;

	private const int BackBufferWidth = 1280;

	private const int BackBufferHeight = 720;

	public const int NUM_LEVELS = 12;

	public const int NUM_JUMPSOUNDS = 1;

	public const int NUM_LANDSOUNDS = 1;

	public const float TEXTSCALESIZE = 3f;

	public const float TEXTSCALERATE = 0.2f;

	private const int shadowMapWidthHeight = 2048;

	public const float LEVEL_RATE = 1f;

	public const float SCROLL_RATE = 0.04f;

	public const float SCREEN_TOP_EDGE = 10f;

	public const float UPDATE_RATE = 5f;

	public const float RENDER_RATE = 15f;

	private const float MAGIC_RUG_BPM = 571.428f;

	private const float FRIDAY_NIGHT_BPM = 500f;

	private const float ECRANOPLAN_BPM = 827.586f;

	private const float CROONISM_BPM = 581.916f;

	private const float TWIN_BPM = 500f;

	private const float WAR_CLANGER_BPM = 600f;

	public const float FADE_TIME_SECS = 0.5f;

	private float[] m_aPausedSilkWidth = new float[3] { 195f, 195f, 325f };

	private float[] m_aPausedOptionsSilkWidth = new float[4] { 455f, 455f, 520f, 400f };

	public static Vector2 PAUSED_TOP = new Vector2(500f, 190f);

	public static Vector2 PAUSED_OFFSET = new Vector2(0f, 75f);

	public static Vector2 PAUSED_ARE_YOU_SURE = new Vector2(500f, 300f);

	public static Vector2 PAUSED_TOP_OFFSET = new Vector2(0f, 110f);

	public static Vector2 OPTIONS_TOP = Options.OPTIONS_TOP + new Vector2(0f, -30f);

	public static Color HUD_TEXT_COL = new Color(byte.MaxValue, (byte)246, (byte)0, byte.MaxValue);

	public static Color HUD_DIGITS_COL = new Color(byte.MaxValue, (byte)246, (byte)170, byte.MaxValue);

	private static Vector2 HUDFLAGPOS = new Vector2(110f, 590f);

	public static Color HUD_TARGETS_COL = new Color((byte)235, (byte)226, (byte)150, byte.MaxValue);

	public GraphicsDeviceManager graphics;

	public SpriteBatch m_SpriteBatch;

	public Texture2D m_Background;

	private Matrix projMatrix;

	private RoundLineManager m_LineManager = new RoundLineManager();

	public List<RoundLine> m_LineList = new List<RoundLine>(4096);

	public List<RoundLine> m_LineListSolid = new List<RoundLine>(2048);

	public List<RoundLine> m_LineListLegs = new List<RoundLine>(256);

	public List<RoundLine> m_LineListSilk = new List<RoundLine>(256);

	public List<RoundLine> m_LineListTrail = new List<RoundLine>(256);

	public List<RoundLine> m_LineListTrailBlue = new List<RoundLine>(256);

	public List<RoundLine> m_LineListTrailGreen = new List<RoundLine>(256);

	public List<RoundLine> m_LineListKill = new List<RoundLine>(2048);

	public RoundLine[] m_LinePool;

	public int m_LinePoolId;

	public STATE m_State = STATE.LOADING;

	public STATE m_PrevState = STATE.LOADING;

	public STATE m_NextState;

	public bool m_Paused;

	private TEXTID m_SubMenuState = TEXTID.RESUME;

	private Vector2 m_SubMenuPosition = Vector2.Zero;

	private float m_MenuMoveTime;

	private Vector2 m_PrevSubMenuPosition = Vector2.Zero;

	public bool m_RenderPause;

	private PAUSE_MENU m_PauseState;

	private int m_PauserId = -1;

	private PlayerIndex m_PauserPadId = (PlayerIndex)(-1);

	private int m_MusicVolCopy;

	private int m_SFXVolCopy;

	private bool m_bVibrationCopy;

	public Texture2D m_TitleBackground;

	public Texture2D m_MenuBackground;

	public Texture2D m_LogoScreen;

	public float m_LogoTime;

	public Texture2D m_MapBackground;

	public Texture2D m_BuyBackground;

	public Texture2D m_LoadingBackground;

	public GameTime m_GameTime;

	public bool m_bEditor;

	public bool m_bEditPaths;

	public KeyboardState m_OldKeyboardState;

	public KeyboardState m_KeyboardState;

	public MouseState m_MouseState;

	public MouseState m_OldMouseState;

	public float m_FrameLength;

	public Vector2 m_MousePos = new Vector2(0f, 0f);

	public Vector2 m_PrevMousePos = new Vector2(0f, 0f);

	public int m_Level = 1;

	public int m_LevelReached = 1;

	public float m_fScreenX = 1280f;

	public float m_fScreenY = 720f;

	public bool m_bFadingUp;

	public float m_fFadeAlpha;

	public Texture2D m_FadeTex;

	public Text m_GameText;

	public Text m_MenuText;

	public Text m_TriggerText;

	public SpriteFont m_SpeechFont;

	public SpriteFont m_MediumFont;

	public SpriteFont m_LargeFont;

	public SpriteFont m_SmallFont;

	public Random m_Rand;

	public float m_LevelWidth = 4096f;

	public float m_LevelHeight = 2048f;

	public BloomManager m_Bloom;

	public Title m_Title;

	public FrontEnd m_FrontEnd;

	public Map m_Map;

	public LevelEnd m_LevelEnd;

	public BackgroundManager m_BackgroundManager;

	public Options m_Options;

	public bool m_bInOptions;

	public Help m_Help;

	public Credits m_Credits;

	public FrontEndScores m_FrontEndScores;

	public PlayerIndex m_PlayerOnePadId;

	public Texture2D m_DialTexture;

	public Texture2D m_NeedleTexture;

	public Texture2D m_FlagToHudTexture;

	public Texture2D m_FlagHudTexture;

	public Texture2D m_GoldFlagTexture;

	public Texture2D m_TSBCupTexture;

	public Texture2D m_TSBCupTextureMed;

	public Texture2D m_SmokeTexture;

	public Texture2D m_ToonSmoke1Texture;

	public Texture2D m_ToonSmokeDarkGreyTexture;

	public Texture2D m_ToonSmokeYellowTexture;

	public Texture2D m_ToonBangTexture;

	public Texture2D m_BulletTexture;

	public Texture2D m_Bullet2Texture;

	public Texture2D m_LaserTexture;

	public Texture2D m_ArrowTexture;

	public Texture2D m_ItemHealthBackTexture;

	public Texture2D m_ItemHealthGradTexture;

	public Texture2D m_StarTexture;

	public Texture2D m_BubbleTextureRight;

	public Texture2D m_BubbleTextureLeft;

	public Texture2D m_BubbleTextureCentre;

	public Texture2D m_MenuPlaneTexture;

	public Texture2D m_WarLogoTexture;

	public Texture2D m_MapPlaneTexture;

	public Texture2D m_MapWeevilTexture;

	public Texture2D m_MapSharkTexture;

	public Texture2D m_MapMedalTexture;

	public Texture2D m_KaBoomTexture;

	public Texture2D m_ZigZagTexture;

	public Texture2D m_CloudTexture;

	public Texture2D m_SearchlightTexture;

	public Texture2D m_SpeechEndTexture;

	public int m_MenuPlaneFrame;

	public int m_MenuPlaneRate;

	public bool m_bUseDustMotes;

	private Stopwatch m_StopWatchUpdate;

	private Stopwatch m_StopWatchRender;

	public Song m_MenuMusic;

	public Song m_JukeBox;

	public int m_JukeBoxIdx;

	public bool m_bUsingJukebox;

	private MediaState m_MediaState;

	public float m_MusicChangeTime;

	public static StorageDevice m_StorageDevice = null;

	public bool m_bSaveExists;

	public bool m_SaveErrorNoSpace;

	public bool m_SaveError;

	public bool m_PrevGuideVisible;

	public bool m_PrevTrialMode;

	public bool m_bWasBought;

	public int m_RumbleFrames;

	public float m_HintTimer;

	public float m_ShowHintTime;

	public int[] m_LevelsVisited;

	public float m_ShowHudTime;

	public float m_ShowBossHealthTime;

	public float m_ShowBossWinTime;

	public bool m_bFadeMusic;

	public int m_LastJumpSoundIdx;

	public int m_LastLandSoundIdx;

	public CustomAvatarAnimationData m_IdleAnimation;

	public CustomAvatarAnimationData m_IdleGripAnimation;

	public CustomAvatarAnimationData m_FlailAnimation;

	public CustomAvatarAnimationData[] m_GestureAnim;

	public int m_MaxGesture;

	public float m_LevelEndTime;

	public float m_PermissionTime;

	public float m_NotInTrialTime;

	public float m_BoneRot1;

	public float m_BoneRot2;

	public float m_BoneRot3;

	public Vector3 m_BonePos = Vector3.Zero;

	public float m_BoneRot;

	public float m_PropRot;

	public int m_JitterCnt;

	public int m_Jitter;

	public float m_SearchlightAngle;

	public float m_SearchlightPosition = 1280f;

	public bool m_bDrawSpeechEnd;

	public Vector2 m_SpeechEndPos = Vector2.Zero;

	public bool m_Cheats;

	public int m_Lives;

	public int m_Medals;

	public float m_JoinTime;

	public int m_JoinFlash;

	public bool bRenderedLoading;

	public float m_NeedleRevs;

	public float m_TrackTime;

	public float m_GoTime;

	public float m_TextScale = 3f;

	public int m_LastRand;

	public float m_CollectFlagTime;

	public Vector2 m_CollectFlagPosition = Vector2.Zero;

	public float m_CollectFlagScale = 1f;

	public Actor m_AvShad_Head;

	public Actor m_AvShad_Body;

	public Actor m_AvShad_UpperArmL;

	public Actor m_AvShad_UpperArmR;

	public Actor m_AvShad_LowerArmL;

	public Actor m_AvShad_LowerArmR;

	public Actor m_AvShad_UpperLegL;

	public Actor m_AvShad_UpperLegR;

	public Actor m_AvShad_LowerLegL;

	public Actor m_AvShad_LowerLegR;

	public TEXTID m_Tip = TEXTID.TOPSCORE;

	public float m_TipDisplayTime;

	public float m_MenuIdleTime;

	private bool m_bCelShaded = true;

	private bool m_bShadows = true;

	private VertexDeclaration m_CelVdecl;

	public Vector3 m_Fog = new Vector3(0.6627451f, 62f / 85f, 47f / 51f);

	public Effect cartoonEffect;

	private int TOON_settingsIndex = 1;

	private Vector2 sketchJitter = Vector2.Zero;

	public Effect SHAD_effect;

	public EffectTechnique SHAD_CreateShadowTechnique;

	private Vector3 lightDir = new Vector3(-0.3333333f, 2f / 3f, 2f / 3f);

	public RenderTarget2D SHAD_shadowRenderTarget;

	public DepthStencilBuffer SHAD_shadowDepthBuffer;

	public Texture2D SHAD_shadowMap;

	private Matrix SHAD_lightViewProjection;

	private BoundingFrustum cameraFrustum = new BoundingFrustum(Matrix.Identity);

	private Matrix tmpProjectionMatrix = Matrix.Identity;

	private Matrix tmpViewMatrix = Matrix.Identity;

	public float m_LevelPos;

	public bool m_bScroll;

	public float m_ScrollSpeed;

	public float m_MaxScrollSpeed = 0.04f;

	public World m_World;

	public Body m_GroundBody;

	public OnlineDataSyncManager mSyncManager;

	public TopScoreListContainer mScores;

	private static Random random = new Random();

	public LANG m_Lang;

	public static string[,] aLocalisedText = new string[1, 172] { 
	{
		"PRESS[A]TO START", "Continue Game", "New Game", "Help & Options", "Global Track Times", "Unlock Full Game", "Exit Game", "[A]Select", "[B]Back", "Press[A]to jump",
		"Hold[A]to climb", "CHECKPOINT!", "Catch flies to unlock new levels", "There are three flies in every level", "Press[X]near ends to hook on silk", "Press[X]again to let go", "Then press[B]near ends to make a silk strand", "MAIN MENU", "Hold[A]to jump higher", "OPTIONS",
		"Music Volume: ", "SFX Volume: ", "Vibration is ", "Help", "Credits", "PAUSED", "Resume", "Options", "Exit to menu", "Are You Sure?",
		"[A]Ok", "[B]Cancel", "[A]Save", "Music Volume: ", "SFX Volume: ", "Vibration is ", "Make a silk strand using[X]and[B]", "Then jump on it for a boost", "on", "off",
		"easy", "normal", "hard", "crazy!", "Overwrite game?", "Select Track", "Track: ", "Saving", "flies ", "total ",
		"save error: no space!", "save error!", "hint: climb the arm to the bright lines", "Boss", "Your spider has grown stronger!", "[X]Buy", "[B]Quit", "Twenty levels!", "Four boss battles!", "Sixty flies!",
		"Jump, Swing and Spring!", "Through", "Congratulations!", "[A]Continue", "BANG!", "ZAP!", "THUNK!", "SPLAT!", "TING!", "KA-TING!",
		"BOOM!", "KABOOM!", "CRASH", "SMASH", "WHOOOOOOOOOOOOOSH!", "CHAAAAAAAAAAAAAAAAAAARGE!", "SCRAAAPE!", "BOMBS AWAY", "HAMMER TIME!", "5000!",
		"10000!", "20000!", "CLANG!", "MAGNETIC!", "STEALTH MODE!", "BATTY!", "PINBALL WIZARD!", "FIRST", "SECOND", "THIRD",
		"FOURTH", "HOOOOOOOOOOOOF!", "TRRRRRRRRING!", "BBBRRRING!", "PINBALL 20000!", "PINBALL 50000!", "PINBALL 75000!", "POW!", "BZZZZZZZZZT!", "WHOOSH!",
		"lives:", "1P press START", "2P press START", "3P press START", "4P press START", "medals:", "EXTRA LIFE!", "LANDING BONUS!", "PRESS[X]TO BUY", "HELP",
		"CREDITS", "               TOY STUNT BIKE  by  Raoghard.", "", "Bike model by Phil (Katharii) McClenaghan.", "Physics by Farseer.", "Online Scoreboards by Spyn Doctor.", "Pulse Remix/Dreamers Choise by George Ellinas", "Put Your Hands Up/Test Drive by Zapac", "DisTOrATion by TDeeJay", "Bullet by DJ Rkod",
		"Rockerchick/GreenDaze/PurpleRider/GroovyBaby", "by Jason Shaw@audionautix.com", "TafiMaradi by Kevin McCloud (incompetech.com)", "Midnight Theme by CDK", "B34 by Zikweb", "", "END", "CONTROLS:", "Right Trigger or (A) - Accelerate.", "Left Trigger or (X) - Brake / Reverse.",
		"(B) - Crash / Bailout.", "(Y) - Restart (once crashed).", "START - Pause.", "Left Stick/DPad - shift riders weight.", "Right/Left Shoulder Buttons - Change music.", "(not when using Custom Soundtracks).", "", "Win CUPS to unlock new levels.", "There are three ways to win cups.", "1. Beat the target time.",
		"2. Beat the target score.", "3. Collect three gold flags.", "END", "Global Track Times", "Track", "Page", "[Y]Retry", "[LTHUMB]", "[A]My rank", "[X]Top",
		"        Use Right Trigger or (A) button to accelerate.", "   Use Left Trigger or (X) button to brake and reverse.", "      Use Left Stick to balance the bike when jumping.", "         Pull back before leaving ramps to gain height.", "                   Use (B) to bailout then (Y) to retry.", "   Use Start button to pause the game and edit options.", "    Beat the target time to win cups and unlock levels.", "   Beat the target score to win cups and unlock levels.", "Collect three gold flags to win cups and unlock levels.", "                  Use Shoulder buttons to change music.",
		"     Use the Guide button to select Custom Soundtracks.", "                         Pull wheelies to score points.", "                       Spin in the air to score points.", "    Keep backwheel on the ground to gain speed quickly.", "GET READY!", "GO!", "FINISHED!", "AIR:", "WHEELIE:", "ENDO:",
		"CONGRATS!", "[X]Checkpoint"
	} };

	private NonPhotoRealisticSettings TOON_Settings => NonPhotoRealisticSettings.PresetSettings[TOON_settingsIndex];

	public static Random Random => random;

	public void StartPause(int playerId, PlayerIndex padId)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		m_Paused = true;
		m_RenderPause = true;
		m_PauseState = PAUSE_MENU.PAUSED;
		m_PauserId = playerId;
		m_PauserPadId = padId;
		m_SubMenuState = TEXTID.RESUME;
		m_SubMenuPosition = PAUSED_TOP + (float)(m_SubMenuState - 26) * PAUSED_OFFSET;
		m_PrevSubMenuPosition = Vector2.Zero;
	}

	public void StopPause()
	{
		m_Paused = false;
		Program.m_PlayerManager.m_Player[0].m_bDebounceJump = true;
	}

	private void UpdatePaused()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Invalid comparison between Unknown and I4
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Invalid comparison between Unknown and I4
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Invalid comparison between Unknown and I4
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Invalid comparison between Unknown and I4
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		if (Fading())
		{
			return;
		}
		Program.m_PlayerManager.m_Player[m_PauserId].m_GamepadState = GamePad.GetState(m_PauserPadId);
		if (m_NextState > STATE.NONE)
		{
			StateEnd();
			m_PrevState = m_State;
			m_State = m_NextState;
			StateStart();
			m_NextState = STATE.NONE;
			StartFade(up: true);
		}
		Program.m_ParticleManager.Update();
		if (Program.m_PlayerManager.m_Player[m_PauserId].Debounce((Buttons)4096) && !Fading())
		{
			OnPauseSelectPressed();
		}
		Program.m_PlayerManager.m_Player[m_PauserId].m_EngineState = Player.EngineState.IDLE;
		Program.m_PlayerManager.m_Player[m_PauserId].UpdateEngineSound();
		if (m_PauseState == PAUSE_MENU.PAUSED)
		{
			if ((Program.m_PlayerManager.m_Player[m_PauserId].Debounce((Buttons)8192) || Program.m_PlayerManager.m_Player[m_PauserId].Debounce((Buttons)16) || Debounce((Keys)32)) && !Fading())
			{
				StopPause();
				m_RenderPause = false;
				Program.m_SoundManager.Play(3);
			}
		}
		else if (m_PauseState == PAUSE_MENU.EXIT_ARE_YOU_SURE)
		{
			if (Program.m_PlayerManager.m_Player[m_PauserId].Debounce((Buttons)8192))
			{
				m_PauseState = PAUSE_MENU.PAUSED;
				Program.m_SoundManager.Play(3);
			}
		}
		else if (m_PauseState == PAUSE_MENU.DROP_OUT_ARE_YOU_SURE)
		{
			if (Program.m_PlayerManager.m_Player[m_PauserId].Debounce((Buttons)8192))
			{
				m_PauseState = PAUSE_MENU.PAUSED;
				Program.m_SoundManager.Play(3);
			}
		}
		else if (m_PauseState == PAUSE_MENU.OPTIONS_INGAME && Program.m_PlayerManager.m_Player[m_PauserId].Debounce((Buttons)8192))
		{
			m_PauseState = PAUSE_MENU.PAUSED;
			m_SubMenuState = TEXTID.OPTIONS_INGAME;
			m_SubMenuPosition = PAUSED_TOP + (float)(m_SubMenuState - 26) * PAUSED_OFFSET;
			m_PrevSubMenuPosition = Vector2.Zero;
			m_Options.m_MusicVol = m_MusicVolCopy;
			m_Options.m_SFXVol = m_SFXVolCopy;
			m_Options.m_bVibration = m_bVibrationCopy;
			MediaPlayer.Volume = (float)m_Options.m_MusicVol / 100f;
			SoundEffect.MasterVolume = (float)m_Options.m_SFXVol / 100f;
			Program.m_SoundManager.Play(3);
		}
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref Program.m_PlayerManager.m_Player[m_PauserId].m_GamepadState)).ThumbSticks;
		float num = ((GamePadThumbSticks)(ref thumbSticks)).Left.Y;
		if (Math.Abs(num) < 0.25f)
		{
			num = 0f;
		}
		GamePadDPad dPad = ((GamePadState)(ref Program.m_PlayerManager.m_Player[m_PauserId].m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad)).Up == 1)
		{
			num = 1f;
		}
		GamePadDPad dPad2 = ((GamePadState)(ref Program.m_PlayerManager.m_Player[m_PauserId].m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad2)).Down == 1)
		{
			num = -1f;
		}
		GamePadThumbSticks thumbSticks2 = ((GamePadState)(ref Program.m_PlayerManager.m_Player[m_PauserId].m_GamepadState)).ThumbSticks;
		float num2 = ((GamePadThumbSticks)(ref thumbSticks2)).Left.X;
		if (Math.Abs(num2) < 0.25f)
		{
			num2 = 0f;
		}
		GamePadDPad dPad3 = ((GamePadState)(ref Program.m_PlayerManager.m_Player[m_PauserId].m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad3)).Right == 1)
		{
			num2 = 1f;
		}
		GamePadDPad dPad4 = ((GamePadState)(ref Program.m_PlayerManager.m_Player[m_PauserId].m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad4)).Left == 1)
		{
			num2 = -1f;
		}
		if (m_PauseState == PAUSE_MENU.PAUSED)
		{
			if (m_MenuMoveTime < (float)m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (num < -0.9f && m_SubMenuState < TEXTID.EXIT_TO_MENU)
				{
					m_SubMenuState++;
					m_MenuMoveTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
					m_SubMenuPosition = PAUSED_TOP + (float)(m_SubMenuState - 26) * PAUSED_OFFSET;
					Program.m_SoundManager.Play(1);
				}
				if (num > 0.9f && m_SubMenuState > TEXTID.RESUME)
				{
					m_SubMenuState--;
					m_MenuMoveTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
					m_SubMenuPosition = PAUSED_TOP + (float)(m_SubMenuState - 26) * PAUSED_OFFSET;
					Program.m_SoundManager.Play(0);
				}
			}
		}
		else if (m_PauseState == PAUSE_MENU.OPTIONS_INGAME)
		{
			if (m_MenuMoveTime < (float)m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (num < -0.9f && m_SubMenuState < TEXTID.INGAME_VIBRATION)
				{
					m_SubMenuState++;
					m_MenuMoveTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
					m_SubMenuPosition = OPTIONS_TOP + (float)(m_SubMenuState - 33) * Options.OPTIONS_OFFSET;
					Program.m_SoundManager.Play(1);
				}
				if (num > 0.9f && m_SubMenuState > TEXTID.INGAME_MUSIC_VOL)
				{
					m_SubMenuState--;
					m_MenuMoveTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
					m_SubMenuPosition = OPTIONS_TOP + (float)(m_SubMenuState - 33) * Options.OPTIONS_OFFSET;
					Program.m_SoundManager.Play(0);
				}
			}
			if (m_Options.m_ValueRepeatTime < (float)m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (num2 < -0.9f)
				{
					if (m_SubMenuState == TEXTID.INGAME_MUSIC_VOL)
					{
						if (m_Options.m_MusicVol > 0)
						{
							m_Options.m_MusicVol--;
						}
						m_Options.m_ValueRepeatTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 40f;
						MediaPlayer.Volume = (float)m_Options.m_MusicVol / 100f;
					}
					if (m_SubMenuState == TEXTID.INGAME_SFX_VOL)
					{
						if (m_Options.m_SFXVol > 0)
						{
							m_Options.m_SFXVol--;
							Program.m_SoundManager.Play(1);
						}
						m_Options.m_ValueRepeatTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 40f;
						SoundEffect.MasterVolume = (float)m_Options.m_SFXVol / 100f;
					}
					if (m_SubMenuState == TEXTID.INGAME_VIBRATION)
					{
						m_Options.m_bVibration = !m_Options.m_bVibration;
						m_Options.m_ValueRepeatTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 500f;
						Program.m_SoundManager.Play(1);
					}
				}
				if (num2 > 0.9f)
				{
					if (m_SubMenuState == TEXTID.INGAME_MUSIC_VOL)
					{
						if (m_Options.m_MusicVol < 100)
						{
							m_Options.m_MusicVol++;
						}
						m_Options.m_ValueRepeatTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 40f;
						MediaPlayer.Volume = (float)m_Options.m_MusicVol / 100f;
					}
					if (m_SubMenuState == TEXTID.INGAME_SFX_VOL)
					{
						if (m_Options.m_SFXVol < 100)
						{
							m_Options.m_SFXVol++;
							Program.m_SoundManager.Play(1);
						}
						m_Options.m_ValueRepeatTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 40f;
						SoundEffect.MasterVolume = (float)m_Options.m_SFXVol / 100f;
					}
					if (m_SubMenuState == TEXTID.INGAME_VIBRATION)
					{
						m_Options.m_bVibration = !m_Options.m_bVibration;
						m_Options.m_ValueRepeatTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 500f;
						Program.m_SoundManager.Play(1);
					}
				}
			}
		}
		_ = m_SubMenuPosition != m_PrevSubMenuPosition;
		m_PrevSubMenuPosition = m_SubMenuPosition;
		Program.m_PlayerManager.m_Player[m_PauserId].m_OldGamepadState = Program.m_PlayerManager.m_Player[m_PauserId].m_GamepadState;
		m_ShowHudTime = (float)m_GameTime.TotalGameTime.TotalSeconds + 0.02f;
	}

	private void OnPauseSelectPressed()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		Program.m_SoundManager.Play(2);
		if (m_PauseState == PAUSE_MENU.PAUSED)
		{
			switch (m_SubMenuState)
			{
			case TEXTID.RESUME:
				StopPause();
				m_RenderPause = false;
				break;
			case TEXTID.OPTIONS_INGAME:
				m_PauseState = PAUSE_MENU.OPTIONS_INGAME;
				m_SubMenuState = TEXTID.INGAME_MUSIC_VOL;
				m_SubMenuPosition = OPTIONS_TOP + (float)(m_SubMenuState - 33) * Options.OPTIONS_OFFSET;
				m_PrevSubMenuPosition = Vector2.Zero;
				m_MusicVolCopy = m_Options.m_MusicVol;
				m_SFXVolCopy = m_Options.m_SFXVol;
				m_bVibrationCopy = m_Options.m_bVibration;
				MediaPlayer.Volume = (float)m_Options.m_MusicVol / 100f;
				SoundEffect.MasterVolume = (float)m_Options.m_SFXVol / 100f;
				break;
			case TEXTID.EXIT_TO_MENU:
				m_PauseState = PAUSE_MENU.EXIT_ARE_YOU_SURE;
				break;
			}
		}
		else if (m_PauseState == PAUSE_MENU.EXIT_ARE_YOU_SURE)
		{
			StopPause();
			m_NextState = STATE.MAINMENU;
			m_bFadeMusic = true;
			StartFade(up: false);
		}
		else if (m_PauseState == PAUSE_MENU.OPTIONS_INGAME)
		{
			m_PauseState = PAUSE_MENU.PAUSED;
			m_SubMenuState = TEXTID.OPTIONS_INGAME;
			m_SubMenuPosition = PAUSED_TOP + (float)(m_SubMenuState - 26) * PAUSED_OFFSET;
			m_PrevSubMenuPosition = Vector2.Zero;
			Program.m_LoadSaveManager.SaveGame();
		}
		else if (m_PauseState == PAUSE_MENU.DROP_OUT_ARE_YOU_SURE)
		{
			Program.m_PlayerManager.DropOut(m_PauserId);
			StopPause();
			m_RenderPause = false;
		}
	}

	private void RenderPaused()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		m_SpriteBatch.Begin();
		Color val = default(Color);
		((Color)(ref val))._002Ector((byte)0, (byte)0, (byte)0, (byte)200);
		new Rectangle(0, 0, 32, 32);
		Rectangle val2 = default(Rectangle);
		((Rectangle)(ref val2))._002Ector(640, 360, 1280, 720);
		m_SpriteBatch.Draw(m_FadeTex, val2, (Rectangle?)null, val, 0f, new Vector2(8f, 8f), (SpriteEffects)0, 0f);
		Vector2 val3 = PAUSED_TOP;
		if (m_PauseState == PAUSE_MENU.PAUSED)
		{
			m_MenuText.mColor = m_FrontEnd.TITLE_COL;
			m_MenuText.Position = val3 - PAUSED_TOP_OFFSET;
			m_MenuText.mText = GetText(TEXTID.PAUSED_TITLE);
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
			GetMenuColour(out m_MenuText.mColor, TEXTID.RESUME);
			m_MenuText.Position = val3;
			m_MenuText.mText = GetText(TEXTID.RESUME);
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
			val3 += PAUSED_OFFSET;
			GetMenuColour(out m_MenuText.mColor, TEXTID.OPTIONS_INGAME);
			m_MenuText.Position = val3;
			m_MenuText.mText = GetText(TEXTID.OPTIONS_INGAME);
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
			val3 += PAUSED_OFFSET;
			GetMenuColour(out m_MenuText.mColor, TEXTID.EXIT_TO_MENU);
			m_MenuText.Position = val3;
			m_MenuText.mText = GetText(TEXTID.EXIT_TO_MENU);
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
			DrawMenuPlane(m_SubMenuPosition);
		}
		else if (m_PauseState == PAUSE_MENU.EXIT_ARE_YOU_SURE)
		{
			val3 = PAUSED_ARE_YOU_SURE;
			m_MenuText.mColor = m_FrontEnd.HIGHLIGHT_COL;
			m_MenuText.Position = val3;
			m_MenuText.mText = GetText(TEXTID.ARE_YOU_SURE);
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
		}
		else if (m_PauseState == PAUSE_MENU.DROP_OUT_ARE_YOU_SURE)
		{
			val3 = PAUSED_ARE_YOU_SURE;
			m_MenuText.mColor = m_FrontEnd.HIGHLIGHT_COL;
			m_MenuText.Position = val3;
			m_MenuText.mText = GetText(TEXTID.ARE_YOU_SURE);
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
		}
		else if (m_PauseState == PAUSE_MENU.OPTIONS_INGAME)
		{
			m_MenuText.mColor = m_FrontEnd.TITLE_COL;
			val3 = OPTIONS_TOP;
			m_MenuText.Position = val3 - Options.OPTIONS_TOP_OFFSET;
			m_MenuText.mText = GetText(TEXTID.OPTIONS_TITLE);
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
			GetMenuColour(out m_MenuText.mColor, TEXTID.INGAME_MUSIC_VOL);
			m_MenuText.Position = val3;
			m_MenuText.mText = GetText(TEXTID.INGAME_MUSIC_VOL);
			m_MenuText.mText = string.Format(m_MenuText.mText + "{0}", m_Options.m_MusicVol);
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
			val3 += Options.OPTIONS_OFFSET;
			GetMenuColour(out m_MenuText.mColor, TEXTID.INGAME_SFX_VOL);
			m_MenuText.mText = GetText(TEXTID.INGAME_SFX_VOL);
			m_MenuText.mText = string.Format(m_MenuText.mText + "{0}", m_Options.m_SFXVol);
			m_MenuText.Position = val3;
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
			val3 += Options.OPTIONS_OFFSET;
			GetMenuColour(out m_MenuText.mColor, TEXTID.INGAME_VIBRATION);
			m_MenuText.mText = GetText(TEXTID.INGAME_VIBRATION);
			if (m_Options.m_bVibration)
			{
				m_MenuText.mText = string.Format(m_MenuText.mText + GetText(TEXTID.ON));
			}
			else
			{
				m_MenuText.mText = string.Format(m_MenuText.mText + GetText(TEXTID.OFF));
			}
			m_MenuText.Position = val3;
			m_MenuText.Draw(m_SpriteBatch, 0.75f);
			DrawMenuPlane(m_SubMenuPosition);
		}
		val3.X = 150f;
		val3.Y = 495f;
		if (m_PauseState == PAUSE_MENU.PAUSED)
		{
			m_GameText.mText = GetText(TEXTID.SELECT);
			m_GameText.Position = val3;
			m_GameText.Draw(m_SpriteBatch, 0.75f);
			val3.Y += 80f;
			m_GameText.mText = GetText(TEXTID.BACK);
			m_GameText.Position = val3;
			m_GameText.Draw(m_SpriteBatch, 0.75f);
		}
		else
		{
			m_GameText.mText = GetText(TEXTID.OK);
			m_GameText.Position = val3;
			m_GameText.Draw(m_SpriteBatch, 0.75f);
			val3.Y += 80f;
			m_GameText.mText = GetText(TEXTID.CANCEL);
			m_GameText.Position = val3;
			m_GameText.Draw(m_SpriteBatch, 0.75f);
		}
		m_MenuText.mText = GetText(TEXTID.LEVEL);
		m_MenuText.mText += $"{m_Level}";
		m_MenuText.Position = new Vector2(140f, 70f);
		m_MenuText.mColor = m_FrontEnd.LOWLIGHT_COL;
		m_MenuText.Draw(m_SpriteBatch, 0.75f);
		m_SpriteBatch.End();
		RenderLines();
	}

	private void GetMenuColour(out Color c, TEXTID highlightedId)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (highlightedId == m_SubMenuState)
		{
			c = m_FrontEnd.HIGHLIGHT_COL;
		}
		else
		{
			c = m_FrontEnd.LOWLIGHT_COL;
		}
	}

	protected override void Draw(GameTime gameTime)
	{
		m_StopWatchRender.Start();
		Render();
		((Game)this).Draw(gameTime);
		m_StopWatchRender.Stop();
	}

	public void Render()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		switch (m_State)
		{
		case STATE.LOADING:
		{
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(547f, 210f);
			((Game)this).GraphicsDevice.Clear(Color.Black);
			m_SpriteBatch.Begin();
			m_SpriteBatch.Draw(m_LoadingBackground, val, Color.White);
			m_SpriteBatch.End();
			bRenderedLoading = true;
			break;
		}
		case STATE.TITLE:
			m_Title.Draw();
			Program.m_DebugManager.DrawSafeFrame();
			break;
		case STATE.MAINMENU:
			m_FrontEnd.Draw();
			Program.m_DebugManager.DrawSafeFrame();
			break;
		case STATE.MAP:
			m_Map.Draw();
			Program.m_DebugManager.DrawSafeFrame();
			break;
		case STATE.OPTIONS:
			m_Options.Draw();
			Program.m_DebugManager.DrawSafeFrame();
			break;
		case STATE.HELP:
			m_Help.Draw();
			Program.m_DebugManager.DrawSafeFrame();
			break;
		case STATE.CREDITS:
			m_Credits.Draw();
			Program.m_DebugManager.DrawSafeFrame();
			break;
		case STATE.FRONTENDSCORES:
			m_FrontEndScores.Draw();
			Program.m_DebugManager.DrawSafeFrame();
			break;
		case STATE.LEVEL_END:
			m_LevelEnd.Draw();
			Program.m_DebugManager.DrawSafeFrame();
			break;
		case STATE.INGAME:
			RenderCelShadedModels();
			m_SpriteBatch.Begin();
			DrawHud();
			Program.m_TriggerManager.Render();
			m_SpriteBatch.End();
			RenderLines();
			if (m_RenderPause)
			{
				RenderPaused();
			}
			DrawFarseer();
			break;
		case STATE.EXIT_BUY:
		{
			m_SpriteBatch.Begin();
			m_SpriteBatch.Draw(m_BuyBackground, new Vector2(0f, 0f), Color.White);
			Vector2 zero = Vector2.Zero;
			zero.X = 150f;
			zero.Y = 585f;
			m_GameText.mText = GetText(TEXTID.EXIT_BUY);
			m_GameText.Position = zero;
			m_GameText.Draw(m_SpriteBatch, 0.75f);
			zero.X += 750f;
			m_GameText.mText = GetText(TEXTID.EXIT_QUIT);
			m_GameText.Position = zero;
			m_GameText.Draw(m_SpriteBatch, 0.75f);
			m_SpriteBatch.End();
			break;
		}
		}
		Program.m_DebugManager.Render();
		RenderFade();
	}

	public void DrawFarseer()
	{
	}

	public static RenderTarget2D CloneRenderTarget(GraphicsDevice device, int numberLevels)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		int backBufferWidth = device.PresentationParameters.BackBufferWidth;
		int backBufferHeight = device.PresentationParameters.BackBufferHeight;
		DisplayMode displayMode = device.DisplayMode;
		return new RenderTarget2D(device, backBufferWidth, backBufferHeight, numberLevels, ((DisplayMode)(ref displayMode)).Format, device.PresentationParameters.MultiSampleType, device.PresentationParameters.MultiSampleQuality);
	}

	public void SetupCelShaded()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		cartoonEffect = ((Game)this).Content.Load<Effect>("BasicEffectShad");
		SHAD_effect = ((Game)this).Content.Load<Effect>("SHAD_ShadowMapping");
		SHAD_CreateShadowTechnique = SHAD_effect.Techniques["CreateShadowMap"];
		SurfaceFormat val = (SurfaceFormat)(-1);
		GraphicsAdapter defaultAdapter = GraphicsAdapter.DefaultAdapter;
		DisplayMode currentDisplayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
		if (defaultAdapter.CheckDeviceFormat((DeviceType)1, ((DisplayMode)(ref currentDisplayMode)).Format, (TextureUsage)1073741824, (QueryUsages)0, (ResourceType)8, (SurfaceFormat)22))
		{
			val = (SurfaceFormat)22;
		}
		else
		{
			GraphicsAdapter defaultAdapter2 = GraphicsAdapter.DefaultAdapter;
			DisplayMode currentDisplayMode2 = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
			if (defaultAdapter2.CheckDeviceFormat((DeviceType)1, ((DisplayMode)(ref currentDisplayMode2)).Format, (TextureUsage)1073741824, (QueryUsages)0, (ResourceType)8, (SurfaceFormat)25))
			{
				val = (SurfaceFormat)25;
			}
		}
		SHAD_shadowRenderTarget = new RenderTarget2D(graphics.GraphicsDevice, 2048, 2048, 1, val);
		SHAD_shadowDepthBuffer = new DepthStencilBuffer(graphics.GraphicsDevice, 2048, 2048, (DepthFormat)51);
	}

	public void DrawLine(Vector2 p1, Vector2 p2, Color c, float fWidth)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		p1.Y = 720f - p1.Y;
		p2.Y = 720f - p2.Y;
		m_LinePool[m_LinePoolId].Move(p1, p2);
		m_LinePool[m_LinePoolId].alpha = (float)(int)((Color)(ref c)).A / 255f;
		if ((float)(int)((Color)(ref c)).R == 0f && fWidth > 4f)
		{
			m_LineListLegs.Add(m_LinePool[m_LinePoolId]);
		}
		else if ((float)(int)((Color)(ref c)).R == 0f && fWidth == 2f)
		{
			m_LineListSilk.Add(m_LinePool[m_LinePoolId]);
		}
		else if (((Color)(ref c)).R == 248)
		{
			m_LineListTrail.Add(m_LinePool[m_LinePoolId]);
		}
		else if (((Color)(ref c)).R == 0 && ((Color)(ref c)).G == 0 && ((Color)(ref c)).B == byte.MaxValue)
		{
			m_LineListTrailBlue.Add(m_LinePool[m_LinePoolId]);
		}
		else if (((Color)(ref c)).R == 0 && ((Color)(ref c)).G == byte.MaxValue && ((Color)(ref c)).B == 0)
		{
			m_LineListTrailGreen.Add(m_LinePool[m_LinePoolId]);
		}
		else
		{
			m_LineList.Add(m_LinePool[m_LinePoolId]);
		}
		m_LinePoolId++;
	}

	public void DrawHud()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_082d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Unknown result type (might be due to invalid IL or missing references)
		//IL_0933: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d98: Unknown result type (might be due to invalid IL or missing references)
		if (m_bEditor)
		{
			return;
		}
		float num = (float)m_GameTime.TotalGameTime.TotalSeconds;
		int num2 = (int)(m_TrackTime / 60000f);
		int num3 = (int)(m_TrackTime % 60000f / 1000f);
		int num4 = (int)(m_TrackTime % 1000f / 1f);
		m_SpriteBatch.DrawString(m_MediumFont, "TIME:", new Vector2(130f, 62f), HUD_TEXT_COL);
		m_SpriteBatch.DrawString(m_MediumFont, $"{num2:d2}:{num3:d2}.{num4:d3}", new Vector2(300f, 62f), HUD_DIGITS_COL);
		m_SpriteBatch.DrawString(m_MediumFont, "SCORE:", new Vector2(720f, 62f), HUD_TEXT_COL);
		m_SpriteBatch.DrawString(m_MediumFont, $"{Program.m_PlayerManager.GetPrimaryPlayer().m_Score:D7}", new Vector2(930f, 62f), HUD_DIGITS_COL);
		if ((Program.m_PlayerManager.GetPrimaryPlayer().m_State == Player.State.READYSTEADY || Program.m_PlayerManager.GetPrimaryPlayer().m_State == Player.State.WAITING_AT_START || m_GoTime > num) && Program.m_App.m_Level < 11)
		{
			float num5 = Map.CupTargets[(Program.m_App.m_Level - 1) * 2];
			float num6 = Map.CupTargets[(Program.m_App.m_Level - 1) * 2 + 1];
			num2 = (int)(num5 / 60000f);
			num3 = (int)(num5 % 60000f / 1000f);
			num4 = (int)(num5 % 1000f / 1f);
			m_SpriteBatch.DrawString(m_SmallFont, $"{num2:d2}:{num3:d2}.{num4:d3}", new Vector2(300f, 110f), HUD_TARGETS_COL);
			m_SpriteBatch.DrawString(m_SmallFont, $"{num6}", new Vector2(930f, 110f), HUD_TARGETS_COL);
		}
		m_SpriteBatch.Draw(m_DialTexture, new Vector2(1050f, 520f), Color.White);
		float num7 = -2.8f;
		float num8 = 27f;
		float num9 = Math.Abs(Program.m_PlayerManager.GetPrimaryPlayer().m_ChasisBody.LinearVelocity.X);
		num9 /= num8;
		num9 *= 3.2f;
		num7 += num9;
		num7 += (float)m_Rand.NextDouble() * 0.2f * (num9 / 3.2f);
		Rectangle value = default(Rectangle);
		((Rectangle)(ref value))._002Ector(0, 0, m_NeedleTexture.Width, m_NeedleTexture.Height);
		m_SpriteBatch.Draw(m_NeedleTexture, new Vector2(1114f, 584f), (Rectangle?)value, Color.White, num7, new Vector2(12f, 64f), 1f, (SpriteEffects)0, 0f);
		if (Program.m_PlayerManager.GetPrimaryPlayer().m_State == Player.State.READYSTEADY)
		{
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(640f, 500f);
			m_SpriteBatch.DrawString(m_LargeFont, GetText(TEXTID.GET_READY), val, new Color((byte)225, (byte)225, (byte)0, byte.MaxValue), 0f, new Vector2(405f, 50f), m_TextScale, (SpriteEffects)0, 0f);
			m_TextScale = MathHelper.Lerp(m_TextScale, 1f, 0.2f);
			m_GoTime = num + 3f;
		}
		else if (m_GoTime > num)
		{
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector(640f, 500f);
			float num10 = (m_GoTime - num) / 3f;
			m_SpriteBatch.DrawString(m_LargeFont, GetText(TEXTID.GO), val2, new Color(1f, 1f, 0f, num10), 0f, new Vector2(140f, 50f), m_TextScale, (SpriteEffects)0, 0f);
			m_TextScale = MathHelper.Lerp(m_TextScale, 1f, 0.2f);
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().m_State == Player.State.FINISHED)
		{
			Vector2 val3 = default(Vector2);
			((Vector2)(ref val3))._002Ector(640f, 150f);
			if (m_Level == 10)
			{
				m_SpriteBatch.DrawString(m_LargeFont, GetText(TEXTID.CONGRATS), val3, Color.Yellow, 0f, new Vector2(335f, 50f), m_TextScale, (SpriteEffects)0, 0f);
			}
			else
			{
				m_SpriteBatch.DrawString(m_LargeFont, GetText(TEXTID.FINISHED), val3, Color.Yellow, 0f, new Vector2(335f, 50f), m_TextScale, (SpriteEffects)0, 0f);
			}
			m_TextScale = MathHelper.Lerp(m_TextScale, 1f, 0.2f);
			val3.X = 120f;
			val3.Y = 475f;
			m_GameText.mText = GetText(TEXTID.ATOCONTINUE);
			m_GameText.Position = val3;
			m_GameText.Draw(m_SpriteBatch, 0.75f);
			val3.Y += 60f;
			m_GameText.mText = GetText(TEXTID.RETRY);
			m_GameText.Position = val3;
			m_GameText.Draw(m_SpriteBatch, 0.75f);
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().m_State == Player.State.RACING && !Program.m_PlayerManager.GetPrimaryPlayer().m_Ragdoll.m_bOnBike)
		{
			Vector2 zero = Vector2.Zero;
			zero.X = 120f;
			zero.Y = 475f;
			m_GameText.mText = GetText(TEXTID.RETRY);
			m_GameText.Position = zero;
			m_GameText.Draw(m_SpriteBatch, 0.75f);
			if (Program.m_LoadSaveManager.m_TrackTime != 0f)
			{
				zero.Y += 60f;
				m_GameText.mText = GetText(TEXTID.RETURNTOCHECKPOINT);
				m_GameText.Position = zero;
				m_GameText.Draw(m_SpriteBatch, 0.75f);
			}
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().m_AirDisplayScoreTime > 0f)
		{
			Vector2 val4 = default(Vector2);
			((Vector2)(ref val4))._002Ector(500f, 600f);
			m_SpriteBatch.DrawString(m_MediumFont, GetText(TEXTID.AIR), val4, new Color(1f, 1f, 0f, Program.m_PlayerManager.GetPrimaryPlayer().m_AirDisplayScoreTime));
			m_SpriteBatch.DrawString(m_MediumFont, $"{Program.m_PlayerManager.GetPrimaryPlayer().m_AirDisplayScore:D}", val4 + new Vector2(150f, 0f), new Color(1f, 0.964f, 0.667f, Program.m_PlayerManager.GetPrimaryPlayer().m_AirDisplayScoreTime));
			Program.m_PlayerManager.GetPrimaryPlayer().m_AirDisplayScoreTime = Math.Max(0f, Program.m_PlayerManager.GetPrimaryPlayer().m_AirDisplayScoreTime - 0.02f);
		}
		else if (Program.m_PlayerManager.GetPrimaryPlayer().m_WheelieDisplayScoreTime > 0f)
		{
			Vector2 val5 = default(Vector2);
			((Vector2)(ref val5))._002Ector(450f, 600f);
			m_SpriteBatch.DrawString(m_MediumFont, GetText(TEXTID.WHEELIE), val5, new Color(1f, 1f, 0f, Program.m_PlayerManager.GetPrimaryPlayer().m_WheelieDisplayScoreTime));
			m_SpriteBatch.DrawString(m_MediumFont, $"{Program.m_PlayerManager.GetPrimaryPlayer().m_WheelieDisplayScore:D}", val5 + new Vector2(300f, 0f), new Color(1f, 0.964f, 0.667f, Program.m_PlayerManager.GetPrimaryPlayer().m_WheelieDisplayScoreTime));
			Program.m_PlayerManager.GetPrimaryPlayer().m_WheelieDisplayScoreTime = Math.Max(0f, Program.m_PlayerManager.GetPrimaryPlayer().m_WheelieDisplayScoreTime - 0.02f);
		}
		else if (Program.m_PlayerManager.GetPrimaryPlayer().m_EndoDisplayScoreTime > 0f)
		{
			Vector2 val6 = default(Vector2);
			((Vector2)(ref val6))._002Ector(480f, 600f);
			m_SpriteBatch.DrawString(m_MediumFont, GetText(TEXTID.ENDO), val6, new Color(1f, 1f, 0f, Program.m_PlayerManager.GetPrimaryPlayer().m_EndoDisplayScoreTime));
			m_SpriteBatch.DrawString(m_MediumFont, $"{Program.m_PlayerManager.GetPrimaryPlayer().m_EndoDisplayScore:D}", val6 + new Vector2(200f, 0f), new Color(1f, 0.964f, 0.667f, Program.m_PlayerManager.GetPrimaryPlayer().m_EndoDisplayScoreTime));
			Program.m_PlayerManager.GetPrimaryPlayer().m_EndoDisplayScoreTime = Math.Max(0f, Program.m_PlayerManager.GetPrimaryPlayer().m_EndoDisplayScoreTime - 0.02f);
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().m_SpinDisplayScoreTime > 0f)
		{
			Vector2 val7 = default(Vector2);
			((Vector2)(ref val7))._002Ector(230f, 550f);
			if (Program.m_PlayerManager.GetPrimaryPlayer().m_BackFlip)
			{
				m_SpriteBatch.DrawString(m_MediumFont, $"BACK FLIP {Program.m_PlayerManager.GetPrimaryPlayer().m_SpinDisplayRevolutions * 360:D}!", val7 + new Vector2(200f, 0f), new Color(1f, 1f, 0f, Program.m_PlayerManager.GetPrimaryPlayer().m_SpinDisplayScoreTime));
			}
			else
			{
				m_SpriteBatch.DrawString(m_MediumFont, $"FRONT FLIP {Program.m_PlayerManager.GetPrimaryPlayer().m_SpinDisplayRevolutions * 360:D}!", val7 + new Vector2(200f, 0f), new Color(1f, 1f, 0f, Program.m_PlayerManager.GetPrimaryPlayer().m_SpinDisplayScoreTime));
			}
			Program.m_PlayerManager.GetPrimaryPlayer().m_SpinDisplayScoreTime = Math.Max(0f, Program.m_PlayerManager.GetPrimaryPlayer().m_SpinDisplayScoreTime - 0.02f);
		}
		if (m_Level != 11)
		{
			m_SpriteBatch.Draw(m_FlagHudTexture, HUDFLAGPOS, Color.White);
			m_SpriteBatch.DrawString(m_MediumFont, $"{Program.m_ItemManager.GetNumFlagsCollectedOnLevel(m_Level - 1)}/3", new Vector2(160f, 600f), HUD_DIGITS_COL);
		}
		if (m_CollectFlagTime > (float)m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			m_CollectFlagPosition.X = MathHelper.Lerp(m_CollectFlagPosition.X, HUDFLAGPOS.X + 22f, 0.2f);
			m_CollectFlagPosition.Y = MathHelper.Lerp(m_CollectFlagPosition.Y, HUDFLAGPOS.Y + 36f, 0.2f);
			m_CollectFlagScale = MathHelper.Lerp(m_CollectFlagScale, 0.5f, 0.2f);
			Rectangle value2 = default(Rectangle);
			((Rectangle)(ref value2))._002Ector(0, 0, m_FlagToHudTexture.Width, m_FlagToHudTexture.Height);
			m_SpriteBatch.Draw(m_FlagToHudTexture, m_CollectFlagPosition, (Rectangle?)value2, Color.White, 0f, new Vector2((float)m_FlagToHudTexture.Width * 0.5f, (float)m_FlagToHudTexture.Height * 0.5f), m_CollectFlagScale, (SpriteEffects)0, 0f);
		}
		if (m_TipDisplayTime > (float)m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			Vector2 val8 = default(Vector2);
			((Vector2)(ref val8))._002Ector(90f, 450f);
			m_SpriteBatch.DrawString(m_SmallFont, GetText(m_Tip), val8, Color.LightGray);
		}
		if (Program.m_App.m_Paused)
		{
			Program.m_LoadSaveManager.m_CheckpointDisplayTime += (float)Program.m_App.m_GameTime.ElapsedGameTime.TotalSeconds;
		}
		if (Program.m_LoadSaveManager.m_CheckpointDisplayTime != 0f && Program.m_LoadSaveManager.m_CheckpointDisplayTime > (float)Program.m_App.m_GameTime.TotalGameTime.TotalSeconds)
		{
			Vector2 val9 = default(Vector2);
			((Vector2)(ref val9))._002Ector(640f, 150f);
			m_SpriteBatch.DrawString(m_MediumFont, GetText(TEXTID.CHECKPOINT), val9, Color.LightSalmon, 0f, new Vector2(220f, 30f), m_TextScale, (SpriteEffects)0, 0f);
			m_TextScale = MathHelper.Lerp(m_TextScale, 1f, 0.2f);
		}
		else
		{
			Program.m_LoadSaveManager.m_CheckpointDisplayTime = 0f;
		}
	}

	private Color GetPlayerCol(int id)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.m_Player[id].m_Id != -1)
		{
			switch (Program.m_PlayerManager.m_Player[id].m_ModelId)
			{
			case 0:
				return new Color(byte.MaxValue, (byte)50, (byte)50);
			case 1:
				return new Color((byte)50, (byte)50, byte.MaxValue);
			case 2:
				return new Color((byte)50, (byte)50, (byte)50);
			case 3:
				return new Color((byte)192, (byte)192, (byte)192);
			}
		}
		return Color.Purple;
	}

	public void RenderLines()
	{
	}

	private void RenderCelShadedModels()
	{
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		GraphicsDevice graphicsDevice = graphics.GraphicsDevice;
		RenderState renderState = graphics.GraphicsDevice.RenderState;
		renderState.CullMode = (CullMode)3;
		renderState.AlphaBlendEnable = false;
		renderState.AlphaTestEnable = false;
		renderState.DepthBufferEnable = true;
		((Game)this).GraphicsDevice.SamplerStates[0].AddressU = (TextureAddressMode)1;
		((Game)this).GraphicsDevice.SamplerStates[0].AddressV = (TextureAddressMode)1;
		((Game)this).GraphicsDevice.SamplerStates[0].MagFilter = (TextureFilter)1;
		((Game)this).GraphicsDevice.SamplerStates[0].MinFilter = (TextureFilter)1;
		((Game)this).GraphicsDevice.SamplerStates[0].MipFilter = (TextureFilter)1;
		if (m_bShadows)
		{
			float num = MathHelper.ToRadians(45f);
			Viewport viewport = graphics.GraphicsDevice.Viewport;
			tmpProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(num, ((Viewport)(ref viewport)).AspectRatio, 1f, 55f);
			tmpViewMatrix = Matrix.CreateLookAt(Program.m_CameraManager3D.m_CameraPositionTarget + new Vector3(0f, 10f, 0f), Program.m_CameraManager3D.m_CameraLookAt + new Vector3(0f, -2f, 0f), Vector3.Up);
			cameraFrustum.Matrix = tmpViewMatrix * tmpProjectionMatrix;
			SHAD_lightViewProjection = CreateLightViewProjectionMatrix();
			_ = graphics.GraphicsDevice.RenderState;
			((Game)this).GraphicsDevice.SetRenderTarget(0, SHAD_shadowRenderTarget);
			DepthStencilBuffer depthStencilBuffer = ((Game)this).GraphicsDevice.DepthStencilBuffer;
			((Game)this).GraphicsDevice.DepthStencilBuffer = SHAD_shadowDepthBuffer;
			((Game)this).GraphicsDevice.Clear(Color.White);
			SHAD_effect.CurrentTechnique = SHAD_CreateShadowTechnique;
			for (int i = 0; i < 4; i++)
			{
				if (Program.m_PlayerManager.m_Player[i].m_Id != -1)
				{
					DrawModelShadowCasters(Program.m_PlayerManager.m_Player[i]);
					DrawModelShadowCasters(Program.m_App.m_AvShad_Head);
					DrawModelShadowCasters(Program.m_App.m_AvShad_Body);
					DrawModelShadowCasters(Program.m_App.m_AvShad_UpperArmR);
					DrawModelShadowCasters(Program.m_App.m_AvShad_UpperArmL);
					DrawModelShadowCasters(Program.m_App.m_AvShad_LowerArmR);
					DrawModelShadowCasters(Program.m_App.m_AvShad_LowerArmL);
					DrawModelShadowCasters(Program.m_App.m_AvShad_UpperLegR);
					DrawModelShadowCasters(Program.m_App.m_AvShad_UpperLegL);
					DrawModelShadowCasters(Program.m_App.m_AvShad_LowerLegR);
					DrawModelShadowCasters(Program.m_App.m_AvShad_LowerLegL);
				}
			}
			for (int j = 0; j < 256; j++)
			{
				if (Program.m_ItemManager.m_Item[j].m_Id != -1 && Program.m_ItemManager.m_Item[j].m_ModelLOD != null && Program.m_ItemManager.m_Item[j].m_bCastShadows && Program.m_ItemManager.m_Item[j].OnScreen())
				{
					DrawModelShadowCasters(Program.m_ItemManager.m_Item[j]);
				}
			}
			((Game)this).GraphicsDevice.SetRenderTarget(0, (RenderTarget2D)null);
			((Game)this).GraphicsDevice.DepthStencilBuffer = depthStencilBuffer;
			SHAD_shadowMap = SHAD_shadowRenderTarget.GetTexture();
		}
		graphicsDevice.Clear(Color.Black);
		graphics.GraphicsDevice.VertexDeclaration = m_CelVdecl;
		string effectTechniqueName = "BasicEffectShad";
		if (m_BackgroundManager.m_Background[0].m_Model != null)
		{
			DrawModelCelShaded(m_BackgroundManager.m_Background[0], effectTechniqueName);
			DrawModelCelShaded(m_BackgroundManager.m_Background[1], effectTechniqueName);
		}
		for (int k = 0; k < 256; k++)
		{
			if (Program.m_ItemManager.m_Item[k].m_Id != -1 && Program.m_ItemManager.m_Item[k].m_Layer == 1)
			{
				DrawModelCelShaded(Program.m_ItemManager.m_Item[k], effectTechniqueName);
			}
		}
		renderState.DepthBufferEnable = true;
		if (m_bCelShaded)
		{
			for (int l = 0; l < 256; l++)
			{
				if (Program.m_ItemManager.m_Item[l].m_Id != -1 && Program.m_ItemManager.m_Item[l].m_Layer == 0 && Program.m_ItemManager.m_Item[l].OnScreen())
				{
					DrawModelCelShaded(Program.m_ItemManager.m_Item[l], effectTechniqueName);
				}
			}
			for (int m = 0; m < 4; m++)
			{
				if (Program.m_PlayerManager.m_Player[m].m_Id != -1)
				{
					DrawModelCelShaded(Program.m_PlayerManager.m_Player[m], effectTechniqueName);
				}
			}
		}
		Program.m_PlayerManager.RenderAvatars();
	}

	public void DrawModelCelShaded(Actor gameobject, string effectTechniqueName)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		Enumerator enumerator = gameobject.m_Model.Meshes.GetEnumerator();
		try
		{
			while (((Enumerator)(ref enumerator)).MoveNext())
			{
				ModelMesh current = ((Enumerator)(ref enumerator)).Current;
				Enumerator enumerator2 = current.Effects.GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator2)).MoveNext())
					{
						Effect current2 = ((Enumerator)(ref enumerator2)).Current;
						current2.CurrentTechnique = current2.Techniques[effectTechniqueName];
						Vector3 zero = Vector3.Zero;
						zero.X = gameobject.m_Position.X + gameobject.m_ShakeVec.X;
						zero.Y = gameobject.m_Position.Y + gameobject.m_ShakeVec.Y;
						zero.Z = gameobject.m_ZDistance;
						float z = gameobject.m_Rotation.Z;
						float x = gameobject.m_Rotation.X;
						float y = gameobject.m_Rotation.Y;
						Matrix value = gameobject.m_Transforms[current.ParentBone.Index] * Matrix.CreateFromYawPitchRoll(y, x, z) * Matrix.CreateScale(gameobject.m_fScale) * Matrix.CreateTranslation(zero);
						current2.Parameters["World"].SetValue(value);
						current2.Parameters["View"].SetValue(Program.m_CameraManager3D.m_CameraViewMatrix);
						current2.Parameters["Projection"].SetValue(Program.m_CameraManager3D.m_CameraProjectionMatrix);
						current2.Parameters["EyePosition"].SetValue(Program.m_CameraManager3D.m_CameraPosition);
						current2.Parameters["LightViewProj"].SetValue(SHAD_lightViewProjection);
						current2.Parameters["ShadowMap"].SetValue((Texture)(object)SHAD_shadowMap);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator2)/*cast due to .constrained prefix*/).Dispose();
				}
				current.Draw();
			}
		}
		finally
		{
			((IDisposable)(Enumerator)(ref enumerator)/*cast due to .constrained prefix*/).Dispose();
		}
	}

	public static void ChangeEffectUsedByModel(Model model, Effect replacementEffect)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<Effect, Effect> dictionary = new Dictionary<Effect, Effect>();
		Enumerator enumerator = model.Meshes.GetEnumerator();
		try
		{
			while (((Enumerator)(ref enumerator)).MoveNext())
			{
				ModelMesh current = ((Enumerator)(ref enumerator)).Current;
				if (((ReadOnlyCollection<Effect>)(object)current.Effects)[0].CurrentTechnique.Name == "BasicEffectShad")
				{
					break;
				}
				Enumerator enumerator2 = current.Effects.GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator2)).MoveNext())
					{
						BasicEffect val = (BasicEffect)((Enumerator)(ref enumerator2)).Current;
						if (!dictionary.ContainsKey((Effect)(object)val))
						{
							Effect val2 = replacementEffect.Clone(replacementEffect.GraphicsDevice);
							val2.Parameters["BasicTexture"].SetValue((Texture)(object)val.Texture);
							dictionary.Add((Effect)(object)val, val2);
						}
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator2)/*cast due to .constrained prefix*/).Dispose();
				}
				Enumerator enumerator3 = current.MeshParts.GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator3)).MoveNext())
					{
						ModelMeshPart current2 = ((Enumerator)(ref enumerator3)).Current;
						current2.Effect = dictionary[current2.Effect];
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator3)/*cast due to .constrained prefix*/).Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)(Enumerator)(ref enumerator)/*cast due to .constrained prefix*/).Dispose();
		}
	}

	private Matrix CreateLightViewProjectionMatrix()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		Matrix val = Matrix.CreateLookAt(Vector3.Zero, -lightDir, Vector3.Up);
		Vector3[] corners = cameraFrustum.GetCorners();
		for (int i = 0; i < corners.Length; i++)
		{
			ref Vector3 reference = ref corners[i];
			reference = Vector3.Transform(corners[i], val);
		}
		BoundingBox val2 = BoundingBox.CreateFromPoints((IEnumerable<Vector3>)corners);
		Vector3 val3 = val2.Max - val2.Min;
		Vector3 val4 = val3 * 0.5f;
		Vector3 val5 = val2.Min + val4;
		val5.Z = val2.Min.Z;
		val5 = Vector3.Transform(val5, Matrix.Invert(val));
		Matrix val6 = Matrix.CreateLookAt(val5, val5 - lightDir, Vector3.Up);
		Matrix val7 = Matrix.CreateOrthographic(val3.X, val3.Y, 0f - val3.Z, val3.Z);
		return val6 * val7;
	}

	public void DrawModelShadowCasters(Actor gameobject)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		SHAD_effect.Begin();
		SHAD_effect.CurrentTechnique.Passes[0].Begin();
		Enumerator enumerator = gameobject.m_ModelLOD.Meshes.GetEnumerator();
		try
		{
			while (((Enumerator)(ref enumerator)).MoveNext())
			{
				ModelMesh current = ((Enumerator)(ref enumerator)).Current;
				Vector3 zero = Vector3.Zero;
				zero.X = gameobject.m_Position.X + gameobject.m_ShakeVec.X;
				zero.Y = gameobject.m_Position.Y + gameobject.m_ShakeVec.Y + 0.1f;
				zero.Z = gameobject.m_ZDistance;
				float num = gameobject.m_Rotation.Z;
				if (current.Name == "Mesh01")
				{
					num = m_PropRot;
				}
				float y = gameobject.m_Rotation.Y;
				Matrix val = Matrix.Identity;
				if (current.Name == "Mesh02")
				{
					val = Matrix.CreateRotationZ(m_PropRot * 0.5f);
				}
				Matrix value = val * gameobject.m_Transforms[current.ParentBone.Index] * Matrix.CreateFromYawPitchRoll(y, gameobject.m_Rotation.X, num) * Matrix.CreateScale(gameobject.m_fScale) * Matrix.CreateTranslation(zero);
				SHAD_effect.Parameters["World"].SetValue(value);
				SHAD_effect.Parameters["LightViewProj"].SetValue(SHAD_lightViewProjection);
				SHAD_effect.CommitChanges();
				Enumerator enumerator2 = current.MeshParts.GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator2)).MoveNext())
					{
						ModelMeshPart current2 = ((Enumerator)(ref enumerator2)).Current;
						((Game)this).GraphicsDevice.VertexDeclaration = current2.VertexDeclaration;
						((Game)this).GraphicsDevice.Vertices[0].SetSource(current.VertexBuffer, 0, current2.VertexStride);
						((Game)this).GraphicsDevice.Indices = current.IndexBuffer;
						((Game)this).GraphicsDevice.DrawIndexedPrimitives((PrimitiveType)4, current2.BaseVertex, 0, current2.NumVertices, current2.StartIndex, current2.PrimitiveCount);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator2)/*cast due to .constrained prefix*/).Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)(Enumerator)(ref enumerator)/*cast due to .constrained prefix*/).Dispose();
		}
		SHAD_effect.CurrentTechnique.Passes[0].End();
		SHAD_effect.End();
	}

	public void DrawMenuPlane(Vector2 pos)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		pos.X -= 78f;
		pos.Y -= 10f;
		m_SpriteBatch.Draw(m_MenuPlaneTexture, pos, new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		m_MenuPlaneRate--;
		if (m_MenuPlaneRate <= 0)
		{
			m_MenuPlaneRate = 4;
			m_MenuPlaneFrame++;
			if (m_MenuPlaneFrame >= 16)
			{
				m_MenuPlaneFrame = 0;
			}
		}
	}

	protected override void Initialize()
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		SignedInGamer.SignedIn += SignedInGamer_SignedIn;
		SignedInGamer.SignedOut += SignedInGamer_SignedOut;
		MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
		m_Title = new Title(this);
		m_FrontEnd = new FrontEnd(this);
		m_Map = new Map(this);
		m_LevelEnd = new LevelEnd(this);
		m_Options = new Options(this);
		m_Help = new Help(this);
		m_Credits = new Credits(this);
		m_FrontEndScores = new FrontEndScores(this);
		m_World = new World(new Vector2(0f, -10f));
		m_GroundBody = Program.m_App.m_World.CreateBody();
		int version = 0;
		mSyncManager = new OnlineDataSyncManager(version, (Game)(object)this);
		((Collection<IGameComponent>)(object)((Game)this).Components).Add((IGameComponent)(object)mSyncManager);
		((Game)this).Initialize();
	}

	private void SignedInGamer_SignedOut(object sender, SignedOutEventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		mSyncManager.stop(null);
		int playerExistsWithPad = Program.m_PlayerManager.GetPlayerExistsWithPad(e.Gamer.PlayerIndex);
		if (playerExistsWithPad != -1)
		{
			Program.m_PlayerManager.m_Player[playerExistsWithPad].m_Avatar = null;
		}
	}

	private void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
	{
		StartOnlineScores();
	}

	public void StartOnlineScores()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (!Guide.IsTrialMode && Gamer.SignedInGamers[m_PlayerOnePadId] != null && Gamer.SignedInGamers[m_PlayerOnePadId].IsSignedInToLive && Gamer.SignedInGamers[m_PlayerOnePadId].Privileges.AllowOnlineSessions)
		{
			Program.m_App.mSyncManager.start(Gamer.SignedInGamers[m_PlayerOnePadId], mScores);
		}
	}

	public Vector2 MousePosition()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return m_MousePos;
	}

	public static float RandomBetween(float min, float max)
	{
		return min + (float)random.NextDouble() * (max - min);
	}

	public App()
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		graphics = new GraphicsDeviceManager((Game)(object)this);
		graphics.PreferredBackBufferWidth = 1280;
		graphics.PreferredBackBufferHeight = 720;
		graphics.PreferMultiSampling = true;
		((Game)this).Content.RootDirectory = "Content";
		((Game)this).TargetElapsedTime = TimeSpan.FromTicks(166666L);
		((Game)this).IsFixedTimeStep = true;
		graphics.SynchronizeWithVerticalRetrace = true;
		graphics.ApplyChanges();
		m_LinePool = new RoundLine[4096];
		for (int i = 0; i < 4096; i++)
		{
			m_LinePool[i] = new RoundLine(new Vector2(0f, 0f), new Vector2(0f, 0f));
		}
		m_OldKeyboardState = Keyboard.GetState();
		m_OldMouseState = Mouse.GetState();
		m_Rand = new Random();
		m_PlayerOnePadId = (PlayerIndex)(-1);
		m_StopWatchUpdate = new Stopwatch();
		m_StopWatchRender = new Stopwatch();
		((Collection<IGameComponent>)(object)((Game)this).Components).Add((IGameComponent)new GamerServicesComponent((Game)(object)this));
		m_LevelsVisited = new int[13];
		for (int j = 0; j < 13; j++)
		{
			m_LevelsVisited[j] = 0;
		}
		m_GestureAnim = (CustomAvatarAnimationData[])(object)new CustomAvatarAnimationData[5];
	}

	protected override void LoadContent()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		m_SpriteBatch = new SpriteBatch(((Game)this).GraphicsDevice);
		m_LoadingBackground = ((Game)this).Content.Load<Texture2D>("Backgrounds/loading");
	}

	protected void PostLoadContent()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		m_FadeTex = ((Game)this).Content.Load<Texture2D>("Overlays/fade");
		m_LineManager.Init(((Game)this).GraphicsDevice, ((Game)this).Content);
		Viewport viewport = ((Game)this).GraphicsDevice.Viewport;
		float num = ((Viewport)(ref viewport)).Width;
		Viewport viewport2 = ((Game)this).GraphicsDevice.Viewport;
		projMatrix = Matrix.CreateOrthographic(num, (float)((Viewport)(ref viewport2)).Height, -100f, 100f);
		projMatrix *= Matrix.CreateTranslation(-1f, -1f, 0f);
		m_GameText = new Text(((Game)this).Content, "", "Fonts/arialb28_blckout2_blcksolidoff4_itlc", new Vector2(0f, 0f), Color.White);
		m_MenuText = new Text(((Game)this).Content, "", "Fonts/arialb28_blckout2_blcksolidoff4_itlc", new Vector2(0f, 0f), Color.White);
		m_TriggerText = new Text(((Game)this).Content, "", "Fonts/arialb28_blckout2_blcksolidoff4_itlc", new Vector2(0f, 0f), Color.White);
		m_SpeechFont = ((Game)this).Content.Load<SpriteFont>("Fonts/comicpro18");
		m_MediumFont = ((Game)this).Content.Load<SpriteFont>("Fonts/arialb28_blckout2_blcksolidoff4_itlc");
		m_LargeFont = ((Game)this).Content.Load<SpriteFont>("Fonts/arialb72_blckout4_blcksolidoff8_itlc");
		m_SmallFont = ((Game)this).Content.Load<SpriteFont>("Fonts/arialb20_blkout2_itlc");
		m_Bloom = new BloomManager();
		m_Bloom.LoadContent(((Game)this).Content, ((Game)this).GraphicsDevice);
		m_NeedleTexture = ((Game)this).Content.Load<Texture2D>("Sprites/needle1");
		m_DialTexture = ((Game)this).Content.Load<Texture2D>("Sprites/speedo_hud1");
		m_FlagToHudTexture = ((Game)this).Content.Load<Texture2D>("Sprites/flagtohud");
		m_FlagHudTexture = ((Game)this).Content.Load<Texture2D>("Sprites/flaghud");
		m_TSBCupTexture = ((Game)this).Content.Load<Texture2D>("Sprites/tsbcup_small");
		m_TSBCupTextureMed = ((Game)this).Content.Load<Texture2D>("Sprites/tsbcup_med");
		m_MenuPlaneTexture = ((Game)this).Content.Load<Texture2D>("Sprites/menu_tick");
		m_GoldFlagTexture = ((Game)this).Content.Load<Texture2D>("Sprites/goldflag");
		m_TitleBackground = ((Game)this).Content.Load<Texture2D>("Backgrounds/title");
		m_MenuBackground = ((Game)this).Content.Load<Texture2D>("Backgrounds/menu");
		m_MapBackground = ((Game)this).Content.Load<Texture2D>("Backgrounds/map");
		m_BuyBackground = ((Game)this).Content.Load<Texture2D>("Backgrounds/buy");
		Program.m_SoundManager.Add(6, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/barn_speech"));
		Program.m_SoundManager.Add(7, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/barn_speech_end"));
		Program.m_SoundManager.Add(8, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/bike_loop"));
		Program.m_SoundManager.Add(9, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/bikeaccel1"));
		Program.m_SoundManager.Add(10, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/body1"));
		Program.m_SoundManager.Add(11, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/body2"));
		Program.m_SoundManager.Add(12, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/body3"));
		Program.m_SoundManager.Add(13, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/body4"));
		Program.m_SoundManager.Add(14, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/body5"));
		Program.m_SoundManager.Add(15, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/crash1"));
		Program.m_SoundManager.Add(16, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/crash2"));
		Program.m_SoundManager.Add(17, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/crash3"));
		Program.m_SoundManager.Add(18, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/crash4"));
		Program.m_SoundManager.Add(19, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/crash5"));
		Program.m_SoundManager.Add(20, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/ready"));
		Program.m_SoundManager.Add(21, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/go"));
		Program.m_SoundManager.Add(22, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/land1"));
		Program.m_SoundManager.Add(23, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/land2"));
		Program.m_SoundManager.Add(24, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/kiting"));
		Program.m_SoundManager.Add(25, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/bump3"));
		Program.m_SoundManager.Add(26, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/skid1"));
		Program.m_SoundManager.Add(27, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/skid2"));
		Program.m_SoundManager.Add(28, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/finished"));
		Program.m_SoundManager.Add(29, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/newlevel"));
		Program.m_SoundManager.Add(30, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/hiss"));
		Program.m_SoundManager.Add(31, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/horn"));
		Program.m_SoundManager.Add(32, ((Game)this).Content.Load<SoundEffect>("Sounds/ingame/checkpoint"));
		Program.m_SoundManager.Add(0, ((Game)this).Content.Load<SoundEffect>("Sounds/switch5down"));
		Program.m_SoundManager.Add(1, ((Game)this).Content.Load<SoundEffect>("Sounds/switch5up"));
		Program.m_SoundManager.Add(2, ((Game)this).Content.Load<SoundEffect>("Sounds/select"));
		Program.m_SoundManager.Add(3, ((Game)this).Content.Load<SoundEffect>("Sounds/back"));
		Program.m_SoundManager.Add(4, ((Game)this).Content.Load<SoundEffect>("Sounds/mapmove"));
		Avatar.Initialize(((Game)this).GraphicsDevice, ((Game)this).Content);
		m_IdleAnimation = ((Game)this).Content.Load<CustomAvatarAnimationData>("Anims/idle");
		m_IdleGripAnimation = ((Game)this).Content.Load<CustomAvatarAnimationData>("Anims/idlegrip");
		m_MaxGesture = 5;
		Program.m_CameraManager3D.Init();
		m_BackgroundManager = new BackgroundManager(this);
		MediaPlayer.IsRepeating = false;
		SetupCelShaded();
		m_CelVdecl = new VertexDeclaration(graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);
		Program.m_PlayerManager.m_Model[0] = ((Game)this).Content.Load<Model>("Models\\player");
		Program.m_PlayerManager.m_Player[0].SetModel(Program.m_PlayerManager.m_Model[0], Program.m_PlayerManager.m_Model[0]);
		Program.m_PlayerManager.m_Player[0].m_ModelId = 0;
		Program.m_PlayerManager.m_Player[0].CalcBounds3D();
		if (m_bCelShaded)
		{
			ChangeEffectUsedByModel(Program.m_PlayerManager.m_Model[0], cartoonEffect);
		}
		Program.m_ItemManager.LoadContent(((Game)this).Content);
		if (m_bCelShaded)
		{
			for (int i = 0; i < 63; i++)
			{
				if (Program.m_ItemManager.m_Model[i] != null)
				{
					ChangeEffectUsedByModel(Program.m_ItemManager.m_Model[i], cartoonEffect);
					Program.m_ItemManager.CalcModelSize(Program.m_ItemManager.m_Model[i], i, bUseAll: false);
				}
			}
		}
		m_AvShad_Head = new Actor();
		m_AvShad_Head.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_head"), ((Game)this).Content.Load<Model>("Models\\avshad_head"));
		m_AvShad_Body = new Actor();
		m_AvShad_Body.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_body"), ((Game)this).Content.Load<Model>("Models\\avshad_body"));
		m_AvShad_UpperArmR = new Actor();
		m_AvShad_UpperArmR.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_arm"), ((Game)this).Content.Load<Model>("Models\\avshad_arm"));
		m_AvShad_UpperArmL = new Actor();
		m_AvShad_UpperArmL.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_arm"), ((Game)this).Content.Load<Model>("Models\\avshad_arm"));
		m_AvShad_LowerArmR = new Actor();
		m_AvShad_LowerArmR.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_arm"), ((Game)this).Content.Load<Model>("Models\\avshad_arm"));
		m_AvShad_LowerArmL = new Actor();
		m_AvShad_LowerArmL.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_arm"), ((Game)this).Content.Load<Model>("Models\\avshad_arm"));
		m_AvShad_UpperLegR = new Actor();
		m_AvShad_UpperLegR.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_arm"), ((Game)this).Content.Load<Model>("Models\\avshad_arm"));
		m_AvShad_UpperLegL = new Actor();
		m_AvShad_UpperLegL.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_arm"), ((Game)this).Content.Load<Model>("Models\\avshad_arm"));
		m_AvShad_LowerLegR = new Actor();
		m_AvShad_LowerLegR.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_arm"), ((Game)this).Content.Load<Model>("Models\\avshad_arm"));
		m_AvShad_LowerLegL = new Actor();
		m_AvShad_LowerLegL.SetModel(((Game)this).Content.Load<Model>("Models\\avshad_arm"), ((Game)this).Content.Load<Model>("Models\\avshad_arm"));
		Init();
		m_MenuMusic = ((Game)this).Content.Load<Song>("Sounds/Music/cdk_Midnight_Theme");
	}

	protected override void Update(GameTime gameTime)
	{
		m_StopWatchUpdate.Start();
		m_GameTime = gameTime;
		UpdateFrameRate();
		Update();
		if (m_Options.m_Difficulty == 4)
		{
			Update();
		}
		if (m_State > STATE.TITLE)
		{
			if (m_PrevTrialMode && !Guide.IsTrialMode)
			{
				m_bWasBought = true;
				StartOnlineScores();
			}
			m_PrevTrialMode = Guide.IsTrialMode;
		}
		((Game)this).Update(gameTime);
		m_StopWatchUpdate.Stop();
	}

	public void HandleRumble()
	{
		Program.m_PlayerManager.HandleRumbleAll();
	}

	public bool Debounce(Keys k)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (((KeyboardState)(ref m_KeyboardState)).IsKeyDown(k) && !((KeyboardState)(ref m_OldKeyboardState)).IsKeyDown(k))
		{
			return true;
		}
		return false;
	}

	public bool LeftButton()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((MouseState)(ref m_MouseState)).LeftButton == 1 && (int)((MouseState)(ref m_OldMouseState)).LeftButton == 0)
		{
			return true;
		}
		return false;
	}

	public bool RightButton()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((MouseState)(ref m_MouseState)).RightButton == 1 && (int)((MouseState)(ref m_OldMouseState)).RightButton == 0)
		{
			return true;
		}
		return false;
	}

	public bool MiddleButton()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((MouseState)(ref m_MouseState)).MiddleButton == 1 && (int)((MouseState)(ref m_OldMouseState)).MiddleButton == 0)
		{
			return true;
		}
		return false;
	}

	private void HandleInput()
	{
	}

	public void SetLevelEnd()
	{
		m_LevelEndTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 3000f;
	}

	private void Init()
	{
		SetLanguage();
		m_State = STATE.NONE;
		m_NextState = STATE.LOADING;
		m_LogoTime = 2f;
	}

	public void UpdateFrameRate()
	{
		m_FrameLength = (float)m_GameTime.ElapsedGameTime.TotalMilliseconds;
	}

	public bool Update()
	{
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		if (!Guide.IsVisible && m_PrevGuideVisible)
		{
			MediaPlayer.Resume();
		}
		m_PrevGuideVisible = Guide.IsVisible;
		HandleRumble();
		if (Guide.IsVisible)
		{
			MediaPlayer.Pause();
			return true;
		}
		HandleInput();
		if (m_Paused || (Fading() && !m_bFadingUp))
		{
			if (Fading() && m_State == STATE.MAINMENU)
			{
				m_FrontEnd.Update();
			}
			if (Fading() && m_State == STATE.OPTIONS)
			{
				m_Options.Update();
			}
			if (m_Paused)
			{
				UpdatePaused();
			}
			return true;
		}
		m_MenuIdleTime += (float)m_GameTime.ElapsedGameTime.TotalMilliseconds;
		if (m_NextState > STATE.NONE)
		{
			StateEnd();
			m_PrevState = m_State;
			m_State = m_NextState;
			StateStart();
			m_NextState = STATE.NONE;
			StartFade(up: true);
			return true;
		}
		switch (m_State)
		{
		case STATE.LOADING:
			if (bRenderedLoading)
			{
				PostLoadContent();
				m_NextState = STATE.LOGO;
			}
			break;
		case STATE.LOGO:
			m_NextState = STATE.TITLE;
			break;
		case STATE.TITLE:
			m_Title.Update();
			break;
		case STATE.MAINMENU:
			m_FrontEnd.Update();
			break;
		case STATE.MAP:
			m_Map.Update();
			UpdateJukebox();
			break;
		case STATE.NEWGAME:
			m_NextState = STATE.INGAME;
			DoChangeLevel();
			break;
		case STATE.CONTINUEGAME:
			m_NextState = STATE.INGAME;
			DoChangeLevel();
			break;
		case STATE.OPTIONS:
			m_Options.Update();
			break;
		case STATE.HELP:
			m_Help.Update();
			break;
		case STATE.CREDITS:
			m_Credits.Update();
			break;
		case STATE.FRONTENDSCORES:
			m_FrontEndScores.Update();
			break;
		case STATE.LEVEL_END:
			m_LevelEnd.Update();
			break;
		case STATE.INGAME:
		{
			UpdateFarseer();
			Program.m_CameraManager3D.Update();
			if (m_bScroll)
			{
				m_ScrollSpeed += 0.0005f;
				if (m_ScrollSpeed > m_MaxScrollSpeed)
				{
					m_ScrollSpeed = m_MaxScrollSpeed;
				}
			}
			m_LevelPos = Program.m_CameraManager3D.m_CameraPositionTarget.X;
			if (Program.m_PlayerManager.GetPrimaryPlayer().m_State == Player.State.RACING)
			{
				m_TrackTime += (float)m_GameTime.ElapsedGameTime.TotalMilliseconds;
			}
			m_BackgroundManager.Update();
			Program.m_TriggerManager.Update();
			Program.m_ItemManager.Update();
			Program.m_ParticleManager.Update();
			Program.m_PlayerManager.Update();
			m_OldKeyboardState = m_KeyboardState;
			m_OldMouseState = m_MouseState;
			for (int i = 0; i < 4; i++)
			{
				if (Program.m_PlayerManager.m_Player[i].m_Id != -1 && !((GamePadState)(ref Program.m_PlayerManager.m_Player[i].m_GamepadState)).IsConnected)
				{
					StartPause(Program.m_PlayerManager.m_Player[i].m_Id, Program.m_PlayerManager.m_Player[i].m_PadId);
				}
			}
			UpdateJukebox();
			break;
		}
		case STATE.EXIT_BUY:
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_PlayerOnePadId);
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)16384) && !Guide.IsVisible)
			{
				if (CanPurchaseContent(m_PlayerOnePadId))
				{
					Guide.ShowMarketplace(m_PlayerOnePadId);
				}
				else if (Gamer.SignedInGamers[m_PlayerOnePadId] != null)
				{
					m_PermissionTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 7000f;
				}
				else
				{
					Guide.ShowSignIn(1, true);
				}
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192) && !Fading())
			{
				m_NextState = STATE.EXITING_TRIAL;
				m_bFadeMusic = true;
				StartFade(up: false);
			}
			if (m_bWasBought)
			{
				m_NextState = STATE.MAINMENU;
				m_bFadeMusic = true;
				StartFade(up: false);
				m_bWasBought = false;
			}
			break;
		}
		return true;
	}

	public void UpdateFarseer()
	{
		if (!m_bEditor)
		{
			m_World.Step(0.01667f);
		}
	}

	public void StateStart()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		m_RumbleFrames = 0;
		switch (m_State)
		{
		case STATE.TITLE:
			m_Title.Start();
			break;
		case STATE.MAINMENU:
			if (Gamer.SignedInGamers[m_PlayerOnePadId] != null)
			{
				Gamer.SignedInGamers[m_PlayerOnePadId].Presence.PresenceMode = (GamerPresenceMode)46;
			}
			m_RenderPause = false;
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background1"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background1"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			if (m_PrevState == STATE.INGAME || m_PrevState == STATE.TITLE || m_PrevState == STATE.MAP)
			{
				StopJukebox();
				MediaPlayer.Play(m_MenuMusic);
				MediaPlayer.Volume = (float)m_Options.m_MusicVol / 100f;
				m_bFadeMusic = false;
			}
			Program.m_PlayerManager.ResetAll();
			Program.m_ItemManager.DeleteAll();
			Program.m_ParticleManager.DeleteAll();
			m_FrontEnd.Start();
			m_bUseDustMotes = true;
			break;
		case STATE.MAP:
			Program.m_PlayerManager.ResetAll();
			Program.m_ItemManager.DeleteAll();
			Program.m_ParticleManager.DeleteAll();
			m_RenderPause = false;
			m_Map.Start();
			m_bUseDustMotes = true;
			m_FrontEnd.m_bGameStarted = true;
			break;
		case STATE.INGAME:
			Program.m_PlayerManager.ResetAll();
			Program.m_ParticleManager.DeleteAll();
			Program.m_PlayerManager.m_Player[0].m_State = Player.State.WAITING_AT_START;
			m_ScrollSpeed = 0f;
			m_bScroll = false;
			if (m_Level == 5)
			{
				m_Bloom.settings = BloomSettings.PresetSettings[4];
			}
			else
			{
				m_Bloom.settings = BloomSettings.PresetSettings[0];
			}
			m_JoinTime = (float)m_GameTime.TotalGameTime.TotalMilliseconds + 15000f;
			m_JoinFlash = 0;
			if (!m_bUsingJukebox)
			{
				m_JukeBoxIdx = m_Rand.Next(12);
				PlaySong();
			}
			break;
		case STATE.OPTIONS:
			m_RenderPause = false;
			m_Options.Start();
			m_bUseDustMotes = true;
			break;
		case STATE.HELP:
			m_RenderPause = false;
			m_Help.Start();
			break;
		case STATE.CREDITS:
			m_RenderPause = false;
			m_Credits.Start();
			break;
		case STATE.FRONTENDSCORES:
			m_RenderPause = false;
			m_FrontEndScores.Start();
			break;
		case STATE.LEVEL_END:
			m_LevelEnd.Start();
			break;
		case STATE.EXITING:
			if (Guide.IsTrialMode)
			{
				m_State = STATE.EXIT_BUY;
				break;
			}
			mSyncManager.stop(delegate
			{
				((Game)this).Exit();
			});
			break;
		case STATE.EXITING_TRIAL:
			mSyncManager.stop(delegate
			{
				((Game)this).Exit();
			});
			break;
		case STATE.NEWGAME:
		case STATE.NEWGAME_ARE_YOU_SURE:
		case STATE.CONTINUEGAME:
		case STATE.EXIT_BUY:
		case STATE.MISSION_COMPLETE:
			break;
		}
	}

	public void StateEnd()
	{
		switch (m_State)
		{
		case STATE.MAINMENU:
			m_FrontEnd.Stop();
			break;
		case STATE.MAP:
			m_Map.Stop();
			break;
		case STATE.OPTIONS:
			m_Options.Stop();
			break;
		case STATE.HELP:
			m_Help.Stop();
			break;
		case STATE.CREDITS:
			m_Credits.Stop();
			break;
		case STATE.FRONTENDSCORES:
			m_FrontEndScores.Stop();
			break;
		case STATE.INGAME:
			Program.m_PlayerManager.GetPrimaryPlayer().StopAllEngineSounds();
			Program.m_PlayerManager.GetPrimaryPlayer().DeletePhysics();
			break;
		case STATE.LEVEL_END:
			m_LevelEnd.Stop();
			break;
		case STATE.TITLE:
		case STATE.NEWGAME:
		case STATE.NEWGAME_ARE_YOU_SURE:
		case STATE.CONTINUEGAME:
		case STATE.EXITING:
		case STATE.EXIT_BUY:
		case STATE.EXITING_TRIAL:
		case STATE.MISSION_COMPLETE:
			break;
		}
	}

	public void SaveLevel()
	{
		Program.m_TriggerManager.Save();
	}

	public void LoadLevel()
	{
		switch (m_Level)
		{
		case 1:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background1"), ((Game)this).Content.Load<Model>("Models/background1"));
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background1"), ((Game)this).Content.Load<Model>("Models/background1"));
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 2:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background1"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background1"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 3:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background1"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background1"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 4:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background1"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background1"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 5:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background3"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background3"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 6:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background3"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background3"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 7:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background3"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background3"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 8:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background4"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background4"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 9:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background4"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background4"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 10:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background4"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background4"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		case 11:
			m_BackgroundManager.m_Background[0].SetModel(((Game)this).Content.Load<Model>("Models/background2"), null);
			m_BackgroundManager.m_Background[1].SetModel(((Game)this).Content.Load<Model>("Models/background2"), null);
			ChangeEffectUsedByModel(m_BackgroundManager.m_Background[0].m_Model, cartoonEffect);
			break;
		}
		ReLoadLevel();
		Program.m_PlayerManager.ResetAll();
	}

	public void ReLoadLevel()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Program.m_TriggerManager.DeleteAll();
		Program.m_ItemManager.DeleteAll();
		Program.m_App.m_World.RemoveBody(Program.m_App.m_GroundBody);
		m_GroundBody = Program.m_App.m_World.CreateBody();
		PolygonShape val = new PolygonShape();
		val.SetAsEdge(new Vector2(-400f, 0f), new Vector2(4000f, 0f));
		Fixture val2 = m_GroundBody.CreateFixture((Shape)(object)val, 0f);
		val2.CollisionCategories = (CollisionCategory)1073741824;
		val2.CollidesWith = (CollisionCategory)1073741824;
		string text = $"Levels\\level{m_Level}.dat";
		text = Path.Combine(StorageContainer.TitleLocation, "Content\\" + text);
		StreamReader streamReader = new StreamReader(text);
		Program.m_TriggerManager.Load(streamReader);
		streamReader.Close();
	}

	public void IncrementLevel()
	{
		m_Level++;
		if (m_Level > m_LevelReached)
		{
			m_LevelReached = m_Level;
		}
		if (m_Level > 10)
		{
			m_Level = 1;
		}
	}

	public void DoChangeLevel()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if (m_Level > m_LevelReached && m_Level < 12)
		{
			m_LevelReached = m_Level;
		}
		m_LevelsVisited[m_Level] = 1;
		Program.m_TriggerManager.DeleteAll();
		Program.m_ItemManager.DeleteAll();
		Program.m_LoadSaveManager.ClearCheckpoint();
		if (Gamer.SignedInGamers[m_PlayerOnePadId] != null)
		{
			Gamer.SignedInGamers[m_PlayerOnePadId].Presence.PresenceMode = (GamerPresenceMode)9;
			Gamer.SignedInGamers[m_PlayerOnePadId].Presence.PresenceValue = m_Level;
		}
		LoadLevel();
		m_LevelPos = 0f;
		m_bScroll = false;
		m_ScrollSpeed = 0f;
		m_MaxScrollSpeed = 0.04f;
		Program.m_CameraManager3D.Init();
		GC.Collect();
	}

	public void SetLanguage()
	{
		string twoLetterISORegionName;
		if ((twoLetterISORegionName = RegionInfo.CurrentRegion.TwoLetterISORegionName) != null)
		{
			_ = twoLetterISORegionName == "GB";
		}
		m_Lang = LANG.ENGLISH;
	}

	public string GetText(TEXTID textId)
	{
		return aLocalisedText[(int)m_Lang, (int)textId];
	}

	public bool CanPurchaseContent(PlayerIndex player)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (Gamer.SignedInGamers[player] != null)
		{
			SignedInGamer val = Gamer.SignedInGamers[player];
			if (val != null)
			{
				return val.Privileges.AllowPurchaseContent;
			}
			return false;
		}
		return false;
	}

	public void RenderFade()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (m_fFadeAlpha == 0f)
		{
			m_bFadeMusic = false;
			return;
		}
		Color val = default(Color);
		((Color)(ref val))._002Ector(0f, 0f, 0f, m_fFadeAlpha);
		m_SpriteBatch.Begin((SpriteBlendMode)1);
		m_SpriteBatch.Draw(m_FadeTex, new Rectangle(0, 0, 1280, 720), (Rectangle?)null, val, 0f, new Vector2(0f, 0f), (SpriteEffects)0, 0f);
		m_SpriteBatch.End();
		if (m_bFadingUp)
		{
			float num = m_FrameLength * 0.001f * 2f;
			m_fFadeAlpha -= num;
			if (m_fFadeAlpha < 0f)
			{
				m_fFadeAlpha = 0f;
			}
		}
		else
		{
			float num2 = m_FrameLength * 0.001f * 2f;
			m_fFadeAlpha += num2;
			if (m_fFadeAlpha > 1f)
			{
				m_fFadeAlpha = 1f;
			}
		}
		if (m_bFadeMusic)
		{
			MediaPlayer.Volume = (float)m_Options.m_MusicVol / 100f * (1f - m_fFadeAlpha);
		}
	}

	public bool Fading()
	{
		if (m_fFadeAlpha == 0f || m_fFadeAlpha == 1f)
		{
			return false;
		}
		return true;
	}

	public void StartFade(bool up)
	{
		if (up)
		{
			m_bFadingUp = true;
			m_fFadeAlpha = 0.9999f;
		}
		else
		{
			m_bFadingUp = false;
			m_fFadeAlpha = 0.0001f;
		}
	}

	public int DifficultyScoreMultiplier()
	{
		return m_Options.m_Difficulty switch
		{
			1 => 1, 
			2 => 1, 
			3 => 2, 
			4 => 2, 
			_ => 1, 
		};
	}

	public void CheckInGamePurchase()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() == null)
		{
			return;
		}
		GamePadButtons buttons = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).Buttons;
		if ((int)((GamePadButtons)(ref buttons)).X == 1 && !Guide.IsVisible)
		{
			if (CanPurchaseContent(m_PlayerOnePadId))
			{
				Guide.ShowMarketplace(m_PlayerOnePadId);
			}
			else if (Gamer.SignedInGamers[m_PlayerOnePadId] != null)
			{
				Program.m_App.m_PermissionTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 7000f;
			}
			else
			{
				Guide.ShowSignIn(1, true);
			}
		}
	}

	public int NonRandomRandom(int r)
	{
		for (int i = 0; i < 100; i++)
		{
			int num = m_Rand.Next(r);
			if (num != m_LastRand)
			{
				m_LastRand = num;
				return num;
			}
		}
		return m_Rand.Next(r);
	}

	public void PlayNextSong()
	{
		if (MediaPlayer.GameHasControl)
		{
			m_JukeBoxIdx++;
			if (m_JukeBoxIdx > 11)
			{
				m_JukeBoxIdx = 0;
			}
			PlaySong();
		}
	}

	public void PlayPrevSong()
	{
		if (MediaPlayer.GameHasControl)
		{
			m_JukeBoxIdx--;
			if (m_JukeBoxIdx < 0)
			{
				m_JukeBoxIdx = 11;
			}
			PlaySong();
		}
	}

	private void PlaySong()
	{
		switch (m_JukeBoxIdx)
		{
		case 0:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/George_Ellinas_Pulse");
			break;
		case 1:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/Audionautix_rockerchick");
			break;
		case 2:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/DJ_RKod_Bullet");
			break;
		case 3:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/Zapac_Test_Drive");
			break;
		case 4:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/Incompetech_Tafi_Maradi");
			break;
		case 5:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/George_Ellinas_Dreamers_Choise");
			break;
		case 6:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/TDeeJay_DisTOrATion");
			break;
		case 7:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/Audionautix_GreenDaze");
			break;
		case 8:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/Zapac_Put_your_hands_up");
			break;
		case 9:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/Audionautix_PurpleRider");
			break;
		case 10:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/zikweb_B34");
			break;
		case 11:
			m_JukeBox = ((Game)this).Content.Load<Song>("Sounds/Music/Audionautix_GroovyBaby");
			break;
		}
		if (MediaPlayer.GameHasControl)
		{
			MediaPlayer.Play(m_JukeBox);
			m_bUsingJukebox = true;
			m_MusicChangeTime = (float)m_GameTime.TotalGameTime.TotalSeconds + 5f;
		}
	}

	public void StopJukebox()
	{
		m_bUsingJukebox = false;
		MediaPlayer.Stop();
	}

	private void UpdateJukebox()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (m_bUsingJukebox && (int)m_MediaState == 0 && m_MusicChangeTime < (float)m_GameTime.TotalGameTime.TotalSeconds)
		{
			PlayNextSong();
			m_MusicChangeTime = (float)m_GameTime.TotalGameTime.TotalSeconds + 5f;
		}
	}

	private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_MediaState = MediaPlayer.State;
	}

	public bool IsIdle()
	{
		if (m_MenuIdleTime > 3000f)
		{
			return true;
		}
		return false;
	}
}
