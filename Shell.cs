using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Aida64_Esp8266_DisplayControler
{
    #region Class Shell
    public class Shell
    {
        #region Members
        private string _filename;
        private string _args;
        private string _workdir;
        private Process _process;


        public string FileName { get { return this._filename; } }
        public string Args { get { return this._args; } }
        public string WorkDir { get { return this._workdir; } }
        public bool HasExited { get { return this._process.HasExited; } }
        #endregion
        #region Constructor
        public Shell(string filename, string args, string workdir, DataReceivedEventHandler OutputDataReceived, EventHandler ShellExited)
        {
            this._filename = filename;
            this._args = args;
            this._workdir = workdir;
            this._process = new Process();
            this._process.StartInfo.FileName = this._filename;
            this._process.StartInfo.Arguments = this._args;
            this._process.StartInfo.WorkingDirectory = this._workdir;
            this._process.StartInfo.RedirectStandardOutput = true;
            this._process.StartInfo.RedirectStandardError = true;
            this._process.StartInfo.UseShellExecute = false;
            this._process.StartInfo.CreateNoWindow = true;
            this._process.EnableRaisingEvents = true;
            this._process.OutputDataReceived += OutputDataReceived;
            this._process.ErrorDataReceived += OutputDataReceived;
            this._process.Exited += ShellExited;
        }
        #endregion
        #region Methods
        public void Start()
        {
            _process.Start();
            this._process.BeginOutputReadLine();
            this._process.BeginErrorReadLine();
        }
        
        public void Stop()
        {
            _process.Kill();
            _process.CancelOutputRead();
            _process.CancelErrorRead();
        }
        #endregion

    }
    #endregion
}
