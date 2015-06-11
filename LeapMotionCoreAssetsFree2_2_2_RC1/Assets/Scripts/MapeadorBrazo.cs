using UnityEngine;
using System.Collections;

//Clase principal que convierte las entradas del Leap en angulos
public class MapeadorBrazo : MonoBehaviour {

	protected int[] Servos = {180,90,100,150,90,90,90,150};	//posiciones de los servomotores
	protected GameObject mPalmaDerecha = null;				//palma derecha (en realizada se encuentran invertidas, por el efecto de espejo que se puso a la aplicacion)
	protected GameObject mPalmaIzquierda = null;			//palma izquierda 
	
	void Start () {

	}

	void Update () {
		if (mPalmaDerecha == null) {	//Si el objeto es nulo, se trata de recuperar de la escena
			mPalmaDerecha = GameObject.FindGameObjectWithTag("palmaIzq");	//Se busca al objeto
		}
		else {
			ServoBase(mPalmaDerecha.transform.position);	//Se calcula la posicion de la base usando la componente 
			CalcularPorcentajeCerrado mCalculador = GameObject.FindObjectOfType<CalcularPorcentajeCerrado>();	//Se obtiene el calculador de la mano cerrada en la mano derecha0
			if (mCalculador != null){	//si lo pudo encontrar, cambie la rotacion de la pinza
				ServoPinza(mCalculador.getManorCerrada());
			}
		}
		if (mPalmaIzquierda == null) {	//Si el objeto es nulo, se trata de recuperar de la escena
			mPalmaIzquierda = GameObject.FindGameObjectWithTag("palmaDer");	//Se busca al objeto
		}
		else {

			ServoHombro(mPalmaIzquierda.transform.position);		//Se calcula la rotacion del hombro utilizando la componente pertinenete
			ServoMunecaB(mPalmaIzquierda.transform.eulerAngles);	//Se calcula la rotacion de la muñeca b utilizando la rotacion de la mano izquierda pertinenete
		}
	}

	//Funcion que calcula la rotacion de la base del servomotor, utilizando puntos fijos en el espacio al aplicarle una funcion lineal
	//Ademas cuenta con limites de rotacion, se utiliza la componente x
	private void ServoBase(Vector3 pPosicion){
		Vector2 mPosInicial = new Vector2 (3f,45f);
		Vector2 mPosFinal = new Vector2 (0f,135f);
		Servos [1] =  FuncionLineal(mPosInicial, mPosFinal, float.Parse(pPosicion.x.ToString("0.0")), 45, 135);
	}
	//Funcion que calcula la rotacion del hombro del servomotor, utilizando puntos fijos en el espacio al aplicarle una funcion lineal
	//Ademas cuenta con limites de rotacion, se utiliza la componente y
	private void ServoHombro(Vector3 pPosicion){
		Vector2 mPosInicial = new Vector2 (10f,60f);
		Vector2 mPosFinal = new Vector2 (4f,180f);
		Servos [2] =  FuncionLineal(mPosInicial, mPosFinal, float.Parse(pPosicion.y.ToString("0.0")), 60, 180);

	}
	//Funcion que calcula la rotacion del codo del servomotor, utilizando puntos fijos en el espacio al aplicarle una funcion lineal
	//Ademas cuenta con limites de rotacion, utiliza la componente z como entrada, esta funcion no es utilizada, pero extiste
	private void ServoCodo(Vector3 pPosicion){
		Vector2 mPosInicial = new Vector2 (-1f,60f);
		Vector2 mPosFinal = new Vector2 (6f,150f);
		Servos [3] = FuncionLineal(mPosInicial, mPosFinal, pPosicion.z, 60, 180);

	}
	//Funcion que calcula la rotacion de la muneca A del servomotor, utilizando puntos fijos en el espacio al aplicarle una funcion lineal
	//Ademas cuenta con limites de rotacion, utiliza la componente x de la mano izquierda, esta articulacion esta fija
	private void ServoMunecaA(Vector3 pPosicion){
		Vector2 mPosInicial = new Vector2 (3f,0f);
		Vector2 mPosFinal = new Vector2 (-3f,180f);
		Servos [4] =  FuncionLineal(mPosInicial, mPosFinal, pPosicion.x, 0, 180);
	}

	//Funcion que calcula la rotacion de la muneca b del servomotor, utilizando puntos fijos en el espacio al aplicarle una funcion lineal
	//Ademas cuenta con limites de rotacion, se utiliza la rotacion en z de la mano izquierda 
	private void ServoMunecaB(Vector3 pAngulosEuler){
		Vector2 mPosInicial = new Vector2 (150f,0);
		Vector2 mPosFinal = new Vector2 (220f,180f);
		Servos [5] = (int) pAngulosEuler.z;
		Servos [5] = FuncionLineal(mPosInicial, mPosFinal, Servos [5], 0, 180);

	}
	//Funcion que cambia la posciion de la pinza dependiendo del estado de la mano
	private void ServoPinza(bool pEstadoPinza){
		if (pEstadoPinza) {
			Servos [7] = 100;
		} else {
			Servos [7] = 150;
		}
	}
	//Esta funcion lo que hace es crear una funcion lineal con los datos de entrada, y limita el resultado segun los limites especificados
	private int FuncionLineal (Vector2 pPosInicial, Vector2 pPosFinal, float pX, int pLimiteInf, int pLimiteSup){
		float m = (pPosFinal.y-pPosInicial.y)/(pPosFinal.x - pPosInicial.x);
		float mNuevaPosicion = m * pX * 1f + pPosInicial.y - m * pPosInicial.x;
		if (mNuevaPosicion < pLimiteInf) {
			mNuevaPosicion = pLimiteInf;
		} else if (mNuevaPosicion > pLimiteSup) {
			mNuevaPosicion = pLimiteSup;
		}
		return (int)mNuevaPosicion;
	}

	//Los angulos de rotacion que se obtiene de los objetos se encuentra entre el rango de 0 a 360, pero el cambio de 0 a -65
	// y posteriores, si no que salta de 0 a 300, cuando este disminuye, esta funcion convierte el 300 a -60
	private int ObtenerAnguloRealDeEuler(int pAngulo){
		int mResultado = pAngulo % 90;
		if (pAngulo < 180) {
			mResultado += 90;
		}
		return mResultado;
	}

	//Se obtienen todoas la rotaciones del objeto
	public int[] getRotaciones(){
		return Servos;
	}
}
