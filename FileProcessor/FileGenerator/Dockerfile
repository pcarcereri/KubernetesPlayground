﻿FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
ENV INPUT_FOLDER=./files

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY FileGenerator.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT dotnet FileGenerator.dll "$INPUT_FOLDER"