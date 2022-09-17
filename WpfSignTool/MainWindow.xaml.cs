using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfSignTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class DC : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private uint years = 1;
        public uint Years
        {
            get
            {
                return years;
            }
            set
            {
                if (value != years)
                {
                    years = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }

    public partial class MainWindow : Window
    {
        private CryptLib.CryptLib Crypt;

        public MainWindow()
        {
            InitializeComponent();
            Crypt = new();
            DataContext = new DC();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            bool? result = ofd.ShowDialog();
            if (result != null && (bool)result)
                fileToSign.Text = ofd.FileName;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Digital signing, signatures verification, self-signed certificates creation\r\n                                       (C) 2022 Andrey Kachamkin\r\n                                           kachamkin@gmail.com", "Sign Tool");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            ofd.Filter = "Certificate files with private key (*.pfx)|*.pfx|All files (*.*)|*.*";
            bool? result = ofd.ShowDialog();
            if (result != null && (bool)result)
                certFile.Text = ofd.FileName;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SaveFileDialog ofd = new();
            bool? result = ofd.ShowDialog();
            if (result != null && (bool)result)
                sigFile.Text = ofd.FileName;
        }

        private void detachedSignature_Click(object sender, RoutedEventArgs e)
        {
            bool check = detachedSignature.IsChecked != null && (bool)detachedSignature.IsChecked;
            detachGroup.Visibility = check ? Visibility.Visible : Visibility.Collapsed;
            signButton.Margin = new Thickness(signButton.Margin.Left, signButton.Margin.Top + (check ? 1 : - 1) * detachGroup.Height, signButton.Margin.Right, signButton.Margin.Bottom);
        }

        private void signButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Crypt.SignFile(fileToSign.Text, certFile.Text, detachedSignature.IsChecked != null && (bool)detachedSignature.IsChecked, sigFile.Text, password.Password);
                MessageBox.Show("File sccessfully signed!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't sign specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            bool? result = ofd.ShowDialog();
            if (result != null && (bool)result)
                fileToDecode.Text = ofd.FileName;
        }

        private void decodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Crypt.VerifySignature(fileToDecode.Text, out string info);
                MessageBox.Show("Signature succesfully verified, file decoded!\r\n\r\n" + info, "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't verify signature!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog ofd = new();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                certDir.Text = ofd.SelectedPath;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sigAlg.Text = "RSA2048";
            hashAlg.Text = "SHA256";
            keyHashAlg.Text = "SHA256";
            OAEP.Text = "SHA256";
            validYears.Text = "1";
        }

        private void validYears_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!uint.TryParse(validYears.Text, out uint newValue) || newValue <= 0)
                validYears.Text = ((DC)DataContext).Years.ToString();

        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Crypt.CreateCertificate(certDir.Text, issuer.Text, sigAlg.Text, hashAlg.Text, passCert.Password, (int)((DC)DataContext).Years);
                MessageBox.Show("Certificate successfully created!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't create certificate!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            ofd.Filter = "Certificate files with private key (*.pfx)|*.pfx|All files (*.*)|*.*";
            bool? result = ofd.ShowDialog();
            if (result != null && (bool)result)
                myCert.Text = ofd.FileName;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            ofd.Filter = "Certificate files (*.cer)|*.cer|All files (*.*)|*.*";
            bool? result = ofd.ShowDialog();
            if (result != null && (bool)result)
                otherCert.Text = ofd.FileName;
        }

        private void createButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            key.Text = "";
            try
            {
                byte[] result = Convert.FromBase64String(Crypt.GenerateECDiffieHellmanKey(myCert.Text, otherCert.Text, keyHashAlg.Text, myCertPass.Password));
                key.Text = (Hex.IsChecked != null && (bool)Hex.IsChecked) ? BitConverter.ToString(result) : Convert.ToBase64String(result);
                MessageBox.Show("Key successfully generated!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't generate key!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            bool? result = ofd.ShowDialog();
            if (result != null && (bool)result)
                fileToEncrDecr.Text = ofd.FileName;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            ofd.Filter = "Certificate files (*.cer)|*.cer|Certificate files with private key (*.pfx)|*.pfx|All files (*.*)|*.*";
            bool? result = ofd.ShowDialog();
            if (result != null && (bool)result)
                certEncrDecr.Text = ofd.FileName;
        }

        private void createButton_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Crypt.RSAEncrypt(fileToEncrDecr.Text, certEncrDecr.Text, OAEP.Text);
                MessageBox.Show("File sccessfully encrypted!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't encrypt specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void createButton_Copy2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Crypt.RSADecrypt(fileToEncrDecr.Text, certEncrDecr.Text, OAEP.Text, encrPassword.Password);
                MessageBox.Show("File sccessfully decrypted!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't decrypt specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            bool? result = ofd.ShowDialog();
            if (result != null && (bool)result)
                AESFile.Text = ofd.FileName;
        }

        private void createButton_Copy3_Click(object sender, RoutedEventArgs e)
        {
            AESIV.Text = "";
            bool check = AESHex.IsChecked != null && (bool)AESHex.IsChecked;
            try
            {
                Crypt.AESEncrypt(check ? Convert.ToBase64String(Convert.FromHexString(AESkey.Text.Replace("-", ""))) : AESkey.Text, AESFile.Text, out string IV);
                AESIV.Text = check ? BitConverter.ToString(Convert.FromBase64String(IV)) : IV;
                MessageBox.Show("File sccessfully encrypted!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't encrypt specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void createButton_Copy4_Click(object sender, RoutedEventArgs e)
        {
            bool check = AESHex.IsChecked != null && (bool)AESHex.IsChecked;
            try
            {
                Crypt.AESDecrypt(check ? Convert.ToBase64String(Convert.FromHexString(AESkey.Text.Replace("-", ""))) : AESkey.Text, check ? Convert.ToBase64String(Convert.FromHexString(AESIV.Text.Replace("-", ""))) : AESIV.Text, AESFile.Text);
                MessageBox.Show("File sccessfully deccrypted!", "Sign Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't decrypt specified file!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Couldn't register Sign Tool for COM!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Couldn't register Sign Tool for COM!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Couldn't open description file!\r\n" + ex.Message, "Sign Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
