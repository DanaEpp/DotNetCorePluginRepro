# Introduction
This project demonstrates an issue when using the [DotNetCorePlugins package](https://github.com/natemcmaster/DotNetCorePlugins) from Nate McMaster.

In this simple example we have three projects:

1. The Plugin Framework (*TestPluginFramework*)
2. A Plugin (*TestPlugin*)
3. A console app (*TestPluginConsole*)

In the plugin we want to have access to useful third party libraries like those in the Azure management namespace. (ie: *StorageManagementClient*). When we do this from within the plugin though, we get an unexpected error during initialization of the storage client. The exception is:

```
System.ArrayTypeMismatchException: Attempted to access an element as a type incompatible with the array.
   at System.Collections.Generic.List`1.AddWithResize(T item)
   at Microsoft.Azure.Management.Storage.StorageManagementClient.Initialize()
   at Microsoft.Azure.Management.Storage.StorageManagementClient..ctor(TokenCredentials credentials, DelegatingHandler[] handlers)
   at TestPlugin.ListAsync(TokenCredentials creds, String subscriptionId)

```

If we run the exact same code outside of the plugin and in the main program, it all works fine. 

**THE ISSUE**: The question becomes... what does the loader do the the assembly that makes the Azure client fail like that?

## Setup to reproduce
You will need to have a working Azure subscription to test this live, and access to Azure AD to generate an enterprise application to get a client ID & app key. More information on how to create an app service principal [can be found here](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal).

You will then need to make the following changes:

1. Add your **subscription id** to `Program.cs` in the `TestPluginConsole` project
2. Add your **tenant** in `AzureResourceManager.cs` in the `TestPluginFramework` project
3. Add your **client id** and **app key** generated with Azure AD in `AzureResourceManager.cs` in the `TestPluginFramework` project
4. Add a breakpoint in the catch block of `TestPlugin.cs`
5. Create a **Plugins** folder at `TestPluginConsole\bin\Debug\netcoreapp2.1\Plugins`
6. Build and run the program in debug

If anyone has any ideas to why this is failing, please let me know. 
 
