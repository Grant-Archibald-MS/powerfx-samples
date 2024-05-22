# Overview

This sample is designed to show how Power FX and be used as a simple string template language. The sample demonstrates for to:

- Replace variables enclosed in {}. For example {name} which will be replaced with the value of the variable name
- How to handle different Power FX types for example. **StringValue** or **NumberValue**
- Be an example of how to process more complicated type like **TableValue** or **RecordValue** to convert the text. This could be a starting point to consider converting this to markdown. For example convert table to a Markdown Table
- Call inbuilt functions like **Now()**
- Call custom Power FX functions
- Query dataverse tables

## Prerequisites for building module

1. Install [.NET Core 8.0.x SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

2. Ensure that your `MSBuildSDKsPath` environment variable is pointing to [.NET Core 8.0.x SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

## Getting Started

1. Add you connection string to config.dev.json. More information on connection strings is available in [Use Connection Strings](https://learn.microsoft.com/power-apps/developer/data-platform/xrm-tooling/use-connection-strings-xrm-tooling-connect)

2. Run the sample

```bash
cd samples/template
dotnet run
```
