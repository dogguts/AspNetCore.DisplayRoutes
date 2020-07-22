# AspNetCore.DisplayRoutes
View route information for AspNetCore projects.  
Given that routes are registered with the (new since 3.0) AspNetCore IEndpointRouteBuilder, like below, or through Attributes; 

```c#
  app.UseEndpoints(endpoints => { ...
```
## Screenshot

![Screenshot](./Screenshot.png)

## Packages
|   |  Stable   |   CI |
| - | -------------- | -------------- | 
| AspNetCore.DisplayRoutes | ![NuGet](https://img.shields.io/nuget/v/AspNetCore.DisplayRoutes?logoColor=%20) | [GPR](https://github.com/dogguts/AspNetCore.DisplayRoutes/packages/324648) |

## Usage
AspNetCode.DisplayRoutes can add an additional route to your application where, when visited, will report all registered routes in your application.

### Add a reference to the AspNetCore.DisplayRoutes nuget package in your (web) project:
```Powershell
#Package manager
Install-Package AspNetCore.DisplayRoutes
```
or
```sh
#CLI
dotnet add package AspNetCore.DisplayRoutes
```
For more options see [NuGet](https://www.nuget.org/packages/AspNetCore.DisplayRoutes/)

### Register PrintRoutes route
```C#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
  ...  
  app.UseEndpoints(endpoints => {
    ...  
    endpoints.PrintRoutes();
    ...  
```
This creates an additional route using the default/predefined options.
Or override the default options; 
```C#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
  ...  
  app.UseEndpoints(endpoints => {  
    ...  
    endpoints.PrintRoutes(o => {
        o.Renderer = new AspNetCore.DisplayRoutes.Render.HtmlRenderer();
        o.Pattern = "/myroutes";
    });
    ...  
```
### Options
|   |  Option   |   Default |
| - | -------------- | -------------- | 
|Pattern| The route path where the route report will be provided  | /routes |
|HttpMethods | Allowed HTTP methods for the registered route path | \["GET"] |
|Renderer | The format the routes will be presented | UnicodeBoxRenderer |

### Renderers
|   |     |   ContentType |
| - | -------------- | -------------- | 
| AsciiBoxRenderer |  ![AsciiBoxRenderer](./.github/images/AsciiBoxRenderer.png) | text/plain; charset=UTF-8 |
| HtmlRenderer | ![HtmlRenderer](./.github/images/HtmlRenderer.png) | text/html; charset=UTF-8 |
| JsonRenderer |  ![JsonRenderer](./.github/images/JsonRenderer.png) | application/json |
| UnicodeBoxRenderer |   ![UnicodeBoxRenderer](./.github/images/UnicodeBoxRenderer.png) | text/plain; charset=UTF-8 |
