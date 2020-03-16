using PoseSportsPredict.Models;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Test;

namespace PoseSportsPredict.ViewModels.Test
{
    public class ItemDetailViewModel : NavigableViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            base.OnInitializeView(datas);

            if (datas[0] != null && datas[0] is Item item)
            {
                Item = item;
                return true;
            }

            return false;
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