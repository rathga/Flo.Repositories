# GoodToDo

GoodToDo is an example todo list app using ASP.Net Core 3.1 to implement a REST web api, and VueJS as a SPA client.

Both API and Client are set up to be run in containers.

## Features

- Register, Login and Logoff, with some basic validation
- Add, update, delete and check completed ToDo items in a Wunderlist/Microsoft ToDo style UI using VueJS
- Responsive bootstrap template
- Loading spinnys, and toast messages when commands execute sucessfully
- Friendly server error message if the API errors (hopefully not...) or the API server is down

## Working demo

Please visit http://goodtodo.uksouth.azurecontainer.io to see a working demo.

SwaggerUI is installed on the API if you wish to play with it directly: http://goodtodoapi.uksouth.azurecontainer.io/swagger

## How to deploy

The simplest way to deploy is to use the existing containers on Docker Hub:

- API: rathga/goodtodoapi:0.1
- Client: rathga/goodtodoclient:0.1

You will also need access to a mongodb server for the API to use as a database.  You can also run this in a container if desired ('mongo' on dockerhub).

### Environment Variables

Both API and Client use environment variables to find each other.

#### API

Set `GOODTODO_MONGODB` to the mongodb connection string.
Set `GOODTODO_CLIENTURL` to the url where the Client will be hosted (needed for CORS configuration).

#### Client

Set `VUE_APP_GOODTODO_APIURL` to the url where the API will be hosted.

### Example local docker commands
```
docker run --publish 27017:27017 --name mongodb --detach mongo
docker run --publish 8080:80 --name gtdapi --detach --env GOODTODO_MONGODB={hostname}:27017  --env GOODTODO_CLIENTURL={clienturl} rathga/goodtodoapi:0.1
docker run --publish 80:80 --name gtdclient --detach --env VUE_APP_GOODTODO_APIURL={apiurl} rathga/goodtodoclient:0.1
```

Replace `{hostname}`, `{clienturl}` and `{apiurl}` with the appropriate addresses and ports, but be careful (see Authentication Cookies below).


### Building custom images

If you want to make changes to the source and rebuild, the docker commands are:

API:

From the GoodToDo.Api directory:
```
docker build --tag goodtodoapi:custom -f Dockerfile ..
```

Client:

From the GoodToDo.ClientApp directory:
```
docker build --tag goodtodoclient:custom .
```

## Running in development or out of containers

The environment variables referred to above can be more easily configured in the appsettings.json file (for the API) and the .env file (for the client).

## Authentication cookies

Note that by default both API and Client use HTTP (not HTTPS).  Due to recent security changes in how browsers handle
cross site cookies, you must run both API and Client on the same major domain name (e.g. api.THESAME.com and client.THESAME.com)
otherwise the authentication cookie will not work.  Alternatively, you can configure at least the API to run on HTTPS, 
which will then allow browers to permit cross site cookies to an HTTP client.

If everything is running on localhost, this can cause problems as port differences can be interpreted as domain differences.  You may need to run the API in HTTPS to get around this (which visual studio can do just fine).