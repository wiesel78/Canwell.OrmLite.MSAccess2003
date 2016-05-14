###########################################################################
#                                                                         #
#             Canwell.OrmLite.MSAccess2003 Buildscript                    #
#                                                                         #
# Argumente                                                               #
#   -Target [Clean,Set-Version,Restore-NuGet-Packages,                    #
#            Build,Copy,Version-Control, Publish-Nuget-Packages]  	      #  
#            default : Compress                            			      #
#            description : Setzt das CakeBuildScriptTarget                #
#   -IncMain    <int> erhöht die Mainnummer in der Verion. default : 0    #
#   -IncFeature <int> erhöht die Featurenummer in der Verion. default : 0 #
#   -IncBugfix  <int> erhöht die Bugfixnummer in der Verion. default : 0  #
#   -IncBuild   <int> erhöht die Buildnummer in der Verion. default : 1   #
#                                                                         #
#                                                                         #
###########################################################################

param(
    [string]$Target="Publish-Nuget-Packages",
    [int]$IncMain=0,
    [int]$IncFeature=0,
    [int]$IncBugfix=0,
    [int]$IncBuild=1
)

$dest_tools = "Tools\"
$dest_cake = $dest_tools + "Cake"


# Tools herunterladen, falls nötig
if( !(Test-Path $dest_cake)){
    .\Tools\Nuget\nuget.exe install Cake -OutputDirectory Tools -ExcludeVersion
}


# zusammenbauen des Cakebuild-Aufrufes
$cakestr = '../Tools/Cake/Cake.exe build.cs '

$cakestr += [string]::Format(" -Target={0}", $Target)
$cakestr += [string]::Format(" -IncMain={0}", $IncMain)
$cakestr += [string]::Format(" -IncFeature={0}", $IncFeature)
$cakestr += [string]::Format(" -IncBugfix={0}", $IncBugfix)
$cakestr += [string]::Format(" -IncBuild={0}", $IncBuild)

cd BuildScripts
Invoke-Expression $cakestr
cd ..
