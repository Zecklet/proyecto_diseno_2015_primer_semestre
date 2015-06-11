using UnityEngine;
using System.Collections;

public class MapeadorBrazoTeclado : MapeadorBrazoDedos {

	private string[] teclasArriba = {"","a","s","d","f","g","h","m"};	//teclas que mueven o aumentan el angulo de los servomoteres
	private string[] teclasAbajo = {"","z","x","c","v","b","n","j"};	//teclas que disminuyen los angulos en los servos
	
	void Start () {
		StartCoroutine (ControlBotones ());	//Se inicia la rutina de los servomotres
	}
	
	void Update () {
		
	}

	private IEnumerator ControlBotones(){	//Rutina que obtiene los datos de las teclas presionadas y modifica los angulos
		for (;;) {
			yield return new WaitForSeconds (0.0125f);	//Se obtiene los datos cada 1.25 ms
			for (int i = 1; i < 8; i++){
				if (Input.GetKey(this.teclasArriba[i])){	//Aumenta el angulo
					base.CambiarAngulo(i,1);
				}
				else if (Input.GetKey(this.teclasAbajo[i])){	//Disminuye el angulo
					base.CambiarAngulo(i,-1);
				}
			}
		}
	}
}
