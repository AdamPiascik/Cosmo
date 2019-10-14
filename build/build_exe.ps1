$projectParentDir = "C:\Users\adamp\OneDrive - AVAMAE Software Solutions Ltd\Sandbox\RestfulTestTool"
$publishDir = "$projectParentDir\bin\publish"
$exeInPath = "RestfulTestTool.App.exe"
$exeOutPath = "$projectParentDir\bin\rtt.exe"
$projectToBuild = "$projectParentDir\src\RestfulTestTool.App\RestfulTestTool.App.csproj"

Get-ChildItem "$projectParentDir\bin" | Remove-Item -Force -Recurse

dotnet publish $projectToBuild -r win10-x64 -c Release -o $publishDir --self-contained false

.\warp-packer --arch windows-x64 --input_dir $publishDir --exec $exeInPath --output $exeOutPath


