using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatcher_lab3
{
    public partial class LR3 : ServiceBase
    {
        file_Manager file_Manager;
        public LR3()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
             file_Manager = new file_Manager();
            Thread loggerThread = new Thread(new ThreadStart(file_Manager.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            file_Manager.Stop();
            Thread.Sleep(1000);
        }

        

            
    }

}
