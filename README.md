# EasyLogger

EasyLogger is a small wrapper library for NLog. It makes it incredibly easy to jump in to developing your application
with logging facilities ready to go with virtually no configuration. Out of the box, EasyLogger will log to both the
console and to a dated file in a `logs` subdirectory in the directory your application executes in. Since EasyLogger
wraps NLog, you can modify the way logging is performed using the NLog.config file that is added to your application.

## Features

- Logs by default show the date and time, the type that called the logger, and the the message prefixed by the name
  of the method which called the logger. This gives you just about everything you need at a glance.
- Logging is fully configurable using the NLog config file, EasyLogger attempts to stay out of your way, and only
  enhances the default NLog logging facilities.
- All log methods at a minimum provide built in string formatting (format string and unlimited object arguments).

## Examples

Using the snippet below:

```csharp

using EasyLogger;
public class Database
{
	private Log Logger { get; set;}

	public Database() 
	{
		Logger = Log.Get<Database>();
	}

	public void Init()
	{
		Logger.Trace("Entering");

		// do stuff
		Logger.Debug("Doing database stuff with these things: Name: {0}, ConnectionString: {1}", this.DatabaseName, this.ConnectionString);

		Logger.Trace("Exiting");
	}
}

```

This would be the expected logging output:

```

2013-02-06 22:03:32.7545 | TRACE | Database | Init: Entering
2013-02-06 22:03:33.7845 | DEBUG | Database | Init: Doing database stuff with these things: Name: Pies, ConnectionString: http://localhost:8080
2013-02-06 22:06:34.8361 | TRACE | Database | Init: Exiting

```

Your best bet is to install the NuGet package: `install-package EasyLogger`, and explore the API yourself! It's very straightforward.

## License

The MIT License (MIT)
Copyright (c) 2013 Paul Schoenfelder

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.