Deployment of app periodically downloading latest adds can be done by running DeployBackgroundProcess.ps1

powershell commands to set ExecutionPolicy to allow running powershell scripts
set-executionpolicy remotesigned
set-executionpolicy restricted

When setting up automatic trigger via TaskScheduler make sure you set in Actions -> Edit -> Start in(optional). 
This should be set to folder containing .exe. Without this the app won't be able to load app.config.

Adding Migration
- in Package manager console set project to point at EFRepository
run following command "add-migration <MigrationScriptName>"

Deploy.ps1 script will publish background app for pull advertisements, copy it to working folder and schedule it via windows taskScheduler.