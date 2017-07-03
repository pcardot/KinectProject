using UnityEngine;
using System.Collections;

public class ObjectMovingScript : MonoBehaviour {

	public float speed = 1.0f;
	public ObjectFactory.positions pos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate(new Vector3(0,0, speed * Time.deltaTime * -1.0f));
	}
}
