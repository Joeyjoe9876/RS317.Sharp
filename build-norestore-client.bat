dotnet publish src/Client/Rs317.Client.Unity/Rs317.Client.Unity.csproj -c Debug
dotnet publish src/Client/Rs317.Client.Unity.GladMMO/Rs317.Client.Unity.GladMMO.csproj -c Debug

if not exist "build\client\unity" mkdir build\client\unity"
if not exist "build\client\gladmmo" mkdir build\client\gladmmo"

xcopy src\Client\Rs317.Client.Unity\bin\Debug\netstandard2.0\publish build\client\unity /Y /q /EXCLUDE:BuildExclude.txt
xcopy src\Client\Rs317.Client.Unity.GladMMO\bin\Debug\netstandard2.0\publish build\client\gladmmo /Y /q /EXCLUDE:BuildExclude.txt