using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mapBlockManager  
{
	private mapBlock m_currentBlock = null;
	private mapBlock m_nextBlock = null;

	private List<mapBlock> m_blockPool = new List<mapBlock>();

	private GameObject m_rootGameObj = null;

	public mapBlockManager()
	{
		//init root
		InitRoot();

		InitAllMapBlock();

		InitStartPoint();
	}

	public void InitRoot()
	{
		m_rootGameObj = new GameObject("rootGameObj");
	}

	public void InitAllMapBlock()
	{
		string[] prefabNames = new string[]{ "Prefab/block1", "Prefab/block2", "Prefab/block3"};

		for(int i = 0; i < prefabNames.Length; ++i)
		{
			GameObject prefabObj = Resources.Load(prefabNames[i]) as GameObject;

			GameObject blockInstance = GameObject.Instantiate(prefabObj);

			mapBlock mapBlockInstance = new mapBlock(blockInstance);
			mapBlockInstance.Deactive();

			mapBlockInstance.instance.transform.SetParent(m_rootGameObj.transform);

			m_blockPool.Add(mapBlockInstance);
		}
	}

	public void InitStartPoint()
	{
		// 初始的时候先随机两个
		System.Random rand = new System.Random();

		int index1 = rand.Next(0, m_blockPool.Count-1);
		m_currentBlock = m_blockPool[index1];
		m_blockPool.RemoveAt(index1);
		m_currentBlock.Active();

		int type = 0;
		m_currentBlock.type = type;
		m_currentBlock.startPoint = Vector3.zero;


		int index2 = rand.Next(0, m_blockPool.Count-1);
		m_nextBlock = m_blockPool[index2];
		m_blockPool.RemoveAt(index2);
		m_nextBlock.Active();

		type = 2;
		m_nextBlock.type = type;

		m_nextBlock.Attach(m_currentBlock);
	}
}

