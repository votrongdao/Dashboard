namespace DashboardSite.Core.ExceptionHandling
{
    public enum ExceptionCategory
    {
        ///////////////////////////////////////////////////////////////////////////////////
        //Category from 101 to 1000 are critical exception
        ///////////////////////////////////////////////////////////////////////////////////
 

        /// <summary>
        /// SqlServerConnectionError
        /// SQL Server down, or maybe SQL connection string is not setup properly
        /// </summary>
        SqlServerConnectionError = 101,


        /// <summary>
        /// SystemConfigSettingError
        /// System configuration error, there may be something wrong on system preference setting data,
        /// web.config or background service config
        /// </summary>
        SystemConfigSettingError = 102,

     
        ///////////////////////////////////////////////////////////////////////////////////
        //Category above 1000 are application level category.
        ///////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Exception is unhandled, but capture by global exception hanlder.
        /// The exception is usually unknown but critical, and it will throw to user. 
        /// User normally see the exception message on the CustomError page.
        /// </summary>
        UnhandledException = 1000,

        /// <summary>
        /// UserConfigSettingError
        /// User setting error, there maybe some user configuration missing, like team setup etc.
        /// ususally, the system continue to work in general, but it will fail in certain case
        /// if such config setting is needed.
        /// </summary>
        UserConfigSettingError = 1002,

        /// <summary>
        /// InvalidDataFormatError
        /// This may be due to user enter wrong data from the UI, and the mis-format data pass through business layer.
        /// </summary>
        InvalidDataFormatError = 1003,

        /// <summary>
        ///SilentException 
        ///The exception is unknown but not critical, but capture for trouble shooting investigation. 
        ///The exception is also not thrown to user so it won't break the user workflow.
        /// </summary>
        SilentException = 1004,

        InvalidArgumentError = 1005,
    }
}
