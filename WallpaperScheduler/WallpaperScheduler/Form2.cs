using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Collections;
using System.IO;
using Microsoft.Win32.TaskScheduler;
using System.Security.Principal;

namespace WallpaperScheduler
{


    public partial class Form2 : Form
    {

        public String currentLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        
        public int mrHour;
        public int mrMinute;
        public int miHour;
        public int miMinute;
        public int snHour;
        public int snMinute;
        public int niHour;
        public int niMinute;

        public int[] morningHistory;
        public int[] dayHistory;
        public int[] sunsetHistory;
        public int[] nightHistory;

        public Form2()
        {
            InitializeComponent();

            this.Size = new Size(this.Size.Width, this.Size.Height + 88);

            morningHistory = new int[2] { 0, 0 };
            dayHistory = new int[2] { 0, 0 };
            sunsetHistory = new int[2] { 0, 0 };
            nightHistory = new int[2] { 0, 0 };

            initializeHours();
            setDropDownValueS();
            
            

            

        }

       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //MORNING
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true)
            {
                numericUpDown1.Minimum = -1;
                morningHistory[0] = (int)numericUpDown1.Value;
                numericUpDown1.Value = -1;
                numericUpDown1.Maximum = -1;

                numericUpDown2.Minimum = -1;
                morningHistory[1] = (int)numericUpDown2.Value;
                numericUpDown2.Value = -1;
                numericUpDown2.Maximum = -1;


            }
            else
            {
                    numericUpDown1.Maximum = 23;
                    numericUpDown2.Maximum = 59;

                    setDropDownValue(numericUpDown1, numericUpDown2, morningHistory[0], morningHistory[1]);

                    numericUpDown1.Minimum = 0;
                    numericUpDown2.Minimum = 0;
            }
        }

        //DAY
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked==true)
            {
                numericUpDown4.Minimum = -1;
                dayHistory[0] = (int)numericUpDown4.Value;
                numericUpDown4.Value = -1;
                numericUpDown4.Maximum = -1;

                numericUpDown3.Minimum = -1;
                dayHistory[1] = (int)numericUpDown3.Value;
                numericUpDown3.Value = -1;
                numericUpDown3.Maximum = -1;


            }
            else
            {
                    numericUpDown4.Maximum = 23;
                    numericUpDown3.Maximum = 59;

                    setDropDownValue(numericUpDown4, numericUpDown3, dayHistory[0], dayHistory[1]);

                    numericUpDown4.Minimum = 0;
                    numericUpDown3.Minimum = 0;
            }
        }

        //SUNSET
        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                numericUpDown6.Minimum = -1;
                sunsetHistory[0] = (int)numericUpDown6.Value;
                numericUpDown6.Value = -1;
                numericUpDown6.Maximum = -1;

                numericUpDown5.Minimum = -1;
                sunsetHistory[1] = (int)numericUpDown5.Value;
                numericUpDown5.Value = -1;
                numericUpDown5.Maximum = -1;


            }
            else
            {
                numericUpDown6.Maximum = 23;
                numericUpDown5.Maximum = 59;

                setDropDownValue(numericUpDown6, numericUpDown5, sunsetHistory[0], sunsetHistory[1]);

                numericUpDown6.Minimum = 0;
                numericUpDown5.Minimum = 0;
            }
        }

        //NIGHT
        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                numericUpDown8.Minimum = -1;
                nightHistory[0] = (int)numericUpDown8.Value;
                numericUpDown8.Value = -1;
                numericUpDown8.Maximum = -1;

                numericUpDown7.Minimum = -1;
                nightHistory[1] = (int)numericUpDown7.Value;
                numericUpDown7.Value = -1;
                numericUpDown7.Maximum = -1;


            }
            else
            {
                numericUpDown8.Maximum = 23;
                numericUpDown7.Maximum = 59;

                setDropDownValue(numericUpDown8, numericUpDown7, nightHistory[0], nightHistory[1]);

                numericUpDown8.Minimum = 0;
                numericUpDown7.Minimum = 0;
            }
        }

        public void initializeHours()
        {
            using (TaskService ts = new TaskService())
            {
                Microsoft.Win32.TaskScheduler.Task t1 = ts.GetTask("TaskCreatedByWallpaperChangermorning");
                Microsoft.Win32.TaskScheduler.Task t2 = ts.GetTask("TaskCreatedByWallpaperChangerday");
                Microsoft.Win32.TaskScheduler.Task t3 = ts.GetTask("TaskCreatedByWallpaperChangersunset");
                Microsoft.Win32.TaskScheduler.Task t4 = ts.GetTask("TaskCreatedByWallpaperChangernight");

                if (t1 != null && t2 != null && t3 != null && t4 != null)
                {
                    if(t1.Definition.Triggers.Count>0)
                    {
                        Trigger tr1 = t1.Definition.Triggers[0];
                        String timeStr1 = tr1.ToString().Split(new string[] { "At " }, StringSplitOptions.None)[1]
                                                 .Split(new string[] { " every" }, StringSplitOptions.None)[0];
                        String[] time1 = timeStr1.Split(' ');
                        mrHour = Int32.Parse(time1[0].Split(':')[0]);
                        mrMinute = Int32.Parse(time1[0].Split(':')[1]);
                        if (time1[1] == "PM" && mrHour != 12)
                        {
                            mrHour = mrHour + 12;
                        }
                        if (time1[1] == "AM" && mrHour == 12)
                        {
                            mrHour = 0;
                        }
                    }
                    else
                    {
                        mrHour = -1;
                        mrMinute = -1;
                    }

                    if (t2.Definition.Triggers.Count > 0)
                    {
                        Trigger tr2 = t2.Definition.Triggers[0];
                        String timeStr2 = tr2.ToString().Split(new string[] { "At " }, StringSplitOptions.None)[1]
                                                 .Split(new string[] { " every" }, StringSplitOptions.None)[0];
                        String[] time2 = timeStr2.Split(' ');
                        miHour = Int32.Parse(time2[0].Split(':')[0]);
                        miMinute = Int32.Parse(time2[0].Split(':')[1]);
                        if (time2[1] == "PM" && miHour != 12)
                        {
                            miHour = miHour + 12;
                        }
                        if (time2[1] == "AM" && miHour == 12)
                        {
                            miHour = 0;
                        }
                    }
                    else
                    {
                        miHour = -1;
                        miMinute = -1;
                    }

                    if (t3.Definition.Triggers.Count > 0)
                    {
                        Trigger tr3 = t3.Definition.Triggers[0];
                        String timeStr3 = tr3.ToString().Split(new string[] { "At " }, StringSplitOptions.None)[1]
                                                .Split(new string[] { " every" }, StringSplitOptions.None)[0];
                        String[] time3 = timeStr3.Split(' ');
                        snHour = Int32.Parse(time3[0].Split(':')[0]);
                        snMinute = Int32.Parse(time3[0].Split(':')[1]);
                        if (time3[1] == "PM" && snHour != 12)
                        {
                            snHour = snHour + 12;
                        }
                        if (time3[1] == "AM" && snHour == 12)
                        {
                            snHour = 0;
                        }
                    }
                    else
                    {
                        snHour = -1;
                        snMinute = -1;
                    }

                    if (t4.Definition.Triggers.Count > 0)
                    {
                        Trigger tr4 = t4.Definition.Triggers[0];
                        String timeStr4 = tr4.ToString().Split(new string[] { "At " }, StringSplitOptions.None)[1]
                                               .Split(new string[] { " every" }, StringSplitOptions.None)[0];
                        String[] time4 = timeStr4.Split(' ');
                        niHour = Int32.Parse(time4[0].Split(':')[0]);
                        niMinute = Int32.Parse(time4[0].Split(':')[1]);
                        if (time4[1] == "PM" && niHour != 12)
                        {
                            niHour = niHour + 12;
                        }
                        if (time4[1] == "AM" && niHour == 12)
                        {
                            niHour = 0;
                        }
                    }
                    else
                    {
                        niHour = -1;
                        niMinute = -1;
                    }
                      
                }
                else
                {
                    mrHour = -1;
                    mrMinute = -1;
                    miHour = -1;
                    miMinute = -1;
                    snHour = -1;
                    snMinute = -1;
                    niHour = -1;
                    niMinute = -1;
                }  
                /*
                MessageBox.Show(mrHour.ToString() + ":" + mrMinute.ToString());
                MessageBox.Show(miHour.ToString() + ":" + miMinute.ToString());
                MessageBox.Show(snHour.ToString() + ":" + snMinute.ToString());
                MessageBox.Show(niHour.ToString() + ":" + niMinute.ToString());
                */
            }
        }

        public void setDropDownValue(NumericUpDown hour,NumericUpDown minute,int hr,int min)
        {
            hour.Value = hr;
            minute.Value = min;
        }

        public void setDropDownValueS()
        {
            if (mrHour == -1 || mrMinute == -1)
            {
                checkBox1.Checked=true;
            }
            else
            {
                setDropDownValue(numericUpDown1, numericUpDown2, mrHour, mrMinute);
            }

            if (miHour == -1 || miMinute == -1)
            {
                checkBox2.Checked = true;
            }
            else
            {
                setDropDownValue(numericUpDown4, numericUpDown3, miHour, miMinute);
            }

            if (snHour == -1 || snMinute == -1)
            {
                checkBox3.Checked = true;
            }
            else
            {
                setDropDownValue(numericUpDown6, numericUpDown5, snHour, snMinute);
            }


            if (niHour == -1 || niMinute == -1)
            {
                checkBox4.Checked = true;
            }
            else
            {
                setDropDownValue(numericUpDown8, numericUpDown7, niHour, niMinute);
            }

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
           
        }


        public void createDayTask(bool privileges)
        {
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                if (privileges)
                    td.Principal.RunLevel = TaskRunLevel.Highest;

                // Add a daily trigger at a given time
                DailyTrigger dt = (DailyTrigger)td.Triggers.Add(new DailyTrigger());
                dt.StartBoundary = DateTime.Today + TimeSpan.FromHours((double)numericUpDown4.Value) + TimeSpan.FromMinutes((double)numericUpDown3.Value);

                // Add an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(currentLocation + @"\Utilities\dayWallpaperChange.vbs", null, null));
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.StartWhenAvailable = true;

                // Register the task in the root folder
                const string taskName = "TaskCreatedByWallpaperChangerday";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }

        public void createSunsetTask(bool privileges)
        {
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                if (privileges)
                    td.Principal.RunLevel = TaskRunLevel.Highest;

                // Add a daily trigger at a given time
                DailyTrigger dt = (DailyTrigger)td.Triggers.Add(new DailyTrigger());
                dt.StartBoundary = DateTime.Today + TimeSpan.FromHours((double)numericUpDown6.Value) + TimeSpan.FromMinutes((double)numericUpDown5.Value);

                // Add an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(currentLocation + @"\Utilities\sunsetWallpaperChange.vbs", null, null));
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.StartWhenAvailable = true;

                // Register the task in the root folder
                const string taskName = "TaskCreatedByWallpaperChangersunset";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }

        public void createNightTask(bool privileges)
        {
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                if (privileges)
                    td.Principal.RunLevel = TaskRunLevel.Highest;

                // Add a daily trigger at a given time
                DailyTrigger dt = (DailyTrigger)td.Triggers.Add(new DailyTrigger());
                dt.StartBoundary = DateTime.Today + TimeSpan.FromHours((double)numericUpDown8.Value) + TimeSpan.FromMinutes((double)numericUpDown7.Value);

                // Add an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(currentLocation + @"\Utilities\nightWallpaperChange.vbs", null, null));
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.StartWhenAvailable = true;

                // Register the task in the root folder
                const string taskName = "TaskCreatedByWallpaperChangernight";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }

        public void createMorningTask(bool privileges)
        {
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                if (privileges)
                    td.Principal.RunLevel = TaskRunLevel.Highest;

                // Add a daily trigger at a given time
                DailyTrigger dt = (DailyTrigger)td.Triggers.Add(new DailyTrigger());
                dt.StartBoundary = DateTime.Today + TimeSpan.FromHours((double)numericUpDown1.Value) + TimeSpan.FromMinutes((double)numericUpDown2.Value);

                // Add an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(currentLocation + @"\Utilities\morningWallpaperChange.vbs", null, null));
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.StartWhenAvailable = true;

                // Register the task in the root folder
                const string taskName = "TaskCreatedByWallpaperChangermorning";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }

        public void createTask(string type,int hour,int minute,bool privileges)
        {
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                if (privileges)
                    td.Principal.RunLevel = TaskRunLevel.Highest;

                // Add a daily trigger at a given time
                DailyTrigger dt = (DailyTrigger)td.Triggers.Add(new DailyTrigger());
                dt.StartBoundary = DateTime.Today + TimeSpan.FromHours((double)hour) + TimeSpan.FromMinutes((double)minute);

                // Add an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(currentLocation + @"\Utilities\"+type+"WallpaperChange.vbs", null, null));
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.StartWhenAvailable = true;

                // Register the task in the root folder
                string taskName = "TaskCreatedByWallpaperChanger"+type;
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }



        public void createEmptyDayTask()
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;

                td.Actions.Add(new ExecAction("Task has been disabled, it wont run any more", null, null));
                
                const string taskName = "TaskCreatedByWallpaperChangerday";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }

        public void createEmptySunsetTask()
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;

                td.Actions.Add(new ExecAction("Task has been disabled, it wont run any more", null, null));

                const string taskName = "TaskCreatedByWallpaperChangersunset";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }

        }

        public void createEmptyNightTask()
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;

                td.Actions.Add(new ExecAction("Task has been disabled, it wont run any more", null, null));

                const string taskName = "TaskCreatedByWallpaperChangernight";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }

        public void createEmptyMorningTask()
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;

                td.Actions.Add(new ExecAction("Task has been disabled, it wont run any more", null, null));

                const string taskName = "TaskCreatedByWallpaperChangermorning";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }

        

        public string checkValues()
        {
            if(numericUpDown6.Value < 12 && numericUpDown6.Value > 0)
            {
                return numericUpDown6.Value.ToString()+":"+"sunset";
            }
            if (numericUpDown8.Value < 12 && numericUpDown8.Value >0)
            {
                return numericUpDown8.Value.ToString() + ":" + "night";
            }
            if (numericUpDown4.Value < 12 && numericUpDown4.Value>0)
            {
                return numericUpDown4.Value.ToString() + ":" + "midday";
            }
            return "not";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\generalConfig.cfg");

            if(checkOrder()!="ok")
            {
                MessageBox.Show(checkOrder());
            }
            else
            {
                if(checkValues() != "not" && text.Contains("dontShowHoursHelp=1;"))
                {
                        Form3 frm3 = new Form3(checkValues());
                        DialogResult d = frm3.ShowDialog();
                        if (d == DialogResult.OK)
                        {
                            saveChanges2();
                        }
                }
                else
                {
                        saveChanges2();     
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.Checked = false;
            }

            if (checkBox2.Checked == true)
            {
                checkBox2.Checked = false;
            }

            if (checkBox3.Checked == true)
            {
                checkBox3.Checked = false;
            }

            if (checkBox4.Checked == true)
            {
                checkBox4.Checked = false;
            }

            numericUpDown1.Value = 5;
            numericUpDown2.Value = 2;

            numericUpDown4.Value = 12;
            numericUpDown3.Value = 2;


            numericUpDown6.Value = 18;
            numericUpDown5.Value = 2;


            numericUpDown8.Value = 22;
            numericUpDown7.Value = 2;
        }



        public void initializeMorningScript(int hour,int minute,int upper)
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\morningWallpaperChangeScript.ps1");


            string[] timeBlock = text.Split(new string[] { "//helper" }, StringSplitOptions.None);
            string times = timeBlock[1].Split(new string[] { "//helperEnd" }, StringSplitOptions.None)[0];

            string replace = Environment.NewLine + "  	 int hour="+hour.ToString()+";" + Environment.NewLine +
                             "  	 int minute="+minute.ToString()+";" + Environment.NewLine +
                             "  	 int upperHour="+upper.ToString()+";" + Environment.NewLine + "  	 ";

            text = text.Replace(times, replace);

         
            File.WriteAllText(currentLocation + @"\Utilities\morningWallpaperChangeScript.ps1", text);
        }

        public void initializeDayScript(int hour, int minute, int upper)
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\dayWallpaperChangeScript.ps1");


            string[] timeBlock = text.Split(new string[] { "//helper" }, StringSplitOptions.None);
            string times = timeBlock[1].Split(new string[] { "//helperEnd" }, StringSplitOptions.None)[0];

            string replace = Environment.NewLine + "  	 int hour=" + hour.ToString() + ";" + Environment.NewLine +
                             "  	 int minute=" + minute.ToString() + ";" + Environment.NewLine +
                             "  	 int upperHour=" + upper.ToString() + ";" + Environment.NewLine + "  	 ";

            text = text.Replace(times, replace);


            File.WriteAllText(currentLocation + @"\Utilities\dayWallpaperChangeScript.ps1", text);
        }

        public void initializeSunsetScript(int hour, int minute, int upper)
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\sunsetWallpaperChangeScript.ps1");


            string[] timeBlock = text.Split(new string[] { "//helper" }, StringSplitOptions.None);
            string times = timeBlock[1].Split(new string[] { "//helperEnd" }, StringSplitOptions.None)[0];

            string replace = Environment.NewLine + "  	 int hour=" + hour.ToString() + ";" + Environment.NewLine +
                             "  	 int minute=" + minute.ToString() + ";" + Environment.NewLine +
                             "  	 int upperHour=" + upper.ToString() + ";" + Environment.NewLine + "  	 ";

            text = text.Replace(times, replace);


            File.WriteAllText(currentLocation + @"\Utilities\sunsetWallpaperChangeScript.ps1", text);
        }

        public void initializeNightScript(int hour, int minute, int upper)
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\nightWallpaperChangeScript.ps1");


            string[] timeBlock = text.Split(new string[] { "//helper" }, StringSplitOptions.None);
            string times = timeBlock[1].Split(new string[] { "//helperEnd" }, StringSplitOptions.None)[0];

            string replace = Environment.NewLine + "  	 int hour=" + hour.ToString() + ";" + Environment.NewLine +
                             "  	 int minute=" + minute.ToString() + ";" + Environment.NewLine +
                             "  	 int upperHour=" + upper.ToString() + ";" + Environment.NewLine + "  	 ";

            text = text.Replace(times, replace);


            File.WriteAllText(currentLocation + @"\Utilities\nightWallpaperChangeScript.ps1", text);
        }

        public void initializeScript(string type,int hour, int minute, int upper)
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\"+type+"WallpaperChangeScript.ps1");


            string[] timeBlock = text.Split(new string[] { "//helper" }, StringSplitOptions.None);
            string times = timeBlock[1].Split(new string[] { "//helperEnd" }, StringSplitOptions.None)[0];

            string replace = Environment.NewLine + "  	 int hour=" + hour.ToString() + ";" + Environment.NewLine +
                             "  	 int minute=" + minute.ToString() + ";" + Environment.NewLine +
                             "  	 int upperHour=" + upper.ToString() + ";" + Environment.NewLine + "  	 ";

            text = text.Replace(times, replace);


            File.WriteAllText(currentLocation + @"\Utilities\"+type+"WallpaperChangeScript.ps1", text);
        }

        public string checkOrder()
        {
            int mor = (int)numericUpDown1.Value;
            int day = (int)numericUpDown4.Value;
            int sun = (int)numericUpDown6.Value;
            int ni = (int)numericUpDown8.Value;

            if(day==-1)
            {
                day = -2;
            }
            if(sun==-1)
            {
                sun = -3;
            }
            if(ni==-1)
            {
                ni = -4;
            }

            int[] hrs = new int[4] {mor,day,sun,ni};


            for(int i=0;i<4;i++)
            {
                int c = 0;
                for(int j=0;j<4;j++)
                {
                    if(hrs[i]==hrs[j])
                    {
                        c++;
                    }
                }
                if(c!=1)
                {
                    return "You cannot have two scheduled wallpaper changes at the same hour";
                }
            }
             
            

            return "ok";
        }

        public void saveChanges()
        {
            if (checkBox1.Checked == true)
            {
                createEmptyMorningTask();

            }
            else
            {
                try
                {
                    createMorningTask(true);
                }
                catch (UnauthorizedAccessException ex2)
                {
                    createMorningTask(true);
                }
               
                int upper;

                if (checkBox2.Checked == false)
                {
                    upper = (int)numericUpDown4.Value;
                }
                else
                {
                    if (checkBox3.Checked == false)
                    {
                        upper = (int)numericUpDown6.Value;
                    }
                    else
                    {
                        if (checkBox4.Checked == false)
                        {
                            upper = (int)numericUpDown8.Value;
                        }
                        else
                        {
                            upper = 25;
                        }
                    }
                }
                initializeMorningScript((int)numericUpDown1.Value, (int)numericUpDown2.Value, upper);
            }

            if (checkBox2.Checked == true)
            {
                createEmptyDayTask();

            }
            else
            {
                try
                {
                    createDayTask(true);
                }
                catch (UnauthorizedAccessException ex2)
                {
                    createDayTask(false);
                }
               
                int upper;

                if (checkBox3.Checked == false)
                {
                    upper = (int)numericUpDown6.Value;
                }
                else
                {
                    if (checkBox4.Checked == false)
                    {
                        upper = (int)numericUpDown8.Value;
                    }
                    else
                    {
                        if (checkBox1.Checked == false)
                        {
                            upper = (int)numericUpDown1.Value;
                        }
                        else
                        {
                            upper = 25;
                        }
                    }
                }
                initializeDayScript((int)numericUpDown4.Value, (int)numericUpDown3.Value, upper);
            }

            if (checkBox3.Checked == true)
            {
                createEmptySunsetTask();
            }
            else
            {
                try
                {
                    createSunsetTask(true);
                }
                catch (UnauthorizedAccessException ex2)
                {
                    createSunsetTask(false);
                }
                
                int upper;

                if (checkBox4.Checked == false)
                {
                    upper = (int)numericUpDown8.Value;
                }
                else
                {
                    if (checkBox1.Checked == false)
                    {
                        upper = (int)numericUpDown1.Value;
                    }
                    else
                    {
                        if (checkBox2.Checked == false)
                        {
                            upper = (int)numericUpDown4.Value;
                        }
                        else
                        {
                            upper = 25;
                        }
                    }
                }
                initializeSunsetScript((int)numericUpDown6.Value, (int)numericUpDown5.Value, upper);
            }

            if (checkBox4.Checked == true)
            {
                createEmptyNightTask();
            }
            else
            {
                try
                {
                    createNightTask(true);
                }
                catch (UnauthorizedAccessException ex2)
                {
                    createNightTask(false);
                }

                int upper;

                if (checkBox1.Checked == false)
                {
                    upper = (int)numericUpDown1.Value;
                }
                else
                {
                    if (checkBox2.Checked == false)
                    {
                        upper = (int)numericUpDown4.Value;
                    }
                    else
                    {
                        if (checkBox3.Checked == false)
                        {
                            upper = (int)numericUpDown6.Value;
                        }
                        else
                        {
                            upper = 25;
                        }
                    }
                }
                initializeNightScript((int)numericUpDown8.Value, (int)numericUpDown7.Value, upper);
            }
            this.DialogResult = DialogResult.OK;
        }

        public void saveChanges2()
        {
            int count = 0;
            if (checkBox1.Checked == false) count++;
            if (checkBox2.Checked == false) count++;
            if (checkBox3.Checked == false) count++;
            if (checkBox4.Checked == false) count++;

            schTask[] tasks = new schTask[count];
            if (count == 0) {
                createEmptyDayTask();
                createEmptyMorningTask();
                createEmptySunsetTask();
                createEmptyNightTask();
            }
            else {
                if (checkBox1.Checked == false)
                {
                    tasks[order((int)numericUpDown1.Value)] = new schTask("morning", (int)numericUpDown1.Value, (int)numericUpDown2.Value);
                }
                else{
                    createEmptyMorningTask();
                }
                if (checkBox2.Checked == false) {
                    tasks[order((int)numericUpDown4.Value)] = new schTask("day", (int)numericUpDown4.Value, (int)numericUpDown3.Value);
                }
                else{
                    createEmptyDayTask();
                }
                if (checkBox3.Checked == false) {
                    tasks[order((int)numericUpDown6.Value)] = new schTask("sunset", (int)numericUpDown6.Value, (int)numericUpDown5.Value);
                }
                else{
                    createEmptySunsetTask();
                }
                if (checkBox4.Checked == false) {
                    tasks[order((int)numericUpDown8.Value)] = new schTask("night", (int)numericUpDown8.Value, (int)numericUpDown7.Value);
                }
                else{
                    createEmptyNightTask();
                }

                try
                {
                    if (count == 1)
                    {
                        initializeScript(tasks[0].name, tasks[0].hour, tasks[0].minute, 25);
                        createTask(tasks[0].name, tasks[0].hour, tasks[0].minute,true);
                    }
                    else
                    {
                        for (int i = 0; i < count - 1; i++)
                        {
                            initializeScript(tasks[i].name, tasks[i].hour, tasks[i].minute, tasks[i + 1].hour);
                            createTask(tasks[i].name, tasks[i].hour, tasks[i].minute, true);
                        }
                        initializeScript(tasks[count - 1].name, tasks[count - 1].hour, tasks[count - 1].minute, tasks[0].hour + 100);
                        createTask(tasks[count - 1].name, tasks[count - 1].hour, tasks[count - 1].minute, true);
                    }
                }
                catch (UnauthorizedAccessException ex2)
                {
                    if (count == 1)
                    {
                        initializeScript(tasks[0].name, tasks[0].hour, tasks[0].minute, 25);
                        createTask(tasks[0].name, tasks[0].hour, tasks[0].minute,false);
                    }
                    else
                    {
                        for (int i = 0; i < count - 1; i++)
                        {
                            initializeScript(tasks[i].name, tasks[i].hour, tasks[i].minute, tasks[i + 1].hour);
                            createTask(tasks[i].name, tasks[i].hour, tasks[i].minute, false);
                        }
                        initializeScript(tasks[count - 1].name, tasks[count - 1].hour, tasks[count - 1].minute, tasks[0].hour + 100);
                        createTask(tasks[count - 1].name, tasks[count - 1].hour, tasks[count - 1].minute, false);
                    }
                }
                

            }

            this.DialogResult = DialogResult.OK;
        }

        public int order(int value)
        {
            int pos = 0;
            if (numericUpDown1.Value != -1 && numericUpDown1.Value < value) pos++;
            if (numericUpDown4.Value != -1 && numericUpDown4.Value < value) pos++;
            if (numericUpDown6.Value != -1 && numericUpDown6.Value < value) pos++;
            if (numericUpDown8.Value != -1 && numericUpDown8.Value < value) pos++;
            return pos;
        }


     

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    public class schTask
    {
        public string name;
        public int hour;
        public int minute;

        public schTask(string n, int h, int m)
        {
            this.name = n;
            this.hour = h;
            this.minute = m;
        }
    }

}
