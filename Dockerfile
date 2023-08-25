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
ENV ConnectionURI="mongodb://khaliphadb:yAlJljJcktmPPq6IlAmWsGcCuye6GLqY74XyEVKUvNJZJlg8F1ue0bGB3zPrk9LDit3kGheAs2UzACDb1IFewQ==@khaliphadb.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@khaliphadb@"
EXPOSE 3000
CMD [ "dotnet", "run" ]


