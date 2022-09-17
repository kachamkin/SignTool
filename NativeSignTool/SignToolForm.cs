using System;
using System.Diagnostics;
using System.Windows.Forms;
using TextWizard;

namespace NativeSignTool
{
    public partial class SignToolForm : Form
    {
        public SignToolForm()
        {
            InitializeComponent();
        }

        private int years = 1;
        private readonly CryptLib Crypt = new();

        private void browseButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
                fileToSign.Text = ofd.FileName;
        }

        private void buttonCert_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            ofd.Filter = "Certificate files with private key (*.pfx)|*.pfx|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
                certFile.Text = ofd.FileName;
        }

        private void signButton_Click(object sender, EventArgs e)
        {
            try
            {
                Crypt.SignFile(fileToSign.Text, certFile.Text, detachedSignature.Checked, sigFile.Text, password.Text);
                MessageBox.Show("File sccessfully signed!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't sign specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonSig_Click(object sender, EventArgs e)
        {
            using SaveFileDialog ofd = new();
            if (ofd.ShowDialog() == DialogResult.OK)
                sigFile.Text = ofd.FileName;
        }

        private void detachedSignature_CheckedChanged(object sender, EventArgs e)
        {
            sigFile.Visible = detachedSignature.Checked;
            buttonSig.Visible = detachedSignature.Checked;
        }

        private void buttonDir_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog ofd = new();
            if (ofd.ShowDialog() == DialogResult.OK)
                certDir.Text = ofd.SelectedPath;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Crypt.CreateCertificate(certDir.Text, issuerBox.Text, sigAlg.Text, hashAlg.Text, passCert.Text, years);
                MessageBox.Show("Certificate successfully created!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't create certificate!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void SignToolForm_Load(object sender, EventArgs e)
        {
            hashAlg.Text = "SHA256";
            sigAlg.Text = "RSA2048";
            keyHashAlg.Text = "SHA256";
            OAEP.Text = "SHA256";
            validYears.DataBindings.Add("Text", years, "");
        }

        private void validYears_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(validYears.Text, out int newValue) && newValue > 0)
                years = newValue;
            else
                validYears.Text = years.ToString();
        }

        private void buttonToDecode_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
                fileToDecode.Text = ofd.FileName;

        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {
            try
            {
                Crypt.VerifySignature(fileToDecode.Text, out string info);
                MessageBox.Show("Signature succesfully verified, file decoded!\r\n\r\n" + info, "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't verify signature!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Digital signing, signatures verification, self-signed certificates creation\r\n                                       (C) 2022 Andrey Kachamkin\r\n                                           kachamkin@gmail.com", "Sign Tool");
        }

        private void myCertButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            ofd.Filter = "Certificate files with private key (*.pfx)|*.pfx|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
                myCert.Text = ofd.FileName;
        }

        private void otherCertButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            ofd.Filter = "Certificate files (*.cer)|*.cer|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
                otherCert.Text = ofd.FileName;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            key.Text = "";
            try
            {
                byte[] result = Convert.FromBase64String(Crypt.GenerateECDiffieHellmanKey(myCert.Text, otherCert.Text, keyHashAlg.Text, myCertPass.Text));
                key.Text = hex.Checked ? BitConverter.ToString(result) : Convert.ToBase64String(result);
                MessageBox.Show("Key successfully generated!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't generate key!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
                fileToEncrDecr.Text = ofd.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            ofd.Filter = "Certificate files (*.cer)|*.cer|Certificate files with private key (*.pfx)|*.pfx|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
                certEncrDecr.Text = ofd.FileName;
        }

        private void Decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                Crypt.RSADecrypt(fileToEncrDecr.Text, certEncrDecr.Text, OAEP.Text, encrPassword.Text);
                MessageBox.Show("File sccessfully decrypted!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't decrypt specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                Crypt.RSAEncrypt(fileToEncrDecr.Text, certEncrDecr.Text, OAEP.Text);
                MessageBox.Show("File sccessfully encrypted!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't encrypt specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
                AESfile.Text = ofd.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AESIV.Text = "";
            try
            {
                Crypt.AESEncrypt(aesHex.Checked ? Convert.ToBase64String(Convert.FromHexString(AESKey.Text.Replace("-", ""))) : AESKey.Text, AESfile.Text, out string IV);
                AESIV.Text = aesHex.Checked ? BitConverter.ToString(Convert.FromBase64String(IV)) : IV;
                MessageBox.Show("File sccessfully encrypted!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't encrypt specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Crypt.AESDecrypt(aesHex.Checked ? Convert.ToBase64String(Convert.FromHexString(AESKey.Text.Replace("-", ""))) : AESKey.Text, aesHex.Checked ? Convert.ToBase64String(Convert.FromHexString(AESIV.Text.Replace("-", ""))) : AESIV.Text, AESfile.Text);
                MessageBox.Show("File sccessfully deccrypted!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't decrypt specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TextWizardForm().Show();
        }

        private void rgisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo si = new("regsvr32", "\"" + Directory.GetCurrentDirectory() + "\\" + typeof(CryptLib.CryptLib).Assembly.GetName().Name + ".comhost.dll\"")
                {
                    UseShellExecute = true,
                    Verb = "runas"
                };
                Process.Start(si);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't register Sign Tool for COM!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void unregisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo si = new("regsvr32", "/u \"" + Directory.GetCurrentDirectory() + "\\" + typeof(CryptLib.CryptLib).Assembly.GetName().Name + ".comhost.dll\"")
                {
                    UseShellExecute = true,
                    Verb = "runas"
                };
                Process.Start(si);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't unregister Sign Tool!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo si = new("\"" + Directory.GetCurrentDirectory() + "\\COM interface description.txt\"")
                {
                    UseShellExecute = true,
                };
                Process.Start(si);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't open description file!\r\n" + ex.Message, "Sign Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
