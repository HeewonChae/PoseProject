using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using PoseSportsPredict.Views.Football;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football
{
    public class ItemDetailViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override Task<bool> PrepareView(params object[] data)
        {
            if (data[0] != null && data[0] is Item item)
            {
                Item = item;
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        #endregion BaseViewModel

        #region Fields

        public Item _item;

        #endregion Fields

        #region Properties

        public Item Item { get => _item; set => SetValue(ref _item, value); }

        #endregion Properties

        #region Constructors

        public ItemDetailViewModel(ItemDetailPage page) : base(page)
        {
        }

        #endregion Constructors
    }
}