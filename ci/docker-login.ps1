#!/usr/bin/env pwsh

# 1. immediate failure if the login fails
# 2. all original response messages are passed through from Docker unchanged
# 3. password is not passed on bash shell command line


# for this script, set preference to continue since we are handling all the errors below
$ErrorActionPreference = 'continue'

# build a scriptblock and pipe apikey into input of docker login
$script = { docker login -u $env:DOCKER_REPO -p $env:DOCKER_PASSWORD }

# run the scriptblock and merge stderr into output, capturing the output pipeline in $result
$result = (& $script 2>&1) 
Write-Host $result

# result is a pipeline - enumerate objects on the pipeline and look for docker bad credentials message
$result | ForEach-Object {
	$msg = $_.ToString()
	if ($msg.Contains("BAD_CREDENTIAL"))
	{
		$ErrorActionPreference = 'stop'
		Write-Error "Aborting build because Docker credentials are invalid"
	}
}