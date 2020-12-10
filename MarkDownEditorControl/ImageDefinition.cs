using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownHelper
{
    public class ImageDefinition
    {

        [MarkDownItemPropertyAttribute("Image URL", "Text")]
        public string ImageUrl { get; set; }

        [MarkDownItemPropertyAttribute("Alt Text", "Text")]
        public string Display { get; set; }

        [MarkDownItemPropertyAttribute("Tool Tip", "Text")]
        public string Tooltip { get; set; }
    }
}
