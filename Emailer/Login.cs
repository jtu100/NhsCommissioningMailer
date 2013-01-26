using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Emailer
{
    public partial class Login : Form
    {
        private string _nameOfExchangeServer;
        private SendEmail _send;

        public Login(string nameOfExchangeServer)
        {
            InitializeComponent();
            _nameOfExchangeServer = nameOfExchangeServer;
        }

        public SendEmail Send
        {
            get { return _send; }
            set { _send = value; }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            ModelLoginDetails loginDetails = new ModelLoginDetails
                                                 {
                                                     UserName = this.textBoxUserName.Text,
                                                     Domain = this.textBoxDomain.Text,
                                                     ExchangeServerAddress = this._nameOfExchangeServer,
                                                     Password = this.textBoxPassword.Text
                                                 };

            _send = new SendEmail(loginDetails);

            this.Hide();
        }
    }
}
