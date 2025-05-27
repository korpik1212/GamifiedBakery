#include <HX711_ADC.h>
#if defined(ESP8266)|| defined(ESP32) || defined(AVR)
#include <EEPROM.h>
#endif
#include <TM1637Display.h>
#include <Arduino.h>
// Module connection pins (Digital Pins)
#define CLK 3
#define DIO 4
#define TEST_DELAY   2000

TM1637Display display(CLK, DIO);
//pins:
const int HX711_dout = 7; //mcu > HX711 dout pin
const int HX711_sck = 6; //mcu > HX711 sck pin

//HX711 constructor:
HX711_ADC LoadCell(HX711_dout, HX711_sck);

const int calVal_eepromAdress = 0;
unsigned long t = 0;
const int buttonPin = 2;  // the number of the pushbutton pin
const int ledPin = 12;    // the number of the LED pin

int loadCellValue=0;
// variables will change:
int buttonState = 0;  // variable for reading the pushbutton status
void setup() {
  // put your setup code here, to run once:
 hxSetup();
 displaySetup();
  pinMode(12, OUTPUT);
    pinMode(2, INPUT);

}

void buttonCheck(){
  int oldButtonState=buttonState;
    buttonState = digitalRead(2);
      if (buttonState == HIGH) {
    // turn LED on:
      Serial.println(loadCellValue);
      
  } else {
    // turn LED off:
  }
}

void displaySetup(){
display.setBrightness(0x0f); // Max brightness
  display.clear();
}

void hxSetup(){
  Serial.begin(9600); delay(10);
 // Serial.println();
 // Serial.println("Starting...");

  LoadCell.begin();
  //LoadCell.setReverseOutput(); //uncomment to turn a negative output value to positive
  float calibrationValue; // calibration value (see example file "Calibration.ino")
  calibrationValue = 696.0; // uncomment this if you want to set the calibration value in the sketch
#if defined(ESP8266)|| defined(ESP32)
  //EEPROM.begin(512); // uncomment this if you use ESP8266/ESP32 and want to fetch the calibration value from eeprom
#endif
  //EEPROM.get(calVal_eepromAdress, calibrationValue); // uncomment this if you want to fetch the calibration value from eeprom

  unsigned long stabilizingtime = 2000; // preciscion right after power-up can be improved by adding a few seconds of stabilizing time
  boolean _tare = true; //set this to false if you don't want tare to be performed in the next step
  LoadCell.start(stabilizingtime, _tare);
  if (LoadCell.getTareTimeoutFlag()) {
    Serial.println("Timeout, check MCU>HX711 wiring and pin designations");
    while (1);
  }
  else {
    LoadCell.setCalFactor(calibrationValue); // set calibration value (float)
  //  Serial.println("Startup is complete");
  }
}



void loop()
{
  printWeightValue();
  displayCurrentWeight();
  listenForLedChange();
  buttonCheck();

}

void displayCurrentWeight(){
displayNumber(loadCellValue);
}


void printWeightValue(){
  static boolean newDataReady = 0;
  const int serialPrintInterval = 0; //increase value to slow down serial print activity

  // check for new data/start next conversion:
  if (LoadCell.update()) newDataReady = true;

  // get smoothed value from the dataset:
  if (newDataReady) {
    if (millis() > t + serialPrintInterval) {
      float i = LoadCell.getData();
      //Serial.print("Load_cell output val: ");
      loadCellValue=i;
   
      newDataReady = 0;
      t = millis();
    }
  }

  // receive command from serial terminal, send 't' to initiate tare operation:
  if (Serial.available() > 0) {
    char inByte = Serial.read();
    if (inByte == 't') LoadCell.tareNoDelay();
  }

  // check if last tare operation is complete:
  if (LoadCell.getTareStatus() == true) {
   // Serial.println("Tare complete");
  }

}

void displayNumber(int number) {
  display.showNumberDec(number, true); // true = show leading zeros
}

void streamCurrentWeight(){
//triggers only when the button is pressed and streams the weight on 9600 port
}

void updateSegmentDisplay(){
  //read weight every second and display it on display
}

void listenForLedChange(){
 if (Serial.available() >= 2) { // Wait for 2 characters: 'L' + '0' or '1'
    char header = Serial.read(); // First character
    char value = Serial.read();  // Second character

    if (header == 'L' && (value == '0' || value == '1')) {
      int newState = value - '0'; // Convert char '0' or '1' to int 0 or 1
      changeLedState(newState);
    }
  }
}
void changeLedState(int newState){
if (newState == 1) {
    digitalWrite(ledPin, HIGH); // Turn LED on
  } else {
    digitalWrite(ledPin, LOW);  // Turn LED off
  }
}
//listen for "L"+ for led state
