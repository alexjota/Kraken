Kraken
======

Kinect Realtime Analysis with Azure Event Hubs.

It uses the Depth-Basics sample from the Kinect SDK with a few tweaks to record events to Event Hubs.
A consumer worker role processes the events, deciding whether to raise a proximity alert (<50 cm from the sensor) and send alerts to a Service Bus queue. Another worker will pick the alert messages and push them to the monitoring website using SignalR.

Requirements

You will need to have a Kinect and the SDK installed (v1.8)
From Azure you will need:
  - An event hub (do configure different send/listen policies).
  - A Service Bus queue
  - A storage account (this is used by the EventProcesorHost to coordinate and checkpoint progress)
  
Configuring the sample

Before you get the sample running, you must configure the Cloud service and the MainWindow.xaml.cs (WindowLoaded method) in the DepthRecorder project with your Azure settings.

If you start debugging, the website and the Cloud service should fire up. It usually takes a bit, but I guess it depends on your environment. You can then execute the DepthRecorder project independently to start the Kinect depth sensor sample and record events.

Read the following blog post for more context:
http://blogs.southworks.net/ajezierski/2014/11/10/azure-event-hubs-the-thing-and-the-internet/


