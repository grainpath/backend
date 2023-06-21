var t;

// 200 (OK), three distinct points

t = {
  "waypoints": [
    {
      "lon": 14.4035264,
      "lat": 50.0884344
    },
    {
      "lon": 14.4088950,
      "lat": 50.0914358
    },
    {
      "lon": 14.4057219,
      "lat": 50.0919964
    }
  ]
};

// 200 (OK), the starting point and destination are the same.

t = {
  "waypoints": [
    {
      "lon": 14.4035264,
      "lat": 50.0884344
    },
    {
      "lon": 14.4035264,
      "lat": 50.0884344
    }
  ]
};

// 200 (OK), two middle points are the same.

t = {
  "waypoints": [
    {
      "lon": 14.4035264,
      "lat": 50.0884344
    },
    {
      "lon": 14.4088950,
      "lat": 50.0914358
    },
    {
      "lon": 14.4088950,
      "lat": 50.0914358
    },
    {
      "lon": 14.4057219,
      "lat": 50.0919964
    }
  ]
};

// 400 (Bad Request), the length of "waypoints" < 2

t = {
  "waypoints": [
    {
      "lon": 14.4035264,
      "lat": 50.0884344
    }
  ]
};
