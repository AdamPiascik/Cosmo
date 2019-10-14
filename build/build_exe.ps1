$projectParentDir = "C:\Users\adamp\OneDrive - AVAMAE Software Solutions Ltd\Sandbox\RestfulTestTool"
$exeInPath = "$projectParentDir\bin\RestfulTestTool.App.exe"
$exeOutPath = "$projectParentDir\bin\rtt.exe"
$publishDir = "$projectParentDir\bin\publish"
$projectToBuild = "$projectParentDir\src\RestfulTestTool.App\RestfulTestTool.App.csproj"

gci "$projectParentDir\bin" | rm -Force

dotnet publish $projectToBuild -r win10-x64 -c Release -o $publishDir --self-contained false

.\warp-packer --arch windows-x64 --input_dir $publishDir --exec $exeInPath --output $exeOutPath


