using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownHelper
{
    public class DefinitionDefinition
    {
        [MarkDownItemPropertyAttribute("Term", "Text")]
        public string Term { get; set; }

        [MarkDownItemPropertyAttribute("Link Text", "MultiLineText")]
        public string Meaning { get; set; }
    }
}
