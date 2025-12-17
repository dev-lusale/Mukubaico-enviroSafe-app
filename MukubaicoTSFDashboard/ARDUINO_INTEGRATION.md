# Arduino Hardware Integration Guide

## Overview
This dashboard is designed to integrate with Arduino sensors for real-time monitoring of TSF (Tailings Storage Facility) compliance parameters.

## Hardware Requirements

### Arduino Board
- Arduino Uno/Mega 2560 (recommended)
- USB cable for connection
- Power supply (9V adapter or USB)

### Sensors Required

#### Water Quality Sensors
1. **pH Sensor** (Analog pH Meter Kit)
   - Measures: pH levels (0-14)
   - Connection: Analog Pin A0
   - ZEMA Compliance

2. **Turbidity Sensor** (TSD-10)
   - Measures: Water clarity (NTU)
   - Connection: Analog Pin A1
   - ZEMA Compliance

3. **TSS Sensor** (Total Suspended Solids)
   - Measures: Particle concentration (mg/L)
   - Connection: Analog Pin A2
   - IFC EHS Compliance

#### Structural Monitoring Sensors
4. **Inclinometer** (ADXL345 or MPU6050)
   - Measures: Tilt/deformation (degrees/mm)
   - Connection: I2C (SDA, SCL)
   - ICOLD Compliance

5. **Pressure Sensor** (BMP180/BMP280)
   - Measures: Pore pressure (kPa)
   - Connection: I2C (SDA, SCL)
   - ICOLD Compliance

6. **Flow Meter** (YF-S201)
   - Measures: Seepage flow rate (L/s)
   - Connection: Digital Pin 2
   - ICOLD Compliance

#### Air Quality Sensors
7. **PM10/PM2.5 Sensor** (PMS5003 or SDS011)
   - Measures: Particulate matter (μg/m³)
   - Connection: Serial (RX/TX)
   - IFC EHS Compliance

8. **Temperature/Humidity Sensor** (DHT22)
   - Measures: Temperature (°C), Humidity (%)
   - Connection: Digital Pin 4
   - Environmental Monitoring

#### Groundwater Monitoring
9. **Ultrasonic Distance Sensor** (HC-SR04)
   - Measures: Water level in wells (cm)
   - Connection: Digital Pins 5, 6
   - ZEMA Compliance

## Arduino Code Example

```cpp
// TSF Monitoring System - Arduino Code
// Mukubaico Enviro-Safe

#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_ADXL345_U.h>
#include <DHT.h>

// Pin Definitions
#define PH_PIN A0
#define TURBIDITY_PIN A1
#define TSS_PIN A2
#define DHT_PIN 4
#define FLOW_PIN 2
#define TRIG_PIN 5
#define ECHO_PIN 6

// Sensor Objects
DHT dht(DHT_PIN, DHT22);
Adafruit_ADXL345_Unified accel = Adafruit_ADXL345_Unified(12345);

// Variables
volatile int flowPulseCount = 0;
float flowRate = 0.0;
unsigned long oldTime = 0;

void setup() {
  Serial.begin(9600);
  
  // Initialize sensors
  dht.begin();
  accel.begin();
  
  pinMode(FLOW_PIN, INPUT);
  pinMode(TRIG_PIN, OUTPUT);
  pinMode(ECHO_PIN, INPUT);
  
  attachInterrupt(digitalPinToInterrupt(FLOW_PIN), flowPulseCounter, FALLING);
  
  Serial.println("TSF Monitoring System Initialized");
}

void loop() {
  // Read pH Sensor
  float phValue = readPH();
  sendData("PH-01", "pH", phValue, "pH");
  
  // Read Turbidity
  float turbidity = readTurbidity();
  sendData("TURB-01", "Turbidity", turbidity, "NTU");
  
  // Read TSS
  float tss = readTSS();
  sendData("TSS-01", "TSS", tss, "mg/L");
  
  // Read Inclinometer
  sensors_event_t event;
  accel.getEvent(&event);
  float tilt = calculateTilt(event.acceleration.x, event.acceleration.y, event.acceleration.z);
  sendData("INCL-01", "Inclinometer", tilt, "mm");
  
  // Read Flow Rate
  calculateFlowRate();
  sendData("SEEP-01", "Seepage", flowRate, "L/s");
  
  // Read Temperature
  float temp = dht.readTemperature();
  sendData("TEMP-01", "Temperature", temp, "°C");
  
  // Read Water Level
  float waterLevel = readWaterLevel();
  sendData("GW-01", "Water Level", waterLevel, "m");
  
  delay(5000); // Send data every 5 seconds
}

float readPH() {
  int sensorValue = analogRead(PH_PIN);
  float voltage = sensorValue * (5.0 / 1024.0);
  float phValue = 3.5 * voltage + 0.0; // Calibration formula
  return phValue;
}

float readTurbidity() {
  int sensorValue = analogRead(TURBIDITY_PIN);
  float voltage = sensorValue * (5.0 / 1024.0);
  float turbidity = -1120.4 * voltage * voltage + 5742.3 * voltage - 4352.9;
  return constrain(turbidity, 0, 3000);
}

float readTSS() {
  int sensorValue = analogRead(TSS_PIN);
  float voltage = sensorValue * (5.0 / 1024.0);
  float tss = voltage * 50.0; // Calibration formula
  return tss;
}

float calculateTilt(float x, float y, float z) {
  float pitch = atan2(x, sqrt(y*y + z*z)) * 180.0 / PI;
  float roll = atan2(y, sqrt(x*x + z*z)) * 180.0 / PI;
  return sqrt(pitch*pitch + roll*roll) * 0.1; // Convert to mm
}

void calculateFlowRate() {
  if((millis() - oldTime) > 1000) {
    detachInterrupt(digitalPinToInterrupt(FLOW_PIN));
    flowRate = ((1000.0 / (millis() - oldTime)) * flowPulseCount) / 7.5;
    oldTime = millis();
    flowPulseCount = 0;
    attachInterrupt(digitalPinToInterrupt(FLOW_PIN), flowPulseCounter, FALLING);
  }
}

void flowPulseCounter() {
  flowPulseCount++;
}

float readWaterLevel() {
  digitalWrite(TRIG_PIN, LOW);
  delayMicroseconds(2);
  digitalWrite(TRIG_PIN, HIGH);
  delayMicroseconds(10);
  digitalWrite(TRIG_PIN, LOW);
  
  long duration = pulseIn(ECHO_PIN, HIGH);
  float distance = duration * 0.034 / 2;
  float waterLevel = (200 - distance) / 100.0; // Convert to meters
  return waterLevel;
}

void sendData(String sensorId, String type, float value, String unit) {
  Serial.print(sensorId);
  Serial.print(",");
  Serial.print(type);
  Serial.print(",");
  Serial.print(value, 2);
  Serial.print(",");
  Serial.println(unit);
}
```

