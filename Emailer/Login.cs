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

        public Login(string nameOfExchangeServer)
        {
            InitializeComponent();
            _nameOfExchangeServer = nameOfExchangeServer;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            SendEmail send = new SendEmail(this.textBoxUserName.Text,
                this.textBoxPassword.Text, 
                this.textBoxDomain.Text,
                this._nameOfExchangeServer);
        }
    }
}
