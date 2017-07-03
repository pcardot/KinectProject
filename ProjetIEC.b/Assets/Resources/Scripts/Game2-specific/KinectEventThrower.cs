using UnityEngine;
using System.Collections;

public class KinectEventThrower : MonoBehaviour {

	public static KinectEventThrower instance = null;
	
	/**
	 * Delegates for events that notify an eligible movement has been detected
	 **/
	public delegate void rightReceivedHandler();
	public delegate void leftReceivedHandler();
	public delegate void upReceivedHandler();
	public delegate void bonusDownReceivedHandler();
	public delegate void jumpReceivedHandler();
	
	/**
	 * Events that notify an eligible movement has been detected
	 **/
	public event rightReceivedHandler rightReceived;
	public event leftReceivedHandler leftReceived;
	public event upReceivedHandler upReceived;
	public event bonusDownReceivedHandler bonusDownReceived;
	public event jumpReceivedHandler jumpReceived;

	// Etat permet de mémoriser la dernière position de la personne (main a droite,
	// a gauche, en haut ou au centre ...)
	private int etat;
	
	// Permet de moduler les distances pour activer les events
	[SerializeField]
	private float ddroite = 0.30f, dgauche = 0.20f, dhaut = 0.30f, dsaut = 0.05f,
				  marge = 0.10f, dbassin = 0.15f;
	
	private float positionDepartBassin, positionDepartPiedDroit, positionDepartPiedGauche;
	
	// Use this for initialization
	void Start () {
		etat = 0;
		positionDepartBassin = (KinectSkeleton.skeleton[0]["Hip_Right"].y + KinectSkeleton.skeleton[0]["Hip_Left"].y)/2;
		positionDepartPiedDroit = KinectSkeleton.skeleton[0]["Foot_Right"].y;
		positionDepartPiedGauche = KinectSkeleton.skeleton[0]["Foot_Left"].y;
	}
	
	// Update is called once per frame
	void Update () {
		// On regarde les mouvements détectés par la kinect et on envoie l'évènement correspondant.
		if (!(etat == 1))
		{
			if (testRight())
			{
				etat = 1;
				rightReceived();
			}
		}
		if (!(etat == 2))
		{
			if (testLeft())
			{
				etat = 2;
				leftReceived();
			}
		}
		if (!(etat == 3))
		{
			if (testUp())
			{
				etat = 3;
				upReceived();
			}
		}
		if (!(etat == 10))
		{
			if (testBonusDown())
			{
				etat = 10;
				bonusDownReceived();
			}
		}
		if (!(etat == 11))
		{
			if (testJump())
			{
				etat = 11;
				jumpReceived();
			}
		}
		if (!testRight() && !testLeft() && !testUp() && 
			!testJump() && !testBonusDown()) // détecte lorsque qu'aucun mouvement n'est fait
		{
			etat = 0;
		}
	}

	
	// Renvoie si l'évènement rightReceived doit être levé lorsque notre main droite se trouve 
	// à plus de 30 cm à droite de l'épaule droite
	bool testRight()
	{
		if (rightReceived != null)
		{
			return (KinectSkeleton.skeleton[0]["Hand_Right"].x > KinectSkeleton.skeleton[0]["Shoulder_Right"].x + ddroite);
		}
		return false;
	}
	
	// Renvoie si l'évènement leftReceived doit être levé lorsque notre main droite se trouve 
	// à plus de 20 cm à gauche de l'épaule gauche
	bool testLeft(){
		if (leftReceived != null)
		{
			return (KinectSkeleton.skeleton[0]["Hand_Right"].x < KinectSkeleton.skeleton[0]["Shoulder_Right"].x - dgauche);
		}
		return false;
	}
	
	// Renvoie si l'évènement upReceived doit être levé lorsque notre main droite et notre 
	// main gauche se trouvent à plus de 30 cm aux dessus de nos épaules.
	bool testUp(){
		if (upReceived != null)
		{
			return (KinectSkeleton.skeleton[0]["Hand_Right"].y > KinectSkeleton.skeleton[0]["Shoulder_Right"].y + dhaut
					&& KinectSkeleton.skeleton[0]["Hand_Left"].y > KinectSkeleton.skeleton[0]["Shoulder_Left"].y + dhaut);
		}
		return false;
	}
	
	// Renvoie si l'évènement bonusDownReceived doit être levé lorsque notre main droite et 
	// notre main gauche se trouvent à +ddroite cm (resp -ddroite cm) de notre épaule droite (resp gauche)
	// (selon x)
	// et si au même niveau que les épaules (selon y) a + ou moins marge cm
	// de plus la hauteur du bassin doit avoir diminué de dbassin par rapport à sa position de départ
	bool testBonusDown(){
		if (bonusDownReceived != null)
		{
			return (KinectSkeleton.skeleton[0]["Hand_Right"].x > KinectSkeleton.skeleton[0]["Shoulder_Right"].x + ddroite &&
					KinectSkeleton.skeleton[0]["Hand_Left"].x > KinectSkeleton.skeleton[0]["Shoulder_Left"].x - ddroite &&
					KinectSkeleton.skeleton[0]["Hand_Right"].y < KinectSkeleton.skeleton[0]["Shoulder_Right"].y + marge &&
					KinectSkeleton.skeleton[0]["Hand_Left"].y < KinectSkeleton.skeleton[0]["Shoulder_Left"].y + marge &&
					KinectSkeleton.skeleton[0]["Hand_Right"].y > KinectSkeleton.skeleton[0]["Shoulder_Right"].y - marge &&
					KinectSkeleton.skeleton[0]["Hand_Left"].y > KinectSkeleton.skeleton[0]["Shoulder_Left"].y - marge &&
					KinectSkeleton.skeleton[0]["Hip_Right"].y < positionDepartBassin - dbassin);
		}
		return false; 
	}
	
	
	// Renvoie si l'évènement jumpReceived doit être levé (lorsque l'on est en train de sauter)
	// On considère que l'on saute si la position en y des pieds a augmenté au moins de dsaut par 
	// rapport à leur position de départ
	bool testJump(){
		if (jumpReceived != null)
		{
			return (KinectSkeleton.skeleton[0]["Foot_Right"].y > positionDepartPiedDroit + dsaut &&
					KinectSkeleton.skeleton[0]["Foot_Left"].y > positionDepartPiedGauche + dsaut);
		}
		return false; 
	}
	
	void Awake () {
		if (instance != null)
			Debug.Log("KinectEventThrower is supposed to be a singleton");
		else
			instance = this;
			instance = this;
	}
}
