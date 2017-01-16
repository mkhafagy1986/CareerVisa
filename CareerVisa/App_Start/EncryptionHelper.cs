using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CareerVisa.App_Start
{
    public class EncryptionHelper
    {
        SymmetricAlgorithm mobjCryptoService;
        string _EncryptionKey;
        public EncryptionHelper(string EncryptionKey)
        {
            mobjCryptoService = new DESCryptoServiceProvider();
            _EncryptionKey = EncryptionKey;
        }
        //public string Encrypt(string Source)
        //{
        //    byte[] bytIn = System.Text.ASCIIEncoding.ASCII.GetBytes(Source);
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //    byte[] bytKey = GetLegalKey(_EncryptionKey);

        //    mobjCryptoService.Key = bytKey;
        //    mobjCryptoService.IV = bytKey;

        //    ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();

        //    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

        //    cs.Write(bytIn, 0, bytIn.Length);
        //    cs.FlushFinalBlock();

        //    byte[] bytOut = ms.GetBuffer();
        //    int i = 0;
        //    for (i = 0; i < bytOut.Length; i++)
        //        if (bytOut[i] == 0)
        //            break;

        //    return System.Convert.ToBase64String(bytOut, 0, i);
        //}

        public string Encrypt(string Source, bool useHashing = true)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(Source);

            string key = _EncryptionKey;
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        //public string Decrypt(string Source)
        //{
        //    byte[] bytIn = System.Convert.FromBase64String(Source);

        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
        //    ms.Position = 0;

        //    byte[] bytKey = GetLegalKey(_EncryptionKey);

        //    mobjCryptoService.Key = bytKey;
        //    mobjCryptoService.IV = bytKey;

        //    ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();

        //    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

        //    System.IO.StreamReader sr = new System.IO.StreamReader(cs);
        //    return sr.ReadToEnd();
        //}

        public string Decrypt(string cipherString, bool useHashing = true)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            string key = _EncryptionKey;

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        //public byte[] GetLegalKey(string Key)
        //{
        //    string sTemp;
        //    if (mobjCryptoService.LegalKeySizes.Length > 0)
        //    {
        //        int lessSize = 0, moreSize = mobjCryptoService.LegalKeySizes[0].MinSize;

        //        while (Key.Length * 8 > moreSize)
        //        {
        //            lessSize = moreSize;
        //            moreSize += mobjCryptoService.LegalKeySizes[0].SkipSize;
        //        }
        //        sTemp = Key.PadRight(moreSize / 8, ' ');
        //    }
        //    else
        //        sTemp = Key;


        //    return ASCIIEncoding.ASCII.GetBytes(sTemp);
        //}

    }
}