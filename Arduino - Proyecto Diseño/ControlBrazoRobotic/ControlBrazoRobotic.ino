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

void setup() 
{ 
  myservo.attach(9);  // attaches the servo on pin 9 to the servo object 
} 
 
void loop() 
{ 
  CambiarAngulo(9,0,180);
  CambiarAngulo(9,180,0);
  
} 

void CambiarAngulo (int pPin,int pGradosInicial,int pGradosFinal){
 int mSigno = pGradosFinal - pGradosInicial;
 myservo.attach(pPin);
 if (mSigno > 0) {
   for(pos = pGradosInicial; pos<pGradosFinal; pos+=1)     // goes from 180 degrees to 0 degrees 
   {                                
    myservo.write(pos);              // tell servo to go to position in variable 'pos' 
    delay(15);                       // waits 15ms for the servo to reach the position 
  } 
 }
 else if (mSigno < 0) {
   for(pos = pGradosInicial; pos>=pGradosFinal; pos-=1)     // goes from 180 degrees to 0 degrees 
   {                                
    myservo.write(pos);              // tell servo to go to position in variable 'pos' 
    delay(15);                       // waits 15ms for the servo to reach the position 
  } 
 }
}
