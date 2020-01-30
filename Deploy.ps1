param(
	[string] $publishPath = "C:\Users\okubicek\source\repos\FlatOffersTracker\FlatOffersTrackerBackgroundApp\",
	[string] $destinationPath = "C:\Users\okubicek\FlatOfferTracker",
	[string] $taskName = "FlatTracker"
)

$projectName = "FlatOffersTrackerBackgroundApp.csproj"
$publishPathSuffix = "bin\Release\publish"

get-scheduledtask -TaskName $taskName -ErrorAction SilentlyContinue -OutVariable task
if ($task){
	Stop-ScheduledTask -TaskName $taskName
}
else{
	$action = New-ScheduledTaskAction -Execute "($destinationPath)\FlatOffersTrackerBackgroundApp.exe" -WorkingDirectory $destinationPath
	$trigger = New-ScheduledTaskTrigger -AtStartup
	$settings = New-ScheduledTaskSettingsSet
	$task = New-ScheduledTask -Action $action -Trigger $trigger -Settings $settings
	Register-ScheduledTask -TaskName $taskName -InputObject $Task
}

$projectPath = $publishPath + $projectName
$publishApplicationPath = $publishPath + $publishPathSuffix
Remove-Item "$publishApplicationPath\*" -Recurse -ErrorAction Ignore

& dotnet publish $projectPath -c Release -r win-x64 -o $publishApplicationPath --self-contained true

copy-item -Path "$publishApplicationPath\*" -destination $destinationPath -Force -recurse

Start-ScheduledTask -TaskName $taskName