using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxyHelpers.EWS;

namespace Emailer
{
    /// <summary>
    /// Mime Types are
    /// application/ms-excel
    /// text/comma-separated-values
    /// </summary>
    public class ModelEmailDetails
    {
        public string SubjectOfEmail { get; set; }
        public string BodyOfEmail { get; set; }
        public string SenderEmail { get; set; }
        public string RecepientEmail { get; set; }
        public string AttachmentLocation { get; set; }
        public BodyType BodyType { get; set; }
        public string ContentType { get; set; }
    }
}
