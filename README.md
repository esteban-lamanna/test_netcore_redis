# test_netcore_redis
Execute

docker-compose up --build -d

This command will start 3 containers: redis, sqlserver & restaurant.api

You can request to this endpoint 
 http://localhost:5000/product
 
The first request has a delay of 3 seconds. The second time you access it, it will return immediately, because it's cached.

To forget this cached data you can access 
 http://localhost:5000/product/invalidate/values