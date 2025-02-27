﻿// Decompiled with JetBrains decompiler
// Type: GumpStudio.ClilocPropEditor
// Assembly: GumpStudioCore, Version=1.8.3024.24259, Culture=neutral, PublicKeyToken=null
// MVID: A77D32E5-7519-4865-AA26-DCCB34429732
// Assembly location: C:\GumpStudio_1_8_R3_quinted-02\GumpStudioCore.dll

using GumpStudio.Forms;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GumpStudio
{
    public class ClilocPropEditor : UITypeEditor
    {
        protected IWindowsFormsEditorService edSvc;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            }

            if (edSvc == null)
            {
                return value;
            }

            var clilocBrowserForm = new ClilocBrowserForm();

            return edSvc.ShowDialog(clilocBrowserForm) == DialogResult.OK ? clilocBrowserForm.CliLocId : value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}