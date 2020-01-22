param(
[string] $sourcePath = 'C:\Users\okubicek\source\repos\FlatOffersTracker\FlatOffersTrackerBackgroundApp\bin\Release\netcoreapp2.2\win-x64\publish',
[string] $destinationPath = 'C:\Users\okubicek\FlatOfferTracker',
[string] $taskName = "FlatTracker"
)

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

$fullSourcePath = $sourcePath + '\*'
copy-item -Path $fullSourcePath -destination $destinationPath -Force -recurse

Start-ScheduledTask -TaskName $taskName

