# Time Series Platfrom

Time Series Platform, built with .NET and Timescale, is a simple web application to store and query time series data.

# Features

## Authentication

Authentication does not play a big role in the application, so the implementation is kinda minimal.

Time Series Platform contains three kinds of authentications:
- _Admin:_ Has access to EVERYTHING
- _Reader:_ Can only query data
- _Integration:_ For pushing data to the platform.

Authentication is handled with API keys stored in the following environment variables:
- `API_KEY_ADMIN`
- `API_KEY_READER`
- `API_KEY__<integration>` (yes, there's two underscores)
    - `<integration>` should match the integration slug stored in the database!

## Metadata Management

Time Series Platform contains a small metadata layer on top of the actual time series data.

Time Series Platform metadata consists of two concepts:
- _Integration:_ Time series data source
    - Integrations should be manually configured to enable data ingestion.
- _Tag:_ Measurement identifier

Metadata can also be queried from the API, which is then used to query the actual measurements.

## Data Ingestion

Time Series Platform data ingestion is handled with just a simple HTTP POST endpoint, which shifts the responsibility of reliability to the sender.

Data ingestion requires the sender to include an integration API key with the data, which is then used to identify the data source.

See the [measurement endpoint](#measurement) for instructions.

## Measurements

TODO

# API

API key header: `api-key`

## `/integration`

### GET

TODO

## `/tag`

### GET

TODO

## `/measurement`

### POST

Request body:
```json
[
    {
        "tag": "example-tag-slug",
        "data": [
            {
                "value": 1.23,
                "timestamp": "2023-12-03T09:19:00.000Z"
            }
        ]
    }
]
```
