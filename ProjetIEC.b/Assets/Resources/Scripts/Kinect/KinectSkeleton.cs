using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KinectSkeleton : MonoBehaviour {

	public static List<Dictionary<string, Vector3>> skeleton = null;
	[SerializeField]
	private SkeletonWrapper sw;
	private List<string> bone_names;

	// Use this for initialization
	void Start () {
		bone_names = new List<string> {
			"Hip_Center", "Spine", "Shoulder_Center", "Head",
			"Shoulder_Left", "Elbow_Left", "Wrist_Left", "Hand_Left",
			"Shoulder_Right", "Elbow_Right", "Wrist_Right", "Hand_Right",
			"Hip_Left", "Knee_Left", "Ankle_Left", "Foot_Left",
			"Hip_Right", "Knee_Right", "Ankle_Right", "Foot_Right"
		};
		skeleton = new List<Dictionary<string,Vector3>> {
			new Dictionary<string,Vector3>(), new Dictionary<string,Vector3>()
		};
		// store bones in a dictionary for easier access
		foreach (string name in bone_names)
		{
			for(int i=0; i<skeleton.Count; i++)
			{
				skeleton[i].Add(name, new Vector3());
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if(sw.pollSkeleton())
		{
			int i = 0;
			foreach(string name in bone_names)
			{
				for (int ii = 0; ii < skeleton.Count; ii++)
				{
					skeleton[ii][name] = sw.bonePos[ii,i];
				}
				i++;
			}
		}
	}
}
