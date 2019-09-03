# AccessPong-BE
Back end repository to AccessPong.

## Endpoints

### Home

GET [/api] (WIP)

Returns a json object with all possible endpoints.

### Get Fixtures

```
GET [/api/fixtures]

Output:
{
    "fixtures": [
        {
            "Id": 1,
            "FixtureId": 1,
            "PlayerOneId": 1,
            "PlayerTwoId": 2,
            "WinnerId": 1
        },
        {
            "Id": 2,
            "FixtureId": 2,
            "PlayerOneId": 2,
            "PlayerTwoId": 1,
            "WinnerId": -1
        },
    ]
}
```

Returns a json object with all the games that have and are yet to be played.

### Next Fixture

```
GET [/api/fixtures/next]

Output:
{
    "Id": 2,
    "FixtureId": 2,
    "PlayerOneId": 2,
    "PlayerTwoId": 3,
    "WinnerId": -1
}
```

Returns a json object with the next game to be played.

### Update Fixture

```
POST [/api/fixtures/update]

Input:
{
    "FixtureId": 1,
    "WinnerId": 2
}

Output:
{
    "Id": 2,
    "FixtureId": 2,
    "PlayerOneId": 5,
    "PlayerTwoId": 3,
    "WinnerId": 3
}
```

Takes a json object containing the fixture and winner ids and returns the update fixture. Returns a json object with the next game to be played.
