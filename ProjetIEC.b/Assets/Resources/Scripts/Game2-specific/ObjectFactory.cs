using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFactory : MonoBehaviour {

	[SerializeField]
	private GameObject objectPrefab;
	private Stack<GameObject> unusedObjects;
	
	public static ObjectFactory instance = null;
	public enum positions{
		left,
		right,
		up
	}

	/**
	 * Unity Generic Event delegates
	 **/
	// Use this for initialization
	void Start () {
		unusedObjects = new Stack<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake () {
		if (instance != null)
			Debug.Log("ObjectFactory is supposed to be a singleton");
		else
			instance = this;
	}
	
	/**
	 * Factory public functions
	 **/
	public GameObject getObject(positions pos)
	{
		GameObject retour;
		if(unusedObjects.Count > 0)
		{
			retour = unusedObjects.Pop();
			retour.gameObject.SetActive(true);
		}
		else
		{
			retour = (GameObject) Instantiate(objectPrefab);
		}
		switch(pos)
		{
			case positions.left:
				retour.transform.position = new Vector3(-2.0f,0,10.0f);
				retour.renderer.material.color = Color.white;
				break;
			
			case positions.right:
				retour.transform.position = new Vector3(2.0f,0,10.0f);
				retour.renderer.material.color = Color.yellow;
				break;
			
			case positions.up:
				retour.transform.position = new Vector3(0,2.0f,10.0f);
				retour.renderer.material.color = Color.red;
				break;
			
			default:
				break;
		}
		ObjectMovingScript OMS = (ObjectMovingScript) retour.GetComponent("ObjectMovingScript");
		OMS.pos = pos;
		return retour;
	}
	
	public void releaseObject(GameObject obj)
	{
		obj.gameObject.SetActive(false);
		unusedObjects.Push(obj);
	}
}
