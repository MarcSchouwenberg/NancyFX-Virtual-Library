namespace NancyApplication
{
    using Nancy;

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", args => View["Home"]);
        }
    }
}
