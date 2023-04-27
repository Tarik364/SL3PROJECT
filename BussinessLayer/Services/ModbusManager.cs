using System;
using System.Threading.Tasks;
using EasyModbus;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Drawing;

namespace BussinessLayer.Services
{
    public class ModbusManager
    {
        public ModbusClient modbusClient;

        public async Task<bool> CONTROL()
        {
            return modbusClient?.Connected ?? false;
        }

        public async Task<bool> Connect(string ipAddress)
        {
            bool isConnect;
            try
            {
                modbusClient = new ModbusClient(ipAddress, 502);
                modbusClient.Connect();

                isConnect = true; // Bağlantı başarılı ise true döndür
            }
            catch (Exception)
            {
                // Hata durumunda false döndür
                isConnect = false;
            }
            return isConnect;
        }
        public async Task<bool> Disconnect(string ipAddress)
        {
            bool isConnected;
            try
            {
                modbusClient = new ModbusClient(ipAddress, 502);
                modbusClient.Disconnect();
                isConnected = false;
            }
            catch (Exception)
            {
                isConnected = true;
            }
            return isConnected;

        }

        public async Task<string> coil1(string status)
        {
            try
            {
                if (status == "Kapalı")
                {
                    modbusClient.WriteSingleCoil(0, true);
                    status = "Açık";
                }
                else
                {
                    modbusClient.WriteSingleCoil(0, false);
                    status = "Kapalı";
                }
                return status;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task mButton(int address)
        {
            try
            {
                modbusClient.WriteSingleCoil(address, true);
                modbusClient.WriteSingleCoil(address, false);
            }
            catch (Exception ex)
            {
            }
        }
        public async Task mStop(int address)
        {
            try
            {
                modbusClient.WriteSingleCoil(address, true);
                modbusClient.WriteSingleCoil(address, false);
            }
            catch (Exception ex)
            {
            }
        }
        public async Task<string> metreSay(int address)
        {
            int[] readHoldingRegisters;
            string metre;
            bool baglanti = await CONTROL();
            while (true)
            {
                readHoldingRegisters = modbusClient.ReadHoldingRegisters(address, 1);
                double dreadHoldingRegisters = Convert.ToDouble(readHoldingRegisters[0]);
                metre = (dreadHoldingRegisters / 1000).ToString();
                return metre;
            }
            return "baglanti hatası";
        }

        public void sifirla(int adres)
        {
            int[] sifirlaRegister = new int[] { 0,00 };
            modbusClient.WriteMultipleRegisters(10, sifirlaRegister);
        }



    }
}
