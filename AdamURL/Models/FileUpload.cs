using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdamURL.Models
{
    /* a representation of a processed uploaded file */
    public class FileUpload
    {
        public List<string> listUrls;

        public FileUpload()
        {
            listUrls = new List<string>();
        }

    }
}
