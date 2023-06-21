var t;

// 200 (OK), standard application request

t = {
  "count": 3,
  "prefix": "mus"
};

// 400 (Bad Request), both "count" and "prefix" are missing.

t = {
};

// 400 (Bad Request), "count" < 1.

t = {
  "count": 0,
  "prefix": "mus"
};