## Connection to Dashboard

### Serial Communication Setup

1. **Connect Arduino to PC**
   - Use USB cable
   - Note the COM port (e.g., COM3, COM4)

2. **Configure Dashboard**
   - Open `Services/ArduinoService.cs`
   - Update port name: `ConnectAsync("COM3", 9600)`
   - Set `_isSimulationMode = false` for real hardware

3. **Data Format**
   - Arduino sends: `SENSOR_ID,TYPE,VALUE,UNIT`
   - Example: `PH-01,pH,7.2,pH`

### Testing Connection

```csharp
// In your dashboard code
var arduinoService = App.ArduinoService;
await arduinoService.ConnectAsync("COM3", 9600);

if (arduinoService.IsConnected)
{
    Console.WriteLine("Arduino connected successfully!");
}
```

## Sensor Calibration

### pH Sensor
1. Use pH 4.0, 7.0, and 10.0 buffer solutions
2. Adjust calibration formula in Arduino code
3. Verify readings against standard meter

### Turbidity Sensor
1. Use formazin standards (0, 100, 400, 1000 NTU)
2. Create calibration curve
3. Update formula in code

### Flow Meter
1. Measure actual flow with graduated cylinder
2. Count pulses per liter
3. Adjust conversion factor

## Wiring Diagram

```
Arduino Mega 2560
┌─────────────────┐
│                 │
│  A0 ─── pH      │
│  A1 ─── Turb    │
│  A2 ─── TSS     │
│  D2 ─── Flow    │
│  D4 ─── DHT22   │
│  D5 ─── Trig    │
│  D6 ─── Echo    │
│  SDA ─── I2C    │
│  SCL ─── I2C    │
│  GND ─── Common │
│  5V ─── Power   │
└─────────────────┘
```

## Troubleshooting

### Arduino Not Detected
- Check USB cable connection
- Verify COM port in Device Manager
- Install CH340 drivers if needed

### No Data Received
- Check baud rate (9600)
- Verify serial port name
- Test with Arduino Serial Monitor first

### Incorrect Readings
- Calibrate sensors
- Check sensor connections
- Verify power supply voltage

## Compliance Mapping

| Sensor | Standard | Parameter |
|--------|----------|-----------|
| pH Sensor | ZEMA | Water Quality |
| Turbidity | ZEMA | Water Quality |
| TSS | IFC EHS | Discharge Quality |
| Inclinometer | ICOLD | Structural Integrity |
| Pressure | ICOLD | Pore Pressure |
| Flow Meter | ICOLD | Seepage Control |
| PM Sensor | IFC EHS | Air Quality |
| Water Level | ZEMA | Groundwater |

## Safety Considerations

1. **Electrical Safety**
   - Use proper insulation
   - Avoid water contact with electronics
   - Use waterproof enclosures

2. **Sensor Placement**
   - Follow safety protocols
   - Maintain safe distances
   - Use proper mounting

3. **Data Validation**
   - Implement range checks
   - Cross-verify with manual readings
   - Set up alerts for anomalies

## Support

For technical support:
- Email: support@mukubaico.com
- Phone: +260-XXX-XXXX
- Documentation: https://docs.mukubaico.com/arduino

---
**Last Updated**: December 2025  
**Version**: 1.0.0
