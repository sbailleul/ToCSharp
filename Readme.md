# C# class generator

This project offer a simple way to generate classes definition from yaml and json files. Once you installed the tool you
can see available options with the following command :

## Global
````shell
to-csharp --help
````
## Local
````shell
dotnet tool run to-csharp -- --help
````

## Create tool

```shell
 dotnet pack  ./ToCSharp.CLI
```

## Install tool

### Local
Init your manifest if it's not done yet:
````shell
dotnet new tool-manifest
````
````shell
dotnet tool install  --add-source ./nupkg ToCSharp.CLI
````
### Global
````shell
dotnet tool install --global --add-source ./nupkg ToCSharp.CLI
````

## Uninstall tool
If your installation is local go to your manifest init directory
````shell
dotnet tool uninstall ToCSharp.CLI 
````