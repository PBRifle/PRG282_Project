using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StudentFormsApp
{
    public partial class Form1 : Form
    {
        private string FilePath = @"students.txt";
   
        public Form1()
        {
            InitializeComponent();
            
        }
        
        // Method (Add student) to add a new student to the file
        private void button1_Click(object sender, EventArgs e)
        {
            string studentID = textBox1.Text;
            string name = textBox2.Text;
            string age = textBox3.Text;
            string course = textBox4.Text;
            // Check if the following fields are entered
            if(string.IsNullOrEmpty(studentID) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(age) || string.IsNullOrEmpty(course))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            // Validate that student ID is numeric and exactly 6 characters long
            if(!int.TryParse(studentID,out _)||studentID.Length != 6)
            {
                MessageBox.Show("Student ID must be a numeric value with exactly 6 digits");
                return;
            }
            // Validate that age is numeric
            if (!int.TryParse(age, out _))
            {
                MessageBox.Show("Age must be a numeric value.");
                return;
            }


            string studentRecord = $"{studentID}, {name}, {age}, {course}";

            try
            {
                //Append the student record to students.txt
                File.AppendAllText(FilePath, studentRecord + Environment.NewLine);
                MessageBox.Show("Student record added successfully!");
                //Clear the text fields after adding
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving student record:"+ex.Message);
            }
        }
        // Method (View Student) to load and display all student records
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridViewStudents.Rows.Clear();
            dataGridViewStudents.Columns.Clear();

            dataGridViewStudents.Columns.Add("StudentID","Student ID");
            dataGridViewStudents.Columns.Add("Name", "Name");
            dataGridViewStudents.Columns.Add("Age", "Age");
            dataGridViewStudents.Columns.Add("Course", "Course");

            try
            {
                // Check if the file exists and read all lines
                if(File.Exists(FilePath))
                {
                    var lines = File.ReadLines(FilePath);
                    foreach(var line in lines)
                    {
                        var parts = line.Split(',');

                        if (parts.Length == 4)
                        {
                            dataGridViewStudents.Rows.Add(parts[0], parts[1], parts[2], parts[3]);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("No records found. Please add some students first.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student records:" + ex.Message);
            }

            // Method to update a student's information
            private void buttonUpdate_Click(object sender, EventArgs e)
            {
                if (dataGridViewStudents.SelectedRows.Count > 0)
                {
                    string selectedID = dataGridViewStudents.SelectedRows[0].Cells[0].Value.ToString();

                    List<string> lines = File.ReadAllLines(FilePath).ToList();

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(',');
                        if (parts[0].Trim() == selectedID) // Find the student by ID
                        {
                            // Update the record with new data
                            lines[i] = $"{textBox1.Text},{textBox2.Text},{textBox3.Text},{textBox4.Text}";
                            break;
                        }
                    }

                    File.WriteAllLines(FilePath, lines);

                    MessageBox.Show("Student updated successfully!");
                    LoadStudentData(); // Refresh the DataGridView
                    ClearFields(); // Clears the input fields after update
                }
                else
                {
                    MessageBox.Show("Please select a student to update.");
                }
            }

            // Method to delete a student's information
            private void buttonDelete_Click(object sender, EventArgs e)
            {
                if (dataGridViewStudents.SelectedRows.Count > 0)
                {
                    string selectedID = dataGridViewStudents.SelectedRows[0].Cells[0].Value.ToString();

                    List<string> lines = File.ReadAllLines(FilePath).ToList();

                    lines = lines.Where(line => !line.StartsWith(selectedID + ",")).ToList();

                    File.WriteAllLines(FilePath, lines);

                    MessageBox.Show("Student deleted successfully!");
                    LoadStudentData(); // Refresh the DataGridView
                }
                else
                {
                    MessageBox.Show("Please select a student to delete.");
                }
            }
            // Method to clear input fields
            private void ClearFields()
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
        }




    }

    }
}
