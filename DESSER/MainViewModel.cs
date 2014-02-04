using DESLib;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace DESSER
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Private Variables
        private string _plaintext;
        private string _ciphertext;
        private string _key;
        #endregion

        #region Properties
        public DelegateCommand EncryptCommand { get; set; }
        public DelegateCommand DecryptCommand { get; set; }
        public Action<string> PopupMessage { get; set; }

        public string PlainText
        {
            get { return _plaintext; }
            set
            {
                if (_plaintext != value)
                {
                    _plaintext = value;
                    NotifyPropertyChanged("PlainText");
                }
            }
        }
        public string CipherText
        {
            get { return _ciphertext; }
            set
            {
                if (_ciphertext != value)
                {
                    _ciphertext = value;
                    NotifyPropertyChanged("CipherText");
                }
            }
        }
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key != value)
                {
                    _key = value;
                    NotifyPropertyChanged("Key");
                }
            }
        }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            EncryptCommand = new DelegateCommand(Encrypt);
            DecryptCommand = new DelegateCommand(Decrypt);

            PlainText = "0123456789ABCDEF";
            Key = "133457799BBCDFF1";
        }
        #endregion

        #region Public
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private
        private void Encrypt()
        {
            try
            {
                if (Validate(true))
                {
                    var ciphertext = DES.Encrypt(
                        PlainText.FromHexStringToBitArray(),
                        Key.FromHexStringToBitArray());
                    CipherText = ciphertext.ToHexString();
                }
            }
            catch (Exception e)
            {
                ShowMessage(string.Format("Error: {0}", e));
            }
        }
        private void Decrypt()
        {
            try
            {
                if (Validate(false))
                {
                    var plaintext = DES.Decrypt(
                        CipherText.FromHexStringToBitArray(),
                        Key.FromHexStringToBitArray());
                    PlainText = plaintext.ToHexString();
                }
            }
            catch (Exception e)
            {
                ShowMessage(string.Format("Error: {0}", e));
            }
        }
        private bool Validate(bool encrypt)
        {
            string regex = @"\A\b[0-9a-fA-F]+\b\Z";

            if (encrypt)
            {
                if (!Regex.IsMatch(PlainText, regex))
                {
                    ShowMessage("Invalid Plaintext. Ensure Plaintext is entered as a hex string.");
                    return false;
                }
                if (string.IsNullOrEmpty(PlainText))
                {
                    ShowMessage("Missing Plaintext.");
                    return false;
                }
            }
            else
            {
                if (!Regex.IsMatch(CipherText, regex))
                {
                    ShowMessage("Invalid Ciphertext. Ensure Ciphertext is entered as a hex string.");
                    return false;
                }
                if (string.IsNullOrEmpty(CipherText) || CipherText.Length % 16 != 0)
                {
                    ShowMessage("Invalid Ciphertext length. Ensure Ciphertext is entered in 64 bit blocks (16 hex characters).");
                    return false;
                }
            }

            if (!Regex.IsMatch(Key, regex))
            {
                ShowMessage("Invalid Key. Ensure Key is entered as a hex string.");
                return false;
            }
            if (string.IsNullOrEmpty(Key) || Key.Length % 16 != 0)
            {
                ShowMessage("Invalid Key length. Ensure Key is entered in 64 bit blocks (16 hex characters).");
                return false;
            }

            return true;
        }
        private void ShowMessage(string message)
        {
            if (PopupMessage != null)
                PopupMessage(message);
        }
        private void NotifyPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion
    }
}
