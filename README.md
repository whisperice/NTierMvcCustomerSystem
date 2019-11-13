# N-Tier Mvc Customer Management System
It follows Model-View-Controller(MVC) Pattern and the N-Tiers Deployment Architecture Pattern.  
It uses a JSON file as the data source to store the details of all customers.  
Newtonsoft.Json is used for processing JSON files.  
NLog is used for logging. 

## Note for Running
1. Please Open with Visual Studio 2017 or later, using <b>Open Soultion</b> and choose "NTierMvcCustomerSystem.sln".
2. Right click the solution, choose <b>Build Solution</b>.
3. Click the whole solution, <b>Run with IIS</b> (Ctrl + F5).
4. If you encounter "Server Error in '/' Application" Problem, please close the opened website tab, then <b>Clean Solution</b>, <b>Build solution</b> again and <b>Run with IIS</b>. It will work after at most triple <b>Clean/Build Solution</b>.
5. If it still doesn't work, please feel free to contact me.
* It is running smoothly in my PC locally, but existing some dependencies problem after downloaded from Github as a new solution. <b>Clean/Build Solution</b> will help. After it works, you don't need to <b>Clean/Build Solution</b> any more.
According to the StackOverFlow, the issue might be caused by the old version of .Net Framework and Visual Studio.
* If some dependencies can not be recognised, please <b>Clean/Build Solution</b>, <b>Close Solution</b> and <b>Open Solution</b> agian.
Since the whole dependencies are up to 114Mb, so it may take time before they are downloaded.
* Please note the .Net Framework version is 4.6.1

## Note for the Test Cases and Data Source File
1. Please make sure the program have read/write right to D:\, otherwise, the test project can not generate the files used for the test, and the test cases will be failed.
2. If you would like to change the generated test files to a specific path, please modify the DataSourcePath value in NTierMvcCustomerSystem\NTierMvcCustomerSystem.Tests\Common\TestConstants.cs
3. Normally, the data source JSON file for the details of all customers will be located in the executing path. The path is like "C:\Users\\{UserName}\AppData\Local\Temp\Temporary ASP.NET Files\vs\\.....". Since call notes are content, they are not stored together with the data source file. Call notes for different customers are saved in separated files.
4. But if the program doesn't have the right of read/write to that path, please change the below two values to a path, where the program has read/write right, to make the program work.  
<add key="DataSourcePath" value="D:\TempFolderForExecuting" /> in NTierMvcCustomerSystem\NTierMvcCustomerSystem\Web.config
<add key="DataSourcePath" value="D:\TempFolderForExecuting" /> in NTierMvcCustomerSystem\NTierMvcCustomerSystem.Tests\App.config

## Note for Log
1. Log files are located under NTierMvcCustomerSystem\NTierMvcCustomerSystem\logs
2. Now the log level is DEBUG. I know in the production environment, normally the log level should not be DEBUG, but I just leave it for you to see more logs in the log file.
3. If you would like to change it to INFO level, please change the enabled value to false in the following place.
<logger name="*" minlevel="Debug" writeTo="logfile" enabled="true" final="true" /> in NTierMvcCustomerSystem\NTierMvcCustomerSystem\Nlog.config

## Solution Structure
The whole solution contains 6 projects.  
* NTierMvcCustomerSystem: the MVC project for the presentation tier
* NTierMvcCustomerSystem.DataAccess: the data access tier for processing the JSON files
* NTierMvcCustomerSystem.BusinessLogic: the service tier for handling the request from the presentation layer and requesting data processing from data access tier
* NTierMvcCustomerSystem.Common: the common files will be used by each project
* NTierMvcCustomerSystem.Model: the model files for business models and their validators
* NTierMvcCustomerSystem.Test: all test cases and the files used by them
