using Microsoft.Xna.Framework;

namespace AppNamespace;

public class Trigger
{
	public enum OBJ
	{
		TOYBLOCK_A,
		BOOK_LARGE_25DEG,
		CHAIR1,
		PENCIL,
		TOYBLOCK_B,
		TOYBLOCK_C,
		TOYBLOCK_A_STATIC,
		TOYBLOCK_B_STATIC,
		TOYBLOCK_C_STATIC,
		RULER,
		BUS,
		CHESSBOARD,
		DRAWINGPIN,
		START,
		FINISH,
		SWORD,
		SWORD2,
		SHIELD,
		SMALLTABLE,
		BOOK2,
		SKISLOPE,
		FLAG1,
		FLAG2,
		FLAG3,
		SKISLOPE_REVERSE,
		TRACK_1M,
		TRACK_50CM,
		TRACK_1M_UP_60,
		TRACK_1M_DOWN_60,
		TRACK_50CM_UP_30,
		TRACK_50CM_DOWN_30,
		TRACK_1M_LOOP,
		DOMINO_1,
		DOMINO_2,
		DOMINO_3,
		DOMINO_4,
		CHAIR2,
		BOOK3,
		HOB,
		MICROWAVE,
		PLACEMAT,
		KNIFE,
		COFFEE,
		TEA,
		SUGAR,
		BISCUITS,
		BOWL,
		TOASTER,
		SPOON,
		MUG,
		PLATE,
		ROLLINGPIN,
		SPADE,
		BALL,
		GARDENCHAIR,
		PLANK_1M,
		PLANK_50CM,
		BRICK,
		RAKE,
		PLANTPOT,
		PLANK2_1M,
		PLANK2_50CM,
		CHECKPOINT,
		END
	}

	public enum TRIGGER_STATE
	{
		SUSPENDED,
		ACTIVE,
		COMPLETED
	}

	public const int FLAG_ONCE = 1;

	public const int FLAG_OFFSCREEN = 2;

	public const int FLAG_RENDER = 4;

	public const int FLAG_UPDATE = 8;

	public int m_Id;

	public int m_Type;

	public Actor m_LinkActor;

	public TRIGGER_STATE m_State;

	public int m_Flags;

	public Vector2 m_Position;

	public bool m_bDistanceTrigger;

	public float m_Rotation;

	public Trigger()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		m_Id = -1;
		m_Type = -1;
		m_LinkActor = null;
		m_State = TRIGGER_STATE.SUSPENDED;
		m_Flags = 0;
		m_Position = Vector2.Zero;
		m_bDistanceTrigger = true;
		m_Rotation = 0f;
	}

	public void Activate()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		m_State = TRIGGER_STATE.ACTIVE;
		Program.m_ItemManager.Create(m_Type, m_Id, m_Position, m_Rotation);
		m_State = TRIGGER_STATE.COMPLETED;
	}

	public void Suspend()
	{
	}

	public void Complete()
	{
	}

	public void Update()
	{
	}

	public void Render()
	{
	}

	public void SetOffScreen()
	{
		m_Flags |= 2;
	}

	public bool OffScreen(float fTol)
	{
		return false;
	}
}
