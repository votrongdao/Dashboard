using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace DashboardSite.Core.Utilities
{
    public class CryptoHelper
    {
        /// <summary>
        /// The method muat be called explicitely to base64-encode a string string.
        /// </summary>
        /// <returns></returns>
        public string Base64Encode(string sData)
        {
            try
            {
                byte[] encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error in Base64Encode " + e.Message);
            }
        }


        /// <summary>
        /// The method muat be called explicitely to decode a base64-encoded string.
        /// We assume that encoded information is text, NOT binary.
        /// </summary>
        /// <returns></returns>
        public static string Base64Decode(string sEncoded)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(sEncoded);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error in Base64Decode " + e.Message);
            }

        }

        public static string GenerateAlphaNumericPassword(int iMinRequiredPasswordLength, int iMinRequiredNonAlphanumericCharacters)
        {
            Random rand = new Random();
            StringBuilder newpassword = new StringBuilder();
            int length = (iMinRequiredPasswordLength < 8 ? 8 : iMinRequiredPasswordLength);
            int symbollength = iMinRequiredNonAlphanumericCharacters;
            int charlength = length - symbollength;
            newpassword.Append("Pw$"); //make sure it meets our password rules.
            for (int index = 0; index < charlength; index++)
            {
                newpassword.Append((char)rand.Next(65, 90));
            }
            for (int index = 0; index < symbollength; index++)
            {
                newpassword.Append((char)rand.Next(33, 47));
            }
            return newpassword.ToString();
        }

        /// <summary>
        /// MD5 Hash an input string and return the hash as a 32 character hexadecimal string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GenerateMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        #region Symmetric Cryptography
        /// <summary>
        /// There are three main points about the sample code that is worth noting, otherwise, 
        /// the comments in the code are self explanatory. 
        /// 1) In the .NET Framework, the cryptography service adopts a stream-oriented design. 
        ///    In the case of the sample code, the CryptoStream class and its derivative class MemoryStream is used. 
        /// 2) SHA1 was used to hash the secret key. SHA1 returns a 160-bit hash and the first 64-bit of the hash is used as the key for DES encryption. 
        /// 3) The return result of the encrypt functions is base64 encoded so that the result can be transported 
        ///    over HTTP and displayed in HTML forms by default. 

        /// </summary>

        private static Byte[] m_Key = new Byte[8];
        private static Byte[] m_IV = new Byte[8];

        //////////////////////////
        //Function to encrypt data
        /// <summary>
        /// This function takes in a Secret Key, strKey and the input data, strData. 
        /// The return string is base64 encoded so that it can be transported over the HTTP GET/POST. 
        /// If encryption fails, an empty string returns
        /// </summary>
        /// <param name="strKey">Secret Key</param>
        /// <param name="strData">Input data to be encrypted</param>
        /// <returns>Encrypted value</returns>
        public static string EncryptData(String strKey, String strData)
        {
            string strResult;		//Return Result

            //1. String Length cannot exceed 90Kb. Otherwise, buffer will overflow. See point 3 for reasons
            if (strData.Length > 92160)
            {
                strResult = "Error. Data String too large. Keep within 90Kb.";
                return strResult;
            }

            //2. Generate the Keys
            if (!InitKey(strKey))
            {
                strResult = "Error. Fail to generate key for encryption";
                return strResult;
            }

            //3. Prepare the String
            //	The first 5 character of the string is formatted to store the actual length of the data.
            //	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
            //	If anyone figure a good way to 'remember' the original length to facilite the decryption without having to use additional function parameters, pls let me know.
            strData = String.Format("{0,5:00000}" + strData, strData.Length);


            //4. Encrypt the Data
            byte[] rbData = new byte[strData.Length];
            ASCIIEncoding aEnc = new ASCIIEncoding();
            aEnc.GetBytes(strData, 0, strData.Length, rbData, 0);

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

            ICryptoTransform desEncrypt = descsp.CreateEncryptor(m_Key, m_IV);


            //5. Perpare the streams:
            //	mOut is the output stream. 
            //	mStream is the input stream.
            //	cs is the transformation stream.
            MemoryStream mStream = new MemoryStream(rbData);
            CryptoStream cs = new CryptoStream(mStream, desEncrypt, CryptoStreamMode.Read);
            MemoryStream mOut = new MemoryStream();

            //6. Start performing the encryption
            int bytesRead;
            byte[] output = new byte[1024];
            do
            {
                bytesRead = cs.Read(output, 0, 1024);
                if (bytesRead != 0)
                    mOut.Write(output, 0, bytesRead);
            } while (bytesRead > 0);

            //7. Returns the encrypted result after it is base64 encoded
            //	In this case, the actual result is converted to base64 so that it can be transported over the HTTP protocol without deformation.
            if (mOut.Length == 0)
                strResult = "";
            else
                strResult = Convert.ToBase64String(mOut.GetBuffer(), 0, (int)mOut.Length);

            return strResult;
        }

        //////////////////////////
        //Function to decrypt data
        /// <summary>
        /// This decryption function takes in the secret key used to encrypt the input data, strData. 
        /// strData must be base64 encoded. The function returns an empty string to indicate error
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string DecryptData(String strKey, String strData)
        {
            string strResult;

            //1. Generate the Key used for decrypting
            if (!InitKey(strKey))
            {
                strResult = "Error. Fail to generate key for decryption";
                return strResult;
            }

            //2. Initialize the service provider
            int nReturn = 0;
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
            ICryptoTransform desDecrypt = descsp.CreateDecryptor(m_Key, m_IV);

            //3. Prepare the streams:
            //	mOut is the output stream. 
            //	cs is the transformation stream.
            MemoryStream mOut = new MemoryStream();
            CryptoStream cs = new CryptoStream(mOut, desDecrypt, CryptoStreamMode.Write);

            //4. Remember to revert the base64 encoding into a byte array to restore the original encrypted data stream
            byte[] bPlain = new byte[strData.Length];
            try
            {
                bPlain = Convert.FromBase64CharArray(strData.ToCharArray(), 0, strData.Length);
            }
            catch (Exception)
            {
                strResult = "Error. Input Data is not base64 encoded.";
                return strResult;
            }

            long lRead = 0;
            long lTotal = strData.Length;

            try
            {
                //5. Perform the actual decryption
                while (lTotal >= lRead)
                {
                    cs.Write(bPlain, 0, (int)bPlain.Length);
                    //descsp.BlockSize=64
                    lRead = mOut.Length + Convert.ToUInt32(((bPlain.Length / descsp.BlockSize) * descsp.BlockSize));
                };

                ASCIIEncoding aEnc = new ASCIIEncoding();
                strResult = aEnc.GetString(mOut.GetBuffer(), 0, (int)mOut.Length);

                //6. Trim the string to return only the meaningful data
                //	Remember that in the encrypt function, the first 5 character holds the length of the actual data
                //	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
                String strLen = strResult.Substring(0, 5);
                int nLen = Convert.ToInt32(strLen);
                strResult = strResult.Substring(5, nLen);
                nReturn = (int)mOut.Length;

                return strResult;
            }
            catch (Exception)
            {
                strResult = "Error. Decryption Failed. Possibly due to incorrect Key or corrputed data";
                return strResult;
            }
        }

        /////////////////////////////////////////////////////////////
        //Private function to generate the keys into member variables
        private static bool InitKey(String strKey)
        {
            try
            {
                // Convert Key to byte array
                byte[] bp = new byte[strKey.Length];
                ASCIIEncoding aEnc = new ASCIIEncoding();
                aEnc.GetBytes(strKey, 0, strKey.Length, bp, 0);

                //Hash the key using SHA1
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                byte[] bpHash = sha.ComputeHash(bp);

                int i;
                // use the low 64-bits for the key value
                for (i = 0; i < 8; i++)
                    m_Key[i] = bpHash[i];

                for (i = 8; i < 16; i++)
                    m_IV[i - 8] = bpHash[i];

                return true;
            }
            catch (Exception)
            {
                //Error Performing Operations
                return false;
            }
        }
        #endregion 
    }
}
