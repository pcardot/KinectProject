using UnityEngine;
using System.Collections;

public class ControlsManager : MonoBehaviour {

	private Vector3 oldKinectPos;
	private Vector3 newKinectPos;
	[SerializeField]
	private float smoothing = 0.0f;
	[SerializeField]
	private float scaling = 1.0f;

	// Use this for initialization
	void Start () {
		newKinectPos = KinectSkeleton.skeleton[0]["Head"];
	}
	
	// Update is called once per frame
	void Update () {
		oldKinectPos = newKinectPos;
		newKinectPos = KinectSkeleton.skeleton[0]["Head"];
		
		/* Options to quit : 
		   - If arms are crossed (right hand to the left of left hand) 
		   - If the button Escape is pressed */
		if(Input.GetKey(KeyCode.Escape) ||
			(KinectSkeleton.skeleton[0]["Hand_Right"].x < KinectSkeleton.skeleton[0]["Hand_Left"].x))
		{
			Application.LoadLevel(0);
		}
		
		gameObject.transform.Translate((newKinectPos - oldKinectPos)*scaling);
		
	}
	
	/**
	 * IS USED FOR DEBUGGING
	 **/
	//private int frame = 0;
}
