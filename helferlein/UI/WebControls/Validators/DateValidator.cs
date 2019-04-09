/*
dnnWerk.at ( https://www.dnnWerk.at )
Michael Tobisch, 2009-2019

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
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace helferlein.UI.WebControls.Validators
{
   public class DateValidator : BaseValidator
   {
      [Description("Indicates if the control is required")]
      public bool Required
      {
         get
         {
            object o = ViewState["Required"];
            if (o == null)
               return false;
            else
               return (bool)ViewState["Required"];
         }
         set { ViewState["Required"] = value; }
      }

      [Description("Error message when the control is required")]
      public string RequiredErrorMessage
      {
         get
         {
            object o = ViewState["RequiredErrorMessage"];
            if (o == null)
               return string.Empty;
            else
               return (string)ViewState["RequiredErrorMessage"];
         }
         set { ViewState["RequiredErrorMessage"] = value; }
      }

      [Description("Error message when the control is not required, but does not validate against the ValidationExpression")]
      public string InvalidErrorMessage
      {
         get
         {
            object o = ViewState["InvalidErrorMessage"];
            if (o == null)
               return string.Empty;
            else
               return (string)ViewState["InvalidErrorMessage"];
         }
         set { ViewState["InvalidErrorMessage"] = value; }
      }

      [Description("Minimum Date value for this field")]
      public DateTime MinDate
      {
         get
         {
            object o = ViewState["MinDate"];
            if (o == null)
               return DateTime.MinValue;
            else
               return Convert.ToDateTime(o);
         }
         set { ViewState["MinDate"] = value; }
      }

      [Description("Maximum Date value for this field")]
      public DateTime MaxDate
      {
         get
         {
            object o = ViewState["MaxDate"];
            if (o == null)
               return DateTime.MaxValue;
            else
               return Convert.ToDateTime(o);
         }
         set { ViewState["MaxDate"] = value; }
      }

      private TextBox _textBoxToValidate;
      protected TextBox TextBoxToValidate
      {
         get
         {
            if (_textBoxToValidate == null)
               _textBoxToValidate = FindControl(ControlToValidate) as TextBox;
            return _textBoxToValidate;
         }
      }

      protected override bool ControlPropertiesValid()
      {
         // Make sure ControlToValidate is set
         if (ControlToValidate.Length == 0)
            throw new HttpException(string.Format("The ControlToValidate property of '{0}' cannot be blank.", ID));

         // Ensure that the control being validated is a TextBox
         if (TextBoxToValidate == null)
            throw new HttpException(string.Format("The DateValidator can only validate controls of type TextBox."));

         return true;    // if we reach here, everything checks out

      }

      protected override bool EvaluateIsValid()
      {
         string s = TextBoxToValidate.Text;
         bool result = true;

         if (Required)
         {
            if (string.IsNullOrEmpty(s))
            {
               result = false;
               SetCorrectErrorMessage(RequiredErrorMessage);
            }
            else
            {
               result = CheckDate(s);
               SetCorrectErrorMessage(InvalidErrorMessage);
            }
         }
         else
         {
            if (!(string.IsNullOrEmpty(s)))
               result = CheckDate(s);
            SetCorrectErrorMessage(InvalidErrorMessage);
         }

         return result;
      }

      protected void SetCorrectErrorMessage(string errorMessage)
      {
         if (!(string.IsNullOrEmpty(errorMessage)))
            ErrorMessage = errorMessage;
      }

      protected override void AddAttributesToRender(HtmlTextWriter writer)
      {
         AddAttributesToRender(writer);

         if (RenderUplevel)
         {
            if (Helpers.EnableLegacyRendering())
            {
               writer.AddAttribute("evaluationfunction", "ClientValidateDate", false);
               writer.AddAttribute("required", Required.ToString().ToLower(), false);
               writer.AddAttribute("dateSeparator", CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator, false);
               writer.AddAttribute("shortDatePattern", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, false);
               writer.AddAttribute("minDate", MinDate.ToString(), false);
               writer.AddAttribute("maxDate", MaxDate.ToString(), false);
               writer.AddAttribute("invalidErrorMessage", InvalidErrorMessage, true);
               writer.AddAttribute("requiredErrorMessage", RequiredErrorMessage, true);
            }
            else
            {
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "ClientValidateDate", false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "required", Required.ToString().ToLower(), false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "dateSeparator", CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator, false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "shortDatePattern", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "minDate", MinDate.ToString(), false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "maxDate", MaxDate.ToString(), false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "invalidErrorMessage", InvalidErrorMessage, true);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "requiredErrorMessage", RequiredErrorMessage, true);
            }
         }
      }

      protected override void OnPreRender(EventArgs e)
      {
         OnPreRender(e);

         // Register the client-side function using WebResource.axd (if needed)
         // see: http://aspnet.4guysfromrolla.com/articles/080906-1.aspx
         if ((RenderUplevel) && (Page != null) && (!(Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), "helferlein.UI.WebControls.Validators"))))
            Page.ClientScript.RegisterClientScriptInclude(GetType(), "helferlein.UI.WebControls.Validators", Page.ClientScript.GetWebResourceUrl(GetType(), "helferlein.UI.WebControls.Validators.helferlein_Validators.js"));
      }

      private bool CheckDate(string s)
      {
         bool result = true;
         try
         {
            DateTime scratch = DateTime.Parse(s);
            if ((scratch < MinDate) || (scratch > MaxDate))
               result = false;
         }
         catch
         {
            result = false;
         }
         return result;
      }
   }
}
