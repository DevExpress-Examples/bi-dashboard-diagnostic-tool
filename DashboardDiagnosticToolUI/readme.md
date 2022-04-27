# The Dashboard Diagnostic Tool UI

The Diagnostic Tool contains a user interface to display and analyze your session data:  

![Dashboard Diagnostic Tool main window](../images/diagnistic-tool-main.png)

The following sections describe main parts of the Dashboard Diagnostic Tool interface:

## Sessions

The **Sessions** window shows created sessions and allows you to navigate between them. 

![Dashboard Diagnostic Tool Sessions](../images/sessions-window.png)

A **Session** is a correlation of all actions executed in a certain time period. When you start session, the Dashboard Diagnostic Tool is monitoring the executed code and collects actions for each event of an event session. After you stop session, the Dashboard Diagnostic Tool generates a log tree. You can inspect it in the UI or save the resulting report. The report is serialized in XML format and can be opened in the Dashboard Diagnostic Tool. 

## Results 

The **Results** window displays resulting benchmarks.

**Benchmark** is an object that serves for performance evaluation or comparison. Each benchmark contains the following information:

- **Name**

  The benchmark name.
        
- **Count**

  The number of code calls.
          
- **MSecs**

  The code block's execution time in milliseconds.

The image below displays a log tree for a dashboard loading operation. The largest amount of time during the session takes the query execution:
     
![Dashboard Diagnostic Tool results](../images/results-window.png)

## Events

The **Events** window displays information about the objects collected since the trace event started.

The image below shows logs for the `DashboardSqldataSource.ExecuteQueryClientMode` benchmark. These logs obtain the executed query and the number of requested columns and rows. 

![Dashboard Diagnostic Tool events](../images/events-window.png)

## Manage Sessions in the UI

The main menu contains the following commands:

- **File**

    ![Diagnostic Tool file options](../images/file-options.png)

    The File menu allows you to save sessions in a report. The report is saved in XML format. Click **Open** to open the existing report. To close the program, select **Exit**.

- **Diagnostic** 

    ![Diagnostic Tool diagnostic options](../images/diagnistic-options.png)

    The Diagnostic menu allows you to manage sessions. Click **Start Session** to create a new session. **Stop Session** ends the session and generates a log tree. To delete the session, select it and use **Delete** command.
