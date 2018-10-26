using System.Collections.Generic;

namespace Server.Api.Models.View
{
    public class ErrorModel
    {
        public IList<string> Username;

        public IList<string> Email;

        public IList<string> Password;

        public IList<string> ConfirmPassword;

        public ErrorModel()
        {
            this.Username = new List<string>();
            this.Email = new List<string>();
            this.Password = new List<string>();
            this.ConfirmPassword = new List<string>();
        }
    }
}