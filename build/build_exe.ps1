$projectParentDir = "C:\Users\adamp\OneDrive - AVAMAE Software Solutions Ltd\Sandbox\Cosmo"
$publishDir = "$projectParentDir\bin\publish"
$exeInPath = "Cosmo.App.exe"
$exeOutPath = "$projectParentDir\bin\cosmo.exe"
$projectToBuild = "$projectParentDir\src\Cosmo.App\Cosmo.App.csproj"
$buildScriptsDir = "$projectParentDir\build"

Get-ChildItem "$projectParentDir\bin" | Remove-Item -Force -Recurse

dotnet publish $projectToBuild -r win10-x64 -c Release -o $publishDir --self-contained false

& "$buildScriptsDir\warp-packer" --arch windows-x64 --input_dir $publishDir --exec $exeInPath --output $exeOutPath

Copy-Item "$buildScriptsDir\cosmo.config.json" "$projectParentDir\bin"
Copy-Item "$buildScriptsDir\HorizonLeeds.TestData.json" "$projectParentDir\bin"


