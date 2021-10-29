[![Build Status](https://travis-ci.org/aslabicki/Smarter-iKettle-API.svg?branch=master)](https://travis-ci.org/aslabicki/Smarter-iKettle-API)

# Smarter iKettle API

Smarter iKettle API is a web API based on .NET 5, which wrapped communication with kettle.
Using this API you can very easily control your kettle from any computer, phone or home automation platform.

## Requirements

The things you need to run this API is .NET 5 runtime, which you can download [here](https://dotnet.microsoft.com/download) or docker to run application in a container and connected kettle to your local network.

## Integrations

Smarter iKettle API can be integrated with every home automation solution because it uses web API.

<img align="left" height="100px" width="100px" src="https://user-images.githubusercontent.com/15276598/59797657-8f899980-92e0-11e9-9461-408a9468a341.png">

### Home Assistant
Sample configuration

```yaml
switch:
  platform: command_line
  switches: 
    ikettle:
      friendly_name: Smarter iKettle 2.0
      command_on: >-
        curl -d -X POST http://192.168.1.10:5021/api/v2/kettle/boil
      command_off: >-
        curl -d -X POST http://192.168.1.10:5021/api/v2/kettle/interrupt
      command_state: >-
        curl -sb -H "Accept: application/json" http://192.168.1.10:5021/api/v2/kettle/details
      value_template: >-
        {%- if value_json['status'] == 1 -%}
        {{ true }}
        {%- else -%}
        {{ false }}
        {%- endif -%}
```

## Configuration


Configure `KettleSettings` section in file `src\Smarter.iKettle.Api\appsettings.json`
```json
"KettleSettings": {
  "Host": "Kettle IP Address",
  "Port": 2081,
  "WaterSensorMax": 2250,
  "WaterSensorMin": 2080
}
```

- Host - Kettle IP address or hostname
- Port - Kettle port (default: 2081)
- WaterSensorMax - The maximum value that you could read from the water sensor when the kettle was full
- WaterSensorMin - The minimum value that you could read from the water sensor when the kettle was empty

If you run the application in docker container, then the easiest way to override those settings are provides environment variables during run container (e.g. -e "KettleSettings: Host=ip_address" -e "KettleSettings: Port=2081" -e "KettleSettings: WaterSensorMax=2250" -e "KettleSettings: WaterSensorMin=2080")

Note: You can read the water sensor value from `/api/v2/kettle/details` endpoint

## Usage

### Run locally

To run application go to `src\Smarter.iKettle.Api` folder and run command:

```sh
$ dotnet run .
```

### Run in docker

To run application in docker container run commands:

1. Pull image from Docker Hub

```sh
$ docker pull aslabicki/smarter-ikettle-api
```

  Or go to root folder and build image locally

```sh
$ docker build -t smarter-ikettle-api .
```

2. Start container
```sh
$ docker run -d --rm -p 5021:80 -e "KettleSettings__Host=ip_address" -e "KettleSettings__Port=2081" -e "KettleSettings__WaterSensorMax=2250" -e "KettleSettings__WaterSensorMin=2080" smarter-ikettle-api
```

## API

### /api/v2/kettle/details

#### GET
##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [Details](#details) |
| default |  | [ProblemDetails](#problemdetails) |
---
### /api/v2/kettle/boil

#### POST
##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 204 | Success |  |
| default |  | [ProblemDetails](#problemdetails) |
---
### /api/v2/kettle/heat

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| request | body |  | Yes | [HeatRequest](#heatrequest) |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 204 | Success |  |
| 400 | Bad Request | string |
| default |  | [ProblemDetails](#problemdetails) |
---
### /api/v2/kettle/heatformula

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| request | body |  | Yes | [HeatFormulaRequest](#heatformularequest) |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 204 | Success |  |
| 400 | Bad Request | string |
| default |  | [ProblemDetails](#problemdetails) |
---
### /api/v2/kettle/interrupt

#### POST
##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 204 | Success |  |
| default |  | [ProblemDetails](#problemdetails) |

### Models


#### Details

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| status | integer |  |  |
| temperature | integer |  |  |
| waterSensor | integer |  |  |
| waterPercent | double |  |  |
| onBase | boolean |  |  |
---
#### ProblemDetails

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| type | string |  |  |
| title | string |  |  |
| status | integer |  |  |
| detail | string |  |  |
| instance | string |  |  |
---
#### HeatRequest

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| temperature | integer |  | Yes |
| keepWarmMinutes | integer |  | No |

#### HeatFormulaRequest

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| temperature | integer |  | Yes |

Note: The application has a swagger installed to generate API documentation which is available under `/docs/index.html` endpoint

## Further development

 - Communication over MQTT
 - Implement other endpoints (e.g. get and set user default settings)
 - Unit tests 😅

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)
