using System;
using System.IO;
using TemplateLayer;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new VariablesLogic();
            t.ExtractDocumentVariables(File.ReadAllBytes(@"C:\1iesniegums.docx"));
        }
    }
}
