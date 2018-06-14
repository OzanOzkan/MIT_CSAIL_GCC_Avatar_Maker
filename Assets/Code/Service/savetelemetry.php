<?php 
	$filename = $_POST["filename"];
	$data = $_POST["data"];
	
	if($filename != "")
	{
		echo("Message received");
		
		$file = fopen("TelemetryData/" . $filename . ".xml", "a");
		fwrite($file, $data);
		fclose($file);
	}
	else
	{
		echo("Message not received");
	}
?>