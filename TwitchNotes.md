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

### Done
1. Added IoT Device Client SDKs
2. Added telemetry temperature sensor
3. We are able to receive cloud messages on device

### Postponed
Build this docker, CI/CD, push to azure container registry

## April 24, 2019

Done:
* Build this docker, CI/CD, 
* Push to azure container registry
* Setup the watchtower. But watchtower is not pulling changes from private registries

Postpone:
* Wireup the docker pull code from Azure IoT hub messages.

Notes:

1. Watch tower REPO_USER, REPO_PASS is not authenticated to pull images from private registry.
2. ASPNETCORE_ENVIRONMENT it should be production. Otherwise, even if you built release built release mode, it mess up in serving client files
3. Rockinoutt shared Terraform blogs -  https://blog.gruntwork.io/a-comprehensive-guide-to-terraform-b3d32832baca
4. ksivaraj - Joined our party. 
5. Cough pills

## April 29, 2019

Planning:
 
 - Watchtower automatic update when the new image is pushed to container registry
 - Disable the pull from watchtower
 - Implement docker api logic to pull the image.

 Outcomes:

 1. Whenever new demo changes are pushed in, immediately restart the iot app container
 2. User/Admin has control on demand update images.

Done:
 - Watchtower automatic update when the new image is pushed to container registry
 - Disable the pull from watchtower 
- Implement docker api logic to pull the image.

Pending:

Move the logical pointer to different versions. (Tag an image programmatically)

Notes:
1. Type_theory joined in our party
2. We discussed Indian food.
3. ML app - to detect different Indian cuisine.

In Theory: 

1. Disable watch tower pull
2. When Azure IoT hub is sending a message, 
    - Programmatically pull the docker image (specific version - Azure iot hub sends)
    - Watch tower refreshed the container, once the local docker image is pulled by our code.


## May 3, 2019

#### Plan

1. Move the logical pointer (local tag) when iot app update is triggerred

Notes:

## Future
Planned:
Deploying this in raspberry device
Demo - Update using docker container


1. IoT image: latest - Run the container
2. Watchtower - watch this iot image: local
3. IoT image: 67 - Pull
4. IoT image: latest

Watchtower - IotImage, local

Iot image - v1
Iot image - v2, local