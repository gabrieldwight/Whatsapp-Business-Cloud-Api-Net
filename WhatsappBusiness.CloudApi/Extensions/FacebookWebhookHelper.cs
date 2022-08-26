using System;
using System.Security.Cryptography;
using System.Text;

namespace WhatsappBusiness.CloudApi.Extensions
{
    /// <summary>
    /// Facebook Webhook Helper class helps to verify the integrity and origin of the payload request
    /// </summary>
    public static class FacebookWebhookHelper
    {
        /// <summary>
        /// The HTTP request will contain an X-Hub-Signature header which contains the SHA1 signature of the request payload,
        /// using the app secret as the key, and prefixed with sha1=.
        /// Your callback endpoint can verify this signature to validate the integrity and origin of the payload
        /// </summary>
        /// <param name="appSecret">facebook app secret</param>
        /// <param name="payload">body of webhook post request</param>
        /// <returns>calculated signature</returns>
        public static string CalculateSignature(string appSecret, string payload)
        {
            /*
             Please note that the calculation is made on the escaped unicode version of the payload, with lower case hex digits.
             If you just calculate against the decoded bytes, you will end up with a different signature.
             For example, the string äöå should be escaped to \u00e4\u00f6\u00e5.
             */

            using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret));
            hmac.Initialize();
            byte[] hashArray = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            string hash = $"SHA256={BitConverter.ToString(hashArray).Replace("-", string.Empty)}";

            return hash;
        }
    }
}
