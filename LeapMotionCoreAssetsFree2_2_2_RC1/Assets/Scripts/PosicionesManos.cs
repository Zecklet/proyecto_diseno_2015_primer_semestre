using UnityEngine;
using System.Collections;

//Clase que asigna la esfera que calcula si la mano esta abierta o cerrada
public class PosicionesManos : MonoBehaviour {

	public GameObject cEsferaPrueba = null;

	private Vector3 cUltimaPosicionPalmaDerecha = Vector3.zero;
	private Vector3 cUltimaPosicionPalmaIzquierda = Vector3.zero;

	private GameObject EsferaIzquierda;
	private GameObject EsferaDerecha;
	
	void Start () {
		this.EsferaIzquierda = (GameObject) GameObject.Instantiate (cEsferaPrueba);
	}

	void Update () {
		GameObject mPalmaIzquierda = GameObject.FindGameObjectWithTag("palmaIzq");

		if (mPalmaIzquierda != null) {	//Si la mano existe, se coloca la esfera detro y debajo de la palma
			cUltimaPosicionPalmaIzquierda = mPalmaIzquierda.transform.position;
			this.EsferaIzquierda.transform.position = cUltimaPosicionPalmaIzquierda - mPalmaIzquierda.transform.up*0f;
			this.EsferaIzquierda.SetActive(true);	
		}
		else{
			this.EsferaIzquierda.SetActive(false);	//Se desactiva si la mano desaparece
		}
	}

}
