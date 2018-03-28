using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using EventList.Pages.Chat;
using MvvmHelpers;
using Realms.Sync;
using Xamarin.Forms;
namespace EventList.ViewModels
{
    public class QRCodeViewModel : ViewModelBase
    {
        public ObservableRangeCollection<QRCode> Codes { get; } = new ObservableRangeCollection<QRCode>();
        public string SelectedQRCode { get; set; }
        public string BarCodeText
        {
            get;
            set;
        }
        public QRCodeViewModel(INavigation navigation) : base(navigation)
        {
        }
        ICommand loadqrcodescommand;
        public ICommand LoadQRCodesCommand => loadqrcodescommand ?? (loadqrcodescommand = new Command(async () => await ExecuteLoadQRCodesCommand()));

        async Task ExecuteLoadQRCodesCommand()
        {
            try
            {
                var qrcodes = App._Realm.All<QRCode>().ToList();
                Codes.ReplaceRange(qrcodes);

            }
            catch (Exception ex)
            {

            }
        }
        ICommand addqrcodescommand;
        public ICommand AddQRCodesCommand => addqrcodescommand ?? (addqrcodescommand = new Command<string>(async (code) => await ExecuteAddQRCodesCommand(code)));

        async Task ExecuteAddQRCodesCommand(string codetext)
        {
            try
            {
                App._Realm.Write(() =>
               {
                    App._Realm.Add(new QRCode() { Code = codetext });
               });
                Codes.Add(new QRCode() { Code = codetext });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
