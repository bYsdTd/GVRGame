using UnityEngine;
using System.Collections;

public class mapBlock 
{
	public GameObject instance { get; private set;}

	public Vector3 startPoint 
	{ 
		get
		{
			return _startPoint;
		}

		set
		{
			//if(_startPoint != value)
			{
				_startPoint = value;

				calculateEndPoint();
			}
		}
	}

	private Vector3 _startPoint;

	public Vector3 endPoint {get; set;}

	public float length {get; set;}

	// 0 前，1 左， 2 右
	public int type 
	{ 
		get
		{
			return _type;
		}

		set
		{
			_type = value;

			if(_type == 0)
			{
				instance.transform.rotation = Quaternion.identity;
			}
			else if(_type == 1)
			{
				instance.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
			}
			else if(_type == 2)
			{
				instance.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
			}
				
		}
	}

	private int _type;

	public mapBlock(GameObject ins)
	{
		instance = ins;

		length = 100;
	}

	public void Active()
	{
		instance.transform.position = Vector3.zero;
	}

	public void Deactive()
	{
		instance.transform.position = new Vector3(999999, 999999, 999999);
	}

	public void Attach(mapBlock other)
	{
		startPoint = other.endPoint;

		instance.transform.position = startPoint;

		// 同时计算出endpoint

	}

	private void calculateEndPoint()
	{
		Vector3 tmp = startPoint;
		if(type == 0)
		{
			tmp.z += length;
		}
		else if(type == 1)
		{
			tmp.x -= length;
		}
		else if(type == 2)
		{
			tmp.x += length;
		}

		endPoint = tmp;
	}

	private float _moveDistance;

	public void InitStartBlock()
	{
		// 当前开始移动了，type 设置成0
		type = 0;
		startPoint = Vector3.zero;
		_moveDistance = 0.0f;
	}

	public void UpdateMoveOther(float moveDistance)
	{
		Vector3 tmp = startPoint;
		tmp.z -= moveDistance;
		startPoint = tmp;

		instance.transform.position = startPoint;

	}

	public bool UpdateMoveCurrent(float moveDistance, out float realMoveDistance)
	{
		// 当前的地图块是需要判断是否移动到终点的，其他的是跟随移动
		bool changeNextBlock = false;
		_moveDistance += moveDistance;
		if(_moveDistance >= length)
		{
			moveDistance = _moveDistance - length;
			changeNextBlock = true;
		}

		Vector3 tmp = startPoint;
		tmp.z -= moveDistance;
		startPoint = tmp;

		instance.transform.position = startPoint;

		realMoveDistance = moveDistance;
		return changeNextBlock;
	}
}
