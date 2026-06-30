**HiTopo**

**V1.2 Readme**

TopoData provides data acquisition and storage capabilities for the industrial automation sector, along with data read/write interfaces for third-party software, aiming to help system integrators bridge the communication gap between IT and OT.

This is a open project, welcome programers take part in this project to develop new features.

The outstanding features of TopoData software are as follows:

- Supports Siemens Profinet 、Modbus TCP and OPC UA protocol for communication with devices.
- Provides HTTP WebAPI interfaces for IT systems to read and write PLC or device data.
- Offers system diagnostics functionality, allowing engineers to view PLC communication status, read/write data, and download recipe parameters.
- Provides relational database data storage functionality with a flexible storage mechanism; data storage triggering conditions can be configured as cyclic storage, event-based storage, or expression-based storage.
- Flexible definition of binding relationships between database table structures and communication points; engineers do not need to understand SQL statements, as the system can generate database table structures with one click.
- Database support includes three options: Microsoft SQL Server, SQLite, and PostgreSQL.
- Provides flexible recipe definition functionality, allowing users to select different recipes and download recipe parameters to PLCs or devices.
- Historical data can be exported to Excel.
- HMI Data acquisition and storage models for industry software engineers.
- Project import and export function.

<a id="_Toc222430977"></a>
# 1 **Installation**

<a id="_Toc222430978"></a>
## 1.1 **Installation Environment**

- The software is compatible with Windows 7 and above operating systems.
- The software is developed based on DotNet 8. Therefore, before using it, the following components need to be installed:

- windowsdesktop-runtime-8.0.11-win-x64.exe
- dotnet-runtime-8.0.11-win-x64.exe
- aspnetcore-runtime-8.0.11-win-x64.exe

<a id="_Toc222430982"></a>
# 2 **System configuration**

![](images/05e88ddc92e45f90cb7ee982865743ab38a988707319752032e75fbe4a5ab439.jpg)

<a id="_Toc222430983"></a>
## 2.1 **WebAPI Configuration**

Third-party IT software can access real-time data of the data acquisition channel through the HTTP WebAPI and also perform write operations.

<a id="_Toc222430984"></a>
## 2.2 **Database configuration**

- Database Type: A drop-down list of database types, allowing you to select the type of database to store, including Microsoft SQL Server, PostgreSQL, and MySQL.
- Database keep Time: In months, with a minimum of 1 month. Data older than this period will be automatically deleted by the service software, and only data within this period will be retained.
- SQL Host: The address of the server where the database is located.
- Database: The name of the database.
- Database Storage Location: Only useful when creating an SQL Server database. For PostgreSQL and MySQL, refer to the corresponding manual for data storage locations. Users select a folder and click the "DB Create" button to create the database (if a remote database is configured, users need to create the same folder structure locally and remotely when selecting. If a local location is selected, the remote database will be stored in the corresponding location).
- Username: The database user.
- Password: The database login password.
- Port: The database port.
- Local Log: Logs will be stored in the Logs folder and are mainly used for software debugging or finding system bugs. It is disabled by default.
- Create DB Button: Create the database based on account, password, port, and other information.
- Test DB Button: Test the database connection status based on the configuration.
- Save Button: Save configuration.

<a id="_Toc222430985"></a>
## 2.3 **Automatic Start**

By creating shortcuts in the startup folder, the software can be made to start automatically upon windows booting.

# 3 **Equipment Management**

<a id="_Toc222430980"></a>
## 3.1 **Simens Profinet Equipment**

User select “Siemens Profinet” Driver Type, this driver provides communication functionality with Siemens S7-300/400 and S7-1200/1500 PLCs.

![](images/dc288feb8c392f6e65e5cd735fb8402efaf9233582e67139b5c2fe7775c0fc7b.jpg)

- Special Settings for S7-1200 and S7-1500 in TIA Portal Software

- The CPU must be configured to allow PUT/GET communication access from remote partners (the interface is similar to the following).

![](images/67cb9d8d06f2fbf82199359410781a317c7fd81ac4667b84464b7978b3e4440c.png)

- If you need to access DB blocks, the DB blocks must be configured for non-optimized address access:

![](images/80ced408ba625dafac86a08c818405fd904b1391c529d37070a222831ade72c9.png)

- 配置点表

TopoData suportated：

