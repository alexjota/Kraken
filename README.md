Kraken
======

Kinect Realtime Analysis with Azure Event Hubs.

It uses the Depth-Basics sample from the Kinect SDK with a few tweaks to record events to Event Hubs.
A consumer worker role processes the events, deciding whether to raise a proximity alert (<50 cm from the sensor).
For the time being, a consumer worker role processes the alerts and displays the alert in the console.

Next steps: build a dashboard to show proximity alerts.
