using System.Collections.Generic;

namespace TechsysLog.Core.Communications
{
    public class ResponseResult
    {
        public ResponseResult()
        {
            errors = new ResponseErrorMessages();
        }
        public string Title { get; set; }
        public int Status { get; set; }
        public ResponseErrorMessages errors { get; set; }
    }

    public class ResponseErrorMessages
    {
        public ResponseErrorMessages()
        {
            Messages = new List<string>();
        }
        public List<string> Messages { get; set; }
    }
}
