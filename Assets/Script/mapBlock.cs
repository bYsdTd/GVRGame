using UnityEngine;
using System.Collections;

public class mapBlock 
{
	private GameObject instance = null;

	public void Init(GameObject prefab)
	{
		instance = GameObject.Instantiate(prefab);
	}
}
