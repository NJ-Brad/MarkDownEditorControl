using PropertyEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownEditor
{
    public class LinkDefinition
    {
        [PropertyAttribute("Link Text", "Text")]
        public string LinkText { get; set; }

        [PropertyAttribute("Display Text", "Text")]
        public string Display { get; set; }

        [PropertyAttribute("Tool Tip", "Text")]
        public string Tooltip { get; set; }
    }
}
