// Decompiled with JetBrains decompiler
// Type: GumpStudio.UndoPoint
// Assembly: GumpStudioCore, Version=1.8.3024.24259, Culture=neutral, PublicKeyToken=null
// MVID: A77D32E5-7519-4865-AA26-DCCB34429732
// Assembly location: C:\GumpStudio_1_8_R3_quinted-02\GumpStudioCore.dll

using GumpStudio.Elements;
using GumpStudio.Forms;
using System;
using System.Collections;

namespace GumpStudio
{
    public class UndoPoint
    {
        public GroupElement ElementStack;
        public GumpProperties GumpProperties;
        public ArrayList Stack;
        public string Text;

        public UndoPoint(DesignerForm Designer)
        {
            this.Stack = new ArrayList();
            this.GumpProperties = (GumpProperties)Designer.GumpProperties.Clone();
            foreach (object stack in Designer.Stacks)
            {
                GroupElement objectValue = (GroupElement)stack;
                GroupElement groupElement = (GroupElement)objectValue.Clone();
                this.Stack.Add(groupElement);
                if (objectValue == Designer.ElementStack)
                    this.ElementStack = groupElement;
            }
        }
    }
}
