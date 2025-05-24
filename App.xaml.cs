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
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)),
                ServiceLifetime.Transient // Important !
            );

            // Ajoute ici tous tes services avant BuildServiceProvider
            // Enregistre tout en Transient
            services.AddTransient<IRepository<Collecte>, CollecteRepository>();
            services.AddTransient<CollecteService>();
            services.AddTransient<IRepository<Utilisateur>, UtilisateurRepository>();
            services.AddTransient<UtilisateurService>();
            services.AddTransient<IRepository<Magasin>, MagasinRepository>();
            services.AddTransient<MagasinService>();
            services.AddTransient<IRepository<Centre>, CentreRepository>();
            services.AddTransient<CentreService>();

            // ViewModels utilisateur
            services.AddTransient<GestionCollectes.Presentation.ViewModels.Utilisateurs.DashboardUtilisateurViewModel>();
            services.AddTransient<GestionCollectes.Presentation.ViewModels.Utilisateurs.CollecteUtilisateurViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            // Ici tu construis le conteneur DI
            ServiceProvider = services.BuildServiceProvider();

            var login = new Presentation.Views.LoginWindow();
            login.Show();



            base.OnStartup(e);
        }










    }
}
