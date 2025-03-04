namespace AppNamespace;

public class GroupManager
{
	private const int MAX_GROUPS = 32;

	private Group[] m_Group;

	private int m_NextGroup;

	public GroupManager()
	{
		m_Group = new Group[32];
	}

	public int FindFree()
	{
		if (m_NextGroup == 32)
		{
			m_NextGroup = 0;
		}
		for (int i = m_NextGroup; i < 31; i++)
		{
			if (m_Group[i].m_RefCount == 0)
			{
				m_NextGroup = i + 1;
				m_Group[i].m_AllDestroyed = true;
				return i;
			}
		}
		for (int j = 0; j < 32; j++)
		{
			if (m_Group[j].m_RefCount == 0)
			{
				m_NextGroup = j + 1;
				m_Group[j].m_AllDestroyed = true;
				return j;
			}
		}
		return -1;
	}

	public void Add(int id)
	{
		m_Group[id].m_RefCount++;
	}

	public void Remove(int id)
	{
		if (m_Group[id].m_RefCount > 0)
		{
			m_Group[id].m_RefCount--;
		}
	}

	public int RefCount(int id)
	{
		return m_Group[id].m_RefCount;
	}

	public void SetAllDestroyed(int id, bool destroyed)
	{
		m_Group[id].m_AllDestroyed = destroyed;
	}

	public bool AllDestroyed(int id)
	{
		return m_Group[id].m_AllDestroyed;
	}
}
