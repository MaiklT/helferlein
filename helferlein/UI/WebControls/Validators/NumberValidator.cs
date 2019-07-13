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
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace helferlein.UI.WebControls.Validators
{
   public class NumberValidator : BaseValidator
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

      [Description("Number Type")]
      public NumberType NumberType
      {
         get
         {
            object o = ViewState["NumberType"];
            if (o == null)
               return NumberType.Integer;
            else
               return (NumberType)ViewState["NumberType"];
         }
         set { ViewState["NumberType"] = value; }
      }

      public string MaxValue
      {
         get
         {
            object o;
            switch (NumberType)
            {
               case NumberType.Integer:
                  o = ViewState["MaxIntValue"];
                  if (o == null)
                     return Int32.MaxValue.ToString();
                  else
                     return ViewState["MaxIntValue"].ToString();
               case NumberType.Double:
                  o = ViewState["MaxDoubleValue"];
                  if (o == null)
                     return Double.MaxValue.ToString();
                  else
                     return ViewState["MaxDoubleValue"].ToString();
               case NumberType.Decimal:
                  o = ViewState["MaxDecimalValue"];
                  if (o == null)
                     return Decimal.MaxValue.ToString();
                  else
                     return ViewState["MaxDecimalValue"].ToString();
               default:
                  return string.Empty;
            }
         }
         set
         {
            switch (NumberType)
            {
               case NumberType.Integer:
                  int maxIntValue;
                  if (Int32.TryParse(value, out maxIntValue))
                     ViewState["MaxIntValue"] = maxIntValue;
                  else
                     ViewState["MaxIntValue"] = null;
                  break;
               case NumberType.Double:
                  double maxDoubleValue;
                  if (Double.TryParse(value, out maxDoubleValue))
                     ViewState["MaxDoubleValue"] = value;
                  else
                     ViewState["MaxDoubleValue"] = null;
                  break;
               case NumberType.Decimal:
                  decimal maxDecimalValue;
                  if (Decimal.TryParse(value, out maxDecimalValue))
                     ViewState["MaxDecimalValue"] = value;
                  else
                     ViewState["MaxDecimalValue"] = null;
                  break;
               default:
                  break;
            }
         }
      }

      public string MinValue
      {
         get
         {
            object o;
            switch (NumberType)
            {
               case NumberType.Integer:
                  o = ViewState["MinIntValue"];
                  if (o == null)
                     return Int32.MinValue.ToString();
                  else
                     return ViewState["MinIntValue"].ToString();
               case NumberType.Double:
                  o = ViewState["MinDoubleValue"];
                  if (o == null)
                     return Double.MinValue.ToString();
                  else
                     return ViewState["MinDoubleValue"].ToString();
               case NumberType.Decimal:
                  o = ViewState["MinDecimalValue"];
                  if (o == null)
                     return Decimal.MinValue.ToString();
                  else
                     return ViewState["MinDecimalValue"].ToString();
               default:
                  return string.Empty;
            }
         }
         set
         {
            switch (NumberType)
            {
               case NumberType.Integer:
                  int minIntValue;
                  if (Int32.TryParse(value, out minIntValue))
                     ViewState["MinIntValue"] = minIntValue;
                  else
                     ViewState["MinIntValue"] = null;
                  break;
               case NumberType.Double:
                  double minDoubleValue;
                  if (Double.TryParse(value, out minDoubleValue))
                     ViewState["MinDoubleValue"] = minDoubleValue;
                  else
                     ViewState["MinDoubleValue"] = null;
                  break;
               case NumberType.Decimal:
                  decimal minDecimalValue;
                  if (Decimal.TryParse(value, out minDecimalValue))
                     ViewState["MinDecimalValue"] = minDecimalValue;
                  else
                     ViewState["MinDecimalValue"] = null;
                  break;
               default:
                  break;
            }
         }
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
            throw new HttpException(string.Format("The NumberValidator can only validate controls of type TextBox."));

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
               ErrorMessage = (string.IsNullOrEmpty(RequiredErrorMessage) ? ErrorMessage : RequiredErrorMessage);
               result = false;
            }
            else
            {
               ErrorMessage = (string.IsNullOrEmpty(InvalidErrorMessage) ? ErrorMessage : InvalidErrorMessage);
               result = CheckNumber(s);
            }
         }
         else
         {
            ErrorMessage = (string.IsNullOrEmpty(InvalidErrorMessage) ? ErrorMessage : InvalidErrorMessage);
            if (!(string.IsNullOrEmpty(s)))
               result = CheckNumber(s);
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
               writer.AddAttribute("evaluationfunction", "ClientValidateNumber", false);
               writer.AddAttribute("required", Required.ToString().ToLower(), false);
               writer.AddAttribute("minvalue", MinValue, false);
               writer.AddAttribute("maxvalue", MaxValue, false);
               writer.AddAttribute("numbertype", NumberType.ToString(), false);
               writer.AddAttribute("invalidErrorMessage", InvalidErrorMessage, true);
               writer.AddAttribute("requiredErrorMessage", RequiredErrorMessage, true);
               writer.AddAttribute("decimalSeparator", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
               writer.AddAttribute("groupSeparator", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
            }
            else
            {
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "ClientValidateNumber", false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "required", Required.ToString().ToLower(), false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "minvalue", MinValue, false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "maxvalue", MaxValue, false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "numbertype", NumberType.ToString(), false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "invalidErrorMessage", InvalidErrorMessage, true);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "requiredErrorMessage", RequiredErrorMessage, true);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "decimalSeparator", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, false);
               Page.ClientScript.RegisterExpandoAttribute(ClientID, "groupSeparator", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, false);
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

      private bool CheckNumber(string s)
      {
         bool result = false;
         switch (NumberType)
         {
            case NumberType.Integer:
               Int32 integerToCheck;
               if (Int32.TryParse(s, out integerToCheck))
               {
                  result = ((integerToCheck >= Int32.Parse(MinValue)) && (integerToCheck <= Int32.Parse(MaxValue)) && (Convert.ToString(integerToCheck) == s));
               }
               break;
            case NumberType.Double:
               Double doubleToCheck;
               if (Double.TryParse(s, out doubleToCheck))
               {
                  result = ((doubleToCheck >= Double.Parse(MinValue)) && (doubleToCheck <= Double.Parse(MaxValue)));
               }
               break;
            case NumberType.Decimal:
               Decimal decimalToCheck;
               if (Decimal.TryParse(s, out decimalToCheck))
               {
                  result = ((decimalToCheck >= Decimal.Parse(MinValue)) && (decimalToCheck <= Decimal.Parse(MaxValue)));
               }
               break;
            default:
               break;
         }
         return result;
      }
   }
}
