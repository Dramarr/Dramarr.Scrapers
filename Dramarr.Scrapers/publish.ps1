if ((Test-Path "bin")) {
	rm -r bin
}

dotnet build -c Release
dotnet pack -c Release
dotnet nuget push bin\Release\*.nupkg -s "https://nuget.org"


