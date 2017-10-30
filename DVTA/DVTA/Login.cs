using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DBAccess;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace DVTA
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String username = txtLgnUsername.Text.Trim();
            String password = txtLgnPass.Text.Trim();
            if (username == string.Empty || password == string.Empty)
            {
                MessageBox.Show("Please enter all the fields!");
                return;
            }

            DBAccessClass db = new DBAccessClass();

            db.openConnection();

           SqlDataReader data = db.checkLogin(username,password);
           if (data.HasRows)
           {
               String user;
               String pass;
               String email;
               int isadmin=0;

              /* Microsoft.Win32.RegistryKey myRegKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Details");
               String uname = (String)myRegKey.GetValue("username", null);
               String pass = (String)myRegKey.GetValue("password", null);*/

               while (data.Read())
               {
                   user = data.GetString(1);
                   pass = data.GetString(2);
                   email = data.GetString(3);
                   isadmin = (int) data.GetValue(4);

                   if (user != "admin")
                   {
                       Microsoft.Win32.RegistryKey key;
                       key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("dvta");
                       key.SetValue("username", user);
                       key.SetValue("password", pass);
                       key.SetValue("email", email);
                       key.SetValue("isLoggedIn", "true");
                       key.Close();
                   }
               }
               txtLgnUsername.Text = "";
               txtLgnPass.Text = "";

               //redirecting to main screen

               if(isadmin != 1)
               {
                   this.Close();
                   Main main = new Main();
                   main.ShowDialog();
                   Application.Exit();
               }
               else
               {                
                   this.Hide();
                   Admin admin = new Admin();
                   admin.ShowDialog();
                   Application.Exit();
               }

               return;
               
           }
           else
           {
             
               MessageBox.Show("Invalid Login");
               txtLgnUsername.Text = "";
               txtLgnPass.Text = "";

           }
            db.closeConnection();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnlgnregister(object sender, EventArgs e)
        {
            this.Hide();
            Register reg = new Register();
            reg.ShowDialog();
            Application.Exit();

        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = PinPublicKey;
            WebRequest wr = WebRequest.Create("https://time.is/Unix_time_nows");
            WebResponse timeResp=wr.GetResponse();
            label1.Text = Convert.ToString(timeResp.ContentLength);
        }
        private static String PUB_KEY = "3082010A0282010100B9A66237F88CC35C385712F002B469704B007A624A9862289926361D46524B589FD5B198BE557D48ECE7EAAC270531B5072030BC04698CE560B84C6E51FE2000EA5FC605D89F6E2C11581B416DCB3B71DE1B1526ABE48B342469C618E9BDA5E68A50655AC96B05E35B1955A3DB9ADF48F32A9F55B836B8EA89BB818094B2B549A5DD37C7F6CA879E0C40EB64F6E5F4B5152169BFD068EF488A5611BD9E37FB15CC2741F1D6F719567775F3333EFCBB9B659E9033FDE01C2B32543EF7E072897FA7CF79EBBEBA9C005A1F0DAD0F7CD7F025FBA3782FD34FF362B97868273F2A9E05CE4AF872DFAEE6F40A83F76348DAC54F710C82B5DFC2141AC2DDABECF1361D0203010001";
        public static bool PinPublicKey(object sender, X509Certificate certificate, X509Chain chain,
                                SslPolicyErrors sslPolicyErrors)
        {
            if (null == certificate)
                return false;

            String pk = certificate.GetPublicKeyString();
            if (pk.Equals(PUB_KEY))
                return true;

            // Bad dog
            return false;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
