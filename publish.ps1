if (!(Test-Path ".nuget\nuget.exe")) {
	New-Item -ItemType directory -Path .nuget
	
	$MyFolder=get-item .\.nuget -Force
	$MyFolder.attributes="Hidden"
	
	Invoke-WebRequest -OutFile .nuget\nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
}

if (!(Test-Path "bin\Release")) {
	Remove-Item bin\Release\*.nupkg
}

dotnet build -c Release

dotnet pack -c Release

.nuget\nuget push bin\Release\*.nupkg -Source "https://nuget.org"

Remove-Item bin\Release\*.nupkg

