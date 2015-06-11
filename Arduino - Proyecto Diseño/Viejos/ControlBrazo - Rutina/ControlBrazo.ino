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
String Nombres[] = {"base2", "base1", "hombro", "codo","munecaa", "munecab", "munecac","pinza"};
int Pines[] = {3, 4, 5, 6, 12, 11, 10, 9};
int ValoresIniciales[] = {90,0,80,150,90,90,90,90};
Servo Servos[8];

void setup() 
{ 
  for (int i = 0; i < 7; i++){
    Servos[i].attach(Pines[i]);
    //Servos[i].write(ValoresIniciales[i]); 
    //CambiarAngulo(Pines[i],0,ValoresIniciales[i]);
    //delay(15);  
  }
} 
 
void loop() 
{ 
  CambiarPosicionBase(90);
  CambiarAngulo(Servos[2],0,80);
  CambiarAngulo(Servos[3],0,150);
  CambiarAngulo(Servos[4],0,90);
  CambiarAngulo(Servos[5],0,90);
  CambiarAngulo(Servos[6],0,90);
  CambiarAngulo(Servos[7],0,90);
  CambiarPosicionBase(40);
  CambiarAngulo(Servos[2],0,60);
  CambiarAngulo(Servos[3],0,100);
  CambiarAngulo(Servos[4],0,0);
  CambiarAngulo(Servos[5],0,0);
  CambiarAngulo(Servos[6],0,0);
  CambiarAngulo(Servos[7],0,0);
  //CambiarAngulo(9,90,0);
  
} 

void InicializarAngulos(){
  
}

void CambiarAngulo (Servo pServo,int pGradosInicial,int pGradosFinal){
 pGradosInicial = pServo.read();
 int mSigno = pGradosFinal - pGradosInicial;
 if (mSigno > 3) {
   for(pos = pGradosInicial; pos<pGradosFinal; pos+=1)     // goes from 180 degrees to 0 degrees 
   {                                
    pServo.write(pos);              // tell servo to go to position in variable 'pos' 
    delay(30);                       // waits 15ms for the servo to reach the position 
  } 
 }
 else if (mSigno < -3) {
   for(pos = pGradosInicial; pos>=pGradosFinal; pos-=1)     // goes from 180 degrees to 0 degrees 
   {                                
    pServo.write(pos);              // tell servo to go to position in variable 'pos' 
    delay(30);                       // waits 15ms for the servo to reach the position 
  } 
 }
}

void CambiarPosicionBase(int pGradosFinal){
 int pGradosInicial = Servos[0].read();
 int mSigno = pGradosFinal - pGradosInicial;
 if (mSigno > 3) {
   for(pos = pGradosInicial; pos<pGradosFinal; pos+=1)     // goes from 180 degrees to 0 degrees 
   {                                
    //Servos[0].write(pos);              // tell servo to go to position in variable 'pos' 
    Servos[0].write(pos);
    delay(30);                       // waits 15ms for the servo to reach the position 
  } 
 }
 else if (mSigno < -3) {
   for(pos = pGradosInicial; pos>pGradosFinal; pos-=1)     // goes from 180 degrees to 0 degrees 
   {                                
    //Servos[0].write(pos);              // tell servo to go to position in variable 'pos'               // tell servo to go to position in variable 'pos' 
    Servos[0].write(pos);
    delay(30);                       // waits 15ms for the servo to reach the position 
  } 
 }
}
