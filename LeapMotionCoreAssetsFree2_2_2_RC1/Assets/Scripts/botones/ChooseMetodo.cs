using UnityEngine;
using System.Collections;

public class ChooseMetodo : MonoBehaviour {
	public MapeadorBrazo MetodoBrazo;
	public MapeadorBrazoDedos MetodoDedos;
	public MapeadorBrazoTeclado MetodoTeclado;
	public Observador ElQuePasaElChisme;
	private GameObject PanelLetras;

	void Start(){
		PanelLetras = GameObject.FindWithTag ("Panel_Letras");
		if (PanelLetras != null) {
			PanelLetras.SetActive(false);
		}

	}

	public void DesactivarTodo(){
		transform.GetComponent<MostrarOpciones> ().Ejecutar ();
		MetodoBrazo.enabled = false;
		MetodoDedos.enabled = false;
		MetodoTeclado.enabled = false;
		if (PanelLetras != null) {
			PanelLetras.SetActive(false);
		}
	}

	public void SetMetodoBrazo(){
		DesactivarTodo ();
		MetodoBrazo.enabled = true;
		ElQuePasaElChisme.Recolector = MetodoBrazo;
	}

	public void SetMetodoDedos(){
		DesactivarTodo ();
		MetodoDedos.enabled = true;
		ElQuePasaElChisme.Recolector = MetodoDedos;
	}

	public void SetMetodoTeclado(){
		DesactivarTodo ();
		MetodoTeclado.enabled = true;
		PanelLetras.SetActive(true);
		ElQuePasaElChisme.Recolector = MetodoTeclado;

	}

}
