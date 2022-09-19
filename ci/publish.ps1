#!/usr/bin/env pwsh

$dockerRepoName="stephenwike";
$imagename="images-info-json-generator";
$version="1.0.0";

Push-Location ./src/ImageJsonGenerator
    docker push "${dockerRepoName}/${imagename}:${version}";
    if (! ($?)) {
        throw "failed to push image";
    }
    docker push "${dockerRepoName}/${imagename}:latest";
    if (! ($?)) {
        throw "failed to push image";
    }
   Pop-Location