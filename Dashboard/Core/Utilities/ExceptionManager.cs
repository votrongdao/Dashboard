using System;
using System.Collections.Specialized;

namespace iPreo.Bigdough.Utilities
{
	public static class ExceptionManager_Obsolete 
	{
        private const string LOCALIZATION_CODE_KEY = "CODE";
        private const string LOCALIZATION_PARAMS_KEY = "PARAMS";


		/// <summary>
		/// Static method to publish the exception information.
		/// </summary>r
        /// <param name="ex">The exception object whose information should be published.</param>
		public static void Publish(Exception ex)
		{
            Publish(ex, null);
		}

        public static void Publish(Exception ex, NameValueCollection additionalInfo)
        {
            try
            {
                if (ex.InnerException != null)
                    iDeal.Common.Utilities.ExceptionManager.Publish(ex.InnerException, additionalInfo);
                else
                    iDeal.Common.Utilities.ExceptionManager.Publish(ex, additionalInfo);
            }
            catch (Exception)
            {
                //Don't throw exception again.
            }
        }

		public static void PublishWarning( Exception ex )
		{
            iDeal.Common.Utilities.ExceptionManager.PublishWarning(ex, null);
		}

		public static void PublishWarning( Exception ex, NameValueCollection additionalInfo )
		{
            iDeal.Common.Utilities.ExceptionManager.PublishWarning(ex, additionalInfo);
		}

        /// <summary>
        /// Adds localization info to Exception
        /// </summary>
        /// <param name="ex">Exception instance</param>
        /// <param name="sLocalizationKey">Localization Key string</param>
        /// <param name="oLocalizationParameters">Optional localization parameters</param>
        /// <returns>Filled original exception.</returns>
	    public static Exception AddLocalizationInfo(Exception ex, string sLocalizationKey, params object[] oLocalizationParameters)
	    {
	        ex.Data[LOCALIZATION_CODE_KEY] = sLocalizationKey;
            ex.Data[LOCALIZATION_PARAMS_KEY] = oLocalizationParameters;
            return ex;
	    }

        /// <summary>
        /// Gets exception's localization code.
        /// </summary>
        /// <param name="ex">Exception instance</param>
        /// <returns>Localization code if it exists otherwise empty string.</returns>
        public static string GetLocalizationCode(Exception ex)
        {
            return IsLocalized(ex) ? (string)ex.Data[LOCALIZATION_CODE_KEY] : string.Empty;
        }

        /// <summary>
        /// Gets exception's localization parameters.
        /// </summary>
        /// <param name="ex">Exception instance</param>
        /// <returns>Localization parameters if they exist otherwise empty array.</returns>
        public static object[] GetLocalizationParameters(Exception ex)
        {
            return hasParameters(ex) ? (object[])ex.Data[LOCALIZATION_PARAMS_KEY] : new object[0];
        }

        /// <summary>
        /// Checks if exception contains localization info
        /// </summary>
        /// <param name="ex">Exception instance</param>
        /// <returns></returns>
        public static bool IsLocalized(Exception ex)
        {
            return ex.Data.Contains(LOCALIZATION_CODE_KEY) && (ex.Data[LOCALIZATION_CODE_KEY] is string);
        }

        private static bool hasParameters(Exception ex)
        {
            return ex.Data.Contains(LOCALIZATION_PARAMS_KEY) && (ex.Data[LOCALIZATION_PARAMS_KEY] is object[]);
        }

    }
}
