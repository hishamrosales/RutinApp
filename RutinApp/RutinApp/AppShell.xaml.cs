namespace RutinApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.AddEjercicioPage), typeof(Views.AddEjercicioPage));
            Routing.RegisterRoute(nameof(Views.AddRutinaPage), typeof(Views.AddRutinaPage));
        }
    }
}
