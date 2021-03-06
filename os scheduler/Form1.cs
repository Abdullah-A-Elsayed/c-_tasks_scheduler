﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace os_scheduler
{
    public struct alaa_data
    {
        public int alaa_start, alaa_burst, alaa_process_id;
        public alaa_data(int a=0, int b=0 ,int c=0)
        {
            alaa_start = a;
            alaa_burst = b;
            alaa_process_id =c;
        }
    }
    public partial class Form1 : Form
    {
        bool first_input = true;
        
        public static scheduler s = new scheduler();
        public List<TextBox> burst_time = new List<TextBox>();
        public static List<int> burst_int = new List<int>();

        List<TextBox> arrival_time = new List<TextBox>();
        public static List<int> arrival_int = new List<int>();

        List<TextBox> priority = new List<TextBox>();
        public List<int> priority_int = new List<int>();

        int rr_quan_int;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // this.MaximizeBox = false;
           // this.MinimizeBox = false;
            //this.BackColor = Color.White;
            //this.ForeColor = Color.Black;
            //this.Size = new System.Drawing.Size(850, 480);
            this.Text = "Processes Scheduler";
           // this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            quan.Visible = false;
            rr_quan.Visible = false;
        }

        private void count_TextChanged(object sender, EventArgs e)
        {
            Form1.s.nprocess = (count.Text == "")? 0:Int32.Parse(count.Text);

            //MessageBox.Show(c.ToString());
            if (  s.method.Length != 0)
            {
                //store this number in your class
                take_processes_data();
            }
        }

        private void method_SelectedIndexChanged(object sender, EventArgs e)
        {
            //store the method in ur class
            Form1.s.method = method.Text;
            //MessageBox.Show(Form1.s.method);
            if ( s.method.Length != 0)
            {
                if (first_input == true)
                {
                    first_input = false;
                    take_processes_data();
                }
                else deactivate_unNecessary();

            }
        }
        private void take_processes_data()
        {
            //MessageBox.Show("ready to retrieve input");
            //reset
            groupBox1.Controls.Clear();
            groupBox1.Size = new System.Drawing.Size(588, 130);
            this.Size = new System.Drawing.Size(628, 290);
            burst_time.Clear();
            arrival_time.Clear();
            priority.Clear();
            if (s.nprocess == 0) return;

            int x = 15, y = 20;//relative to groupBox

            Label write_arrival = new Label();
            write_arrival.Text = "arrival time";
            write_arrival.Size = new System.Drawing.Size(120, 20);
            write_arrival.Location = new System.Drawing.Point(x+360+20, y);
            this.groupBox1.Controls.Add(write_arrival);

            Label write_bur_time = new Label();
            write_bur_time.Text = "burst time";
            write_bur_time.Size = new System.Drawing.Size(120, 20);
            write_bur_time.Location = new System.Drawing.Point(x + 120, y);
            this.groupBox1.Controls.Add(write_bur_time);

            Label prio = new Label();
            prio.Text = "priority";
            prio.Size = new System.Drawing.Size(120, 20);
            prio.Location = new System.Drawing.Point(x + 240 + 10, y);
            this.groupBox1.Controls.Add(prio);


            for (int i = 0; i < Form1.s.nprocess; ++i)
            {
                // int x = (i!=0) ? burst_time[i - 1].Location.X : 15;
                //  int y = (i != 0) ? burst_time[i - 1].Location.Y+30 : 20;

                Label name = new Label();
                name.Text = "task" + (i+1).ToString();
                name.Size = new System.Drawing.Size(120, 20);
                name.Location = new System.Drawing.Point(x, y + ((i + 1) * 35));
                this.groupBox1.Controls.Add(name);

                TextBox bur = new TextBox();
                burst_time.Add(bur);
                burst_time[i].Location = new System.Drawing.Point(x + 120, y + (i+1) * 35);
                burst_time[i].Size = new System.Drawing.Size(120, 20);
                this.groupBox1.Controls.Add(burst_time[i]);

                TextBox pr = new TextBox();
                priority.Add(pr);
                priority[i].Location = new System.Drawing.Point(x + 240 + 10, y + (i + 1) * 35);
                priority[i].Size = new System.Drawing.Size(120, 20);
                this.groupBox1.Controls.Add(priority[i]);

                TextBox arr = new TextBox();
                arrival_time.Add(arr);
                arrival_time[i].Location = new System.Drawing.Point(x + 360+20, y + (i + 1) * 35);
                arrival_time[i].Size = new System.Drawing.Size(120, 20);
                this.groupBox1.Controls.Add(arrival_time[i]);


                groupBox1.Size = new System.Drawing.Size(groupBox1.Size.Width, groupBox1.Size.Height + 35);
                this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height + 35);
            }
            y += (Form1.s.nprocess+1)*35;
          
           // y += 25;
            // y+=25;
            // rr_quan (input)
            //quan (label)

            //button Run
            Button run = new Button();
            run.Size = new System.Drawing.Size(120, 50);
            run.Location = new System.Drawing.Point(x + 10 + 240, y);
            run.Text = "Run";
            run.Name = "run";
            run.Click += new System.EventHandler(this.run_Click);
            this.groupBox1.Controls.Add(run);
            run.Font = new Font("Lucida Calligraphy", 17, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            run.ForeColor = Color.White;
            run.BackColor = Color.DeepSkyBlue;
            deactivate_unNecessary();
        }

        private void fill_int()
        {
            if (Form1.s.method == "RR")
            {
                if (rr_quan.Text == "")
                {
                    Exception e = new Exception("please fill quantum field");
                    throw (e);
                }
                rr_quan_int = Int32.Parse(rr_quan.Text);
            }
            if (Form1.s.method == "Priority (non pre-emptive)" || Form1.s.method == "Priority (pre-emptive)")
            {
                priority_int.Clear();
                for (int i = 0; i < s.nprocess; ++i)
                {
                    if (priority[i].Text == "")
                    {
                        Exception e = new Exception("please fill all possible fields");
                        throw (e);
                    }
                    priority_int.Add(Int32.Parse(priority[i].Text));
                }
            }

            arrival_int.Clear();
            burst_int.Clear();
            for (int i = 0; i < s.nprocess; ++i)
            {
                if (burst_time[i].Text == "" || arrival_time[i].Text == "")
                {
                    Exception e = new Exception("please fill all possible fields");
                    throw (e);
                }

                arrival_int.Add(Int32.Parse(arrival_time[i].Text));
                burst_int.Add(Int32.Parse(burst_time[i].Text));
            }
        }

        private void deactivate_unNecessary()
        {
            if (s.nprocess == 0) return;
            if (Form1.s.method != "RR")
            {
                this.rr_quan.Visible = false;
                quan.Visible = false;
            }
            else
            {
                this.rr_quan.Visible = true;
                quan.Visible = true;
            }

            if (Form1.s.method != "Priority (non pre-emptive)" && Form1.s.method != "Priority (pre-emptive)")
            {
                for (int i = 0; i < s.nprocess; ++i)
                {
                    priority[i].Enabled = false;
                }
            }
            else if (!priority[0].Enabled)
            {
                for (int i = 0; i < s.nprocess; ++i)
                {
                    priority[i].Enabled = true;
                }
            }
        }

        private void run_Click(object sender, EventArgs e)
        {
            try
            {
                fill_int();
                draw();
                //MessageBox.Show("Gantt chart");
            }
            catch(Exception exc){
                MessageBox.Show(exc.Message);
            }
        }

        private void draw()
        {
            List<alaa_data> drawable_data = get_drawable_data();
            //test
            //string g = "";
            //for (int i = 0; i <= s.nprocess; i++)
            //{
            //    g += drawable_data[i].alaa_start.ToString() + "  " + drawable_data[i].alaa_process_id.ToString() + "\n";
            //}
            //MessageBox.Show(g);
            //draw
            //MessageBox.Show("draw size: "+drawable_data.Count.ToString()
            //    +"\n output size: "+s.output.Count.ToString()
            //    +"\n input size: "+ s.input.Count.ToString()
            //    );
            string s = "";
            for (int i = 0; i < drawable_data.Count; ++i)
            {
                s += "id: " + drawable_data[i].alaa_process_id.ToString();
                s += " - burst: " + drawable_data[i].alaa_burst.ToString();
                s += " - start: " + drawable_data[i].alaa_start.ToString();
                s += "\n";
            }
            //MessageBox.Show(s);
            //if time line is zero don't draw
            if (drawable_data.Last().alaa_start == 0 && drawable_data.Last().alaa_burst == 0)
            {
                MessageBox.Show("average waiting time: 0\nNo processes will be assigned to CPU");
            }
            else{
                Form2 chart = new Form2(drawable_data);
                chart.ShowDialog();
            }
        }

        private List<alaa_data> get_drawable_data()//interface
        {
            s.input = new List<Process>(s.nprocess);
            s.output = new List<Process>(s.nprocess);
            
            //call alaa's functions based on Form1.s.method to fill drawable_data
            //preparing input
            for (int i = 0; i < s.nprocess; ++i)
            {
                s.input.Add(new Process(i+1, arrival_int[i], burst_int[i]));
                if (s.method == "Priority (non pre-emptive)"
                    ||
                    s.method == "Priority (pre-emptive)")
                {
                    s.input[i].priority = priority_int[i];
                }
            }
            //calling functions
            
            if (s.method == "FCFS") s.fcfs();
            else if (s.method == "Priority (non pre-emptive)") s.priority_non_preemptive();
            else if (s.method == "Priority (pre-emptive)") s.priority_preemptive();
            else if (s.method == "SJF (non pre-emptive)") s.sjf_non_preemptive();
            else if (s.method == "SJF (pre-emptive)") s.sjf_preemptive();
            else s.RR(rr_quan_int);

            s.insert_idle();

            List<alaa_data> drawable_data = new List<alaa_data>(s.output_with_idle.Count);
            //testing
                //for (int i = 0; i < s.nprocess; i++)
                //{
                //    drawable_data.Add(new alaa_data(i * 2, 2, i + 1));
                //}
            //end testing

            //real values
                for (int i = 0; i < s.output_with_idle.Count; i++)
                {
                    drawable_data.Add(new alaa_data(s.output_with_idle[i].start,
                        s.output_with_idle[i].end - s.output_with_idle[i].start,
                        s.output_with_idle[i].id
                        ));
                }
            //end real vals
            return drawable_data;
        }

       
    }
}
