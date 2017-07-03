using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

/*
		La classe cursor permet de gérer le déplacement du curseur dans le GUI lancer au début de l'application (choix du jeu ou de quitter)
		La mise à jour de la position de ce curseur se fait avec le déplacement de la main droite (détecté par la kinect)
		Le déplacement de la main est obtenu en comparant l'ancienne position détectée par la kinect et la position actuelle
*/

	// Permet éventuellement de dire qu'on est gauché par la suite
	[SerializeField]
	private string main = "Hand_Right";
	
	// Permet d'augmenter le déplacement du curseur par rapport au déplacement de la main si nécessaire
	[SerializeField]
	private float scale = 1.0f;
	public float cursorSizeX, cursorSizeY;
	public Texture2D customTexture;
	
	private Vector2 position ;
	public Vector2 Position 
	{	
		get
		{
			return position;
		}
		
		private set
		{
			position = value;
		}
	}

	
	private Vector3 newkinectposition; 
	public Vector3 NewKinectPosition 
	{	
		get
		{
			return newkinectposition;
		}
		
		private set
		{
			newkinectposition = value;
		}
	}

	private Vector3 oldkinectposition;
	public Vector3 OldKinectPosition 
	{	
		get
		{
			return oldkinectposition;
		}
		
		private set
		{
			oldkinectposition = value;
		}
	}
	
	
	
	
	// Use this for initialization
	void Start () 
	{
		// Initialiser la taille du curseur
		cursorSizeX = customTexture.width; 
		cursorSizeY = customTexture.height;
		
		// Initialisation de la position du curseur au milieu de l'écran
		position = new Vector2();
			position.x = Screen.width / 2 ;
			position.y = Screen.height / 2 ;	
			
			
		oldkinectposition = new Vector3();
		newkinectposition = new Vector3();
		
		
	}
	

	// Mise à jour de la position du curseur a partir de old et new position 
	void majPosCurseur()
	{
		float deplacementx = newkinectposition.x - oldkinectposition.x;
		float deplacementy = newkinectposition.y - oldkinectposition.y;
	
	// On évite avec ce if le bon du curseur lorsque le joueur est détecté
		if ((newkinectposition.x - oldkinectposition.x)< 0.2 && (newkinectposition.x - oldkinectposition.x)>-0.2 &&
			(newkinectposition.y - oldkinectposition.y)< 0.2 && (newkinectposition.y - oldkinectposition.y)>-0.2 &&
			(newkinectposition.z - oldkinectposition.z)< 0.2 && (newkinectposition.z - oldkinectposition.z)>-0.2)
		{
		// Mise à jour de la position du curseur en fonction du déplacement 
			position.x = position.x + deplacementx * scale * Screen.width;				
			position.y = position.y + deplacementy * scale * Screen.height;
			
		// On vérifie alors que le curseur reste dans les limitations de l'écran.
			if (position.x < 0) 
				position.x = 0;
			if (position.x > Screen.width)
				position.x = Screen.width;
			if (position.y < 0) 
				position.y = 0;
			if (position.y > Screen.height)
				position.y = Screen.height;
		}
	}

	// Retourne le vecteur de position de la main envoyé par la kinect
	Vector3 getPositionMain()
	{
		return (KinectSkeleton.skeleton[0][main]);
	}
	
	// Met à jour les old et new posisitons en fonction de ce qui est détecté par la kinect
	void majPositionMain()
	{
		oldkinectposition = newkinectposition;
		newkinectposition = getPositionMain();
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		// Ici il faut faire l'update en fonction des mouvements de la kinect
		// On a accès à un dictionnaire contenant les positions de toutes les parties du corps vues par la kinect
		// On considère que le jeu est sélectionné lorsque le cursor passe plus de trois secondes sur le bouton considéré
		majPositionMain();
		majPosCurseur();
	}
}
