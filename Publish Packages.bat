@REM Builds and pushes NuGet packages

mkdir deploy
mkdir deploy\packages
del deploy\packages\*.nupkg

nuget pack "source\N8IZedGraph.nuspec" -outputdir deploy\packages 
nuget pack "source\N8IZedGraph.WinForms.nuspec" -outputdir deploy\packages 

nuget push deploy\packages\*.nupkg
