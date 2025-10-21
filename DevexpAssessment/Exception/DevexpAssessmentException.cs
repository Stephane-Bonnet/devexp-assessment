using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevexpAssessment.Exception
{
    public class DevexpAssessmentException : System.Exception
    {
        public DevexpAssessmentException(string message) : base(message) { }
        public DevexpAssessmentException(System.Exception internalException) : base("An internal error occurred in the DevexpAssessment SDK.", internalException) { }
    }
}
