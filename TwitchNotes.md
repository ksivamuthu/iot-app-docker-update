# Azure IoT Hub - Updating IoT Apps in a docker container

## Overview of the solution

1a. Create a sample IoT app in docker container (.Net core)
    * Serial Port communication (to demonstrate hardware communication)
    * Dashboard UI (to show some changes)
1b. Docker Devops ( Building Cross - ARM images from Azure Devops)

2a. Manage IoT app updates from Azure IoT Hub

Device 1: sivaiot.registry.io/iot-app:1
Device 2: sivaiot.registry.io/iot-app:2
2b. Device pulls the image from registry and doing restart once update is done

## April 15, 2019
### Planned:
1. Create a Azure IoT Hub app
2. Create a sample IoT app in docker container

### Done:
1. Created a sample Azure IoT Hub app
2. Created a sample IoT app using .NET Core

Docker is not building - or not reporting progress (hanging). Restart of the machine will work

## April 19, 2019
Docker commands - (port mapping didn't work) 
Then realized port mapping command is wrong

## April 22, 2019

Planned:
* IoT Hub is connected with .net core application
* Able to send message (Telemetry - Temperature, sensor data)
* Able to receive message (IoT Firmware update)
* Build this docker, CI/CD, push to azure container registry

## Done
1. Added IoT Device Client SDKs
2. Added telemetry temperature sensor
3. We are able to receive cloud messages on device

## Postponed
Build this docker, CI/CD, push to azure container registry

## April 24, 2019

Planned:
* Build this docker, CI/CD, 
* push to azure container registry
* Setup the watchtower
* Wireup the docker pull code from Azure IoT hub messages.

## Future
Planned:
Deploying this in raspberry device
Demo - Update using docker container