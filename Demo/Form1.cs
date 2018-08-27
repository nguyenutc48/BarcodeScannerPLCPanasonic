using FP_CONNECTLib;
using Motorola.Snapi;
using Motorola.Snapi.Constants;
using Motorola.Snapi.Constants.Enums;
using Motorola.Snapi.EventArguments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Demo
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #region Define
        // PLC errors
        const int NO_FP_CONNECT_ERROR = 0;
        const int FP_CONNECT_ERROR_BASE = -20000;
        const int ERROR_PORT_CLOSED = FP_CONNECT_ERROR_BASE - 1;
        const int ERROR_WRITEDATA_MISSING = FP_CONNECT_ERROR_BASE - 2;
        const int ERROR_STATIONNUMBER_OUT_OF_RANGE = FP_CONNECT_ERROR_BASE - 3;
        const int ERROR_DOWNLOAD_PLCCODE_MISSING = FP_CONNECT_ERROR_BASE - 4;
        const int ERROR_REGISTER_OUT_OF_RANGE = FP_CONNECT_ERROR_BASE - 5;
        const int ERROR_STRING_TO_LARGE = FP_CONNECT_ERROR_BASE - 6;
        const int ERROR_INVALID_LANGUAGE_CODE = FP_CONNECT_ERROR_BASE - 8;
        const int ERROR_INVALID_COMSETTING = FP_CONNECT_ERROR_BASE - 9;
        const int ERROR_COMSETTING_PORTISOPEN = FP_CONNECT_ERROR_BASE - 10;
        const int ERROR_COMSETTING_INVALID_SIZE = FP_CONNECT_ERROR_BASE - 11;
        const int ERROR_INVALID_RTC_DATATYPE = FP_CONNECT_ERROR_BASE - 12;
        const int ERROR_INCOMPLETE_RTC_DATA = FP_CONNECT_ERROR_BASE - 13;

        // FP File Import
        const int ERROR_FP_INVALIDPATH = FP_CONNECT_ERROR_BASE - 14;
        const int ERROR_FP_NO_FP_FILE = FP_CONNECT_ERROR_BASE - 15;
        const int ERROR_FP_FILEOPEN = FP_CONNECT_ERROR_BASE - 16;
        const int ERROR_FP_INVALIDHEADER1 = FP_CONNECT_ERROR_BASE - 17;
        const int ERROR_FP_INVALIDHEADER2 = FP_CONNECT_ERROR_BASE - 18;
        const int ERROR_FP_INVALIDHEADER3 = FP_CONNECT_ERROR_BASE - 19;
        const int ERROR_FP_INVALIDVERSION = FP_CONNECT_ERROR_BASE - 20;
        const int ERROR_FP_PLC_NOTSUPPORTED = FP_CONNECT_ERROR_BASE - 21;
        const int ERROR_FP_INVALIDHEADER6 = FP_CONNECT_ERROR_BASE - 22;
        const int ERROR_SYSREG_FORMAT1 = FP_CONNECT_ERROR_BASE - 23;
        const int ERROR_SYSREG_FORMAT2 = FP_CONNECT_ERROR_BASE - 24;
        const int ERROR_SYSREG_FORMAT3 = FP_CONNECT_ERROR_BASE - 25;

        const int ERROR_PROGRAM_FORMAT1 = FP_CONNECT_ERROR_BASE - 26;
        const int ERROR_PROGRAM_FORMAT2 = FP_CONNECT_ERROR_BASE - 27;
        const int ERROR_PROGRAM_FORMAT3 = FP_CONNECT_ERROR_BASE - 28;
        const int ERROR_PROGRAM_FORMAT4 = FP_CONNECT_ERROR_BASE - 29;
        const int ERROR_PROGRAM_FORMAT5 = FP_CONNECT_ERROR_BASE - 30;

        const int ERROR_MACHINE_FORMAT1 = FP_CONNECT_ERROR_BASE - 31;
        const int ERROR_MACHINE_FORMAT2 = FP_CONNECT_ERROR_BASE - 32;
        const int ERROR_MACHINE_FORMAT3 = FP_CONNECT_ERROR_BASE - 33;
        const int ERROR_MACHINE_FORMAT4 = FP_CONNECT_ERROR_BASE - 34;
        const int ERROR_CONFIG_FORMAT1 = FP_CONNECT_ERROR_BASE - 35;
        const int ERROR_CONFIG_FORMAT2 = FP_CONNECT_ERROR_BASE - 36;
        const int ERROR_CONFIG_FORMAT3 = FP_CONNECT_ERROR_BASE - 37;
        const int ERROR_CONFIG_FORMAT4 = FP_CONNECT_ERROR_BASE - 38;
        const int ERROR_UPLOAD_PLCHEADER = FP_CONNECT_ERROR_BASE - 39;
        const int ERROR_PLCTYPE_MISMATCH = FP_CONNECT_ERROR_BASE - 40;
        const int ERROR_FPFILE_DOWNLOAD = FP_CONNECT_ERROR_BASE - 41;
        const int ERROR_PLC_COMMUNICATION = FP_CONNECT_ERROR_BASE - 42;
        const int ERROR_DOWNLOAD_STRING = FP_CONNECT_ERROR_BASE - 43;
        const int ERROR_BINARYUPLOAD_STRING = FP_CONNECT_ERROR_BASE - 44;

        // Login errors
        const int ERROR_GETPASSWORD_DATA = FP_CONNECT_ERROR_BASE - 45;
        const int ERROR_UPLOADPROTECTED = FP_CONNECT_ERROR_BASE - 46;
        const int ERROR_ATTEMPTS_EXCEEDED = FP_CONNECT_ERROR_BASE - 47;
        const int ERROR_OCXINTERNAL = FP_CONNECT_ERROR_BASE - 48;
        const int ERROR_PLCLOGINTIME = FP_CONNECT_ERROR_BASE - 49;
        const int ERROR_PASSWORD_INVALID = -74; // compatibility with communication manger 
        const int ERROR_PORTISOPEN = -84;   // compatibility with communication manger 

        // Warnings
        const int FP_CONNECT_WARNING_BASE = -25000;
        const int WARNING_PLC_STRING_SIZE = FP_CONNECT_WARNING_BASE - 1;
        const int WARNING_PLC_UPLOADPROTECTED = FP_CONNECT_WARNING_BASE - 2;

        // Mewnetmanager language codes
        const int RESOURCE_CASE_JAPANESE = 1;
        const int RESOURCE_CASE_ENGLISH = 0;
        const int RESOURCE_CASE_CHINESE = 2;
        const int RESOURCE_CASE_CHINESE_T = 3;
        const int RESOURCE_CASE_KOREAN = 4;
        const int RESOURCE_CASE_SPANISH = 5;
        const int RESOURCE_CASE_ITALIAN = 6;
        const int RESOURCE_CASE_GERMAN = 7;
        const int RESOURCE_CASE_FRENCH = 8;

        // Port open methode			
        const int OPEN_PLC = 0;
        const int OPEN_GT = 1;

        // Port open return codes
        const int IDOK = 1;
        const int IDCANCEL = 2;
        const int IDABORT = 3;
        const int MEWMODEM_IDCONNECT = 10;

        // Response on GetPortOpenStatus
        const int PORTCLOSED = 0;
        const int PORTOPEN_ON_MODEM = 1;
        const int PORTOPEN = 2;

        // Password			
        const int NEWPASSWORD = 0;
        const int DELETEPASSWORD = 1;
        const int PLC_LOGIN = 2;
        const int PLC_LOGOUT = 3;
        const int RESTRICTED_PLC = 0;
        const int NOTRESTRICTED_PLC = 8;
        const int LOGIN_FORUPLOAD = 0; // 32 bit PLC
        const int LOGIN_FORDOWNLOAD = 1; // 32 bit PLC

        // Password array indices
        const int PW_UPLOAD_PROTECTION = 0;
        const int PW_LENGTH = 1;
        const int PW_CLOSEDSTATUS = 2;
        const int PW_RETRYCOUNTER = 3;
        const int PW_RESTRICTION = 4;


        // PLC Mode
        const int PLC_PROG_MODE = 0;    // bit 0 return by function IsPLCInRunMode
        const int PLC_RUN_MODE = 1; // bit 0 return by function IsPLCInRunMode. if used together with ReadPLCInformation RUN, TEST, REMOTE are combined as bit positions

        // New generation PLC
        const int PLC_TYPE_16BIT = 0;       //FP0, FP-X, FP-SIGMA, FP2, FP2SH
        const int PLC_TYPE_32BIT = 1;       //FP7

        const int AREA_WORDMODE = 1;
        const int AREA_BITMODE = 0;
        // Clear FP7 plc memory types
        const int INITIALIZE_OPERATIONMEMORY = 0;
        const int INITIALIZE_GLOBALDEVICES = 1;
        const int INITIALIZE_LOCALDEVICES = 2;

        // PLC Information
        const int INFO_PLCTYPE = 0;
        const int INFO_PLCVERSION = 1;
        const int INFO_PLCPROSIZE = 2;
        const int INFO_PLCMODE = 3;
        const int INFO_PLCLINKUNIT = 4;
        const int INFO_PLCERRORFLAG = 5;
        const int INFO_PLCWERROR = 6;
        const int INFO_PLCHARDWARE = 7;
        const int INFO_PLCPRONUM = 8;
        const int INFO_PLCPROALLSIZE = 9;
        const int INFO_PLCHEADERSIZE = 10;
        const int INFO_PLCSYSREGSIZE = 11;
        const int INFO_PLCPORTNUMBER = 12;
        const int INFO_PLCCOMMEMTYPE = 13;
        const int INFO_PLCCOMMEMSIZE = 14;
        const int INFO_PLCCOMMEMBLKSIZE = 15;
        // hold register
        const int INFO_PLCCOUNTERSTART = 16;
        const int INFO_PLCTIMERCOUNTERHOLD = 17;
        const int INFO_PLC_WRHOLD = 18;
        const int INFO_PLC_DTHOLD = 19;
        const int INFO_PLC_FLHOLD = 20;
        const int INFO_PLC0_WLHOLD = 21;
        const int INFO_PLC1_WLHOLD = 22;
        const int INFO_PLC0_LDHOLD = 23;
        const int INFO_PLC1_LDHOLD = 24;
        const int INFO_PLC_STEPLADDERHOLD = 25;

        const int TRANSPARENT_NO_RESPONSE = 0;
        const int TRANSPARENT_RESPONSE = 1;

        // Communication setting
        // Type
        const int COMSETTING_CNET = 1;
        const int COMSETTING_USB = 10;
        const int COMSETTING_MODEM = 6;
        const int COMSETTING_ETHERNET = 5;
        // Type size	
        const int COMSETTING_CNET_SIZE = 8;
        const int COMSETTING_USB_SIZE = 3;
        const int COMSETTING_MODEM_SIZE = 9;
        const int COMSETTING_ETHERNET_SIZE = 9;

        // Serial settings
        const int COMSETTING_BAUD1200 = 0;
        const int COMSETTING_BAUD2400 = 1;
        const int COMSETTING_BAUD4800 = 2;
        const int COMSETTING_BAUD9600 = 3;
        const int COMSETTING_BAUD19200 = 4;
        const int COMSETTING_BAUD38400 = 5;
        const int COMSETTING_BAUD57600 = 6;
        const int COMSETTING_BAUD115200 = 7;
        const int COMSETTING_BAUD230400 = 8;
        const int COMSETTING_LENGHT7 = 7;
        const int COMSETTING_LENGHT8 = 8;
        const int COMSETTING_PARITY_NO = 0;
        const int COMSETTING_PARITY_ODD = 1;
        const int COMSETTING_PARITY_EVEN = 2;
        const int COMSETTING_PARITY_NULL = 3;
        const int COMSETTING_STOPBIT1 = 1;
        const int COMSETTING_STOPBIT2 = 2;

        // Modem
        const int COMSETTING_DIAL_PULS = 0;
        const int COMSETTING_DIAL_TON = 1;
        const int COMSETTING_DIAL_OTHER = 2;

        // ETHERNET
        const int COMSETTING_PC_IP_AUTOMATIC = 1;
        const int COMSETTING_PC_IP_MANUAL = 0;
        const int COMSETTING_USE_ET_LAN = 1;
        const int COMSETTING_DONTUSE_ET_LAN = 2;
        const int COMSETTING_ETHERNET_PORT_AUTO = 0;    // COM Host Port automatic

        // Auto baudreate detection bits.  Logical OR different settings or use COMSETTING_AUTODETECT_ALL 
        const int COMSETTING_AUTODETECT_NO = 0;
        const int COMSETTING_AUTODETECT_LENGTH = 1;
        const int COMSETTING_AUTODETECT_PARITY = 2;
        const int COMSETTING_AUTODETECT_BAUDRATE = 8;
        const int COMSETTING_AUTODETECT_ALL = 11;

        // GetCurrentlyUsedCommunicationParameter and SetCommunicationParameter uses an array for the COM Type specific items which are addressed with the defines below
        const int COMARRAY_COMTYPE = 0;
        const int COMARRAY_COMPORT = 1;
        const int COMARRAY_COMBAUDRATE = 2;
        const int COMARRAY_COMLENGHT = 3;
        const int COMARRAY_COMSTOPBIT = 4;
        const int COMARRAY_COMPARITY = 5;
        const int COMARRAY_COMAUTOSETTINGS = 6;
        const int COMARRAY_COMTIMEOUT = 7;
        #endregion
        FP_CONNECTClass m_ocxFP_CONNECT = null;
        private const string ImageSaveLocation = @"C:\Users\Jason\Desktop\Image.jpg";

        private static string _lastScanned;

        private static bool _scannerAttached;
        public Form1()
        {
            InitializeComponent();
            try
            {
                m_ocxFP_CONNECT = new FP_CONNECTClass();
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot create FP Connect");
                return;
            }
            m_ocxFP_CONNECT.AttachHostHandle((int)Handle);
        }

        private void Instance_ImageReceived(object sender, ImageEventArgs e)
        {
            e.Image.Save(ImageSaveLocation, e.Format);
            Console.WriteLine("Image saved to \"{0}\"", ImageSaveLocation);
        }

        private void Instance_DataReceived(object sender, BarcodeScanEventArgs e)
        {
            _lastScanned = e.Data;
            var raw = Encoding.Default.GetString(e.RawData);
            //Console.WriteLine("Barcode type: " + e.BarcodeType.GetDescription());
            //Console.WriteLine("Data: " + e.Data);
            Invoke((ThreadStart)delegate
            {
                lblbarcodeshow.Text = e.Data;
                lblnumberofcode.Text = e.Data.Length.ToString();
            });
            //lblbarcodeshow.Text = e.Data;
        }

        private void Instance_ScannerDetached(object sender, PnpEventArgs e)
        {
            _scannerAttached = false;
        }

        private void Instance_ScannerAttached(object sender, PnpEventArgs e)
        {
            _scannerAttached = true;
            ConnectScanners();
        }

        private void ConnectScanners()
        {
            foreach (var scanner in BarcodeScannerManager.Instance.GetDevices())
            {
                scanner.SetHostMode(HostMode.USB_SNAPI_Imaging);
                if (scanner.Info.UsbHostMode != HostMode.USB_SNAPI_Imaging)
                {
                    scanner.SetHostMode(HostMode.USB_SNAPI_Imaging);
                    while (_scannerAttached == false)
                    {
                        Thread.Sleep(3000);
                    }
                }

                //scanner.Defaults.Restore();
                //scanner.CaptureMode = CaptureMode.Barcode;
                //GetAttributes(scanner);

                //PerformCommands(scanner);
                //scanner.Trigger.TriggerByCommand = true;
                //scanner.Trigger.PullTrigger();

                scanner.Actions.SetAttribute(138, 'B', 0); // sound beeper via attribute
            }
        }

        private void Sample_SetcommunicationDirectC_NET(FP_CONNECTClass ocxInstance)
        {
            int[] arIntWriteData = new int[COMSETTING_CNET_SIZE];
            arIntWriteData[0] = COMSETTING_CNET;
            arIntWriteData[1] = 1;
            arIntWriteData[2] = COMSETTING_BAUD115200;
            arIntWriteData[3] = COMSETTING_LENGHT8;
            arIntWriteData[4] = COMSETTING_STOPBIT1;
            arIntWriteData[5] = COMSETTING_PARITY_ODD;
            arIntWriteData[6] = COMSETTING_AUTODETECT_ALL;
            arIntWriteData[7] = 25;         // timeout            
            int nResult = ocxInstance.SetCommunicationParameter(arIntWriteData, "", "", "");
            // check error code in nResult
        }// Sample_SetcommunicationDirectC_NET

        /* Sample_SetcommunicationDirectC_NET
        *  set the communication parameters of the current connection without utilizing the communication settings dialog
         *  This sample shows how to set the USB communication parameter
        */
        private void Sample_SetcommunicationDirect_USB(FP_CONNECTClass ocxInstance)
        {
            // Sample shows how to read the C_NET communication parameter without openeing the communication settings dialog
            int[] arIntWriteData = new int[COMSETTING_USB_SIZE];
            arIntWriteData[0] = COMSETTING_USB;
            arIntWriteData[1] = 20;         // timeout            
            int nResult = ocxInstance.SetCommunicationParameter(arIntWriteData, "", "", "");
        }// Sample_SetcommunicationDirect_USB

        private void Form1_Load(object sender, EventArgs e)
        {
            //Scanner
            _scannerAttached = false;
            BarcodeScannerManager.Instance.Open();            
            BarcodeScannerManager.Instance.RegisterForEvents(EventType.Barcode, EventType.Pnp, EventType.Image, EventType.Other, EventType.Rmd);
            //var b = BarcodeScannerManager.Instance.RegisteredEvents;
            //BarcodeScannerManager.Instance.Keyboard.EnableEmulation = false;
            //BarcodeScannerManager.Instance.ScannerAttached += Instance_ScannerAttached;
            //BarcodeScannerManager.Instance.ScannerDetached += Instance_ScannerDetached;
            var a = BarcodeScannerManager.Instance.DriverVersion;
            ConnectScanners();
            BarcodeScannerManager.Instance.DataReceived += Instance_DataReceived;
            //BarcodeScannerManager.Instance.ImageReceived += Instance_ImageReceived;

            //PLC
            Sample_SetcommunicationDirect_USB(m_ocxFP_CONNECT);
            byte[] byteData = new byte[110];
            if (m_ocxFP_CONNECT.PortOpen(OPEN_PLC, 1) != NO_FP_CONNECT_ERROR)
            {
                // check errorcode
                return;
            }
            if (m_ocxFP_CONNECT.PortOpen(OPEN_PLC, 1) != NO_FP_CONNECT_ERROR)
            {
                this.m_ocxFP_CONNECT.PortClose();
                // check errorcode
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            this.button2.BackColor = System.Drawing.Color.Red;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            this.button2.BackColor = System.Drawing.Color.LightGray;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            this.button1.BackColor = System.Drawing.Color.Yellow;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.button1.BackColor = System.Drawing.Color.LightGray;
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
                notifyIcon.Visible = true;
                //notifyIcon.ShowBalloonTip(500);
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }
    }
}
