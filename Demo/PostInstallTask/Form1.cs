using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Media;
using System.Runtime.InteropServices;

namespace PostInstallTask;

public partial class Form1 : Form
{
    private int tiempoRestante = 300; // 300 segundos = 5 minutos

    public Form1()
    {
        InitializeComponent();

        timerCuentaRegresiva.Interval = 1000;
        timerCuentaRegresiva.Tick += TimerCuentaRegresiva_Tick;
        timerCuentaRegresiva.Start();

#if !DEBUG
        button1.Visible = false;
#endif

    }

    private void TimerCuentaRegresiva_Tick(object sender, EventArgs e)
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante--;

            int minutos = tiempoRestante / 60;
            int segundos = tiempoRestante % 60;

            labelCuentaRegresiva.Text = $"{minutos:D2}:{segundos:D2}";
        }
        else
        {
            timerCuentaRegresiva.Stop();

            //El sistema se reiniciara ahora! :boom: ;
            Process.Start("shutdown", "/r /f /t 0");
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        timerCuentaRegresiva.Stop();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        Process.Start("shutdown", "/r /f /t 0");
    }
}
