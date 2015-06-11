using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System;
	
public class PuertoSerial : MonoBehaviour
{
	public static SerialPort sp = new SerialPort ("COM3", 115200);	//Se configura lo que es el puerto de serial del arduino
	public static string strIn,message;        
		
	void Start ()
	{
		OpenConnection ();	//Al inicio se intenta abrir el puerto 
	}
		
	void Update ()
	{
	}

	//Lee la entrada del puerto
	public string Read(){
		strIn = sp.ReadLine ();
		return strIn;
	}

	//Funcion que se encarga de enviar una cadena de caracteres por el puerto serial
	public void Write(string pDatos){
		if (sp != null && sp.IsOpen) {
			sp.WriteLine (pDatos);
		} else {
			sp.Open ();  // si el puerto esta cerrado intenta abrir la conexion 
			sp.ReadTimeout = 50;  // se coloca el tiempo maximo de espera en 50 ms 
			message = "Port Opened!";	
		}
	}

	//Se obtiene el estado del puerto
	public bool Conectado(){
		return sp.IsOpen;
	}

	//funcon que abre el puerto
	public void OpenConnection ()
	{
		if (sp != null) {
			if (sp.IsOpen) {
				sp.Close ();
				message = "Closing port, because it was already open!";
			} else {
				sp.Open ();  // opens the connection
				sp.ReadTimeout = 50;  // sets the timeout value before reporting error
				message = "Port Opened!";
			}
		} else {
			if (sp.IsOpen) {
				print ("Port is already open");
			} else {
				print ("Port == null");
			}
		}
	}
		
	//CUando la aplicacion termina, el puerto es cerrado
	void OnApplicationQuit ()
	{
		sp.Close ();
	}
}