<table><tr><td><p>num</p></td><td><p>TopoData DataType</p></td><td><p>TIA DataType</p></td><td><p>Descirption</p></td></tr><tr><td><p>1</p></td><td><p>Int</p></td><td><p>Dint</p></td><td></td></tr><tr><td><p>2</p></td><td><p>Bool</p></td><td><p>Bool</p></td><td><p>boolean</p></td></tr><tr><td><p>3</p></td><td><p>Float</p></td><td><p>Float32</p></td><td><p>Float 32</p></td></tr><tr><td><p>4</p></td><td><p>String</p></td><td><p>String</p></td><td><p>string</p></td></tr><tr><td><p>5</p></td><td><p>Wstring</p></td><td><p>Wstring</p></td><td><p>Unicode string</p></td></tr><tr><td><p>6</p></td><td><p>WORD</p></td><td><p>WORD</p></td><td><p>int 16</p></td></tr></table>

![](images/8f7f5345407ea66a127d542229daf08f1856ff2682c02008415d66d818722dbe.jpg)

As shown in the figure below, TopoData supports configuring the point table via Excel import and export. The first column is the variable name, which will be used later for database storage and external interfaces; the second column is the PLC point address (the address naming is the same as in TIA Portal, e.g., DB301.wstring12.10, where 301 is the DB block number, 12 is the absolute address of the string, and 10 is the string length); the third column is the data type; the fourth column is the unit of the point (if no unit is required, use another character as a placeholder, preferably not left blank); the fifth column is the remark or description of the variable name.

*Notes:*

- *For the first-time configuration, users can click the "Export" button. The software provides a sample address format, which users can edit based on, then import and save.*
- *To improve communication efficiency, it is recommended to place data communication points in one or as few DB blocks as possible, with data locations as adjacent as possible.*
- *The current driver only supports read/write operations for DB blocks; other memory areas are not supported at this time.*
- *Bool-type data does not support write operations.*

<a id="_Toc222430981"></a>
## 3.2 **OPC UA Equipment**

User select “OPCUA” Driver Type, this driver provides communication functionality with devices via OPC UA communication type.

![](images/54b76a844e0be50b1fe1a1fda0a2581d8f9ecf5a0be84e204ca2ad44ba1b85a2.jpg)

- "Get Endpoint" button: After correctly filling in the URL, click the "Get Endpoint" button. The system will enumerate all Endpoints. The user selects one and clicks the "Save" button to save it.
- The "Brows Tag" button: A dialog box pops up for selection. Right-click on the parent node and select "Get all subItems" from the context menu to add sub-level all Tags, or select the yellow icon node individually and choose "Get this item" to add the current OPC UA Tag.

![](images/26cc64ea7c2353550c3bc637b82d51e0d0da4dafc40a9c8989699a4a2abb5ea1.jpg)

- Import and export tags

As shown in the figure below, TopoData also supports configuring the point table through Excel import and export. The first column is the variable name, which will be used later for database storage and external interfaces; the second column is the UA point address (must start with "ns"); the third column is the unit of the point (if no unit is required, use another character as a placeholder, preferably not left blank); the fourth column is the remark or description of the variable name.

![](images/feb502469d659a82b68caf622174a3f65bbaa4eeb108be64e81087283e6bebe6.jpg)

*Note：*

- *For the first time, users can click the "Export" button. The software will provide an example address format, which users can edit based on, then import and save.*
- *The "On-Off switch" allows users to enable or disable the device communication function.*

## 3.3 **Modbus TCP Equipment**

User select “Modbus TCP” Driver Type, this driver provides communication functionality with devices via Modbus TCP communication type.

![](images/99b5f29537d2e3ac1d7b97ccd55ed6fd17c6455adcc1600dff2d66b649b77592.jpg)

Modbus TCP includes following address formats：

- Holding register：

Data address type： 4xxxxx

Read/Write: Read and write

Data Type：

<table><tr><td><p>num</p></td><td><p>DataType</p></td><td><p>Longth(byte)</p></td></tr><tr><td><p>1</p></td><td><p>Int</p></td><td><p>4</p></td></tr><tr><td><p>2</p></td><td><p>Int64</p></td><td><p>8</p></td></tr><tr><td><p>3</p></td><td><p>Word</p></td><td><p>2</p></td></tr><tr><td><p>4</p></td><td><p>Int16</p></td><td><p>2</p></td></tr><tr><td><p>5</p></td><td><p>Float</p></td><td><p>4</p></td></tr><tr><td><p>6</p></td><td><p>Double</p></td><td><p>8</p></td></tr></table>

