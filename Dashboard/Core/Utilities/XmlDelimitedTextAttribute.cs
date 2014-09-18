#region Header
/*****************************************************************
** $Header: /iPreo Bigdough System/Utilities/iPreo.Bigdough.Utilities/XmlDelimitedTextAttribute.cs 1     5/03/07 4:46p Lus $
**
 * Created By Lus
** Created on  Wednesday, May 2, 2007
** All rights reserved, i-Deal LLC
**
** DESCRIPTION 
** XmlDelimitedTextAttribute class
**
** $Log: /iPreo Bigdough System/Utilities/iPreo.Bigdough.Utilities/XmlDelimitedTextAttribute.cs $
 * 
 * 1     5/03/07 4:46p Lus
 * 
 * 
*******************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace iPreo.Bigdough.Utilities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XmlDelimitedTextAttribute : System.Attribute
    {
        private char[] m_chSeperator;

        public XmlDelimitedTextAttribute(char[] chSeperator)
        {
            m_chSeperator = chSeperator;
        }

        public char[] Seperator
        {
            get { return m_chSeperator; }
            set { m_chSeperator = value; }
        }
    }
}
