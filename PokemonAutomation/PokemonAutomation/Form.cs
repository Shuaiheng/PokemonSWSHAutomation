﻿using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PokemonAutomation
{
    public partial class PokemonAutomation : Form
    {
        enum ButtonType : byte
        {
            RIGHT = 0,
            LEFT,
            UP,
            DOWN,
            A,
            B,
            X,
            Y,
            R,
            L,
            ZR,
            ZL,
            RSTICK,
            LSTICK,
            RCLICK,
            LCLICK,
            HOME,
            CAPTURE,
            PLUS,
            MINUS
        }

        enum ButtonState : byte
        {
            PRESS = 0,
            RELEASE
        }

        enum Stick : byte
        {
            MIN = 0,
            CENTER = 128,
            MAX = 255,
            CUSTOM_EGG = 50,
            CUSTOM_INCUBATE = 70
        }

        private int[,] numberPanel = new int[10, 2] { { 3, 1 }, { 0, 0 }, { 0, 1 }, { 0, 2 }, { 1, 0 }, { 1, 1 }, { 1, 2 }, { 2, 0 }, { 2, 1 }, { 2, 2 } };

        private CancellationTokenSource token_source;
        private CancellationToken cancel_token;
        private uint day_count;
        private DateTime current_date;


        public PokemonAutomation()
        {
            InitializeComponent();
        }

        private void getSerialPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            comPortComboBox.Items.Clear();
            foreach (string port in ports)
            {
                comPortComboBox.Items.Add(port);
            }
            if (comPortComboBox.Items.Count > 0)
            {
                comPortComboBox.SelectedIndex = 0;
            }
        }

        private delegate void delegateUpdateDateLabel();

        private void updateDateLabel()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new delegateUpdateDateLabel(this.updateDateLabel));
                return;
            }
            LabelDate.Text = "当前日期： " + current_date.ToString("yyyy/MM/dd");
        }

        private delegate void delegateUpdateCountLabelWithRaidHole(int count, int max);

        private void updateCountLabelWithRaidHole(int count, int max)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new delegateUpdateCountLabelWithRaidHole(this.updateCountLabelWithRaidHole), count, max);
                return;
            }
            CountLabelWithRaidHole.Text = "已过：" + count.ToString();
        }

        private delegate void delegateUpdateCountLabel(int count, int max);

        private void updateCountLabel(int count, int max)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new delegateUpdateCountLabel(this.updateCountLabel), count, max);
                return;
            }
            CountLabel.Text = "已过： " + count.ToString();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            getSerialPorts();
            YearComboBox.SelectedIndex = 0;
            MonthComboBox.SelectedIndex = 0;
            DayComboBox.SelectedIndex = 0;

            updateDateLabel();
        }

        private void comPortComboBox_Click(object sender, EventArgs e)
        {
            getSerialPorts();
        }

        private void connectSerial()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            serialPort.BaudRate = 115200;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.PortName = comPortComboBox.Text;
            serialPort.Open();
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                //int data_len = serialPort.BytesToRead;
                //Byte[] data = new Byte[data_len];
                //serialPort.Read(data, 0, data_len);

                String data = serialPort.ReadExisting();

                Invoke((MethodInvoker)(() =>	// 受信用スレッドから切り替えてデータを書き込む
                {
                    Console.Write(data);
                    Thread.Sleep(1);
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comPortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            connectSerial();
        }

        private void pressButton(ButtonType button)
        {
            if (serialPort.IsOpen)
            {
                Byte[] data = new byte[2];
                data[0] = (Byte)button;
                data[1] = (Byte)ButtonState.PRESS;

                serialPort.Write(data, 0, 2);
            }
        }

        private void releaseButton(ButtonType button)
        {
            if (serialPort.IsOpen)
            {
                Byte[] data = new byte[2];
                data[0] = (Byte)button;
                data[1] = (Byte)ButtonState.RELEASE;

                serialPort.Write(data, 0, 2);
            }
        }

        private void left_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.LEFT);
        }

        private void left_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.LEFT);
        }

        private void right_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.RIGHT);
        }

        private void right_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.RIGHT);
        }

        private void up_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.UP);
        }

        private void up_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.UP);
        }

        private void down_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.DOWN);
        }

        private void down_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.DOWN);
        }

        private void ButtonA_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.A);
        }

        private void ButtonA_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.A);
        }

        private void ButtonB_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.B);
        }

        private void ButtonB_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.B);
        }

        private void ButtonY_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.Y);
        }

        private void ButtonY_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.Y);
        }

        private void ButtonX_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.X);
        }

        private void ButtonX_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.X);
        }

        private void ButtonZR_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.ZR);
        }

        private void ButtonZR_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.ZR);
        }

        private void ButtonZL_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.ZL);
        }

        private void ButtonZL_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.ZL);
        }

        private void ButtonR_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.R);
        }

        private void ButtonR_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.R);
        }

        private void ButtonL_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.L);
        }

        private void ButtonL_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.L);
        }

        private void ButtonPlus_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.PLUS);
        }

        private void ButtonPlus_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.PLUS);
        }

        private void ButtonMinus_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.MINUS);
        }

        private void ButtonMinus_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.MINUS);
        }

        private void ButtonHome_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.HOME);
        }

        private void ButtonHome_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.HOME);
        }

        private void ButtonCapture_MouseDown(object sender, MouseEventArgs e)
        {
            pressButton(ButtonType.CAPTURE);
        }

        private void ButtonCapture_MouseUp(object sender, MouseEventArgs e)
        {
            releaseButton(ButtonType.CAPTURE);
        }

        private void moveStick(ButtonType button, Stick x_stick, Stick y_stick)
        {
            if (serialPort.IsOpen)
            {
                Byte[] data = new byte[3];
                data[0] = (Byte)button;
                data[1] = (Byte)x_stick;
                data[2] = (Byte)y_stick;

                serialPort.Write(data, 0, 3);
            }
        }
        private void releaseStick(ButtonType button)
        {
            if (serialPort.IsOpen)
            {
                Byte[] data = new byte[3];
                data[0] = (Byte)button;
                data[1] = (Byte)Stick.CENTER;
                data[2] = (Byte)Stick.CENTER;

                serialPort.Write(data, 0, 3);
            }
        }

        private void LeftN_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.LSTICK, Stick.CENTER, Stick.MIN);
        }

        private void LeftN_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.LSTICK);
        }

        private void LeftS_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.LSTICK, Stick.CENTER, Stick.MAX);
        }

        private void LeftS_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.LSTICK);
        }

        private void LeftE_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.LSTICK, Stick.MAX, Stick.CENTER);
        }

        private void LeftE_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.LSTICK);
        }

        private void LeftW_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.LSTICK, Stick.MIN, Stick.CENTER);
        }

        private void LeftW_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.LSTICK);
        }

        private void LeftNE_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.LSTICK, Stick.MAX, Stick.MIN);
        }

        private void LeftNE_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.LSTICK);
        }

        private void LeftNW_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.LSTICK, Stick.MIN, Stick.MIN);
        }

        private void LeftNW_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.LSTICK);
        }

        private void LeftSE_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.LSTICK, Stick.MAX, Stick.MAX);
        }

        private void LeftSE_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.LSTICK);
        }

        private void LeftSW_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.LSTICK, Stick.MIN, Stick.MAX);
        }

        private void LeftSW_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.LSTICK);
        }

        private void RightN_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.RSTICK, Stick.CENTER, Stick.MIN);
        }

        private void RightN_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.RSTICK);
        }

        private void RightS_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.RSTICK, Stick.CENTER, Stick.MAX);
        }

        private void RightS_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.RSTICK);
        }

        private void RightE_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.RSTICK, Stick.MAX, Stick.CENTER);
        }

        private void RightE_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.RSTICK);
        }

        private void RightW_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.RSTICK, Stick.MIN, Stick.CENTER);
        }

        private void RightW_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.RSTICK);
        }

        private void RightNE_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.RSTICK, Stick.MAX, Stick.MIN);
        }

        private void RightNE_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.RSTICK);
        }

        private void RightSE_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.RSTICK, Stick.MAX, Stick.MAX);
        }

        private void RightSE_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.RSTICK);
        }

        private void RightSW_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.RSTICK, Stick.MIN, Stick.MAX);
        }

        private void RightSW_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.RSTICK);
        }

        private void RightNW_MouseDown(object sender, MouseEventArgs e)
        {
            moveStick(ButtonType.RSTICK, Stick.MAX, Stick.MAX);
        }

        private void RightNW_MouseUp(object sender, MouseEventArgs e)
        {
            releaseStick(ButtonType.RSTICK);
        }

        private async Task increaseDate(bool rade_hole_mode = false)
        {
            TimeSpan oneday = new TimeSpan(1, 0, 0, 0);
            DateTime tommorow = current_date + oneday;
            int year_diff = tommorow.Year - current_date.Year;
            int month_diff = tommorow.Month - current_date.Month;
            current_date = current_date + oneday;

            int update_num = 1;
            if (year_diff == 1) update_num = 3;
            if (month_diff == 1) update_num = 2;

            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(150);

            if (rade_hole_mode)
            {
                for (int i = 0; i < 2; ++i)
                {
                    pressButton(ButtonType.RIGHT);
                    await Task.Delay(40);
                    releaseButton(ButtonType.RIGHT);
                    await Task.Delay(40);
                }

                for (int i = 0; i < update_num; ++i)
                {
                    pressButton(ButtonType.UP);
                    await Task.Delay(40);
                    releaseButton(ButtonType.UP);
                    await Task.Delay(40);

                    if (i != update_num - 1)
                    {
                        pressButton(ButtonType.LEFT);
                        await Task.Delay(40);
                        releaseButton(ButtonType.LEFT);
                        await Task.Delay(40);
                    }
                }

                for (int i = 0; i < update_num + 3; ++i)
                {
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(40);
                }
            }
            else
            {
                for (int i = 0; i < 2; ++i)
                {
                    pressButton(ButtonType.LEFT);
                    await Task.Delay(40);
                    releaseButton(ButtonType.LEFT);
                    await Task.Delay(40);
                }

                for (int i = 0; i < update_num; ++i)
                {
                    pressButton(ButtonType.LEFT);
                    await Task.Delay(40);
                    releaseButton(ButtonType.LEFT);
                    await Task.Delay(40);
                    pressButton(ButtonType.UP);
                    await Task.Delay(40);
                    releaseButton(ButtonType.UP);
                    await Task.Delay(40);
                }

                for (int i = 0; i < 3 + update_num; ++i)
                {
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(40);
                }
                await Task.Delay(80);
            }
            updateDateLabel();
        }

        private async Task increaseDateWithRaidHole()
        {
            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(500);
            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(3000);


            pressButton(ButtonType.HOME);
            await Task.Delay(50);
            releaseButton(ButtonType.HOME);
            await Task.Delay(1000);
            pressButton(ButtonType.DOWN);
            await Task.Delay(50);
            for (int j = 0; j < 4; ++j)
            {
                pressButton(ButtonType.RIGHT);
                await Task.Delay(50);
                releaseButton(ButtonType.RIGHT);
                await Task.Delay(50);
            }
            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(50);
            pressButton(ButtonType.DOWN);
            await Task.Delay(2200);
            releaseButton(ButtonType.DOWN);
            await Task.Delay(50);
            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(50);

            for (int j = 0; j < 4; ++j)
            {
                pressButton(ButtonType.DOWN);
                await Task.Delay(50);
                releaseButton(ButtonType.DOWN);
                await Task.Delay(50);
            }

            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(300);

            for (int j = 0; j < 2; ++j)
            {
                pressButton(ButtonType.DOWN);
                await Task.Delay(50);
                releaseButton(ButtonType.DOWN);
                await Task.Delay(50);
            }

            //increase date
            await increaseDate(true);

            pressButton(ButtonType.HOME);
            await Task.Delay(50);
            releaseButton(ButtonType.HOME);
            await Task.Delay(1000);
            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(500);

            pressButton(ButtonType.B);
            await Task.Delay(100);
            releaseButton(ButtonType.B);
            await Task.Delay(1000);
            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(4000);

            for (int j = 0; j < 2; ++j)
            {
                pressButton(ButtonType.A);
                await Task.Delay(50);
                releaseButton(ButtonType.A);
                await Task.Delay(500);
            }

            for (int j = 0; j < 3; ++j)
            {
                pressButton(ButtonType.A);
                await Task.Delay(50);
                releaseButton(ButtonType.A);
                await Task.Delay(100);
            }
        }

        private async void CheckboxPlus1Day_CheckedChanged(object sender, EventArgs e)
        {
            DayComboBox.Enabled = false;
            if (CheckboxPlus1Day.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    await Task.Run(async () =>
                    {

                        if (cancel_token.IsCancellationRequested)
                        {
                            return;
                        }
                        await increaseDateWithRaidHole();
                    }, cancel_token);

                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                CheckboxPlus1Day.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            DayComboBox.Enabled = true;
        }

        private async void CheckboxPlus3Days_CheckedChanged(object sender, EventArgs e)
        {
            DayComboBox.Enabled = false;
            if (CheckboxPlus3Days.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    await Task.Run(async () =>
                    {
                        for (uint i = 0; i < 3; ++i)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }

                            await increaseDateWithRaidHole();
                        }
                    }, cancel_token);

                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                CheckboxPlus3Days.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            DayComboBox.Enabled = true;
        }

        private async void CheckboxPlus4Days_CheckedChanged(object sender, EventArgs e)
        {
            DayComboBox.Enabled = false;
            if (CheckboxPlus4Days.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    await Task.Run(async () =>
                    {
                        for (uint i = 0; i < 4; ++i)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }

                            await increaseDateWithRaidHole();
                        }
                    }, cancel_token);

                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                CheckboxPlus4Days.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            DayComboBox.Enabled = true;
        }

        private async void CheckboxPlusNDaysWithRaidHole_CheckedChanged(object sender, EventArgs e)
        {
            DayComboBox.Enabled = false;
            if (CheckboxPlusNDaysWithRaidHole.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    int n_days = int.Parse(DayTextboxWithRaidHole.Text);
                    updateCountLabelWithRaidHole(0, n_days);

                    await Task.Run(async () =>
                    {
                        for (uint i = 0; i < n_days; ++i)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }

                            await increaseDateWithRaidHole();
                            updateCountLabelWithRaidHole((int)i + 1, n_days);
                        }
                    }, cancel_token);
                    TaskFinished taskFinished = new TaskFinished();
                    DialogResult rc = taskFinished.ShowDialog();
                    taskFinished.Dispose();
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                CheckboxPlusNDaysWithRaidHole.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            DayComboBox.Enabled = true;
        }

        private async Task Reload()
        {
            await Task.Delay(300);
            pressButton(ButtonType.HOME);
            await Task.Delay(40);
            releaseButton(ButtonType.HOME);
            await Task.Delay(1000);
            // shut down the software
            pressButton(ButtonType.X);
            await Task.Delay(40);
            releaseButton(ButtonType.X);
            await Task.Delay(300);
            // confirm
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(4000);
            // start the game
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(1000);
            // confirm account
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(17000);
            // skip the opening animation
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(10000);
        }

        private async void ReloadThenPlus3Days_CheckedChanged(object sender, EventArgs e)
        {
            DayComboBox.Enabled = false;
            if (ReloadThenPlus3Days.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    await Reload();
                    await Task.Delay(1000);
                    // get into the den
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(1000);

                    await Task.Run(async () =>
                    {
                        for (uint i = 0; i < 3; ++i)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }

                            await increaseDateWithRaidHole();
                        }
                    }, cancel_token);

                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                ReloadThenPlus3Days.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            DayComboBox.Enabled = true;
        }

        private async void CheckboxPlusNDays_CheckedChanged(object sender, EventArgs e)
        {
            DayComboBox.Enabled = false;
            if (CheckboxPlusNDays.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    int n_days = int.Parse(DayTextbox.Text);
                    updateCountLabel(0, n_days);

                    await Task.Delay(300);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(300);
                    pressButton(ButtonType.RIGHT);
                    await Task.Delay(1000);
                    releaseButton(ButtonType.RIGHT);
                    await Task.Delay(40);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(300);

                    await Task.Run(async () =>
                    {
                        for (int i = 0; i < n_days; ++i)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }

                            await increaseDate();
                            updateCountLabel(i + 1, n_days);
                        }
                    }, cancel_token);

                    TaskFinished taskFinished = new TaskFinished();
                    DialogResult rc = taskFinished.ShowDialog();
                    taskFinished.Dispose();
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxPlusNDays.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            DayComboBox.Enabled = true;
        }

        private async Task SaveThenBackToPlusNDays()
        {
            await Task.Delay(300);
            pressButton(ButtonType.HOME);
            await Task.Delay(40);
            releaseButton(ButtonType.HOME);
            await Task.Delay(1000);
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(1000);
            pressButton(ButtonType.X);
            await Task.Delay(40);
            releaseButton(ButtonType.X);
            await Task.Delay(1000);
            pressButton(ButtonType.R);
            await Task.Delay(40);
            releaseButton(ButtonType.R);
            await Task.Delay(1500);
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(3000);
            pressButton(ButtonType.HOME);
            await Task.Delay(50);
            releaseButton(ButtonType.HOME);
            await Task.Delay(1000);
            pressButton(ButtonType.DOWN);
            await Task.Delay(50);
            for (int j = 0; j < 4; ++j)
            {
                pressButton(ButtonType.RIGHT);
                await Task.Delay(50);
                releaseButton(ButtonType.RIGHT);
                await Task.Delay(50);
            }
            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(50);
            pressButton(ButtonType.DOWN);
            await Task.Delay(2200);
            releaseButton(ButtonType.DOWN);
            await Task.Delay(50);
            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(50);

            for (int j = 0; j < 4; ++j)
            {
                pressButton(ButtonType.DOWN);
                await Task.Delay(50);
                releaseButton(ButtonType.DOWN);
                await Task.Delay(50);
            }

            pressButton(ButtonType.A);
            await Task.Delay(50);
            releaseButton(ButtonType.A);
            await Task.Delay(300);

            for (int j = 0; j < 2; ++j)
            {
                pressButton(ButtonType.DOWN);
                await Task.Delay(50);
                releaseButton(ButtonType.DOWN);
                await Task.Delay(50);
            }

            await Task.Delay(300);
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(300);
            pressButton(ButtonType.RIGHT);
            await Task.Delay(1000);
            releaseButton(ButtonType.RIGHT);
            await Task.Delay(40);
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(300);
        }

        private async void CheckboxPlusNDaysWithSave_CheckedChanged(object sender, EventArgs e)
        {
            DayComboBox.Enabled = false;
            if (CheckboxPlusNDaysWithSave.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    int n_days = int.Parse(DayTextbox.Text);
                    updateCountLabel(0, n_days);

                    await Task.Delay(300);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(300);
                    pressButton(ButtonType.RIGHT);
                    await Task.Delay(1000);
                    releaseButton(ButtonType.RIGHT);
                    await Task.Delay(40);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(300);

                    await Task.Run(async () =>
                    {
                        for (int i = 0; i < n_days; ++i)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }
                            if (i !=0 && i % 300 == 0)
                            {
                                await SaveThenBackToPlusNDays();
                                await Task.Delay(1000);
                            }

                            await increaseDate();
                            updateCountLabel(i + 1, n_days);
                        }
                    }, cancel_token);

                    TaskFinished taskFinished = new TaskFinished();
                    DialogResult rc = taskFinished.ShowDialog();
                    taskFinished.Dispose();
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxPlusNDaysWithSave.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            DayComboBox.Enabled = true;
        }

        private void YearComboBox_Click(object sender, EventArgs e)
        {
            DayComboBox.SelectedIndex = -1;
        }

        private void MonthComboBox_Click(object sender, EventArgs e)
        {
            DayComboBox.SelectedIndex = -1;
        }

        private void DayComboBox_Click(object sender, EventArgs e)
        {
            DayComboBox.Items.Clear();
            int year = int.Parse(YearComboBox.SelectedItem.ToString());
            int month = int.Parse(MonthComboBox.SelectedItem.ToString());

            int days = DateTime.DaysInMonth(year, month);

            for (uint i = 1; i <= days; ++i)
            {
                DayComboBox.Items.Add(i);
            }
        }

        private void DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DayComboBox.SelectedIndex >= 0)
            {
                int year = int.Parse(YearComboBox.SelectedItem.ToString());
                int month = int.Parse(MonthComboBox.SelectedItem.ToString());
                int day = int.Parse(DayComboBox.SelectedItem.ToString());

                current_date = new DateTime(year, month, day, 0, 0, 0);
                updateDateLabel();
            }
        }

        private async void CheckboxLotoID_CheckedChanged(object sender, EventArgs e)
        {
            DayComboBox.Enabled = false;
            if (CheckboxLotoID.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    await Task.Run(async () =>
                    {
                        while (true)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }
                            pressButton(ButtonType.HOME);
                            await Task.Delay(100);
                            releaseButton(ButtonType.HOME);
                            await Task.Delay(1000);
                            pressButton(ButtonType.DOWN);
                            await Task.Delay(50);
                            for (int i = 0; i < 4; ++i)
                            {
                                pressButton(ButtonType.RIGHT);
                                await Task.Delay(50);
                                releaseButton(ButtonType.RIGHT);
                                await Task.Delay(50);
                            }
                            pressButton(ButtonType.A);
                            await Task.Delay(50);
                            releaseButton(ButtonType.A);
                            await Task.Delay(50);
                            pressButton(ButtonType.DOWN);
                            await Task.Delay(2200);
                            releaseButton(ButtonType.DOWN);
                            await Task.Delay(50);
                            pressButton(ButtonType.A);
                            await Task.Delay(50);
                            releaseButton(ButtonType.A);
                            await Task.Delay(50);

                            for (int i = 0; i < 4; ++i)
                            {
                                pressButton(ButtonType.DOWN);
                                await Task.Delay(50);
                                releaseButton(ButtonType.DOWN);
                                await Task.Delay(50);
                            }

                            pressButton(ButtonType.A);
                            await Task.Delay(50);
                            releaseButton(ButtonType.A);
                            await Task.Delay(300);

                            for (int i = 0; i < 2; ++i)
                            {
                                pressButton(ButtonType.DOWN);
                                await Task.Delay(50);
                                releaseButton(ButtonType.DOWN);
                                await Task.Delay(50);
                            }

                            await increaseDate(true);

                            pressButton(ButtonType.HOME);
                            await Task.Delay(50);
                            releaseButton(ButtonType.HOME);
                            await Task.Delay(1000);
                            pressButton(ButtonType.A);
                            await Task.Delay(50);
                            releaseButton(ButtonType.A);
                            await Task.Delay(500);

                            //loto
                            pressButton(ButtonType.A);
                            await Task.Delay(50);
                            releaseButton(ButtonType.A);
                            await Task.Delay(500);
                            pressButton(ButtonType.A);
                            await Task.Delay(50);
                            releaseButton(ButtonType.A);
                            await Task.Delay(600);
                            pressButton(ButtonType.DOWN);
                            await Task.Delay(250);
                            releaseButton(ButtonType.DOWN);
                            await Task.Delay(250);
                            pressButton(ButtonType.A);
                            await Task.Delay(100);
                            releaseButton(ButtonType.A);
                            await Task.Delay(800);

                            for (int i = 0; i < 4; ++i)
                            {
                                pressButton(ButtonType.A);
                                await Task.Delay(120);
                                releaseButton(ButtonType.A);
                                await Task.Delay(550);
                            }
                            await Task.Delay(1200);

                            for (int i = 0; i < 6; ++i)
                            {
                                pressButton(ButtonType.A);
                                await Task.Delay(100);
                                releaseButton(ButtonType.A);
                                await Task.Delay(500);
                            }
                            await Task.Delay(2200);

                            for (int i = 0; i < 6; ++i)
                            {
                                pressButton(ButtonType.A);
                                await Task.Delay(100);
                                releaseButton(ButtonType.A);
                                await Task.Delay(500);
                            }
                            await Task.Delay(2500);

                            for (int i = 0; i < 3; ++i)
                            {
                                pressButton(ButtonType.A);
                                await Task.Delay(100);
                                releaseButton(ButtonType.A);
                                await Task.Delay(500);
                            }
                        }
                    }, cancel_token);

                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                CheckboxLotoID.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            DayComboBox.Enabled = true;
        }

        private async Task NumberPanelGoUp(int step)
        {
            await Task.Delay(50);
            for (uint i = 0; i < step; ++i)
            {
                pressButton(ButtonType.UP);
                await Task.Delay(40);
                releaseButton(ButtonType.UP);
                await Task.Delay(50);
            }   
        }

        private async Task NumberPanelGoDown(int step)
        {
            await Task.Delay(50);
            for (uint i = 0; i < step; ++i)
            {
                pressButton(ButtonType.DOWN);
                await Task.Delay(40);
                releaseButton(ButtonType.DOWN);
                await Task.Delay(50);
            }
        }

        private async Task NumberPanelGoLeft(int step)
        {
            await Task.Delay(50);
            for (uint i = 0; i < step; ++i)
            {
                pressButton(ButtonType.LEFT);
                await Task.Delay(40);
                releaseButton(ButtonType.LEFT);
                await Task.Delay(50);
            }
        }

        private async Task NumberPanelGoRight(int step)
        {
            await Task.Delay(50);
            for (uint i = 0; i < step; ++i)
            {
                pressButton(ButtonType.RIGHT);
                await Task.Delay(40);
                releaseButton(ButtonType.RIGHT);
                await Task.Delay(50);
            }
        }

        private async Task ChangeNumberPanel(int[] current, int[] target)
        {
            await Task.Delay(300);
            // if current number is 0
            if (current[0] == 3)
            {
                await NumberPanelGoUp(current[0] - target[0]);
                if (target[1] == 0)
                {
                    await NumberPanelGoLeft(1);
                }
                else if (target[1] == 2)
                {
                    await NumberPanelGoRight(1);
                }
            }
            else
            {
                // move horizontally first
                if (current[1] > target[1])
                {
                    await NumberPanelGoLeft(current[1] - target[1]);
                }
                else if (current[1] < target[1])
                {
                    await NumberPanelGoRight(target[1] - current[1]);
                }
                // then move vertically
                if (current[0] > target[0])
                {
                    await NumberPanelGoUp(current[0] - target[0]);
                }
                else if (current[0] < target[0])
                {
                    await NumberPanelGoDown(target[0] - current[0]);
                }
            }
        }

        private async Task InputCode(int code)
        {
            await Task.Delay(300);
            int number4 = code % 10;
            int number3 = code / 10 % 10;
            int number2 = code / 100 % 10;
            int number1 = code / 1000 % 10;
            int number0 = 1;
            int[] number4Position = { numberPanel[number4, 0], numberPanel[number4, 1] };
            int[] number3Position = { numberPanel[number3, 0], numberPanel[number3, 1] };
            int[] number2Position = { numberPanel[number2, 0], numberPanel[number2, 1] };
            int[] number1Position = { numberPanel[number1, 0], numberPanel[number1, 1] };
            int[] init = { 0, 0 };
            if (number0 != number1)
            {
                await ChangeNumberPanel(init, number1Position);
            }
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(100);

            if (number1 != number2)
            {
                await ChangeNumberPanel(number1Position, number2Position);
            }
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(100);

            if (number2 != number3)
            {
                await ChangeNumberPanel(number2Position, number3Position);
            }
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(100);

            if (number3 != number4)
            {
                await ChangeNumberPanel(number3Position, number4Position);
            }
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(100);
            pressButton(ButtonType.PLUS);
            await Task.Delay(40);
            releaseButton(ButtonType.PLUS);
            await Task.Delay(1500);
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(500);
        }

        private async Task ConnectToTheInternet()
        {
            await Task.Delay(300);
            pressButton(ButtonType.Y);
            await Task.Delay(40);
            releaseButton(ButtonType.Y);
            await Task.Delay(1000);
            pressButton(ButtonType.PLUS);
            await Task.Delay(40);
            releaseButton(ButtonType.PLUS);
            await Task.Delay(18000);
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(300);
        }

        private async Task DisconnectFromTheInternet()
        {
            await Task.Delay(300);
            pressButton(ButtonType.HOME);
            await Task.Delay(1000);
            releaseButton(ButtonType.HOME);
            await Task.Delay(500);
            for (uint i = 0; i < 4; ++i)
            {
                pressButton(ButtonType.DOWN);
                await Task.Delay(40);
                releaseButton(ButtonType.DOWN);
                await Task.Delay(50);
            }
            for (uint i = 0; i < 2; ++i)
            {
                pressButton(ButtonType.A);
                await Task.Delay(40);
                releaseButton(ButtonType.A);
                await Task.Delay(500);
            }
            pressButton(ButtonType.B);
            await Task.Delay(40);
            releaseButton(ButtonType.B);
            await Task.Delay(500);
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(300);
        }

        private async void CheckboxStartMax_CheckedChanged(object sender, EventArgs e)
        {
            CodeTextBox.Enabled = false;
            if (CheckboxStartMax.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    int code = int.Parse(CodeTextBox.Text);

                    await Task.Run(async () =>
                    {
                        if (cancel_token.IsCancellationRequested)
                        {
                            return;
                        }
                        while (true)
                        {
                            await ConnectToTheInternet();
                            pressButton(ButtonType.Y);
                            await Task.Delay(40);
                            releaseButton(ButtonType.Y);
                            await Task.Delay(2500);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(9000);
                            if (code != 0)
                            {
                                pressButton(ButtonType.PLUS);
                                await Task.Delay(40);
                                releaseButton(ButtonType.PLUS);
                                await Task.Delay(1000);
                                await InputCode(code);
                            }

                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(10000);
                            pressButton(ButtonType.UP);
                            await Task.Delay(40);
                            releaseButton(ButtonType.UP);
                            await Task.Delay(90000);

                            // start
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(500);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(1000);

                            // in case there are less than 4 players
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(1000);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(15000);

                            await DisconnectFromTheInternet();

                            await Task.Delay(25000);
                        }
                    }, cancel_token);
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxStartMax.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            CodeTextBox.Enabled = true;
        }

        private async void CheckboxStartMaxReload_CheckedChanged(object sender, EventArgs e)
        {
            CodeTextBox.Enabled = false;
            if (CheckboxStartMaxReload.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    int code = int.Parse(CodeTextBox.Text);

                    await Task.Run(async () =>
                    {
                        while (true)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }
                            await ConnectToTheInternet();
                            pressButton(ButtonType.Y);
                            await Task.Delay(40);
                            releaseButton(ButtonType.Y);
                            await Task.Delay(2500);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(9000);
                            if (code != 0)
                            {
                                pressButton(ButtonType.PLUS);
                                await Task.Delay(40);
                                releaseButton(ButtonType.PLUS);
                                await Task.Delay(1000);
                                await InputCode(code);
                            }

                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(10000);
                            pressButton(ButtonType.UP);
                            await Task.Delay(40);
                            releaseButton(ButtonType.UP);
                            await Task.Delay(90000);

                            // start
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(500);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(1000);

                            // in case there are less than 4 players
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(1000);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(15000);

                            await Reload();
                            await Task.Delay(1000);
                        }
                    }, cancel_token);
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxStartMaxReload.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            CodeTextBox.Enabled = true;
        }

        private async void CheckboxPlus3DaysStartMaxReload_CheckedChanged(object sender, EventArgs e)
        {
            CodeTextBox.Enabled = false;
            DayComboBox.Enabled = false;
            if (CheckboxPlus3DaysStartMaxReload.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    int code = int.Parse(CodeTextBox.Text);

                    await Task.Run(async () =>
                    {
                        while (true)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }
                            // get into the den
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(1000);

                            await Task.Run(async () =>
                            {
                                for (uint i = 0; i < 3; ++i)
                                {
                                    if (cancel_token.IsCancellationRequested)
                                    {
                                        return;
                                    }

                                    await increaseDateWithRaidHole();
                                }
                            }, cancel_token);

                            pressButton(ButtonType.B);
                            await Task.Delay(40);
                            releaseButton(ButtonType.B);
                            await Task.Delay(1000);

                            await ConnectToTheInternet();
                            pressButton(ButtonType.Y);
                            await Task.Delay(40);
                            releaseButton(ButtonType.Y);
                            await Task.Delay(2500);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(9000);
                            if (code != 0)
                            {
                                pressButton(ButtonType.PLUS);
                                await Task.Delay(40);
                                releaseButton(ButtonType.PLUS);
                                await Task.Delay(1000);
                                await InputCode(code);
                            }

                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(10000);
                            pressButton(ButtonType.UP);
                            await Task.Delay(40);
                            releaseButton(ButtonType.UP);
                            await Task.Delay(90000);

                            // start
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(500);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(1000);

                            // in case there are less than 4 players
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(1000);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(15000);

                            await Reload();
                            await Task.Delay(1000);
                        }
                    }, cancel_token);
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxPlus3DaysStartMaxReload.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            CodeTextBox.Enabled = true;
            DayComboBox.Enabled = true;
        }

        private async void CheckBoxConnetToDudu_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxConnetToDudu.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    pressButton(ButtonType.Y);
                    await Task.Delay(40);
                    releaseButton(ButtonType.Y);
                    await Task.Delay(1000);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(1000);
                    pressButton(ButtonType.DOWN);
                    await Task.Delay(40);
                    releaseButton(ButtonType.DOWN);
                    await Task.Delay(300);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(500);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(500);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(1000);

                    await InputCode(9162);

                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(500);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(500);
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckBoxConnetToDudu.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
        }

        private async void CheckboxConnectAndInputCode_CheckedChanged(object sender, EventArgs e)
        {
            CodeTextBox.Enabled = false;
            if (CheckboxConnectAndInputCode.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    int code = int.Parse(CodeTextBox.Text);

                    await ConnectToTheInternet();
                    pressButton(ButtonType.Y);
                    await Task.Delay(40);
                    releaseButton(ButtonType.Y);
                    await Task.Delay(2500);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(9000);
                    if (code != 0)
                    {
                        pressButton(ButtonType.PLUS);
                        await Task.Delay(40);
                        releaseButton(ButtonType.PLUS);
                        await Task.Delay(1000);
                        await InputCode(code);
                    }
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxConnectAndInputCode.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            CodeTextBox.Enabled = true;
        }

        private async void CheckboxDisconnect_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckboxDisconnect.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    await DisconnectFromTheInternet();
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxDisconnect.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
        }

        private async void CheckboxDisconnectThenConnect_CheckedChanged(object sender, EventArgs e)
        {
            CodeTextBox.Enabled = false;
            if (CheckboxDisconnectThenConnect.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    int code = int.Parse(CodeTextBox.Text);

                    await DisconnectFromTheInternet();
                    await Task.Delay(25000);

                    await ConnectToTheInternet();
                    pressButton(ButtonType.Y);
                    await Task.Delay(40);
                    releaseButton(ButtonType.Y);
                    await Task.Delay(2500);
                    pressButton(ButtonType.A);
                    await Task.Delay(40);
                    releaseButton(ButtonType.A);
                    await Task.Delay(9000);
                   
                    if (code != 0)
                    {
                        pressButton(ButtonType.PLUS);
                        await Task.Delay(40);
                        releaseButton(ButtonType.PLUS);
                        await Task.Delay(1000);
                        await InputCode(code);
                    }
                   
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxDisconnectThenConnect.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            CodeTextBox.Enabled = true;
        }

        private void DuduIP_LinkClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "http://116.202.105.91/");
        }

        private async void CheckboxACombo_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckboxACombo.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    await Task.Run(async () =>
                    {
                        while(true)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(300);
                        }
                    }, cancel_token);
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxACombo.Checked = false;                
            }
            else
            {
                token_source.Cancel();
            }
        }

        private async void CheckboxReceiveEggs_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckboxReceiveEggs.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    await Task.Run(async () =>
                    {
                        while (true)
                        {
                            if (cancel_token.IsCancellationRequested)
                            {
                                return;
                            }
                            for (uint i = 0; i < 2; i++)
                            {
                                moveStick(ButtonType.LSTICK, Stick.MIN, Stick.CUSTOM_EGG);
                                await Task.Delay(2300);
                                releaseStick(ButtonType.LSTICK);
                                await Task.Delay(500);
                                moveStick(ButtonType.LSTICK, Stick.MAX, Stick.CUSTOM_EGG);
                                await Task.Delay(2400);
                                releaseStick(ButtonType.LSTICK);
                                await Task.Delay(500);
                            }
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(1000);
                            pressButton(ButtonType.A);
                            await Task.Delay(40);
                            releaseButton(ButtonType.A);
                            await Task.Delay(3500);
                            pressButton(ButtonType.B);
                            await Task.Delay(40);
                            releaseButton(ButtonType.B);
                            await Task.Delay(2000);
                            pressButton(ButtonType.B);
                            await Task.Delay(40);
                            releaseButton(ButtonType.B);
                            await Task.Delay(2000);
                            pressButton(ButtonType.B);
                            await Task.Delay(40);
                            releaseButton(ButtonType.B);
                            await Task.Delay(300);
                        }
                    }, cancel_token);
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxReceiveEggs.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
        }

        private async Task IncubationCycle(int milliseconds)
        {
            moveStick(ButtonType.LSTICK, Stick.CENTER, Stick.MIN);
            moveStick(ButtonType.RSTICK, Stick.MIN, Stick.CENTER);
            await Task.Delay(milliseconds);
            releaseStick(ButtonType.LSTICK);
            releaseStick(ButtonType.RSTICK);
        }

        private async Task IncubationGetEggsFromBox(int currentBox, uint currentColumn, bool next)
        {
            pressButton(ButtonType.X);
            await Task.Delay(40);
            releaseButton(ButtonType.X);
            await Task.Delay(1000);
            if (currentBox == 1 && currentColumn == 1)
            {
                await MenuLocatePokemon();
            }
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(2000);
            pressButton(ButtonType.R);
            await Task.Delay(40);
            releaseButton(ButtonType.R);
            await Task.Delay(2000);
            pressButton(ButtonType.Y);
            await Task.Delay(40);
            releaseButton(ButtonType.Y);
            await Task.Delay(300);
            pressButton(ButtonType.Y);
            await Task.Delay(40);
            releaseButton(ButtonType.Y);
            await Task.Delay(300);
            pressButton(ButtonType.LEFT);
            await Task.Delay(40);
            releaseButton(ButtonType.LEFT);
            await Task.Delay(300);
            pressButton(ButtonType.DOWN);
            await Task.Delay(40);
            releaseButton(ButtonType.DOWN);
            await Task.Delay(300);
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(300);
            for (uint j = 0; j < 4; j++)
            {
                pressButton(ButtonType.DOWN);
                await Task.Delay(40);
                releaseButton(ButtonType.DOWN);
                await Task.Delay(50);
            }
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(300);
            pressButton(ButtonType.UP);
            await Task.Delay(40);
            releaseButton(ButtonType.UP);
            await Task.Delay(300);
            for (uint j = 0; j < currentColumn; j++)
            {
                pressButton(ButtonType.RIGHT);
                await Task.Delay(40);
                releaseButton(ButtonType.RIGHT);
                await Task.Delay(50);
            }
            pressButton(ButtonType.A);
            await Task.Delay(40);
            releaseButton(ButtonType.A);
            await Task.Delay(300);

            if (currentColumn < 6)
            {
                pressButton(ButtonType.RIGHT);
                await Task.Delay(40);
                releaseButton(ButtonType.RIGHT);
                await Task.Delay(300);
                pressButton(ButtonType.A);
                await Task.Delay(40);
                releaseButton(ButtonType.A);
                await Task.Delay(300);
                for (uint j = 0; j < 4; j++)
                {
                    pressButton(ButtonType.DOWN);
                    await Task.Delay(40);
                    releaseButton(ButtonType.DOWN);
                    await Task.Delay(50);
                }
                pressButton(ButtonType.A);
                await Task.Delay(40);
                releaseButton(ButtonType.A);
                await Task.Delay(300);
                pressButton(ButtonType.DOWN);
                await Task.Delay(40);
                releaseButton(ButtonType.DOWN);
                await Task.Delay(300);
                for (uint j = 0; j < currentColumn + 1; j++)
                {
                    pressButton(ButtonType.LEFT);
                    await Task.Delay(40);
                    releaseButton(ButtonType.LEFT);
                    await Task.Delay(50);
                }
                pressButton(ButtonType.A);
                await Task.Delay(40);
                releaseButton(ButtonType.A);
                await Task.Delay(300);
                for (uint j = 0; j < 3; j++)
                {
                    pressButton(ButtonType.B);
                    await Task.Delay(40);
                    releaseButton(ButtonType.B);
                    await Task.Delay(2000);
                }
            }

            if (currentColumn == 6 && next)
            {
                pressButton(ButtonType.R);
                await Task.Delay(40);
                releaseButton(ButtonType.R);
                await Task.Delay(500);
                for (uint j = 0; j < currentColumn - 1; j++)
                {
                    pressButton(ButtonType.LEFT);
                    await Task.Delay(40);
                    releaseButton(ButtonType.LEFT);
                    await Task.Delay(50);
                }
                pressButton(ButtonType.A);
                await Task.Delay(40);
                releaseButton(ButtonType.A);
                await Task.Delay(300);
                for (uint j = 0; j < 4; j++)
                {
                    pressButton(ButtonType.DOWN);
                    await Task.Delay(40);
                    releaseButton(ButtonType.DOWN);
                    await Task.Delay(50);
                }
                pressButton(ButtonType.A);
                await Task.Delay(40);
                releaseButton(ButtonType.A);
                await Task.Delay(300);
                pressButton(ButtonType.DOWN);
                await Task.Delay(40);
                releaseButton(ButtonType.DOWN);
                await Task.Delay(300);
                pressButton(ButtonType.LEFT);
                await Task.Delay(40);
                releaseButton(ButtonType.LEFT);
                await Task.Delay(50);
                pressButton(ButtonType.A);
                await Task.Delay(40);
                releaseButton(ButtonType.A);
                await Task.Delay(300);
                for (uint j = 0; j < 3; j++)
                {
                    pressButton(ButtonType.B);
                    await Task.Delay(40);
                    releaseButton(ButtonType.B);
                    await Task.Delay(2000);
                }
            }            
            
        }

        private async Task MenuLocatePokemon()
        {
            pressButton(ButtonType.UP);
            await Task.Delay(1000);
            releaseButton(ButtonType.UP);
            await Task.Delay(300);
            pressButton(ButtonType.LEFT);
            await Task.Delay(1000);
            releaseButton(ButtonType.LEFT);
            await Task.Delay(300);
            pressButton(ButtonType.RIGHT);
            await Task.Delay(40);
            releaseButton(ButtonType.RIGHT);
            await Task.Delay(300);
        }

        private delegate void delegateUpdateIncuationLabel(int box, uint column);

        private void updateIncuationLabel(int box, uint column)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new delegateUpdateIncuationLabel(this.updateIncuationLabel), box, column);
                return;
            }
            IncubationLabel.Text = "当前：第" + box.ToString() + "箱第" + column.ToString() + "列";
        }

        private async void CheckboxIncubation_CheckedChanged(object sender, EventArgs e)
        {
            EggTextbox.Enabled = false;
            BoxAmountTextbox.Enabled = false;
            if (CheckboxIncubation.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    int period = int.Parse(EggTextbox.Text);
                    int n_boxes = int.Parse(BoxAmountTextbox.Text);
                    await Task.Run(async () =>
                    {
                        for (int n = 0; n < n_boxes; n++)
                        {
                            for (uint i = 0; i < 6; i++)
                            {
                                if (cancel_token.IsCancellationRequested)
                                {
                                    return;
                                }                                
                                updateIncuationLabel(n + 1, i + 1);
                                pressButton(ButtonType.L);
                                await Task.Delay(40);
                                releaseButton(ButtonType.L);
                                await Task.Delay(300);
                                await IncubationCycle((period * 3 + 8) * 1000);
                                for (uint j = 0; j < 5; j++)
                                {
                                    pressButton(ButtonType.A);
                                    await Task.Delay(40);
                                    releaseButton(ButtonType.A);
                                    await Task.Delay(16000);
                                    pressButton(ButtonType.A);
                                    await Task.Delay(40);
                                    releaseButton(ButtonType.A);
                                    await Task.Delay(3000);
                                    if (j != 4)
                                    {
                                        pressButton(ButtonType.L);
                                        await Task.Delay(40);
                                        releaseButton(ButtonType.L);
                                        await Task.Delay(300);
                                        moveStick(ButtonType.LSTICK, Stick.CUSTOM_INCUBATE, Stick.MIN);
                                        await Task.Delay(100);
                                        releaseStick(ButtonType.LSTICK);
                                        await Task.Delay(500);
                                    }
                                }
                                await IncubationGetEggsFromBox(n + 1, i + 1, n < n_boxes - 1);
                            }     
                        }
                                           
                    }, cancel_token);
                    TaskFinished taskFinished = new TaskFinished();
                    DialogResult rc = taskFinished.ShowDialog();
                    taskFinished.Dispose();
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxIncubation.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
            EggTextbox.Enabled = true;
            BoxAmountTextbox.Enabled = true;
        }

        private async void CheckboxReload_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckboxReload.Checked)
            {
                try
                {
                    token_source = new CancellationTokenSource();
                    cancel_token = token_source.Token;

                    await Reload();
                }
                catch (System.Threading.Tasks.TaskCanceledException exception)
                {
                }
                catch (System.FormatException formatException)
                {
                }
                CheckboxReload.Checked = false;
            }
            else
            {
                token_source.Cancel();
            }
        }
    }
}
