# test_netcore_redis
Execute

docker-compose up --build -d

This command will start 3 containers: redis, sqlserver & restaurant.api

You can request to this endpoint 
 [HTTPGET] http://localhost:5000/product
 
The first request has a delay of 3 seconds. The second time you access it, it will return immediately, because it's cached.

To forget this cached data you can access 
 [HTTPDELETE] http://localhost:5000/product/invalidate?key=values
 
 In docker-compose I have declared 3 volume mapping to my C:\EstebanSQL. This is needed to keep your database alive. Otherwise your data will banish everytime you remove your sql container.
 
 
 