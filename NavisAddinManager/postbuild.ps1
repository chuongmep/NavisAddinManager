param ($Configuration, $ProjectDir, $SolutionDir,$Version)
Write-Host "Solution Directory:" $SolutionDir
Write-Host "Project Directory:" $ProjectDir
Write-Host "Configuration Name Current:" $Configuration
Write-Host "Autodesk Product Version:" $Version
$bundle = "C:\ProgramData\Autodesk\ApplicationPlugins\NavisAddinManager.bundle\"
$content = "PackageContents.xml"
$listExeclude = @("Autodesk.Navisworks.Api.dll","navisworks.gui.roamer.dll","AdWindows.dll")
$Resouces = $bundle+"Contents\"+"$Version\"
$projectName = "NavisAddinManager"
$AutodeskProduct = "Navisworks"
$AutodeskProcessName = "Roamer"

# use to write log console output in log file
function WriteConsole([string]$name)
{
	Write-Host "╬════════════════════"
	Write-Host "║ $name"
	Write-Host "╬════════════════════"
}
#check process and kill
function KillProcessNavis()
{
	WriteConsole("Kill Process $AutodeskProduct")
	$proc = Get-Process $AutodeskProcessName -ErrorAction SilentlyContinue
	if($proc){
		Stop-Process -InputObject $proc
		Write-Host "Stopping  Process $AutodeskProcessName"
		Start-Sleep -s 5
	}
	else{
		Write-Host "Process $AutodeskProduct  is not running"
	}
}
# check folder and create folder
function CopyAssembly(){
	WriteConsole("Start Create Bundle Package")
	if(Test-Path $Resouces){
		Remove-Item ($Resouces) -Recurse
		Write-Host "Removed All File Exist In $Resouces"
	}
	Write-Host "Start Check Folder : $Resouces"
	if(Test-Path $Resouces -IsValid){
		mkdir $Resouces
	}
	Write-Host "Start Copy Assembly Resource : $Resouces "
	Get-ChildItem -Path $ProjectDir | Copy-Item -Destination $Resouces -Recurse -Container -Exclude $listExeclude
	Write-Host "Copy Assembly Resource Success! ＼（＾_＾）／"
}
# #Move PackageContents to resource
function MovePackageContent(){
	WriteConsole("Move File $content To Bundle")
	$pathxml = "$SolutionDir" + "$projectName\"+$content
	Write-Host "Path XML :" $pathxml
	if(Test-Path -Path $pathxml -PathType Leaf){
		
		if(-not(Test-Path -Path $bundle$content -PathType Leaf)){
			Write-Host $bundle$content "Not Exist"
			Copy-Item -Path $pathxml -Destination ($bundle+$content) -Force
			if(Test-Path -Path $bundle$content){
				Write-Host "Move File $content Success! ＼（＾_＾）／"
			}
			else{
				Write-Error "Move File $content Fail! ＼（＾_＾）／"
			}
		}
	}
	else{
		Write-Host "File $content Not Found!"
	}
	
}

# Config Use for Debug or Release

function RunDebug(){
	KillProcessNavis
	CopyAssembly
	Write-Host  -ForegroundColor Green "Warning: Please Toggle To Release Mode If You Want Publish Release Version !' ＼（＾_＾）／ , config in postbuild.ps1"
}
function RunRelease(){
	KillProcessNavis
	CopyAssembly
	Write-Host  -ForegroundColor Green "Warning: Please Toggle To Debug Mode If You Want Copy File And Debug Mode' ＼（＾_＾）／ , config in postbuild.ps1"
}

MovePackageContent

if($Configuration -match 'Debug N\d\d'){

	RunDebug
}
elseif($Configuration -match 'Release N\d\d'){

	RunRelease
}
else
{
	Write-Host "Error: Have Some Problem with setup Configuration, Please Remove Or Add Correct Config to Release ' ＼（＾~＾）／ ,see config in postbuild.ps1"
	Write-Host "Learning Poweshell at https://www.tutorialspoint.com/powershell/"
}
