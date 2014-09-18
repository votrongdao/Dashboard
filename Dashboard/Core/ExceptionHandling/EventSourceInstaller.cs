using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Threading;

namespace iPreo.Bigdough.ExceptionHandling
{
    [RunInstaller(true)]
    public partial class EventSourceInstaller : Installer
    {
        private EventLogInstaller mEventLogInstaller;
        public EventSourceInstaller()
        {
            InitializeComponent();
            // Create an instance of an EventLogInstaller.
            mEventLogInstaller = new EventLogInstaller();
            // Set the source name of the event log.
            mEventLogInstaller.Source = ConstantString.BillfoldAppEventSource;
            // Set the event log that the source writes entries to.
            mEventLogInstaller.Log = ConstantString.ApplicationLog;
            // Add myEventLogInstaller to the Installer collection.
            Installers.Add(mEventLogInstaller);
        }
    }
}
