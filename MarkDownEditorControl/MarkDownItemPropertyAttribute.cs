using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownHelper
{
    public class MarkDownItemPropertyAttribute : Attribute
    {
        public string Prompt { get; set; }
        public string PropertyType { get; set; }
        public string[] Choices { get; set; }

        public MarkDownItemPropertyAttribute(string prompt, string propertyType, params string[] choices)
        {
            this.Prompt = prompt;
            this.PropertyType = propertyType;
            this.Choices = choices;
        }
    }
}
