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
        // public static Utilisateur? UtilisateurCourant { get; set; } // Removed

        protected override async void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            var connectionString = "server=localhost;port=3306;database=projet_restos_coeur;user=restos;password=restos123;SslMode=none;";
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)),
                ServiceLifetime.Scoped // Changed DbContext lifetime to Scoped
            );

            // Store Singleton
            services.AddSingleton<CentreStore>();

            // Service for current user (Singleton to hold state across the app)
            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            // Repos & Services

            // Repository générique (remplace tous les AddTransient<IRepository<X>, XRepository>())
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Services métiers
            services.AddScoped<FamilleService>();
            services.AddScoped<SousFamilleService>();
            services.AddScoped<ProduitCatalogueService>();

            services.AddScoped<IRepository<Collecte>, CollecteRepository>(); 
            services.AddScoped<CollecteService>(); // Changed to Scoped
            services.AddScoped<IRepository<Utilisateur>, UtilisateurRepository>(); 
            services.AddScoped<UtilisateurService>(); // Changed to Scoped
            services.AddScoped<IRepository<Magasin>, MagasinRepository>(); 
            services.AddScoped<MagasinService>(); // Changed to Scoped
            services.AddScoped<IRepository<Centre>, CentreRepository>(); 
            services.AddScoped<CentreService>(); // Changed to Scoped
            services.AddScoped<CollecteCentreService>(); // Changed to Scoped
            services.AddScoped<ICollecteCentreRepository, CollecteCentreRepository>(); 

            // Windows (vues principales)
            services.AddTransient<LoginWindow>();
            services.AddTransient<DashboardAdminWindow>();
            services.AddTransient<DashboardCentreWindow>();
            services.AddTransient<DashboardUtilisateurWindow>();

            // ViewModels Admin (navigation globale)
            services.AddTransient<CollecteViewModel>();
            services.AddTransient<CentresViewModel>();
            services.AddTransient<UtilisateursViewModel>();
            services.AddTransient<MagasinsViewModel>();
            services.AddTransient<MagasinsActivationViewModel>();
            services.AddTransient<DashboardAdminViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<AdminProduitViewModel>();
            // ViewModels Utilisateur (navigation locale)
            services.AddTransient<DashboardUtilisateurViewModel>(sp =>
                new DashboardUtilisateurViewModel(
                    sp.GetRequiredService<CollecteService>(),
                    sp.GetRequiredService<MagasinService>(),
                    sp.GetRequiredService<ICurrentUserService>() // IServiceProvider (sp) no longer passed
                )
            );
            services.AddTransient<CollecteUtilisateurViewModel>(); 
            services.AddTransient<ChoixMagasinViewModel>();     // Added registration

            // (Pas besoin d’injecter CollecteUtilisateurViewModel ou ChoixMagasinViewModel : ils sont instanciés à la main dans le parent) --> This comment is now outdated but will be kept for now.

            // Navigation globale (pour l’admin)
            services.AddSingleton<INavigationService, NavigationService>(provider =>
                new NavigationService(
                    () => provider.GetRequiredService<CollecteViewModel>(),
                    () => provider.GetRequiredService<CentresViewModel>(),
                    () => provider.GetRequiredService<UtilisateursViewModel>(),
                    () => provider.GetRequiredService<MagasinsViewModel>(),
                    () => provider.GetRequiredService<MagasinsActivationViewModel>(),
                     () => provider.GetRequiredService<AdminProduitViewModel>()
                )
            );

            // Navigation entre fenêtres principales
            services.AddSingleton<IWindowNavigationService, WindowNavigationService>(provider =>
                new WindowNavigationService(
                    () => provider.GetRequiredService<LoginWindow>(),
                    () => provider.GetRequiredService<DashboardAdminWindow>(),
                    () => provider.GetRequiredService<DashboardCentreWindow>(),
                    () => provider.GetRequiredService<DashboardUtilisateurWindow>()
                )
            );

            try
            {
                ServiceProvider = services.BuildServiceProvider(); // Ensure ServiceProvider is initialized before use.

                // Initialisation du CentreStore au démarrage
                await InitCentresStoreAsync();

                // Démarre l'UI
                var login = ServiceProvider.GetRequiredService<LoginWindow>();
                login.Show();
            }
            catch (Exception ex)
            {
                // Logique de gestion de l'exception
                // Pour l'instant, nous allons afficher un MessageBox et ensuite fermer l'application.
                // Dans une application réelle, on pourrait utiliser un logger plus sophistiqué.
                MessageBox.Show(
                    $"Une erreur critique est survenue au démarrage de l'application : {ex.Message}{Environment.NewLine}{ex.StackTrace}",
                    "Erreur de démarrage",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                // Envisager de fermer l'application proprement
                // Current.Shutdown(-1); 
            }

            base.OnStartup(e);
        }

        /// <summary>
        /// Charge la liste des centres dans le store partagé au démarrage
        /// </summary>
        private static async Task InitCentresStoreAsync()
        {
            // Créez un scope pour résoudre les services scopés
            using (var scope = ServiceProvider.CreateScope())
            {
                var centreService = scope.ServiceProvider.GetRequiredService<CentreService>();
                var centreStore = scope.ServiceProvider.GetRequiredService<CentreStore>(); // CentreStore est Singleton, donc ok de le résoudre ici aussi.
                var centres = await centreService.GetAllAsync();
                centreStore.SetCentres(centres);
            }
        }
    }
}
