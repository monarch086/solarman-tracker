Write-Output "Packaging services...";

Set-Location ./TrackingLambda
dotnet restore
dotnet lambda package --configuration Release --framework net8.0 --output-package bin/Release/net8.0/deploy-tracking-lambda.zip
Write-Output ">>> Finished packaging TrackingLambda";
Set-Location ..

Write-Output ">>> >>> >>> Finished all services.";