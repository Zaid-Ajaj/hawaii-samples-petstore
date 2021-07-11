namespace rec PetStore

open System.Net
open System.Net.Http
open System.Text
open PetStore.Types
open PetStore.Http

///This is a sample Pet Store Server based on the OpenAPI 3.0 specification.  You can find out more about
///Swagger at [http://swagger.io](http://swagger.io). In the third iteration of the pet store, we've switched to the design first approach!
///You can now help us improve the API whether it's by making changes to the definition itself or to the code.
///That way, with time, we can improve the API in general, and expose some of the new features in OAS3.
///Some useful links:
///- [The Pet Store repository](https://github.com/swagger-api/swagger-petstore)
///- [The source API definition for the Pet Store](https://github.com/swagger-api/swagger-petstore/blob/master/src/main/resources/openapi.yaml)
type PetStoreClient(httpClient: HttpClient) =
    ///<summary>
    ///Update an existing pet by Id
    ///</summary>
    member this.updatePet(body: Pet) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/pet" requestParts

        if status = HttpStatusCode.OK then
            UpdatePet.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            UpdatePet.BadRequest
        else if status = HttpStatusCode.NotFound then
            UpdatePet.NotFound
        else
            UpdatePet.MethodNotAllowed

    ///<summary>
    ///Add a new pet to the store
    ///</summary>
    member this.addPet(body: Pet) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/pet" requestParts

        if status = HttpStatusCode.OK then
            AddPet.OK(Serializer.deserialize content)
        else
            AddPet.MethodNotAllowed

    ///<summary>
    ///Multiple status values can be provided with comma separated strings
    ///</summary>
    ///<param name="status">Status values that need to be considered for filter</param>
    member this.findPetsByStatus(?status: string) =
        let requestParts =
            [ if status.IsSome then
                  RequestPart.query ("status", status.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/pet/findByStatus" requestParts

        if status = HttpStatusCode.OK then
            FindPetsByStatus.OK(Serializer.deserialize content)
        else
            FindPetsByStatus.BadRequest

    ///<summary>
    ///Multiple tags can be provided with comma separated strings. Use tag1, tag2, tag3 for testing.
    ///</summary>
    ///<param name="tags">Tags to filter by</param>
    member this.findPetsByTags(?tags: list<string>) =
        let requestParts =
            [ if tags.IsSome then
                  RequestPart.query ("tags", tags.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/pet/findByTags" requestParts

        if status = HttpStatusCode.OK then
            FindPetsByTags.OK(Serializer.deserialize content)
        else
            FindPetsByTags.BadRequest

    ///<summary>
    ///Returns a single pet
    ///</summary>
    ///<param name="petId">ID of pet to return</param>
    member this.getPetById(petId: int64) =
        let requestParts = [ RequestPart.path ("petId", petId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/pet/{petId}" requestParts

        if status = HttpStatusCode.OK then
            GetPetById.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetPetById.BadRequest
        else
            GetPetById.NotFound

    ///<summary>
    ///Updates a pet in the store with form data
    ///</summary>
    ///<param name="petId">ID of pet that needs to be updated</param>
    ///<param name="name">Name of pet that needs to be updated</param>
    ///<param name="status">Status of pet that needs to be updated</param>
    member this.updatePetWithForm(petId: int64, ?name: string, ?status: string) =
        let requestParts =
            [ RequestPart.path ("petId", petId)
              if name.IsSome then
                  RequestPart.query ("name", name.Value)
              if status.IsSome then
                  RequestPart.query ("status", status.Value) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/pet/{petId}" requestParts

        if status = HttpStatusCode.MethodNotAllowed then
            UpdatePetWithForm.MethodNotAllowed
        else
            UpdatePetWithForm.DefaultResponse

    ///<summary>
    ///Deletes a pet
    ///</summary>
    ///<param name="petId">Pet id to delete</param>
    ///<param name="apiKey"></param>
    member this.deletePet(petId: int64, ?apiKey: string) =
        let requestParts =
            [ RequestPart.path ("petId", petId)
              if apiKey.IsSome then
                  RequestPart.header ("api_key", apiKey.Value) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/pet/{petId}" requestParts

        if status = HttpStatusCode.BadRequest then
            DeletePet.BadRequest
        else
            DeletePet.DefaultResponse

    ///<summary>
    ///uploads an image
    ///</summary>
    ///<param name="petId">ID of pet to update</param>
    ///<param name="additionalMetadata">Additional Metadata</param>
    ///<param name="requestBody"></param>
    member this.uploadFile(petId: int64, ?additionalMetadata: string, ?requestBody: byte []) =
        let requestParts =
            [ RequestPart.path ("petId", petId)
              if additionalMetadata.IsSome then
                  RequestPart.query ("additionalMetadata", additionalMetadata.Value)
              if requestBody.IsSome then
                  RequestPart.binaryContent requestBody.Value ]

        let (status, content) =
            OpenApiHttp.post httpClient "/pet/{petId}/uploadImage" requestParts

        UploadFile.OK(Serializer.deserialize content)

    ///<summary>
    ///Returns a map of status codes to quantities
    ///</summary>
    member this.getInventory() =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/store/inventory" requestParts

        GetInventory.OK(Serializer.deserialize content)

    ///<summary>
    ///Place a new order in the store
    ///</summary>
    member this.placeOrder(body: Order) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/store/order" requestParts

        if status = HttpStatusCode.OK then
            PlaceOrder.OK(Serializer.deserialize content)
        else
            PlaceOrder.MethodNotAllowed

    ///<summary>
    ///For valid response try integer IDs with value &amp;lt;= 5 or &amp;gt; 10. Other values will generated exceptions
    ///</summary>
    ///<param name="orderId">ID of order that needs to be fetched</param>
    member this.getOrderById(orderId: int64) =
        let requestParts =
            [ RequestPart.path ("orderId", orderId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/store/order/{orderId}" requestParts

        if status = HttpStatusCode.OK then
            GetOrderById.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetOrderById.BadRequest
        else
            GetOrderById.NotFound

    ///<summary>
    ///For valid response try integer IDs with value &amp;lt; 1000. Anything above 1000 or nonintegers will generate API errors
    ///</summary>
    ///<param name="orderId">ID of the order that needs to be deleted</param>
    member this.deleteOrder(orderId: int64) =
        let requestParts =
            [ RequestPart.path ("orderId", orderId) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/store/order/{orderId}" requestParts

        if status = HttpStatusCode.BadRequest then
            DeleteOrder.BadRequest
        else if status = HttpStatusCode.NotFound then
            DeleteOrder.NotFound
        else
            DeleteOrder.DefaultResponse

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    member this.createUser(body: User) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/user" requestParts

        CreateUser.DefaultResponse(Serializer.deserialize content)

    ///<summary>
    ///Creates list of users with given input array
    ///</summary>
    member this.createUsersWithListInput(body: CreateUsersWithListInputPayload) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/user/createWithList" requestParts

        if status = HttpStatusCode.OK then
            CreateUsersWithListInput.OK(Serializer.deserialize content)
        else
            CreateUsersWithListInput.DefaultResponse

    ///<summary>
    ///Logs user into the system
    ///</summary>
    ///<param name="username">The user name for login</param>
    ///<param name="password">The password for login in clear text</param>
    member this.loginUser(?username: string, ?password: string) =
        let requestParts =
            [ if username.IsSome then
                  RequestPart.query ("username", username.Value)
              if password.IsSome then
                  RequestPart.query ("password", password.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/user/login" requestParts

        if status = HttpStatusCode.OK then
            LoginUser.OK content
        else
            LoginUser.BadRequest

    ///<summary>
    ///Logs out current logged in user session
    ///</summary>
    member this.logoutUser() =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/user/logout" requestParts

        LogoutUser.DefaultResponse

    ///<summary>
    ///Get user by user name
    ///</summary>
    ///<param name="username">The name that needs to be fetched. Use user1 for testing. </param>
    member this.getUserByName(username: string) =
        let requestParts =
            [ RequestPart.path ("username", username) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/user/{username}" requestParts

        if status = HttpStatusCode.OK then
            GetUserByName.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetUserByName.BadRequest
        else
            GetUserByName.NotFound

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    ///<param name="username">name that need to be deleted</param>
    ///<param name="body"></param>
    member this.updateUser(username: string, body: User) =
        let requestParts =
            [ RequestPart.path ("username", username)
              RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/user/{username}" requestParts

        UpdateUser.DefaultResponse

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    ///<param name="username">The name that needs to be deleted</param>
    member this.deleteUser(username: string) =
        let requestParts =
            [ RequestPart.path ("username", username) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/user/{username}" requestParts

        if status = HttpStatusCode.BadRequest then
            DeleteUser.BadRequest
        else if status = HttpStatusCode.NotFound then
            DeleteUser.NotFound
        else
            DeleteUser.DefaultResponse
