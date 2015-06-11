using UnityEngine;
using System.Collections;

//Este fue el segundo metodo de movimiento del brazo que utilizaba la inclinacion de los dedos
//fue descartado por el uso tan complicado que tenia
public class MapeadorBrazoDedos : MapeadorBrazo {

	private Transform[] DedosDerecha = new Transform[5];	//dedos de la mano derecha 
	private Transform[] DedosIzquierda = new Transform[5];	//todos los dedos de la mano izquierda 
	private string mRuta;									//variable utilizada para encontrar todos los dedos
	private string FormatoRuta = "Bip01 R Finger{0}/Bip01 R Finger{0}1/Bip01 R Finger{0}2/Bip01 R Finger{0}Nub";	//formato o ruta necesario para encontrar los dedos en la mano

	private int [] AngulosMinimos = {180, 45, 60, 60, 0, 0, 0, 100};	//Limite inferior de cada angulo, al momento de calculara las posioines de los servos 
	private int [] AngulosMaximos = {180, 135, 180, 180, 180, 180, 180, 150};	//Limite superior del angulo de los servos
	private int[] mIndiceDedos = {1, 2, 3, 4, 5, 7};	//Indices de los angulos que se deben modificar
	
	void Start () {
		StartCoroutine (ControlDedos ());				//Empieza la rutina que se encarga de controlar los dedos
	}	

	void Update () {
		if (base.mPalmaDerecha == null) {				//Si la mano no existe, se tiene que tratar de obtener 
			base.mPalmaDerecha = GameObject.FindGameObjectWithTag ("palmaIzq");	//Se busca la mano izquierda
			if (base.mPalmaDerecha != null) {									//Si existe, se obtienen todos los dedos de la mano
				ObtenerDedos(DedosDerecha, base.mPalmaDerecha.transform);		
			}
		} 
		if (base.mPalmaIzquierda == null) {				//Para la mano izquierda, se trata de la misma manera
			base.mPalmaIzquierda = GameObject.FindGameObjectWithTag("palmaDer");
			if (base.mPalmaIzquierda != null){
				ObtenerDedos(DedosIzquierda, base.mPalmaIzquierda.transform);
			}
		}
	}

	//Se sacan los dedos de la mano
	private void ObtenerDedos(Transform[] pArrayDedos, Transform pPalma){
		if (pPalma!=null){
			for (int i = 0; i < 5; i++){
				mRuta = string.Format(FormatoRuta, i);
				pArrayDedos[i] = pPalma.Find (mRuta);	//Utilizando la ruta de los dedos, se obtiene cada uno de ellos
			}
		}
	}

	//Funcion que se encarga de obtener los datos de los dedos y transformarlos en angulos 
	private IEnumerator ControlDedos(){
		float mDiferencia = 0;						//variable que guarda la diferencia de la altura de los dedos con respecto a la palma
		float mDiferenciaMinimaCambio = 0.6f;		//Diferencia que se necesita para empezar a cambiar la poscion del angulo
		for (;;) {							
			yield return new WaitForSeconds (0.025f); 	//el algoritmo se ejecuta cada 25ms
			if (base.mPalmaDerecha != null) {
				for (int i = 0; i < 3; i++){			//El algoritmo se aplica para el pulgar, el idice y el dedos central
					mDiferencia = DedosDerecha [i].transform.position.y - base.mPalmaDerecha.transform.position.y;	//Se calcula la diferencia de posicones y
					if (mDiferencia<-mDiferenciaMinimaCambio){	//si es negativa, la poscion del servo motor disminuye de uno en uno
						CambiarAngulo(mIndiceDedos[i], -1);
					}
					else if (mDiferencia>mDiferenciaMinimaCambio){	//Si es positiva, aumenta de uno en uno 
						CambiarAngulo(mIndiceDedos[i], 1);
					}
				}
			}
			if (base.mPalmaIzquierda != null) {		//Mismo proceso para los dedos de la mano izquierda
				for (int i = 0; i < 3; i++){
					mDiferencia = DedosIzquierda [i].transform.position.y - base.mPalmaIzquierda.transform.position.y;
					if (mDiferencia<-mDiferenciaMinimaCambio){
						CambiarAngulo(mIndiceDedos[i+3], -1);
					}
					else if (mDiferencia>mDiferenciaMinimaCambio){
						CambiarAngulo(mIndiceDedos[i+3], 1);
					}
				}
			}
		}
	}

	//Funcion que se encarga de cambiar el angulo de un servo motor, utilizando un indice y la direccion (-1 o +1)
	//Ademas utiliza los limites especificados al inicio de la clase
	protected void CambiarAngulo(int pIndiceAngulo, int pDireccion){
		int mNuevoValor = base.Servos [pIndiceAngulo] + pDireccion;
		if (mNuevoValor >= this.AngulosMinimos [pIndiceAngulo] && mNuevoValor <= this.AngulosMaximos[pIndiceAngulo]) {
			base.Servos[pIndiceAngulo] = mNuevoValor;
		}
	}

}
