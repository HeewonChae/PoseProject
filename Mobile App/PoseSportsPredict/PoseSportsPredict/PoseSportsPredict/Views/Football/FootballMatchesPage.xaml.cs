﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMatchesPage : TabbedPage
    {
        public FootballMatchesPage()
        {
            InitializeComponent();

            this.CurrentPage = today;
        }
    }
}