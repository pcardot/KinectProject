using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game2Manager : MonoBehaviour {

	public static Game2Manager instance = null;
	private ObjectFactory objFactory; // creates/destroys objects properly
	private Score scoreManager;
	
	private Queue<GameObject> aliveObjects; // lists all alive objects coming to us
	private int time = 0; // allows to create objects periodically
	private System.Random rand; // allows all sorts of random stuff that make the game funnier
	
	[SerializeField]
	private Texture2D warningSign;
	
	private bool gameContinues = true;
	
	private Cursor curseur;
	private int compteurRetry = 0, compteurQuit = 0;
	private bool afficheCurseur = false;

	/**
	 * Unity Generic Event delegates
	 **/
	// Use this for initialization
	void Start () {
		KinectEventThrower eventsThrower = KinectEventThrower.instance;
		eventsThrower.rightReceived += new KinectEventThrower.rightReceivedHandler(rightActivated);
		eventsThrower.leftReceived += new KinectEventThrower.leftReceivedHandler(leftActivated);
		eventsThrower.upReceived += new KinectEventThrower.upReceivedHandler(upActivated);
		
		objFactory = ObjectFactory.instance;
		scoreManager = Score.instance;
		
		aliveObjects = new Queue<GameObject>();
		rand = new System.Random();
		
		curseur = (Cursor) GetComponent("Cursor");
	}
	
	// Update is called once per frame
	void Update () {
		if(gameContinues && time % 100 == 0)
		{
			double randNB = rand.NextDouble();
			if(randNB < 1.0/3)
			{
				aliveObjects.Enqueue(objFactory.getObject(ObjectFactory.positions.left));
			}
			else if(randNB < 2.0/3)
			{
				aliveObjects.Enqueue(objFactory.getObject(ObjectFactory.positions.right));
			}
			else
			{
				aliveObjects.Enqueue(objFactory.getObject(ObjectFactory.positions.up));
			}
		}
		
		
		if (aliveObjects.Count > 0 && aliveObjects.Peek().transform.position.z <= -6)
		{
			objFactory.releaseObject(aliveObjects.Dequeue());
			gameContinues = scoreManager.scoreFail();
			if(!gameContinues)
			{
				while(aliveObjects.Count > 0)
				{
					objFactory.releaseObject(aliveObjects.Dequeue());
				}
			}
		}
		
		time++;
	}
	
	void Awake () {
		if (instance != null)
			Debug.Log("Game2Manager is supposed to be a singleton");
		else
			instance = this;
	}
	
	void OnGUI()
	{
		float larg = Screen.width;
		float haut = Screen.height;
		if(gameContinues)
		{
			
			if (aliveObjects.Count > 0 && aliveObjects.Peek().transform.position.z <= 0)
			{
				ObjectMovingScript OMS = (ObjectMovingScript) aliveObjects.Peek().GetComponent("ObjectMovingScript");
				switch (OMS.pos)
				{
					case ObjectFactory.positions.left:
						GUI.DrawTexture(new Rect(larg /3-15*larg/200, haut/2-15*larg/200, 
												15*larg/100, 15*larg/100), 
										warningSign);
						break;
					case ObjectFactory.positions.right:
						GUI.DrawTexture(new Rect(2* larg /3-15*larg/200, haut/2-15*larg/200, 
												15*larg/100, 15*larg/100), 
										warningSign);
						break;
					case ObjectFactory.positions.up:
						GUI.DrawTexture(new Rect(larg /2-15*larg/200, haut/3-15*larg/200, 
												15*larg/100, 15*larg/100), 
										warningSign);
						break;
					default:
						break;
				}
			}
		}
		else
		{
			afficheCurseur = true;
		
			// On vérifie si notre curseur est situé sur le bouton
			// S'il passe plus de trois secondes sur le même bouton, il est considéré comme activé
			if (estSurBouton(curseur.Position, larg*15/100, 2*haut/3 + haut/25, larg/4, haut/4))
				compteurRetry++;
			else
				compteurRetry = 0;
			if (estSurBouton(curseur.Position, larg*60/100, 2*haut/3 + haut/25, larg/4, haut/4))
				compteurQuit++;
			else
				compteurQuit = 0;
				
			
			if (GUI.Button(new Rect(larg*15/100, 2*haut/3 + haut/25, larg/4, haut/4), "Réessayer") || (compteurRetry > 120))
				{
					scoreManager.reset();
					afficheCurseur = false;
					time=0;
					gameContinues = true;
				}
				
			if (GUI.Button(new Rect(larg*60/100, 2*haut/3 + haut/25, larg/4, haut/4), "Quitter") || (compteurQuit > 120))
				{
					Application.LoadLevel(0);
				}
		}
		
		if(afficheCurseur)
		{
			GUI.DrawTexture (new Rect(curseur.Position.x-curseur.cursorSizeX/2, 
						  (Screen.height-curseur.Position.y)-curseur.cursorSizeY/2, 
						  curseur.cursorSizeX, 
						  curseur.cursorSizeY), 
					 curseur.customTexture);
		}
	}
	
	/**
	 * Custom Event delegates
	 **/
	void rightActivated()
	{
		if (aliveObjects.Count > 0 && aliveObjects.Peek().transform.position.z <= 0)
		{
			ObjectMovingScript OMS = (ObjectMovingScript) aliveObjects.Peek().GetComponent("ObjectMovingScript");
			if(OMS.pos == ObjectFactory.positions.right)
			{
				gameContinues = scoreManager.scoreSuccess(-1 * aliveObjects.Peek().transform.position.z);
				objFactory.releaseObject(aliveObjects.Dequeue());
			}
		}
	}
	
	void leftActivated()
	{
		if (aliveObjects.Count > 0 && aliveObjects.Peek().transform.position.z <= 0)
		{
			ObjectMovingScript OMS = (ObjectMovingScript) aliveObjects.Peek().GetComponent("ObjectMovingScript");
			if(OMS.pos == ObjectFactory.positions.left)
			{
				gameContinues = scoreManager.scoreSuccess(-1 * aliveObjects.Peek().transform.position.z);
				objFactory.releaseObject(aliveObjects.Dequeue());
			}
		}
	}
	
	void upActivated()
	{
		if (aliveObjects.Count > 0 && aliveObjects.Peek().transform.position.z <= 0)
		{
			ObjectMovingScript OMS = (ObjectMovingScript) aliveObjects.Peek().GetComponent("ObjectMovingScript");
			if(OMS.pos == ObjectFactory.positions.up)
			{
				gameContinues = scoreManager.scoreSuccess(-1 * aliveObjects.Peek().transform.position.z);
				objFactory.releaseObject(aliveObjects.Dequeue());
			}
		}
	}
	
	/**
	 * Private tools
	 **/
	// Checks if the custom cursor is on a button which position is specified by args2-5
	private bool estSurBouton(Vector2 curseurposition,
							  float xbouton, 
							  float ybouton, 
							  float largeur, 
							  float hauteur)
	{
		return (
				 (curseurposition.x > xbouton) && 
				 (curseurposition.x < xbouton + largeur ) &&
				 (Screen.height - curseurposition.y > ybouton) &&
				 (Screen.height - curseurposition.y < ybouton + hauteur)
				);
	}
}
