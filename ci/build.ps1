#!/usr/bin/env pwsh

$dockerRepoName="stephenwike";
$imagename="images-info-json-generator";
$version="1.0.0";

Push-Location ./src/ImageJsonGenerator
    dotnet publish -c "Production" -f netcoreapp3.1  -r linux-x64 -o out
    docker build -t "${dockerRepoName}/${imagename}:${version}" -t "${dockerRepoName}/${imagename}:latest" .
Pop-Location