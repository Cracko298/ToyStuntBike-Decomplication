using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace AppNamespace;

public class Item : Actor
{
	public int m_Type;

	public int m_TriggerId;

	public Vector2 m_TriggerPos;

	public Vector2 m_Accel;

	public bool m_Active;

	public int m_FrameCnt;

	public bool m_bDying;

	public int m_Health = 100;

	public int m_MaxHealth;

	public bool m_bDead;

	public bool m_bHasPlayerCollision = true;

	public bool m_bCastShadows = true;

	public int m_Status;

	public int m_Layer;

	public bool m_bShowHealth = true;

	public SoundEffectInstance m_ItemSound;

	public Fixture m_Fixture;

	public int m_UniqueId = -1;

	public Item()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		m_Type = -1;
		m_Id = -1;
		m_fScale = 1f;
		m_TriggerId = -1;
		m_TriggerPos = Vector2.Zero;
		m_Velocity = Vector2.Zero;
		m_Accel = Vector2.Zero;
		m_Active = false;
	}

	public bool OnScreen()
	{
		if (Program.m_App.m_bEditor)
		{
			return true;
		}
		if (m_Position.X > Program.m_CameraManager3D.m_CameraPosition.X + 60f || m_Position.X < Program.m_CameraManager3D.m_CameraPosition.X - 20f)
		{
			return false;
		}
		return true;
	}

	public void Render()
	{
	}

	public void Update()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (m_FlashModel > 0)
		{
			m_FlashModel--;
		}
		UpdateShake();
		if (m_Fixture != null)
		{
			m_Position.X = m_Fixture.Body.Position.X;
			m_Position.Y = m_Fixture.Body.Position.Y;
			m_Rotation.Z = m_Fixture.Body.Rotation;
		}
		Vector3 zero = Vector3.Zero;
		zero.X = m_Position.X;
		zero.Y = m_Position.Y;
		zero.Z = m_ZDistance;
		m_ScreenPos = Program.m_CameraManager3D.WorldToScreen(zero);
		CheckPlayerCollision();
		if (m_bDead)
		{
			Delete();
		}
	}

	public void Delete()
	{
		m_Id = -1;
		if (m_ItemSound != null)
		{
			m_ItemSound.Stop();
		}
		if (m_Fixture != null)
		{
			Program.m_App.m_World.RemoveBody(m_Fixture.Body);
			m_Fixture = null;
		}
	}

	public bool TakeDamage(int damage, int ownerId)
	{
		return false;
	}

	public bool TakeDamage(int damage, Trigger.OBJ type, int ownerId)
	{
		if (ownerId != -1)
		{
			damage *= 2;
		}
		TakeDamage(damage, ownerId);
		return true;
	}

	public void SetDying()
	{
		m_bDying = true;
	}

	public void CheckPlayerCollision()
	{
		if (Program.m_App.m_bEditor || (m_Type != 21 && m_Type != 22 && m_Type != 23 && m_Type != 12 && m_Type != 38 && m_Type != 58 && m_Type != 62))
		{
			return;
		}
		CalcBounds2D(bUseAll: false, bUsePitch: false);
		if (!IsBoundsValid())
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (Program.m_PlayerManager.m_Player[i].m_Id != -1 && !(Program.m_PlayerManager.m_Player[i].m_Bounds2DMin.X < 0f) && !(Program.m_PlayerManager.m_Player[i].m_Bounds2DMax.X > 1280f))
			{
				float num = 0f;
				float num2 = 0f;
				if (m_Type == 21 || m_Type == 22 || m_Type == 23)
				{
					num = -60f;
					num2 = -60f;
				}
				float num3 = -40f;
				if (m_Type == 12)
				{
					num2 = -30f;
				}
				if (m_Bounds2DMax.X - num > Program.m_PlayerManager.m_Player[i].m_Bounds2DMin.X && m_Bounds2DMin.X + num < Program.m_PlayerManager.m_Player[i].m_Bounds2DMax.X && m_Bounds2DMax.Y - num2 > Program.m_PlayerManager.m_Player[i].m_Bounds2DMin.Y + num3 && m_Bounds2DMin.Y + num2 < Program.m_PlayerManager.m_Player[i].m_Bounds2DMax.Y - num3)
				{
					Program.m_PlayerManager.m_Player[i].GivePickup(m_Id);
				}
			}
		}
	}

	public bool IsBoundsValid()
	{
		if (m_Bounds2DMax.X > 2000f || m_Bounds2DMax.Y > 2000f || m_Bounds2DMin.X < -2000f || m_Bounds2DMin.Y < -2000f)
		{
			return false;
		}
		return true;
	}

	public void CollectFlag()
	{
		if (Program.m_ItemManager.m_FlagsCollected[m_UniqueId] == 0)
		{
			Program.m_ItemManager.m_FlagsCollected[m_UniqueId] = 1;
		}
	}
}
