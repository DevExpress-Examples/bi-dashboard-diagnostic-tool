<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1085109)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# BI Dashboard Diagnostic Tool

The BI Dashboard Diagnostic Tool allows you to monitor performance of the main DevExpress BI Dashboard's data processing operations (such as data load and filter operations). You can estimate execution time for each operation or see the number of code calls for the session period.

## Example Overview

The BI Dashboard Diagnostic Tool consists of three projects:

- [DashboardDiagnosticToolUI](./DashboardDiagnosticToolUI) 

  The main project that contains a user interface to display and analyze your session data.

  Read more: [The Dashboard Diagnostic Tool UI](./DashboardDiagnosticToolUI/readme.md).

- [DiagnosticTool](./DiagnosticTool) 

  Contains the source code of the BI Dashboard Diagnostic Tool.
  
- [DiagnosticToolTest](./DiagnosticToolTest)

  For internal use. Contains the BI Dashboard Diagnostic Tool tests.

## Prerequisites

Specify your DevExpress NuGet feed autorization key in [nuget.config](./nuget.config#L7).

## Use Diagnostic Tool in the UI

1. Run the Diagnostic Tool.

2. Select **Start Session**.

3. Run the code you want to diagnose.

4. Click **Stop Session** after the code is compiled. Wait until the log tree is built.

5. Save the report.

You can analyze the resulting report or send it to [Support Center](https://supportcenter.devexpress.com/ticket/list) for assistance.

The following GIF image illustrates how to use the Diagnostic Tool to examine the performance of the dashboard load operation:

![BI Dashboard Diagnostic Tool](./images/bi-dashboard-diagnostic-tool.gif)

## Use Diagnostic Tool in Code

1. Reference `DiagnosticTool.dll` and install the [Microsoft.Diagnostics.Tracing.TraceEvent](https://www.nuget.org/packages/Microsoft.Diagnostics.Tracing.TraceEvent/) package in your dashboard project. 

2. Create a `DiagnosticController` object. 

3. Call the controller's `Start` and `Stop` methods to collect performance data for your code. 

4. Implement the `IFileController` interface and specify the output file path in the `TrySaveFile` method. Pass a new class instance that implements `IFileController` to the controller's constructor. 

5. Call the controller's `SaveAs` method to generate an XML report.

## Documentation

## Examples 

[Dashboard for WinForms - Inspect the Dashboard Performance](https://github.com/DevExpress-Examples/dashboard-for-winforms-diagnose-performance)
