# Hawaii - PetStore sample

This repository shows an example of using [Hawaii](https://github.com/Zaid-Ajaj/Hawaii) to generate a type-safe F# client for the standard [PetStore API](https://petstore3.swagger.io/) and using that client from a simple console application.

### Run the sample
```
dotnet restore
dotnet run
```
### Regenrate the PetStore client
install `hawaii` locally on your machine:
```
dotnet tool install hawaii -g
```
Then run the following in the root directory
```
hawaii
```