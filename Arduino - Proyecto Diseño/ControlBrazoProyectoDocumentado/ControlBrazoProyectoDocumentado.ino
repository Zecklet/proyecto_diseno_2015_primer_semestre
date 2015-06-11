#include <Servo.h> 
 
Servo Servos[8];
 
String Nombres[] = {"base1", "base2", "hombro", "codo","munecaa", "munecab", "munecac","pinza"};    //Nombre de los servos en el brazo, utilizado unicamente como referencia
int Pines[] = {12, 10, 5, 4, 7, 9, 11, 3};                   //Corresponde al pin en el que cada servo esta conectado 
int ValoresIniciales[] = {180,90,60,180,90,90,90,150};      //Corresponde con los valores iniciales de los servos y ademas se utiliza para almacenar los valores actuales de los mismos
int mMovimientosServos[] = {180,90,60,180,90,90,90,150};      //Corresponde con los valores a los cuales se tiene que mover el servo

int PrimerLoop = true;              //Variable que se utiliza para saber si es el primer loop del arduino
String inputString = "";            //posee el string que se envia por el serial 
boolean stringComplete = false;     //bool utilizado para saber que hay una nueva entrada en el serial

void setup() 
{ 
  Serial.begin(115200);            //Inicializa el puerto serial a su maxima velocidad
  for (int i = 0; i < 8; i++){     
    Servos[i].attach(Pines[i]);    //Asigna el pin que le corresponde a cada servo
  }
  inputString.reserve(200);        //Para el string de entrada se reserva un maximo de 200 bytes
} 

void loop() {
  if (stringComplete) { //Es verdadero cuando el mensaje enviado fue completado
    RecibirInstrucciones(inputString); //Decodifica el mensaje 
    inputString = "";                  //Reinicia el string para el siguiente mensaje
    stringComplete = false;            //Se coloca nuevamente la variable en false para que pueda recibir el siguiente mensaje
  }
  MoverServos();                      //Muevo los servos
}

/*
  SerialEvent occurs whenever a new data comes in the
 hardware serial RX.  This routine is run between each
 time loop() runs, so using delay inside loop can delay
 response.  Multiple bytes of data may be available.
 */
void serialEvent() {
  while (Serial.available()) {
    char inChar = (char)Serial.read();       //Lee un byte del puerto serial
    inputString += inChar;                   //Concatena el char a la cadena de string
    if (inChar == '\n') {                    //Si el dato entrante es un salto de linea, quiere decir que es el fin del mensaje
      stringComplete = true;
    } 
  }
}

//Funcion que se encarga de decodificar el string enviado por el serial, para colocar los servos en esa posicion
void RecibirInstrucciones(String pInstrucciones){
  int mEstado = 0;                            //Parametro utilizado para saber por si es un indice o un valor
  int mLargo = pInstrucciones.length()-2;     //Largo de la cadena menos el salto de linea 
  int mIndice = 0;                            //Nuevo indice encontrado
  int mValor = 0;                             //Valor del indice encontrado
  int mSigno;                                 //Signo del nuevo valor
  char mCaracter = 'a';                       //Corresponde al caracter que esta siendo evaluado
  for (int i = 0; i < mLargo; i++){
    mCaracter = pInstrucciones.charAt(i);
    if (mCaracter == '.'){
     mEstado = 1; 
    }
    else{ 
      if (mCaracter == ','){
        mEstado = 2; 
        mValor = 0;
        mSigno = 1;
      }
      else{
        if (mEstado == 1){
          mIndice = int(mCaracter) - 48;
        }
        else {
          if (mCaracter == '-'){
            mSigno = -1;
          }
          else{
            mValor = mValor*10 + mSigno*(int(mCaracter) - 48);      //Se reconstruye el valor entrante
          }
        }
      }
    }
    if ((i != 0 && mCaracter == '.') || (i+1 == mLargo)){
      if (mValor<=180 && mValor >= 0){
        mMovimientosServos[mIndice] = mValor;                      //Nuevo valor del servo
      }
    }
  }
}
//Funcion que se encarga de mover todos los servos es cada ciclo
void MoverServos(){
   if (PrimerLoop){    //En el primer loop se inicializan los angulos de los servos
     PrimerLoop = false;  
     InicializarAngulos();  
  }
  int mDireccion;                                  //Direccion en la cual se mueve el servo sea 1 (positivo) o -1 (negativo)
  int mDiferencia = 0;                             //Diferencia entre la posicion final del servo y la actual, para tomar una desicion de en que direccion ir
  for (int i = 1; i < 8; i++){
      mDiferencia = mMovimientosServos[i] - ValoresIniciales[i];
      if (2 > mDiferencia){                        //Si la diferencia es negativa, entonces tengo que disminuir los angulos
        mDireccion = -1;
      }
      else if (2 < mDiferencia){                    //Si la diferencia es positiva, entonces tengo que aumentar los angulos
        mDireccion = 1;
      }
      else{
        mDireccion = 0;                             //en caso contraio es porque ya llegue a mi destino
      }
      if (mDireccion!=0){
        if (ValoresIniciales[i]+mDireccion <= 180 || ValoresIniciales[i]+mDireccion >= 0)    //Restriccion de posiciones maximas y minimas de los servos
        {
          ValoresIniciales[i] += mDireccion;       //Aumenta o disminuye el valor del servo
          Servos[i].write(ValoresIniciales[i]);    //Escribe el nuevo angulo en el pin del serv
          
        }
      }
  }
  delay(15); 
}

//Funcion que pone los servomotores en la posicion incial de cada uno
void InicializarAngulos(){
  Servos[0].write(ValoresIniciales[0]);    //Escribe el primer valor en la base, para que llegue a una posicion fija y el otro servo lo mueva
  delay(1000);                             //Le un segundo de tiempo al primer servo de que se coloque en su posicion
  for (int i = 1; i < 8; i++){
    Servos[i].write(ValoresIniciales[i]);  //Inicializa los otros angulos, segun el array de valores iniciales
  }
  delay(1000); 
}
