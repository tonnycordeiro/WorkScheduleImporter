#pragma checksum "..\..\..\..\RetainedStretcher\DispatchableEventsRetainedStretcherWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "4EB6ACC3978FFA0BFF24F446A76FC8FA8C6261B7CA14A0F74198741F9CC418A5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Sisgraph.Ips.Samu.AddIn.Views.RetainedStretcher;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Sisgraph.Ips.Samu.AddIn.Views.RetainedStretcher
{


    /// <summary>
    /// DispatchableEventsRetainedStretcherWindow
    /// </summary>
    public partial class DispatchableEventsRetainedStretcherWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Sisgraph.Ips.Samu.AddIn.Views;component/retainedstretcher/dispatchableeventsreta" +
                    "inedstretcherwindow.xaml", System.UriKind.Relative);

#line 1 "..\..\..\..\RetainedStretcher\DispatchableEventsRetainedStretcherWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.Button btnRefresh;
        internal System.Windows.Controls.DataGrid grdDispatchableUnitWithReteinedStretcher;
        internal System.Windows.Controls.DataGridTextColumn gdcRetainedStretcherUnitId;
        internal System.Windows.Controls.DataGridTextColumn gdcRetainedStretcherStationName;
        internal System.Windows.Controls.DataGridTextColumn gdcRetainedStretcherUnitType;
        internal System.Windows.Controls.DataGridTextColumn gdcRetainedStretcherPagerId;
        internal System.Windows.Controls.DataGridTextColumn gdcRetainedStretcherUnitStatus;
        internal System.Windows.Controls.DataGridTextColumn gdcRetainedStretcherSpecialContact;
        internal System.Windows.Controls.DataGridTextColumn gdcRetainedStretcherDispatchGroup;
        internal System.Windows.Controls.DataGridTextColumn gdcRetainedStretcherStartRetainedStretcher;
        internal System.Windows.Controls.ContextMenu ctxtDispatchableUnitWithReteinedStretcher;
        internal System.Windows.Controls.MenuItem cmiDispatchUnit;
        internal System.Windows.Controls.MenuItem cmiNextStatus;
    }
}
