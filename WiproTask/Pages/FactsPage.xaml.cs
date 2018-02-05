using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WiproTask
{
    public partial class FactsPage : ContentPage
    {
        FactsPageViewModel viewModel;
        /// <summary>
        /// Gets the listview facts.
        /// </summary>
        /// <value>The listview facts.</value>
        public ListView listviewFacts { get { return listview; } }
        public FactsPage()
        {
            InitializeComponent();

            this.Title = "Wipro Task";
            /// <summary>
             /// Intialize The ViewModel 
            /// </summary>
            viewModel = new FactsPageViewModel(this);

            /// <summary>
            /// Assign ViewModel to Page
            /// </summary>
            BindingContext = viewModel;

        }
    }
}
