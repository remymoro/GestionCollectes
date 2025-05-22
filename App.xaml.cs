using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GestionCollectes.Infrastructure.Data;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Repositories;
using GestionCollectes.ApplicationLayer.Services;



namespace GestionCollectes
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static GestionCollectes.Domain.Entities.Utilisateur? UtilisateurCourant { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            var connectionString = "server=localhost;port=3306;database=projet_restos_coeur;user=restos;password=restos123;SslMode=none;";
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Ajoute ici tous tes services avant BuildServiceProvider
            services.AddScoped<IRepository<Collecte>, CollecteRepository>();
            services.AddScoped<CollecteService>(); // <-- AJOUTE CETTE LIGNE !
            services.AddScoped<IRepository<Utilisateur>, UtilisateurRepository>();
            services.AddScoped<UtilisateurService>();



            services.AddScoped<IRepository<Centre>, CentreRepository>();
            services.AddScoped<CentreService>();

            // Ici tu construis le conteneur DI
            ServiceProvider = services.BuildServiceProvider();

            var login = new Presentation.Views.LoginWindow();
            login.Show();



            base.OnStartup(e);
        }










    }
}
