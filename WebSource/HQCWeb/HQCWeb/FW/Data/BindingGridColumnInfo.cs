using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HQCWeb.FW.Data
{
    public class BindingGridColumnInfo
    {

        public BindingGridColumnInfo() { }
        public string ColumnName { get; set; } = string.Empty;
        public bool WithoutEditAction { get; set; } = false;
        public bool Visibie { get; set; } = true;
        public bool EnableEditForCreate { get; set; } = false;
    }
}
