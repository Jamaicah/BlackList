using System;
using System.Management;
using System.Windows.Forms;

namespace DemonsMC_HWID_System
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
			
			new Form1.clsComputerInfo();
			Form1.clsComputerInfo clsComputerInfo = new Form1.clsComputerInfo();
			string processorId = clsComputerInfo.GetProcessorId();
			string volumeSerial = clsComputerInfo.GetVolumeSerial("C");
			string motherBoardID = clsComputerInfo.GetMotherBoardID();
			string macaddress = clsComputerInfo.GetMACAddress();

			string a = macaddress + motherBoardID + volumeSerial + processorId;
			MessageBox.Show("Il Tuo HWID " + a);
			Clipboard.SetText(a);

			if (a == "hwid tipo")
			{
				panel1.Show();
			}
			else
			{
				panel2.Show();
			}
		}
		public class clsComputerInfo
		{
			internal string GetProcessorId()
			{
				string result = string.Empty;
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher(new SelectQuery("Win32_processor")).Get())
				{
					result = managementBaseObject.GetPropertyValue("processorId").ToString();
				}
				return result;
			}

			// Token: 0x060000BA RID: 186 RVA: 0x00009CB8 File Offset: 0x00007EB8
			internal string GetMACAddress()
			{
				ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
				string text = string.Empty;
				foreach (ManagementBaseObject managementBaseObject in instances)
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					if (text.Equals(string.Empty))
					{
						if (Convert.ToBoolean(managementObject.GetPropertyValue("IPEnabled")))
						{
							text = managementObject.GetPropertyValue("MacAddress").ToString();
						}
						managementObject.Dispose();
					}
					text = text.Replace(":", string.Empty);
				}
				return text;
			}

			// Token: 0x060000BB RID: 187 RVA: 0x00009D5C File Offset: 0x00007F5C
			internal string GetVolumeSerial(string strDriveLetter = "C")
			{
				ManagementObject managementObject = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}:\"", strDriveLetter));
				managementObject.Get();
				return managementObject.GetPropertyValue("VolumeSerialNumber").ToString();
			}

			// Token: 0x060000BC RID: 188 RVA: 0x00009D84 File Offset: 0x00007F84
			public string GetMotherBoardID()
			{
				string result = string.Empty;
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher(new SelectQuery("Win32_BaseBoard")).Get())
				{
					result = managementBaseObject.GetPropertyValue("product").ToString();
				}
				return result;
			}
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
