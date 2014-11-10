Kraken
======

Kinect Realtime Analysis with Azure Event Hubs.

It uses the Depth-Basics sample from the Kinect SDK with a few tweaks to record events to Event Hubs.
A consumer worker role processes the events, deciding whether to raise a proximity alert (<50 cm from the sensor).
For the time being, a consumer worker role processes the alerts and displays the alert in the console.

Before you get the sample running, you must configure the Cloud service and the MainWindow.xaml.cs (WindowLoaded method) in the DepthRecorder project with your Azure settings.

If you start debugging, the website and the Cloud service should fire up. you can then execute the DepthRecorder project to start the Kinect depth sensor sample.
