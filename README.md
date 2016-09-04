Imported from google code. The "Watch My TED" Desktop application gets power readings from your TED Gateway via the TED XML API.

Power is charted in a desktop application per-second, per-minute, and per hour.

You can configure this application to send per-minute power readings to the http://myenergyuse.appspot.com application. The source for this is found at https://github.com/mganley2000/myenergyuse

Or you can send to any GAE application that will accept the meter readings (JSON formatted.) For example, you can publish your own private site, using "myenergyuse" as a template.

The GAE application charts the power in several views. Per minute for the hour, per minute for the day, per 15-minute for the week, and monthly. Per second for the hour will be available soon.

You may host these GAE charts within a Google Gadget on your iGoogle page.

Source code for the desktop application (C#) is found right here, https://github.com/mganley2000/watchmyted/ 
Once again, the website is Python, and is found at https://github.com/mganley2000/myenergyuse/

A 32bit compiled binaries is available here for download.
