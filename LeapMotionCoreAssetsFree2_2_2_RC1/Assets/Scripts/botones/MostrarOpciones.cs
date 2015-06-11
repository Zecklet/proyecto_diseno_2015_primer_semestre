using UnityEngine;
using System.Collections;

public class MostrarOpciones : MonoBehaviour {
	public GameObject Panel;

	public void Ejecutar(){
		if (this.Panel!=null){
			this.Panel.SetActive(!this.Panel.activeSelf);
		}
	}
}
