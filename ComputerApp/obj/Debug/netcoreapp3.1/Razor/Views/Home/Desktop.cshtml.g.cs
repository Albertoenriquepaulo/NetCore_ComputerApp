#pragma checksum "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\Home\Desktop.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "12e73601cfc8b199c239cf526a03acaf495a7c18"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Desktop), @"mvc.1.0.view", @"/Views/Home/Desktop.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\_ViewImports.cshtml"
using ComputerApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\_ViewImports.cshtml"
using ComputerApp.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"12e73601cfc8b199c239cf526a03acaf495a7c18", @"/Views/Home/Desktop.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"332b4c4e18bd3ab1808c41af3c3e1b093295c5d0", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Desktop : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<Computer>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Computers", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "BuildComputer", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\Home\Desktop.cshtml"
  
    ViewData["Title"] = "Desktop";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Build Your Desktop</h1>\r\n\r\n<div class=\"row\">\r\n");
#nullable restore
#line 10 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\Home\Desktop.cshtml"
     if (ViewBag.isDesktop)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"col-md-6 col-sm-6\">\r\n");
            WriteLiteral(@"            <div class=""card ml-5 text-center bg-transparent border-0 col-md-3"" style=""width: 20rem;"">
                <img src=""https://c1.neweggimages.com/NeweggImage/ProductImage/83-221-575-V09.jpg"" class=""card-img-top"" alt=""Show details for build your own computer"" height=""250"">
                <div class=""card-body"">
                    <h5 class=""card-title"">");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "12e73601cfc8b199c239cf526a03acaf495a7c184727", async() => {
                WriteLiteral("Build your own computer");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("</h5>\r\n");
            WriteLiteral("                </div>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 23 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\Home\Desktop.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 25 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\Home\Desktop.cshtml"
     foreach (Computer computerItem in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"col-md-6 col-sm-6\">\r\n");
            WriteLiteral("            <div class=\"card ml-5 text-center bg-transparent border-0 col-md-3\" style=\"width: 20rem;\">\r\n                <img");
            BeginWriteAttribute("src", " src=\"", 1329, "\"", 1355, 1);
#nullable restore
#line 30 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\Home\Desktop.cshtml"
WriteAttributeValue("", 1335, computerItem.ImgUrl, 1335, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"card-img-top\" alt=\"Show details for build your own computer\" height=\"280\">\r\n                <div class=\"card-body\">\r\n                    <h5 class=\"card-title text-secondary\">");
#nullable restore
#line 32 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\Home\Desktop.cshtml"
                                                     Write(computerItem.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n                    <p class=\"card-text\">Price: <strong>");
#nullable restore
#line 33 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\Home\Desktop.cshtml"
                                                   Write(computerItem.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong></p>\r\n                    <a href=\"#\" class=\"btn btn-primary\">Add to Cart</a>\r\n                </div>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 38 "D:\BBKBootcamp\Projects\03 Modulo EF\ComputerApp\ComputerApp\Views\Home\Desktop.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<Computer>> Html { get; private set; }
    }
}
#pragma warning restore 1591
