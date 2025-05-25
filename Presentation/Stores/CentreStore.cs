using GestionCollectes.Domain.Entities;
using System.Collections.ObjectModel;

namespace GestionCollectes.Presentation.Stores
{
    public class CentreStore
    {

        
        public ObservableCollection<Centre> Centres { get; private set; } = new();

        public event EventHandler? CentresChanged;

      
        // Liste observable partagée partout

        // 🔥 Événement notifié quand la liste change (ajout/suppression)

        // Appelle ceci après chaque modification majeure !
        public void OnCentresChanged()
        {
            CentresChanged?.Invoke(this, EventArgs.Empty);
        }

        // Pour initialiser (ex : après GetAllAsync de CentreService)
        public void SetCentres(IEnumerable<Centre> centres)
        {
            Centres.Clear();
            foreach (var c in centres)
                Centres.Add(c);
            OnCentresChanged();
        }

        // (Optionnel) Pour ajouter un centre dans le store et notifier :
        public void AddCentre(Centre centre)
        {
            Centres.Add(centre);
            OnCentresChanged();
        }

        // (Optionnel) Pour retirer un centre du store :
        public void RemoveCentre(Centre centre)
        {
            Centres.Remove(centre);
            OnCentresChanged();
        }
    }
}
