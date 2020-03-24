using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.Services
{
    public class GlobalValuesService
    {
        //private bool IsDeleted { get; set; }
        private bool ShowMessage;
        private string Message;

        public GlobalValuesService()
        {
            ShowMessage = false;
            Message = null;
        }
        public void SetShowMessage(bool value)
        {
            ShowMessage = value;
        }

        public void SetMessage(string value)
        {
            Message = value;
        }
        public void ClearMessage()
        {
            Message = null;
        }

        public bool GetShowMessage()
        {
            return ShowMessage;
        }

        public string GetMessage()
        {
            return Message;
        }

    }
}
