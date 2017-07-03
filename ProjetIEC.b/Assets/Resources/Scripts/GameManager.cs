using UnityEngine;
using System.Collections;

/* 	GameManager est utilisé pour charger les niveaux
	Il contient le GUI du menu principal, ainsi qu'un curseur (qui se déplace en fonction des gestes de la main),
	et active les "levels" (qui représente les jeux) en fonction des boutons activés.
	
	Le GameManager doit ne pas être détruit pour permettre de revenir des jeux ; 
	il est de plus attaché à un objet persistant (appellé lui-même GameManager) qui contient des informations utiles comme
	les classes d'accès à la Kinect.
*/
public class GameManager : MonoBehaviour {
	
	private static GameManager gm = null;
	
	private bool afficheGUI = true;		
	private bool afficheCurseur = true;
	private Cursor curseur;
	private int compteur1 = 0, compteur2 = 0, compteurquit = 0;
	
	// Use this for initialization
	void Start () {
		Object.DontDestroyOnLoad(gameObject);
		curseur = (Cursor) GetComponent("Cursor");
	}
	
	// Update is called once per frame
	void Update () {
		/* Options to quit : 
		   - If the button Escape is pressed 
		   - If the custom cursor stays on the "Quitter" button for 2 seconds
		   - If the mouse clicks on "Quitter" */
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	
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
	
	
    void OnGUI () {
		// Lorsque l'on affiche le menu :
		if (afficheGUI)
		{
			float largeur = Screen.width ;
			float hauteur = Screen.height;
			// On vérifie si notre curseur est situé sur le bouton
			// S'il passe plus de trois secondes sur le même bouton, il est considéré comme activé
			if (estSurBouton(curseur.Position, largeur*15/100, hauteur*15/100, largeur/4, hauteur/4))
				compteur1++;
			else
				compteur1 = 0;
			if (estSurBouton(curseur.Position, largeur*60/100, hauteur*15/100, largeur/4, hauteur/4))
				compteur2++;
			else
				compteur2 = 0;
			if (estSurBouton(curseur.Position, largeur*37/100, hauteur*60/100, largeur/4, hauteur/4))
				compteurquit++;
			else
				compteurquit = 0;
				
			
			if (GUI.Button(new Rect(largeur*15/100, hauteur*15/100, largeur/4, hauteur/4), "Jeu n°1") || (compteur1 > 120))
				{
					afficheGUI = false;
					afficheCurseur = false;
					Application.LoadLevel(1);
				}
				
			if (GUI.Button(new Rect(largeur*60/100, hauteur*15/100, largeur/4, hauteur/4), "Jeu n°2") || (compteur2 > 120))
				{
					afficheGUI = false;
					afficheCurseur = false;
					Application.LoadLevel(2);
				}
				
			if (GUI.Button(new Rect(largeur*37/100, hauteur*60/100, largeur/4, hauteur/4), "Quitter") || (compteurquit > 120))
				{
					afficheGUI = false;
					afficheCurseur = false;
					Application.Quit();
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
	
	void Awake()
	{
		// The whole GameObject should be a singleton.
		// if there is more than one (which happens each time scene 0 is loaded beyond first time)
		// the new one should be destroyed
		if(gm == null)
			gm = this;
		else
		{
			gm.afficheGUI = true;
			gm.afficheCurseur = true;
			Destroy(gameObject);
		}
	}
}
