Command for publishing Release version of background app
dotnet publish -c Release -r win-x64 --self-contained false

When setting up automatic trigger via TaskScheduler make sure you set in Actions -> Edit -> Start in(optional). 
This should be set to folder containing .exe. Without this the app won't be able to load app.config.

Adding Migration
- in Package manager console set project to point at EFRepository
run following command "add-migration <MigrationScriptName>"