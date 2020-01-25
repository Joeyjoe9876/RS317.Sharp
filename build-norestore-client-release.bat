dotnet publish src/Client/Rs317.Client.Unity/Rs317.Client.Unity.csproj -c Release
dotnet publish src/Client/Rs317.Client.Unity.GladMMO/Rs317.Client.Unity.GladMMO.csproj -c Release

if not exist "build\client\release\unity" mkdir build\client\release\unity"
if not exist "build\client\release\\gladmmo" mkdir build\client\release\gladmmo"

xcopy src\Client\Rs317.Client.Unity\bin\Release\netstandard2.0\publish build\client\release\unity /Y /q /EXCLUDE:BuildExclude.txt
xcopy src\Client\Rs317.Client.Unity.GladMMO\bin\Release\netstandard2.0\publish build\client\release\gladmmo /Y /q /EXCLUDE:BuildExclude.txt