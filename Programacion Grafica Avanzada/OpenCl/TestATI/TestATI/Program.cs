using System;
using OpenCL.Net;

class Program
{
    static void Main()
    {
        ErrorCode error;

        Platform[] platforms = Cl.GetPlatformIDs(out error);

        Console.WriteLine("Plataformas OpenCL encontradas: " + platforms.Length);

        foreach (Platform platform in platforms)
        {
            string platformName =
                Cl.GetPlatformInfo(platform, PlatformInfo.Name, out error).ToString();

            Console.WriteLine("Plataforma: " + platformName);

            Device[] devices =
                Cl.GetDeviceIDs(platform, DeviceType.Gpu, out error);

            foreach (Device device in devices)
            {
                string deviceName =
                    Cl.GetDeviceInfo(device, DeviceInfo.Name, out error).ToString();

                string vendor =
                    Cl.GetDeviceInfo(device, DeviceInfo.Vendor, out error).ToString();

                string version =
                    Cl.GetDeviceInfo(device, DeviceInfo.Version, out error).ToString();

                Console.WriteLine("GPU detectada: " + deviceName);
                Console.WriteLine("Fabricante: " + vendor);
                Console.WriteLine("Versión OpenCL: " + version);
                Console.WriteLine("---------------------------");
            }
        }
    }
}