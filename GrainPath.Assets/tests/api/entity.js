var t;

// 200 (OK), 

t = {
  "grainId": "64878b360b308ed470e2498a"
};

// bad request due to invalid MongoDB identifier.

t = {
  "grainId": "1"
};

// 404 (Not Found), non-existent identifier.

t = {
  "grainId": "64878b360b308ed470e24910"
};

// 
