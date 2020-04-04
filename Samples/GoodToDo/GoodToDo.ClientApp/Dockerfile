#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM node:lts-alpine

# install simple http server for serving static content
RUN npm install -g pushstate-server

# make the 'app' folder the current working directory
WORKDIR /app

# copy both 'package.json' and 'package-lock.json' (if available)
COPY package*.json ./

# install project dependencies
RUN npm install

# copy project files and folders to the current working directory (i.e. 'app' folder)
COPY . .

# build app for production with minification
RUN npm run build

EXPOSE 80

RUN dos2unix start.sh

RUN chmod +x start.sh

RUN apk add gettext

ENTRYPOINT ./start.sh