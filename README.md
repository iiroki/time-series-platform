# Time Series Platfrom

Time Series Platform, built with .NET and Timescale, is a simple web application to store and query time series data.

The project is mainly done for learning purposes and the main thing about this project is to experiment with different technologies and see how they integrate with each other.

Requirements for the project, as simple as they currently are, come from real-life scenarios,
as the is also meant to be used in production (at least by me). 

See [**"Tech Stack"**](#tech-stack) and [**"Features"**](#features) below for more information about the used technologies.

# Tech Stack

- **Framework:**
    - .NET 8
    - ASP.NET Core Web API
- **Database:**
    - [Timescale](https://www.timescale.com/) for time series data using Postgres
    - [EF Core](https://learn.microsoft.com/en-us/ef/core/) for schema definition and migrations
    - [Npgsql](https://www.npgsql.org/) as the Postgres provider for .NET.
- **Notifications:**
    - (TODO) WebSocket for pushing changes to clients.

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
- `API_KEY_INTEGRATION__<integration>` (yes, there's two underscores)
    - `<integration>` should match the integration slug stored in the database!

## Metadata Management

Time Series Platform contains a small metadata layer on top of the actual time series data.

Time Series Platform metadata consists of two concepts:
- _Integration:_ Time series data source
    - Integrations should be manually configured to enable data ingestion.
- _Tag:_ Measurement identifier

By querying metadata from the API, users can get an overview of the time series data stored in the plaftorm, which can then be utilized to query the actual measurements.

Metadata is stored in regular Postgres tables.

## Data Ingestion

Time Series Platform data ingestion is handled with just a simple HTTP POST endpoint,
which shifts the responsibility of reliability to the sender.
This design is by choice to keep things simple in the platform's end,
but the data ingestion implementation might be extended in future...

Data ingestion requires the sender to include an integration API key with the data,
which is then used to identify the data source.

See [**`/measurement`**](#measurement) for instructions.

Measurements are stored in Timescale hypertables, which have references to the metadata tables.

## Notifications

TODO

## OpenAPI

Time Series Platform also contains a comprehensive OpenAPI documentation implemented with Swagger.

As the authentication is as simple as it is, the Swagger page is also a handy tool to test the API.

# API

**API key header:** `X-API-KEY`

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
