using PropertyEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownEditor
{
    public class DefinitionDefinition
    {
        [PropertyAttribute("Term", "Text")]
        public string Term { get; set; }

        [PropertyAttribute("Link Text", "MultiLineText")]
        public string Meaning { get; set; }
    }
}
