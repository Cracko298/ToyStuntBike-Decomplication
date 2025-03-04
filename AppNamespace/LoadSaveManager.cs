using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;

namespace AppNamespace;

public class LoadSaveManager
{
	public Vector2 m_Checkpoint;

	public float m_CheckpointDisplayTime;

	public Item[] m_Enemy;

	public int[] m_FlagsCollected;

	public int[] m_CupsCollected;

	public Trigger[] m_Trigger;

	public SaveGame m_SaveGame;

	public float m_TrackTime;

	public int m_Score;

	public LoadSaveManager()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		m_Checkpoint = Vector2.Zero;
		m_FlagsCollected = new int[30];
		for (int i = 0; i < 30; i++)
		{
			m_FlagsCollected[i] = 0;
		}
		m_CupsCollected = new int[30];
		for (int j = 0; j < 30; j++)
		{
			m_CupsCollected[j] = 0;
		}
		m_Score = 0;
		m_TrackTime = 0f;
		m_SaveGame = new SaveGame();
	}

	public void ClearCheckpoint()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_Checkpoint = Vector2.Zero;
	}

	public void SaveCheckpoint(Vector2 pos, bool bShowMessage)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = pos + new Vector2(0f, 0f);
		if (!(m_Checkpoint == val) || !(pos != Vector2.Zero))
		{
			m_Checkpoint = val;
			Program.m_ItemManager.CopyFlagsCollected(Program.m_ItemManager.m_FlagsCollected, m_FlagsCollected);
			Program.m_ItemManager.CopyCupsCollected(Program.m_ItemManager.m_CupsCollected, m_CupsCollected);
			m_Score = Program.m_PlayerManager.GetPrimaryPlayer().m_Score;
			m_TrackTime = Program.m_App.m_TrackTime;
			if (bShowMessage)
			{
				m_CheckpointDisplayTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalSeconds + 2f;
				Program.m_App.m_TextScale = 3f;
				Program.m_SoundManager.Play(32);
			}
		}
	}

	public void LoadCheckpoint(ref Vector2 pos)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		pos = m_Checkpoint;
		Program.m_ItemManager.CopyFlagsCollected(m_FlagsCollected, Program.m_ItemManager.m_FlagsCollected);
		Program.m_ItemManager.CopyCupsCollected(m_CupsCollected, Program.m_ItemManager.m_CupsCollected);
		Program.m_PlayerManager.GetPrimaryPlayer().m_Score = m_Score;
		Program.m_App.m_TrackTime = m_TrackTime;
	}

	public void Update()
	{
	}

	public void SaveGame()
	{
		try
		{
			if (!Guide.IsTrialMode && !Guide.IsVisible)
			{
				if (App.m_StorageDevice != null && App.m_StorageDevice.IsConnected)
				{
					SaveGameCallback(null);
				}
				else
				{
					Guide.BeginShowStorageDeviceSelector((AsyncCallback)SaveGameCallback, (object)null);
				}
				Program.m_App.m_bSaveExists = true;
			}
		}
		catch (Exception)
		{
		}
	}

	private void SaveGameCallback(IAsyncResult result)
	{
		if (result != null && result.IsCompleted)
		{
			App.m_StorageDevice = Guide.EndShowStorageDeviceSelector(result);
		}
		if (App.m_StorageDevice == null || !App.m_StorageDevice.IsConnected)
		{
			return;
		}
		Program.m_App.m_SaveError = false;
		Program.m_App.m_SaveErrorNoSpace = false;
		try
		{
			long totalSpace = App.m_StorageDevice.TotalSpace;
			if (totalSpace < 74752)
			{
				Program.m_App.m_SaveErrorNoSpace = true;
				return;
			}
			StorageContainer val = App.m_StorageDevice.OpenContainer("Toy Stunt Bike");
			try
			{
				string path = Path.Combine(val.Path, "gamesave.xml");
				using (FileStream stream = File.Create(path))
				{
					m_SaveGame.Update();
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveGame));
					xmlSerializer.Serialize(stream, m_SaveGame);
				}
				string path2 = Path.Combine(val.Path, "scores");
				using FileStream output = File.Create(path2);
				BinaryWriter binaryWriter = new BinaryWriter(output);
				Program.m_App.mScores.save(binaryWriter);
				binaryWriter.Close();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception)
		{
		}
	}

	public void LoadGame()
	{
		try
		{
			if (!Guide.IsTrialMode && !Guide.IsVisible)
			{
				if (App.m_StorageDevice != null && App.m_StorageDevice.IsConnected)
				{
					LoadGameCallback(null);
				}
				else
				{
					Guide.BeginShowStorageDeviceSelector((AsyncCallback)LoadGameCallback, (object)null);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void LoadGameCallback(IAsyncResult result)
	{
		if (result != null && result.IsCompleted)
		{
			App.m_StorageDevice = Guide.EndShowStorageDeviceSelector(result);
		}
		if (App.m_StorageDevice == null || !App.m_StorageDevice.IsConnected)
		{
			return;
		}
		try
		{
			StorageContainer val = App.m_StorageDevice.OpenContainer("Toy Stunt Bike");
			try
			{
				string path = Path.Combine(val.Path, "gamesave.xml");
				if (File.Exists(path))
				{
					using FileStream stream = File.Open(path, FileMode.Open);
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveGame));
					m_SaveGame = (SaveGame)xmlSerializer.Deserialize(stream);
					m_SaveGame.Restore();
					Program.m_App.m_bSaveExists = true;
				}
				else
				{
					Program.m_App.m_bSaveExists = false;
				}
				string path2 = Path.Combine(val.Path, "scores");
				if (File.Exists(path2))
				{
					using (FileStream input = File.Open(path2, FileMode.Open))
					{
						BinaryReader binaryReader = new BinaryReader(input);
						Program.m_App.mScores = new TopScoreListContainer(binaryReader);
						binaryReader.Close();
						return;
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception)
		{
		}
	}
}
