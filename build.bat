@echo Off

set NUnitPath=packages\NUnit.Runners.2.6.4\tools\nunit-console.exe

REM Package restore
cmd /c %nuget% restore Twilio.2013.sln -NoCache -NonInteractive

REM Build Source from Projects

REM ****** SimpleRestClient *********
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild SimpleRestClient.Net35\SimpleRestClient.Net35.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild SimpleRestClient.Net35.Tests\SimpleRestClient.Net35.Tests.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
if not "%errorlevel%"=="0" goto buildfailure

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild SimpleRestClient.Pcl\SimpleRestClient.Pcl.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild SimpleRestClient.Pcl.Tests\SimpleRestClient.Pcl.Tests.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
if not "%errorlevel%"=="0" goto buildfailure

REM ****** Twilio.Api *********
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Twilio.Api.Net35\Twilio.Api.Net35.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Twilio.Api.Net35.Tests\Twilio.Api.Net35.Tests.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
if not "%errorlevel%"=="0" goto buildfailure

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Twilio.Api.Pcl\Twilio.Api.Pcl.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Twilio.Api.Pcl.Tests\Twilio.Api.Pcl.Tests.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
if not "%errorlevel%"=="0" goto buildfailure

REM ****** Twilio.Api.TaskRouter *********
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Twilio.Api.TaskRouter.Net35\Twilio.Api.TaskRouter.Net35.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Twilio.Api.TaskRouter.Net35.Tests\Twilio.Api.TaskRouter.Net35.Tests.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
if not "%errorlevel%"=="0" goto buildfailure

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Twilio.Api.TaskRouter.Pcl\Twilio.Api.TaskRouter.Pcl.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Twilio.Api.TaskRouter.Pcl.Tests\Twilio.Api.TaskRouter.Pcl.Tests.csproj /p:Configuration=Release /p:VRevision=%BuildCounter% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Rebuild
if not "%errorlevel%"=="0" goto buildfailure


REM Run Unit tests
%NUnitPath% SimpleRestClient.Net35.Tests\bin\Release\SimpleRestClient.Net35.Tests.dll
if %errorlevel% LSS 0 goto testsfailure
if %errorlevel% GTR 0 goto nonpassingfailure
ECHO Errorlevel: %errorlevel%

%NUnitPath% Twilio.Api.Net35.Tests\bin\Release\Twilio.Api.Net35.Tests.dll
if %errorlevel% LSS 0 goto testsfailure
if %errorlevel% GTR 0 goto nonpassingfailure
ECHO Errorlevel: %errorlevel%

%NUnitPath% Twilio.Api.TaskRouter.Net35.Tests\bin\Release\Twilio.Api.TaskRouter.Net35.Tests.dll
if %errorlevel% LSS 0 goto testsfailure
if %errorlevel% GTR 0 goto nonpassingfailure
ECHO Errorlevel: %errorlevel%


%NUnitPath% SimpleRestClient.Pcl.Tests\bin\Release\SimpleRestClient.Pcl.Tests.dll /framework=4.0.30319
if %errorlevel% LSS 0 goto testsfailure
if %errorlevel% GTR 0 goto nonpassingfailure
ECHO Errorlevel: %errorlevel%

%NUnitPath% Twilio.Api.Pcl.Tests\bin\Release\Twilio.Api.Pcl.Tests.dll /framework=4.0.30319
if %errorlevel% LSS 0 goto testsfailure
if %errorlevel% GTR 0 goto nonpassingfailure
ECHO Errorlevel: %errorlevel%

%NUnitPath% Twilio.Api.TaskRouter.Pcl.Tests\bin\Release\Twilio.Api.TaskRouter.Pcl.Tests.dll /framework=4.0.30319
if %errorlevel% LSS 0 goto testsfailure
if %errorlevel% GTR 0 goto nonpassingfailure
ECHO Errorlevel: %errorlevel%


REM Package Folders Setup
rd p /s /q  REM delete the old stuff

setlocal enableextensions

REM if not exist p mkdir p
REM if not exist p\twilio mkdir p\twilio
REM if not exist p\twilio\lib mkdir p\twilio\lib

if not exist p\twilio\lib\net35 mkdir "p\twilio\lib\net35"
if not exist p\twilio\lib\net40 mkdir "p\twilio\lib\net40"
if not exist p\twilio\lib\portable-net403+sl5+netcore45+wp8+MonoAndroid1+MonoTouch1 mkdir "p\twilio\lib\portable-net403+sl5+netcore45+wp8+MonoAndroid1+MonoTouch1"

if not exist p\twilio.taskrouter\lib\net35 mkdir "p\twilio.taskrouter\lib\net35"
if not exist p\twilio.taskrouter\lib\net40 mkdir "p\twilio.taskrouter\lib\net40"
if not exist p\twilio.taskrouter\lib\portable-net403+sl5+netcore45+wp8+MonoAndroid1+MonoTouch1 mkdir "p\twilio.taskrouter\lib\portable-net403+sl5+netcore45+wp8+MonoAndroid1+MonoTouch1"


REM Copy files into Nuget Package structure
REM copy LICENSE.txt download
copy Twilio.Api.Net35\bin\Release\Twilio.Api.* "p\twilio\lib\net35\"
copy Twilio.Api.Pcl\bin\Release\Twilio.Api.* "p\twilio\lib\net40\"
copy Twilio.Api.Pcl\bin\Release\Twilio.Api.* "p\twilio\lib\portable-net403+sl5+netcore45+wp8+MonoAndroid1+MonoTouch1\"

REM copy LICENSE.txt download
copy Twilio.Api.TaskRouter.Net35\bin\Release\Twilio.Api.* "p\twilio.taskrouter\lib\net35\"
copy Twilio.Api.TaskRouter.Pcl\bin\Release\Twilio.Api.* "p\twilio.taskrouter\lib\net40\"
copy Twilio.Api.TaskRouter.Pcl\bin\Release\Twilio.Api.* "p\twilio.taskrouter\lib\portable-net403+sl5+netcore45+wp8+MonoAndroid1+MonoTouch1\"

REM Create Packages
REM mkdir Build

FOR /F "tokens=* delims=" %%x in (version.txt) DO SET ver=%%x
cmd /c %nuget% pack "Twilio.nuspec" -Version %ver%.%BuildCounter%-beta -BasePath p\twilio -o download
if not "%errorlevel%"=="0" goto packagefailure

FOR /F "tokens=* delims=" %%x in (version.portable.txt) DO SET ver=%%x
cmd /c %nuget% pack "Twilio.TaskRouter.nuspec" -Version %ver%.%BuildCounter%-beta -BasePath p\twilio.taskrouter -o download
if not "%errorlevel%"=="0" goto packagefailure


:success
REM use github status API to indicate commit compile success
exit 0

:buildfailure
ECHO Build Failure
REM use github status API to indicate commit compile failure
exit -1

:testsfailure
ECHO Test Run Failure
REM use github status API to indicate commit compile failure
exit -1

:nonpassingfailure
ECHO Non-Passing Tests Failure
REM use github status API to indicate commit compile failure
exit -1

:packagefailure
ECHO Package Failure
REM use github status API to indicate commit compile failure
exit -1