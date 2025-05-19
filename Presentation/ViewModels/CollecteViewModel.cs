using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.ViewModels
{
    public class CollecteViewModel
    {
        private readonly CollecteService _service;
        public ObservableCollection<Collecte> Collectes { get; set; } = new();

        public CollecteViewModel(CollecteService service)
        {
            _service = service;
            Task.Run(LoadCollectesAsync);
        }

        public async Task LoadCollectesAsync()
        {
            var result = await _service.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Collectes.Clear();
                foreach (var collecte in result)
                    Collectes.Add(collecte);
            });
        }
    }
}
