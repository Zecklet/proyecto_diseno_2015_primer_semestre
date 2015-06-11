using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//Esta clase lo que hace es recolectar informacion de los mapeadores que convierten la informacion obtenida del 
//Leap, la transforma en posiciones y luego las enviar al puerto serial
public class Observador : MonoBehaviour {
	public GameObject LedRojo;	//Led rojo que se muestre en la interfaz grafica cuando no hay conexion con el arduino
	public GameObject LedVerde;	//Led verde que se muestra cuando hay conexion con el arduino

	public PuertoSerial Enviador;	//Objeto que se encarga de enviar la informacion por el serial 
	public MapeadorBrazo Recolector;//Mapeador del leap a angulos

	private Slider[] ValoresServosGraficos = new Slider[7];	//Corresponde con las barras de progreso
	private string[] TagsServos = {"base", "hombro", "codo", "munecac", "munecab", "munecaa", "pinza"};	//Tags utilizados para obtener las barras de progreso

	void Start () {
		for (int i = 0; i < 7; i++) {
			ValoresServosGraficos[i] = GameObject.FindGameObjectWithTag(TagsServos[i]).GetComponent<Slider>();	//Se obtienen las barras de progreso
		}
		StartCoroutine (ActualizarPosicionesArduino ()); //Inicia la rutina que envia las posiciones de los servomotores 

	}

	void Update () {
		ActualizarGUI (); 
	}

	//Metodo que se encarga de enviar la informacion de las posiciones por el puerto serial
	private IEnumerator ActualizarPosicionesArduino(){
		string mDatos;
		yield return new WaitForSeconds (2f);	//Al inicio espera dos segundos para esperar al brazo robotico
		for (;;) {
			yield return new WaitForSeconds (0.1f);	//este wait hace esperar al enviardor una decima de segundo, por lo que se actializan las posiciones diez veces por segundo
			mDatos = "";
			for (int i = 1; i < 8; i++){
				mDatos += "." + i + "," + Recolector.getRotaciones()[i]; //construye el formato que acepta el arduino
			}
			Enviador.Write (mDatos);	//Envia los datos
		}
	}

	//Se encarga de actualizar los valores de la interfaz
	private void ActualizarGUI(){
		int[] mRotaciones = Recolector.getRotaciones ();
		for (int i = 0; i < 7; i++) {
			ValoresServosGraficos[i].value = mRotaciones[i+1];	//Actualiza las posiciones de los servomotores
		}

		if (this.LedRojo != null && this.LedVerde != null) {	//Actulaiza el estado de la conexion 
			if (Enviador.Conectado ()) {
				this.LedRojo.SetActive (false);
				this.LedVerde.SetActive (true);
			} else {
				this.LedRojo.SetActive (true);
				this.LedVerde.SetActive (false);
			}
		}
	}
}
