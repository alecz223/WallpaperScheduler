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
    public partial class Form1 : Form
    {
        public String currentLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
     
        public Form1()
        {
            InitializeComponent();
            string text = File.ReadAllText(currentLocation + @"\Utilities\generalConfig.cfg");

            if (text.Contains("startedFirstTime=0;")){
                

                if (checkFiles()==true) MessageBox.Show("It was detected that you changed the folder wallpaper scheduler works from , for everything to function properly you need to set it up again by pressing the \"Start using wallpaper scheduler\" button ");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width,this.Size.Height+80);

        }

        // The INFORMATIONS button
        private void button5_Click(object sender, EventArgs e)
        {
            string text ="             DESCRIPTION\n\n" +
                    "    This program creates windows task scheduler " +
                    "tasks that change your wallpaper at 4 points in " +
                    "the day (morning, midday, sunset, and night) with " +
                    "photos from each respective folder (you can put whatever photos you want in these 4 folders).\n\n " +
                    "             SETTING UP\n\n" +
                    "   Pressing the start using wallpaper scheduler button is enough to set everything up. " +
                    "This will configure the program for the current folder (if you move it to a different " +
                    "folder you will have to press the start button there too) and for the " +
                    "default times (5 am , 12 am , 6 pm and 11 pm).\n" +
                    "   If you want to change these times press the modify times button," +
                    "wich will open the TIME SETTINGS MENU.\n\n" +
                    "             IMPORTANT NOTICE!!\n\n" +
                    "   Wallpaper changes will happen even if your pc is turned off or"+
                    " sleeping at the scheduled time"+
                    " as soon as you turn on your pc (EX: you have changes scheduled at 1 pm from midday and 6 pm from sunset. "+
                    "if you turn your pc on at 4pm the day wallpaper will be set , if you turn your pc on "+
                    "at 7 pm only the sunset wallpaper will be set). Because of this its recomended to set the morning change time to something like 4-5 am.\n\n"+
                    "             PREFERENCES\n\n" +
                    "   If you  want your wallpaper to only change 1 or 2 or 3 times a day " +
                    "you can choose to skip any of the wallpaper changes by ticking the skip " +
                    "wallpaper change box in the TIME SETTINGS MENU.\n\n" +
                    "             USE YOUR OWN PHOTOS\n\n" +
                    "   You can add or remove photos to each of the photo folders ,and they will be used as wallpapers in their respective times of the day\n\n" +
                    "             DISABLING\n\n" +
                    "   If you wish to stop using wallpaper scheduler press the stop using button, " +
                    "wich will deactivate the wallpaper changing tasks. If you want you can delete them" +
                    "in WindowsTask scheduler after";
            
            MessageBox.Show(text);
            
        }

        //The EXIT button
        private void button4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        //The START USING WALLPAPER SCHEDULER button
        private void button1_Click(object sender, EventArgs e)
        {
            
            Form frm = new Form();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Size = new Size(250, 40);
            frm.Text = "Please wait";
            frm.Show();
            try
            {
                initializeDayScript();
                initializeDayVbs();
                initializeNightScript();
                initializeNightVbs();
                initializeMorningScript();
                initializeMorningVbs();
                initializeSunsetScript();
                initializeSunsetVbs();

                try
                {
                    createDayTask(true);
                    createNightTask(true);
                    createMorningTask(true);
                    createSunsetTask(true);
                }
                catch (UnauthorizedAccessException ex2)
                {
                    createDayTask(false);
                    createNightTask(false);
                    createMorningTask(false);
                    createSunsetTask(false);
                }
            

                frm.Hide();
                frm.Enabled = false;
                frm = null;

                string text = File.ReadAllText(currentLocation + @"\Utilities\generalConfig.cfg");

                text = text.Replace("startedFirstTime=1;", "startedFirstTime=0;");
                File.WriteAllText(currentLocation + @"\Utilities\\generalConfig.cfg", text);

                MessageBox.Show("Wallaper changer succesfully configured.\n\n                  You are ready to go");
            }
            catch (Exception ex)
            {
                frm.Hide();
                frm.Enabled = false;
                frm = null;
                MessageBox.Show("Something went wrong:( , error message:\n\n\n"+ex.Message.ToString());
            }
            
            
        }

        //The STOP USING WALLPAPER SCHEDULER button
        private void button3_Click(object sender, EventArgs e)
        {

            Form frm = new Form();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Size = new Size(250, 40);
            frm.Text = "Please wait";
            frm.Show();
            try
            {
                createEmptyTasks();
                using (TaskService ts = new TaskService())
                {
                    
                    try
                    {
                      //  ts.RootFolder.DeleteTask("TaskCreatedByWallpaperChangermorning");
                       // ts.RootFolder.DeleteTask("TaskCreatedByWallpaperChangerday");
                       // ts.RootFolder.DeleteTask("TaskCreatedByWallpaperChangernight");
                      //  ts.RootFolder.DeleteTask("TaskCreatedByWallpaperChangersunset");
                    }
                    catch (Exception) { }
                }


                frm.Hide();
                frm.Enabled = false;
                frm = null;

                MessageBox.Show("Removal succesfull.\n\n                  You are no longer using wallpaper scheduler");
            }
            catch (Exception ex)
            {
                frm.Hide();
                frm.Enabled = false;
                frm = null;
                MessageBox.Show("Something went wrong:( , error message:\n\n\n" + ex.Message);
            }
            
        }

        //The MODIFY button
        private void button2_Click(object sender, EventArgs e)
        {
            
            try
            {
                Form frm = new Form2();
                frm.Text = "Time settings menu";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:( , error message:\n\n\n" + ex.Message);
            }

        }

        public void initializeDayScript()
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\dayWallpaperChangeScript.ps1");
            string[] words = text.Split(new string[] { "currentDir=" }, StringSplitOptions.None);
            string value = words[1].Split(';')[0];

            string[] timeBlock = text.Split(new string[] { "//helper" }, StringSplitOptions.None);
            string times = timeBlock[1].Split(new string[] { "//helperEnd" }, StringSplitOptions.None)[0];

            string replace = Environment.NewLine+ "  	 int hour=12;" + Environment.NewLine +
                             "  	 int minute=0;" + Environment.NewLine +
                             "  	 int upperHour=18;" + Environment.NewLine+ "  	 ";

            text = text.Replace(times, replace);


            text = text.Replace(value, "@\"" + currentLocation + "\"");
            File.WriteAllText(currentLocation + @"\Utilities\dayWallpaperChangeScript.ps1", text);
        }

        public void initializeDayVbs()
        {
            string text2 = File.ReadAllText(currentLocation + @"\Utilities\dayWallpaperChange.vbs");
            string[] words2 = text2.Split(new string[] { "strPath=" }, StringSplitOptions.None);
            string value2 = words2[1].Split(new string[] { "\';" }, StringSplitOptions.None)[0];


            text2 = text2.Replace(value2, "\"" + currentLocation + @"\Utilities\dayWallpaperChangeScript.ps1" + "\"");
            File.WriteAllText(currentLocation + @"\Utilities\dayWallpaperChange.vbs", text2);
        }

        public void initializeNightScript()
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\nightWallpaperChangeScript.ps1");
            string[] words = text.Split(new string[] { "currentDir=" }, StringSplitOptions.None);
            string value = words[1].Split(';')[0];

            string[] timeBlock = text.Split(new string[] { "//helper" }, StringSplitOptions.None);
            string times = timeBlock[1].Split(new string[] { "//helperEnd" }, StringSplitOptions.None)[0];

            string replace = Environment.NewLine + "  	 int hour=22;" + Environment.NewLine +
                             "  	 int minute=0;" + Environment.NewLine +
                             "  	 int upperHour=5;" + Environment.NewLine + "  	 ";

            text = text.Replace(times, replace);

            text = text.Replace(value, "@\"" + currentLocation + "\"");
            File.WriteAllText(currentLocation + @"\Utilities\nightWallpaperChangeScript.ps1", text);
        }

        public void initializeNightVbs()
        {
            string text2 = File.ReadAllText(currentLocation + @"\Utilities\nightWallpaperChange.vbs");
            string[] words2 = text2.Split(new string[] { "strPath=" }, StringSplitOptions.None);
            string value2 = words2[1].Split(new string[] { "\';" }, StringSplitOptions.None)[0];


            text2 = text2.Replace(value2, "\"" + currentLocation + @"\Utilities\nightWallpaperChangeScript.ps1" + "\"");
            File.WriteAllText(currentLocation + @"\Utilities\nightWallpaperChange.vbs", text2);
        }

        public void initializeSunsetScript()
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\sunsetWallpaperChangeScript.ps1");
            string[] words = text.Split(new string[] { "currentDir=" }, StringSplitOptions.None);
            string value = words[1].Split(';')[0];

            string[] timeBlock = text.Split(new string[] { "//helper" }, StringSplitOptions.None);
            string times = timeBlock[1].Split(new string[] { "//helperEnd" }, StringSplitOptions.None)[0];

            string replace = Environment.NewLine + "  	 int hour=18;" + Environment.NewLine +
                             "  	 int minute=0;" + Environment.NewLine +
                             "  	 int upperHour=22;" + Environment.NewLine + "  	 ";

            text = text.Replace(times, replace);

            text = text.Replace(value, "@\"" + currentLocation + "\"");
            File.WriteAllText(currentLocation + @"\Utilities\sunsetWallpaperChangeScript.ps1", text);
        }

        public void initializeSunsetVbs()
        {
            string text2 = File.ReadAllText(currentLocation + @"\Utilities\sunsetWallpaperChange.vbs");
            string[] words2 = text2.Split(new string[] { "strPath=" }, StringSplitOptions.None);
            string value2 = words2[1].Split(new string[] { "\';" }, StringSplitOptions.None)[0];


            text2 = text2.Replace(value2, "\"" + currentLocation + @"\Utilities\sunsetWallpaperChangeScript.ps1" + "\"");
            File.WriteAllText(currentLocation + @"\Utilities\sunsetWallpaperChange.vbs", text2);
        }

        public void initializeMorningScript()
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\morningWallpaperChangeScript.ps1");
            string[] words = text.Split(new string[] { "currentDir=" }, StringSplitOptions.None);
            string value = words[1].Split(';')[0];

            string[] timeBlock = text.Split(new string[] { "//helper" }, StringSplitOptions.None);
            string times = timeBlock[1].Split(new string[] { "//helperEnd" }, StringSplitOptions.None)[0];

            string replace = Environment.NewLine + "  	 int hour=5;" + Environment.NewLine +
                             "  	 int minute=0;" + Environment.NewLine +
                             "  	 int upperHour=12;" + Environment.NewLine + "  	 ";

            text = text.Replace(times, replace);

            text = text.Replace(value, "@\"" + currentLocation + "\"");
            File.WriteAllText(currentLocation + @"\Utilities\morningWallpaperChangeScript.ps1", text);
        }

        public void initializeMorningVbs()
        {
            string text2 = File.ReadAllText(currentLocation + @"\Utilities\morningWallpaperChange.vbs");
            string[] words2 = text2.Split(new string[] { "strPath=" }, StringSplitOptions.None);
            string value2 = words2[1].Split(new string[] { "\';" }, StringSplitOptions.None)[0];


            text2 = text2.Replace(value2, "\"" + currentLocation + @"\Utilities\morningWallpaperChangeScript.ps1" + "\"");
            File.WriteAllText(currentLocation + @"\Utilities\morningWallpaperChange.vbs", text2);
        }

        public void createDayTask(bool privileges)
        {
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                if(privileges)
                td.Principal.RunLevel = TaskRunLevel.Highest;
                td.Principal.LogonType = TaskLogonType.InteractiveToken;

                // Add a daily trigger at a given time
                DailyTrigger dt = (DailyTrigger)td.Triggers.Add(new DailyTrigger());
                dt.StartBoundary = DateTime.Today + TimeSpan.FromHours(12) + TimeSpan.FromMinutes(2);

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
                dt.StartBoundary = DateTime.Today + TimeSpan.FromHours(18) + TimeSpan.FromMinutes(2);

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
                dt.StartBoundary = DateTime.Today + TimeSpan.FromHours(22) + TimeSpan.FromMinutes(2);

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
                dt.StartBoundary = DateTime.Today + TimeSpan.FromHours(5) + TimeSpan.FromMinutes(2);

                // Add an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(currentLocation + @"\Utilities\morningWallpaperChange.vbs", null, null));
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.StartWhenAvailable = true;

                // Register the task in the root folder
                const string taskName = "TaskCreatedByWallpaperChangermorning";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }

        public void createEmptyTasks()
        {
            using (TaskService ts = new TaskService())
            {
                
                TaskDefinition td = ts.NewTask();
                td.Principal.LogonType = TaskLogonType.InteractiveToken;

                TaskDefinition td2 = ts.NewTask();
                td2.Principal.LogonType = TaskLogonType.InteractiveToken;

                TaskDefinition td3 = ts.NewTask();
                td3.Principal.LogonType = TaskLogonType.InteractiveToken;

                TaskDefinition td4 = ts.NewTask();
                td4.Principal.LogonType = TaskLogonType.InteractiveToken;

                td.Actions.Add(new ExecAction("Task has been disabled, it wont run any more", null, null));
                td2.Actions.Add(new ExecAction("Task has been disabled, it wont run any more", null, null));
                td3.Actions.Add(new ExecAction("Task has been disabled, it wont run any more", null, null));
                td4.Actions.Add(new ExecAction("Task has been disabled, it wont run any more", null, null));


                // Register the task in the root folder
                const string taskName = "TaskCreatedByWallpaperChangerday";
                const string taskName2 = "TaskCreatedByWallpaperChangernight";
                const string taskName3 = "TaskCreatedByWallpaperChangersunset";
                const string taskName4 = "TaskCreatedByWallpaperChangermorning";
                ts.RootFolder.RegisterTaskDefinition(taskName, td);
                ts.RootFolder.RegisterTaskDefinition(taskName2, td2);
                ts.RootFolder.RegisterTaskDefinition(taskName3, td3);
                ts.RootFolder.RegisterTaskDefinition(taskName4, td4);
            }
        }

        public bool checkFiles()
        {
            string text = File.ReadAllText(currentLocation + @"\Utilities\dayWallpaperChangeScript.ps1");
            bool ok = false;
            if (!text.Contains(currentLocation))
            {
                ok = true;
            }
            text = File.ReadAllText(currentLocation + @"\Utilities\morningWallpaperChangeScript.ps1");

            if (!text.Contains(currentLocation))
            {
                ok = true;
            }
            text = File.ReadAllText(currentLocation + @"\Utilities\sunsetWallpaperChangeScript.ps1");

            if (!text.Contains(currentLocation))
            {
                ok = true;
            }
            text = File.ReadAllText(currentLocation + @"\Utilities\nightWallpaperChangeScript.ps1");

            if (!text.Contains(currentLocation))
            {
                ok = true;
            }

            text = File.ReadAllText(currentLocation + @"\Utilities\dayWallpaperChange.vbs");
            if (!text.Contains(currentLocation))
            {
                ok = true;
            }
            text = File.ReadAllText(currentLocation + @"\Utilities\sunsetWallpaperChange.vbs");
            if (!text.Contains(currentLocation))
            {
                ok = true;
            }
            text = File.ReadAllText(currentLocation + @"\Utilities\morningWallpaperChange.vbs");
            if (!text.Contains(currentLocation))
            {
                ok = true;
            }
            text = File.ReadAllText(currentLocation + @"\Utilities\nightWallpaperChange.vbs");
            if (!text.Contains(currentLocation))
            {
                ok = true;
            }
            return ok;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool ok = false;
            try
            {
              
                using (TaskService ts = new TaskService())
                {
                    List<String> strings = new List<string>();
                    strings.Add("TaskCreatedByWallpaperChangerday");
                    strings.Add("TaskCreatedByWallpaperChangermorning");
                    strings.Add("TaskCreatedByWallpaperChangernight");
                    strings.Add("TaskCreatedByWallpaperChangersunset");
                    foreach (Task task in ts.RootFolder.Tasks)
                    {
                        if (strings.Contains(task.ToString()))
                        {
                            task.Run();
                            if(task.IsActive)
                            {
                                ok = true;
                            }
                           
                        }

                    }
                    if(ok)
                    {
                        MessageBox.Show("Task succesfully ran, unless something went wrong your wallpaper should change.\nThis could however take up to 1-2 minutes the first time\n\nIf the wallpaper dosen't change at all try to run the program as administrator and press the start button again");
                    }
                    else
                    {
                        MessageBox.Show("Could not find any task to run");
                    }
                  

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        }
    }
}
