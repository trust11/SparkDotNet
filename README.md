# SparkDotNet

An unofficial dotnet library for consuming RESTful APIs for Cisco Spark. Please visit Cisco at https://developer.webex.com/.

```{.cs}
using SparkDotNet;

var token = System.Environment.GetEnvironmentVariable("SPARK_TOKEN");
var spark = new Spark(token);
var rooms = await spark.GetRoomsAsync(max: 10).ConfigureAwait(false);
foreach (var room in rooms)
{
    WriteLine(room);
}
```

# Installation

## Prerequisites

This library requires .NET 4.5, .NET 4.6.1 or .NET Standard 1.6.

It can be used across multiple platforms using .NET Core and you will therefore require the appropriate components based on your choice of environment.

* [.NET Core for Windows and Visual Studio](https://www.microsoft.com/net/core#windowsvs2015)
* [.NET Core for Windows Command Line](https://www.microsoft.com/net/core#windowscmd)
* [.NET Core for Mac](https://www.microsoft.com/net/core#macos)
* [.NET Core for Linux](https://www.microsoft.com/net/core#linuxredhat)

You can also use Visual Studio 2015 and .NET 4.6.1 or .Net 4.5.

The following assumes you have an existing project to which you wish to add the library.

### .NET Core

1. Edit your `<application>.csproj` file to include `SparkDotNet` as a dependency.  

2. Run `dotnet restore`.

3. There is no step 3.

The following shows an example application.csproj file:

```{.csproj}
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp1.1</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="SparkDotNet" Version="1.2.0" />
  </ItemGroup>
</Project>
```

For previous versions of dotnet core, the following shows an example project.json file:

```{.json}
{
  "version": "1.0.0-*",
  "buildOptions": {
    "debugType": "portable",
    "emitEntryPoint": true
  },
  "dependencies": {
      "SparkDotNet": "1.0.4"
  },
  "frameworks": {
    "netcoreapp1.0": {
      "dependencies": {
        "Microsoft.NETCore.App": {
          "type": "platform",
          "version": "1.0.1"
        }
      },
      "imports": "dnxcore50"
    }
  }
}
```



### Windows Visual Studio

1. Open Tools -> NuGet Package Manager -> Package Manager Console

2. Run `Install-Package SparkDotNet`

3. There is no step 3.


# Using the library

1. Create a new project 

> `dotnet new console`

2. Include the `SparkDotNet` dependency  

> `dotnet add package SparkDotNet`  
> `dotnet restore`

3. Edit `Program.cs` to resemble the following:

```{.cs}
using static System.Console;
using SparkDotNet;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(List<string> args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            var token = System.Environment.GetEnvironmentVariable("SPARK_TOKEN");
            var spark = new Spark(token);

            try
            {
                var orgs = await spark.GetOrganizationsAsync().ConfigureAwait(false);
                foreach (var org in orgs)
                {
                    WriteLine(org);
                }

                WriteLine(await spark.GetMeAsync().ConfigureAwait(false));
            }
            catch (SparkException ex)
            {
                WriteLine(ex.Message);
            }
        }
    }
}

```
4. Run the Program

> `set SPARK_TOKEN=<your token here`  
> `dotnet run`

# Reference

There are 33 endpoints covered by this library. Please refer to the Cisco documentation for details of their use:

* [Admin Audit Events](https://developer.webex.com/docs/api/v1/admin-audit-events)
* [Attachment Actions](https://developer.webex.com/docs/api/v1/attachment-actions)
* [Call Controls](https://developer.webex.com/docs/api/v1/call-controls)
* [Devices](https://developer.webex.com/docs/api/v1/devices)
* [Events](https://developer.webex.com/docs/api/v1/events)
* [Hybrid Clusters](https://developer.webex.com/docs/api/v1/hybrid-clusters)
* [Hybrid Connectors](https://developer.webex.com/docs/api/v1/hybrid-connectors)
* [Licenses](https://developer.webex.com/docs/api/v1/licenses)
* [Locations](https://developer.webex.com/docs/api/v1/locations)
* [Meeting Invitees](https://developer.webex.com/docs/api/v1/meeting-invitees)
* [Meeting Participants](https://developer.webex.com/docs/api/v1/meeting-participants)
* [Meeting Preferences](https://developer.webex.com/docs/api/v1/meeting-preferences)
* [Meeting Qualities](https://developer.webex.com/docs/api/v1/meeting-qualities)
* [Meetings](https://developer.webex.com/docs/api/v1/meetings)
* [Memberships](https://developer.webex.com/docs/api/v1/memberships)
* [Messages](https://developer.webex.com/docs/api/v1/messages)
* [Organizations (including XSI)](https://developer.webex.com/docs/api/v1/organizations)
* [People](https://developer.webex.com/docs/api/v1/people)
* [Places](https://developer.webex.com/docs/api/v1/places)
* [Recordings](https://developer.webex.com/docs/api/v1/recordings)
* [Report Templates](https://developer.webex.com/docs/api/v1/report-templates)
* [Reports](https://developer.webex.com/docs/api/v1/reports)
* [Resource Group Membership](https://developer.webex.com/docs/api/v1/resource-group-memberships)
* [Resource Groups](https://developer.webex.com/docs/api/v1/resource-groups)
* [Roles](https://developer.webex.com/docs/api/v1/roles)
* [Rooms Tabs](https://developer.webex.com/docs/api/v1/room-tabs)
* [Rooms](https://developer.webex.com/docs/api/v1/rooms)
* [Space Classifications](https://developer.webex.com/docs/api/v1/space-classifications)
* [Team Mebership](https://developer.webex.com/docs/api/v1/team-memberships)
* [Teams](https://developer.webex.com/docs/api/v1/teams)
* [Webhooks](https://developer.webex.com/docs/api/v1/webhooks)
* [Workspaces](https://developer.webex.com/docs/api/v1/workspaces)
* [xAPI](https://developer.webex.com/docs/api/v1/xapi)

Most endpoints have a corresponding method in the Spark class mapping to `Get`, `Create`, `Update` and `Delete` based on the HTTP method used, `GET`, `POST`, `PUT` and `DELETE` respectively.  As an example, using the People endpoint, these map as follows:

| Cisco Endpoint | HTTP Method | Spark Class Method |
| -------------- |:-----------:|:-------------------|
| [List People](https://developer.webex.com/docs/api/v1/people/list-people)| GET |  GetPeopleAsync() | 
| [Create a Person](https://developer.webex.com/docs/api/v1/people/create-a-person)| POST | CreatePersonAsync() | 
| [Get Person Details](https://developer.webex.com/docs/api/v1/people/get-person-details)| GET | GetPersonAsync() | 
| [Update a Person](https://developer.webex.com/docs/api/v1/people/update-a-person)   | PUT | UpdatePersonAsync() | 
| [Delete a Person](https://developer.webex.com/docs/api/v1/people/delete-a-person)| DELETE | DeletePersonAsync() |
| [Get My Own Details](https://developer.webex.com/docs/api/v1/people/get-my-own-details)| GET | GetMeAsync() |

Where parameters are optional, they are also optional in the class methods.  These are variables that have default values specified. As usual, you must specify any required variables in the correct order before specifying any optional variables.

As an example, the `max` parameter is typically optional and as such you do not need to specify it.  If you wish to specify it, you can use the named argument syntax as shown below:

```
spark.GetRoomsAsync(max: 400);
```

Where a parameter requires a string array, these can be specified as follows:

```
spark.UpdatePersonAsync(id,new List<string> {"someone@example.com"},orgId, new List<string> {role}, name);
```

Most methods return an object of the type you are retrieving or updating.  Methods where you expect multiple objects are returned as a List<> of that object.  The exception to this is when deleting an object where the returned value is a boolean indicating the success of the operation.

# Files

As of version 1.2.0, when posting files in messages using `CreateMessageAsync()` you now have the option of sending local files.

You could already send files available publicly using the existing `files` object:

`var message = await spark.CreateMessageAsync(roomId, text:"Here is the logo you wanted.", files:new List<string> {"https://developer.cisco.com/images/mobility/Spark.png"} );`

To upload a local file, you simple replace the URI with a local filename:

`var message = await spark.CreateMessageAsync(roomId, text:"Here is the logo you wanted.", files:new List<string> {"Spark.png"} );`

This example assumes the `Spark.png` file is in the same location you are running the application. 

Note that you can only send a single file at a time using this method, even though the parameter accepts a string array.


# Pagination

An initial version of pagination has been added to provide support for Cisco Spark API as outlined on [the Cisco Spark Developer Portal](https://developer.webex.com/docs/api/basics#pagination).  

For backwards compatibility, this has been added as a separate method (`GetItemsWithLinksAsync`) which you must use over and above those specific to each resource if you specifically want to use pagination.  

This method returns a new `PaginationResult` class which contains the `Items` of the type you requested and `Links` to any additional resources for `First`, `Prev` and `Next`.

The following provides an example of returning `Person` items with pagination:

```{.cs}
var result = await spark.GetItemsWithLinksAsync<SparkDotNet.Person>("/v1/people?max=3").ConfigureAwait(false);
foreach (var person in result.Items)
{
    WriteLine(person);
}
if (!string.IsNullOrEmpty(result.Links.Next))
{
    var result2 = await spark.GetItemsWithLinksAsync<SparkDotNet.Person>(result.Links.Next).ConfigureAwait(false);
    foreach (var person in result2.Items)
    {
        WriteLine(person);
    }
}
```

# Exceptions

As of version 1.1.0, all calls to the Cisco Spark platform will throw a `SparkException` if an unexpected result is received. 

To avoid runtime errors, wrap calls to Cisco Spark in a `try-catch` statement:

```{.cs}
try
{
    WriteLine(await spark.GetMeAsync().ConfigureAwait(false));    
}
catch (SparkException ex)
{
    WriteLine(ex.Message)
}
```

`SparkException` provides the following properties:

* `Message` (`string`)
* `StatusCode` : (`int`)
* `Headers` : (`System.Net.Http.Headers.HttpResponseHeaders`)

The `Headers` property is useful since this will provide the Cisco `Trackingid` for troubleshooting purposes.

```{.cs}
using System.Linq;

try
{
    WriteLine(await spark.GetMeAsync().ConfigureAwait(false));    
}
catch (SparkException ex)
{
    WriteLine(ex.Headers.GetValues("Trackingid").FirstOrDefault());
}
```

# Adaptive Cards

As of version 1.2.6, support has been added for the attachments option when creating messages.  This enables the use of Adaptive Cards as per the [Cisco Documentation](https://developer.webex.com/docs/api/guides/cards).

The simplest way to use this feature is to use the [Microsoft Adaptive Cards package](https://docs.microsoft.com/en-us/adaptive-cards/sdk/authoring-cards/net):

```{.sh-session}
dotnet add package AdaptiveCards
```

Then in your code:

```{.cs}
AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
card.Body.Add(new AdaptiveTextBlock()
{
    Text = "Hello",
    Size = AdaptiveTextSize.ExtraLarge
});

card.Body.Add(new AdaptiveImage()
{
    Url = new Uri("http://adaptivecards.io/content/cats/1.png")
});

string json = card.ToJson();
var webexTeamsAttachment = "{\"contentType\": \"application/vnd.microsoft.card.adaptive\",\"content\":" + json + "}";

var res = await spark.CreateMessageAsync(roomId: RoomID, text: "Test Message", attachments: webexTeamsAttachment).ConfigureAwait(false);
```

The content is expected to be JSON as a string.

# Attachment Actions

As of version 1.2.7, support has been added (with thanks to @mayankmaxsharma) for the Attachment Actions endpoint in order to retrieve attachment action details.
