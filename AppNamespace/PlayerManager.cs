using System;
using AvatarWrapper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class PlayerManager
{
	public const int MAX_PLAYERS = 4;

	public Player[] m_Player;

	public Vector2 m_SpawnPos;

	public Vector2 m_TargetPos;

	public Model[] m_Model;

	public bool m_bAlignToLandingTower;

	public int m_RequestJoin = -1;

	public PlayerManager()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		m_Player = new Player[4];
		for (int i = 0; i < 4; i++)
		{
			m_Player[i] = new Player();
		}
		for (int j = 0; j < 4; j++)
		{
			m_Player[j].m_Id = -1;
		}
		m_Model = (Model[])(object)new Model[4];
		for (int k = 0; k < 4; k++)
		{
			m_Model[k] = null;
		}
		m_SpawnPos = Vector2.Zero;
		m_TargetPos = Vector2.Zero;
		m_bAlignToLandingTower = false;
	}

	public Player Create()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id == -1)
			{
				m_Player[i].m_Id = i;
				m_Player[i].m_Position = Vector2.Zero;
				m_Player[i].m_Rotation.Y = -(float)Math.PI / 2f;
				m_Player[i].m_OldGamepadState = GamePad.GetState((PlayerIndex)i);
				m_Player[i].m_GamepadState = GamePad.GetState((PlayerIndex)i);
				m_Player[i].m_PadId = (PlayerIndex)m_Player[i].m_Id;
				m_Player[i].m_bOnGround = false;
				m_Player[i].m_bPrevOnGround = false;
				m_Player[i].m_bDebounceJump = false;
				m_Player[i].m_bDoCollision = true;
				m_Player[i].m_Avatar = null;
				m_Player[i].m_AssignAvatarCount = 100;
				m_Player[i].m_PropRot = 0f;
				m_Player[i].m_Roll = 0f;
				m_Player[i].m_Height = 0f;
				m_Player[i].m_LAX = 0f;
				m_Player[i].m_LAY = 0f;
				m_Player[i].m_SmokeTrailId = -1;
				m_Player[i].m_WheelTrailId = -1;
				m_Player[i].m_SparkTrailId = -1;
				m_Player[i].m_fNextFireTime = 0f;
				m_Player[i].m_bDying = false;
				m_Player[i].m_Health = 100;
				m_Player[i].m_Recover = 0;
				m_Player[i].m_MaxRecover = 6;
				m_Player[i].m_Score = 0;
				m_Player[i].m_ScoreTally = 0;
				m_Player[i].m_LevelScore = 0;
				m_Player[i].m_bCanBeOnGround = true;
				m_Player[i].m_TaxiingTime = 0f;
				m_Player[i].m_WaitingAtTowerTime = 0f;
				m_Player[i].m_RumbleFrames = 0;
				m_Player[i].m_EngineSound = null;
				m_Player[i].m_State = Player.State.WAITING_AT_START;
				m_Player[i].m_DeathSmokeId = -1;
				m_Player[i].m_DeathSparksId = -1;
				m_Player[i].m_Rank = 0;
				m_Player[i].m_InvulnerableTime = 0f;
				m_Player[i].m_ModelId = i;
				m_Player[i].m_bSignedIn = false;
				m_Player[i].SetupPhysics(bReset: false, Vector2.Zero);
				return m_Player[i];
			}
		}
		return null;
	}

	public void Delete(int id)
	{
		m_Player[id].Reset();
		m_Player[id].Delete();
	}

	public void DeleteAll()
	{
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1)
			{
				m_Player[i].Reset();
				m_Player[i].Delete();
			}
		}
	}

	public void Render()
	{
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1)
			{
				m_Player[i].Render();
			}
		}
	}

	public void RenderAvatars()
	{
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1)
			{
				m_Player[i].RenderAvatar();
			}
		}
	}

	public void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1)
			{
				m_Player[i].Update();
			}
		}
		CheckSignOuts();
	}

	public void CheckDropIn()
	{
	}

	public bool PlayerExistsWithPad(PlayerIndex padId)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1 && m_Player[i].m_PadId == padId)
			{
				return true;
			}
		}
		return false;
	}

	public int GetPlayerExistsWithPad(PlayerIndex padId)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1 && m_Player[i].m_PadId == padId)
			{
				return m_Player[i].m_Id;
			}
		}
		return -1;
	}

	public void CheckSignOuts()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1 && Gamer.SignedInGamers[m_Player[i].m_PadId] == null && m_Player[i].m_bSignedIn)
			{
				m_Player[i].m_Avatar = new Avatar(AvatarDescription.CreateRandom());
				m_Player[i].m_Avatar.StartAnimation(Program.m_App.m_IdleGripAnimation, LoopAnimation: false);
				m_Player[i].m_bSignedIn = false;
			}
		}
	}

	public void DropOut(int id)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (m_Player[id].m_PadId == Program.m_App.m_PlayerOnePadId)
		{
			for (int i = 0; i < 4; i++)
			{
				if (m_Player[i].m_Id != -1 && m_Player[i].m_Id != id)
				{
					Program.m_App.m_PlayerOnePadId = m_Player[i].m_PadId;
					break;
				}
			}
		}
		Delete(id);
	}

	public int NumPlayers()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1)
			{
				num++;
			}
		}
		return num;
	}

	public int NearPos(Vector2 pos, float fTolSq)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = 1E+09f;
		int result = -1;
		if (Program.m_App.m_bEditor)
		{
			return -1;
		}
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1 && m_Player[i].m_bDoCollision)
			{
				Vector2 val = pos - m_Player[i].m_Position;
				num = ((Vector2)(ref val)).LengthSquared();
				if (num < fTolSq && num < num2)
				{
					num2 = num;
					result = i;
				}
			}
		}
		return result;
	}

	public Vector2 FindNearestScreenPos(Vector2 screenPos)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = 1E+09f;
		int num3 = -1;
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1 && m_Player[i].m_bDoCollision)
			{
				Vector2 val = screenPos - m_Player[i].m_Position;
				num = ((Vector2)(ref val)).LengthSquared();
				if (num < num2)
				{
					num2 = num;
					num3 = i;
				}
			}
		}
		if (num3 == -1)
		{
			return Vector2.Zero;
		}
		return new Vector2(m_Player[num3].m_ScreenPos.X, m_Player[num3].m_ScreenPos.Y);
	}

	public void AddScore(int score)
	{
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1)
			{
				m_Player[i].m_ScoreTally += score * Program.m_App.DifficultyScoreMultiplier();
			}
		}
	}

	public void HandleRumbleAll()
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1)
			{
				if (Program.m_App.m_RumbleFrames > 0)
				{
					m_Player[i].m_RumbleFrames += Program.m_App.m_RumbleFrames;
				}
				if (m_Player[i].m_RumbleFrames > 0 && Program.m_App.m_Options.m_bVibration)
				{
					m_Player[i].m_RumbleFrames--;
					GamePad.SetVibration(m_Player[i].m_PadId, 0.5f, 0.5f);
				}
				else
				{
					GamePad.SetVibration(m_Player[i].m_PadId, 0f, 0f);
				}
			}
		}
		Program.m_App.m_RumbleFrames = 0;
	}

	public void ResetAll()
	{
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id != -1)
			{
				m_Player[i].Reset();
			}
		}
		m_bAlignToLandingTower = false;
	}

	public void CalculateRank()
	{
		for (int i = 0; i < 4; i++)
		{
			if (m_Player[i].m_Id == -1)
			{
				continue;
			}
			m_Player[i].m_Rank = 0;
			for (int j = 0; j < 4; j++)
			{
				if (m_Player[j].m_Id != -1 && i != j && m_Player[i].m_Score < m_Player[j].m_Score)
				{
					m_Player[i].m_Rank++;
				}
			}
		}
	}

	public Player GetPrimaryPlayer()
	{
		return m_Player[0];
	}

	public void AlignToLandingTower()
	{
		m_bAlignToLandingTower = true;
	}
}
