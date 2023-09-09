remove-item Build\*

dotnet publish -c Release -r linux-arm -p:PublishSingleFile=true --self-contained false
dotnet publish -c Release -r linux-arm64 -p:PublishSingleFile=true --self-contained false
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained false
dotnet publish -c Release -r linux-musl-x64 -p:PublishSingleFile=true --self-contained false

dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained false
dotnet publish -c Release -r linux-musl-x64 -p:PublishSingleFile=true --self-contained false

dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false
dotnet publish -c Release -r win-x86 -p:PublishSingleFile=true --self-contained false


wsl tar -czf 'Build/recli-linux-arm.gz' --directory './bin/Release/net7.0/linux-arm/publish/' recli
wsl tar -czf 'Build/recli-linux-arm64.gz' --directory  './bin/Release/net7.0/linux-arm64/publish/' recli
wsl tar -czf 'Build/recli-linux-musl-x64.gz' --directory  './bin/Release/net7.0/linux-musl-x64/publish/' recli
wsl tar -czf 'Build/recli-linux-x64.gz' --directory  './bin/Release/net7.0/linux-x64/publish/' recli
wsl zip -j "Build/recli-win-x64" "./bin/Release/net7.0/win-x64/publish/recli.exe"
wsl zip -j "Build/recli-win-x86" "./bin/Release/net7.0/win-x86/publish/recli.exe"