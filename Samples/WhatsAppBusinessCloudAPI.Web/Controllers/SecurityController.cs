using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace WhatsAppBusinessCloudAPI.Web.Controllers
{
	public class SecurityController
	{
		private readonly ILogger<HomeController> _logger;
		public SecurityController(ILogger<HomeController> logger) {
			_logger = logger;
		}

		private string GetRandomCharacters(string characters, int count)
		{
			Random random = new Random();
			string shuffledCharacters = "";

			// Make sure the string is long enough
			while (shuffledCharacters.Length <= count)
			{
				shuffledCharacters += new string(characters.OrderBy(c => random.Next()).ToArray());
			}

			int randomNumber = 0;
			string ret = "";
			
			randomNumber = random.Next(0, shuffledCharacters.Length - count);
			ret = shuffledCharacters.Substring(randomNumber, Math.Min(count, shuffledCharacters.Length));
			
			return ret;
		}

		/// <summary>
		/// Minimum length must be 4
		/// Compiling a random string
		/// </summary>
		/// <param name="len"></param>
		/// <returns></returns>
		public string GenerateRandomString(int len = 256)
		{
			try
			{
				if (len <4) { len = 4; };
				// Define the characters to include in the random string
				string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				string lower = "abcdefghijklmnopqrstuvwxyz";
				string digits = "0123456789";
				string special = "!@#$%^&*()-_+=,.;:'|/<>?{}[]";

				// Calculate the maximum number of characters for each category
				int maxUpper = (int)Math.Ceiling(len * 0.3);
				int maxLower = (int)Math.Ceiling(len * 0.2);
				int maxDigits = (int)Math.Ceiling(len * 0.3);
				int maxSpecial = (int)Math.Ceiling(len * 0.2);

				// Ensure at least one of each character type
				string result = string.Empty;
				result += GetRandomCharacters(upper, 1);
				result += GetRandomCharacters(lower, 1);
				result += GetRandomCharacters(digits, 1);
				result += GetRandomCharacters(special, 1);

				// Generate the rest of the random string
				result += GetRandomCharacters(upper, Math.Min(maxUpper, len - result.Length));
				result += GetRandomCharacters(lower, Math.Min(maxLower, len - result.Length));
				result += GetRandomCharacters(digits, Math.Min(maxDigits, len - result.Length));
				result += GetRandomCharacters(special, Math.Min(maxSpecial, len - result.Length));

				// Shuffle the characters in the string
				result = new string(result.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
				int mylen = result.Length;
				if (mylen > len)
				{
					result = result.Substring(0, len);
				}
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return "Sorry I failed";
			}
		}
	}
}
