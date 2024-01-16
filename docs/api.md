# Time Series Platform - API

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