- Input register：

Data address type： 3xxxxx

Read/Write: Read only

Data Type：

<table><tr><td><p>num</p></td><td><p>DataType</p></td><td><p>Longth(byte)</p></td></tr><tr><td><p>1</p></td><td><p>Int</p></td><td><p>4</p></td></tr><tr><td><p>2</p></td><td><p>Int64</p></td><td><p>8</p></td></tr><tr><td><p>3</p></td><td><p>Word</p></td><td><p>2</p></td></tr><tr><td><p>4</p></td><td><p>Int16</p></td><td><p>2</p></td></tr><tr><td><p>5</p></td><td><p>Float</p></td><td><p>4</p></td></tr><tr><td><p>6</p></td><td><p>Double</p></td><td><p>8</p></td></tr></table>

- Coil ：

Data address type： 0xxxxx

Data Type：bool

Read/Write: Read only

- Input：

Data address type： 1xxxxx

Data Type：bool

Read/Write: Read only

<a id="_Toc222430986"></a>
# 4 **DataTable storage**

This functional module is mainly used for data storage configuration. Users can utilize a fixed database table structure or create their own, bind corresponding PLC communication points, and store the data.

![](images/1d1bc35e8a5c23ea3e8b80f2c2dbad7bed3e5ac7e2b13b935465bcde423532cd.jpg)

- Storage Information

Task: Storage Task

DB Table: user defined Database table name

Equipment: equipment name

Switch button: enable or disable database storage

Time scale table checkbox: If this option is not selected, data will be stored in the user-defined table structure, with the table name taken from the "DB Table" text box; if this option is selected, data will be stored in the table "tb\_au\_pubHisvalues".

- 数据存储规则

Consecutive Acquisition: As shown in the figure below, select the " Consecutive Acquisition" radio button to enable the system to continuously collect data according to the acquisition cycle. Users need to fill in the acquisition cycle and a bool-type trigger condition data acquisition point. When the bool value is true, the software performs table data storage; when false, the system does not store data.

![](images/cfcb5c53b35236b7d67f29379dbbd1e4d3c7a3f814429b1ed3ddaaeb9358e63d.jpg)

To achieve more complex acquisition and storage rules, please check the " Expression" checkbox, click the "Expression" button, and a dialog box will pop up for configuring the expression conditions.

Rising Edge Acquisition: As shown in the figure below, select the "Rising Edge Acquisition" radio button to enable table data storage when the system detects a bool value of On/True. Users need to configure a bool-type trigger condition point.

![](images/0e3dfb4fca7097c3430f5a53f141006bf512145bbbebb31d06a41a46877987d6.jpg)

To achieve more complex acquisition and storage rules, please check the " Expression" checkbox, click the "Expression" button, and a dialog box will pop up for configuring the expression conditions.

Use Expression: Expressions can be used in both Rising Edge and Continuous Acquisition modes. An expression is a formula that combines multiple variable conditions to determine the data storage condition.

Users check the " Expression" checkbox and click the "Expression" button to pop up the Expression Editor dialog. First, users click the plus button to the right of the parameter list in the middle to add one or more internal variables for the expression. Then, edit the expression in the expression text box, click the red checkmark button to verify if the expression is correct. If correct, click the Save button to save the expression.

![](images/1b5d301ac5060d10a4fb72242a599cc9aa23b35745b8a99d7014edcbc8b99c85.jpg)

- Data Collection

As shown in the figure below, users can custom-configure the storage data table structure, where each data column corresponds to a device communication point. The system stores a set of real-time data into this table according to the acquisition rules configured above.

![](images/b069835157d9d188835b6a4eae71a19e7a7de33b01dd643b572c6d0e54675230.jpg)

- Batch Tag

This configuration is optional. If a batch number communication point is configured, the system will assign work order (WO) information to the corresponding reserved WO field in the table based on this point. This communication point must be in string format.

![](images/12776c16c6a3e6a28e8e97a1a196533bdb078b550dd9311a1b8e79417e5fab62.jpg)

<a id="_Toc222430987"></a>
# 5 **Recipe Management**

Users can define multiple sets of recipe download parameters, where each recipe can define different download parameters as well as different default values.

