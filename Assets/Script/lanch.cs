using UnityEngine;
using System.Collections;

public class lanch : MonoBehaviour {

	private mapBlockManager manager = null;
	// Use this for initialization
	void Start () 
	{
		manager = new mapBlockManager();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float dt = Time.deltaTime;
		if(manager != null)
		{
			manager.Update(dt);	
		}
	}
}
