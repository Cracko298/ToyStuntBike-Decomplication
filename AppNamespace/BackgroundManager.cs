namespace AppNamespace;

public class BackgroundManager
{
	public const float TILE_SIZE = 101f;

	public const float TILE_CAMERA_BIAS = 40f;

	public const float NUM_TILES = 2f;

	private App m_App;

	public Actor[] m_Background;

	private int m_CurrentTileId;

	private int m_NextTileId;

	public BackgroundManager(App app)
	{
		m_App = app;
		m_CurrentTileId = 0;
		m_NextTileId = 1;
		m_Background = new Actor[2];
		m_Background[0] = new Actor();
		m_Background[1] = new Actor();
		m_Background[0].m_Position.Y = 0f;
		m_Background[0].m_ZDistance = 0f;
		m_Background[1].m_Position.Y = 0f;
		m_Background[1].m_Position.X = -101f;
		m_Background[1].m_ZDistance = 0f;
	}

	public void Start()
	{
	}

	public void Update()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer().m_Ragdoll.Body.LinearVelocity.X >= 0f)
		{
			if (Program.m_CameraManager3D.m_CameraPositionTarget.X > m_Background[m_CurrentTileId].m_Position.X - 40f)
			{
				m_Background[m_NextTileId].m_Position.X = m_Background[m_CurrentTileId].m_Position.X + 101f;
				m_CurrentTileId = m_NextTileId;
				m_NextTileId++;
				if ((float)m_NextTileId >= 2f)
				{
					m_NextTileId = 0;
				}
			}
		}
		else if (Program.m_CameraManager3D.m_CameraPositionTarget.X < m_Background[m_CurrentTileId].m_Position.X - 40f)
		{
			m_Background[m_NextTileId].m_Position.X = m_Background[m_CurrentTileId].m_Position.X - 101f;
			m_CurrentTileId = m_NextTileId;
			m_NextTileId++;
			if ((float)m_NextTileId >= 2f)
			{
				m_NextTileId = 0;
			}
		}
	}

	public void Reset()
	{
		m_Background[0].m_Position.X = 0f;
		m_Background[1].m_Position.X = 0f;
		m_CurrentTileId = 0;
		m_NextTileId = 1;
	}

	public void Draw()
	{
	}
}
