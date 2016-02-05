$msBuildCmd = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
$solution =  "Lucy.sln"
$destination = 'deploy_tmp'
$nsisScript ='deploy\lucy_alpha.nsi'

& $msBuildCmd  @($solution,  "/t:Rebuild", "/p:Configuration=Release" , "/consoleloggerparameters:summary")

if ( (Test-Path $destination)  -eq $false){
    New-Item $destination -ItemType Directory
}
else
{ 
   ls $destination -Recurse |  Remove-Item   
}

ls  "Lucy.Client.Desktop\bin\Release\" -Recurse -Exclude "*.pdb" | % { Copy-Item $_.FullName  $destination }

& 'C:\Program Files (x86)\NSIS\makensis.exe' @($nsisScript)