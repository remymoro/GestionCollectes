using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GestionCollectes.Infrastructure.Data;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Repositories;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Presentation.Stores;
using GestionCollectes.Presentation.Navigations;
using GestionCollectes.Presentation.ViewModels.Admin;
using GestionCollectes.Presentation.ViewModels.Utilisateurs;
using GestionCollectes.Presentation.Navigation;
using GestionCollectes.Presentation.Views.Admin;
using GestionCollectes.Presentation.Views.Centre;
using GestionCollectes.Presentation.Views.Utilisateurs;
using GestionCollectes.Presentation.Views;
using GestionCollectes.Presentation.ViewModels;

namespace GestionCollectes
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static Utilisateur? UtilisateurCourant { get; set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            var connectionString = "server=localhost;port=3306;database=projet_restos_coeur;user=restos;password=restos123;SslMode=none;";
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)),
                ServiceLifetime.Transient
            );

            // Store Singleton
            services.AddSingleton<CentreStore>();

            // Repos & Services (en Transient)
            services.AddTransient<IRepository<Collecte>, CollecteRepository>();
            services.AddTransient<CollecteService>();
            services.AddTransient<IRepository<Utilisateur>, UtilisateurRepository>();
            services.AddTransient<UtilisateurService>();
            services.AddTransient<IRepository<Magasin>, MagasinRepository>();
            services.AddTransient<MagasinService>();
            services.AddSingleton<IRepository<Centre>, CentreRepository>();
            services.AddSingleton<CentreService>();
            services.AddTransient<CollecteCentreService>();
            services.AddTransient<IRepository<CollecteCentre>, CollecteCentreRepository>();


            services.AddTransient<LoginWindow>();
            services.AddTransient<DashboardAdminWindow>();
            services.AddTransient<DashboardCentreWindow>();
            services.AddTransient<DashboardUtilisateurWindow>();





            // ViewModels Admin
            services.AddTransient<CollecteViewModel>();
            services.AddTransient<CentresViewModel>();
            services.AddTransient<UtilisateursViewModel>();
            services.AddTransient<MagasinsViewModel>();
            services.AddTransient<MagasinsActivationViewModel>();
            services.AddTransient<DashboardAdminViewModel>();
            services.AddTransient<LoginViewModel>();

            // ViewModels Utilisateur
            services.AddTransient<DashboardUtilisateurViewModel>();
            services.AddTransient<CollecteUtilisateurViewModel>();
            // Ajoute ici MagasinSelectionViewModel ou autres ViewModels utilisateur si besoin




            // Ajoute aussi CollecteService (si pas déjà présent)
            services.AddTransient<CollecteService>();

            // Injection du ViewModel ParticipationCollecteMagasin avec tous ses services
           



            // INavigationService pour Admin (et Utilisateur si partagé)
            services.AddSingleton<INavigationService, NavigationService>(provider =>
                new NavigationService(
                    () => provider.GetRequiredService<CollecteViewModel>(),
                    () => provider.GetRequiredService<CentresViewModel>(),
                    () => provider.GetRequiredService<UtilisateursViewModel>(),
                    () => provider.GetRequiredService<MagasinsViewModel>(),
                    () => provider.GetRequiredService<MagasinsActivationViewModel>()
                )
            );





            services.AddSingleton<IWindowNavigationService, WindowNavigationService>(provider =>
                        new WindowNavigationService(
                            () => provider.GetRequiredService<LoginWindow>(),
                            () => provider.GetRequiredService<DashboardAdminWindow>(),
                            () => provider.GetRequiredService<DashboardCentreWindow>(),
                            () => provider.GetRequiredService<DashboardUtilisateurWindow>()
                        )
                    );


            ServiceProvider = services.BuildServiceProvider();

            // Initialisation du CentreStore au démarrage
            await InitCentresStoreAsync();

            // Démarre l'UI
            var login = new Presentation.Views.LoginWindow();
            login.Show();

            base.OnStartup(e);
        }

        /// <summary>
        /// Charge la liste des centres dans le store partagé au démarrage
        /// </summary>
        private static async Task InitCentresStoreAsync()
        {
            var centreService = ServiceProvider.GetRequiredService<CentreService>();
            var centreStore = ServiceProvider.GetRequiredService<CentreStore>();
            var centres = await centreService.GetAllAsync();
            centreStore.SetCentres(centres);
        }
    }
}
