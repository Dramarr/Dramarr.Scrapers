:: make sure we have a clean release build
dotnet build -c Release

:: remove any existing nupkg files
del bin\Release\*.nupkg

:: build the nuget packages
dotnet pack -c Release

:: upload the nuget packages
:: .nuget\nuget push *.nupkg -Source "https://nuget.org"

:: remove nupkg files after uploading them
del bin\Release\*.nupkg