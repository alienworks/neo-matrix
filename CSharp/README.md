# NeoMatrix .NET Client

[![.NET Core](https://img.shields.io/badge/.NET%20Core-%203.1-brightgreen)][DotNetCoreUrl]

[DotNetCoreUrl]: https://dotnet.microsoft.com/download

## Overview

An API checker use to get API status infos from nodes running by NEO eco.

## Getting Started

### Setup

|         Name         |  Version  |
| :------------------: | :-------: |
|   DotNet Core SDK    | __3.1+__  |
| EntityFramework Core | __3.1+__  |
|  Visual Studio 2019  | __16.5+__ |
|        MySQL         | __8.0+__  |

### Usage

1. Create Datebase which is called, for example, `neonodes_local_v1.4.0` in MySQL manually.

2. Check the *ConnectionStrings* among [appsettings.json](https://github.com/alienworks/neo-matrix/blob/master/CSharp/NeoMatrix/appsettings.json). Replace it with your own setting if necessary.

3. Build the solution in Visual Studio.

4. Open the ***Package Manager Console*** and set the default project as `src\NeoMatrix.Data`.

5. Execute the command line like: `Add-Migration neonodes_local_v1.4.0 -c MatrixDbContext`, and wait for the end of migration files creating.

6. Execute the command line: `Update-Database`, and then check the database whether the data tables are created.

7. Run the console.

### Validation Progress

1. Send Http request and check if Http status is OK in a limit of TIMEOUT.

2. Try to deserialize the Http response and check if the content is a valid format of JSON.

3. Check if the JSON is in the contemplation of what the *appsettings.json* defines.
