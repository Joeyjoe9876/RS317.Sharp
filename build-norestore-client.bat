dotnet publish src/Client/Rs317.Client.Unity/Rs317.Client.Unity.csproj -c Debug

if not exist "build\client\unity" mkdir build\client\unity"

xcopy src\Client\Rs317.Client.Unity\bin\Debug\netstandard2.0\publish build\client\unity /Y /q /EXCLUDE:BuildExclude.txt