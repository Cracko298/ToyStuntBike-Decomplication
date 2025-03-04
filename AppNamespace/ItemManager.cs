using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppNamespace;

internal class ItemManager
{
	public const int MAX_ITEMS = 256;

	public const int MAX_FLAGS = 30;

	public const int MAX_CUPS = 30;

	private const float S = 2.54f;

	public Item[] m_Item;

	public Vector2[] m_ModelSize2D;

	public Vector3[] m_ModelSizeMax3D;

	public Vector3[] m_ModelSizeMin3D;

	public Model[] m_Model;

	public Model[] m_ModelLOD;

	public int[] m_FlagsCollected;

	public int[] m_CupsCollected;

	private int m_NextId;

	private Vector2 m_LastEdge = Vector2.Zero;

	private float m_ItemImpactSoundTime;

	public ItemManager()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_Item = new Item[256];
		for (int i = 0; i < 256; i++)
		{
			m_Item[i] = new Item();
		}
		m_ModelSize2D = (Vector2[])(object)new Vector2[63];
		m_ModelSizeMax3D = (Vector3[])(object)new Vector3[63];
		m_ModelSizeMin3D = (Vector3[])(object)new Vector3[63];
		m_Model = (Model[])(object)new Model[63];
		m_ModelLOD = (Model[])(object)new Model[63];
		m_FlagsCollected = new int[30];
		for (int j = 0; j < 30; j++)
		{
			m_FlagsCollected[j] = 0;
		}
		m_CupsCollected = new int[30];
		for (int k = 0; k < 30; k++)
		{
			m_CupsCollected[k] = 0;
		}
	}

	public int TotalFlagsCollected()
	{
		int num = 0;
		for (int i = 0; i < 30; i++)
		{
			if (m_FlagsCollected[i] > 0)
			{
				num++;
			}
		}
		return num;
	}

	public int TotalCupsCollected()
	{
		int num = 0;
		for (int i = 0; i < 30; i++)
		{
			if (m_CupsCollected[i] > 0)
			{
				num++;
			}
		}
		return num;
	}

	public void ClearFlagsCollected()
	{
		for (int i = 0; i < 30; i++)
		{
			m_FlagsCollected[i] = 0;
		}
	}

	public void ClearCupsCollected()
	{
		for (int i = 0; i < 30; i++)
		{
			m_CupsCollected[i] = 0;
		}
	}

	public void CopyFlagsCollected(int[] src, int[] dest)
	{
		for (int i = 0; i < 30; i++)
		{
			dest[i] = src[i];
		}
	}

	public void CopyCupsCollected(int[] src, int[] dest)
	{
		for (int i = 0; i < 30; i++)
		{
			dest[i] = src[i];
		}
	}

	public int GetNumFlagsCollectedOnLevel(int level)
	{
		int num = 0;
		int num2 = level * 3;
		if (m_FlagsCollected[num2] > 0)
		{
			num++;
		}
		if (m_FlagsCollected[num2 + 1] > 0)
		{
			num++;
		}
		if (m_FlagsCollected[num2 + 2] > 0)
		{
			num++;
		}
		return num;
	}

	public void GiveTimeCupOnLevel(int level)
	{
		int num = level * 3;
		m_CupsCollected[num] = 1;
	}

	public int GetTimeCupOnLevel(int level)
	{
		int num = level * 3;
		return m_CupsCollected[num];
	}

	public void GiveScoreCupOnLevel(int level)
	{
		int num = level * 3;
		m_CupsCollected[num + 1] = 1;
	}

	public int GetScoreCupOnLevel(int level)
	{
		int num = level * 3;
		return m_CupsCollected[num + 1];
	}

	public void GiveFlagsCupOnLevel(int level)
	{
		int num = level * 3;
		m_CupsCollected[num + 2] = 1;
	}

	public int GetFlagsCupOnLevel(int level)
	{
		int num = level * 3;
		return m_CupsCollected[num + 2];
	}

	public int Create(int type, int triggerId, Vector2 pos, float rot)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Expected O, but got Unknown
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Expected O, but got Unknown
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Expected O, but got Unknown
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Expected O, but got Unknown
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Expected O, but got Unknown
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Expected O, but got Unknown
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_0976: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Expected O, but got Unknown
		//IL_0a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfd: Expected O, but got Unknown
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4d: Expected O, but got Unknown
		//IL_0d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f98: Expected O, but got Unknown
		//IL_0fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1018: Unknown result type (might be due to invalid IL or missing references)
		//IL_101f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Unknown result type (might be due to invalid IL or missing references)
		//IL_1026: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Unknown result type (might be due to invalid IL or missing references)
		//IL_103d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1042: Unknown result type (might be due to invalid IL or missing references)
		//IL_1044: Unknown result type (might be due to invalid IL or missing references)
		//IL_108b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1092: Unknown result type (might be due to invalid IL or missing references)
		//IL_1097: Unknown result type (might be due to invalid IL or missing references)
		//IL_1099: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1105: Unknown result type (might be due to invalid IL or missing references)
		//IL_110a: Unknown result type (might be due to invalid IL or missing references)
		//IL_110c: Unknown result type (might be due to invalid IL or missing references)
		//IL_111c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1123: Unknown result type (might be due to invalid IL or missing references)
		//IL_1128: Unknown result type (might be due to invalid IL or missing references)
		//IL_112a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1171: Unknown result type (might be due to invalid IL or missing references)
		//IL_1178: Unknown result type (might be due to invalid IL or missing references)
		//IL_117d: Unknown result type (might be due to invalid IL or missing references)
		//IL_117f: Unknown result type (might be due to invalid IL or missing references)
		//IL_118f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1196: Unknown result type (might be due to invalid IL or missing references)
		//IL_119b: Unknown result type (might be due to invalid IL or missing references)
		//IL_119d: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ca: Expected O, but got Unknown
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11de: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1201: Unknown result type (might be due to invalid IL or missing references)
		//IL_1203: Unknown result type (might be due to invalid IL or missing references)
		//IL_1242: Unknown result type (might be due to invalid IL or missing references)
		//IL_1249: Expected O, but got Unknown
		//IL_1256: Unknown result type (might be due to invalid IL or missing references)
		//IL_125d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1262: Unknown result type (might be due to invalid IL or missing references)
		//IL_1264: Unknown result type (might be due to invalid IL or missing references)
		//IL_1274: Unknown result type (might be due to invalid IL or missing references)
		//IL_127b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1280: Unknown result type (might be due to invalid IL or missing references)
		//IL_1282: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_133c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1343: Unknown result type (might be due to invalid IL or missing references)
		//IL_1348: Unknown result type (might be due to invalid IL or missing references)
		//IL_134a: Unknown result type (might be due to invalid IL or missing references)
		//IL_135a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1361: Unknown result type (might be due to invalid IL or missing references)
		//IL_1366: Unknown result type (might be due to invalid IL or missing references)
		//IL_1368: Unknown result type (might be due to invalid IL or missing references)
		//IL_13af: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13db: Unknown result type (might be due to invalid IL or missing references)
		//IL_1422: Unknown result type (might be due to invalid IL or missing references)
		//IL_1429: Unknown result type (might be due to invalid IL or missing references)
		//IL_142e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1430: Unknown result type (might be due to invalid IL or missing references)
		//IL_1440: Unknown result type (might be due to invalid IL or missing references)
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_144c: Unknown result type (might be due to invalid IL or missing references)
		//IL_144e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1495: Unknown result type (might be due to invalid IL or missing references)
		//IL_149c: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1508: Unknown result type (might be due to invalid IL or missing references)
		//IL_150f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1514: Unknown result type (might be due to invalid IL or missing references)
		//IL_1516: Unknown result type (might be due to invalid IL or missing references)
		//IL_1526: Unknown result type (might be due to invalid IL or missing references)
		//IL_152d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1532: Unknown result type (might be due to invalid IL or missing references)
		//IL_1534: Unknown result type (might be due to invalid IL or missing references)
		//IL_157b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1582: Unknown result type (might be due to invalid IL or missing references)
		//IL_1587: Unknown result type (might be due to invalid IL or missing references)
		//IL_1589: Unknown result type (might be due to invalid IL or missing references)
		//IL_1599: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_160c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1613: Unknown result type (might be due to invalid IL or missing references)
		//IL_1618: Unknown result type (might be due to invalid IL or missing references)
		//IL_161a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1659: Unknown result type (might be due to invalid IL or missing references)
		//IL_1660: Expected O, but got Unknown
		//IL_166d: Unknown result type (might be due to invalid IL or missing references)
		//IL_167c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1683: Unknown result type (might be due to invalid IL or missing references)
		//IL_168d: Unknown result type (might be due to invalid IL or missing references)
		//IL_169c: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_16bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_16dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1703: Unknown result type (might be due to invalid IL or missing references)
		//IL_170d: Unknown result type (might be due to invalid IL or missing references)
		//IL_171c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1723: Unknown result type (might be due to invalid IL or missing references)
		//IL_172d: Unknown result type (might be due to invalid IL or missing references)
		//IL_173c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1743: Unknown result type (might be due to invalid IL or missing references)
		//IL_174d: Unknown result type (might be due to invalid IL or missing references)
		//IL_175c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1763: Unknown result type (might be due to invalid IL or missing references)
		//IL_176d: Unknown result type (might be due to invalid IL or missing references)
		//IL_177c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1783: Unknown result type (might be due to invalid IL or missing references)
		//IL_178d: Unknown result type (might be due to invalid IL or missing references)
		//IL_179c: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_17bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_17dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Unknown result type (might be due to invalid IL or missing references)
		//IL_180d: Unknown result type (might be due to invalid IL or missing references)
		//IL_181c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1823: Unknown result type (might be due to invalid IL or missing references)
		//IL_182d: Unknown result type (might be due to invalid IL or missing references)
		//IL_183c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1843: Unknown result type (might be due to invalid IL or missing references)
		//IL_184d: Unknown result type (might be due to invalid IL or missing references)
		//IL_185c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1863: Unknown result type (might be due to invalid IL or missing references)
		//IL_186d: Unknown result type (might be due to invalid IL or missing references)
		//IL_187c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1883: Unknown result type (might be due to invalid IL or missing references)
		//IL_188d: Unknown result type (might be due to invalid IL or missing references)
		//IL_189c: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d6: Expected O, but got Unknown
		//IL_18e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1903: Unknown result type (might be due to invalid IL or missing references)
		//IL_1912: Unknown result type (might be due to invalid IL or missing references)
		//IL_1919: Unknown result type (might be due to invalid IL or missing references)
		//IL_1923: Unknown result type (might be due to invalid IL or missing references)
		//IL_1932: Unknown result type (might be due to invalid IL or missing references)
		//IL_1939: Unknown result type (might be due to invalid IL or missing references)
		//IL_1943: Unknown result type (might be due to invalid IL or missing references)
		//IL_1952: Unknown result type (might be due to invalid IL or missing references)
		//IL_1959: Unknown result type (might be due to invalid IL or missing references)
		//IL_1963: Unknown result type (might be due to invalid IL or missing references)
		//IL_1972: Unknown result type (might be due to invalid IL or missing references)
		//IL_1979: Unknown result type (might be due to invalid IL or missing references)
		//IL_1983: Unknown result type (might be due to invalid IL or missing references)
		//IL_1992: Unknown result type (might be due to invalid IL or missing references)
		//IL_1999: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6c: Expected O, but got Unknown
		//IL_1b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e97: Expected O, but got Unknown
		//IL_1ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d41: Expected O, but got Unknown
		//IL_1d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e04: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1beb: Expected O, but got Unknown
		//IL_1bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c18: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c67: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fec: Expected O, but got Unknown
		//IL_1ff9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2008: Unknown result type (might be due to invalid IL or missing references)
		//IL_200f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2019: Unknown result type (might be due to invalid IL or missing references)
		//IL_2028: Unknown result type (might be due to invalid IL or missing references)
		//IL_202f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2039: Unknown result type (might be due to invalid IL or missing references)
		//IL_2048: Unknown result type (might be due to invalid IL or missing references)
		//IL_204f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2059: Unknown result type (might be due to invalid IL or missing references)
		//IL_2068: Unknown result type (might be due to invalid IL or missing references)
		//IL_206f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2079: Unknown result type (might be due to invalid IL or missing references)
		//IL_2088: Unknown result type (might be due to invalid IL or missing references)
		//IL_208f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2099: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20af: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f16: Expected O, but got Unknown
		//IL_1f23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c2: Expected O, but got Unknown
		//IL_20cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_20de: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2105: Unknown result type (might be due to invalid IL or missing references)
		//IL_210f: Unknown result type (might be due to invalid IL or missing references)
		//IL_211e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2125: Unknown result type (might be due to invalid IL or missing references)
		//IL_212f: Unknown result type (might be due to invalid IL or missing references)
		//IL_213e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2145: Unknown result type (might be due to invalid IL or missing references)
		//IL_214f: Unknown result type (might be due to invalid IL or missing references)
		//IL_215e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2165: Unknown result type (might be due to invalid IL or missing references)
		//IL_216f: Unknown result type (might be due to invalid IL or missing references)
		//IL_217e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2185: Unknown result type (might be due to invalid IL or missing references)
		//IL_218f: Unknown result type (might be due to invalid IL or missing references)
		//IL_219e: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21af: Unknown result type (might be due to invalid IL or missing references)
		//IL_21be: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_21de: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_21fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2205: Unknown result type (might be due to invalid IL or missing references)
		//IL_220f: Unknown result type (might be due to invalid IL or missing references)
		//IL_221e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2225: Unknown result type (might be due to invalid IL or missing references)
		//IL_222f: Unknown result type (might be due to invalid IL or missing references)
		//IL_223e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2245: Unknown result type (might be due to invalid IL or missing references)
		//IL_224f: Unknown result type (might be due to invalid IL or missing references)
		//IL_225e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2265: Unknown result type (might be due to invalid IL or missing references)
		//IL_226f: Unknown result type (might be due to invalid IL or missing references)
		//IL_227e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2285: Unknown result type (might be due to invalid IL or missing references)
		//IL_228f: Unknown result type (might be due to invalid IL or missing references)
		//IL_229e: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22af: Unknown result type (might be due to invalid IL or missing references)
		//IL_22be: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_22de: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_22fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2305: Unknown result type (might be due to invalid IL or missing references)
		//IL_230f: Unknown result type (might be due to invalid IL or missing references)
		//IL_231e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2325: Unknown result type (might be due to invalid IL or missing references)
		//IL_232f: Unknown result type (might be due to invalid IL or missing references)
		//IL_233e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2345: Unknown result type (might be due to invalid IL or missing references)
		//IL_23c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_242a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2434: Expected O, but got Unknown
		//IL_2434: Unknown result type (might be due to invalid IL or missing references)
		//IL_243e: Expected O, but got Unknown
		//IL_2444: Unknown result type (might be due to invalid IL or missing references)
		//IL_244b: Expected O, but got Unknown
		//IL_2458: Unknown result type (might be due to invalid IL or missing references)
		//IL_245f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2464: Unknown result type (might be due to invalid IL or missing references)
		//IL_2466: Unknown result type (might be due to invalid IL or missing references)
		//IL_247d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2484: Unknown result type (might be due to invalid IL or missing references)
		//IL_2489: Unknown result type (might be due to invalid IL or missing references)
		//IL_248b: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_24fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2511: Unknown result type (might be due to invalid IL or missing references)
		//IL_2518: Unknown result type (might be due to invalid IL or missing references)
		//IL_251d: Unknown result type (might be due to invalid IL or missing references)
		//IL_251f: Unknown result type (might be due to invalid IL or missing references)
		//IL_252b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2532: Expected O, but got Unknown
		//IL_2563: Unknown result type (might be due to invalid IL or missing references)
		//IL_256a: Expected O, but got Unknown
		//IL_2577: Unknown result type (might be due to invalid IL or missing references)
		//IL_257e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2583: Unknown result type (might be due to invalid IL or missing references)
		//IL_2585: Unknown result type (might be due to invalid IL or missing references)
		//IL_259c: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_25cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_260b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2612: Unknown result type (might be due to invalid IL or missing references)
		//IL_2617: Unknown result type (might be due to invalid IL or missing references)
		//IL_2619: Unknown result type (might be due to invalid IL or missing references)
		//IL_2630: Unknown result type (might be due to invalid IL or missing references)
		//IL_2637: Unknown result type (might be due to invalid IL or missing references)
		//IL_263c: Unknown result type (might be due to invalid IL or missing references)
		//IL_263e: Unknown result type (might be due to invalid IL or missing references)
		//IL_264a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2651: Expected O, but got Unknown
		//IL_2686: Unknown result type (might be due to invalid IL or missing references)
		//IL_268d: Expected O, but got Unknown
		//IL_269a: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2776: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_27cf: Expected O, but got Unknown
		//IL_27dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_27fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2801: Unknown result type (might be due to invalid IL or missing references)
		//IL_2806: Unknown result type (might be due to invalid IL or missing references)
		//IL_2808: Unknown result type (might be due to invalid IL or missing references)
		//IL_2847: Unknown result type (might be due to invalid IL or missing references)
		//IL_284e: Expected O, but got Unknown
		//IL_285b: Unknown result type (might be due to invalid IL or missing references)
		//IL_286a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2871: Unknown result type (might be due to invalid IL or missing references)
		//IL_287b: Unknown result type (might be due to invalid IL or missing references)
		//IL_288a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2891: Unknown result type (might be due to invalid IL or missing references)
		//IL_289b: Unknown result type (might be due to invalid IL or missing references)
		//IL_28aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28db: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_296e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a8a: Expected O, but got Unknown
		//IL_2a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2acd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b97: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be0: Expected O, but got Unknown
		//IL_2bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c63: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d56: Expected O, but got Unknown
		//IL_2d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e32: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e39: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e6c: Expected O, but got Unknown
		//IL_2e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ed9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f59: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f68: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe2: Expected O, but got Unknown
		//IL_2fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ffe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3005: Unknown result type (might be due to invalid IL or missing references)
		//IL_300f: Unknown result type (might be due to invalid IL or missing references)
		//IL_301e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3025: Unknown result type (might be due to invalid IL or missing references)
		//IL_302f: Unknown result type (might be due to invalid IL or missing references)
		//IL_303e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3045: Unknown result type (might be due to invalid IL or missing references)
		//IL_304f: Unknown result type (might be due to invalid IL or missing references)
		//IL_305e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3065: Unknown result type (might be due to invalid IL or missing references)
		//IL_306f: Unknown result type (might be due to invalid IL or missing references)
		//IL_307e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3085: Unknown result type (might be due to invalid IL or missing references)
		//IL_30fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_314f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3156: Expected O, but got Unknown
		//IL_3163: Unknown result type (might be due to invalid IL or missing references)
		//IL_3172: Unknown result type (might be due to invalid IL or missing references)
		//IL_3179: Unknown result type (might be due to invalid IL or missing references)
		//IL_3183: Unknown result type (might be due to invalid IL or missing references)
		//IL_3192: Unknown result type (might be due to invalid IL or missing references)
		//IL_3199: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_31e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3203: Unknown result type (might be due to invalid IL or missing references)
		//IL_3212: Unknown result type (might be due to invalid IL or missing references)
		//IL_3219: Unknown result type (might be due to invalid IL or missing references)
		//IL_3223: Unknown result type (might be due to invalid IL or missing references)
		//IL_3232: Unknown result type (might be due to invalid IL or missing references)
		//IL_3239: Unknown result type (might be due to invalid IL or missing references)
		//IL_3243: Unknown result type (might be due to invalid IL or missing references)
		//IL_3252: Unknown result type (might be due to invalid IL or missing references)
		//IL_3259: Unknown result type (might be due to invalid IL or missing references)
		//IL_3260: Unknown result type (might be due to invalid IL or missing references)
		//IL_3267: Expected O, but got Unknown
		//IL_3274: Unknown result type (might be due to invalid IL or missing references)
		//IL_3283: Unknown result type (might be due to invalid IL or missing references)
		//IL_328a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3294: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_32aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_32b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_3375: Unknown result type (might be due to invalid IL or missing references)
		//IL_33c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_33cf: Expected O, but got Unknown
		//IL_33dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_33ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_3401: Unknown result type (might be due to invalid IL or missing references)
		//IL_3408: Unknown result type (might be due to invalid IL or missing references)
		//IL_340d: Unknown result type (might be due to invalid IL or missing references)
		//IL_340f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3426: Unknown result type (might be due to invalid IL or missing references)
		//IL_342d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3432: Unknown result type (might be due to invalid IL or missing references)
		//IL_3434: Unknown result type (might be due to invalid IL or missing references)
		//IL_344b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3452: Unknown result type (might be due to invalid IL or missing references)
		//IL_3457: Unknown result type (might be due to invalid IL or missing references)
		//IL_3459: Unknown result type (might be due to invalid IL or missing references)
		//IL_3465: Unknown result type (might be due to invalid IL or missing references)
		//IL_346c: Expected O, but got Unknown
		//IL_349d: Unknown result type (might be due to invalid IL or missing references)
		//IL_34a4: Expected O, but got Unknown
		//IL_34b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_34d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_34dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_34fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3502: Unknown result type (might be due to invalid IL or missing references)
		//IL_3507: Unknown result type (might be due to invalid IL or missing references)
		//IL_3509: Unknown result type (might be due to invalid IL or missing references)
		//IL_3520: Unknown result type (might be due to invalid IL or missing references)
		//IL_3527: Unknown result type (might be due to invalid IL or missing references)
		//IL_352c: Unknown result type (might be due to invalid IL or missing references)
		//IL_352e: Unknown result type (might be due to invalid IL or missing references)
		//IL_353a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3541: Expected O, but got Unknown
		//IL_3572: Unknown result type (might be due to invalid IL or missing references)
		//IL_3579: Expected O, but got Unknown
		//IL_3586: Unknown result type (might be due to invalid IL or missing references)
		//IL_358d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3592: Unknown result type (might be due to invalid IL or missing references)
		//IL_3594: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_35d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_35d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_35dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_35de: Unknown result type (might be due to invalid IL or missing references)
		//IL_35f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_35fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3601: Unknown result type (might be due to invalid IL or missing references)
		//IL_3603: Unknown result type (might be due to invalid IL or missing references)
		//IL_360f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3616: Expected O, but got Unknown
		//IL_3647: Unknown result type (might be due to invalid IL or missing references)
		//IL_364e: Expected O, but got Unknown
		//IL_365b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3662: Unknown result type (might be due to invalid IL or missing references)
		//IL_3667: Unknown result type (might be due to invalid IL or missing references)
		//IL_3669: Unknown result type (might be due to invalid IL or missing references)
		//IL_3680: Unknown result type (might be due to invalid IL or missing references)
		//IL_3687: Unknown result type (might be due to invalid IL or missing references)
		//IL_368c: Unknown result type (might be due to invalid IL or missing references)
		//IL_368e: Unknown result type (might be due to invalid IL or missing references)
		//IL_36a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_36b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_36b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_36eb: Expected O, but got Unknown
		//IL_371b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3722: Expected O, but got Unknown
		//IL_372f: Unknown result type (might be due to invalid IL or missing references)
		//IL_373e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3745: Unknown result type (might be due to invalid IL or missing references)
		//IL_374f: Unknown result type (might be due to invalid IL or missing references)
		//IL_375e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3765: Unknown result type (might be due to invalid IL or missing references)
		//IL_376f: Unknown result type (might be due to invalid IL or missing references)
		//IL_377e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3785: Unknown result type (might be due to invalid IL or missing references)
		//IL_378f: Unknown result type (might be due to invalid IL or missing references)
		//IL_379e: Unknown result type (might be due to invalid IL or missing references)
		//IL_37a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3822: Unknown result type (might be due to invalid IL or missing references)
		//IL_38e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_39a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_39fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a01: Expected O, but got Unknown
		//IL_3a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a24: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_3abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_3add: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ae4: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		int num = -1;
		if (m_NextId >= 256)
		{
			m_NextId = 0;
		}
		for (int i = m_NextId; i < 256; i++)
		{
			if (m_Item[i].m_Id == -1)
			{
				num = i;
				flag = true;
				m_NextId = i + 1;
				break;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < 256; j++)
			{
				if (m_Item[j].m_Id == -1)
				{
					num = j;
					flag = true;
					m_NextId = j + 1;
					break;
				}
			}
		}
		if (num == -1)
		{
			return -1;
		}
		m_Item[num].m_Type = type;
		m_Item[num].m_Position = pos;
		m_Item[num].m_ZDistance = 0f;
		m_Item[num].m_Rotation = Vector3.Zero;
		m_Item[num].m_Rotation.Z = rot;
		m_Item[num].m_TriggerPos = pos;
		m_Item[num].m_TriggerId = triggerId;
		m_Item[num].m_Id = num;
		m_Item[num].m_fScale = 1f;
		m_Item[num].m_Accel = Vector2.Zero;
		m_Item[num].m_Active = true;
		m_Item[num].m_FrameCnt = 0;
		m_Item[num].m_bDying = false;
		m_Item[num].m_bDead = false;
		m_Item[num].m_bCastShadows = true;
		m_Item[num].m_Layer = 0;
		m_Item[num].m_Health = 0;
		m_Item[num].m_MaxHealth = 0;
		m_Item[num].m_bShowHealth = true;
		m_Item[num].m_Status = 0;
		m_Item[num].m_Bounds3DMax = Program.m_ItemManager.m_ModelSizeMax3D[type];
		m_Item[num].m_Bounds3DMin = Program.m_ItemManager.m_ModelSizeMin3D[type];
		m_Item[num].m_Velocity = Vector2.Zero;
		m_Item[num].m_bHasPlayerCollision = true;
		if (m_Model[type] == null)
		{
			Delete(num);
			return -1;
		}
		m_Item[num].SetModel(m_Model[type], m_ModelLOD[type]);
		Vector2 zero = Vector2.Zero;
		zero = m_Item[num].m_Position;
		Fixture val = null;
		switch (type)
		{
		case 0:
		case 4:
		case 5:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 1.524f, 1.524f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture2 = m_Item[num].m_Fixture;
			fixture2.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture2.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 1:
		case 19:
		case 37:
		{
			PolygonShape val16 = new PolygonShape();
			val16.SetAsEdge(TrigRotate(new Vector2(-3.95986f, 0f), rot) + zero, TrigRotate(new Vector2(-0.5461f, 1.95834f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val16, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val16.SetAsEdge(TrigRotate(new Vector2(-0.5461f, 1.95834f), rot) + zero, TrigRotate(new Vector2(-0.00762f, 1.905f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val16, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val16.SetAsEdge(TrigRotate(new Vector2(-0.00762f, 1.905f), rot) + zero, TrigRotate(new Vector2(0.60705996f, 1.9659599f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val16, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val16.SetAsEdge(TrigRotate(new Vector2(0.60705996f, 1.9659599f), rot) + zero, TrigRotate(new Vector2(3.97256f, 0f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val16, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 2:
		{
			Vertices val14 = new Vertices(6);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-5.17144f, 10.16f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(5.2527204f, 10.2108f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(5.2527204f, 11.341101f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(4.0106597f, 11.66114f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-3.9979599f, 11.65098f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-5.17144f, 11.417299f), rot) + zero);
			PolygonShape val15 = new PolygonShape(val14);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val15, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val14 = new Vertices(4);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-4.3789597f, 3.61696f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-4.3789597f, 4.37134f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-5.27304f, 4.37134f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-5.27304f, 3.61696f), rot) + zero);
			val15 = new PolygonShape(val14);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val15, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val14 = new Vertices(4);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(5.27304f, 3.61696f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(5.27304f, 4.37134f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(4.3789597f, 4.37134f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(4.3789597f, 3.61696f), rot) + zero);
			val15 = new PolygonShape(val14);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val15, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 3:
		{
			Vertices val19 = new Vertices(5);
			((List<Vector2>)(object)val19).Clear();
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(-2.16408f, 0.07874f), rot) + zero);
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(-1.8465799f, 0f), rot) + zero);
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(2.159f, 0f), rot) + zero);
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(2.159f, 0.15494f), rot) + zero);
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(-1.8465799f, 0.15494f), rot) + zero);
			PolygonShape val20 = new PolygonShape(val19);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val20, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 6:
		case 7:
		case 8:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 1.524f, 1.524f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 9:
		{
			PolygonShape val18 = new PolygonShape();
			val18.SetAsEdge(TrigRotate(new Vector2(-3.81f, 0.0508f), rot) + zero, TrigRotate(new Vector2(3.81f, 0.0508f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val18, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 10:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 5.1485796f, 2.54f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 20f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			break;
		case 11:
		{
			PolygonShape val17 = new PolygonShape();
			val17.SetAsEdge(TrigRotate(new Vector2(-3.81f, 0.127f), rot) + zero, TrigRotate(new Vector2(3.81f, 0.127f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val17, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 12:
			m_Item[num].m_bCastShadows = false;
			break;
		case 21:
			m_Item[num].m_UniqueId = (Program.m_App.m_Level - 1) * 3;
			if (m_FlagsCollected[m_Item[num].m_UniqueId] == 1)
			{
				Delete(num);
				return -1;
			}
			break;
		case 22:
			m_Item[num].m_UniqueId = (Program.m_App.m_Level - 1) * 3 + 1;
			if (m_FlagsCollected[m_Item[num].m_UniqueId] == 1)
			{
				Delete(num);
				return -1;
			}
			break;
		case 23:
			m_Item[num].m_UniqueId = (Program.m_App.m_Level - 1) * 3 + 2;
			if (m_FlagsCollected[m_Item[num].m_UniqueId] == 1)
			{
				Delete(num);
				return -1;
			}
			break;
		case 15:
		{
			PolygonShape val13 = new PolygonShape();
			val13.SetAsEdge(TrigRotate(new Vector2(-6.24078f, 0.00254f), rot) + zero, TrigRotate(new Vector2(2.64668f, 0.00254f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val13.SetAsEdge(TrigRotate(new Vector2(2.64668f, 0.00254f), rot) + zero, TrigRotate(new Vector2(2.6542997f, 0.37846f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val13.SetAsEdge(TrigRotate(new Vector2(2.6542997f, 0.37846f), rot) + zero, TrigRotate(new Vector2(5.29844f, 0.29463997f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val13.SetAsEdge(TrigRotate(new Vector2(5.29844f, 0.29463997f), rot) + zero, TrigRotate(new Vector2(6.23316f, 0.29463997f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val13.SetAsEdge(TrigRotate(new Vector2(6.23316f, 0.29463997f), rot) + zero, TrigRotate(new Vector2(6.23316f, -0.29463997f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 16:
		{
			PolygonShape val12 = new PolygonShape();
			val12.SetAsEdge(TrigRotate(new Vector2(-6.23316f, -0.2921f), rot) + zero, TrigRotate(new Vector2(-6.23316f, 0.28956f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val12.SetAsEdge(TrigRotate(new Vector2(-6.23316f, 0.28956f), rot) + zero, TrigRotate(new Vector2(-5.29844f, 0.29463997f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val12.SetAsEdge(TrigRotate(new Vector2(-5.29844f, 0.29463997f), rot) + zero, TrigRotate(new Vector2(-2.6542997f, 0.37846f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val12.SetAsEdge(TrigRotate(new Vector2(-2.6542997f, 0.37846f), rot) + zero, TrigRotate(new Vector2(-2.64668f, 0.00254f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val12.SetAsEdge(TrigRotate(new Vector2(-2.64668f, 0.00254f), rot) + zero, TrigRotate(new Vector2(6.24078f, 0.00254f), rot) + zero);
			Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			break;
		}
		case 17:
		{
			PolygonShape val11 = new PolygonShape();
			val11.SetAsEdge(TrigRotate(new Vector2(-3.81f, 0.127f), rot) + zero, TrigRotate(new Vector2(3.81f, 0.127f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val11, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 18:
		{
			PolygonShape val10 = new PolygonShape();
			val10.SetAsEdge(TrigRotate(new Vector2(-7.01802f, 5.2857404f), rot) + zero, TrigRotate(new Vector2(-8.86714f, 5.2857404f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-8.86714f, 5.2857404f), rot) + zero, TrigRotate(new Vector2(-8.851899f, 7.0485f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-8.851899f, 7.0485f), rot) + zero, TrigRotate(new Vector2(-9.687559f, 7.05866f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-8.851899f, 7.0485f), rot) + zero, TrigRotate(new Vector2(-9.796781f, 7.20852f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-9.796781f, 7.20852f), rot) + zero, TrigRotate(new Vector2(-9.796781f, 7.6073f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-9.796781f, 7.6073f), rot) + zero, TrigRotate(new Vector2(-9.65454f, 7.7343f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-9.65454f, 7.7343f), rot) + zero, TrigRotate(new Vector2(9.64438f, 7.7343f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(9.64438f, 7.7343f), rot) + zero, TrigRotate(new Vector2(9.78662f, 7.6479397f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(9.78662f, 7.6479397f), rot) + zero, TrigRotate(new Vector2(9.80694f, 7.2212195f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 20:
		{
			PolygonShape shape15 = new PolygonShape();
			AddEdge(shape15, new Vector2(-2.521f, 2.819f), new Vector2(-2.243f, 2.801f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-1.967f, 2.743f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-1.694f, 2.647f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-1.426f, 2.512f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-1.165f, 2.34f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-0.912f, 2.131f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-0.67f, 1.888f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-0.439f, 1.611f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-0.222f, 1.302f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-0.018f, 0.963f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(0.198f, 0.634f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(0.439f, 0.37f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(0.701f, 0.176f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(0.976f, 0.058f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(1.258f, 0.018f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(1.541f, 0.058f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(1.816f, 0.176f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(2.319f, 0.633f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(2.525f, 0.962f), rot, zero);
			break;
		}
		case 24:
		{
			PolygonShape shape14 = new PolygonShape();
			AddEdge(shape14, new Vector2(-2.525f, 0.962f), new Vector2(-2.319f, 0.633f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(-2.078f, 0.37f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(-1.816f, 0.176f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(-1.541f, 0.058f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(-1.258f, 0.018f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(-0.976f, 0.058f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(-0.701f, 0.176f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(-0.439f, 0.37f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(-0.198f, 0.634f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(0.018f, 0.963f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(0.222f, 1.302f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(0.439f, 1.611f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(0.67f, 1.888f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(0.912f, 2.131f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(1.165f, 2.34f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(1.426f, 2.512f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(1.694f, 2.647f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(1.967f, 2.743f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(2.243f, 2.801f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(2.521f, 2.819f), rot, zero);
			break;
		}
		case 25:
		{
			PolygonShape val9 = new PolygonShape();
			val9.SetAsEdge(TrigRotate(new Vector2(-12.7f, 0.0508f), rot) + zero, TrigRotate(new Vector2(12.7f, 0.0508f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val9, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 28:
		{
			PolygonShape shape13 = new PolygonShape();
			AddEdge(shape13, new Vector2(-5f, 0.01f), new Vector2(-4.01f, -0.016f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(-3.032f, -0.095f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(-2.075f, -0.224f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(-1.15f, -0.402f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(-0.267f, -0.627f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(0.564f, -0.898f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(1.332f, -1.21f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(2.032f, -1.561f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(2.655f, -1.947f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(3.192f, -2.363f), rot, zero);
			break;
		}
		case 27:
		{
			PolygonShape shape12 = new PolygonShape();
			AddEdge(shape12, new Vector2(-5f, 0.011f), new Vector2(-3.994f, 0.037f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(-2.998f, 0.117f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(-2.025f, 0.248f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(-1.085f, 0.429f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(-0.187f, 0.659f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(0.657f, 0.934f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(1.44f, 1.255f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(2.151f, 1.609f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(2.784f, 2.001f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(3.332f, 2.424f), rot, zero);
			break;
		}
		case 26:
		{
			PolygonShape val8 = new PolygonShape();
			val8.SetAsEdge(TrigRotate(new Vector2(-6.35f, 0.0508f), rot) + zero, TrigRotate(new Vector2(6.35f, 0.0508f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val8, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 30:
		{
			PolygonShape shape11 = new PolygonShape();
			AddEdge(shape11, new Vector2(-2.5f, 0.013f), new Vector2(-1.671f, -0.026f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(-0.848f, -0.134f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(-0.039f, -0.315f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(0.752f, -0.566f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(1.518f, -0.884f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(2.253f, -1.268f), rot, zero);
			break;
		}
		case 29:
		{
			PolygonShape shape10 = new PolygonShape();
			AddEdge(shape10, new Vector2(-2.5f, 0.01f), new Vector2(-1.666f, 0.048f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.835f, 0.157f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.018f, 0.34f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(0.778f, 0.593f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.552f, 0.913f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(2.293f, 1.3f), rot, zero);
			break;
		}
		case 31:
		{
			PolygonShape shape9 = new PolygonShape();
			AddEdge(shape9, new Vector2(-1.843f, 0.01f), new Vector2(-1.334f, 0.029f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(-0.835f, 0.083f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(-0.335f, 0.17f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(0.096f, 0.29f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(0.509f, 0.44f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(0.877f, 0.617f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.192f, 0.818f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.449f, 1.039f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.642f, 1.276f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.768f, 1.523f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.824f, 1.777f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.809f, 2.032f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.724f, 2.284f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.571f, 2.527f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.35f, 2.758f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.069f, 2.971f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(0.732f, 3.163f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(0.344f, 3.329f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(-0.085f, 3.467f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(-0.549f, 3.574f), rot, zero);
			break;
		}
		case 32:
		case 33:
		case 34:
		case 35:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 0.2032f, 1.524f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture = m_Item[num].m_Fixture;
			fixture.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 36:
		{
			Vertices val6 = new Vertices(6);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.89204f, 11.0109f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-5.1815996f, 10.7061f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.90728f, 10.39114f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.8894997f, 10.39114f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(5.17906f, 10.7061f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.89712f, 11.0109f), rot) + zero);
			PolygonShape val7 = new PolygonShape(val6);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val7, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val6 = new Vertices(6);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.73964f, 5.4737f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.8514f, 5.26288f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.73964f, 5.0317397f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.76758f, 5.0317397f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.88696f, 5.26288f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.76758f, 5.4737f), rot) + zero);
			val7 = new PolygonShape(val6);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val7, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 38:
		{
			PolygonShape val5 = new PolygonShape();
			val5.SetAsEdge(TrigRotate(new Vector2(-7.0358f, 0.2794f), rot) + zero, TrigRotate(new Vector2(7.0358f, 0.2794f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val5, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 39:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 12.7f, 7.62f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 40:
		{
			PolygonShape val4 = new PolygonShape();
			val4.SetAsEdge(TrigRotate(new Vector2(-3.683f, 0.0889f), rot) + zero, TrigRotate(new Vector2(3.683f, 0.0889f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val4, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 41:
		{
			PolygonShape shape8 = new PolygonShape();
			AddEdge(shape8, new Vector2(-1.7f, 0f), new Vector2(0.353f, 0.012f), rot, zero);
			AddEdge(shape8, Vector2.Zero, new Vector2(0.42f, 0.086f), rot, zero);
			AddEdge(shape8, Vector2.Zero, new Vector2(1.454f, 0.086f), rot, zero);
			AddEdge(shape8, Vector2.Zero, new Vector2(1.665f, 0.045f), rot, zero);
			AddEdge(shape8, Vector2.Zero, new Vector2(1.7f, 0f), rot, zero);
			break;
		}
		case 42:
		case 43:
		case 44:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 2.54f, 4.572f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 45:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 3.556f, 4.572f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 46:
		{
			PolygonShape shape7 = new PolygonShape();
			AddEdge(shape7, new Vector2(-0.754f, 0f), new Vector2(-0.555f, 0.432f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(-0.51f, 0.462f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(-0.427f, 0.483f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(-0.367f, 0.49f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(0f, 0.499f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(0.367f, 0.49f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(0.427f, 0.483f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(0.51f, 0.462f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(0.555f, 0.432f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(0.754f, 0f), rot, zero);
			break;
		}
		case 47:
		{
			PolygonShape shape6 = new PolygonShape();
			AddEdge(shape6, new Vector2(-1.35f, 0f), new Vector2(-1.35f, 1.432f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-1.778f, 1.484f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-1.778f, 1.573f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-1.35f, 1.625f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-1.35f, 1.801f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-1.319f, 1.871f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-0.93f, 1.899f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(0.939f, 1.899f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(1.32f, 1.87f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(1.354f, 1.803f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(1.354f, 0f), rot, zero);
			break;
		}
		case 48:
		{
			PolygonShape shape5 = new PolygonShape();
			AddEdge(shape5, new Vector2(-1.616f, 0.017f), new Vector2(-1.616f, 0.077f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-1.246f, 0.146f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-1.041f, 0.155f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-0.909f, 0.145f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-0.45f, 0.035f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-0.114f, 0.002f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(1.5f, 0f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(1.5f, -0.102f), rot, zero);
			break;
		}
		case 49:
		{
			PolygonShape shape4 = new PolygonShape();
			AddEdge(shape4, new Vector2(-0.378f, 0f), new Vector2(-0.325f, 0.345f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(-0.293f, 0.453f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(-0.275f, 0.48f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(-0.22f, 0.503f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(-0.123f, 0.514f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(0.123f, 0.514f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(0.22f, 0.503f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(0.275f, 0.48f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(0.293f, 0.453f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(0.325f, 0.345f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(0.378f, 0f), rot, zero);
			break;
		}
		case 50:
		{
			PolygonShape shape3 = new PolygonShape();
			AddEdge(shape3, new Vector2(-1.347f, 0f), new Vector2(-1.208f, 0.042f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(-1.118f, 0.108f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(1.118f, 0.108f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(1.208f, 0.042f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(1.347f, 0f), rot, zero);
			break;
		}
		case 51:
			m_Item[num].m_Fixture = FixtureFactory.CreateCircle(Program.m_App.m_World, 0.508f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 52:
		{
			PolygonShape shape2 = new PolygonShape();
			AddEdge(shape2, new Vector2(4.672f, 0.678f), new Vector2(0.697f, 0.405f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(-0.864f, 0.451f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(-2f, 0.587f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(-2.968f, 0.632f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(-3.826f, 0.577f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(-4.13f, 0.474f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(-6.874f, 0.13f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(-6.865f, 0f), rot, zero);
			shape2 = new PolygonShape();
			AddEdge(shape2, new Vector2(6.584f, 0.637f), new Vector2(6.639f, 0.815f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(6.776f, 0.829f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(6.861f, 0.66f), rot, zero);
			break;
		}
		case 53:
			m_Item[num].m_Fixture = FixtureFactory.CreateCircle(Program.m_App.m_World, 2.794f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 1f;
			m_Item[num].m_Fixture.Restitution = 0.75f;
			m_Item[num].m_Fixture.Body.Inertia = 0.1f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 54:
		{
			Vertices val2 = new Vertices(4);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-5.96392f, 5.9309f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-5.96392f, 4.1427402f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-4.73456f, 4.1427402f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-4.73456f, 5.9309f), rot) + zero);
			PolygonShape val3 = new PolygonShape(val2);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val3, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val2 = new Vertices(4);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(4.7802796f, 5.9334397f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(4.7802796f, 4.14528f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(6.0274196f, 4.14528f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(6.0274196f, 5.9334397f), rot) + zero);
			val3 = new PolygonShape(val2);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val3, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val2 = new Vertices(4);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-5.9842396f, 15.466061f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-5.9842396f, 14.25956f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-4.7371f, 14.25956f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-4.7371f, 15.466061f), rot) + zero);
			val3 = new PolygonShape(val2);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val3, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val2 = new Vertices(4);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(4.7752f, 15.49146f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(4.7752f, 14.29004f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(6.02488f, 14.29004f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(6.02488f, 15.49146f), rot) + zero);
			val3 = new PolygonShape(val2);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val3, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val3 = new PolygonShape();
			AddEdge(val3, new Vector2(-1.859f, 3.191f), new Vector2(-2.349f, 3.191f), rot, zero);
			AddEdge(val3, Vector2.Zero, new Vector2(-2.334f, 4.116f), rot, zero);
			AddEdge(val3, Vector2.Zero, new Vector2(2.342f, 4.116f), rot, zero);
			AddEdge(val3, Vector2.Zero, new Vector2(2.369f, 3.191f), rot, zero);
			break;
		}
		case 55:
		case 60:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 25.4f, 0.508f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 56:
		case 61:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 12.7f, 0.254f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 57:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 2.58318f, 1.5595601f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 59:
		{
			PolygonShape shape = new PolygonShape();
			AddEdge(shape, new Vector2(-0.917f, 0f), new Vector2(0.917f, 0f), rot, zero);
			AddEdge(shape, Vector2.Zero, new Vector2(0.915f, 0.301f), rot, zero);
			AddEdge(shape, Vector2.Zero, new Vector2(0.848f, 0.327f), rot, zero);
			AddEdge(shape, Vector2.Zero, new Vector2(0.629f, 1.747f), rot, zero);
			AddEdge(shape, Vector2.Zero, new Vector2(-0.629f, 1.747f), rot, zero);
			AddEdge(shape, Vector2.Zero, new Vector2(-0.848f, 0.327f), rot, zero);
			AddEdge(shape, Vector2.Zero, new Vector2(-0.915f, 0.301f), rot, zero);
			break;
		}
		}
		Vector3 zero2 = Vector3.Zero;
		zero2.X = m_Item[num].m_Position.X;
		zero2.Y = m_Item[num].m_Position.Y;
		zero2.Z = m_Item[num].m_ZDistance;
		m_Item[num].m_ScreenPos = Program.m_CameraManager3D.WorldToScreen(zero2);
		if (flag)
		{
			return num;
		}
		return -1;
	}

	private void AddEdge(PolygonShape shape, Vector2 prev, Vector2 next, float rot, Vector2 off)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		if (prev == Vector2.Zero)
		{
			shape.SetAsEdge(TrigRotate(m_LastEdge * 2.54f, rot) + off, TrigRotate(next * 2.54f, rot) + off);
		}
		else
		{
			shape.SetAsEdge(TrigRotate(prev * 2.54f, rot) + off, TrigRotate(next * 2.54f, rot) + off);
		}
		Fixture val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)shape, 0f);
		val.CollisionCategories = (CollisionCategory)1073741824;
		val.CollidesWith = (CollisionCategory)1073741824;
		m_LastEdge = next;
	}

	private Vector2 TrigRotate(Vector2 v1, float rot)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.Zero;
		Matrix val = Matrix.CreateRotationZ(rot);
		return Vector2.Transform(v1, val);
	}

	protected virtual bool ItemOnCollision(Fixture FixtureB, Fixture FixtureA, Contact contact)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
		if (m_ItemImpactSoundTime < num)
		{
			Vector2 linearVelocity = FixtureB.Body.LinearVelocity;
			if (!(((Vector2)(ref linearVelocity)).LengthSquared() > 10f))
			{
				Vector2 linearVelocity2 = FixtureA.Body.LinearVelocity;
				if (!(((Vector2)(ref linearVelocity2)).LengthSquared() > 10f))
				{
					goto IL_0070;
				}
			}
			Program.m_SoundManager.Play(25);
			m_ItemImpactSoundTime = num + 1000f;
		}
		goto IL_0070;
		IL_0070:
		return true;
	}

	public void DeleteAll()
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1)
			{
				m_Item[i].Delete();
			}
		}
	}

	public void Delete(int id)
	{
		m_Item[id].Delete();
	}

	public void DeleteByGraphId(int graphId)
	{
	}

	public void DeleteByTriggerId(int triggerId)
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_TriggerId == triggerId)
			{
				m_Item[i].Delete();
			}
		}
	}

	public void Update()
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1)
			{
				m_Item[i].Update();
			}
		}
	}

	public void Render()
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1)
			{
				m_Item[i].Render();
			}
		}
	}

	public void CalcModelSize(Model model, int type, bool bUseAll)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		ref Vector3 reference = ref m_ModelSizeMax3D[type];
		reference = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		ref Vector3 reference2 = ref m_ModelSizeMin3D[type];
		reference2 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Matrix[] array = (Matrix[])(object)new Matrix[((ReadOnlyCollection<ModelBone>)(object)model.Bones).Count];
		model.CopyAbsoluteBoneTransformsTo(array);
		Enumerator enumerator = model.Meshes.GetEnumerator();
		try
		{
			Vector3 val = default(Vector3);
			while (((Enumerator)(ref enumerator)).MoveNext())
			{
				ModelMesh current = ((Enumerator)(ref enumerator)).Current;
				Enumerator enumerator2 = current.MeshParts.GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator2)).MoveNext())
					{
						ModelMeshPart current2 = ((Enumerator)(ref enumerator2)).Current;
						int vertexStride = current2.VertexStride;
						int numVertices = current2.NumVertices;
						byte[] array2 = new byte[vertexStride * numVertices];
						current.VertexBuffer.GetData<byte>(array2);
						for (int i = 0; i < array2.Length; i += vertexStride)
						{
							float num = BitConverter.ToSingle(array2, i);
							float num2 = BitConverter.ToSingle(array2, i + 4);
							float num3 = BitConverter.ToSingle(array2, i + 8);
							((Vector3)(ref val))._002Ector(num, num2, num3);
							Vector3 val2 = Vector3.Transform(val, array[current.ParentBone.Index]);
							if (val2.X < m_ModelSizeMin3D[type].X)
							{
								m_ModelSizeMin3D[type].X = val2.X;
							}
							if (val2.X > m_ModelSizeMax3D[type].X)
							{
								m_ModelSizeMax3D[type].X = val2.X;
							}
							if (val2.Y < m_ModelSizeMin3D[type].Y)
							{
								m_ModelSizeMin3D[type].Y = val2.Y;
							}
							if (val2.Y > m_ModelSizeMax3D[type].Y)
							{
								m_ModelSizeMax3D[type].Y = val2.Y;
							}
							if (val2.Z < m_ModelSizeMin3D[type].Z)
							{
								m_ModelSizeMin3D[type].Z = val2.Z;
							}
							if (val2.Z > m_ModelSizeMax3D[type].Z)
							{
								m_ModelSizeMax3D[type].Z = val2.Z;
							}
						}
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
		Vector3[] array3 = (Vector3[])(object)new Vector3[8];
		Vector3 zero = Vector3.Zero;
		zero.Y = 5f;
		zero.Z = 0f;
		Matrix val3 = Matrix.CreateFromYawPitchRoll((float)Math.PI / 2f, 0f, 0f);
		Vector3 val4 = Vector3.Transform(m_ModelSizeMin3D[type], val3);
		Vector3 val5 = Vector3.Transform(m_ModelSizeMax3D[type], val3);
		int num4 = 4;
		ref Vector3 reference3 = ref array3[0];
		reference3 = new Vector3(val4.X, val4.Y, val4.Z);
		ref Vector3 reference4 = ref array3[1];
		reference4 = new Vector3(val4.X, val5.Y, val4.Z);
		ref Vector3 reference5 = ref array3[2];
		reference5 = new Vector3(val5.X, val4.Y, val4.Z);
		ref Vector3 reference6 = ref array3[3];
		reference6 = new Vector3(val5.X, val5.Y, val4.Z);
		if (bUseAll)
		{
			num4 = 8;
			ref Vector3 reference7 = ref array3[4];
			reference7 = new Vector3(val4.X, val4.Y, val5.Z);
			ref Vector3 reference8 = ref array3[5];
			reference8 = new Vector3(val4.X, val5.Y, val5.Z);
			ref Vector3 reference9 = ref array3[6];
			reference9 = new Vector3(val5.X, val4.Y, val5.Z);
			ref Vector3 reference10 = ref array3[7];
			reference10 = new Vector3(val5.X, val5.Y, val5.Z);
		}
		Vector2 val6 = default(Vector2);
		((Vector2)(ref val6))._002Ector(9999f, 9999f);
		Vector2 val7 = default(Vector2);
		((Vector2)(ref val7))._002Ector(-9999f, -9999f);
		for (int j = 0; j < num4; j++)
		{
			Vector3 val8 = Program.m_CameraManager3D.WorldToScreen(array3[j] + zero);
			if (val8.X < val6.X)
			{
				val6.X = val8.X;
			}
			if (val8.X > val7.X)
			{
				val7.X = val8.X;
			}
			if (val8.Y < val6.Y)
			{
				val6.Y = val8.Y;
			}
			if (val8.Y > val7.Y)
			{
				val7.Y = val8.Y;
			}
		}
		m_ModelSize2D[type].X = val7.X - val6.X;
		m_ModelSize2D[type].Y = val7.Y - val6.Y;
	}

	public Item FindObjectByType(int type)
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1 && m_Item[i].m_Type == type)
			{
				return m_Item[i];
			}
		}
		return null;
	}

	public int DamageAll(Vector2 pos, float radiusSq, int damage, int ownerId)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1 && !m_Item[i].m_bDying && !m_Item[i].m_bDead)
			{
				Vector2 val = m_Item[i].m_Position - pos;
				float num = ((Vector2)(ref val)).LengthSquared();
				if (num < radiusSq)
				{
					m_Item[i].TakeDamage(damage, ownerId);
					m_Item[i].m_Velocity.X = (float)Program.m_App.m_Rand.NextDouble() - 0.5f;
					m_Item[i].m_Velocity.Y = 0.25f;
					m_Item[i].m_Rotation.X = (float)Program.m_App.m_Rand.NextDouble() - 0.5f;
					m_Item[i].m_Rotation.Y = (float)Program.m_App.m_Rand.NextDouble() - 0.5f;
					m_Item[i].m_Rotation.Z = (float)Program.m_App.m_Rand.NextDouble() - 0.5f;
				}
			}
		}
		return -1;
	}

	public void ClearAllFlashes()
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1)
			{
				m_Item[i].m_FlashModel = 0;
			}
		}
	}

	public void Copy(Item[] src, Item[] dest)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 256; i++)
		{
			dest[i].m_Id = src[i].m_Id;
			dest[i].m_Position = src[i].m_Position;
			dest[i].m_Rotation = src[i].m_Rotation;
			dest[i].m_Active = src[i].m_Active;
			dest[i].m_fScale = src[i].m_fScale;
			dest[i].m_PrevPosition = src[i].m_PrevPosition;
			dest[i].m_PrevRotation = src[i].m_PrevRotation;
			dest[i].m_TriggerId = src[i].m_TriggerId;
			dest[i].m_TriggerPos = src[i].m_TriggerPos;
			dest[i].m_Type = src[i].m_Type;
			dest[i].m_Velocity = src[i].m_Velocity;
		}
	}

	public void UpdateAllFromTriggers()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1 && m_Item[i].m_TriggerId != -1 && (m_Item[i].m_Position != Program.m_TriggerManager.m_Trigger[m_Item[i].m_TriggerId].m_Position || m_Item[i].m_Rotation.Z != Program.m_TriggerManager.m_Trigger[m_Item[i].m_TriggerId].m_Rotation))
			{
				m_Item[i].m_Position = Program.m_TriggerManager.m_Trigger[m_Item[i].m_TriggerId].m_Position;
				m_Item[i].m_Rotation.Z = Program.m_TriggerManager.m_Trigger[m_Item[i].m_TriggerId].m_Rotation;
				if (m_Item[i].m_Fixture != null)
				{
					m_Item[i].m_Fixture.Body.Position = m_Item[i].m_Position;
					m_Item[i].m_Fixture.Body.Rotation = m_Item[i].m_Rotation.Z;
				}
			}
		}
	}

	public void LoadContent(ContentManager Content)
	{
		m_Model[1] = Content.Load<Model>("Models\\book_large_25deg");
		m_ModelLOD[1] = Content.Load<Model>("Models\\book_large_25deg");
		m_Model[2] = Content.Load<Model>("Models\\chair1");
		m_ModelLOD[2] = Content.Load<Model>("Models\\chair1");
		m_Model[3] = Content.Load<Model>("Models\\pencil");
		m_ModelLOD[3] = Content.Load<Model>("Models\\pencil");
		m_Model[0] = Content.Load<Model>("Models\\toyblock_a");
		m_ModelLOD[0] = Content.Load<Model>("Models\\toyblock_a");
		m_Model[4] = Content.Load<Model>("Models\\toyblock_b");
		m_ModelLOD[4] = Content.Load<Model>("Models\\toyblock_b");
		m_Model[5] = Content.Load<Model>("Models\\toyblock_c");
		m_ModelLOD[5] = Content.Load<Model>("Models\\toyblock_c");
		m_Model[6] = m_Model[0];
		m_ModelLOD[6] = m_ModelLOD[0];
		m_Model[7] = m_Model[4];
		m_ModelLOD[7] = m_ModelLOD[4];
		m_Model[8] = m_Model[5];
		m_ModelLOD[8] = m_ModelLOD[5];
		m_Model[9] = Content.Load<Model>("Models\\ruler");
		m_ModelLOD[9] = Content.Load<Model>("Models\\ruler");
		m_Model[10] = Content.Load<Model>("Models\\bus");
		m_ModelLOD[10] = Content.Load<Model>("Models\\bus");
		m_Model[11] = Content.Load<Model>("Models\\chessboard");
		m_ModelLOD[11] = Content.Load<Model>("Models\\chessboard");
		m_Model[12] = Content.Load<Model>("Models\\drawingpin");
		m_ModelLOD[12] = Content.Load<Model>("Models\\drawingpin");
		m_Model[13] = Content.Load<Model>("Models\\startfinish");
		m_ModelLOD[13] = Content.Load<Model>("Models\\startfinish");
		m_Model[14] = m_Model[13];
		m_ModelLOD[14] = m_Model[13];
		m_Model[21] = Content.Load<Model>("Models\\flag");
		m_ModelLOD[21] = Content.Load<Model>("Models\\flag");
		m_Model[22] = m_Model[21];
		m_ModelLOD[22] = m_ModelLOD[21];
		m_Model[23] = m_Model[21];
		m_ModelLOD[23] = m_ModelLOD[21];
		m_Model[15] = Content.Load<Model>("Models\\sword");
		m_ModelLOD[15] = Content.Load<Model>("Models\\sword");
		m_Model[16] = Content.Load<Model>("Models\\sword2");
		m_ModelLOD[16] = Content.Load<Model>("Models\\sword2");
		m_Model[17] = Content.Load<Model>("Models\\shield");
		m_ModelLOD[17] = Content.Load<Model>("Models\\shield");
		m_Model[18] = Content.Load<Model>("Models\\smalltable");
		m_ModelLOD[18] = Content.Load<Model>("Models\\smalltable");
		m_Model[19] = Content.Load<Model>("Models\\book_large2");
		m_ModelLOD[19] = Content.Load<Model>("Models\\book_large2");
		m_Model[20] = Content.Load<Model>("Models\\track_skislope");
		m_ModelLOD[20] = Content.Load<Model>("Models\\track_skislope");
		m_Model[24] = Content.Load<Model>("Models\\track_skislope_reverse");
		m_ModelLOD[24] = Content.Load<Model>("Models\\track_skislope_reverse");
		m_Model[25] = Content.Load<Model>("Models\\track_1m");
		m_ModelLOD[25] = m_Model[25];
		m_Model[26] = Content.Load<Model>("Models\\track_50cm");
		m_ModelLOD[26] = m_Model[26];
		m_Model[27] = Content.Load<Model>("Models\\track_1m_up60");
		m_ModelLOD[27] = m_Model[27];
		m_Model[28] = Content.Load<Model>("Models\\track_1m_down60");
		m_ModelLOD[28] = m_Model[28];
		m_Model[29] = Content.Load<Model>("Models\\track_50cm_up30");
		m_ModelLOD[29] = m_Model[29];
		m_Model[30] = Content.Load<Model>("Models\\track_50cm_down30");
		m_ModelLOD[30] = m_Model[30];
		m_Model[31] = Content.Load<Model>("Models\\track_loop");
		m_ModelLOD[31] = m_Model[31];
		m_Model[32] = Content.Load<Model>("Models\\domino_one");
		m_ModelLOD[32] = m_Model[32];
		m_Model[33] = Content.Load<Model>("Models\\domino_two");
		m_ModelLOD[33] = m_Model[33];
		m_Model[34] = Content.Load<Model>("Models\\domino_three");
		m_ModelLOD[34] = m_Model[34];
		m_Model[35] = Content.Load<Model>("Models\\domino_four");
		m_ModelLOD[35] = m_Model[35];
		m_Model[36] = Content.Load<Model>("Models\\chair2");
		m_ModelLOD[36] = m_Model[36];
		m_Model[37] = Content.Load<Model>("Models\\book_large3");
		m_ModelLOD[37] = m_Model[37];
		m_Model[38] = Content.Load<Model>("Models\\hob");
		m_ModelLOD[38] = m_Model[38];
		m_Model[39] = Content.Load<Model>("Models\\microwave");
		m_ModelLOD[39] = m_Model[39];
		m_Model[40] = Content.Load<Model>("Models\\placemat");
		m_ModelLOD[40] = m_Model[40];
		m_Model[41] = Content.Load<Model>("Models\\knife");
		m_ModelLOD[41] = m_Model[41];
		m_Model[42] = Content.Load<Model>("Models\\coffee");
		m_ModelLOD[42] = m_Model[42];
		m_Model[43] = Content.Load<Model>("Models\\tea");
		m_ModelLOD[43] = m_Model[43];
		m_Model[44] = Content.Load<Model>("Models\\sugar");
		m_ModelLOD[44] = m_Model[44];
		m_Model[45] = Content.Load<Model>("Models\\biscuits");
		m_ModelLOD[45] = m_Model[45];
		m_Model[46] = Content.Load<Model>("Models\\bowl");
		m_ModelLOD[46] = m_Model[46];
		m_Model[47] = Content.Load<Model>("Models\\toaster");
		m_ModelLOD[47] = m_Model[47];
		m_Model[48] = Content.Load<Model>("Models\\spoon");
		m_ModelLOD[48] = m_Model[48];
		m_Model[49] = Content.Load<Model>("Models\\mug");
		m_ModelLOD[49] = m_Model[49];
		m_Model[50] = Content.Load<Model>("Models\\plate");
		m_ModelLOD[50] = m_Model[50];
		m_Model[51] = Content.Load<Model>("Models\\rollingpin");
		m_ModelLOD[51] = m_Model[51];
		m_Model[52] = Content.Load<Model>("Models\\spade");
		m_ModelLOD[52] = m_Model[52];
		m_Model[53] = Content.Load<Model>("Models\\ball");
		m_ModelLOD[53] = m_Model[53];
		m_Model[54] = Content.Load<Model>("Models\\gardenchair");
		m_ModelLOD[54] = m_Model[54];
		m_Model[55] = Content.Load<Model>("Models\\plank_1m");
		m_ModelLOD[55] = m_Model[55];
		m_Model[56] = Content.Load<Model>("Models\\plank_50cm");
		m_ModelLOD[56] = m_Model[56];
		m_Model[57] = Content.Load<Model>("Models\\brick");
		m_ModelLOD[57] = m_Model[57];
		m_Model[58] = Content.Load<Model>("Models\\rake");
		m_ModelLOD[58] = m_Model[58];
		m_Model[59] = Content.Load<Model>("Models\\plantpot");
		m_ModelLOD[59] = m_Model[59];
		m_Model[60] = Content.Load<Model>("Models\\plank2_1m");
		m_ModelLOD[60] = m_Model[60];
		m_Model[61] = Content.Load<Model>("Models\\plank2_50cm");
		m_ModelLOD[61] = m_Model[61];
		m_Model[62] = Content.Load<Model>("Models\\checkpoint");
		m_ModelLOD[62] = m_Model[62];
	}
}
