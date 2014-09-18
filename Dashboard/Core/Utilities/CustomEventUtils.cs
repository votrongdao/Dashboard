#region All rights reserved, i-Deal LLC

/*****************************************************************
 * ** $Header: /iPreo Bigdough System/Utilities/iPreo.Bigdough.Utilities/CustomEventUtils.cs 3     3/06/08 1:11p Vkryvosheiev $
 * **
 * ** Created By: Vitaliy Kryvosheiev
 * ** Created on  2007.08.15
 * ** All rights reserved, i-Deal LLC
 * **
 * ** DESCRIPTION 
 * ** Event Utils
 * **
 * ** $Log: /iPreo Bigdough System/Utilities/iPreo.Bigdough.Utilities/CustomEventUtils.cs $
 * 
 * 3     3/06/08 1:11p Vkryvosheiev
 * Fixing http://pmcpct.epam.com/pmc/bug/detail.do?id=3680741400002808980,
 * CQ: ideal00128216
 * 
 * 2     8/15/07 11:53a Vkryvosheiev
 * refactored
 * 
 * 1     8/15/07 11:41a Vkryvosheiev
 * 
 * 
 * **
 * *******************************************************************/

#endregion All rights reserved, i-Deal LLC

using System;


namespace iPreo.Bigdough.Utilities
{
    /// <summary>
    /// Custom Event Helper
    /// </summary>
    /// <typeparam name="TData">Event data Type</typeparam>
    public static class CustomEventUtils<TData>
    {
        #region Event Arguments

        /// <summary>
        /// Event with custom data
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        public class CustomEventArgs : EventArgs
        {
            public CustomEventArgs(TData oEventData)
            {
                m_oEventData = oEventData;
            }


            private TData m_oEventData;

            public TData EventData
            {
                get { return m_oEventData; }
                set { m_oEventData = value; }
            }
        }

        #endregion Event Arguments

        public delegate void CustomEventHandler(object sender, CustomEventArgs eventArgs);


        /// <summary>
        /// Fire Event helper
        /// </summary>
        /// <param name="handler">Event handler</param>
        /// <param name="sender">Sender</param>
        /// <param name="eventData">Event data</param>
        /// <returns>True if event was fired</returns>
        public static bool FireEvent(CustomEventHandler handler, object sender, TData eventData)
        {
            bool bCanFire = handler != null;
            if (bCanFire)
            {
                CustomEventArgs eventArgs = new CustomEventArgs(eventData);
                handler(sender, eventArgs);
            }
            return bCanFire;
        }


        /// <summary>
        /// Fire Event helper
        /// </summary>
        /// <param name="handler">Event handler</param>
        /// <param name="sender">Sender</param>
        /// <param name="eventData">Event data</param>
        /// <returns>True if event was fired</returns>
        public static TData FireEventEx(CustomEventHandler handler, object sender, TData eventData)
        {
            if (handler != null)
            {
                CustomEventArgs eventArgs = new CustomEventArgs(eventData);
                handler(sender, eventArgs);
                eventData = eventArgs.EventData;
            }
            return eventData;
        }
    }
}