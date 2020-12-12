using PropertyEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownEditor
{
    public class ImageDefinition
    {

        [PropertyAttribute("Image URL", "Text")]
        public string ImageUrl { get; set; }

        [PropertyAttribute("Alt Text", "Text")]
        public string Display { get; set; }

        [PropertyAttribute("Tool Tip", "Text")]
        public string Tooltip { get; set; }
    }
}
