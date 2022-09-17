using System.Windows.Forms;

namespace NativeSignTool
{
    partial class SignToolForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignToolForm));
            this.fileToSign = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.certFile = new System.Windows.Forms.TextBox();
            this.buttonCert = new System.Windows.Forms.Button();
            this.signButton = new System.Windows.Forms.Button();
            this.password = new System.Windows.Forms.TextBox();
            this.detachedSignature = new System.Windows.Forms.CheckBox();
            this.buttonSig = new System.Windows.Forms.Button();
            this.sigFile = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.buttonDecode = new System.Windows.Forms.Button();
            this.fileToDecode = new System.Windows.Forms.TextBox();
            this.buttonToDecode = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.validYears = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.hashAlg = new System.Windows.Forms.ComboBox();
            this.sigAlg = new System.Windows.Forms.ComboBox();
            this.passCert = new System.Windows.Forms.TextBox();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.issuerBox = new System.Windows.Forms.TextBox();
            this.certDir = new System.Windows.Forms.TextBox();
            this.buttonDir = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.base64 = new System.Windows.Forms.RadioButton();
            this.hex = new System.Windows.Forms.RadioButton();
            this.keyHashAlg = new System.Windows.Forms.ComboBox();
            this.myCertPass = new System.Windows.Forms.TextBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.key = new System.Windows.Forms.TextBox();
            this.otherCert = new System.Windows.Forms.TextBox();
            this.otherCertButton = new System.Windows.Forms.Button();
            this.myCert = new System.Windows.Forms.TextBox();
            this.myCertButton = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.Encrypt = new System.Windows.Forms.Button();
            this.encrPassword = new System.Windows.Forms.TextBox();
            this.OAEP = new System.Windows.Forms.ComboBox();
            this.Decrypt = new System.Windows.Forms.Button();
            this.fileToEncrDecr = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.certEncrDecr = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.AESIV = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.aesHex = new System.Windows.Forms.RadioButton();
            this.AESKey = new System.Windows.Forms.TextBox();
            this.AESfile = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unregisterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToSign
            // 
            this.fileToSign.Location = new System.Drawing.Point(3, 19);
            this.fileToSign.Name = "fileToSign";
            this.fileToSign.PlaceholderText = "File to sign";
            this.fileToSign.Size = new System.Drawing.Size(530, 23);
            this.fileToSign.TabIndex = 0;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(535, 18);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(24, 24);
            this.browseButton.TabIndex = 0;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // certFile
            // 
            this.certFile.Location = new System.Drawing.Point(3, 49);
            this.certFile.Name = "certFile";
            this.certFile.PlaceholderText = "Certificate file";
            this.certFile.Size = new System.Drawing.Size(530, 23);
            this.certFile.TabIndex = 1;
            // 
            // buttonCert
            // 
            this.buttonCert.Location = new System.Drawing.Point(535, 49);
            this.buttonCert.Name = "buttonCert";
            this.buttonCert.Size = new System.Drawing.Size(24, 24);
            this.buttonCert.TabIndex = 2;
            this.buttonCert.Text = "...";
            this.buttonCert.UseVisualStyleBackColor = true;
            this.buttonCert.Click += new System.EventHandler(this.buttonCert_Click);
            // 
            // signButton
            // 
            this.signButton.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.signButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.signButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.signButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.signButton.Location = new System.Drawing.Point(1, 135);
            this.signButton.Name = "signButton";
            this.signButton.Size = new System.Drawing.Size(558, 28);
            this.signButton.TabIndex = 3;
            this.signButton.Text = "Sign";
            this.signButton.UseVisualStyleBackColor = false;
            this.signButton.Click += new System.EventHandler(this.signButton_Click);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(3, 79);
            this.password.Name = "password";
            this.password.PlaceholderText = "Private key password (if any)";
            this.password.Size = new System.Drawing.Size(555, 23);
            this.password.TabIndex = 4;
            this.password.UseSystemPasswordChar = true;
            // 
            // detachedSignature
            // 
            this.detachedSignature.AutoSize = true;
            this.detachedSignature.Location = new System.Drawing.Point(3, 110);
            this.detachedSignature.Name = "detachedSignature";
            this.detachedSignature.Size = new System.Drawing.Size(128, 19);
            this.detachedSignature.TabIndex = 5;
            this.detachedSignature.Text = "Detached signature";
            this.detachedSignature.UseVisualStyleBackColor = true;
            this.detachedSignature.CheckedChanged += new System.EventHandler(this.detachedSignature_CheckedChanged);
            // 
            // buttonSig
            // 
            this.buttonSig.Location = new System.Drawing.Point(534, 108);
            this.buttonSig.Name = "buttonSig";
            this.buttonSig.Size = new System.Drawing.Size(24, 24);
            this.buttonSig.TabIndex = 6;
            this.buttonSig.Text = "...";
            this.buttonSig.UseVisualStyleBackColor = true;
            this.buttonSig.Visible = false;
            this.buttonSig.Click += new System.EventHandler(this.buttonSig_Click);
            // 
            // sigFile
            // 
            this.sigFile.Location = new System.Drawing.Point(133, 108);
            this.sigFile.Name = "sigFile";
            this.sigFile.PlaceholderText = "Signature file";
            this.sigFile.Size = new System.Drawing.Size(400, 23);
            this.sigFile.TabIndex = 7;
            this.sigFile.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(6, 25);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(571, 214);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileToSign);
            this.tabPage1.Controls.Add(this.buttonSig);
            this.tabPage1.Controls.Add(this.browseButton);
            this.tabPage1.Controls.Add(this.sigFile);
            this.tabPage1.Controls.Add(this.certFile);
            this.tabPage1.Controls.Add(this.detachedSignature);
            this.tabPage1.Controls.Add(this.buttonCert);
            this.tabPage1.Controls.Add(this.password);
            this.tabPage1.Controls.Add(this.signButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 44);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(563, 166);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sign file (CMS / PKCS #7)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonDecode);
            this.tabPage3.Controls.Add(this.fileToDecode);
            this.tabPage3.Controls.Add(this.buttonToDecode);
            this.tabPage3.Location = new System.Drawing.Point(4, 44);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(563, 166);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Verify signature and decode (CMS / PKCS #7)";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonDecode
            // 
            this.buttonDecode.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonDecode.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonDecode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonDecode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonDecode.Location = new System.Drawing.Point(1, 134);
            this.buttonDecode.Name = "buttonDecode";
            this.buttonDecode.Size = new System.Drawing.Size(558, 28);
            this.buttonDecode.TabIndex = 5;
            this.buttonDecode.Text = "Decode";
            this.buttonDecode.UseVisualStyleBackColor = false;
            this.buttonDecode.Click += new System.EventHandler(this.buttonDecode_Click);
            // 
            // fileToDecode
            // 
            this.fileToDecode.Location = new System.Drawing.Point(3, 61);
            this.fileToDecode.Name = "fileToDecode";
            this.fileToDecode.PlaceholderText = "File to decode";
            this.fileToDecode.Size = new System.Drawing.Size(530, 23);
            this.fileToDecode.TabIndex = 1;
            // 
            // buttonToDecode
            // 
            this.buttonToDecode.Location = new System.Drawing.Point(535, 60);
            this.buttonToDecode.Name = "buttonToDecode";
            this.buttonToDecode.Size = new System.Drawing.Size(24, 24);
            this.buttonToDecode.TabIndex = 2;
            this.buttonToDecode.Text = "...";
            this.buttonToDecode.UseVisualStyleBackColor = true;
            this.buttonToDecode.Click += new System.EventHandler(this.buttonToDecode_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.validYears);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.hashAlg);
            this.tabPage2.Controls.Add(this.sigAlg);
            this.tabPage2.Controls.Add(this.passCert);
            this.tabPage2.Controls.Add(this.buttonCreate);
            this.tabPage2.Controls.Add(this.issuerBox);
            this.tabPage2.Controls.Add(this.certDir);
            this.tabPage2.Controls.Add(this.buttonDir);
            this.tabPage2.Location = new System.Drawing.Point(4, 44);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(563, 166);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Create self-signed certificate";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // validYears
            // 
            this.validYears.Location = new System.Drawing.Point(477, 100);
            this.validYears.Name = "validYears";
            this.validYears.Size = new System.Drawing.Size(80, 23);
            this.validYears.TabIndex = 10;
            this.validYears.TextChanged += new System.EventHandler(this.validYears_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(406, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Valid, years";
            // 
            // hashAlg
            // 
            this.hashAlg.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.hashAlg.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.hashAlg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hashAlg.FormattingEnabled = true;
            this.hashAlg.Items.AddRange(new object[] {
            "SHA256",
            "SHA384",
            "SHA512"});
            this.hashAlg.Location = new System.Drawing.Point(205, 101);
            this.hashAlg.Name = "hashAlg";
            this.hashAlg.Size = new System.Drawing.Size(162, 23);
            this.hashAlg.TabIndex = 7;
            // 
            // sigAlg
            // 
            this.sigAlg.AccessibleDescription = "";
            this.sigAlg.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.sigAlg.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.sigAlg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sigAlg.FormattingEnabled = true;
            this.sigAlg.Items.AddRange(new object[] {
            "RSA1024",
            "RSA2048",
            "RSA4096",
            "ECDSA"});
            this.sigAlg.Location = new System.Drawing.Point(5, 101);
            this.sigAlg.Name = "sigAlg";
            this.sigAlg.Size = new System.Drawing.Size(170, 23);
            this.sigAlg.TabIndex = 6;
            // 
            // passCert
            // 
            this.passCert.Location = new System.Drawing.Point(4, 70);
            this.passCert.Name = "passCert";
            this.passCert.PlaceholderText = "Private key password (if any)";
            this.passCert.Size = new System.Drawing.Size(555, 23);
            this.passCert.TabIndex = 5;
            this.passCert.UseSystemPasswordChar = true;
            // 
            // buttonCreate
            // 
            this.buttonCreate.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonCreate.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCreate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonCreate.Location = new System.Drawing.Point(2, 135);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(558, 28);
            this.buttonCreate.TabIndex = 4;
            this.buttonCreate.Text = "Create";
            this.buttonCreate.UseVisualStyleBackColor = false;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // issuerBox
            // 
            this.issuerBox.Location = new System.Drawing.Point(5, 15);
            this.issuerBox.Name = "issuerBox";
            this.issuerBox.PlaceholderText = "Issuer";
            this.issuerBox.Size = new System.Drawing.Size(552, 23);
            this.issuerBox.TabIndex = 3;
            // 
            // certDir
            // 
            this.certDir.Location = new System.Drawing.Point(5, 42);
            this.certDir.Name = "certDir";
            this.certDir.PlaceholderText = "Folder to save certificate files";
            this.certDir.Size = new System.Drawing.Size(530, 23);
            this.certDir.TabIndex = 1;
            // 
            // buttonDir
            // 
            this.buttonDir.Location = new System.Drawing.Point(537, 41);
            this.buttonDir.Name = "buttonDir";
            this.buttonDir.Size = new System.Drawing.Size(24, 24);
            this.buttonDir.TabIndex = 2;
            this.buttonDir.Text = "...";
            this.buttonDir.UseVisualStyleBackColor = true;
            this.buttonDir.Click += new System.EventHandler(this.buttonDir_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.base64);
            this.tabPage4.Controls.Add(this.hex);
            this.tabPage4.Controls.Add(this.keyHashAlg);
            this.tabPage4.Controls.Add(this.myCertPass);
            this.tabPage4.Controls.Add(this.generateButton);
            this.tabPage4.Controls.Add(this.key);
            this.tabPage4.Controls.Add(this.otherCert);
            this.tabPage4.Controls.Add(this.otherCertButton);
            this.tabPage4.Controls.Add(this.myCert);
            this.tabPage4.Controls.Add(this.myCertButton);
            this.tabPage4.Location = new System.Drawing.Point(4, 44);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(563, 166);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Generate EC Diffie-Hellman key";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // base64
            // 
            this.base64.AutoSize = true;
            this.base64.Location = new System.Drawing.Point(6, 112);
            this.base64.Name = "base64";
            this.base64.Size = new System.Drawing.Size(61, 19);
            this.base64.TabIndex = 12;
            this.base64.Text = "Base64";
            this.base64.UseVisualStyleBackColor = true;
            // 
            // hex
            // 
            this.hex.AutoSize = true;
            this.hex.Checked = true;
            this.hex.Location = new System.Drawing.Point(6, 91);
            this.hex.Name = "hex";
            this.hex.Size = new System.Drawing.Size(46, 19);
            this.hex.TabIndex = 11;
            this.hex.TabStop = true;
            this.hex.Text = "Hex";
            this.hex.UseVisualStyleBackColor = true;
            // 
            // keyHashAlg
            // 
            this.keyHashAlg.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.keyHashAlg.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.keyHashAlg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keyHashAlg.FormattingEnabled = true;
            this.keyHashAlg.Items.AddRange(new object[] {
            "SHA256",
            "SHA384",
            "SHA512"});
            this.keyHashAlg.Location = new System.Drawing.Point(397, 64);
            this.keyHashAlg.Name = "keyHashAlg";
            this.keyHashAlg.Size = new System.Drawing.Size(162, 23);
            this.keyHashAlg.TabIndex = 10;
            // 
            // myCertPass
            // 
            this.myCertPass.Location = new System.Drawing.Point(4, 64);
            this.myCertPass.Name = "myCertPass";
            this.myCertPass.PlaceholderText = "Private key password (if any)";
            this.myCertPass.Size = new System.Drawing.Size(392, 23);
            this.myCertPass.TabIndex = 9;
            this.myCertPass.UseSystemPasswordChar = true;
            // 
            // generateButton
            // 
            this.generateButton.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.generateButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.generateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generateButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.generateButton.Location = new System.Drawing.Point(2, 136);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(558, 28);
            this.generateButton.TabIndex = 8;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = false;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // key
            // 
            this.key.Location = new System.Drawing.Point(94, 91);
            this.key.Multiline = true;
            this.key.Name = "key";
            this.key.PlaceholderText = "Key";
            this.key.ReadOnly = true;
            this.key.Size = new System.Drawing.Size(465, 39);
            this.key.TabIndex = 7;
            // 
            // otherCert
            // 
            this.otherCert.Location = new System.Drawing.Point(3, 37);
            this.otherCert.Name = "otherCert";
            this.otherCert.PlaceholderText = "Certificate with public key file";
            this.otherCert.Size = new System.Drawing.Size(530, 23);
            this.otherCert.TabIndex = 5;
            // 
            // otherCertButton
            // 
            this.otherCertButton.Location = new System.Drawing.Point(535, 37);
            this.otherCertButton.Name = "otherCertButton";
            this.otherCertButton.Size = new System.Drawing.Size(24, 24);
            this.otherCertButton.TabIndex = 6;
            this.otherCertButton.Text = "...";
            this.otherCertButton.UseVisualStyleBackColor = true;
            this.otherCertButton.Click += new System.EventHandler(this.otherCertButton_Click);
            // 
            // myCert
            // 
            this.myCert.Location = new System.Drawing.Point(3, 9);
            this.myCert.Name = "myCert";
            this.myCert.PlaceholderText = "Certificate with private key file";
            this.myCert.Size = new System.Drawing.Size(530, 23);
            this.myCert.TabIndex = 3;
            // 
            // myCertButton
            // 
            this.myCertButton.Location = new System.Drawing.Point(535, 9);
            this.myCertButton.Name = "myCertButton";
            this.myCertButton.Size = new System.Drawing.Size(24, 24);
            this.myCertButton.TabIndex = 4;
            this.myCertButton.Text = "...";
            this.myCertButton.UseVisualStyleBackColor = true;
            this.myCertButton.Click += new System.EventHandler(this.myCertButton_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.Encrypt);
            this.tabPage5.Controls.Add(this.encrPassword);
            this.tabPage5.Controls.Add(this.OAEP);
            this.tabPage5.Controls.Add(this.Decrypt);
            this.tabPage5.Controls.Add(this.fileToEncrDecr);
            this.tabPage5.Controls.Add(this.button1);
            this.tabPage5.Controls.Add(this.certEncrDecr);
            this.tabPage5.Controls.Add(this.button2);
            this.tabPage5.Location = new System.Drawing.Point(4, 44);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(563, 166);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "RSA encryption & decryption";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // Encrypt
            // 
            this.Encrypt.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Encrypt.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Encrypt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Encrypt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Encrypt.Location = new System.Drawing.Point(1, 103);
            this.Encrypt.Name = "Encrypt";
            this.Encrypt.Size = new System.Drawing.Size(558, 28);
            this.Encrypt.TabIndex = 13;
            this.Encrypt.Text = "Encrypt";
            this.Encrypt.UseVisualStyleBackColor = false;
            this.Encrypt.Click += new System.EventHandler(this.Encrypt_Click);
            // 
            // encrPassword
            // 
            this.encrPassword.Location = new System.Drawing.Point(3, 68);
            this.encrPassword.Name = "encrPassword";
            this.encrPassword.PlaceholderText = "Private key password (if any)";
            this.encrPassword.Size = new System.Drawing.Size(392, 23);
            this.encrPassword.TabIndex = 12;
            this.encrPassword.UseSystemPasswordChar = true;
            // 
            // OAEP
            // 
            this.OAEP.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.OAEP.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.OAEP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OAEP.FormattingEnabled = true;
            this.OAEP.Items.AddRange(new object[] {
            "SHA256",
            "SHA384",
            "SHA512"});
            this.OAEP.Location = new System.Drawing.Point(397, 68);
            this.OAEP.Name = "OAEP";
            this.OAEP.Size = new System.Drawing.Size(162, 23);
            this.OAEP.TabIndex = 11;
            // 
            // Decrypt
            // 
            this.Decrypt.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Decrypt.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Decrypt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Decrypt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Decrypt.Location = new System.Drawing.Point(2, 133);
            this.Decrypt.Name = "Decrypt";
            this.Decrypt.Size = new System.Drawing.Size(558, 28);
            this.Decrypt.TabIndex = 9;
            this.Decrypt.Text = "Decrypt";
            this.Decrypt.UseVisualStyleBackColor = false;
            this.Decrypt.Click += new System.EventHandler(this.Decrypt_Click);
            // 
            // fileToEncrDecr
            // 
            this.fileToEncrDecr.Location = new System.Drawing.Point(3, 8);
            this.fileToEncrDecr.Name = "fileToEncrDecr";
            this.fileToEncrDecr.PlaceholderText = "File to encrypt/decrypt";
            this.fileToEncrDecr.Size = new System.Drawing.Size(530, 23);
            this.fileToEncrDecr.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(535, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 24);
            this.button1.TabIndex = 4;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // certEncrDecr
            // 
            this.certEncrDecr.Location = new System.Drawing.Point(3, 38);
            this.certEncrDecr.Name = "certEncrDecr";
            this.certEncrDecr.PlaceholderText = "Certificate file";
            this.certEncrDecr.Size = new System.Drawing.Size(530, 23);
            this.certEncrDecr.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(535, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 24);
            this.button2.TabIndex = 6;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.button4);
            this.tabPage6.Controls.Add(this.button5);
            this.tabPage6.Controls.Add(this.AESIV);
            this.tabPage6.Controls.Add(this.radioButton1);
            this.tabPage6.Controls.Add(this.aesHex);
            this.tabPage6.Controls.Add(this.AESKey);
            this.tabPage6.Controls.Add(this.AESfile);
            this.tabPage6.Controls.Add(this.button3);
            this.tabPage6.Location = new System.Drawing.Point(4, 44);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(563, 166);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "AES encryption & decryption";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button4.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button4.Location = new System.Drawing.Point(2, 107);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(558, 28);
            this.button4.TabIndex = 18;
            this.button4.Text = "Encrypt";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button5.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button5.Location = new System.Drawing.Point(3, 136);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(558, 28);
            this.button5.TabIndex = 17;
            this.button5.Text = "Decrypt";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // AESIV
            // 
            this.AESIV.Location = new System.Drawing.Point(93, 81);
            this.AESIV.Multiline = true;
            this.AESIV.Name = "AESIV";
            this.AESIV.PlaceholderText = "IV (must be set to decrypt or will be generated during encryption)";
            this.AESIV.Size = new System.Drawing.Size(465, 25);
            this.AESIV.TabIndex = 16;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(5, 59);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(61, 19);
            this.radioButton1.TabIndex = 15;
            this.radioButton1.Text = "Base64";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // aesHex
            // 
            this.aesHex.AutoSize = true;
            this.aesHex.Checked = true;
            this.aesHex.Location = new System.Drawing.Point(5, 38);
            this.aesHex.Name = "aesHex";
            this.aesHex.Size = new System.Drawing.Size(46, 19);
            this.aesHex.TabIndex = 14;
            this.aesHex.TabStop = true;
            this.aesHex.Text = "Hex";
            this.aesHex.UseVisualStyleBackColor = true;
            // 
            // AESKey
            // 
            this.AESKey.Location = new System.Drawing.Point(93, 38);
            this.AESKey.Multiline = true;
            this.AESKey.Name = "AESKey";
            this.AESKey.PlaceholderText = "Key";
            this.AESKey.Size = new System.Drawing.Size(465, 39);
            this.AESKey.TabIndex = 13;
            // 
            // AESfile
            // 
            this.AESfile.Location = new System.Drawing.Point(3, 6);
            this.AESfile.Name = "AESfile";
            this.AESfile.PlaceholderText = "File to encrypt/decrypt";
            this.AESfile.Size = new System.Drawing.Size(530, 23);
            this.AESfile.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(535, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 24);
            this.button3.TabIndex = 6;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.textWizardToolStripMenuItem,
            this.cOMToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.menuStrip1.Size = new System.Drawing.Size(581, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // textWizardToolStripMenuItem
            // 
            this.textWizardToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.textWizardToolStripMenuItem.Name = "textWizardToolStripMenuItem";
            this.textWizardToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.textWizardToolStripMenuItem.Text = "Text wizard";
            this.textWizardToolStripMenuItem.Click += new System.EventHandler(this.textWizardToolStripMenuItem_Click);
            // 
            // cOMToolStripMenuItem
            // 
            this.cOMToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cOMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registerToolStripMenuItem,
            this.unregisterToolStripMenuItem,
            this.descriptionToolStripMenuItem});
            this.cOMToolStripMenuItem.Name = "cOMToolStripMenuItem";
            this.cOMToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.cOMToolStripMenuItem.Text = "COM";
            // 
            // registerToolStripMenuItem
            // 
            this.registerToolStripMenuItem.Name = "registerToolStripMenuItem";
            this.registerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.registerToolStripMenuItem.Text = "Register";
            this.registerToolStripMenuItem.Click += new System.EventHandler(this.rgisterToolStripMenuItem_Click);
            // 
            // unregisterToolStripMenuItem
            // 
            this.unregisterToolStripMenuItem.Name = "unregisterToolStripMenuItem";
            this.unregisterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.unregisterToolStripMenuItem.Text = "Unregister";
            this.unregisterToolStripMenuItem.Click += new System.EventHandler(this.unregisterToolStripMenuItem_Click);
            // 
            // descriptionToolStripMenuItem
            // 
            this.descriptionToolStripMenuItem.Name = "descriptionToolStripMenuItem";
            this.descriptionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.descriptionToolStripMenuItem.Text = "Description";
            this.descriptionToolStripMenuItem.Click += new System.EventHandler(this.descriptionToolStripMenuItem_Click);
            // 
            // SignToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(581, 244);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "SignToolForm";
            this.Text = "Sign tool";
            this.Load += new System.EventHandler(this.SignToolForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox fileToSign;
        private Button browseButton;
        private TextBox certFile;
        private Button buttonCert;
        private Button signButton;
        private TextBox password;
        private CheckBox detachedSignature;
        private Button buttonSig;
        private TextBox sigFile;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox certDir;
        private Button buttonDir;
        private TextBox issuerBox;
        private Button buttonCreate;
        private TextBox passCert;
        private ComboBox sigAlg;
        private ComboBox hashAlg;
        private Label label1;
        private TextBox validYears;
        private TabPage tabPage3;
        private TextBox fileToDecode;
        private Button buttonToDecode;
        private Button buttonDecode;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private TabPage tabPage4;
        private TextBox myCert;
        private Button myCertButton;
        private TextBox otherCert;
        private Button otherCertButton;
        private TextBox key;
        private Button generateButton;
        private TextBox myCertPass;
        private ComboBox keyHashAlg;
        private RadioButton base64;
        private RadioButton hex;
        private TabPage tabPage5;
        private TextBox fileToEncrDecr;
        private Button button1;
        private TextBox certEncrDecr;
        private Button button2;
        private Button Decrypt;
        private ComboBox OAEP;
        private TextBox encrPassword;
        private Button Encrypt;
        private TabPage tabPage6;
        private TextBox AESfile;
        private Button button3;
        private RadioButton radioButton1;
        private RadioButton aesHex;
        private TextBox AESKey;
        private Button button4;
        private Button button5;
        private TextBox AESIV;
        private ToolStripMenuItem textWizardToolStripMenuItem;
        private ToolStripMenuItem cOMToolStripMenuItem;
        private ToolStripMenuItem registerToolStripMenuItem;
        private ToolStripMenuItem unregisterToolStripMenuItem;
        private ToolStripMenuItem descriptionToolStripMenuItem;
    }
}