![](images/3f7d60207e47834c9abd64305d3336add8620ab5664f5868f0a30fc416ce132c.jpg)

<a id="_Toc222430988"></a>
## 5.1 **Button action**

Add，Delete and Save configuration buttons.

<a id="_Toc222430989"></a>
## 5.2 **Basic information**

Recipe name and description define as well as enable configuration.

<a id="_Toc222430990"></a>
## 5.3 **Recipe parameters**

Table for recipe parameters as well as configuration buttons.

<a id="_Toc222430991"></a>
# 6 **Parameter Diagnose**

![](images/845e41462794d39bca6cb947982927347b7f5e2c785a37529e56ee6ec114ce48.jpg)

If all modules have been correctly configured according to the requirements in Chapters 3, 4, and 5, click the 'Start' button at the top to switch to the 'Parameter Diagnose' module for real-time monitoring and operation of the equipment.

- The module provides filtering functionality by TagName.
- The module provides recipe parameter download functionality.
- Double-click the communication point to pop up a dialog box, where users can enter the value they want to write and send it to the controller.

![](images/341102ea09ffccd897dd23926b2e8b946a659ff2c47893600970010815fd3641.jpg)

<a id="_Toc222430992"></a>
# 7 **Historian query**

![](images/2c4e1135e0c823aa6f8059a7b7e84099b427908294f2db0b5195ae75244db6f8.jpg)

Data stored in the relational database can be queried through the Historical Query module and exported to Excel.

If 'DataTable' is selected, the queried data comes from the database table 'ProcessData' configured in Chapter 5; if 'Time Scale Table' is selected, the queried data comes from the database table 'tb\_au\_pubHisvalues'.

<a id="_Toc222430993"></a>
# 8 **WebAPI Read and Write**

If external WebAPI interface functionality is required, the WebAPI service address must first be configured in Section 4.1

<a id="_Toc222430994"></a>
## 8.1 **WebAPI TagName**

![](images/02b168f6e81181ac0eb1e79faca3115bce3837de76fa1029f565a8aea57af6c0.jpg)

In the WebAPI and Parameter Diagnostics modules, the TagName is equivalent to 'Data Acquisition Channel + Point Name' in the Device Management module.

<a id="_Toc222430995"></a>
## 8.2 **Read Tags**

![](images/c11c9df171a0b6a3d08317bd27d78d2f8d2fbcd6dcbb8d06abc84f7114e93633.jpg)

WebAPI Post：

[http://127.0.0.1:8093/api/Data/GetRealtimeValues](http://127.0.0.1:8093/api/Data/GetRealtimeValues)

Content-Type: application/json

The input parameter is a JSON string array.：

Request Body:

[

"R0021.alm1",

"R0021.pH",

"R0021.pkg\_Bucket"

]

<a id="_Toc222430996"></a>
## 8.3 **Write Tags**

![](images/b0fa1d111238b3ca2ca9b005a5a3b03e79cc49540cb0682393210acd03c1e4ce.jpg)

WebAPI Post：

[http://127.0.0.1:8093/api/Data/WriteRealtimeValues](http://127.0.0.1:8093/api/Data/WriteRealtimeValues)

Content-Type: application/json

The input parameter is a JSON string array.

Request Body:

[

{

"CannelID": "R0021",

"ValueName": "R0021.alm1",

"value": 1

}

]

## 8.4 **Download Recipe**

![](images/8392c1105df7bee4bb7d9a5742b6cc923f6a33724a4876575cd7e5801dd98fac.jpg)

WebAPI Post：

[http://127.0.0.1:8093/api/Recipe/DownloadRecipe](http://127.0.0.1:8093/api/Recipe/DownloadRecipe)

Content-Type: application/json

The input parameter is a JSON string array.

Request Body:

"多肽配方"

<a id="_Toc222430997"></a>
# 9 **License**

MIT License

Copyright (c) [2026] [Topotech Team]

Permission is hereby granted, free of charge, to any person obtaining a copy

of this software and associated documentation files (the "Software"), to deal

in the Software without restriction, including without limitation the rights

to use, copy, modify, merge, publish, distribute, sublicense, and/or sell

copies of the Software, and to permit persons to whom the Software is

furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all

copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR

IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,

FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE

AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER

LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,

OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE

SOFTWARE.
