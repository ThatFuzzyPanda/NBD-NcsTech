namespace NBDProject
{
    public class Startup
    {


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ... (other middleware configuration)

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
