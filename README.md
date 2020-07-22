# AspNetCore.DisplayRoutes
View route information for AspNetCore projects.  
Given that routes are registered with the (new since 3.0) AspNetCore IEndpointRouteBuilder, like below, or through Attributes; 

```c#
  app.UseEndpoints(endpoints => {
      // signalR hub
      endpoints.MapHub<Service.SignalR.NotificationHub>("/hubs/notification");

      // default route
      endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
      ...
```
## Screenshot

![Screenshot](./screenshot.png)
