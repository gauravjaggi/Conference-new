using System;
using System.Threading.Tasks;
using EventList.Models;
using EventList.ViewModels;

namespace EventList.Interfaces
{
    public interface IInputAlert
    {
        Task Show(FeedViewModel vm);

        string GetSavedRealmSetting();
    }
}
