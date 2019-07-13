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

using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace helferlein.UI.WebControls
{
   [ToolboxData("<{0}:LetterSearch runat=\"server\" />")]
   public class LetterSearch : WebControl, IPostBackEventHandler
   {
#region Events
      public event EventHandler Click;
#endregion

      private string letters;
      private string separator;
      private string otherLetters;
      private string allLetters;
      private string selectedLetter;
      private string linkCssClass;
      private string currentCssClass;
      private string separatorCssClass;

      public const string ALL_LETTERS = "HELFERLEIN_UI_WEBCONTROLS_LETTERSEARCH_ALLLETTERS";
      public const string OTHER_LETTER = "HELFERLEIN_UI_WEBCONTROLS_LETTERSEARCH_OTHERLETTERS";

      public string Letters
      {
         get
         {
            if (string.IsNullOrEmpty(letters))
               return "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            else
               return letters;
         }
         set
         {
            if (string.IsNullOrEmpty(value))
               letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            else
               letters = value;
         }
      }

      public string Separator
      {
         get
         {
            if (string.IsNullOrEmpty(separator))
               return "&nbsp;";
            else
               return separator;
         }
         set
         {
            if (string.IsNullOrEmpty(value))
               separator = "&nbsp;";
            else
               separator = value;
         }
      }

      public string OtherLetters
      {
         get { return otherLetters; }
         set { otherLetters = value; }
      }

      public string AllLetters
      {
         get { return allLetters; }
         set { allLetters = value; }
      }

      public string SelectedLetter
      {
         get { return selectedLetter; }
      }

      public string LinkCssClass
      {
         get { return linkCssClass; }
         set { linkCssClass = value; }
      }

      public string CurrentCssClass
      {
         get { return currentCssClass; }
         set { currentCssClass = value; }
      }

      public string SeparatorCssClass
      {
         get { return separatorCssClass; }
         set { separatorCssClass = value; }
      }

      protected override void Render(HtmlTextWriter writer)
      {
         // Add the letters
         for (int i = 0; i < Letters.Length; i++)
         {
            string s = Letters.Substring(i, 1);
            if (SelectedLetter != s)
            {
               HtmlAnchor hyperlink = new HtmlAnchor();
               hyperlink.HRef = "javascript:" + Page.ClientScript.GetPostBackEventReference(this, s);
               hyperlink.Name = UniqueID + "_" + s;
               hyperlink.InnerHtml = s;
               hyperlink.Attributes.Add("class", LinkCssClass);
               Controls.Add(hyperlink);
            }
            else
            {
               Label letterLabel = new Label();
               letterLabel.Text = s;
               letterLabel.CssClass = CurrentCssClass;
               Controls.Add(letterLabel);
            }
            if (i < Letters.Length - 1)
            {
               Label separatorLabel = new Label();
               separatorLabel.Text = Separator;
               separatorLabel.CssClass = SeparatorCssClass;
               Controls.Add(separatorLabel);
            }
         }

         if (!(string.IsNullOrEmpty(OtherLetters)))
         {
            Label separatorLabel = new Label();
            separatorLabel.Text = Separator;
            separatorLabel.CssClass = SeparatorCssClass;
            Controls.Add(separatorLabel);
            if (SelectedLetter != OTHER_LETTER)
            {
               HtmlAnchor hyperlink = new HtmlAnchor();
               hyperlink.HRef = "javascript:" + Page.ClientScript.GetPostBackEventReference(this, OTHER_LETTER);
               hyperlink.Name = UniqueID + "_" + OTHER_LETTER;
               hyperlink.InnerHtml = OtherLetters;
               hyperlink.Attributes.Add("class", LinkCssClass);
               Controls.Add(hyperlink);
            }
            else
            {
               Label otherLabel = new Label();
               otherLabel.Text = OtherLetters;
               otherLabel.CssClass = CurrentCssClass;
               Controls.Add(otherLabel);
            }
         }

         if (!(string.IsNullOrEmpty(AllLetters)))
         {
            Label separatorLabel = new Label();
            separatorLabel.Text = Separator;
            separatorLabel.CssClass = SeparatorCssClass;
            Controls.Add(separatorLabel);
            if (SelectedLetter != ALL_LETTERS)
            {
               HtmlAnchor hyperlink = new HtmlAnchor();
               hyperlink.HRef = "javascript:" + Page.ClientScript.GetPostBackEventReference(this, ALL_LETTERS);
               hyperlink.Name = UniqueID + "_" + ALL_LETTERS;
               hyperlink.InnerHtml = AllLetters;
               hyperlink.Attributes.Add("class", LinkCssClass);
               Controls.Add(hyperlink);
            }
            else
            {
               Label allLabel = new Label();
               allLabel.Text = AllLetters;
               allLabel.CssClass = CurrentCssClass;
               Controls.Add(allLabel);
            }
         }
         Render(writer);
      }

      protected override void CreateChildControls()
      {
         Controls.Clear();
      }

      protected override bool OnBubbleEvent(object source, EventArgs args)
      {
         return OnBubbleEvent(source, args);
      }

      public void RaisePostBackEvent(string eventArgument)
      {
         string eventArgName = Page.Request.Form["__EVENTARGUMENT"];
         selectedLetter = eventArgName;
         Click(this, EventArgs.Empty);
      }
   }
}
