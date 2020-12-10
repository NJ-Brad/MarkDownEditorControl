using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownHelper
{
    public class CodeDefinition
    {
        [MarkDownItemPropertyAttribute("Language", "Choice", new[] {"",
            "asax",
            "ashx",
            "aspx",
            "aspxcs",
            "aspxvb",
            "cpp",
            "csharp",
            "css",
            "html",
            "java",
            "javascript",
            "powershell",
            "sql",
            "vbdotnet",
            "xml" } )]
        public string Language { get; set; }
    }
}
