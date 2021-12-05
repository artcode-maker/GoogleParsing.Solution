using System.ComponentModel;
using System.Runtime.CompilerServices;
using GoogleParsing.Models;
using GoogleParsing.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleParsing.ViewModels
{
    public class MainPageViewModel :INotifyPropertyChanged
    {
        private ObservableCollection<Reference> references;
        private RelayCommand searchCommand;

        public ObservableCollection<Reference> References
        {
            get => references;
            set
            {
                references = value;
                OnPropertyChanged(nameof(References));
            }
        }


        public RelayCommand SearchCommand
        {
            get
            {
                return searchCommand ??
                  (searchCommand = new RelayCommand(async () =>
                  {
                      Reference refer = new Reference("");
                      await refer.GoToGoogleAsync();
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        public MainPageViewModel()
        {
            References = new ObservableCollection<Reference>
            { };
        }
    }
}
