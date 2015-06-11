using UnityEngine;
using System.Collections;

//Esta clase lo que hace es cambiar el aspecto de la aplicacion a 800x500
public class ApplicationManager : MonoBehaviour {
	
	void Start(){
		PlayerPrefs.SetInt("Screenmanager Resolution Width", 800);
		PlayerPrefs.SetInt("Screenmanager Resolution Height", 500);
		PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
	}

	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	void OnApplicationQuit () {
		PlayerPrefs.SetInt("Screenmanager Resolution Width", 800);
		PlayerPrefs.SetInt("Screenmanager Resolution Height", 500);
		PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
	}
}
