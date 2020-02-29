@echo Start WebProxy.bat

rem 경로 셋팅
SET MSBUILDDIR=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin
SET ProjectPath=C:\PoseProject_Git\Web App\PoseSportsWebService\CodeGenerator
SET OutputPath1=C:\PoseProject_Git\Web App\PoseSportsWebService\CodeGenerator\bin\Release\Proxy
SET OutputPath2=C:\PoseProject_Git\Web App\PoseSportsWebService\CodeGenerator\bin\Release\ErrorDescription
SET CopyPath1=C:\PoseProject_Git\Common Library\DotNet_Standard\PosePacket\Proxy
SET CopyPath2=C:\PoseProject_Git\Web App\PoseSportsWebService\SportsWebService\App_Data

@echo 1. Build
@echo Clean CodeGenerator.sin
@"%MSBUILDDIR%\MSBuild.exe" "%ProjectPath%\CodeGenerator.csproj" /t:Clean
IF %ERRORLEVEL% NEQ 0 (
	echo fail. msbuild.exe clean "CodeGenerator.sin"
	pause
	exit /b 1
)

@echo Build CodeGenerator.sin
@"%MSBUILDDIR%\msbuild.exe" "%ProjectPath%\CodeGenerator.csproj" /p:DeployOnBuild=true /p:VisualStudioVersion=15.0 /p:Configuration=Release
IF %ERRORLEVEL% NEQ 0 (
	echo fail. msbuild.exe build "CodeGenerator.sin"
	pause
	exit /b 1
)

@echo call exe
pushd "C:\PoseProject_Git\Web App\PoseSportsWebService\CodeGenerator\bin\Release"
	CodeGenerator.exe
popd

@echo 2. Copy to work dir
xcopy /s /e /y /k /r "%OutputPath1%" "%CopyPath1%"
xcopy /s /e /y /k /r "%OutputPath2%" "%CopyPath2%"
