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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace helferlein.UI.WebControls.Validators
{
   public class RegExpValidator : BaseValidator
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

      [Description("The Regular Expression to validate")]
      public string ValidationExpression
      {
         get
         {
            object o = ViewState["ValidationExpression"];
            if (o == null)
               return string.Empty;
            else
               return (string)ViewState["ValidationExpression"];
         }
         set { ViewState["ValidationExpression"] = value; }
      }

      [Description("Error message when the control is required")]
      public string RequiredErrorMessage
      {
         get
         {
            object o = ViewState["RequiredErrorMessage"];
            if (o == null)
               return ID;
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
               return ID;
            else
               return (string)ViewState["InvalidErrorMessage"];
         }
         set { ViewState["InvalidErrorMessage"] = value; }
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
            throw new HttpException(string.Format("The RegExValidator can only validate controls of type TextBox."));

         // ... and that the ValidationExpression is set
         if (string.IsNullOrEmpty(ValidationExpression))
            throw new HttpException(string.Format("The ValidationExpression property of '{0}'must be set.", ID));

         return true;    // if we reach here, everything checks out
      }

      protected override bool EvaluateIsValid()
      {
         Regex r = new Regex(ValidationExpression);
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
               result = (r.Match(s).Success);
               SetCorrectErrorMessage(InvalidErrorMessage);
            }
         }
         else
         {
            if (!(string.IsNullOrEmpty(s)))
               result = (r.Match(s).Success);
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

         // Add the client side-code (if needed)
         if (RenderUplevel)
         {
            // Indicate the required and validationExpression values and the client-side function to be used for evaluation
            // Use AddAttribute if Helpers.EnableLegacyRendering is true; otherwise, use expando attributes
            if (Helpers.EnableLegacyRendering())
            {
               writer.AddAttribute("evaluationfunction", "ClientValidateExpression", false);
               writer.AddAttribute("required", Required.ToString().ToLower(), false);
               writer.AddAttribute("validationExpression", ValidationExpression, true);
               writer.AddAttribute("requiredErrorMessage", RequiredErrorMessage, true);
               writer.AddAttribute("invalidErrorMessage", InvalidErrorMessage, true);
            }
            else
            {
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "ClientValidateExpression", false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "required", Required.ToString().ToLower(), false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "validationExpression", ValidationExpression, true);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "requiredErrorMessage", RequiredErrorMessage, true);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "invalidErrorMessage", InvalidErrorMessage, true);
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
   }
}
