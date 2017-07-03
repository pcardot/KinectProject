using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	public static Score instance = null;
	
	[SerializeField]
	private int nbOfLifeTotal = 3, basegain = 100, malus = 10, scoreWin = 100000;
	private int comboMultiplyer, nbOfLife; 
	private int score;
	[SerializeField]
	private float timeRemaining = 10;
	
	[SerializeField]
	private Texture2D defeatSign, successSign;
	
	// Use this for initialization
	void Start () {
		comboMultiplyer = 0;
		score = 0;
		nbOfLife = nbOfLifeTotal;
		timeRemaining = timeRemaining * 60 * 60; // On convertie les minutes en frame
	}
	
	public void reset () {
		Start();
	}
	
	// Update is called once per frame
	void Update () {
		timeRemaining--;
	}
	
	void Awake()
	{
		if(instance != null)
			Debug.Log("Score devrait être un singleton");
		else
			instance = this;		
	}
	
	// scoreFail est appelée lorsque l'on n'a pas détruit un objet
	// elle décrémente notre nombre de vie (nbOfLife) et remet le comboMultiplyer à 0
	// renvoie true lorsque le jeu continue (encore des vies) et false sinon
	public bool scoreFail()
	{
		nbOfLife--;
		comboMultiplyer = 0;
		score = (score - basegain * 6 * malus <= 0) ? 0 : score - basegain * 6 * malus;
		return (! (nbOfLife <= 0));
	}
	
	// scoreSuccess est appelée lorsque l'on a détruit un objet
	// incrémente le comboMultiplyer et le score correctement
	// renvoie true lorsque le jeu continue, false sinon
	public bool scoreSuccess(float multi)
	{
		comboMultiplyer ++;
		score +=(int) (basegain * comboMultiplyer * multi);
		return (!(score >= scoreWin));
	}
	
	void OnGUI()
	{
		float longueur = Screen.width;
		float hauteur = Screen.height;
		if(nbOfLife > 0 && score <= scoreWin)
		{
			GUI.Box(new Rect(5*longueur/100, 80*hauteur/100,longueur/10, hauteur/25), score.ToString());
			GUI.Box(new Rect(5*longueur/100, 85*hauteur/100,longueur/3, hauteur/25), 
					"vie" + ((nbOfLife > 1) ? "s" : "") + " restante" + ((nbOfLife > 1) ? "s" : "") + " : "+ nbOfLife.ToString() );
		
			int nbMinute = (int) (timeRemaining /3600);
			GUI.Box(new Rect(80 * longueur/100 - longueur/20, 85*hauteur/100, longueur/10, hauteur/25),
			nbMinute.ToString() + "min" + ((timeRemaining - nbMinute)*60).ToString() + "sec");
		}
		else
		{
			if(nbOfLife <= 0)
			{
				GUI.DrawTexture(new Rect(15*longueur/100, hauteur/3, 
												70*longueur/100, hauteur/3), 
										defeatSign);
			}
			else
			{
				GUI.DrawTexture(new Rect(15*longueur/100, hauteur/3, 
												70*longueur/100, hauteur/3), 
										successSign);
			}
			GUI.Box(new Rect(0, 2*hauteur/3,longueur, hauteur/25), score.ToString());
		}
	}
}
