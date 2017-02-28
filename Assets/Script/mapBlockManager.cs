using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mapBlockManager  
{
	private mapBlock m_currentBlock = null;
	private mapBlock m_nextBlock = null;

	private List<mapBlock> m_blockPool = new List<mapBlock>();

	private GameObject m_rootGameObj = null;

	private bool m_switchingMapBlock { set; get;}

	public mapBlockManager()
	{
		m_switchingMapBlock = false;

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

		m_currentBlock = generateBlock();
		m_currentBlock.InitStartBlock();

		m_nextBlock = generateBlock();
		m_nextBlock.Attach(m_currentBlock);


	}

	public void Update(float dt)
	{
		if(m_switchingMapBlock)
		{
			// 正在切换, 不做任何事
		}
		else
		{
			// 更新当前块和下个块，判断是否该转向
			float moveSpeed = 20;
			float moveDistance = dt * moveSpeed;

			float realMoveDistance = moveDistance;
			bool shouldChange = m_currentBlock.UpdateMoveCurrent(moveDistance, out realMoveDistance);

			m_nextBlock.UpdateMoveOther(realMoveDistance);

			if(shouldChange)
			{
				m_switchingMapBlock = true;

				// 切换地图的逻辑
				handleSwitchMapBlock();
			}
		}
	}

	private mapBlock generateBlock()
	{
		System.Random rand = new System.Random();
		int index1 = rand.Next(0, m_blockPool.Count-1);
		mapBlock block = m_blockPool[index1];
		m_blockPool.RemoveAt(index1);
		block.Active();

		int type = rand.Next(0, 2);
		block.type = type;

		return block;
	}

	private void recycleBlock(mapBlock block)
	{
		m_blockPool.Add(block);
		block.Deactive();
	}

	private void handleSwitchMapBlock()
	{
		recycleBlock(m_currentBlock);

		if(m_nextBlock.type != 0)
		{
			float turnTime = 1.0f;
			float turnSpeed = 90/turnTime;
			float turnAngle = -90;

			if(m_nextBlock.type == 2)
			{
				turnSpeed = - turnSpeed;
				turnAngle = 90;
			}

			TimerManager.Instance().RepeatCallFunc(delegate(float dt) {

				turnAngle += turnSpeed * dt;

				m_nextBlock.instance.transform.rotation = Quaternion.AngleAxis(turnAngle, Vector3.up);

				}, 0, turnTime, null, delegate() {

				// 旋转结束
				doSwitch();

				m_switchingMapBlock = false;
			});

		}
		else
		{
			doSwitch();

			m_switchingMapBlock = false;
		}


	}

	private void doSwitch()
	{
		m_currentBlock = m_nextBlock;
		m_currentBlock.InitStartBlock();

		// 生成一个新的
		m_nextBlock = generateBlock();
		m_nextBlock.Attach(m_currentBlock);
	}
}

