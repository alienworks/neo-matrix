# Overview
An API checker use to get API status infos from nodes running by NEO eco. 

Currently it just a simple scripting. Providing with basic CSV output and errlog features.

# Getting Started
## Setup
```sh
git clone https://github.com/alientworks/neo-matrix
cd neo-matrix
npm i
```

## Usage
1. Config the node that you wanted to check in the `src/nodes.json` file.
2. Then run `npm start` command. While script is running, the log will be printed at the shell.
3. And a `output.csv` and several`*.log` file been outputted in the folder from where you run the command.
4. Open the csv file up to see the info we get from the script, and the log files to diagnose whether the API is ok or not. 
