Remove-Item packages\*.*
dotnet pack -o packages
dotnet nuget push packages\*.nupkg --source "Local Packages"