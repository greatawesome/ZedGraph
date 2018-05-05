@REM Builds and pushes NuGet packages

mkdir deploy
mkdir deploy\packages
del deploy\packages\*.nupkg

nuget pack "source\ZedGraph.csproj" -outputdir deploy\packages -Prop Configuration=Release -Prop Platform=AnyCPU

nuget push deploy\packages\*.nupkg
