/* Sweep
 by BARRAGAN <http://barraganstudio.com> 
 This example code is in the public domain.

 modified 8 Nov 2013
 by Scott Fitzgerald
 http://arduino.cc/en/Tutorial/Sweep
*/ 

#include <Servo.h> 
 
Servo myservo;  // create servo object to control a servo 
                // twelve servo objects can be created on most boards
 
int pos = 0;    // variable to store the servo position 
String Nombres[] = {"base1", "base2", "hombro", "codo","munecaa", "munecab", "munecac","pinza"};
//int Pines[] = {3, 4, 5, 6, 12, 11, 10, 9};
int Pines[] = {12, 10, 5, 4, 7, 9, 11, 3};
int ValoresIniciales[] = {180,90,100,180,90,90,90,150};
int mMovimientosServos[] = {180,90,100,90,90,90,0,100};
int mUltimoValor = 0;


int PrimerLoop = true;
String LecturaSerial = "";

String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete

Servo Servos[8];

void setup() 
{ 
  Serial.begin(115200);
  for (int i = 0; i < 8; i++){
    Servos[i].attach(Pines[i]);
  }
  inputString.reserve(200);
} 

void loop() {
  // print the string when a newline arrives:
  if (stringComplete) {
    RecibirInstrucciones(inputString); 
    // clear the string:
    inputString = "";
    stringComplete = false;
  }
  MoverServos();
}

/*
  SerialEvent occurs whenever a new data comes in the
 hardware serial RX.  This routine is run between each
 time loop() runs, so using delay inside loop can delay
 response.  Multiple bytes of data may be available.
 */
void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read(); 
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag
    // so the main loop can do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    } 
  }
}

void LeerSerial(){
   if (Serial.available() > 0) {
                // read the incoming byte:
                LecturaSerial = Serial.readString();

                // say what you got:
                //Serial.print("I received: ");
                //Serial.println(LecturaSerial);
                RecibirInstrucciones(LecturaSerial);
        }
}

void RecibirInstrucciones(String pInstrucciones){
  int mEstado = 0;
  int mLargo = pInstrucciones.length()-2;
  int mIndice = 0;
  int mValor = 0;
  int mSigno;
  char mCaracter = 'a';
  for (int i = 0; i < mLargo; i++){
    mCaracter = pInstrucciones.charAt(i);
    if (mCaracter == '.'){
     //Serial.println("Indice encontrado");
     mEstado = 1; 
    }
    else{ 
      if (mCaracter == ','){
        //Serial.println("Valor encontrado");
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
            mValor = mValor*10 + mSigno*(int(mCaracter) - 48);
            //Serial.println(mValor);
          }
        }
      }
    }
    if ((i != 0 && mCaracter == '.') || (i+1 == mLargo)){
      if (mValor<=180 && mValor >= 0){
        mMovimientosServos[mIndice] = mValor - ValoresIniciales[mIndice];
        Serial.print(mIndice);
        Serial.println(mMovimientosServos[mIndice]);
        Serial.print("\n");
      }
      //Serial.print("I received: i: ");
      //Serial.print(mIndice);
      //Serial.print(" mValor: ");
      //Serial.println(mValor);
      //Serial.print("\n");
    }
  }
  mUltimoValor = mValor;
}

void MoverServos(){
   if (PrimerLoop){
     PrimerLoop = false;
     InicializarAngulos();
  }
  int mDireccion;
  int mValorServo = 0;
  int mDiferencia = 0;
  for (int i = 1; i < 8; i++){
      mDiferencia = mMovimientosServos[i] - ValoresIniciales[i];
      if (2 > mDiferencia){
        mDireccion = -1;
      }
      else if (2 < mDiferencia){
        mDireccion = 1;
      }
      else{
        mDireccion = 0;
      }
      if (mDireccion!=0){
        if (ValoresIniciales[i]+mDireccion <= 180 || ValoresIniciales[i]+mDireccion >= 0)
        {
          ValoresIniciales[i] += mDireccion;
          Servos[i].write(ValoresIniciales[i]);
          
        }
      }
      mDireccion = 0;
  }
  delay(15); 
}

void InicializarAngulos(){
  Servos[0].write(ValoresIniciales[0]);
  delay(1000); 
  for (int i = 1; i < 8; i++){
    Servos[i].write(ValoresIniciales[i]);
  }
  delay(1000); 
}
