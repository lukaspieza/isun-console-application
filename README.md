# isun Weather Forecast Console Application
This application will show and save weather forecasts retrieved from External API.
## Run
_Prerequisites: compiled for win-x64 runtime._
- Download already compiled application from [Google Drive](https://drive.google.com/uc?id=1KEZK2-iVWv5lXJd9-D_4UBzsiSvs-3ly&export=download)
- SHA-256 Checksum for Zip file: **8A97CC8F0DE2B46A439AB5FEE164F5526417C435A04FE20BE6A6E50FBC8A9B33**
- Extract zip file
- Open directory so you could see isun.exe
- Enter command in File Explorer address bar and hit Enter
    ```cmd
    cmd.exe /k isun --cities Vilnius, Kaunas, Klaipėda
    ```
    ![N|Solid](https://i.imgur.com/lsHBwOg.png)
- You should see application running in command prompt
    ![N|Solid](https://i.imgur.com/UX7I8fw.png)

## Diagram of Application Use Case (Auto Generated)
More details about Diagrams as Code 2.0 on [YouTube](https://youtu.be/Za1-v4Zkq5E)
[![N|Solid](https://i.imgur.com/ddIWvBR.png)](/diagram/application.dsl)

## NuGet Packages

isun console application is currently extended with the following NuGet packages.

| NuGet | Version |
| ------ | ------ |
| AutoMapper | 11.0.1 |
| AutoMapper.Extensions.Microsoft.DependencyInjection | 11.0.0 |
| Microsoft.Extensions.Configuration | 6.0.1 |
| Microsoft.Extensions.Configuration.Binder | 6.0.0 |
| Microsoft.Extensions.DependencyInjection | 6.0.0 |
| Microsoft.Extensions.Hosting | 6.0.1 |
| Microsoft.Extensions.Hosting.Abstractions | 6.0.0 |
| Microsoft.Extensions.Logging | 6.0.0 |
| Microsoft.NET.Test.Sdk | 17.2.0 |
| Moq | 4.18.1 |
| Newtonsoft.Json | 13.0.1 |
| NLog | 5.0.1 |
| NLog.Schema | 5.0.1 |
| NLog.Web.AspNetCore | 5.1.0 |
| NUnit | 3.13.3 |
| NUnit.Analyzers | 3.3.0 |
| NUnit3TestAdapter | 4.2.1 |
| RestSharp | 108.0.1 |
