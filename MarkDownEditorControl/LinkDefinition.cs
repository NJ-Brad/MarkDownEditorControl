using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownHelper
{
    public class LinkDefinition
    {
        [MarkDownItemPropertyAttribute("Link Text", "Text")]
        public string LinkText { get; set; }

        [MarkDownItemPropertyAttribute("Display Text", "Text")]
        public string Display { get; set; }

        [MarkDownItemPropertyAttribute("Tool Tip", "Text")]
        public string Tooltip { get; set; }
    }
}
