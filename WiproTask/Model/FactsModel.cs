using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WiproTask
{
    public class FactsModel
    {
        public string title { get; set; }
        public ObservableCollection<Row> rows { get; set; }
    }
    public class Row
    {
        public string title { get; set; }
        public string description { get; set; }
        public string imageHref { get; set; }
    }
}
