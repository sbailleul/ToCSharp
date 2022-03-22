# C# class generator

This project offer a simple way to generate classes definition from yaml and json files. Once you installed the tool you
can see available options with the following command :

````shell
class-generator --help
````

## Create tool

```shell
dotnet pack 
```

## Install tool

````shell
dotnet tool install --global --add-source ./nupkg ToCSharp.CLI
````

## Uninstall tool
````shell
dotnet tool uninstall ToCSharp.CLI --global 
````