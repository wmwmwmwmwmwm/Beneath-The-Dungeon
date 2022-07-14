//using System;
//using System.Security.Cryptography;
//using System.Text;


//public static class AESCryptography
//{
//	const string kkkk = "Do not Modify Save Data Please ";
//	public static string Decrypt(string textToDecrypt)
//	{
//		RijndaelManaged rijndaelCipher = new RijndaelManaged();
//		rijndaelCipher.Mode = CipherMode.CBC;
//		rijndaelCipher.Padding = PaddingMode.PKCS7;
//		rijndaelCipher.KeySize = 128;
//		rijndaelCipher.BlockSize = 128;
//		byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
//		byte[] pwdBytes = Encoding.UTF8.GetBytes(kkkk);
//		byte[] keyBytes = new byte[16];
//		int len = pwdBytes.Length;
//		if (len > keyBytes.Length)
//		{
//			len = keyBytes.Length;
//		}
//		Array.Copy(pwdBytes, keyBytes, len);
//		rijndaelCipher.Key = keyBytes;
//		rijndaelCipher.IV = keyBytes;
//		byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
//		return Encoding.UTF8.GetString(plainText);
//	}

//	public static string Encrypt(string textToEncrypt)
//	{
//		RijndaelManaged rijndaelCipher = new RijndaelManaged();
//		rijndaelCipher.Mode = CipherMode.CBC;
//		rijndaelCipher.Padding = PaddingMode.PKCS7;
//		rijndaelCipher.KeySize = 128;
//		rijndaelCipher.BlockSize = 128;
//		byte[] pwdBytes = Encoding.UTF8.GetBytes(kkkk);
//		byte[] keyBytes = new byte[16];
//		int len = pwdBytes.Length;
//		if (len > keyBytes.Length)
//		{
//			len = keyBytes.Length;
//		}
//		Array.Copy(pwdBytes, keyBytes, len);
//		rijndaelCipher.Key = keyBytes;
//		rijndaelCipher.IV = keyBytes;
//		ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
//		byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
//		return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
//	}
//}