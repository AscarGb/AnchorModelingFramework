### AnchorModelingFramework
Implementation of the anchor modeling for Entity Framework Core as a Fody plugin.
### AnchorModeling.Fody
[![NuGet](https://img.shields.io/badge/nuget-0.9.5-blue)](https://www.nuget.org/packages/AnchorModeling.Fody/)
### AnchorModeling.Libraries
[![NuGet](https://img.shields.io/badge/nuget-0.9.5-blue)](https://www.nuget.org/packages/AnchorModeling.Libraries/)
### AnchorModeling.Extensions
[![NuGet](https://img.shields.io/badge/nuget-0.9.5-blue)](https://www.nuget.org/packages/AnchorModeling.Extensions/)

### Quick start tutorial

The example uses Visual Studio 2019

First you need to create a database model as in [this example](https://github.com/AscarGb/AnchorModelingFramework/tree/master/Examples/AnchorModelingExample/AnchorModelingExample.Models)

Next install the following packages:
```xml
<ItemGroup>
    <PackageReference Include="AnchorModeling.Fody" Version="0.9.5" />
    <PackageReference Include="AnchorModeling.Libraries" Version="0.9.5" />
    <PackageReference Include="Fody" Version="6.1.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="3.1.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="3.1.3" />   
  </ItemGroup>
```
Make sure the **FodyWeavers.xml** file is added:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Weavers xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="FodyWeavers.xsd">
  <AnchorModeling />
</Weavers>
```
Now rebuild the project. In the output you will see:
```
1>    Fody/AnchorModeling:   Constructing entities...
1>    Fody/AnchorModeling:   Take entity: User
1>    Fody/AnchorModeling:   Make ancor: A_Users
1>    Fody/AnchorModeling:   Make attribute: P_Users_Name
1>    Fody/AnchorModeling:   Take entity: Computer
1>    Fody/AnchorModeling:   Make ancor: A_Computers
1>    Fody/AnchorModeling:   Take entity: MotherBoard
1>    Fody/AnchorModeling:   Make ancor: A_MotherBoards
1>    Fody/AnchorModeling:   Make attribute: P_MotherBoards_Name
1>    Fody/AnchorModeling:   Take entity: Processor
1>    Fody/AnchorModeling:   Make ancor: A_Processors
1>    Fody/AnchorModeling:   Make attribute: P_Processors_Name
1>    Fody/AnchorModeling:   Take entity: RAM
1>    Fody/AnchorModeling:   Make ancor: A_RAMs
1>    Fody/AnchorModeling:   Make attribute: P_RAMs_Name
1>    Fody/AnchorModeling:   Take entity: SoundCard
1>    Fody/AnchorModeling:   Make ancor: A_SoundCards
1>    Fody/AnchorModeling:   Make attribute: P_SoundCards_Name
1>    Fody/AnchorModeling:   Take entity: VideoCard
1>    Fody/AnchorModeling:   Make ancor: A_VideoCards
1>    Fody/AnchorModeling:   Make attribute: P_VideoCards_Name
1>    Fody/AnchorModeling:   Make attribute: P_VideoCards_Bytes
1>    Fody/AnchorModeling:   Make tie: T_H_Users_HomeComputer_to_Computers
1>    Fody/AnchorModeling:   Make tie: T_H_Users_WorkComputer_to_Computers
1>    Fody/AnchorModeling:   Make tie: T_H_Computers_MotherBoard_to_MotherBoards
1>    Fody/AnchorModeling:   Make tie: T_H_Computers_Processor_to_Processors
1>    Fody/AnchorModeling:   Make tie: T_H_Computers_RAM1_to_RAMs
1>    Fody/AnchorModeling:   Make tie: T_H_Computers_RAM2_to_RAMs
1>    Fody/AnchorModeling:   Make tie: T_H_Computers_SoundCard_to_SoundCards
1>    Fody/AnchorModeling:   Make tie: T_H_Computers_VideoCard_to_VideoCards
```

These are the new types that have been added to the assembly.

Now create a [console application](https://github.com/AscarGb/AnchorModelingFramework/tree/master/Examples/AnchorModelingExample/AnchorModelingExample) and add a **link to the project with the model** and the **AnchorModeling.Libraries** package.

Then **unload** the project with the model

**When you insert data into your entities, the data will be automatically saved in the anchor model.**
[See an example](https://github.com/AscarGb/AnchorModelingFramework/blob/master/Examples/AnchorModelingExample/AnchorModelingExample/Program.cs).