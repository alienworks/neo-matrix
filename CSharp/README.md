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

1. Create Datebase called `neo_matrix` in MySQL manually.

2. Check the *ConnectionStrings* among [appsettings.json](https://github.com/alienworks/neo-matrix/blob/master/CSharp/NeoMatrix/appsettings.json). Replace it with your own setting if necessary.

3. Build the solution in Visual Studio.

4. Open the ***Package Manager Console*** and set the default project as `src\NeoMatrix.Data`.

5. Execute the command line: `Add-Migration neo_matrix -c MatrixDbContext`, and wait for the end of migration files creating.

6. Execute the command line: `Update-Database`, and then check the database whether the data tables are created.

7. Run the console.