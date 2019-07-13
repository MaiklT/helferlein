/*
dnnWerk.at ( https://www.dnnWerk.at )
(C) Michael Tobisch 2009-2019

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

using System.Web.UI;
using System.Web.UI.WebControls;

namespace helferlein.UI.WebControls
{
   public class LabelTemplateField : ITemplate
   {
      private string s;

      public LabelTemplateField(string label)
      {
         s = label;
      }
#region ITemplate Member
      public void InstantiateIn(Control container)
      {
         Label templateLabel = new Label();
         templateLabel.ID = s + "Label";
         templateLabel.Visible = true;
         container.Controls.Add(templateLabel);
      }
#endregion
   }

   public class CheckBoxTemplateField : ITemplate
   {
      private string s;

      public CheckBoxTemplateField(string label)
      {
         s = label;
      }

#region ITemplate Member
      public void InstantiateIn(Control container)
      {
         CheckBox templateCheckBox = new CheckBox();
         templateCheckBox.ID = s + "CheckBox";
         templateCheckBox.Visible = true;
         templateCheckBox.Enabled = false;
         container.Controls.Add(templateCheckBox);
      }
#endregion
   }
}