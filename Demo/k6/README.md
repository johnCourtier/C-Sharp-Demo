# How to run K6 test

Make sure webservice is actually running.
Notice that there is no deleting of documents - webservice needs to be restarted to execute writing test again

docker compose build k6
docker compose run --rm k6 run /k6/reading.js
docker compose run --rm k6 run /k6/writing.js
docker compose run --rm k6 run /k6/updating.js