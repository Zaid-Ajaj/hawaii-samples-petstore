open System
open System.Net.Http
open PetStore
open PetStore.Types

let petStoreUri = Uri "https://petstore3.swagger.io/api/v3"
let httpClient = new HttpClient(BaseAddress=petStoreUri)
let petStore = PetStoreClient(httpClient)

let availablePets() =
    let status = PetStatus.Available.Format()
    match petStore.findPetsByStatus(status) with
    | FindPetsByStatus.OK pets -> for pet in pets do printfn $"{pet.name}"
    | FindPetsByStatus.BadRequest -> printfn "Bad request"

availablePets()

// inventory : Map<string, int>
let (GetInventory.OK(inventory)) = petStore.getInventory()

for (status, quantity) in Map.toList inventory do
    printfn $"There are {quantity} pet(s) {status}"