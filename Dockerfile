FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# COPYING FILES
COPY /MinimalAPIDemo/MinimalAPIDemo.Test.csproj ./MinimalAPIDemo/MinimalAPIDemo.Test.csproj
COPY /MinimalAPIDemo/MinimalAPIDemo.csproj ./MinimalAPIDemo/MinimalAPIDemo.csproj

RUN ls -l
