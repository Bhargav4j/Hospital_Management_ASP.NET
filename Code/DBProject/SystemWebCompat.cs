using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace System.Web.UI
{
    public class Page
    {
        public bool IsValid { get; set; } = true;
        public bool IsPostBack { get; set; } = false;
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public IDictionary<string, object> Session { get; set; } = new Dictionary<string, object>();

        protected virtual void Page_Load(object sender, EventArgs e) { }
    }
    
    public class MasterPage : Page
    {
    }
    
    namespace WebControls
    {
        public class TextBox
        {
            public string Text { get; set; }
        }

        public class Label
        {
            public string Text { get; set; }
            public bool Visible { get; set; }
        }

        public class Button
        {
            public string Text { get; set; }
            public bool Visible { get; set; } = true;
        }

        public class DropDownList
        {
            public string SelectedValue { get; set; }
            public string Text { get; set; }
            public bool Visible { get; set; } = true;
            public ListItem SelectedItem { get; set; } = new ListItem();
        }

        public class ListItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public class GridView
        {
            public object DataSource { get; set; }
            public GridViewRowCollection Rows { get; set; } = new GridViewRowCollection();
            public int EditIndex { get; set; } = -1;
            public string Caption { get; set; }
            public void DataBind() { }
        }

        public class GridViewRow
        {
            public TableCellCollection Cells { get; set; } = new TableCellCollection();
        }

        public class GridViewRowCollection : List<GridViewRow>
        {
        }

        public class TableCell
        {
            public string Text { get; set; }
        }

        public class TableCellCollection : List<TableCell>
        {
            public TableCell this[int index]
            {
                get => index < Count ? base[index] : new TableCell();
            }
        }

        public class RadioButton
        {
            public bool Checked { get; set; }
        }

        public class HyperLink
        {
            public string NavigateUrl { get; set; }
        }

        public class Image
        {
            public string ImageUrl { get; set; }
        }

        public class RequiredFieldValidator
        {
            public string ErrorMessage { get; set; }
            public string ControlToValidate { get; set; }
        }

        public class RegularExpressionValidator
        {
            public string ErrorMessage { get; set; }
            public string ControlToValidate { get; set; }
            public string ValidationExpression { get; set; }
        }

        public class CompareValidator
        {
            public string ErrorMessage { get; set; }
            public string ControlToValidate { get; set; }
            public string ControlToCompare { get; set; }
        }

        public class RangeValidator
        {
            public string ErrorMessage { get; set; }
            public string ControlToValidate { get; set; }
            public int MinimumValue { get; set; }
            public int MaximumValue { get; set; }
        }

        public class CustomValidator
        {
            public string ErrorMessage { get; set; }
            public string ControlToValidate { get; set; }
        }

        public class ContentPlaceHolder
        {
            public string ID { get; set; }
        }

        public class GridViewCommandEventArgs : EventArgs
        {
            public string CommandName { get; set; }
            public object CommandArgument { get; set; }
        }

        public class GridViewDeleteEventArgs : EventArgs
        {
            public int RowIndex { get; set; }
            public bool Cancel { get; set; }
        }

        public class ServerValidateEventArgs : EventArgs
        {
            public string Value { get; set; }
            public bool IsValid { get; set; }
        }
    }
    
    namespace HtmlControls
    {
        public class HtmlGenericControl
        {
            public string InnerHtml { get; set; }
        }

        public class HtmlInputFile
        {
            public HttpPostedFile PostedFile { get; set; }
        }

        public class HtmlForm
        {
            public string ID { get; set; }
            public string Action { get; set; }
        }
    }
}

namespace System.Web
{
    public class HttpPostedFile
    {
        public string FileName { get; set; }
        public int ContentLength { get; set; }
        public void SaveAs(string filename) { }
    }

    public static class HttpResponseExtensions
    {
        private static bool _bufferOutput = true;

        public static void Write(this Microsoft.AspNetCore.Http.HttpResponse response, string content)
        {
            response.WriteAsync(content).Wait();
        }

        public static bool GetBufferOutput(this Microsoft.AspNetCore.Http.HttpResponse response)
        {
            return _bufferOutput;
        }

        public static void SetBufferOutput(this Microsoft.AspNetCore.Http.HttpResponse response, bool value)
        {
            _bufferOutput = value;
        }
    }

    public static class HttpResponseHelper
    {
        public static bool BufferOutput
        {
            get => true;
            set { }
        }
    }
}
