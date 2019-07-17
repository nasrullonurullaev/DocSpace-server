PUSHD %~dp0
call start\stop.bat

PUSHD %~dp0..
echo "ASC.Web.Components"
call yarn install --cwd web/ASC.Web.Components --frozen-lockfile > build\ASC.Web.Components.log

echo "ASC.Web.Storybook"
call yarn install --cwd web/ASC.Web.Storybook --frozen-lockfile  > build\ASC.Web.Storybook.log

echo "ASC.Web.Client"
call yarn install --cwd web/ASC.Web.Client --frozen-lockfile > build\ASC.Web.Client.log

echo "ASC.Web.People.Client"
call yarn install --cwd products/ASC.People/Client --frozen-lockfile > build\ASC.Web.People.Client.log

xcopy build\cra\*.* products\ASC.People\Client\node_modules\ /E /R /Y

echo "ASC.Web.sln"
call dotnet build ASC.Web.sln  /fl1 /flp1:LogFile=build/ASC.Web.log;Verbosity=Normal

start /b call build\start\start.bat

pause