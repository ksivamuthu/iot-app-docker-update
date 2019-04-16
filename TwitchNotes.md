# Azure IoT Hub - Updating IoT Apps in a docker container

## Overview of the solution

1a. Create a sample IoT app in docker container (NodeJS or .net core)
    * Serial Port communication (to demonstrate hardware communication)
    * Dashboard UI (to show some changes)
1b. Docker Devops ( Building Cross - ARM images from Azure Devops)

2a. Manage IoT app updates from Azure IoT Hub

Device 1: sivaiot.registry.io/iot-app:1
Device 2: sivaiot.registry.io/iot-app:2
2b. Device pulls the image from registry and doing restart once update is done

## April 15, 2019

1. Create a Azure IoT Hub app
2. Create a sample IoT app in docker container
