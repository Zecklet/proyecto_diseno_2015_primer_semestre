using UnityEngine;
using System.Collections;

//Esta clase calcula el porcetanje al cual fue cerrada la mano de la persona
public class CalcularPorcentajeCerrado : MonoBehaviour {

	private SphereCollider cCollider;	//Esfera que crece hasta colisionar con lso dedos
	private float cRadioInicial = 0;	//Radio inicial de la esfera
	private int cCantidadContactos = 1;	//Cantidad de contactos minimos
	private int cCantidadContactosRealizados = 0;	//Cantidad de contactos realizardos
	private float cRazonCambio = 1;		//La razon de cambio con respecto al radio inicial
	
	void Start () {
		cCollider = GetComponent<SphereCollider> ();	//Al inicio se obtiene la esfera en la esena
		if (cCollider != null) {							
			cRadioInicial = cCollider.radius;			//Si no es vacio, se obtiene el radio inicial
		}
	}

	void FixedUpdate () {
		if (cCollider != null) {
			cCollider.radius += Time.deltaTime*10;		//Aumenta el radio en 10 unidades cada segundo
		}
	}

	void OnTriggerEnter(Collider other) {				//Si colisiona con el hueso de 3 de los dedos, se cuenta como contacto
		if (other.name == "bone3") {					
			cCantidadContactosRealizados++;				//Aumenta el contador
			if (cCantidadContactosRealizados == cCantidadContactos) {	//Si la cantidad de contactos es la minima es la solicitada
				cCantidadContactosRealizados = 0;						//se reainicia el contados de contactos
				cRazonCambio = Mathf.RoundToInt((cCollider.radius - cRadioInicial) / cRadioInicial);	//se calcula la razon de cambio 
				cCollider.radius = cRadioInicial;
			}
		} 
	}

	//La mano se considera cerrado si se cumple la condicion
	public bool getManorCerrada(){
		return cRazonCambio > 6;
	}
}
