using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mapBlockManager  
{
	private static mapBlockManager instance = null;
	private static List<mapBlock> m_mapBlockList = new List<mapBlock>();

	private static string[] m_prefabNames = new string[] {"mapBlockStraight", "mapBlockLeft", "mapBlockRight"};

	private static List<GameObject> m_prefabList = new List<GameObject>();

	public static mapBlockManager Instance()
	{
		if( instance == null )
		{
			instance = new mapBlockManager();
		}

		return instance;
	}

	public static void InitAllMapBlock()
	{
		for(int i = 0; i < m_prefabNames.Length; ++i)
		{
			GameObject prefab = Resources.Load(m_prefabNames[i]) as GameObject;
			m_prefabList.Add(prefab);
		}

	}

	public static void generateNextBlock()
	{
		var random = new System.Random();

		int count = 5;
		while(count > 0)
		{
			int randNum = random.Next(0, 2);

			mapBlock block = new mapBlock();
			block.Init(m_prefabList[randNum]);

			m_mapBlockList.Add(block);

			--count;
		}
	}
}

