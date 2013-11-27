namespace H2O.Applications.SixtyFourBitAssemblyChecker
{
    #region using Directives

    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows;

    #endregion

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [ComVisible(false)]
    public partial class App : Application
    {
        static MachineType ReadMachineType(Stream stream)
        {
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                stream.Seek(0x3c, SeekOrigin.Begin);
                Int32 peOffset = binaryReader.ReadInt32();

                stream.Seek(peOffset, SeekOrigin.Begin);
                UInt32 peHead = binaryReader.ReadUInt32();

                if (peHead != 0x00004550) // "PE\0\0", little-endian
                {
                    throw new PEHeaderNotFoundException();
                }

                return (MachineType)binaryReader.ReadUInt16();
            }
        }

        static void DisplayMachineType(MachineType type)
        {
            switch (type)
            {
                case MachineType.IMAGE_FILE_MACHINE_I386:
                    MessageBox.Show("Assembly is 32-bit.");
                    break;

                case MachineType.IMAGE_FILE_MACHINE_AMD64:
                case MachineType.IMAGE_FILE_MACHINE_IA64:
                    MessageBox.Show("Assembly is 64-bit.");
                    break;

                default:
                    MessageBox.Show("Assembly is unknown.");
                    break;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            Nullable<bool> result = dialog.ShowDialog();

            if (result != true)
            {
                Shutdown();
                return;
            }

            MachineType machineType;

            FileInfo fileInfo = new FileInfo(dialog.FileName);

            try
            {
                using (FileStream filestream = fileInfo.Open(FileMode.Open, FileAccess.Read))
                {
                    machineType = ReadMachineType(filestream);
                }
            }
            catch (PEHeaderNotFoundException error)
            {
                MessageBox.Show(error.Message);
                Shutdown();
                return;
            }

            DisplayMachineType(machineType);

            Shutdown();
        }
    } // class
} // namespace
