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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace helferlein.UI.WebControls
{
   [ToolboxData("<{0}:MailLink runat=\"server\" />")]
   public class MailLink : WebControl
   {
      private string _emailAddress;
      private string _subject;
      private string _displayValue;

      public string EmailAddress
      {
         get
         {
            if (EnableViewState)
            {
               object o = ViewState["EmailAddress"];
               if (o != null)
                  return (Convert.ToString(o));
               else
                  return string.Empty;
            }
            else
               return _emailAddress;
         }
         set
         {
            if (EnableViewState)
               ViewState["EmailAddress"] = value;
            else
               _emailAddress = value;
         }
      }

      public string Subject
      {
         get
         {
            if (EnableViewState)
            {
               object o = ViewState["Subject"];
               if (o != null)
                  return (Convert.ToString(o));
               else
                  return string.Empty;
            }
            else
               return _subject;
         }
         set
         {
            if (EnableViewState)
               ViewState["Subject"] = value;
            else
               _subject = value;
         }
      }

      public string DisplayValue
      {
         get
         {
            if (EnableViewState)
            {
               object o = ViewState["DisplayValue"];
               if (o != null)
                  return (Convert.ToString(o));
               else
                  return string.Empty;
            }
            else
               return _displayValue;
         }
         set
         {
            if (EnableViewState)
               ViewState["DisplayValue"] = value;
            else
               _displayValue = value;
         }
      }

      public MailLink()
      {
      }

      public MailLink(string emailAddress)
      {
         EmailAddress = emailAddress;
      }

      public MailLink(string emailAddress, string displayValue)
      {
         EmailAddress = emailAddress;
         DisplayValue = displayValue;
      }

      public MailLink(string emailAddress, string displayValue, string subject)
      {
         EmailAddress = emailAddress;
         DisplayValue = displayValue;
         Subject = subject;
      }

      protected override void Render(HtmlTextWriter writer)
      {
         Label result = new Label();
         if (!(string.IsNullOrEmpty(EmailAddress)))
         {
            string[] splittedEmailAddress = EmailAddress.Split(new char[] { '@' });
            // Let's do a very basic check of the email address:
            // - it must not start with "@"
            // - it must not end with "@"
            // - it must contain the character "@" exactly 1 time, so the splitted array must have 2 elements
            if ((EmailAddress.StartsWith("@")) || (EmailAddress.EndsWith("@")) || (splittedEmailAddress.Length != 2))
               throw new HttpException(string.Format("The email address of '{0}' is not valid: \"{1}\"", ID, EmailAddress));

            if (!(string.IsNullOrEmpty(CssClass)))
               result.Text = string.Format("<a href=\"{0}:{1}('{2}','{3}','{4}');\" class=\"{5}\">{6}</a>", "javascript", "ftbs", splittedEmailAddress[0], splittedEmailAddress[1], Subject, CssClass, (string.IsNullOrEmpty(DisplayValue) ? EmailAddress : DisplayValue));
            else
               result.Text = string.Format("<a href=\"{0}:{1}('{2}','{3}','{4}');\">{5}</a>", "javascript", "ftbs", splittedEmailAddress[0], splittedEmailAddress[1], Subject, (string.IsNullOrEmpty(DisplayValue) ? EmailAddress : DisplayValue));
         }
         else
         {
            result.Text = (string.IsNullOrEmpty(DisplayValue) ? EmailAddress : DisplayValue);
         }
         Controls.Add(result);
         Render(writer);
      }

      protected override void CreateChildControls()
      {
         Controls.Clear();
      }

      protected override void OnPreRender(EventArgs e)
      {
         if ((Page != null) && (!(Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), "helferlein.UI.WebControls.ftbs"))))
            Page.ClientScript.RegisterClientScriptInclude(GetType(), "helferlein.UI.WebControls.ftbs", Page.ClientScript.GetWebResourceUrl(GetType(), "helferlein.UI.WebControls.ftbs.js"));
         OnPreRender(e);
      }
   }
}
