$projectParentDir = "C:\Users\adamp\OneDrive - AVAMAE Software Solutions Ltd\Sandbox\RestfulTestTool"
$publishDir = "$projectParentDir\bin\publish"
$exeInPath = "RestfulTestTool.App.exe"
$exeOutPath = "$projectParentDir\bin\rtt.exe"
$projectToBuild = "$projectParentDir\src\RestfulTestTool.App\RestfulTestTool.App.csproj"
$buildScriptsDir = "$projectParentDir\build"

Get-ChildItem "$projectParentDir\bin" | Remove-Item -Force -Recurse

dotnet publish $projectToBuild -r win10-x64 -c Release -o $publishDir --self-contained false

& "$buildScriptsDir\warp-packer" --arch windows-x64 --input_dir $publishDir --exec $exeInPath --output $exeOutPath

Copy-Item "$buildScriptsDir\rtt.config.json" "$projectParentDir\bin"


