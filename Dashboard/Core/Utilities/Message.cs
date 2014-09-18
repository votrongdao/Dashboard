using System;
using System.Collections.Generic;
using System.Text;

namespace DashboardSite.Core.Utilities
{
	public enum MessageType
	{
		None = 0,
		Information = 1,
		Warning = 2,
		Error = 3
	}

	[Serializable]
	public class Message
	{
		private MessageType m_eType;
		private string m_sMessage;
		private Exception m_oException;

		public Message()
		{
		}

		public Message(MessageType eType, string sMessage)
		{
			m_eType = eType;
			m_sMessage = sMessage;
		}

		public Message(System.Exception ex)
		{
			m_oException = ex;
			m_eType = MessageType.Error;
			m_sMessage = ex.Message;
		}

		public MessageType MessageType
		{
			get { return m_eType; }
			set { m_eType = value; }
		}

		public string MessageText
		{
			get { return m_sMessage; }
			set { m_sMessage = value; }
		}

		public Exception InnerExcpetion
		{
			get { return m_oException; }
		}
	}
}
