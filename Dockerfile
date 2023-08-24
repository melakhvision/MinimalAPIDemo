FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
RUN mkdir MinimalAPIDemo && mkdir MinimalAPIDemo.Test

# COPYING FILES
COPY MinimalAPIDemo.sln .
WORKDIR /MinimalAPIDemo
COPY /MinimalAPIDemo/MinimalAPIDemo.csproj .
RUN dotnet restore
WORKDIR /MinimalAPIDemo.Test
COPY /MinimalAPIDemo.Test/MinimalAPIDemo.Test.csproj .
RUN dotnet restore

RUN ls -ls
RUN ls -ls /MinimalAPIDemo

## install packages
WORKDIR /app
# RUN dotnet restore
COPY . .
WORKDIR /app/MinimalAPIDemo
ENV PORT=3000
ENV ConnectionURI=
EXPOSE 3000
CMD [ "dotnet", "run" ]


